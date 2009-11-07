using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
using ManagedHttp.Net.Client;
using Newtonsoft.Json.Linq;
using Timer=System.Timers.Timer;

namespace GoogleWaveNotifier
{
    public class GWavePoller : IDisposable
    {
        private GWaveSession _session;
        private object _pollLocker = new object();
        private Timer _pollTimer;
        private TimeSpan _pollTime = TimeSpan.FromSeconds(10);

        public TimeSpan PollTime
        {
            get { return _pollTime; }
            set
            {
                if (_pollTime != value)
                {
                    _pollTime = value;
                    if (_pollTimer != null)
                    {
                        _pollTimer.Stop();
                        _pollTimer.Interval = _pollTime.TotalMilliseconds;
                        _pollTimer.Start();
                    }
                }
            }
        }

        public Exception LastException { get; private set; }

        public List<Wave> UnreadWaves { get; private set; }

        public GWavePoller(GWaveSession session)
        {
            UnreadWaves = new List<Wave>();
            _pollTimer = new Timer(_pollTime.TotalMilliseconds);
            _pollTimer.AutoReset = false;
            _pollTimer.Elapsed += TimerElapsed;
            _session = session;
        }

        public void StartPolling()
        {
            lock (_pollLocker)
            {
                if (_pollTimer == null)
                    return;
                _pollTimer.Stop();
                _pollTimer.Start();
                PollNow();
            }
        }

        public void StopPolling()
        {
            lock (_pollLocker)
            {
                if (_pollTimer == null)
                    return;
                _pollTimer.Stop();
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            DoPoll();
        }

        public void PollNow()
        {
            ThreadPool.QueueUserWorkItem(delegate
                                         {
                                             DoPoll();
                                         });
        }

        private void DoPoll()
        {
            lock (_pollLocker)
            {
                if (_pollTimer == null)
                    return;
                _pollTimer.Stop();
                try
                {
                    UpdateWaves();
                    LastException = null;
                    OnPolled(EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    LastException = ex;
                    OnPollFailed(new ExceptionEventArgs(ex));
                }
                _pollTimer.Start();
            }
        }

        public void UpdateWaves()
        {
            ICollection<Wave> currentUnreadWaves = null;
            _session.Use(delegate(IHttpClient client, string authenticationToken) { currentUnreadWaves = GetUnreadWaves(client, authenticationToken); });

            if (currentUnreadWaves != null)
            {
                // Remove all old waves.
                UnreadWaves.RemoveAll(wave => !currentUnreadWaves.Contains(wave));

                // Select all new unread waves.
                var newWaves = (from wave in currentUnreadWaves where !UnreadWaves.Contains(wave) select wave).ToList();

                // Sync _unreadWaves with currentUnreadWaves by adding all newWaves.
                UnreadWaves.AddRange(newWaves);

                if (UnreadWaves.Count == 0)
                    Trace.WriteLine("No unread waves.", "Poller");
                else
                {
                    Trace.WriteLine(string.Format("{0} unread waves.", UnreadWaves.Count), "Poller");
                    if (newWaves.Count > 0)
                    {
                        foreach (Wave wave in newWaves)
                        {
                            OnWaveChanged(new WaveEventArgs(wave));
                        }
                    }
                }
            }
        }

        public static ICollection<Wave> GetUnreadWaves(IHttpClient webclient, string authenticationToken)
        {
            string url = "https://wave.google.com/wave/";
            if (authenticationToken != null)
                url += "?nouacheck&auth=" + Uri.EscapeDataString(authenticationToken);
            string responseText = webclient.DownloadString(new Uri(url));
            Match jsonMatch = Regex.Match(responseText, @"var json = (\{""r"":""\^d1"".*);");
            if (!jsonMatch.Success)
                throw new AuthenticationException("Incorrect page received.");

            string json = jsonMatch.Groups[1].Value;

            JObject jsonObj = JObject.Parse(json);
            JArray inbox = jsonObj["p"]["1"] as JArray;
            if (inbox == null)
                throw new AuthenticationException("Incorrect json received.");
            List<Wave> unreadwaves = new List<Wave>();
            foreach (var item in inbox.Children())
            {
                int unread = Convert.ToInt32((item["7"] as JValue).Value);

                if (unread != 0)
                {
                    Wave wave = new Wave
                                    {
                                        Id = Convert.ToString((item["1"] as JValue).Value),
                                        Title = Convert.ToString((item["9"]["1"] as JValue).Value),
                                        Summary = Convert.ToString((item["10"][0]["1"] as JValue).Value)
                                    };
                    unreadwaves.Add(wave);
                }
            }
            return unreadwaves;
        }

        public void Dispose()
        {
            lock (_pollLocker)
            {
                _pollTimer.Elapsed -= TimerElapsed;
                _pollTimer.Stop();
                _pollTimer.Dispose();
                _pollTimer = null;
            }
        }

        #region WaveChanged Event

        protected virtual void OnWaveChanged(WaveEventArgs e)
        {
            if (WaveChanged == null)
                return;
            WaveChanged(this, e);
        }

        public event EventHandler<WaveEventArgs> WaveChanged;

        #endregion

        #region Polled Event

        protected virtual void OnPolled(EventArgs e)
        {
            if (Polled == null)
                return;
            Polled(this, e);
        }

        public event EventHandler<EventArgs> Polled;

        #endregion

        #region PollFailed Event

        protected virtual void OnPollFailed(ExceptionEventArgs e)
        {
            if (PollFailed == null)
                return;
            PollFailed(this, e);
        }

        public event EventHandler<ExceptionEventArgs> PollFailed;

        #endregion
    }
}