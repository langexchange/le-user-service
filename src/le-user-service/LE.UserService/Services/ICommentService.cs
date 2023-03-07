using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface ICommentService
    {
        Task<List<CommentDto>> GetComments(Guid postId, CancellationToken cancellationToken = default);
        Task<Guid> CreateComment(Guid postId, CommentDto commentDto, CancellationToken cancellationToken = default);
        Task UpdateComment(Guid commentId, CommentDto commentDto, CancellationToken cancellationToken = default);
        Task<bool> IsBelongToPost(Guid postId, Guid commentId, CancellationToken cancellationToken = default);
        Task<bool> IsBelongToUser(Guid useId, Guid commentId, CancellationToken cancellationToken = default);
        Task DeleteComment(Guid commentId, CancellationToken cancellationToken = default);
        Task InteractComment(Guid commentId, Guid userId, string mode, CancellationToken cancellationToken = default);
        Task<int> GetCmtInteract(Guid commentId, CancellationToken cancellationToken = default);
    }
}
