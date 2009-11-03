using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using GoogleWaveNotifier.Properties;
using System.Configuration;

namespace GoogleWaveNotifier
{
    internal class Program
    {
        private static NotifyIcon _notifyIcon;
        private static GWaveSession _session;
        private static GWavePoller _poller;
        private static GrowlNotifier _growl;
        private static PreferencesForm _preferencesForm;
        private static object _pollLocker = new object();

        #region Information
        public static string Title
        {
            get
            {
                object[] attributes = typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                return null;
            }
        }

        public static Version Version
        {
            get { return typeof(Program).Assembly.GetName().Version; }
        }

        public static string Description
        {
            get
            {
                object[] attributes = typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                if (attributes.Length > 0)
                {
                    var descriptionAttribute = (AssemblyDescriptionAttribute)attributes[0];
                    if (descriptionAttribute.Description != "")
                        return descriptionAttribute.Description;
                }
                return "";
            }
        }

        public static string Author
        {
            get
            {
                object[] attributes = typeof(Program).Assembly.GetCustomAttributes(typeof(AssemblyAuthorAttribute), false);
                if (attributes.Length > 0)
                {
                    var authorAttribute = (AssemblyAuthorAttribute)attributes[0];
                    if (authorAttribute.Author != "")
                        return authorAttribute.Author;
                }
                return null;
            }
        }

        public static string Website
        {
            get { return "http://www.softwarebakery.com/frozencow/googlewavenotifier.html"; }
        }
        #endregion

        private static void Main(string[] args)
        {
            // Upgrade the configuration file if this is the first run (for this version).
            if (Settings.Default.FirstRun)
            {
                Settings.Default.Upgrade();
                Settings.Default.FirstRun = false;
                Settings.Default.Save();
            }

            // Load the settings.
            Settings.Default.SettingsSaving += SettingsSaving;

            Application.EnableVisualStyles();

            // Initialize notify icon.
            _notifyIcon = new NotifyIcon
                              {
                                  Text = Title,
                                  Visible = true
                              };
            _notifyIcon.MouseDoubleClick += NotifyIconDoubleClick;

            // Initialize notify-menu.
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip()
                                               {
                                                   ShowImageMargin = false
                                               };
            _notifyIcon.ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                new ToolStripMenuItem("Open Google Wave", null, OpenGoogleWaveClicked),
                new ToolStripMenuItem("Preferences...", null, PreferencesClicked),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Update Now", null, UpdateNowClicked),
                new ToolStripMenuItem("Show Me Again", null, ShowMeAgainClicked),
                new ToolStripSeparator(),
                new ToolStripMenuItem("Exit", null, ExitClicked)
            });

            // Initialize Growl (and register).
            _growl = new GrowlNotifier();
            _growl.Registered += GrowlRegistered;
            _growl.RegisteringFailed += GrowlRegisteringFailed;
            _growl.NotificationFailed += GrowlNotificationFailed;

            UpdateIcon();

            InitializeSettings();

            // Start application loop.
            Application.Run();

            // Application.Exit called.

