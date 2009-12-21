using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace GoogleWaveNotifier.Browser
{
    public static class BrowserManager
    {
        private static InstalledBrowserApplication[] _installedBrowsers;
        public static ICollection<InstalledBrowserApplication> InstalledBrowsers
        {
            get
            {
                if (_installedBrowsers != null)
                    return _installedBrowsers;
                return _installedBrowsers = GetInstalledBrowsers();
            }
        }

        public static IBrowserApplication DefaultBrowser { get; private set; }

        static BrowserManager()
        {
            DefaultBrowser = new DefaultBrowserApplication();
        }

        private static InstalledBrowserApplication[] GetInstalledBrowsers()
        {
            var browsers = new List<InstalledBrowserApplication>();
            using (var browsersKey = Registry.LocalMachine.OpenSubKey(@"Software\Clients\StartMenuInternet"))
            {
                if (browsersKey != null)
                {
                    foreach (var browser in browsersKey.GetSubKeyNames())
                    {
                        var installedBrowser = new InstalledBrowserApplication(browser);
                        if (installedBrowser.HasCommandLine)
                            browsers.Add(new InstalledBrowserApplication(browser));
                    }
                }
            }
            return browsers.ToArray();
        }

        public static InstalledBrowserApplication GetInstalledBrowser(string name)
        {
            InstalledBrowserApplication installedBrowser = new InstalledBrowserApplication(name);
            return installedBrowser.HasCommandLine ? installedBrowser : null;
        }
    }
}