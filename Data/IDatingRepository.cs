using System.Collections.Generic;
using System.Threading.Tasks;
using AngularApp.Helpers;
using AngularApp.Models;

namespace AngularApp.API.Data
{
    public interface IDatingRepository
    {
         void Add<T>(T entity) where T: class;
         void Delete<T>(T entity) where T: class;
         Task<bool> SaveAll();
         Task<List<User>> GetUsers(UserParams userParams);
         Task<User> GetUser(int id, bool isCurrentUser);
         Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(int userId);

    }
}