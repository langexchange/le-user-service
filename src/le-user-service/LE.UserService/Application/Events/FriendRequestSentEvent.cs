using LE.Library.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LE.UserService.Application.Events
{
    public class FriendRequestSentEvent : BaseMessage
    {
        [JsonProperty("fromId")]
        public Guid FromId { get; set; }

        [JsonProperty("toId")]
        public Guid ToId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("notifyIds")]
        public List<Guid> NotifyIds { get; set; }

        public FriendRequestSentEvent() : base(MessageValue.FRIEND_REQUEST_SENT_EVENT)
        {

        }
    }
}
