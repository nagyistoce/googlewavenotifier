using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace GoogleWaveNotifier
{
    public class AutoStart
    {
        public string ApplicationName { get; private set; }
        public AutoStart(string applicationName)
        {
            ApplicationName = applicationName;
        }

        public bool IsEnabled
        {
            get
            {
                return GetIsEnabled();
            }
            set
            {
                if (value)
                    Enable();
                else
                    Disable();
            }
        }

        public bool GetIsEnabled()
        {
            return Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", ApplicationName, null) != null;
        }

        public void Enable()
        {
            string location = Assembly.GetEntryAssembly().Location;
            if (location == null)
                return;

            Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", ApplicationName, "\"" + location + "\"", RegistryValueKind.String);
        }

        public void Disable()
        {
            var runKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (runKey == null)
                return;
            using (runKey)
            {
                runKey.DeleteValue(ApplicationName, false);
            }
        }
    }
}
