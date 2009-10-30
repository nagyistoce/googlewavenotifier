using System;
using System.IO;
using System.Windows.Forms;
using GoogleWaveNotifier.Properties;
using Microsoft.Win32;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace GoogleWaveNotifier
{
    public partial class PreferencesForm : Form
    {
        private AutoStart _autoStart = new AutoStart("Google Wave Notifier");

        public PreferencesForm()
        {
            InitializeComponent();
            Icon = Resources.App;
            LoadPreferences();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            SavePreferences();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            SavePreferences();
        }

        protected void LoadPreferences()
        {
            logLink.Text = TraceLogger.LogFileName;
            Text = string.Format("{0} Settings", Program.Title);

            autoStartBox.Checked = _autoStart.IsEnabled;

            emailBox.Text = Settings.Default.Email;
            passwordBox.Text = Settings.Default.GetPassword();
            pollingIntervalBox.Value = (decimal)Settings.Default.PollInterval.TotalSeconds;
            enableLoggingBox.Checked = Settings.Default.EnableLog;

            LoadAbout();
        }

        private void LoadAbout()
        {
            titleLabel.Text = string.Format("{0} {1}", Program.Title, Program.Version);
            authorLabel.Text = string.Format("by {0}", Program.Author);
            descriptionLabel.Text = Program.Description;

            var b = new Bitmap(iconBox.Width, iconBox.Height);
            using (var g = Graphics.FromImage(b))
            {
                g.DrawIcon(Resources.App, new Rectangle(0, 0, b.Width, b.Height));
            }
            iconBox.Image = b;
        }

        protected void SavePreferences()
        {
            _autoStart.IsEnabled = autoStartBox.Checked;

            Trace.WriteLine("Saving preferences...", "Preferences");
            Settings.Default.Email = emailBox.Text;
            Settings.Default.SetPassword(passwordBox.Text);
            Settings.Default.PollInterval = TimeSpan.FromSeconds((double)pollingIntervalBox.Value);
            Settings.Default.EnableLog = enableLoggingBox.Checked;
            Settings.Default.Save();
            Trace.WriteLine("Preferences saved.", "Preferences");
        }

        private void WebsiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.Execute(Program.Website);
        }

        private void growlForWindowsLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.Execute("http://www.growlforwindows.com/");
        }


        private void logLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.Execute(TraceLogger.LogPath);
        }

        protected override void OnClosed(EventArgs e)
        {
            // Dispose the created bitmap for the about-icon.
            if (iconBox.Image != null)
            {
                var image = iconBox.Image;
                iconBox.Image = null;
                image.Dispose();
            }
            base.OnClosed(e);
        }
    }
}