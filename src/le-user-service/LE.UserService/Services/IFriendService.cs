using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IFriendService
    {
        Task MakeFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken);
        Task AcceptFriendRequestAsync(Guid fromId, Guid toId, CancellationToken cancellationToken);
        Task UnFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken);
        Task UnFollowAsync(Guid fromId, Guid toId, CancellationToken cancellationToken);
        Task FollowFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken);
        Task DeleteFriendRequest(Guid requestId, Guid uId, CancellationToken cancellationToken);
        Task<IEnumerable<SuggestUserDto>> GetFriendsAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<SuggestUserDto>> GetFriendRequestsAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<SuggestUserDto>> SuggestFriendsAsync(Guid id, string[] naviveLangs, string[] targetLangs, string[] countryCodes, CancellationToken cancellationToken);
    }
}
