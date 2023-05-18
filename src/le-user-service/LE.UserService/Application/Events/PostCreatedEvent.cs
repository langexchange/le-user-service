using LE.Library.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LE.UserService.Application.Events
{
    public class PostCreatedEvent : BaseMessage
    {
        [JsonProperty("postId")]
        public Guid PostId { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("notifyIds")]
        public List<Guid> NotifyIds { get; set; }

        public PostCreatedEvent() : base(MessageValue.POST_CREATED_EVENT)
        {
        }
    }
}
