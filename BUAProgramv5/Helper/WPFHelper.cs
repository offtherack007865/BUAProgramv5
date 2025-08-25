using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.ServerFunctions.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BUAProgramv5.Helper
{
    internal class WPFHelper
    {
        internal WPFHelper()
        {

        }

        /// <summary>
        /// Disable Account Page logic for setting user values from reflection via the User model.
        /// </summary>
        /// <param name="deactivatingAccount"></param>
        internal Task<string> DisablePageSetUserValues(User deactivatingAccount)
        {
            Task<string> userStatusTask = Task.Run(() =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("User Account Disabled by " + Environment.UserName);

                foreach (System.Reflection.PropertyInfo item in typeof(User).GetProperties())
                {
                    if (item.GetValue(deactivatingAccount, null) != null)
                    {
                        if (!item.GetValue(deactivatingAccount, null).GetType().IsGenericType && !item.GetValue(deactivatingAccount, null).GetType().IsGenericTypeDefinition)
                        {
                            if (item.Name != "UserPassword" && item.Name != "IsEnabled")
                            {
                                stringBuilder.AppendLine(string.Format("{0}: {1}", item.Name, item.GetValue(deactivatingAccount, null)));
                            }
                        }
                    }
                }

                stringBuilder.AppendLine(string.Format("{0}{1}GROUPS", Environment.NewLine, Environment.NewLine));

                foreach (ADPrincipalObject item in deactivatingAccount.Groups.OrderBy(x => x.Name))
                {
                    stringBuilder.AppendLine(item.Name);
                }

                string userStatus = stringBuilder.ToString().Replace(Environment.NewLine, "<br>");
                return userStatus;
            });
            Task.WhenAll(userStatusTask);
            return userStatusTask;
        }

        /// <summary>
        /// Disable Account Page logic disabling account.
        /// </summary>
        /// <param name="deactivatingAccount"></param>
        internal void DisablePageDisableAccount(User deactivatingAccount, ListBox listBox, IClientService service, IEmailer emailer, ILogger logger, string userStatus, INotificationBar notification, MainWindow mainWindow)
        {
            try
            {
                mainWindow.NotificationBar_Control.DataContext = notification;
                notification.StatusText = GeneralMessages.DisableUserAndGroups;
                mainWindow.StatusBar_TextBlock.Refresh();
                service.DisableUserandRemoveGroups(deactivatingAccount);

                try
                {
                    User currentUser = service.GetUserByDisplayName(listBox.SelectedItem.ToString());
                    if (currentUser.IsEnabled == true)
                    {
                        userStatus.Insert(0, GeneralMessages.EmailErrorNotification);
                        emailer.SendEmail(subject: GeneralMessages.AccountDisableFailure + deactivatingAccount.DisplayName, body: userStatus);
                    }
                    else
                    {
                        emailer.SendEmail(subject: GeneralMessages.AccountDisableSuccessful + deactivatingAccount.DisplayName, body: userStatus);
                    }

                    service.MoveUsersToDisabledOU(currentUser.DistinguishedName);
                    notification.StatusText = GeneralMessages.AccountDisableSuccessful + deactivatingAccount.DisplayName;
                }
                catch (Exception ex)
                {
                    logger.LogError(string.Format(ErrorMessages.EmailandGroupsError, ex.Message, ex.StackTrace));
                    MessageBox.Show(ErrorMessages.BlockedAVError, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format(ErrorMessages.UserGroupsError, ex.Message, ex.StackTrace));
                MessageBox.Show(ErrorMessages.UserGroupsError, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Builds AD group information for accounts.
        /// </summary>
        /// <param name="siteCombobox"></param>
        /// <param name="siteInfo"></param>
        /// <param name="userTypeCombobox"></param>
        /// <param name="userInfo"></param>
        /// <param name="groupMembership"></param>
        /// <param name="acctInfo"></param>
        internal void BuildGroupInformation(ComboBox siteCombobox, List<SiteInformation> siteInfo, ComboBox userTypeCombobox, List<UserInformation> userInfo, List<ADPrincipalObject> groupMembership, IAccountInformation acctInfo)
        {
            List<ADPrincipalObject> groups = GenerateGroupMembership(siteCombobox, siteInfo, userTypeCombobox, userInfo, groupMembership);
            StringBuilder sb = new StringBuilder();
            foreach (ADPrincipalObject group in groups.OrderBy(x => x.Name))
            {
                sb.AppendLine(group.Name);
            }

            acctInfo.GroupInformation = sb.ToString();
        }

        /// <summary>
        /// Generates group membership and related security groups.
        /// </summary>
        /// <param name="siteCombobox"></param>
        /// <param name="siteInfo"></param>
        /// <param name="_cctInfo"></param>
        /// <param name="departmentLabel"></param>
        /// <param name="departmentTextbox"></param>
        internal void DetermineSiteCombobox(ComboBox siteCombobox, List<SiteInformation> siteInfo, IAccountInformation _cctInfo, Label departmentLabel, TextBox departmentTextbox)
        {
            if (siteCombobox.SelectedItem != null)
            {
                SiteInformation selectedSiteInfo = siteInfo.Single(x => x.Name == siteCombobox.SelectedItem.ToString());

                StringBuilder sb = new StringBuilder();

                sb.AppendLine(selectedSiteInfo.Name);
                sb.AppendLine(selectedSiteInfo.Phone);
                sb.AppendLine(selectedSiteInfo.OU);

                _cctInfo.SiteInformation = sb.ToString();

                if (selectedSiteInfo.Name.Contains("099"))
                {
                    departmentLabel.Visibility = Visibility.Visible;
                    departmentTextbox.Visibility = Visibility.Visible;
                }
                else
                {
                    departmentLabel.Visibility = Visibility.Hidden;
                    departmentTextbox.Visibility = Visibility.Hidden;
                }
            }
        }

        /// <summary>
        /// Determine the user Listbox when user input changes on UI.
        /// </summary>
        /// <param name="userListbox"></param>
        /// <param name="userDetails"></param>
        /// <param name="activeDirectory"></param>
        /// <param name="reenableAccount"></param>
        internal void DetermineUserListbox(ListBox userListbox, TextBox userDetails, IClientService service, Button reenableAccount)
        {
            userDetails.Clear();

            if (userListbox.Items.Count != 0)
            {
                User userToReEnable = service.GetUserByDisplayName(userListbox.SelectedItem.ToString());
                foreach (System.Reflection.PropertyInfo item in typeof(User).GetProperties())
                {
                    if (item.GetValue(userToReEnable, null) != null)
                    {
                        if (!item.GetValue(userToReEnable, null).GetType().IsGenericType && !item.GetValue(userToReEnable, null).GetType().IsGenericTypeDefinition)
                        {
                            if (item.Name != "UserPassword")
                            {
                                userDetails.AppendText(string.Format("{0}: {1}{2}", item.Name, item.GetValue(userToReEnable, null), Environment.NewLine));
                            }
                        }
                    }
                }

                userDetails.AppendText(string.Format("{0}{1}GROUPS{2}", Environment.NewLine, Environment.NewLine, Environment.NewLine));

                foreach (ADPrincipalObject item in userToReEnable.Groups.OrderBy(x => x.Name))
                {
                    userDetails.AppendText(item.Name + Environment.NewLine);
                }
            }

            reenableAccount.IsEnabled = true;
        }

        /// <summary>
        /// Enables values based on UI changes.
        /// </summary>
        /// <param name="combobox"></param>
        /// <param name="displayLabel"></param>
        /// <param name="title"></param>
        internal void DetermineUserType(ComboBox combobox, Label displayLabel, TextBox title)
        {
            if (combobox.SelectedItem != null)
            {
                switch (combobox.SelectedItem.ToString())
                {
                    case "MidLevel":
                    case "Doctor":
                        displayLabel.Visibility = Visibility.Visible;
                        title.Visibility = Visibility.Visible;
                        break;
                    default:
                        displayLabel.Visibility = Visibility.Hidden;
                        title.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        /// <summary>
        /// Generates group membership and related security groups.
        /// </summary>
        /// <param name="siteCombobox"></param>
        /// <param name="siteInfo"></param>
        /// <param name="userTypeCombobox"></param>
        /// <param name="userInfo"></param>
        /// <param name="companyCombobox"></param>
        /// <param name="groupMembership"></param>
        /// <returns></returns>
        internal List<ADPrincipalObject> GenerateGroupMembership(ComboBox siteCombobox, List<SiteInformation> siteInfo, ComboBox userTypeCombobox, List<UserInformation> userInfo, List<ADPrincipalObject> groupMembership)
        {
            List<ADPrincipalObject> groups = new List<ADPrincipalObject>();

            if (siteCombobox.SelectedItem != null)
            {
                AddPrintersToGroups(siteCombobox, groups);
                switch (siteCombobox.SelectedItem.ToString())
                {
                    case "Floater Pool":
                    case "NonSummit Accounts":
                        return groups;
                    default:
                        foreach (ADPrincipalObject item in siteInfo.SingleOrDefault(x => x.Name == siteCombobox.SelectedItem.ToString()).SecurityGroups)
                        {
                            if (!string.IsNullOrEmpty(item.Name))
                            {
                                groups.Add(item);
                            }
                        }
                        break;
                }
            }
            if (userTypeCombobox.SelectedItem != null)
            {
                groups.AddRange(userInfo.SingleOrDefault(x => x.Name == userTypeCombobox.SelectedItem.ToString()).SecurityGroups);
            }
            if (groupMembership.Count > 0)
            {
                foreach (ADPrincipalObject group in groupMembership)
                {
                    if (!groups.Any(x => x.Name == group.Name))
                    {
                        groups.Add(group);
                    }
                }
            }
            return groups;
        }

        /// <summary>
        /// Adds printers to groups for specified Central office locations.
        /// </summary>
        /// <param name="siteCombobox"></param>
        /// <param name="groups"></param>
        private void AddPrintersToGroups(ComboBox siteCombobox, List<ADPrincipalObject> groups)
        {
            switch(siteCombobox.SelectedItem.ToString())
            {
                case "9970 - Accounting":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9975 - Accounts Receivable":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9980 - SSS Administration":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9981 - Credentialing":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9982 - Operations":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9983 - Quality":
                    groups.Add(new ADPrincipalObject { Name = "ACO Printers", DistinguishedName = "CN=ACO Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9984 - Recruiting":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9985 - Board":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9986 - Marketing & Communications":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9990 - Compliance":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9995 - Human Resources or TSOD":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "9999 - Information Systems":
                    groups.Add(new ADPrincipalObject { Name = "Central Printers", DistinguishedName = "CN=Central Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "400 - Summit Health Advantage":
                    groups.Add(new ADPrincipalObject { Name = "ACO Printers", DistinguishedName = "CN=ACO Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "6010 - SHS Admin":
                    groups.Add(new ADPrincipalObject { Name = "ACO Printers", DistinguishedName = "CN=ACO Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "6015 - Care Coordination":
                    groups.Add(new ADPrincipalObject { Name = "ACO Printers", DistinguishedName = "CN=ACO Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                case "6020 - Chart Review":
                    groups.Add(new ADPrincipalObject { Name = "ACO Printers", DistinguishedName = "CN=ACO Printers,OU=Security Groups,DC=ad,DC=sumg,DC=int" });
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Builds list of groups based on Active Directory Objects.
        /// </summary>
        internal StringBuilder BuildGroupInformation(List<ADPrincipalObject> groups)
        {
            StringBuilder sb = new StringBuilder();

            foreach (ADPrincipalObject group in groups.OrderBy(x => x.Name))
            {
                sb.AppendLine(group.Name);
            }

            return sb;
        }

        /// <summary>
        /// Generates user's display name in the program.
        /// </summary>
        /// <param name="titleTextbox"></param>
        /// <param name="middleInitialTextBox"></param>
        /// <param name="lastNameTextBox"></param>
        /// <param name="firstNameTextBox"></param>
        /// <returns></returns>
        internal string GenerateDisplayName(TextBox titleTextbox, TextBox middleInitialTextBox, TextBox lastNameTextBox, TextBox firstNameTextBox)
        {
            string result = string.Empty;

            if (titleTextbox.Visibility == Visibility.Visible)
            {
                result = (string.IsNullOrEmpty(middleInitialTextBox.Text) ? string.Format("{0}, {1}  {2}", lastNameTextBox.Text.Trim(), firstNameTextBox.Text.Trim(), titleTextbox.Text.Trim()) : string.Format("{0}, {1} {2}  {3}", lastNameTextBox.Text.Trim(), firstNameTextBox.Text.Trim(), middleInitialTextBox.Text.Trim(), titleTextbox.Text.Trim()));
            }
            else
            {
                result = (string.IsNullOrEmpty(middleInitialTextBox.Text) ? string.Format("{0}, {1}", lastNameTextBox.Text.Trim(), firstNameTextBox.Text.Trim()) : string.Format("{0}, {1} {2}", lastNameTextBox.Text.Trim(), firstNameTextBox.Text.Trim(), middleInitialTextBox.Text.Trim()));
            }

            return result;
        }
    }
}
