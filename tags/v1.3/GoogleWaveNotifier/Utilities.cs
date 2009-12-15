using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using GoogleWaveNotifier.Properties;

namespace GoogleWaveNotifier
{
    public static class Utilities
    {
        public static void OpenBrowser(string url)
        {
            Settings.Default.GetBrowser().Launch(url);
        }

        public static void Execute(string commandLine)
        {
            Execute(commandLine, null);
        }

        public static void Execute(string commandLine, string arguments)
        {
            Trace.WriteLine(string.Format("Executing '{0}'", commandLine), "Utility");
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    if (string.IsNullOrEmpty(arguments))
                    {
                        Process.Start(commandLine);
                    }
                    else
                    {
                        Process.Start(commandLine, arguments);
                    }
                }
                catch(Exception e)
                {
                    Trace.TraceError("Error executing commandline \"{0}\":", commandLine);
                    Trace.Indent();
                    Trace.TraceError(e.ToString());
                    Trace.Unindent();
                }
            });
        }
    }
}
