using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class CommentService : ICommentService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        public CommentService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Guid> CreateComment(Guid postId, CommentDto commentDto, CancellationToken cancellationToken = default)
        {
            var comment = new Comment
            {
                Userid = commentDto.UserId,
                Postid = commentDto.PostId,
                Text = commentDto.Text,
                IsCorrect = !string.IsNullOrWhiteSpace(commentDto.Correctcmt),
                IsAudio = commentDto.Audiocmts.Count > 0 ? true : false,
                IsImage = commentDto.Imagecmts.Count > 0 ? true : false
            };

            await _context.Comments.AddAsync(comment);

            //if (comment.IsAudio.Value)
            //{
            //    var audioCmts = commentDto.Audiocmts.Select(x => new Audiopost { Postid = post.Postid, Url = x.Url }).ToList();
            //    await _context.Audiocmts.AddRangeAsync(audioCmts);
            //}
            //if (comment.IsImage.Value)
            //{
            //    var imageCmts = postDto.ImagePost.Select(x => new Imagepost { Postid = post.Postid, Url = x.Url }).ToList();
            //    await _context.Imageposts.AddRangeAsync(imagePosts);
            //}
            //if (comment.IsCorrect.Value)
            //{
            //    var videoPosts = postDto.VideoPost.Select(x => new Videopost { Postid = post.Postid, Url = x.Url }).ToList();
            //    await _context.Videoposts.AddRangeAsync(videoPosts);
            //}

            //await _context.SaveChangesAsync();

            return comment.Commentid;
        }

        public Task<List<CommentDto>> GetComments(Guid postId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsBelongToPost(Guid postId, Guid commentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateComment(Guid commentId, CommentDto commentDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteComment(Guid commentId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
