using BUAProgramv5.Models.Config;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Utilities
{
    public static class Config
    {
        /// <summary>
        /// Gets server related information based on configuration.
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetEmailInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                switch (item)
                {
                    case "SenderName":
                        results.MailFrom = collection["SenderName"];
                        break;
                    case "SenderEmail":
                        results.MailFromAddress = collection["SenderEmail"];
                        break;
                    case "Dev":
                        results.DeveloperOne = collection["Dev"];
                        break;
                    case "SA":
                        results.SA = collection["SA"];
                        break;
                    case "Host":
                        results.Host = collection["Host"];
                        break;
                    case "MailGroup":
                        results.MailGroup = collection["MailGroup"];
                        break;
                    default:
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Gets network information based on configuration
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetNetworkInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                switch (item)
                {
                    case "NetworkPath":
                        results.NetworkPath = collection["NetworkPath"];
                        break;
                    case "ITGroupOne":
                        results.SecurityGroupOne = collection["ITGroupOne"];
                        break;
                    case "ITGroupTwo":
                        results.SecurityGroupTwo = collection["ITGroupTwo"];
                        break;
                    default:
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Gets connection related information based on configuration.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetConnectionInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                if (item == "mssqlConnection")
                {
                    results.MssqlConnection = collection["mssqlConnection"];
                }
            }

            return results;
        }

        /// <summary>
        /// Gets Authentication related information based on configuration.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetAuthenticationInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                switch (item)
                {
                    case "365User":
                        results.Office365User = collection["365User"];
                        break;
                    case "365Password":
                        SecureString securedString = collection["365Password"].ToSecureString();
                        results.Office365Password = securedString;
                        break;
                    case "Password":
                        results.Password = collection["Password"];
                        break;
                    case "Domain":
                        results.Domain = collection["Domain"];
                        break;
                    case "User":
                        results.User = collection["User"];
                        break;
                    default:
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Gets exchange related information based on configuration.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetExchangeInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                switch (item)
                {
                    case "2007":
                        results.Exchange2007 = collection["2007"];
                        break;
                    case "2010":
                        results.Exchange2010 = collection["2010"];
                        break;
                    case "2013":
                        results.Exchange2013 = collection["2013"];
                        break;
                    default:
                        break;
                }
            }

            return results;
        }

        /// <summary>
        /// Gets Internal api related information based on configuration.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static BUAConfiguration GetAPIInfo(string path)
        {
            BUAConfiguration results = new BUAConfiguration();
            NameValueCollection collection = ConfigurationManager.GetSection(path) as NameValueCollection;

            foreach (string item in collection)
            {
                switch (item)
                {
                    case "AuthUrl":
                        results.AuthURL = collection["AuthUrl"];
                        break;
                    case "RootUrl":
                        results.RootURL = collection["RootUrl"];
                        break;
                    case "APIUser":
                        results.User = collection["APIUser"];
                        break;
                    case "APIPassword":
                        results.Password = collection["APIPassword"];
                        break;
                    default:
                        break;
                }
            }

            return results;
        }
    }
}
