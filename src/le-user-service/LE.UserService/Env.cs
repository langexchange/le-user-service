using System;

namespace LE.UserService
{
    public static class Env
    {
        public readonly static string DB_CONNECTION_STRING = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
    }
}
