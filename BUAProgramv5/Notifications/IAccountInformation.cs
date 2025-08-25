using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Notifications
{
    public interface IAccountInformation
    {
        string GroupInformation { get; set; }
        string SiteInformation { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}
