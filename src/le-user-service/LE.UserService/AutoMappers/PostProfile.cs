using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;

namespace LE.UserService.AutoMappers
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDto>()
                .ReverseMap();
            CreateMap<PostRequest, PostDto>();
            CreateMap<FileOfPostRequest, FileOfPost>();
        }
    }
}
