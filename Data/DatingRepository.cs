using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularApp.API.Data;
using AngularApp.Data;
using AngularApp.Helpers;
using AngularApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;
        public DatingRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<List<User>> GetUsers(UserParams  userParams)
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();

            //return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
            return users;
        }

        public async Task<Photo> GetMainPhotoForUser(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

   
        public async Task<bool> SaveAll()
        {   

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<User> GetUser(int id, bool isCurrentUser)
        {
            var query = _context.Users.AsQueryable();
            
            if (isCurrentUser)
                query = query.IgnoreQueryFilters();
              
            var user = await query.FirstOrDefaultAsync(u => u.Id == id);

            return user ;
        }


    }
}