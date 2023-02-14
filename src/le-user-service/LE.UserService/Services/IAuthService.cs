using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using System.Collections.Generic;

namespace LE.UserService.Services
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest model);
        //IEnumerable<User> GetAll();
        //User GetById(int id);
        //void Register(RegisterRequest model);
        //void Update(int id, UpdateRequest model);
        void Delete(int id);
    }
}
