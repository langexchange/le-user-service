using LE.Library.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LE.UserService.Application.Events
{
    public class InteractCommentEvent : BaseMessage
    {
        [JsonProperty("postId")]
        public Guid PostId { get; set; }

        [JsonProperty("commentId")]
        public Guid CommentId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("currentInteract")]
        public int CurrentInteract { get; set; }

        [JsonProperty("notifyIds")]
        public List<Guid> NotifyIds { get; set; }

        public InteractCommentEvent() : base(MessageValue.INTERACTED_COMMENT_EVENT)
        {

        }
    }
}
