using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using System;
using System.Collections.Generic;

namespace LE.UserService.Services
{
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest model);
        //IEnumerable<User> GetAll();
        User GetById(Guid id);
        void Register(RegisterRequest model);
        void UpdatePassword(Guid id, string password);
        //void Update(int id, UpdateRequest model);
        void Delete(Guid id);
    }
}
