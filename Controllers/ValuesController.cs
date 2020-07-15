using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularApp.Data;
using AngularApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularApp.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext _context;
        public ValuesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Values
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Value>>> GetBanks()
        {
            return await _context.Values.ToListAsync();
        }


        public ActionResult GetUser()
        {
            return Ok();
        }
        // GET: api/Values/5
    
        [HttpGet("{id}")]
        public async Task<ActionResult<Value>> GetBank(int id)
        {
            var bank = await _context.Values.FindAsync(id);

            if (bank == null)
            {
                return NotFound();
            }

            return bank;
        }
    

        // POST: api/Values
       

        // PUT: api/Values/5
        

        // DELETE: api/ApiWithActions/5
        
    }
}
