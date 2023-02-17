using System.Linq;
using System.Security.Cryptography;

namespace LE.UserService.Extensions
{
    public static class Stringextension
    {
        public static string Shuffle(this string str, int? take = null)
        {
            var array = str.ToCharArray();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = RandomNumberGenerator.GetInt32(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            if (take.HasValue)
                array = array.Take(take.Value).ToArray();

            return new string(array);
        }
    }
}
