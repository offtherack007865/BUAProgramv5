using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.ServerFunctions.Mail
{
    public interface IEmailer
    {
        MemoryStream memStream { get; set; }

        void Dispose();
        void SendEmail(string subject, string body, bool copyArchitecture = false);
    }
}
