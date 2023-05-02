using LE.Library.Kernel;

namespace LE.UserService.Application.Events.ChatHelperEvent
{
    public class FriendStateUpdatedEvent : BaseMessage
    {
        public string Jid1 { get; set; }
        public string Jid2 { get; set; }
        public string State { get; set; }

        public FriendStateUpdatedEvent() : base(MessageValue.CHATHELPER_FRIEND_STATE_UPDATED)
        {

        }
    }
}
