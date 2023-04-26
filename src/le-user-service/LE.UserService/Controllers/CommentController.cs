using AutoMapper;
using LE.Library.Kernel;
using LE.UserService.Dtos;
using LE.UserService.Helpers;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/users/{id}/posts")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        private readonly IRequestHeader _requestHeader;

        public CommentController(ICommentService commentService, IMapper mapper, IRequestHeader requestHeader)
        {
            _commentService = commentService;
            _mapper = mapper;
            _requestHeader = requestHeader;
        }

        [HttpPost("{postId}/comment/create")]
        public async Task<IActionResult> CreateComment(Guid id, Guid postId, CommentRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest("Empty request");

            var dto = _mapper.Map<CommentDto>(request);
            dto.UserId = id;
            dto.PostId = postId;
            var commentId = await _commentService.CreateComment(postId, dto, cancellationToken);
            return Ok(commentId);
        }

        [HttpPut("{postId}/comments/{commentId}/update")]
        public async Task<IActionResult> UpdateComment(Guid id, Guid postId, Guid commentId, CommentRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest("Empty request");
            if (!await _commentService.IsBelongToPost(postId, commentId, cancellationToken))
                return BadRequest("Comment is not belong to post");
            if (!await _commentService.IsBelongToUser(id, commentId, cancellationToken))
                return BadRequest("Comment is not belong to user");

            var dto = _mapper.Map<CommentDto>(request);
            await _commentService.UpdateComment(commentId, dto, cancellationToken);
            return Ok();
        }

        [HttpGet("/api/posts/{postId}/comments")]
        public async Task<IActionResult> GetComments(Guid postId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            var dtos = await _commentService.GetComments(uuid, postId, cancellationToken);
            return Ok(dtos);
        }

        [HttpDelete("{postId}/comments/{commentId}")]
        public async Task<IActionResult> DeleteComments(Guid postId, Guid commentId, CancellationToken cancellationToken = default)
        {
            if (!await _commentService.IsBelongToPost(postId, commentId, cancellationToken))
                return BadRequest("Comment is not belong to post");

            await _commentService.DeleteComment(commentId, cancellationToken);
            return Ok();
        }

        [HttpPost("/api/users/{id}/interact/{mode}/comments/{commentId}")]
        public async Task<IActionResult> InteractComment(Guid id, Guid commentId, int mode, CancellationToken cancellationToken = default)
        {
            var state = PostHelper.GetInteractMode(mode);
            await _commentService.InteractComment(commentId, id, state, cancellationToken);
            return Ok();
        }

        [HttpGet("/api/comments/{commentId}/interacts")]
        public async Task<IActionResult> GetInteractComment(Guid commentId, CancellationToken cancellationToken = default)
        {
            var numOfInteract = await _commentService.GetCmtInteract(commentId, cancellationToken);
            return Ok(numOfInteract);
        }
    }
}
