using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GoogleWaveNotifier
{
    public static class Utilities
    {
        public static void Execute(string commandLine)
        {
            Trace.WriteLine(string.Format("Executing '{0}'", commandLine), "Utility");
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    Process.Start(commandLine);
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
