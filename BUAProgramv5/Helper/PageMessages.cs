using BUAProgramv5.Messaging;
using BUAProgramv5.Models.AD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Helper
{
    internal class PageMessages
    {
        /// <summary>
        /// Creates email message for ReEnable Account page.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        internal StringBuilder CreateEmailReEnableMessage(User user)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("User Account Re-enabled by " + Environment.UserName);
            sb.AppendLine("User account has been re-enabled.");
            sb.AppendLine("Password: " + user.UserPassword);
            sb.AppendLine("Mailbox will need to be reattached along with NAS folder.");

            return sb;
        }

        /// <summary>
        /// Creates email message for Move Esisting User Account page.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="oldSiteInfo"></param>
        /// <param name="newSiteInfo"></param>
        /// <returns></returns>
        internal StringBuilder CreateEmailSiteMessage(User user, SiteInformation oldSiteInfo, SiteInformation newSiteInfo)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("User Account Moved by " + Environment.UserName);
            sb.AppendLine("User Account Moved for : " + user.DisplayName);
            sb.AppendLine(string.Format("Moved from: {0}", oldSiteInfo.OU));
            sb.AppendLine(string.Format("Moved to: {0}", newSiteInfo.OU));

            return sb;
        }

        /// <summary>
        /// Creates body of email sent to IT Support.
        /// </summary>
        /// <param name="newUser"></param>
        /// <param name="result"></param>
        /// <param name="creationError"></param>
        /// <returns></returns>
        internal StringBuilder CreateEmailMessage(User newUser, bool result, int creationError)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("User Account created by " + Environment.UserName);
            sb.AppendLine("User Account created for " + newUser.DisplayName);
            sb.AppendLine("Username: " + newUser.Username);
            sb.AppendLine("Password: " + newUser.UserPassword);

            if (result == false)
            {
                if (string.IsNullOrEmpty(newUser.EmailAddress))
                {
                    sb.AppendLine("Email verified created for: " + newUser.Username + GeneralMessages.CompanyEmail);
                }
                else
                {
                    sb.AppendLine("Email verified created for: " + newUser.EmailAddress);
                }
            }
            else
            {
                //until off hybrid this is commented out since no email account is created.
                //sb.AppendLine("Email creation failed for: " + newUser.EmailAddress);
            }
            if (creationError > 0)
            {
                sb.AppendLine(ErrorMessages.PostAccountCreationError);
            }

            return sb;
        }
    }
}
