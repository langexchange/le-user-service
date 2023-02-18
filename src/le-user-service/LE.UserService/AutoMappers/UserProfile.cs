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
                .ForMember(d => d.Id, s => s.MapFrom(x => x.Userid));

            //Map for user service
            CreateMap<User, UserDto>();
            CreateMap<BasicInfoRequest, UserDto>();
        }
    }
}
