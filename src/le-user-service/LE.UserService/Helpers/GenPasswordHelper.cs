using LE.UserService.Extensions;
using System;
using System.Net.Mail;

namespace LE.UserService.Helpers
{
    public class GenPasswordHelper
    {
        public static string GenerateRandomPassword()
        {
            return $"{Env.AZNormalChars.Shuffle(1)}{Env.NumberChars.Shuffle(5)}";
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
