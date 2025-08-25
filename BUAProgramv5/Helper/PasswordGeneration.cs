using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Helper
{
    internal static class PasswordGeneration
    {
        /// <summary>
        /// Random password generator that generates a random password from an accepted list of values based on mod values. 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        internal static string GenerateUniquePassword()
        {
            string password = string.Empty;
            int maxLength = 12;
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890?!".ToCharArray();
            byte[] data = new byte[1];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
                data = new byte[maxLength];
                rng.GetBytes(data);
            }

            StringBuilder result = new StringBuilder(maxLength);

            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            password = result.ToString();

            if (password.Contains("?") || password.Contains("!"))
            {
                return password;
            }
            else
            {
                password += "?!";

                return password;
            }
        }
    }
}
