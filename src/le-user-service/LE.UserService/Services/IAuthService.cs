using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using System;

namespace LE.UserService.Services
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest model);
        //IEnumerable<User> GetAll();
        User GetById(Guid id);
        User GetByEmail(string email);
        void Register(RegisterRequest model);
        void UpdatePassword(Guid id, string password);
        //void Update(int id, UpdateRequest model);
        void Delete(Guid id);
    }
}
