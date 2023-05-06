using LE.Library.Kernel;
using Newtonsoft.Json;

namespace LE.UserService.Application.Events.ChatHelperEvent
{
    public class UserInfoUpdatedEvent : BaseMessage
    {
        public string Jid { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar { get; set; }

        public UserInfoUpdatedEvent() : base(MessageValue.CHATHELPER_USER_INFO_UPDATED)
        {

        }
    }
}
