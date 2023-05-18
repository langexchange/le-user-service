using AutoMapper;
using LE.Library.Kernel;
using LE.Library.MessageBus;
using LE.UserService.Application.Events.ChatHelperEvent;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Neo4jData.DALs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class UserService : IUserService
    {
        private LanggeneralDbContext _context;
        private IUserDAL _userDAL;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IRequestHeader _requestHeader;

        public UserService(LanggeneralDbContext context, IMapper mapper, IUserDAL userDAL, IMessageBus messageBus, IRequestHeader requestHeader)
        {
            _context = context;
            _mapper = mapper;
            _userDAL = userDAL;
            _messageBus = messageBus;
            _requestHeader = requestHeader;
        }
        public async Task<UserDto> GetUser(Guid urequestId, Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id && x.IsRemoved != true);
            if (user == null)
                return null;

            var dto = _mapper.Map<UserDto>(user);
            if (user.NativeLang != null)
            {
                var nativeLangs = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == user.NativeLang);
                dto.NativeLanguage = _mapper.Map<LanguageDto>(nativeLangs);
                dto.NativeLanguage.Level = user.NativeLevel.Value;
            }
            var targetLangs = await _context.Targetlangs.Where(x => x.Userid == user.Userid).ToListAsync();
            var targetLangDtos = _mapper.Map<List<LanguageDto>>(targetLangs);
            foreach(var targetLangDto in targetLangDtos)
            {
                var language = await _context.Languages.FirstOrDefaultAsync(y => y.Langid == targetLangDto.Id);
                targetLangDto.Name = language.Name;
                targetLangDto.LocaleCode = language.LocaleCode;
            }

            dto.TargetLanguages = targetLangDtos;

            var hobbyIds = await _context.Userhobbies.Where(x => x.Userid == id).Select(x => x.Hobbyid).ToListAsync();
            var hobbies = await _context.Hobbies.Where(x => hobbyIds.Contains(x.Hobbyid)).Select(x => x.Name).ToArrayAsync();
            dto.Hobbies = hobbies;

            var numOfPosts = await _context.Posts.Where(x => x.Userid == id && x.IsPublic == true && x.IsPublic == true).CountAsync();
            dto.NumOfPosts = numOfPosts;

            var toId = await _context.Relationships
                                        .Where(x => x.User1 == id && x.Action.Equals(Env.SendRequest) && x.Type == true && x.User2 == urequestId)
                                        .FirstOrDefaultAsync();
           
            var fromId = await _context.Relationships
                                    .Where(x => x.User2 == id && x.Action.Equals(Env.SendRequest) && x.Type == true && x.User1 == urequestId)
                                    .FirstOrDefaultAsync();
            if (toId != null || fromId != null)
                dto.IsFriend = true;

            var friendIds = await _context.Relationships
                                       .Where(x => (x.User1 == id || x.User2 == id) && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                       .ToListAsync();
            dto.NumOfPartners = friendIds.Count;
            return dto;
        }

        public async Task<bool> SetBasicInfor(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            var isCreated = user.UpdatedAt != null ? false : true;

            if (user == null)
                return false;

            user.FirstName = userDto.FirstName?? user.FirstName;
            user.MiddleName = userDto.MiddleName ?? user.MiddleName;
            user.LastName = userDto.LastName ?? user.LastName;
            user.Gender = userDto.Gender ?? user.Gender;
            user.Country = userDto.Country ?? user.Country;
            user.Introduction = userDto.Introduction ?? user.Introduction;
            user.NativeLang = userDto.NativeLanguage.Id == Guid.Empty ? user.NativeLang : userDto.NativeLanguage.Id;
            user.NativeLevel = userDto.NativeLanguage.Level == 0 ? user.NativeLevel : userDto.NativeLanguage.Level;
            user.UpdatedAt = DateTime.UtcNow;
            _context.Update(user);

            var oldTargetLangs = await _context.Targetlangs.Where(x => x.Userid == id).ToListAsync();
            _context.Targetlangs.RemoveRange(oldTargetLangs);

            var targetLangs =  userDto.TargetLanguages.Select(x => new Targetlang { Langid = x.Id, Userid = id, TargetLevel = x.Level}).ToList();
            await _context.Targetlangs.AddRangeAsync(targetLangs);

            var oldHobbies = await _context.Userhobbies.Where(x => x.Userid == id).ToListAsync();
            _context.Userhobbies.RemoveRange(oldHobbies);

            foreach(var userHobby in userDto.Hobbies)
            {
                var hobby = await _context.Hobbies.Where(x => x.Name.Equals(userHobby)).FirstOrDefaultAsync();
                var hobbyId = hobby != null ? hobby.Hobbyid : Guid.NewGuid();
                if (hobby == null)
                {
                    _context.Hobbies.Add(new Hobby { Hobbyid = hobbyId, Name = userHobby });
                    await _context.SaveChangesAsync();
                }
                _context.Userhobbies.Add(new Userhobby { Hobbyid = hobbyId, Userid = user.Userid });
            }

            await _context.SaveChangesAsync();


            //crud graph db
            await _userDAL.SetBasicInforAsync(id, userDto, cancellationToken);

            //publish event
            //var uName = string.IsNullOrWhiteSpace(user.UserName) ? user.Email.Substring(0, user.Email.LastIndexOf("@")) : user.UserName;

            var nativeLang = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == userDto.NativeLanguage.Id);
            var langIds = await _context.Targetlangs.Where(x => x.Userid == id).Select(x => x.Langid).ToListAsync();
            var userTargetLangs = await _context.Languages.Where(x => langIds.Contains(x.Langid)).ToListAsync();

            var userInfoUpdatedEvent = new UserInfoUpdatedEvent
            {
                Jid = $"{id}@{Env.CHAT_DOMAIN}",
                FullName = $"{user.FirstName} {user.MiddleName} {user.LastName}",
                IsCreated = isCreated,
                NativeLanguage = nativeLang.LocaleCode,
                TargetLanguage = userTargetLangs.Select(x => x.LocaleCode).ToList()
            };
            await _messageBus.PublishAsync(userInfoUpdatedEvent, _requestHeader);
            return true;
        }

        public async Task<List<LanguageDto>> GetUserLanguages(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            if(user == null)
                return null;
            var dtos = new List<LanguageDto>();
            var nativeLang = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == user.NativeLang);
            dtos.Add(_mapper.Map<LanguageDto>(nativeLang));

            var langIds = await _context.Targetlangs.Where(x => x.Userid == id).Select(x => x.Langid).ToListAsync();
            var targetLangs = await _context.Languages.Where(x => langIds.Contains(x.Langid)).ToListAsync();
            dtos.AddRange(_mapper.Map<IEnumerable<LanguageDto>>(targetLangs));
            return dtos;
        }

        public async Task<bool> ChangeAvatar(Guid id, string avatar, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            if (user == null)
                return false;

            user.Avartar = avatar;
            _context.Update(user);
            await _context.SaveChangesAsync();

            await _userDAL.ChangeAvatarAsync(id, avatar, cancellationToken);

            //publish event
            //var uName = string.IsNullOrWhiteSpace(user.UserName) ? user.Email.Substring(0, user.Email.LastIndexOf("@")) : user.UserName;
            var userInfoUpdatedEvent = new UserInfoUpdatedEvent
            {
                Jid = $"{id}@{Env.CHAT_DOMAIN}",
                Avatar = user.Avartar
            };
            await _messageBus.PublishAsync(userInfoUpdatedEvent, _requestHeader);
            return true;
        }

        public async Task<List<UserDto>> GetUsers(Guid urequestId, CancellationToken cancellationToken = default)
        {
            var userIds = await _context.Users.Where(x => x.IsRemoved != true).Select(x => x.Userid).ToListAsync();
            var dtos = new List<UserDto>();
            foreach(var id in userIds)
            {
                var dto = await GetUser(urequestId, id, cancellationToken);
                dtos.Add(dto);
            }
            return dtos;
        }

        public async Task<List<SuggestUserDto>> GetUsersNeo4jAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var dtos = await _userDAL.GetUsersAsync(ids, cancellationToken);
            return dtos;
        }
    }
}
