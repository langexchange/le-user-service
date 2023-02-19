using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
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
        private readonly IMapper _mapper;

        public UserService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserDto> GetUser(Guid id, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);
            var dto = _mapper.Map<UserDto>(user);
            if (user.NativeLang != null)
            {
                var nativeLangs = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == user.NativeLang);
                dto.NativeLanguage = _mapper.Map<LanguageDto>(nativeLangs);
                dto.NativeLanguage.Level = user.NativeLevel.Value;
            }
            var targetLangs = await _context.Targetlangs.Where(x => x.Userid == user.Userid).ToListAsync();
            var targetLangDtos = _mapper.Map<List<LanguageDto>>(targetLangs);
            targetLangDtos.ForEach(x => { x.Name = _context.Languages.FirstOrDefault(y => y.Langid == x.Id).Name; });
            dto.TargetLanguages = targetLangDtos;

            return dto;
        }

        public async Task<bool> SetBasicInfor(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == id);

            if (user == null)
                return false;

            user.FirstName = userDto.FirstName?? user.FirstName;
            user.MiddleName = userDto.MiddleName ?? user.MiddleName;
            user.LastName = userDto.LastName ?? user.LastName;
            user.Gender = userDto.Gender ?? user.Gender;
            user.Introduction = userDto.Introduction ?? user.Introduction;
            user.NativeLang = userDto.NativeLanguage.Id == Guid.Empty ? user.NativeLang : userDto.NativeLanguage.Id;
            user.NativeLevel = userDto.NativeLanguage.Level == 0 ? user.NativeLevel : userDto.NativeLanguage.Level;
            _context.Update(user);

            var oldTargetLangs = await _context.Targetlangs.Where(x => x.Userid == id).ToListAsync();
            _context.Targetlangs.RemoveRange(oldTargetLangs);

            var targetLangs =  userDto.TargetLanguages.Select(x => new Targetlang { Langid = x.Id, Userid = id, TargetLevel = x.Level}).ToList();
            await _context.Targetlangs.AddRangeAsync(targetLangs);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
