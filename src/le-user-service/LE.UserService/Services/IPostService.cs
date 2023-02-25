using LE.UserService.Dtos;
using LE.UserService.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IPostService
    {
        Task<PostDto> GetPost(Guid postId, CancellationToken cancellationToken = default);
        Task<List<PostDto>> GetPostsOfUser(Guid userId, CancellationToken cancellationToken = default);
        Task CreatePost(PostDto postDto, CancellationToken cancellationToken = default);
        Task UpdatePost(Guid postId, PostDto postDto, CancellationToken cancellationToken = default);
        Task SetPostState(Guid postId, PostState state, CancellationToken cancellationToken = default);
    }
}
