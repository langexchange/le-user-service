﻿using AutoMapper;
using LE.Library.Kernel;
using LE.Library.MessageBus;
using LE.UserService.Application.Events;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Neo4jData.DALs;
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
        private IPostDAL _postDAL;
        private IUserService _userService;
        private ILangService _langService;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IRequestHeader _requestHeader;

        public PostService(LanggeneralDbContext context, IMapper mapper, IPostDAL postDAL, IUserService userService, ILangService langService
            , IMessageBus messageBus, IRequestHeader requestHeader)
        {
            _context = context;
            _mapper = mapper;
            _postDAL = postDAL;
            _userService = userService;
            _langService = langService;
            _messageBus = messageBus;
            _requestHeader = requestHeader;
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
                CreatedAt = postDto.CreatedAt,
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

            //crud neo4j db
            postDto.PostId = post.Postid;
            await _postDAL.CreateOrUpdatePost(postDto, cancellationToken);

            //collect data to notifi
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == postDto.UserId);
            var ids = new List<Guid>();
            var toIds = await _context.Relationships
                                        .Where(x => x.User1 == postDto.UserId && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                        .Select(x => x.User2).ToListAsync();
            var fromIds = await _context.Relationships
                                        .Where(x => x.User2 == postDto.UserId && x.Action.Equals(Env.SendRequest) && x.Type == true)
                                        .Select(x => x.User1).ToListAsync();
            ids.AddRange(toIds);
            ids.AddRange(fromIds);

            var @event = new PostCreatedEvent
            {
                PostId = post.Postid,
                UserId = postDto.UserId,
                UserName = user == null ? "": $"{user.FirstName} {user.LastName}",
                NotifyIds = ids
            };

            await _messageBus.PublishAsync(@event, _requestHeader, cancellationToken);

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
            post.UpdatedAt = post.UpdatedAt;

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

            //crud neo4j db
            postDto.PostId = postId;
            await _postDAL.CreateOrUpdatePost(postDto, cancellationToken);
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
                    await _postDAL.ConfigPost(postId, true, null, cancellationToken);
                    break;
                case PostState.Private:
                    post.IsPublic = false;
                    await _postDAL.ConfigPost(postId, false, null, cancellationToken);
                    break;
                case PostState.Delete:
                    post.IsRemoved = true;
                    await _postDAL.ConfigPost(postId, null, true, cancellationToken);
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
                case PostState.TurnOnShare:
                    post.RestrictBits.Set(0, true);
                    break;
                case PostState.TurnOnCorrect:
                    post.RestrictBits.Set(2, true);
                    break;
                default:
                    return;
            }
            _context.Update(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PostDto> GetPost(Guid postId, CancellationToken cancellationToken = default)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId && x.IsRemoved.Value == false);
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

            var numOfCmt = await _context.Comments.Where(x => x.Postid == postId && x.IsRemoved.Value == false).CountAsync(cancellationToken);
            postDto.NumOfCmt = numOfCmt;

            postDto.CreatedAt = post.CreatedAt?.ToLocalTime();
            postDto.UpdatedAt = post.UpdatedAt?.ToLocalTime();

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == post.Userid);
            postDto.UserInfo.Id = post.Userid.Value;
            postDto.UserInfo.FirstName = user?.FirstName;
            postDto.UserInfo.LastName = user?.LastName;
            postDto.UserInfo.Avatar = user?.Avartar;

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

        public async Task<PostDto> GetPost(Guid urequestId, Guid postId, CancellationToken cancellationToken = default)
        {
            var postDto = await GetPost(postId, cancellationToken);
            if (postDto == null || (postDto.IsPublic == false && postDto.UserId != urequestId))
                return null;
            var userInteracted = await _context.Userintposts.Where(x => x.Postid == postId).Select(x => x.Userid).ToListAsync();
            postDto.IsUserInteracted = userInteracted != null && userInteracted.Any(x => x == urequestId);
            return postDto;
        }
        public async Task<List<PostDto>> GetPosts(Guid uresquestId, Guid userId, Mode mode, CancellationToken cancellationToken = default)
        {
            var postIds = new List<Guid>(); 
            switch (mode)
            {
                case Mode.Get:
                    if(uresquestId == userId)
                        postIds = await _context.Posts.Where(x => x.Userid == userId && x.IsRemoved.Value == false).Select(x => x.Postid).ToListAsync();
                    else postIds = await _context.Posts.Where(x => x.Userid == userId && x.IsRemoved.Value == false && x.IsPublic == true).Select(x => x.Postid).ToListAsync();
                    break;
                case Mode.Recommend:
                    var langs = await _userService.GetUserLanguages(userId);
                    var langIds = langs.Select(x => x.Id).ToList();
                    postIds = await _postDAL.FilterPostByLanguages(langIds);
                    break;
            }
            if (postIds.Count == 0)
                return null;

            //need to query langguage
            var postDtos = new List<PostDto>();
            foreach(var postId in postIds)
            {
                var postDto = await GetPost(postId, cancellationToken);
                if (postDto == null)
                    continue;
                var userInteracted = await _context.Userintposts.Where(x => x.Postid == postId).Select(x => x.Userid).ToListAsync();
                postDto.IsUserInteracted = userInteracted != null && userInteracted.Any(x => x == uresquestId);
                postDtos.Add(postDto);
            }
            return postDtos;
        }

        public async Task<List<PostDto>> SuggestPostsAsync(Guid uresquestId, string[] filterLangs, bool isOnlyFriend = false, bool isNewest = true, CancellationToken cancellationToken = default)
        {
            var filterUpperLangs = filterLangs.Select(x => x.ToUpper()).ToArray();
            var langIds = await _langService.GetLangIds(filterUpperLangs, cancellationToken);
            if (filterUpperLangs.Length == 0)
            {
                var langs = await _userService.GetUserLanguages(uresquestId);
                langIds = langs.Select(x => x.Id).ToList();
            }

            var postIds = await _postDAL.SuggestPostsAsync(uresquestId, langIds, isOnlyFriend, isNewest, cancellationToken);

            var postDtos = new List<PostDto>();
            foreach (var postId in postIds)
            {
                var postDto = await GetPost(postId, cancellationToken);
                if (postDto == null)
                    continue;
                var userInteracted = await _context.Userintposts.Where(x => x.Postid == postId).Select(x => x.Userid).ToListAsync();
                postDto.IsUserInteracted = userInteracted != null && userInteracted.Any(x => x == uresquestId);
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
            var userInteractPost = await _context.Userintposts.FirstOrDefaultAsync(x => x.Userid == userId && x.Postid == postId);

            if (userInteractPost == null && mode.Equals("UnLike"))
                return;
            if(mode.Equals("UnLike"))
            {
                _context.Userintposts.Remove(userInteractPost);
                await _context.SaveChangesAsync();
                return;
            }

            var interactType = await _context.Interactions.ToListAsync();
            var interactTypeId = interactType.Where(x => x.Stringcode.Equals(mode)).FirstOrDefault()?.Interactid;
            if (userInteractPost == null)
                _context.Add(new Userintpost { Userid = userId, Postid = postId, InteractType = interactTypeId.Value });
            else
            {
                userInteractPost.InteractType = interactTypeId.HasValue ? interactTypeId.Value : userInteractPost.InteractType;
                _context.Userintposts.Update(userInteractPost);
            }
            await _context.SaveChangesAsync();
            //publish event

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == userId);
            var currentInteract = await _context.Userintposts.Where(x => x.Postid == postId).CountAsync();
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Postid == postId);
            var notifyIds = new List<Guid>();
            notifyIds.Add(post.Userid.Value);

            var @event = new InteractPostEvent
            {
                UserId = userId,
                UserName = $"{user.FirstName} {user.LastName}",
                CurrentInteract = currentInteract,
                PostId = postId,
                InteractType = mode,
                NotifyIds = notifyIds
            };
            await _messageBus.PublishAsync(@event, _requestHeader, cancellationToken);
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
