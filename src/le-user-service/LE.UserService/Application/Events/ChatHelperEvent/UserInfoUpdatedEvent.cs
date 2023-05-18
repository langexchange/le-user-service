using LE.Library.Kernel;
using Newtonsoft.Json;
using System.Collections.Generic;

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

        [JsonProperty("is_created")]
        public bool IsCreated { get; set; } = false;

        [JsonProperty("target_langs")]
        public List<string> TargetLanguage { get; set; }

        [JsonProperty("native_lang")]
        public string NativeLanguage { get; set; }

        

        public UserInfoUpdatedEvent() : base(MessageValue.CHATHELPER_USER_INFO_UPDATED)
        {
            TargetLanguage = new List<string>();
        }
    }
}
