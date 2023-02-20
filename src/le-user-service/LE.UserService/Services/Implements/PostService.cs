using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class PostService /*: IPostService*/
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;

        public PostService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public Task CreatePost(PostDto postDto, CancellationToken cancellationToken = default)
        //{
        //    var post = new Post
        //    {
        //        Postid = Guid.NewGuid(),
        //        Langid = postDto.LangId,
        //        Userid = postDto.UserId,
        //        //IsAudio = postDto.AudioPost.Count > 0 ? 1 : 0;
        //    };
        //}
    }
}
