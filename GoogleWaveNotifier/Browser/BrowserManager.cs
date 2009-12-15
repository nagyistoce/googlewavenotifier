using System;
using System.Collections.Generic;
using System.Linq;

namespace GoogleWaveNotifier.Browser
{
    public static class BrowserManager
    {
        public static ICollection<PredefinedBrowserApplication> PredefinedBrowsers { get; private set; }
        private static PredefinedBrowserApplication[] _installedBrowsers;
        public static ICollection<PredefinedBrowserApplication> InstalledBrowsers
        {
            get
            {
                if (_installedBrowsers != null)
                    return _installedBrowsers;
                return _installedBrowsers = (from b in PredefinedBrowsers where b.IsInstalled select b).ToArray();
            }
        }
        public static IBrowserApplication DefaultBrowser { get; private set; }

        static BrowserManager()
        {
            DefaultBrowser = new DefaultBrowserApplication();
            PredefinedBrowsers = new[]
                                     {
                                         new PredefinedBrowserApplication { Name = "Firefox", Executable = "firefox.exe" },
                                         new PredefinedBrowserApplication { Name = "Google Chrome", Executable = "chrome.exe" },
                                         new PredefinedBrowserApplication { Name = "Opera", Executable = "Opera.exe" },
                                         new PredefinedBrowserApplication { Name = "Internet Explorer", Executable = "iexplore.exe" }
                                     };
        }

        public static PredefinedBrowserApplication GetPredefinedBrowser(string name)
        {
            return (from b in PredefinedBrowsers where b.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) select b).FirstOrDefault();
        }

        public static PredefinedBrowserApplication GetInstalledBrowser(string name)
        {
            PredefinedBrowserApplication predefined = GetPredefinedBrowser(name);
            if (predefined == null)
                return null;
            return predefined.IsInstalled ? predefined : null;
        }
    }
}