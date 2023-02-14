using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Exceptions;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;
using System.Linq;

namespace LE.UserService.Services.Implements
{
    public class AuthService : IAuthService
    {
        private langgeneralContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public AuthService(langgeneralContext context, IJwtUtils jwtUtils, IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthResponse Authenticate(AuthRequest model)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public UserDto GetById(int id)
        {
            throw new System.NotImplementedException();
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
    }
}
