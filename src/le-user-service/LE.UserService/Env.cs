using System;

namespace LE.UserService
{
    public static class Env
    {
        public readonly static string DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public readonly static string SECRET_KEY = Environment.GetEnvironmentVariable("SECRET_KEY") ?? string.Empty;
        public const string AZNormalChars = "abcdefghijklmntuvwvz";
        public const string NumberChars = "1234567890";
        public const string SplitChar = "#";
        public const string XUserId = "x-user-id";
        public const string SendRequest = "send-request";
        public const string Follow = "follow";
    }
}
