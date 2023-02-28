using System;

namespace LE.UserService
{
    public static class Env
    {
        public readonly static string DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        public const string AZNormalChars = "abcdefghijklmntuvwvz";
        public const string NumberChars = "1234567890";
        public const string SplitChar = "#";
    }
}
