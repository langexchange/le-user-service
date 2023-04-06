using LE.Library.Kernel;
using Newtonsoft.Json;
using System;

namespace LE.UserService.Application.Events
{
    public class InteractPostEvent : BaseMessage
    {
        [JsonProperty("postId")]
        public Guid PostId { get; set; }

        [JsonProperty("streamID")]
        public Guid UserId { get; set; }

        [JsonProperty("interactType")]
        public string InteractType { get; set; }
        public InteractPostEvent(): base(MessageValue.INTERACTED_POST_EVENT)
        {
        }
    }
}
