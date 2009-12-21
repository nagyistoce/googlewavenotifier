using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleWaveNotifier.Browser;
using GoogleWaveNotifier.Properties;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace GoogleWaveNotifier
{
    internal static class Extensions
    {
        private static readonly byte[] EncryptionEntropy =
            { 7, 23, 14, 55, 156, 103, 250, 19, 4, 37, 45, 121, 10, 176, 204, 67 };

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
            
            // The password is encrypted, so decrypt it.
            string encryptedBase64 = password.Substring(11, password.Length - 11 - 12);
            byte[] encrypted = Convert.FromBase64String(encryptedBase64);
            try
            {
                password = Encoding.ASCII.GetString(ProtectedData.Unprotect(encrypted, EncryptionEntropy, DataProtectionScope.CurrentUser));
            }
            catch(CryptographicException)
            {
                return string.Empty;
            }

            return password;
        }

        public static void SetPassword(this Settings settings, string password)
        {
            // Only save encrypted passwords.
            byte[] decrypted = Encoding.ASCII.GetBytes(password);
            byte[] encrypted = ProtectedData.Protect(decrypted, EncryptionEntropy, DataProtectionScope.CurrentUser);
            settings.Password = "<encrypted>" + Convert.ToBase64String(encrypted) + "</encrypted>";
        }

        public static IBrowserApplication GetBrowser(this Settings settings)
        {
            string settingValue = settings.Browser;
            if (string.IsNullOrEmpty(settingValue))
                return BrowserManager.DefaultBrowser;
            var match = Regex.Match(settingValue, @"^\[(?<Name>.*)\]$");
            if (!match.Success)
                return new CustomBrowserApplication { Name = "Custom", CommandLine = settingValue };
            return BrowserManager.GetInstalledBrowser(match.Groups["Name"].Value) ?? BrowserManager.DefaultBrowser;
        }

        public static void SetBrowser(this Settings settings, IBrowserApplication browser)
        {
            browser = browser ?? BrowserManager.DefaultBrowser;
            var customBrowser = browser as CustomBrowserApplication;
            var installedBrowser = browser as InstalledBrowserApplication;
            if (customBrowser != null)
                settings.Browser = customBrowser.CommandLine;
            if (installedBrowser != null)
                settings.Browser = "[" + installedBrowser.Key + "]";
            else
                settings.Browser = "[" + browser.Name + "]";
        }
    }
}
