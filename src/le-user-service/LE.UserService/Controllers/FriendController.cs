﻿using LE.Library.Kernel;
using LE.UserService.Dtos;
using LE.UserService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/friends")]
    [Authorize]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        private readonly IRequestHeader _requestHeader;
        public FriendController(IRequestHeader requestHeader, IFriendService friendService)
        {
            _requestHeader = requestHeader;
            _friendService = friendService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetFriendsAsync(CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            var response = await _friendService.GetFriendsAsync(uuid, cancellationToken);
            return Ok(response);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetFriendsByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var response = await _friendService.GetFriendsAsync(id, cancellationToken);
            return Ok(response);
        }

        [HttpGet("requests")]
        public async Task<IActionResult> GetFriendRequestsAsync(CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            var response = await _friendService.GetFriendRequestsAsync(uuid, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("/api/friends/{id}/request")]
        public async Task<IActionResult> DeleteFriendRequestsAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            await _friendService.DeleteFriendRequest(id, uuid, cancellationToken);
            return Ok();
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> SuggestFriendsAsync([FromQuery] string[] nativeLangs, [FromQuery] string[] targetLangs, [FromQuery] string[] countryCodes, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            var response = await _friendService.SuggestFriendsAsync(uuid, nativeLangs, targetLangs, countryCodes, cancellationToken);
            return Ok(response);
        }

        [HttpPost("/api/makefriend/users/{id}")]
        public async Task<IActionResult> MakeFriendAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == id)
                return BadRequest($"Can't request to user id {id}");

            await _friendService.MakeFriendAsync(uuid, id, cancellationToken);
            return Ok();
        }

        [HttpPost("/api/unfriend/users/{id}")]
        public async Task<IActionResult> UnFriendAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == id)
                return BadRequest($"Can't request to user id {id}");

            await _friendService.UnFriendAsync(uuid, id, cancellationToken);
            return Ok();
        }

        [HttpPost("/api/accept/users/{id}")]
        public async Task<IActionResult> AcceptFriendAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == id)
                return BadRequest($"Can't request to user id {id}");

            await _friendService.AcceptFriendRequestAsync(uuid, id, cancellationToken);
            return Ok();
        }

        [HttpPost("/api/follow/users/{id}")]
        public async Task<IActionResult> FollowUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == id)
                return BadRequest($"Can't request to user id {id}");

            await _friendService.FollowFriendAsync(uuid, id, cancellationToken);
            return Ok();
        }

        [HttpPost("/api/unfollow/users/{id}")]
        public async Task<IActionResult> UnFollowUserAsync(Guid id, CancellationToken cancellationToken)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == id)
                return BadRequest($"Can't request to user id {id}");

            await _friendService.UnFollowAsync(uuid, id, cancellationToken);
            return Ok();
        }
    }
}
