using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.MailServer
{
    public class OnPremiseExchange
    {
        public List<string> ExchangeProperties { get; set; } = new List<string>();
        public string ServerNames { get; set; } = string.Empty;
        public string ServerVersion { get; set; } = string.Empty;
    }
}
