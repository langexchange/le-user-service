using AutoMapper;
using LE.Library.Kernel;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Helpers;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;
        private readonly IRequestHeader _requestHeader;
        public PostController(IPostService postService, IMapper mapper, IRequestHeader requestHeader)
        {
            _postService = postService;
            _mapper = mapper;   
            _requestHeader = requestHeader;
        }

        [HttpPost("{id}/post/create")]
        public async Task<IActionResult> CreatePost(Guid id, PostRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest("Empty request");

            var dto = _mapper.Map<PostDto>(request);
            dto.UserId = id;
            var labels = string.Empty;
            request.Labels.ToList().ForEach(x => { labels += $"{Env.SplitChar}{x}"; });
            dto.Label = labels;
            dto.CreatedAt = DateTime.UtcNow;
            var postId = await _postService.CreatePost(dto, cancellationToken);
            return Ok(postId);
        }

        [HttpPut("{id}/posts/{postId}/update")]
        public async Task<IActionResult> CreatePost(Guid id, Guid postId, PostRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest("Empty request");
            if (!await _postService.IsBelongToUser(postId, id, cancellationToken))
                return BadRequest("Post is not belong to user");

            var dto = _mapper.Map<PostDto>(request);
            dto.UserId = id;
            dto.UpdatedAt = DateTime.UtcNow;
            var labels = string.Empty;
            request.Labels.ToList().ForEach(x => { labels += $"{Env.SplitChar}{x}"; });
            dto.Label = labels;

            await _postService.UpdatePost(postId, dto, cancellationToken);
            return Ok();
        }

        [HttpPut("{id}/posts/{postId}/configure/{mode}")]
        public async Task<IActionResult> ConfigurePost(Guid id, Guid postId, int mode, CancellationToken cancellationToken = default)
        {
            if (!await _postService.IsBelongToUser(postId, id, cancellationToken))
                return BadRequest("Post is not belong to user");

            PostState state = PostHelper.GetState(mode);
            await _postService.SetPostState(postId, state, cancellationToken);
            return Ok();
        }

        [HttpGet("/api/posts/{postId}")]
        public async Task<IActionResult> GetPost(Guid postId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            var dto = await _postService.GetPost(uuid, postId, cancellationToken);
            return Ok(dto);
        }

        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetPosts(Guid id, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            var dtos = await _postService.GetPosts(uuid, id, Mode.Get, cancellationToken);
            return Ok(dtos);
        }

        [HttpGet("/api/posts/suggest")]
        public async Task<IActionResult> GetPostsRecommend([FromQuery] string[] filterLangs, [FromQuery] bool isNewest, [FromQuery] bool isOnlyFriend = false, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            var dtos = await _postService.SuggestPostsAsync(uuid, filterLangs, isOnlyFriend, isNewest, cancellationToken);

            if(isNewest)
            {
                var sortDtos = dtos.OrderByDescending(x => x.CreatedAt);
                return Ok(sortDtos);
            }
            return Ok(dtos);
        }

        [HttpPost("{id}/interact/{mode}/posts/{postId}")]
        public async Task<IActionResult> InteractPost(Guid id, Guid postId, int mode, CancellationToken cancellationToken = default)
        {
            var state = PostHelper.GetInteractMode(mode);
            await _postService.InteractPost(postId, id, state, cancellationToken);
            return Ok();
        }

        [HttpGet("/api/posts/{postId}/interacts")]
        public async Task<IActionResult> GetInteractPost(Guid postId, CancellationToken cancellationToken = default)
        {
            var numOfInteract = await _postService.GetPostInteract(postId, cancellationToken);
            return Ok(numOfInteract);
        }
    }
}
