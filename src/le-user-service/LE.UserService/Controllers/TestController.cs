using LE.UserService.Neo4jData.DALs;
using LE.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IPostDAL _postDAL;
        private readonly IUserService _userService;
        public TestController(IPostDAL postDAL, IUserService userService)
        {
            _postDAL = postDAL;
            _userService = userService;
        }

        [HttpGet("getlistpost")]
        public async Task<IActionResult> Test(Guid userId)
        {
            var langs = await _userService.GetUserLanguages(userId);
            var langIds = langs.Select(x => x.Id).ToList();
            var ids = await _postDAL.FilterPostByLanguages(langIds);
            return Ok(ids);
        }
    }
}
