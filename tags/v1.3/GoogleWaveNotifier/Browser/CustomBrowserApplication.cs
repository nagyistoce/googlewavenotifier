using System.Diagnostics;
using System.Text;

namespace GoogleWaveNotifier.Browser
{
    public class CustomBrowserApplication : CommandLineBrowserApplication
    {
        public string CommandLine { get; set; }
        protected override string GetCommandLine()
        {
            return CommandLine;
        }
    }
}