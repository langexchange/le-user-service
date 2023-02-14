using System.ComponentModel.DataAnnotations;

namespace LE.UserService.Models.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
