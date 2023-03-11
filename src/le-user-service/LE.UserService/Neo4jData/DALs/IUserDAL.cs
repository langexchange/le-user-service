using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IUserDAL
    {
        Task<bool> SetBasicInfor(Guid id, UserDto userDto, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetUsers(CancellationToken cancellationToken = default);
        Task<bool> ChangeAvatar(Guid id, string avatar, CancellationToken cancellationToken = default);
    }
}
