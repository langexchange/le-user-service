using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/friends")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        public FriendController()
        {

        }

        [HttpGet("")]
        public async Task<IActionResult> GetFriendsAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetFriendsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequestsAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> SuggestFriendsAsync(CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost("/api/makefriend/users/{id}")]
        public async Task<IActionResult> MakeFriendAsync(Guid id, CancellationToken cancellationToken)
        {
            //check userId && id
            return Ok();
        }

        [HttpPost("/api/follow/users/{id}")]
        public async Task<IActionResult> FollowUserAsync(Guid id, CancellationToken cancellationToken)
        {
            //check userId && id
            return Ok();
        }
    }
}
