using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
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
    public class PostService : IPostService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;

        public PostService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task CreatePost(PostDto postDto, CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Postid = Guid.NewGuid(),
                Langid = postDto.LangId,
                Userid = postDto.UserId,
                Label = postDto.Label,
                Text = postDto.Text,
                IsAudio = postDto.AudioPost.Count > 0 ? true: false,
                IsImage = postDto.ImagePost.Count > 0 ? true: false,
                IsVideo = postDto.VideoPost.Count > 0 ? true: false,
            };
            await _context.Posts.AddAsync(post);

            if (post.IsAudio.Value)
            {
                var audioPosts = postDto.AudioPost.Select(x => new Audiopost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Audioposts.AddRangeAsync(audioPosts);
            }
            if (post.IsImage.Value)
            {
                var imagePosts = postDto.ImagePost.Select(x => new Imagepost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Imageposts.AddRangeAsync(imagePosts);
            }
            if (post.IsVideo.Value)
            {
                var videoPosts = postDto.VideoPost.Select(x => new Videopost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Videoposts.AddRangeAsync(videoPosts);
            }

            await _context.SaveChangesAsync();
        }

        public async Task UpdatePost(Guid postId, PostDto postDto, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId);
            if (post == null)
                throw new Exception("Not found post");

            post.Langid = postDto.LangId;
            post.Userid = postDto.UserId;
            post.Label = postDto.Label;
            post.Text = postDto.Text;
            post.IsAudio = postDto.AudioPost.Count > 0 ? true : false;
            post.IsImage = postDto.ImagePost.Count > 0 ? true : false;
            post.IsVideo = postDto.VideoPost.Count > 0 ? true : false;

            var oldAudioPosts = await _context.Audioposts.Where(x => x.Postid == postId).ToListAsync();
            var oldImagePosts = await _context.Imageposts.Where(x => x.Postid == postId).ToListAsync();
            var oldVideoPosts = await _context.Videoposts.Where(x => x.Postid == postId).ToListAsync();

            _context.Audioposts.RemoveRange(oldAudioPosts);
            _context.Imageposts.RemoveRange(oldImagePosts);
            _context.Videoposts.RemoveRange(oldVideoPosts);

            if (postDto.AudioPost.Count > 0)
            {
                var audioPosts = postDto.AudioPost.Select(x => new Audiopost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Audioposts.AddRangeAsync(audioPosts);
            }
            if (post.IsImage.Value)
            {
                var imagePosts = postDto.ImagePost.Select(x => new Imagepost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Imageposts.AddRangeAsync(imagePosts);
            }
            if (post.IsVideo.Value)
            {
                var videoPosts = postDto.VideoPost.Select(x => new Videopost { Postid = post.Postid, Url = x.Url }).ToList();
                await _context.Videoposts.AddRangeAsync(videoPosts);
            }

            await _context.SaveChangesAsync();
        }

        public async Task SetPostState(Guid postId, PostState state, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId);
            if (post == null)
                throw new Exception("Not found post");

            switch (state)
            {
                case PostState.Publish:
                    post.IsPublic = true;
                    break;
                case PostState.Private:
                    post.IsPublic = false;
                    break;
                case PostState.Delete:
                    post.IsRemoved = true;
                    break;
                default:
                    return;
            }
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PostDto> GetPost(Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId);
            if (post == null)
                return null;

            var postDto = _mapper.Map<PostDto>(post);
            if (post.IsAudio.Value)
            {
                var audioPosts = _context.Audioposts.Where(x => x.Postid == postId).ToList();
                postDto.AudioPost = audioPosts.Select(x => new FileOfPost { Type = "audio", Url = x.Url }).ToList();
            }
            if (post.IsImage.Value)
            {
                var imagePosts = _context.Imageposts.Where(x => x.Postid == postId).ToList();
                postDto.AudioPost = imagePosts.Select(x => new FileOfPost { Type = "image", Url = x.Url }).ToList();
            }
            if (post.IsVideo.Value)
            {
                var videoPosts = _context.Imageposts.Where(x => x.Postid == postId).ToList();
                postDto.AudioPost = videoPosts.Select(x => new FileOfPost { Type = "video", Url = x.Url }).ToList();
            }

            return postDto;
        }

        public async Task<List<PostDto>> GetPostsOfUser(Guid userId, CancellationToken cancellationToken = default)
        {
            var posts = await _context.Posts.Where(x => x.Userid == userId).ToListAsync();
            if (posts == null)
                return null;

            //need to query langguage
            var postDtos = new List<PostDto>();
            foreach(var post in posts)
            {
                var postDto = _mapper.Map<PostDto>(post);
                if (post.IsAudio.Value)
                {
                    var audioPosts = _context.Audioposts.Where(x => x.Postid == post.Postid).ToList();
                    postDto.AudioPost = audioPosts.Select(x => new FileOfPost { Type = "audio", Url = x.Url }).ToList();
                }
                if (post.IsImage.Value)
                {
                    var imagePosts = _context.Imageposts.Where(x => x.Postid == post.Postid).ToList();
                    postDto.AudioPost = imagePosts.Select(x => new FileOfPost { Type = "image", Url = x.Url }).ToList();
                }
                if (post.IsVideo.Value)
                {
                    var videoPosts = _context.Imageposts.Where(x => x.Postid == post.Postid).ToList();
                    postDto.AudioPost = videoPosts.Select(x => new FileOfPost { Type = "video", Url = x.Url }).ToList();
                }
                postDtos.Add(postDto);
            }
            

            return postDtos;
        }
    }
}
