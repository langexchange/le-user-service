using System.ComponentModel.DataAnnotations;

namespace LE.UserService.Models.Requests
{
    public class AuthRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
