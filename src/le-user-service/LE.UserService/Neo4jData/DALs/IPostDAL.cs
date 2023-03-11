using LE.UserService.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IPostDAL
    {
        Task<bool> CreateOrUpdatePost(PostDto postDto, CancellationToken cancellationToken = default);
        Task<bool> ConfigPost(Guid postId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default);
    }
}
