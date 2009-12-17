using Microsoft.Win32;

namespace GoogleWaveNotifier.Browser
{
    public class PredefinedBrowserApplication : CommandLineBrowserApplication
    {
        public string RegistryLocation { get; set; }
        public bool IsInstalled
        {
            get { return GetCommandLine() != null; }
        }
        protected override string GetCommandLine()
        {
            return Registry.GetValue(RegistryLocation, null, null) as string;
        }
    }
}