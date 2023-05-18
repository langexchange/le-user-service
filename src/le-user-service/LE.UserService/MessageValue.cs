namespace LE.UserService
{
    public static class MessageValue
    {
        public const string INTERACTED_POST_EVENT = "interacted-post-event";
        public const string COMMENTED_POST_EVENT = "commented-post-event";
        public const string INTERACTED_COMMENT_EVENT = "interacted-comment-event";
        public const string POST_CREATED_EVENT = "post-created-event";
        public const string FRIEND_REQUEST_SENT_EVENT = "friend-request-sent-event";
        public const string FRIEND_REQUEST_ACCEPT_EVENT = "friend-request-accepted-event";
        public const string STATISTICAL_SIGNAL = "statistical-signal";
        public const string LEARNING_PROCESS_CALCULATED_EVENT = "learning-process-calculated-event";

        //chat-helper event
        public const string CHATHELPER_FRIEND_STATE_UPDATED = "chathelper-friendstate";
        public const string CHATHELPER_USER_INFO_UPDATED = "chathelper-userinfo";
    }
}
