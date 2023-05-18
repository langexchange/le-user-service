using LE.Library.Kernel;
using Newtonsoft.Json;

namespace LE.UserService.Application.Events.ChatHelperEvent
{
    public class FriendStateUpdatedEvent : BaseMessage
    {
        [JsonProperty("jid1")]
        public string Jid1 { get; set; }

        [JsonProperty("jid2")]
        public string Jid2 { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        public FriendStateUpdatedEvent() : base(MessageValue.CHATHELPER_FRIEND_STATE_UPDATED)
        {

        }
    }
}
