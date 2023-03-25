using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IPostDAL
    {
        Task<bool> CreateOrUpdatePost(PostDto postDto, CancellationToken cancellationToken = default);
        Task<bool> ConfigPost(Guid postId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default);
        Task<List<Guid>> FilterPostByLanguages(List<Guid> langIds, CancellationToken cancellationToken = default);
        Task<List<Guid>> SuggestPostsAsync(Guid id, List<Guid> langIds, bool isOnlyFriend = false, bool isNewest = true, CancellationToken cancellationToken = default);
    }
}
