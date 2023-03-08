using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Helpers;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;   
        }

        [HttpPost("{id}/post/create")]
        public async Task<IActionResult> CreatePost(Guid id, PostRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest("Empty request");
            
            var dto = _mapper.Map<PostDto>(request);
            dto.UserId = id;
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
            var dto = await _postService.GetPost(postId, cancellationToken);
            return Ok(dto);
        }

        [HttpGet("{id}/posts")]
        public async Task<IActionResult> GetPosts(Guid id, CancellationToken cancellationToken = default)
        {
            var dtos = await _postService.GetPosts(id, Mode.Get, cancellationToken);
            return Ok(dtos);
        }

        [HttpGet("{id}/posts/suggest")]
        public async Task<IActionResult> GetPostsRecommend(Guid id, CancellationToken cancellationToken = default)
        {
            var dtos = await _postService.GetPosts(id, Mode.Recommend, cancellationToken);
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
