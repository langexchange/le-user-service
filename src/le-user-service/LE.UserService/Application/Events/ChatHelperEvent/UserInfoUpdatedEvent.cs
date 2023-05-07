using LE.Library.Kernel;
using Newtonsoft.Json;

namespace LE.UserService.Application.Events.ChatHelperEvent
{
    public class UserInfoUpdatedEvent : BaseMessage
    {
        [JsonProperty("jid")]
        public string Jid { get; set; }

        [JsonProperty("fullname")]
        public string FullName { get; set; }

        [JsonProperty("nickname")]
        public string NickName { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }

        public UserInfoUpdatedEvent() : base(MessageValue.CHATHELPER_USER_INFO_UPDATED)
        {

        }
    }
}
