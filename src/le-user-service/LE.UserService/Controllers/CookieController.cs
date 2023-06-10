using LE.UserService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LE.UserService.Controllers
{
    [Route("api/cookies")]
    [ApiController]
    public class CookieController : ControllerBase
    {
        private IJwtUtils _jwtUtils;
        public CookieController(IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }

        [HttpGet("credentials")]
        public IActionResult GetCredentialCookie()
        {
            // Get the cookie value
            string id = Request.Cookies["id"];
            string accessToken = Request.Cookies["accessToken"];

            Guid uuid;
            if (!Guid.TryParse(id, out uuid) || _jwtUtils.ValidateToken(accessToken) == null)
            {
                return NoContent();
            }
            // Use the cookie value as needed
            return Ok(new {jid = $"{uuid}@{Env.CHAT_DOMAIN}", accessToken = accessToken});
        }

        [HttpDelete]
        public IActionResult LogOut()
        {
            // Remove the cookie by setting its expiration date to a past value
            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("id");

            return Ok();
        }
    }
}
