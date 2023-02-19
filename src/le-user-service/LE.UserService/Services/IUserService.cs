using LE.UserService.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUser(Guid id, CancellationToken cancellationToken = default);
        Task<bool> SetBasicInfor(Guid id, UserDto userDto, CancellationToken cancellationToken = default);
    }
}
