using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Exceptions;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using System;
using System.Linq;

namespace LE.UserService.Services.Implements
{
    public class AuthService : IAuthService
    {
        private LanggeneralDbContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public AuthService(LanggeneralDbContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthResponse Authenticate(AuthRequest model)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password));

            // validate
            if (user == null)
            {
                throw new AppException("Username or password is incorrect");
            }

            // authentication successful
            var response = _mapper.Map<AuthResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public void Delete(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(x => x.Userid == id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.Email.Equals(model.Email)))
                throw new AppException("Email '" + model.Email + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<User>(model);
            // hash password
            //user.PasswordHash = BCryptNet.HashPassword(model.Password);

            // save user
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdatePassword(Guid id, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Userid == id);
            if (user == null)
                throw new AppException($"Not exist user has id: {id}");

            user.Password = password;
            _context.Update(user);
            _context.SaveChanges();
        }

    }
}
