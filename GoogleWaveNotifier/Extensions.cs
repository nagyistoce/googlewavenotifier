using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleWaveNotifier.Properties;
using System.Security.Cryptography;
using System.IO;

namespace GoogleWaveNotifier
{
    internal static class Extensions
    {
        private static SymmetricAlgorithm GetCryptoAlgorithm()
        {
            var algorithm = Rijndael.Create();

            // Sigh, suggestions are welcome...
            var key = new byte[32];
            for (int i = 0; i < key.Length; i++)
                key[i] = (byte)i;
            algorithm.Key = key;

            var iv = new byte[16];
            for (int i = 0; i < iv.Length; i++)
                iv[i] = (byte)i;
            algorithm.IV = iv;

            return algorithm;
        }

        public static string GetPassword(this Settings settings)
        {
            string password = settings.Password;
            // The password is encrypted if it is in the following format:
            // <encrypted>myencryptedpassword</encrypted>
            // If it is not in this format, it is plain-text.

            if (!password.StartsWith("<encrypted>") || !password.EndsWith("</encrypted>"))
            {
                // The password is plain-text, just return it.
                return password;
            }
            
            // The password is encrypted, so encrypt it.
            string encryptedBase64 = password.Substring(11, password.Length - 11 - 12);
            byte[] encrypted = Convert.FromBase64String(encryptedBase64);
            var algorithm = GetCryptoAlgorithm();
            using (var decryptor = algorithm.CreateDecryptor())
            {
                byte[] decrypted = decryptor.TransformFinalBlock(encrypted, 0, encrypted.Length);
                password = Encoding.ASCII.GetString(decrypted);
            }
            return password;
        }

        public static void SetPassword(this Settings settings, string password)
        {
            // Only save encrypted passwords.
            var algorithm = GetCryptoAlgorithm();
            byte[] decrypted = Encoding.ASCII.GetBytes(password);
            using (var encryptor = algorithm.CreateEncryptor())
            {
                byte[] encrypted = encryptor.TransformFinalBlock(decrypted, 0, decrypted.Length);
                settings.Password = "<encrypted>" + Convert.ToBase64String(encrypted) + "</encrypted>";
            }
        }
    }
}
