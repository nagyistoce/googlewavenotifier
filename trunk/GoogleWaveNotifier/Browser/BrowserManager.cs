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
                                         new PredefinedBrowserApplication { Name = "Firefox", RegistryLocation = @"HKEY_CLASSES_ROOT\FirefoxURL\shell\open\command" },
                                         new PredefinedBrowserApplication { Name = "Google Chrome", RegistryLocation = @"HKEY_CLASSES_ROOT\ChromeHTML\shell\open\command" },
                                         new PredefinedBrowserApplication { Name = "Opera", RegistryLocation = @"HKEY_CLASSES_ROOT\Opera.HTML\shell\open\command" },
                                         new PredefinedBrowserApplication { Name = "Internet Explorer", RegistryLocation = @"HKEY_CLASSES_ROOT\htmlfile\shell\open\command" },
                                         new PredefinedBrowserApplication { Name = "Safari", RegistryLocation = @"HKEY_CLASSES_ROOT\SafariURL\shell\open\command" }
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