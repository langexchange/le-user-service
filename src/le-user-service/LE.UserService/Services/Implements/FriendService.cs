using AutoMapper;
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

        public FriendService(LanggeneralDbContext context, IMapper mapper, IUserDAL userDAL)
        {
            _context = context;
            _mapper = mapper;
            _userDAL = userDAL;
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

        public async Task<IEnumerable<UserDto>> GetFriendRequestsAsync(Guid id, CancellationToken cancellationToken)
        {
            var friendIdRequests = await _context.Relationships
                                        .Where(x => x.User2 == id && x.Action.Equals(Env.SendRequest) && x.Type == false)
                                        .Select(x => x.User1).ToListAsync();
            //query graph database
            var friends = await _userDAL.GetUsersAsync(friendIdRequests, cancellationToken);
            return friends;
        }

        public async Task<IEnumerable<UserDto>> GetFriendsAsync(Guid id, CancellationToken cancellationToken)
        {
            var friendIdRequests = await _context.Relationships
                                        .Where(x => (x.User1 == id || x.User2 == id) && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                        .Select(x => x.User2).ToListAsync();
            //query graph database
            var friends = await _userDAL.GetUsersAsync(friendIdRequests, cancellationToken);
            return friends;
        }

        public async Task MakeFriendAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => (x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId));
            if (request == null)
            {
                _context.Relationships.Add(new Relationship { User1 = fromId, User2 = toId, Type = false, Action = Env.SendRequest });
                _context.SaveChanges();
            }
        }

        public async Task AcceptFriendRequestAsync(Guid fromId, Guid toId, CancellationToken cancellationToken)
        {
            var request = await _context.Relationships.FirstOrDefaultAsync(x => (x.User1 == fromId && x.User2 == toId) || (x.User2 == fromId && x.User1 == toId));
            if (request == null)
                return;

            request.Type = true;
            _context.Relationships.Update(request);

            //crud neo4j
            await _userDAL.CrudFriendRelationshipAsync(fromId, toId, RelationValues.HAS_FRIEND, ModifiedState.Create, cancellationToken);

        }

        public Task<IEnumerable<Dictionary<string, object>>> SuggestFriendsAsync(Guid id, string[] naviveLangs, string[] targetLangs, string[] countryCodes, CancellationToken cancellationToken)
        {
            //suggest friend live in same country
            //suggest friend have the same nativelang(parner)
            //suggest friend have the same 
            //suggest friend learn hard
            throw new NotImplementedException();
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
    }
}
