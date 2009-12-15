using System.IO;
using System.Text.RegularExpressions;

namespace GoogleWaveNotifier.Browser
{
    public abstract class CommandLineBrowserApplication : IBrowserApplication
    {
        public string Name { get; set; }
        protected abstract string GetCommandLine();
        public void Launch(string uri)
        {
            string commandLine = GetCommandLine();
            string filePath;
            string arguments;

            Match match;
            if ((match = Regex.Match(commandLine, "^\"(?<FilePath>[^\"]*)\"\\s*(?<Arguments>.*)$")).Success ||
                ((match = Regex.Match(commandLine, "^(?<FilePath>\\S+)\\s*(?<Arguments>.*)$")).Success))
            {
                filePath = match.Groups["FilePath"].Value;
                arguments = match.Groups["Arguments"].Value;
                if (arguments.Contains("%1"))
                    arguments = arguments.Replace("%1", uri);
                else
                    arguments += " " + uri;
            }
            else
            {
                filePath = commandLine;
                arguments = uri;
            }

            filePath = filePath.Replace("\"", "");

            Utilities.Execute(filePath, arguments);
        }
    }
}