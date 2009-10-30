using System;
using System.IO;
using Growl.Connector;
using Growl.CoreLibrary;
using System.Threading;
using GoogleWaveNotifier.Properties;
using System.Diagnostics;

namespace GoogleWaveNotifier
{
    public class GrowlNotifier
    {
        private GrowlConnector _connector;
        public bool IsRegistered { get; private set; }
        private object _responseLocker = new object();
        private Response _lastResponse = null;

        private Resource _googlewaveicon;
        private Resource _googlewavenotifiericon;

        public GrowlNotifier()
        {
            if (File.Exists("googlewave.png"))
                _googlewaveicon = new BinaryData(File.ReadAllBytes("googlewave.png"));
            if (File.Exists("googlewavenotifier.png"))
                _googlewavenotifiericon = new BinaryData(File.ReadAllBytes("googlewavenotifier.png"));
            _connector = new GrowlConnector();
            _connector.ErrorResponse += ConnectorErrorResponse;
            _connector.OKResponse += ConnectorOkResponse;
            Register();
        }

        private void EnsureRegistered()
        {
            if (!IsRegistered)
                Register();
        }

        private void Register()
        {
            lock (_responseLocker)
            {
                _connector.Register(new Application("Google Wave Notifier")
                                        {
                                            Icon = _googlewavenotifiericon
                                        }, new[]
                                               {
                                                   new NotificationType("unreadwave", "Unread wave", _googlewaveicon, true),
                                                   new NotificationType("error", "Error", _googlewaveicon, true),
                                               });
                _lastResponse = null;
                Monitor.Wait(_responseLocker, 1000);
                if (_lastResponse != null && _lastResponse.IsOK)
                {
                    IsRegistered = true;
                    OnRegistered(EventArgs.Empty);
                }
                else
                {
                    IsRegistered = false;
                    OnRegisteringFailed(EventArgs.Empty);
                }
            }
        }

        private void ConnectorOkResponse(Response response)
        {
            Trace.WriteLine("Growl responded OK", "Growl");
            lock (_responseLocker)
            {
                _lastResponse = response;
                IsRegistered = true;
                Monitor.PulseAll(_responseLocker);
            }
        }

        private void ConnectorErrorResponse(Response response)
        {
            Trace.WriteLine(string.Format("Growl responded with an error: {0} {1}", response.ErrorCode, response.ErrorDescription), "Growl");
            lock (_responseLocker)
            {
                _lastResponse = response;
                IsRegistered = false;
                Monitor.PulseAll(_responseLocker);
            }
        }

        public void Notify(Wave wave)
        {
            var context = new CallbackContext("https://wave.google.com/wave/#restored:wave:" + Uri.EscapeDataString(wave.Id));
            string title = string.IsNullOrEmpty(wave.Title) ? "-" : wave.Title;
            string description = string.IsNullOrEmpty(wave.Summary) ? "-" : wave.Summary;
            Notify(new Notification("Google Wave Notifier", "unreadwave", _googlewaveicon, title, description), context);
        }

        public void Notify(Exception exception)
        {
            Notify(new Notification("Google Wave Notifier", "unreadwave", _googlewaveicon, "An error occured", exception.Message));
        }

        public void Notify(Notification notification)
        {
            Notify(notification, null);
        }

        public void Notify(Notification notification, CallbackContext callback)
        {
            EnsureRegistered();
            if (!IsRegistered)
                OnNotificationFailed(new NotificationEventArgs(notification, callback));
            lock (_responseLocker)
            {
                _connector.Notify(notification, callback);
                _lastResponse = null;
                Monitor.Wait(_responseLocker, 1000);
                if (_lastResponse == null || !_lastResponse.IsOK)
                    OnNotificationFailed(new NotificationEventArgs(notification, callback));
            }
        }

        #region Registered Event
        protected virtual void OnRegistered(EventArgs e)
        {
            if (Registered == null)
                return;
            Registered(this, e);
        }
        public event EventHandler<EventArgs> Registered;
        #endregion
        #region RegisteringFailed Event
        protected virtual void OnRegisteringFailed(EventArgs e)
        {
            if (RegisteringFailed == null)
                return;
            RegisteringFailed(this, e);
        }
        public event EventHandler<EventArgs> RegisteringFailed;
        #endregion
        #region NotificationFailed Event
        protected virtual void OnNotificationFailed(NotificationEventArgs e)
        {
            if (NotificationFailed == null)
                return;
            NotificationFailed(this, e);
        }
        public event EventHandler<NotificationEventArgs> NotificationFailed;
        #endregion
	
    }
}