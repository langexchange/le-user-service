using LE.UserService.Dtos;
using LE.UserService.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IUserDAL
    {
        Task<bool> SetBasicInforAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default);
        Task<IEnumerable<Dictionary<string, object>>> GetUsersAsync(CancellationToken cancellationToken = default);
        Task<bool> ChangeAvatarAsync(Guid id, string avatar, CancellationToken cancellationToken = default);
        Task<IEnumerable<Dictionary<string, object>>> GetUsersAsync(List<Guid> ids, CancellationToken cancellationToken = default);
        Task CrudFriendRelationshipAsync(Guid fromId, Guid toId, string relation, ModifiedState mode, CancellationToken cancellationToken);
    }
}
