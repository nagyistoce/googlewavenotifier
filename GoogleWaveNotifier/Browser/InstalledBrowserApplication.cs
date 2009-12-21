using Microsoft.Win32;

namespace GoogleWaveNotifier.Browser
{
    public class InstalledBrowserApplication : CommandLineBrowserApplication
    {
        public InstalledBrowserApplication(string key)
        {
            Key = key;
            Name = Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Clients\StartMenuInternet\" + key, null, null) as string ?? "";
        }

        public string Key { get; set; }

        public bool HasCommandLine
        {
            get { return GetCommandLine() != null; }
        }
        protected override string GetCommandLine()
        {
            return Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Clients\StartMenuInternet\" + Key + @"\shell\open\command", null, null) as string;
        }
    }
}