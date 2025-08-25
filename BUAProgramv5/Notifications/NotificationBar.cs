using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Notifications
{
    /// <summary>
    /// Notification Bar is on the UI screens when displaying output to users of each action.
    /// </summary>
    public class NotificationBar : INotifyPropertyChanged, INotificationBar
    {
        private string _statusText;
        public string StatusText { get { return _statusText; } set { _statusText = value; this.NotifyPropertyChanged("StatusText"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
