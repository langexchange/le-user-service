using LE.Library.Kernel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Application.Events
{
    public class InteractPostEvent : BaseMessage
    {
        [JsonProperty("postId")]
        public Guid PostId { get; set; }

        [JsonProperty("interactType")]
        public string InteractType { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("currentInteract")]
        public int CurrentInteract { get; set; }

        [JsonProperty("notifyIds")]
        public List<Guid> NotifyIds { get; set; }

        public InteractPostEvent(): base(MessageValue.INTERACTED_POST_EVENT)
        {
        }
    }
}
