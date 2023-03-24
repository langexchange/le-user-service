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
        Task<PostDto> GetPost(Guid urequestId, Guid postId, CancellationToken cancellationToken = default);
        Task<List<PostDto>> GetPosts(Guid uresquestId, Guid userId, Mode mode, CancellationToken cancellationToken = default);
        Task<Guid> CreatePost(PostDto postDto, CancellationToken cancellationToken = default);
        Task UpdatePost(Guid postId, PostDto postDto, CancellationToken cancellationToken = default);
        Task SetPostState(Guid postId, PostState state, CancellationToken cancellationToken = default);
        Task<bool> IsBelongToUser(Guid postId, Guid userId, CancellationToken cancellationToken = default);
        Task InteractPost(Guid postId, Guid userId, string mode, CancellationToken cancellationToken = default);
        Task<int> GetPostInteract(Guid postId, CancellationToken cancellationToken = default);
    }
}
