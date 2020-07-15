using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AngularApp.Data;
using AngularApp.Dtos;
using AngularApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;

namespace AngularApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly  IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegistrationDto)
        {
            if (await _repo.UserExists(userForRegistrationDto.Username))
                return BadRequest("Username already exists");

            User userToCreate = new User();
            userToCreate.City = userForRegistrationDto.City;
            userToCreate.Country = userForRegistrationDto.Country;
            userToCreate.Gender = userForRegistrationDto.Gender;
            userToCreate.DateOfBirth = userForRegistrationDto.DateOfBirth;
            userToCreate.UserName = userForRegistrationDto.Username;
            
            //var userToCreate = _mapper.Map<User>(userForRegistrationDto); 

            var createdUser = await _repo.Register(userToCreate, userForRegistrationDto.Password);

            //var userToReturn = _mapper.Map<UserForDetailedDto>(createdUser); 

            return CreatedAtRoute("GetUser", new {controller = "Users",id = createdUser.Id},userToCreate);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var userFromRepo = await _repo.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);
                if (userFromRepo == null)
                    return Unauthorized();

                var claims = new[]
                {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.UserName)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.
                            GetBytes(_config.GetSection("AppSettings:Token").Value));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.Now.AddDays(1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}