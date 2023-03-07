using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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

        public async Task<Guid> CreatePost(PostDto postDto, CancellationToken cancellationToken = default)
        {
            var post = new Post
            {
                Postid = Guid.NewGuid(),
                Langid = postDto.LangId,
                Userid = postDto.UserId,
                Label = postDto.Label,
                Text = postDto.Text,
                IsAudio = postDto.AudioPost.Count > 0 ? true : false,
                IsImage = postDto.ImagePost.Count > 0 ? true : false,
                IsVideo = postDto.VideoPost.Count > 0 ? true : false,
                IsPublic = postDto.IsPublic,
                RestrictBits = new BitArray(3, true)
            };
            if (postDto.IsTurnOffComment)
                post.RestrictBits.Set(1, false);
            if (postDto.IsTurnOffCorrection)
                post.RestrictBits.Set(2, false);
            if (postDto.IsTurnOffShare)
                post.RestrictBits.Set(0, false);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

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

            return post.Postid;
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
            post.IsPublic = postDto.IsPublic;

            if (postDto.IsTurnOffComment)
                post.RestrictBits.Set(1, false);
            if(postDto.IsTurnOffCorrection)
                post.RestrictBits.Set(2, false);
            if (postDto.IsTurnOffShare)
                post.RestrictBits.Set(0, false);

            _context.Update(post);
            
            //update reference table
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
                case PostState.TurnOffComment:
                    post.RestrictBits.Set(1, false);
                    break;
                case PostState.TurnOffShare:
                    post.RestrictBits.Set(0, false);
                    break;
                case PostState.TurnOffCorrect:
                    post.RestrictBits.Set(2, false);
                    break;
                default:
                    return;
            }
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PostDto> GetPost(Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId && x.IsRemoved.Value == false && x.IsPublic.Value == true);
            if (post == null)
                return null;

            var postDto = _mapper.Map<PostDto>(post);
            var language = await _context.Languages.FirstOrDefaultAsync(x => x.Langid == post.Langid);
            postDto.LangName = language.Name;
            postDto.IsTurnOffCorrection = !post.RestrictBits.Get(2);
            postDto.IsTurnOffShare = !post.RestrictBits.Get(0);
            postDto.IsTurnOffComment = !post.RestrictBits.Get(1);

            var numOfInteract = await _context.Userintposts.Where(x => x.Postid == postId).CountAsync(cancellationToken);
            postDto.NumOfInteract = numOfInteract;

            if (post.IsAudio.Value)
            {
                var audioPosts = _context.Audioposts.Where(x => x.Postid == postId).ToList();
                postDto.AudioPost = audioPosts.Select(x => new FileOfPost { Type = "audio", Url = x.Url }).ToList();
            }
            if (post.IsImage.Value)
            {
                var imagePosts = _context.Imageposts.Where(x => x.Postid == postId).ToList();
                postDto.ImagePost = imagePosts.Select(x => new FileOfPost { Type = "image", Url = x.Url }).ToList();
            }
            if (post.IsVideo.Value)
            {
                var videoPosts = _context.Videoposts.Where(x => x.Postid == postId).ToList();
                postDto.VideoPost = videoPosts.Select(x => new FileOfPost { Type = "video", Url = x.Url }).ToList();
            }
            return postDto;
        }

        public async Task<List<PostDto>> GetPosts(Guid userId, Mode mode, CancellationToken cancellationToken = default)
        {
            var posts = new List<Post>(); 
            switch (mode)
            {
                case Mode.Get:
                    posts = await _context.Posts.Where(x => x.Userid == userId && x.IsRemoved.Value == false).ToListAsync();
                    break;
                case Mode.Recommend:
                    // implement
                    break;
            }
            if (posts == null || posts.Count == 0)
                return null;

            //need to query langguage
            var postDtos = new List<PostDto>();
            foreach(var post in posts)
            {
                var postDto = await GetPost(post.Postid, cancellationToken);
                postDtos.Add(postDto);
            }
            return postDtos;
        }

        public async Task<bool> IsBelongToUser(Guid postId, Guid userId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId);
            return post?.Userid == userId;
        }

        public async Task InteractPost(Guid postId, Guid userId, string mode, CancellationToken cancellationToken = default)
        {
            await InitInteraction();
            var interactType = await _context.Interactions.ToListAsync();

            var userInteractPost = await _context.Userintposts.FirstOrDefaultAsync(x => x.Userid == userId && x.Postid == postId);
            var interactTypeId = interactType.Where(x => x.Stringcode.Equals(mode)).FirstOrDefault()?.Interactid;
            if (userInteractPost == null)
                _context.Add(new Userintpost { Userid = userId, Postid = postId, InteractType = interactTypeId.Value });
            else
            {
                userInteractPost.InteractType = interactTypeId.HasValue ? interactTypeId.Value : userInteractPost.InteractType;
                _context.Userintposts.Update(userInteractPost);
            }
            _context.SaveChanges();
        }

        private async Task InitInteraction()
        {
            var interactType = await _context.Interactions.FirstOrDefaultAsync();
            if (interactType != null)
                return;
            
            _context.Interactions.AddRange(
                new Interaction { Stringcode = "Like" },
                new Interaction { Stringcode = "Favorite" }
                );
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetPostInteract(Guid postId, CancellationToken cancellationToken = default)
        {
            var numOfInteract = await _context.Userintposts.Where(x => x.Postid == postId).CountAsync(cancellationToken);
            return numOfInteract;
        }
    }
}
