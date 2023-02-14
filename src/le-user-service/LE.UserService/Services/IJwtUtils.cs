using LE.UserService.Infrastructure.Infrastructure.Entities;

namespace LE.UserService.Services
{
    public interface IJwtUtils
    {
        public string GenerateToken(User user);
        public int? ValidateToken(string token);
    }
}
