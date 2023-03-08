using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Models.Responses;

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

            CreateMap<Comment, CommentDto>()
                .ReverseMap();
            CreateMap<CommentRequest, CommentDto>();
            CreateMap<FileOfCommentRequest, FileOfComment>();
        }
    }
}
