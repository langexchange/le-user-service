using LE.UserService.Extensions;

namespace LE.UserService.Helpers
{
    public class GenPasswordHelper
    {
        public static string GenerateRandomPassword()
        {
            return $"{Env.AZNormalChars.Shuffle(1)}{Env.NumberChars.Shuffle(5)}";
        }
    }
}
