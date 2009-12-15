using Microsoft.Win32;

namespace GoogleWaveNotifier.Browser
{
    public class PredefinedBrowserApplication : CommandLineBrowserApplication
    {
        public string Executable { get; set; }
        public bool IsInstalled
        {
            get { return GetCommandLine() != null; }
        }
        protected override string GetCommandLine()
        {
            var commandKey = Registry.ClassesRoot.OpenSubKey(@"Applications\" + Executable + @"\shell\open\command");
            if (commandKey != null)
            {
                using (commandKey)
                {
                    return commandKey.GetValue(null) as string;
                }
            }
            return null;
        }
    }
}