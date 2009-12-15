using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Growl.Connector;

namespace GoogleWaveNotifier
{
    public class NotificationEventArgs: EventArgs
    {
        public Notification Notification { get; set; }
        public CallbackContext Callback { get; set; }
        public NotificationEventArgs(Notification notification, CallbackContext callback)
        {
            Notification = notification;
            Callback = callback;
        }
    }
}
