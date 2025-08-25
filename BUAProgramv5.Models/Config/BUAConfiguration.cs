using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.Config
{
    public class BUAConfiguration
    {
        public string MssqlConnection { get; set; }
        public string Office365User { get; set; }
        public SecureString Office365Password { get; set; }
        public string Domain { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string MailFrom { get; set; }
        public string MailFromAddress { get; set; }
        public string DeveloperOne { get; set; }
        public string SA { get; set; }
        public string Host { get; set; }
        public string EndUserOne { get; set; }
        public string MailServer { get; set; }
        public string MailGroup { get; set; }
        public string NetworkPath { get; set; }
        public string SecurityGroupOne { get; set; }
        public string SecurityGroupTwo { get; set; }
        public string Exchange2007 { get; set; }
        public string Exchange2010 { get; set; }
        public string Exchange2013 { get; set; }
        public string Exchange365 { get; set; }
        public string ExchangeClient { get; set; }
        public string AuthURL { get; set; }
        public string RootURL { get; set; }

    }
}
