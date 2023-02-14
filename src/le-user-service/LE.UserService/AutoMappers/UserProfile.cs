using AutoMapper;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;

namespace LE.UserService.AutoMappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterRequest, User>();
        }
    }
}
