using System;
using System.Windows.Forms;
using GoogleWaveNotifier.Browser;
using GoogleWaveNotifier.Properties;
using Microsoft.Win32;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
            Text = string.Format("{0} Preferences", Program.Title);

            autoStartBox.Checked = _autoStart.IsEnabled;

            emailBox.Text = Settings.Default.Email;
            passwordBox.Text = Settings.Default.GetPassword();
            pollingIntervalBox.Value = (decimal)Settings.Default.PollInterval.TotalSeconds;
            enableLoggingBox.Checked = Settings.Default.EnableLog;

            LoadAbout();
        }

        protected void SavePreferences()
        {
            _autoStart.IsEnabled = autoStartBox.Checked;

            Trace.WriteLine("Saving preferences...", "Preferences");
            Settings.Default.Email = emailBox.Text;
            Settings.Default.SetPassword(passwordBox.Text);
            Settings.Default.PollInterval = TimeSpan.FromSeconds((double)pollingIntervalBox.Value);
            Settings.Default.EnableLog = enableLoggingBox.Checked;
            if (browserBox.SelectedItem != null) // Only save if the browserBox loaded.
                Settings.Default.SetBrowser(browserBox.SelectedItem as IBrowserApplication);
            Settings.Default.Save();
            Trace.WriteLine("Preferences saved.", "Preferences");
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

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == browserTab)
                BrowserTabOpened();
        }

        #region Behaviour Tab
        private void logLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.Execute(TraceLogger.LogPath);
        }
        #endregion

        #region Browser Tab
        CustomBrowserApplication _customBrowser = new CustomBrowserApplication { Name = "Custom" };

        private void browserPathBrowseButton_Click(object sender, EventArgs e)
        {
            openBrowserDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            if (openBrowserDialog.ShowDialog() == DialogResult.OK)
                browserPathBox.Text = openBrowserDialog.FileName;
        }

        private void BrowserTabOpened()
        {
            // Load browsers only once.
            if (browserBox.Items.Count > 0)
                return;

            IBrowserApplication preferedBrowser = Settings.Default.GetBrowser();

            if (preferedBrowser is CustomBrowserApplication)
            {
                _customBrowser = (CustomBrowserApplication)preferedBrowser;
                browserPathBox.Text = _customBrowser.CommandLine;
            }

            browserBox.Items.Clear();
            browserBox.Items.Add(BrowserManager.DefaultBrowser);
            foreach (var browser in BrowserManager.InstalledBrowsers)
                browserBox.Items.Add(browser);
            browserBox.Items.Add(_customBrowser);

            browserBox.SelectedItem = preferedBrowser;
        }

        private void browserBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            browserPathBrowseButton.Enabled = browserPathBox.Enabled = browserBox.SelectedItem is CustomBrowserApplication;
        }

        private void browserPathBox_TextChanged(object sender, EventArgs e)
        {
            _customBrowser.CommandLine = browserPathBox.Text;
        }
        #endregion

        #region About Tab
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

        private void WebsiteLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.OpenBrowser(Program.Website);
        }

        private void growlForWindowsLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Utilities.OpenBrowser("http://www.growlforwindows.com/");
        }
        #endregion
    }
}