using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CallEmailWebApiFromDotNetFramework4.Data;
using CallEmailWebApiFromDotNetFramework4.CallFrom4Dot8;

namespace BUAProgramv5.ServerFunctions.Mail
{
    public class Emailer : IEmailer
    {
        private ILogger _logger;
        private BUAConfiguration _emailees;
        private readonly BUAConfiguration _impersonation;
        public Emailer(ILogger logger)
        {
            _logger = logger;
            _emailees = Config.GetEmailInfo("Emailees");
            _impersonation = Config.GetAuthenticationInfo("Impersonation");
        }
        public MemoryStream memStream { get; set; }

        public void Dispose()
        {
            if (memStream != null)
            {
                memStream.Dispose();
                memStream = null;
            }
        }

        /// <summary>
        /// Send emails to users and groups.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="copyArchitecture"></param>
        public void SendEmail(string subject, string body, bool copyArchitecture = false)
        {
            string mySubject = subject;
            List<string> myBodyLineList = new List<string>();
            myBodyLineList.Add(body);

            List<string> myEmailAddressList = new List<string>();
            myEmailAddressList.Add("bstair@summithealthcare.com");
            myEmailAddressList.Add("nlwolf@summithealthcare.com");
            myEmailAddressList.Add("zbeason@summithealthcare.com");
            myEmailAddressList.Add("kvenson@summithealthcare.com");

            string myFromEmailAddress = "smgapplications@summithealthcare.com";
            string myEmailWebApiBaseUrl = "http://webservices:8081/api/EmailWebApi/SendEmailWithHtmlStringInput";

            List<string> myAttachmentList = new List<string>();

            CallFrom4Dot8Class
                myCallFrom4Dot8Class =
                    new CallFrom4Dot8Class
                    (
                        mySubject // string inputEemailSubject
                        , myBodyLineList // List<string> inputEmailBodyLineList
                        , myEmailAddressList // List<string> inputEmailAddressList
                        , myFromEmailAddress // string inputFromEmailAddress
                        , myEmailWebApiBaseUrl // string inputEmailWebApiBaseUrl
                        , myAttachmentList // List<string> inputAttachmentList
                    );

            EmailSendWithHtmlStringOutput
                myEmailSendWithHtmlStringOutput =
                    myCallFrom4Dot8Class
                    .CallIHtmlStringBody();

            
        }
    }
}