            StopPolling();
            _notifyIcon.Dispose();
        }

        #region Growl
        private static void GrowlRegistered(object sender, EventArgs e)
        {
            Trace.WriteLine("Registered successfully", "Growl");
            UpdateIcon();
        }

        private static void GrowlRegisteringFailed(object sender, EventArgs e)
        {
            Trace.WriteLine("Registering failed", "Growl");
            UpdateIcon();
        }

        private static void GrowlNotificationFailed(object sender, NotificationEventArgs e)
        {
            // As a fallback, notification will be displayed using 'BalloonTips'...
            Trace.WriteLine(string.Format("Notification failed: Title: \"{0}\", Summary: \"{1}\"", e.Notification.Title, e.Notification.Text), "Growl");
            _notifyIcon.ShowBalloonTip(10000, e.Notification.Title, e.Notification.Text, ToolTipIcon.Info);
            UpdateIcon();
        }
        #endregion

        #region Settings
        private static void SettingsSaving(object sender, CancelEventArgs e)
        {
            InitializeSettings();
        }

        private static void InitializeSettings()
        {
            if (Settings.Default.EnableLog)
                TraceLogger.Enable();
            else
                TraceLogger.Disable();
            StartPolling();
        }
        #endregion

        #region Notify menu
        static void NotifyIconDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                OpenGoogleWaveClicked(sender, e);
        }

        private static void OpenGoogleWaveClicked(object sender, EventArgs e)
        {
            Utilities.Execute("https://wave.google.com/");
        }

        private static void PollerWaveChanged(object sender, WaveEventArgs e)
        {
            _growl.Notify(e.Wave);
        }

        private static void ShowMeAgainClicked(object sender, EventArgs e)
        {
            if (_poller != null)
            {
                foreach (var wave in _poller.UnreadWaves.ToArray())
                {
                    _growl.Notify(wave);
                }
            }
        }

        private static void UpdateNowClicked(object sender, EventArgs e)
        {
            lock (_pollLocker)
            {
                if (_poller != null)
                {
                    _poller.UnreadWaves.Clear();
                    _poller.PollNow();
                }
            }
        }

        private static void PreferencesClicked(object sender, EventArgs e)
        {
            if (_preferencesForm != null)
                _preferencesForm.Activate();
            else
            {
                using (_preferencesForm = new PreferencesForm())
                {
                    _preferencesForm.ShowDialog();
                }
                _preferencesForm = null;
            }
        }

        private static void ExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Poller
        private static void StartPolling()
        {
            lock (_pollLocker)
            {
                StopPolling();
                if (string.IsNullOrEmpty(Settings.Default.Email) || string.IsNullOrEmpty(Settings.Default.Password))
                {
                    UpdateIcon();
                    return;
                }
                _session = new GWaveSession
                               {
                                   Email = Settings.Default.Email,
                                   Password = Settings.Default.GetPassword()
                               };

                _poller = new GWavePoller(_session)
                              {
                                  PollTime = Settings.Default.PollInterval
                              };
                _poller.WaveChanged += PollerWaveChanged;
                _poller.Polled += PollerPolled;
                _poller.PollFailed += PollerFailed;
            }
            ThreadPool.QueueUserWorkItem(delegate
                                         {
                                             lock (_pollLocker)
                                             {
                                                 if (_poller != null)
                                                     _poller.PollNow();
                                             }
                                         });
        }

        private static void PollerFailed(object sender, ExceptionEventArgs e)
        {
            UpdateIcon();
            _growl.Notify(e.Exception);
            Trace.WriteLine(e.Exception.ToString(), "Poller");
        }

        private static void PollerPolled(object sender, EventArgs e)
        {
            UpdateIcon();
        }

        private static void StopPolling()
        {
            lock (_pollLocker)
            {
                if (_poller != null)
                {
                    _poller.WaveChanged -= PollerWaveChanged;
                    _poller.Polled -= PollerPolled;
                    _poller.PollFailed -= PollerFailed;
                    _poller.Dispose();
                    _poller = null;
                }
                _session = null;
            }
            UpdateIcon();
        }
        #endregion

        #region Notify icon
        private struct IconState
        {
            public bool WaveEnabled;
            public bool WaveError;
            public bool GrowlEnabled;
            public int UnreadWaves;
        }

        private static IconState oldIconStatus;

        public static void UpdateIcon()
        {
            if (_notifyIcon == null)
                return;
            lock (_notifyIcon)
            {
                IconState status;
                status.GrowlEnabled = _growl != null && _growl.IsRegistered;
                status.WaveEnabled = _session != null && _poller != null && _session.IsAuthenticated;
                status.WaveError = _poller != null && _poller.LastException != null;
                if (_poller != null)
                    status.UnreadWaves = _poller.UnreadWaves.Count;
                else
                    status.UnreadWaves = 0;

                Icon icon = null;
                if (_notifyIcon.Icon == null || !oldIconStatus.Equals(status))
                {
                    icon = CreateIcon(status);
                    oldIconStatus = status;

                    Icon oldIcon = _notifyIcon.Icon;
                    _notifyIcon.Icon = icon;
                    if (oldIcon != null)
                        oldIcon.Dispose();
                }
            }
        }

        private static Icon CreateIcon(IconState status)
        {
            Icon result;

            using (Bitmap bitmap = new Bitmap(16, 16))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.Transparent);
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    string gwaveIcon = status.WaveEnabled ? "notify-googlewave.png" : "notify-googlewave-disabled.png";
                    using (Bitmap overlay = new Bitmap(gwaveIcon))
                    {
                        g.DrawImageUnscaled(overlay, 0, 0);
                    }

                    if (status.GrowlEnabled)
                    {
                        using (Bitmap overlay = new Bitmap("notify-growl.png"))
                        {
                            g.DrawImageUnscaled(overlay, 0, 0);
                        }
                    }

                    if (status.UnreadWaves > 0)
                    {
                        using (Brush green = new SolidBrush(Color.FromArgb(0x00, 0x7F, 0x0E)))
                        {
                            g.FillEllipse(green, 7, 0, 8, 8);

                            string text = status.UnreadWaves.ToString();
                            int fontSize = text.Length == 1 ? 6 : 5;
                            using (Font arial = new Font("Arial", fontSize))
                            {
                                using (StringFormat format = new StringFormat
                                                                 {
                                                                     Alignment = StringAlignment.Center,
                                                                     LineAlignment = StringAlignment.Center
                                                                 })
                                {
                                    g.DrawString(text, arial, Brushes.White, 11, 5, format);
                                }
                            }
                        }
                    }
                    if (status.WaveError)
                    {
                        using (Brush red = new SolidBrush(Color.FromArgb(0xb8, 0x1b, 0x15)))
                        {
                            g.FillEllipse(red, 7, 0, 8, 8);
                            g.DrawLine(Pens.White, 7, 0, 15, 8);
                            g.DrawLine(Pens.White, 15, 0, 7, 8);
                        }
                    }
                }
                result = Icon.FromHandle(bitmap.GetHicon());
            }
            return result;
        }
        #endregion
    }
}