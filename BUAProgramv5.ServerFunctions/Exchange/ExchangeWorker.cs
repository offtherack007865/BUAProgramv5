using System;
using System.Collections.Generic;
using System.Linq;
using System.DirectoryServices;
using System.Text;
using System.Threading.Tasks;
using BUAProgramv5.Models.MailServer;
using System.DirectoryServices.ActiveDirectory;
using System.Collections;
using BUAProgramv5.Models.AD;

namespace BUAProgramv5.ServerFunctions.Exchange
{
    public class ExchangeWorker : IExchangeWorker
    {
        /// <summary>
        /// Uses LDAP to establish connection to exchange servers.
        /// </summary>
        /// <returns></returns>
        private SearchResultCollection ExchangeLDAPConnection()
        {
            string domain = Domain.GetCurrentDomain().ToString();
            DirectoryEntry rootDSE = new DirectoryEntry(string.Format("LDAP://{0}/rootDSE", domain));
            DirectoryEntry objectDirectoryEntry = new DirectoryEntry(string.Format("LDAP://{0}/{1}", domain, rootDSE.Properties["configurationNamingContext"].Value.ToString()));
            DirectorySearcher searcher = new DirectorySearcher(objectDirectoryEntry, "(&(objectClass=msExchExchangeServer))");
            SearchResultCollection collection = searcher.FindAll();

            return collection;
        }

        /// <summary>
        /// Uses LDAP query to get the exchange version from its object.
        /// </summary>
        /// <returns></returns>
        public OnPremiseExchange GetExchangeServerVersions()
        {
            SearchResultCollection collection = ExchangeLDAPConnection();
            OnPremiseExchange results = new OnPremiseExchange();
            List<string> maxRepeated = new List<string>();
            if (collection != null && collection.Count > 0)
            {
                foreach (SearchResult result in collection)
                {
                    DirectoryEntry user = result.GetDirectoryEntry();
                    maxRepeated.Add(user.Properties["serialNumber"].Value.ToString());
                }

                //Finds the total amount of exchange servers. Determines which type is most common, I.E. 2007,2010,2013.
                //This is done to find which version exists the most in the enviroment, when creating mailboxes.
                //If there is only one exchange server and it does not set when looking for multiples, pick the first one to set server version.
                results.ServerVersion = maxRepeated.GroupBy(s => s).OrderByDescending(s => s.Count()).First().Key;
                if (results.ServerVersion == null)
                {
                    results.ServerVersion = maxRepeated[0].ToString();
                }

                return results;
            }

            return results;
        }

        /// <summary>
        /// User LDAP to query infomration about the exchange server names.
        /// </summary>
        /// <param name="exchangeValue"></param>
        /// <returns></returns>
        public List<OnPremiseExchange> GetExchangeServerNames()
        {
            List<OnPremiseExchange> results = new List<OnPremiseExchange>();
            SearchResultCollection collection = ExchangeLDAPConnection();

            foreach (SearchResult result in collection)
            {
                OnPremiseExchange exchange = new OnPremiseExchange();
                DirectoryEntry serverName = result.GetDirectoryEntry();
                object ldapQuery = serverName.Properties["cn"].Value;
                exchange.ServerNames = ldapQuery.ToString();
                results.Add(exchange);
            }

            return results;
        }

        /// <summary>
        /// Gets all possible values for exchange options.
        /// </summary>
        /// <returns></returns>
        public OnPremiseExchange GetExchangeProperties()
        {
            OnPremiseExchange exchange = new OnPremiseExchange();
            SearchResultCollection collection = ExchangeLDAPConnection();

            foreach (SearchResult item in collection)
            {
                ICollection propertyList = item.Properties.PropertyNames;
                foreach (string value in propertyList)
                {
                    exchange.ExchangeProperties.Add(value);
                }
                break;
            }

            return exchange;
        }

        /// <summary>
        /// Determines which exchange options are needed to create mailbox.
        /// </summary>
        /// <param name="serverVersion"></param>
        /// <param name="newUser"></param>
        /// <param name="qualifier"></param>
        public void CreatePowershellMailbox(string serverVersion, User newUser, string qualifier)
        {
            if (serverVersion.Contains("Version 8"))
            {
                CreateMailbox2007(newUser, qualifier);
            }
            else if (serverVersion.Contains("Version 14"))
            {
                CreateMailBox2010(newUser, qualifier);
            }
            else if (serverVersion.Contains("Version 15"))
            {
                CreateMailBox2013(newUser, qualifier);
            }
        }

        /// <summary>
        /// Creates mailbox on 2013 exchange.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="qualifier"></param>
        private void CreateMailBox2013(User newUser, string qualifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates mailbox on 2010 exchange.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="qualifier"></param>
        private void CreateMailBox2010(User newUser, string qualifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates mailbox on 2007 exchange.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="qualifier"></param>
        private void CreateMailbox2007(User newUser, string qualifier)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines which exchange options are needed to disable mailbox.
        /// </summary>
        /// <param name="serverVersion"></param>
        /// <param name="userPrincipalName"></param>
        public void DisablePowershellMailbox(string serverVersion, string userPrincipalName)
        {
            if (serverVersion.Contains("Version 8"))
            {
                DisableMailbox2007(userPrincipalName);
            }
            else if (serverVersion.Contains("Version 14"))
            {
                DisableMailBox2010();
            }
            else if (serverVersion.Contains("Version 15"))
            {
                DisableMailbox2013();
            }
        }

        /// <summary>
        /// Disables mailbox on 2013 exchange.
        /// </summary>
        private void DisableMailbox2013()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disables mailbox on 2010 exchange.
        /// </summary>
        private void DisableMailBox2010()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Disables mailbox on 2007 exchange.
        /// </summary>
        /// <param name="userPrincipalName"></param>
        private void DisableMailbox2007(string userPrincipalName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes mail greater than 90 days on any exchange server.
        /// </summary>
        /// <param name="serverVersion"></param>
        /// <param name="userPrincipalName"></param>
        public void DeletePowershellMailbox(string serverVersion, string userPrincipalName)
        {
            DeletePowershellMailboxes(serverVersion, userPrincipalName);
        }

        /// <summary>
        /// Deletes mail on exchange.
        /// </summary>
        private void DeletePowershellMailboxes(string serverVersion, string userPrincipalName)
        {
            //Get-Mailbox -ResultSize Unlimited –RecipientTypeDetails UserMailbox,SharedMailbox | Where {(Get-MailboxStatistics $_.Identity).LastLogonTime -lt (Get-Date).AddDays(-90)} | Remove-Mailbox -whatif
        }
    }
}
