using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;

namespace GoogleWaveNotifier
{
    public class FileTraceListener: TraceListener
    {
        public string FilePath { get; private set; }

        public FileTraceListener(string filePath)
        {
            FilePath = filePath;
        }

        public override void Write(string message)
        {
            using (var writer = new StreamWriter(FilePath, true))
            {
                writer.Write(message);
                writer.Flush();
            }
        }

        public override void WriteLine(string message)
        {
            using (var writer = new StreamWriter(FilePath, true))
            {
                writer.Write(DateTime.Now);
                writer.Write(": ");
                for (int i = 0; i < IndentSize * IndentLevel; i++)
                    writer.Write(' ');
                writer.WriteLine(message);
                writer.Flush();
            }
        }
    }

    public static class TraceLogger
    {
        private static object _locker = new object();
        private static TraceListener _traceListener;
        public static string LogPath
        {
            get
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                return Path.Combine(Path.GetDirectoryName(config.FilePath), LogFileName);
            }
        }
        public static string LogFileName { get { return "GoogleWaveNotifier.log"; } }
        public static bool IsEnabled
        {
            get { return _traceListener != null; }
        }

        private static TraceListener CreateTraceListener()
        {
            return new FileTraceListener(LogPath);
            //return new TextWriterTraceListener(new StreamWriter(LogPath, true));
        }

        public static void Enable()
        {
            lock (_locker)
            {
                Disable();
                Trace.Listeners.Add(_traceListener = CreateTraceListener());
                Trace.WriteLine("Logging enabled.", "Log");
            }
        }

        public static void Disable()
        {
            lock (_locker)
            {
                if (_traceListener != null)
                {
                    TraceListener diposingListener = _traceListener;
                    _traceListener = null;
                    Trace.Listeners.Remove(diposingListener);
                    diposingListener.Dispose();
                }
            }
        }
    }
}
