using AutoMapper;
using LE.Library.Kernel;
using LE.Library.MessageBus;
using LE.UserService.Application.Events;
using LE.UserService.Application.Events.ChatHelperEvent;
using LE.UserService.Constants;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Neo4jData.DALs;
using LE.UserService.Neo4jData.DALs.NodeRelationConstants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class FriendService : IFriendService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserDAL _userDAL;
        private readonly IMessageBus _messageBus;
        private readonly IRequestHeader _requestHeader;

        public FriendService(LanggeneralDbContext context, IMapper mapper, IUserDAL userDAL, IMessageBus messageBus, IRequestHeader requestHeader)
        {
            _context = context;
            _mapper = mapper;
            _userDAL = userDAL;
            _messageBus = messageBus;
            _requestHeader = requestHeader;
        }

        public async Task FollowFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => (x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId));
            if (request == null)
            {
                _context.Relationships.Add(new Relationship { User1 = fromId, User2 = toId, Type = false, Action = Env.Follow });
                _context.SaveChanges();
            }

            //crud neo4j
            await _userDAL.CrudFriendRelationshipAsync(fromId, toId, RelationValues.FOLLOW, ModifiedState.Create, cancellationToken);
        }

        public async Task<IEnumerable<SuggestUserDto>> GetFriendRequestsAsync(Guid id, CancellationToken cancellationToken)
        {
            var friendIdRequests = await _context.Relationships
                                        .Where(x => x.User2 == id && x.Action.Equals(Env.SendRequest) && x.Type == false)
                                        .Select(x => x.User1).ToListAsync();
            //query graph database
            var friends = await _userDAL.GetUsersAsync(friendIdRequests, cancellationToken);
            return friends;
        }

        public async Task<IEnumerable<SuggestUserDto>> GetFriendsAsync(Guid id, CancellationToken cancellationToken)
        {
            var ids = new List<Guid>();
            var toIds = await _context.Relationships
                                        .Where(x => x.User1 == id && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                        .Select(x => x.User2).ToListAsync();
            var fromIds = await _context.Relationships
                                        .Where(x => x.User2 == id && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                        .Select(x => x.User1).ToListAsync();
            ids.AddRange(toIds);
            ids.AddRange(fromIds);

            //query graph database
            var friends = await _userDAL.GetUsersAsync(id, ids, cancellationToken);
            return friends;
        }

        public async Task MakeFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => (x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId));
            if (request == null)
            {
                _context.Relationships.Add(new Relationship { User1 = fromId, User2 = toId, Type = false, Action = Env.SendRequest });
                await _context.SaveChangesAsync();

                //publish event
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == fromId);
                if (user == null)
                    return;
                var @event = new FriendRequestSentEvent
                {
                    FromId = fromId,
                    ToId = toId,
                    UserName = $"{user.FirstName} {user.LastName}",
                    NotifyIds = new List<Guid> { toId }
                };
                await _messageBus.PublishAsync(@event, _requestHeader, cancellationToken);
            }
        }

        public async Task AcceptFriendRequestAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => (x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId));
            if (request == null)
                return;

            request.Type = true;
            _context.Relationships.Update(request);
            await _context.SaveChangesAsync();

            //crud neo4j
            await _userDAL.CrudFriendRelationshipAsync(fromId, toId, RelationValues.HAS_FRIEND, ModifiedState.Create, cancellationToken);

            //publish event
            var fromUser = await _context.Users.FirstOrDefaultAsync(x => x.Userid == fromId);
            //var toUser = await _context.Users.FirstOrDefaultAsync(x => x.Userid == toId);
            //var fromUname = string.IsNullOrWhiteSpace(fromUser.UserName) ? fromUser.Email.Substring(0, fromUser.Email.LastIndexOf("@")): fromUser.UserName;
            //var toUname = string.IsNullOrWhiteSpace(toUser.UserName) ? toUser.Email.Substring(0, toUser.Email.LastIndexOf("@")): toUser.UserName;

            if (fromUser == null)
                return;
            var @event = new FriendRequestAcceptedEvent
            {
                FromId = fromId,
                ToId = toId,
                UserName = $"{fromUser.FirstName} {fromUser.LastName}",
                NotifyIds = new List<Guid> { toId }
            };
            var friendStateEvent = new FriendStateUpdatedEvent
            {
                Jid1 = $"{fromId}@{Env.CHAT_DOMAIN}",
                Jid2 = $"{toId}@{Env.CHAT_DOMAIN}",
                State = FriendState.FRIEND
            };
            await _messageBus.PublishAsync(@event, _requestHeader, cancellationToken);
            await _messageBus.PublishAsync(friendStateEvent, _requestHeader, cancellationToken);
        }

        public async Task<IEnumerable<SuggestUserDto>> SuggestFriendsAsync(Guid id, string[] naviveLangs, string[] targetLangs, string[] countryCodes, CancellationToken cancellationToken)
        {
            var upperNativeLangs = naviveLangs.Select(x => x.ToUpper()).ToArray();
            var upperTargetLangs = targetLangs.Select(x => x.ToUpper()).ToArray();
            var upperCountryCodes = countryCodes.Select(x => x.ToUpper()).ToArray();

            var result = await _userDAL.SuggestFriendsAsync(id, upperNativeLangs, upperTargetLangs, upperCountryCodes, cancellationToken);
            return result;
        }

        public async Task UnFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => x.Action.Equals(Env.SendRequest) && ((x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId)));
            if (request == null)
                return;

            _context.Relationships.Remove(request);
            _context.SaveChanges();

            //crud neo4j
            await _userDAL.CrudFriendRelationshipAsync(fromId, toId, RelationValues.HAS_FRIEND, ModifiedState.Delete, cancellationToken);

            //publish event
            //var fromUser = await _context.Users.FirstOrDefaultAsync(x => x.Userid == fromId);
            //var toUser = await _context.Users.FirstOrDefaultAsync(x => x.Userid == toId);
            //var fromUname = string.IsNullOrWhiteSpace(fromUser.UserName) ? fromUser.Email.Substring(0, fromUser.Email.LastIndexOf("@")) : fromUser.UserName;
            //var toUname = string.IsNullOrWhiteSpace(toUser.UserName) ? toUser.Email.Substring(0, toUser.Email.LastIndexOf("@")) : toUser.UserName;

            var friendStateEvent = new FriendStateUpdatedEvent
            {
                Jid1 = $"{fromId}@{Env.CHAT_DOMAIN}",
                Jid2 = $"{toId}@{Env.CHAT_DOMAIN}",
                State = FriendState.UNFRIEND
            };
            await _messageBus.PublishAsync(friendStateEvent, _requestHeader, cancellationToken);
        }

        public async Task UnFollowAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => x.Action.Equals(Env.Follow) && ((x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId)));
            if (request == null)
                return;

            _context.Relationships.Remove(request);
            _context.SaveChanges();

            //crud neo4j
            await _userDAL.CrudFriendRelationshipAsync(fromId, toId, RelationValues.FOLLOW, ModifiedState.Delete, cancellationToken);
        }

        public async Task DeleteFriendRequest(Guid requestId, Guid uId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => x.User1 == requestId && x.User2 == uId && x.Action.Equals(Env.SendRequest) && x.Type == false);
            if (request == null)
                return;
            _context.Relationships.Remove(request);
            await _context.SaveChangesAsync();
        }
    }
}
