using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Notifications
{
    /// <summary>
    /// Account Information is returned on the UI screens when a property is changed.
    /// </summary>
    public class AccountInformation : INotifyPropertyChanged, IAccountInformation
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _siteInformation;
        private string _groupInformation;

        public AccountInformation()
        {
            this.SiteInformation = string.Empty;
            this.GroupInformation = string.Empty;
        }
        public string SiteInformation
        {
            get { return _siteInformation; }
            set
            {
                this._siteInformation = value;
                this.NotifyPropertyChanged("SiteInformation");
            }
        }

        public string GroupInformation
        {
            get { return _groupInformation; }
            set
            {
                _groupInformation = value;
                NotifyPropertyChanged("GroupInformation");
            }
        }

        private void NotifyPropertyChanged(String info)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}
