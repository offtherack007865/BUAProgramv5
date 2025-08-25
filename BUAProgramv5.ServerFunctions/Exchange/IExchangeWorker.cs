using BUAProgramv5.Models.AD;
using BUAProgramv5.Models.MailServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.ServerFunctions.Exchange
{
    public interface IExchangeWorker
    {
        OnPremiseExchange GetExchangeServerVersions();
        List<OnPremiseExchange> GetExchangeServerNames();
        OnPremiseExchange GetExchangeProperties();
        void CreatePowershellMailbox(string serverVersion, User newUser, string qualifier);
        void DisablePowershellMailbox(string serverVersion, string userPrincipalName);
        void DeletePowershellMailbox(string serverVersion, string userPrincipalName);
    }
}
