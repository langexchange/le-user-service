using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;

namespace LE.UserService.AutoMappers
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>()
                .ReverseMap();
        }
    }
}
