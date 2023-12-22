using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Core.Utilities.Security.Link
{
    public class UniqueCodeGenerator
    {
        public static string GenerateUniqueLink()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            char[] link = new char[80];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[80];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < 50; i++)
                {
                    link[i] = chars[randomBytes[i] % chars.Length];
                }
            }

            return new string(link);
        }

        public static string GenerateUniqueCode(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] link = new char[length];

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                for (int i = 0; i < 8; i++)
                {
                    link[i] = chars[randomBytes[i] % chars.Length];
                }
            }

            return new string(link);
        }
    }
}
