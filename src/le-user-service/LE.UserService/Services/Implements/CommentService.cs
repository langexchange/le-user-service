using AutoMapper;
using LE.Library.Kernel;
using LE.Library.MessageBus;
using LE.UserService.Application.Events;
using LE.UserService.Dtos;
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
    public class CommentService : ICommentService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMessageBus _messageBus;
        private readonly IRequestHeader _requestHeader; 
        public CommentService(LanggeneralDbContext context, IMapper mapper, IMessageBus messageBus, IRequestHeader requestHeader)
        {
            _context = context;
            _mapper = mapper;
            _messageBus = messageBus;
            _requestHeader = requestHeader;
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
            await _context.SaveChangesAsync();

            if (comment.IsAudio.Value)
            {
                var audioCmts = commentDto.Audiocmts.Select(x => new Audiocmt { Commentid = comment.Commentid, Url = x.Url }).ToList();
                await _context.Audiocmts.AddRangeAsync(audioCmts);
            }
            if (comment.IsImage.Value)
            {
                var imageCmts = commentDto.Imagecmts.Select(x => new Imagecmt { Commentid = comment.Commentid, Url = x.Url }).ToList();
                await _context.Imagecmts.AddRangeAsync(imageCmts);
            }
            if (comment.IsCorrect.Value)
            {
                var correctCmt = new Correctcmt { Commentid = comment.Commentid, CorrectText = commentDto.Correctcmt };
                await _context.Correctcmts.AddAsync(correctCmt);
            }

            await _context.SaveChangesAsync();

            return comment.Commentid;
        }
        private async Task<CommentDto> GetComment(Guid commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId && x.IsRemoved.Value == false);
            if (comment == null)
                return null;

            var commentDto = _mapper.Map<CommentDto>(comment);

            var numOfInteract = await _context.Cmtinteracts.Where(x => x.Commentid == commentId).CountAsync(cancellationToken);
            commentDto.NumOfInteract = numOfInteract;
            commentDto.CreatedAt = comment.CreatedAt?.ToLocalTime();
            commentDto.UpdatedAt = comment.UpdatedAt?.ToLocalTime();

            var user = await _context.Users.Where(x => x.Userid == comment.Userid).FirstOrDefaultAsync();
            commentDto.UserInfo.Id = user.Userid;
            commentDto.UserInfo.FirstName = user?.FirstName;
            commentDto.UserInfo.LastName = user?.LastName;
            commentDto.UserInfo.Avatar = user?.Avartar;

            if (comment.IsCorrect.Value)
            {
                var correctCmt = await _context.Correctcmts.FirstOrDefaultAsync(x => x.Commentid == commentId);
                commentDto.Correctcmt = correctCmt.CorrectText;
            }
            if (comment.IsImage.Value)
            {
                var imageCmts = await _context.Imagecmts.Where(x => x.Commentid == commentId).ToListAsync();
                commentDto.Imagecmts = imageCmts.Select(x => new FileOfComment { Type = "image", Url = x.Url}).ToList();
            }
            if (comment.IsAudio.Value)
            {
                var audioCmts = await _context.Audiocmts.Where(x => x.Commentid == commentId).ToListAsync();
                commentDto.Audiocmts = audioCmts.Select(x => new FileOfComment { Type = "audio", Url = x.Url }).ToList();
            }
            return commentDto;
        }
        public async Task<List<CommentDto>> GetComments(Guid userId, Guid postId, CancellationToken cancellationToken = default)
        {
            var comments = await _context.Comments.Where(x => x.Postid == postId && x.IsRemoved.Value == false).ToListAsync(cancellationToken);
            if (comments == null)
                return null;

            var commentDtos = new List<CommentDto>();
            foreach(var comment in comments)
            {
                var commentDto = await GetComment(comment.Commentid, cancellationToken);
                var interacts = await _context.Cmtinteracts.Where(x => x.Commentid == comment.Commentid).Select(x => x.Userid).ToListAsync();
                commentDto.IsUserInteracted = interacts.Any(x => x == userId);
                commentDtos.Add(commentDto);
            }
            return commentDtos;
        }

        public async Task<bool> IsBelongToPost(Guid postId, Guid commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId);
            return comment?.Postid == postId;
        }

        public async Task<bool> IsBelongToUser(Guid useId, Guid commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId);
            return comment?.Userid == useId;
        }

        public async Task UpdateComment(Guid commentId, CommentDto commentDto, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId);
            if (comment == null)
                throw new Exception("Not found comment");

            comment.Text = commentDto.Text;
            comment.IsCorrect = !string.IsNullOrWhiteSpace(commentDto.Correctcmt);
            comment.IsAudio = commentDto.Audiocmts.Count > 0 ? true : false;
            comment.IsImage = commentDto.Imagecmts.Count > 0 ? true : false;

            _context.Update(comment);
            //update reference table
            var oldaudioCmts = await _context.Audiocmts.Where(x => x.Commentid == commentId).ToListAsync();
            var oldimageCmts = await _context.Imagecmts.Where(x => x.Commentid == commentId).ToListAsync();
            var oldcorrectCmt = await _context.Correctcmts.Where(x => x.Commentid == commentId).ToListAsync();

            _context.Audiocmts.RemoveRange(oldaudioCmts);
            _context.Imagecmts.RemoveRange(oldimageCmts);
            _context.Correctcmts.RemoveRange(oldcorrectCmt);

            if (comment.IsAudio.Value)
            {
                var audioCmts = commentDto.Audiocmts.Select(x => new Audiocmt { Commentid = comment.Commentid, Url = x.Url }).ToList();
                await _context.Audiocmts.AddRangeAsync(audioCmts);
            }
            if (comment.IsImage.Value)
            {
                var imageCmts = commentDto.Imagecmts.Select(x => new Imagecmt { Commentid = comment.Commentid, Url = x.Url }).ToList();
                await _context.Imagecmts.AddRangeAsync(imageCmts);
            }
            if (comment.IsCorrect.Value)
            {
                var correctCmt = await _context.Correctcmts.FirstOrDefaultAsync(x => x.Commentid == commentId);
                if (correctCmt != null)
                {
                    correctCmt.CorrectText = commentDto.Correctcmt;
                    _context.Correctcmts.Update(correctCmt);
                }
                else
                {
                    await _context.Correctcmts.AddAsync(new Correctcmt { Commentid = comment.Commentid, CorrectText = commentDto.Correctcmt });
                }    
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteComment(Guid commentId, CancellationToken cancellationToken = default)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId);
            if (comment == null)
                throw new Exception("Not found comment");
            comment.IsRemoved = true;
            _context.Update(comment);
            _context.SaveChanges();
        }

        public async Task InteractComment(Guid commentId, Guid userId, string mode, CancellationToken cancellationToken = default)
        {
            await InitInteraction();
            var cmtInteract = await _context.Cmtinteracts.FirstOrDefaultAsync(x => x.Userid == userId && x.Commentid == commentId);

            if (cmtInteract == null && mode.Equals("UnLike"))
                return;
            if (mode.Equals("UnLike"))
            {
                _context.Cmtinteracts.Remove(cmtInteract);
                await _context.SaveChangesAsync();
                return;
            }

            var interactType = await _context.Interactions.ToListAsync();
            var interactTypeId = interactType.Where(x => x.Stringcode.Equals(mode)).FirstOrDefault()?.Interactid;
            if (cmtInteract == null)
                _context.Add(new Cmtinteract { Userid = userId, Commentid = commentId, InteractType = interactTypeId.Value });
            else
            {
                cmtInteract.InteractType = interactTypeId.HasValue ? interactTypeId.Value : cmtInteract.InteractType;
                _context.Cmtinteracts.Update(cmtInteract);
            }
            await _context.SaveChangesAsync();

            //publish event

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Userid == userId);
            var currentInteract = await _context.Cmtinteracts.Where(x => x.Commentid == commentId).CountAsync();
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Commentid == commentId);
            var notifyIds = new List<Guid>();
            notifyIds.Add(comment.Userid);

            var @event = new InteractCommentEvent
            {
                UserId = userId,
                UserName = $"{user.FirstName} {user.LastName}",
                CurrentInteract = currentInteract,
                PostId = comment.Postid,
                CommentId = comment.Commentid,
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

        public async Task<int> GetCmtInteract(Guid commentId, CancellationToken cancellationToken = default)
        {
            var numOfInteract = await _context.Cmtinteracts.Where(x => x.Commentid == commentId).CountAsync(cancellationToken);
            return numOfInteract;
        }
    }
}
