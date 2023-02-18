using LE.UserService.Helpers;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [ApiController]
    [Route("/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private IAuthService _authService;
        private readonly IMailService _mailService;

        public AuthController(ILogger<AuthController> logger, IAuthService authService, IMailService mailService)
        {
            _logger = logger;
            _authService = authService;
            _mailService = mailService;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterRequest model)
        {
            _authService.Register(model);
            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthRequest model)
        {
            var response = _authService.Authenticate(model);
            return Ok(response);
        }
        
        [HttpPost("send-mail/{id}")]
        public async Task<IActionResult> SendMail(Guid id)
        {
            var user = _authService.GetById(id);
            if (user == null)
                return NotFound($"No exist user has id: {id}");

            var newPass = GenPasswordHelper.GenerateRandomPassword();
            var request = new MailRequest
            {
                ToEmail = user.Email,
                Subject = "LangEnchange Reset Password",
                Body = $@"
                HỆ THỐNG QUẢN LÝ LangExchange <br /> <br />

                Chúng tôi đã hỗ trỡ bạn reset mật khẩu. Đây là mật khẩu mới của bạn: {newPass}
                
                <br /><br />
                Trân trọng!
                Cảm ơn
                    "
            };
            try
            {
                await _mailService.SendEmailAsync(request);
                _authService.UpdatePassword(id, newPass);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
