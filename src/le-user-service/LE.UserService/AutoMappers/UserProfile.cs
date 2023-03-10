using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;

namespace LE.UserService.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //Map for auth service
            CreateMap<RegisterRequest, User>();
            CreateMap<User, AuthResponse>()
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Userid))
                .ForMember(d => d.IncId, s => s.MapFrom(x => x.IncreateId));

            //Map for user service
            CreateMap<User, UserDto>()
                .ForMember(d => d.Avatar, s => s.MapFrom(x => x.Avartar));
            CreateMap<BasicInfoRequest, UserDto>()
                .ForMember(d => d.NativeLanguage, s => s.MapFrom(x => x.NativeLanguage))
                .ForMember(d => d.TargetLanguages, s => s.MapFrom(x => x.TargetLanguages));
        }
    }
}
