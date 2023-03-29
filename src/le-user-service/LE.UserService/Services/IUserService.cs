using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUser(Guid urequestId, Guid id, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetUsers(Guid urequestId, CancellationToken cancellationToken = default);
        Task<bool> SetBasicInfor(Guid id, UserDto userDto, CancellationToken cancellationToken = default);
        Task<List<LanguageDto>> GetUserLanguages(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ChangeAvatar(Guid id, string avatar, CancellationToken cancellationToken = default);

    }
}
