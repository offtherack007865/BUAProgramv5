using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BUAProgramv5.Helper
{
    internal class DataValidation
    {
        /// <summary>
        /// Validates the textboxes on the ReEnable form.
        /// </summary>
        /// <param name="listBox"></param>
        /// <param name="user"></param>
        /// <param name="jobTitle"></param>
        /// <param name="manager"></param>
        /// <param name="site"></param>
        /// <param name="userInfo"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        internal bool ValidateReEnableForm(ListBox listBox, TextBox user, TextBox jobTitle, ComboBox manager, ComboBox site, ComboBox userInfo)
        {
            ValidateTextBox(user);
            ValidateTextBox(jobTitle);
            ValidateComboBox(manager);
            ValidateComboBox(site);
            ValidateComboBox(userInfo);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Please verify that the following fields have been filled out:");
            sb.AppendLine();

            if (string.IsNullOrEmpty(listBox.SelectedItem.ToString()))
            {
                sb.AppendLine("Select a user");
            }
            if (string.IsNullOrEmpty(jobTitle.Text.ToString()))
            {
                sb.AppendLine("Job Title/Description");
            }
            if (string.IsNullOrEmpty(manager.SelectedItem.ToString()))
            {
                sb.AppendLine("Manager");
            }
            if (string.IsNullOrEmpty(site.SelectedItem.ToString()))
            {
                sb.AppendLine("Site");
            }
            if (string.IsNullOrEmpty(userInfo.SelectedItem.ToString()))
            {
                sb.AppendLine("User type");
            }

            if (sb.ToString().Length > 65)
            {
                MessageBox.Show(sb.ToString(), "Validation", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates textbox to determine if it is null or empty.
        /// </summary>
        /// <param name="textBox"></param>
        internal void ValidateTextBox(TextBox textBox)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.BorderBrush = Brushes.Red;
                textBox.BorderThickness = new Thickness(2);
            }
            else
            {
                textBox.BorderBrush = Brushes.Gray;
                textBox.BorderThickness = new Thickness(1);
            }
        }

        /// <summary>
        /// Validates Combobox to determine if contains a null value.
        /// </summary>
        /// <param name="comboBox"></param>
        internal void ValidateComboBox(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null)
            {
                comboBox.Foreground = Brushes.Red;
                comboBox.BorderBrush = Brushes.Red;
                comboBox.BorderThickness = new Thickness(2);
            }
            else
            {
                comboBox.BorderBrush = Brushes.Gray;
                comboBox.BorderThickness = new Thickness(1);
            }
        }

        /// <summary>
        /// Validates data to determine if button is enabled.
        /// </summary>
        /// <param name="site"></param>
        /// <param name="user"></param>
        /// <param name="manager"></param>
        /// <param name="description"></param>
        /// <param name="button"></param>
        internal void ValidateMoveUserForm(ComboBox site, ListBox user, ComboBox manager, TextBox description, Button button)
        {
            if (site.SelectedItem != null && user.SelectedItem != null && manager.SelectedItem != null && !string.IsNullOrEmpty(description.Text))
            {
                button.IsEnabled = true;
            }
            else
            {
                button.IsEnabled = false;
            }
        }

        /// <summary>
        /// Validates first name, middle initial and last name are not null or empty.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleInit"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        internal string ValidateGeneratedUserName(string firstName, string middleInit, string lastName)
        {
            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrEmpty(firstName.ToString()))
            {
                sb.Append(firstName.ToString().Substring(0, 1).Replace(" ", ""));
            }
            if (!string.IsNullOrEmpty(middleInit.ToString()))
            {
                sb.Append(middleInit.ToString().Substring(0, 1).Replace(" ", ""));
            }
            if (!string.IsNullOrEmpty(lastName.ToString()))
            {
                sb.Append(lastName.ToString().Replace(" ", ""));
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Validates Username.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userAvailability"></param>
        /// <param name="activeDirectory"></param>
        /// <returns></returns>
        internal bool ValidateUserName(string username, Label userAvailability, IClientService service)
        {
            if (!string.IsNullOrEmpty(username) || service == null)
            {
                bool available = service.IsUserNameAvailable(username.Trim().Replace(" ", ""));
                if (available)
                {
                    userAvailability.Visibility = Visibility.Visible;
                    userAvailability.Content = "Username Available";
                    userAvailability.Foreground = Brushes.Green;
                    return true;
                }
                else
                {
                    userAvailability.Visibility = Visibility.Visible;
                    userAvailability.Content = "Username Already Exists!";
                    userAvailability.Foreground = Brushes.Red;
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Validates each security group to verify it exists.
        /// </summary>
        /// <param name="securityGroup"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        internal ObservableCollection<ADObjectCheckList> ValidateSecurityGroupList(ObservableCollection<ADObjectCheckList> list, IAccountInformation acctInfo)
        {
            if (!string.IsNullOrEmpty(acctInfo.GroupInformation))
            {
                string[] groups = acctInfo.GroupInformation.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string group in groups)
                {
                    if (!string.IsNullOrEmpty(group))
                    {
                        ADObjectCheckList adoGroup = list.SingleOrDefault(x => x.Name == group);

                        if (adoGroup != null)
                        {
                            adoGroup.Checked = true;
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Validates Create user account data.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="manager"></param>
        /// <param name="site"></param>
        /// <param name="department"></param>
        /// <param name="userType"></param>
        /// <param name="title"></param>
        /// <param name="userAvailability"></param>
        /// <param name="activeDirectory"></param>
        /// <returns></returns>
        internal bool ValidateForm(TextBox firstName, TextBox lastName, TextBox userName, ComboBox manager, ComboBox site, TextBox department, ComboBox userType, ComboBox title, Label userAvailability, IClientService service)
        {
            ValidateTextBox(firstName);
            ValidateTextBox(lastName);
            ValidateTextBox(userName);
            ValidateComboBox(manager);
            ValidateComboBox(site);
            ValidateComboBox(userType);

            StringBuilder sb = ValidateCreateAccountForm(firstName, lastName, userName, manager, site, department, userType, title);
            sb.AppendLine();

            if (!ValidateUserName(userName.Text.Trim(), userAvailability, service))
            {
                sb.AppendLine("Username");
            }
            if (sb.Length > 65)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Prompts to tell end user if the forgot to fill out a box.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="userName"></param>
        /// <param name="manager"></param>
        /// <param name="site"></param>
        /// <param name="department"></param>
        /// <param name="userType"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private StringBuilder ValidateCreateAccountForm(TextBox firstName, TextBox lastName, TextBox userName, ComboBox manager, ComboBox site, TextBox department, ComboBox userType, ComboBox title)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Please verify that the following fields have been filled out:");
            sb.AppendLine();

            if (string.IsNullOrEmpty(firstName.Text.Trim()))
            {
                sb.AppendLine("First Name");
            }
            if (string.IsNullOrEmpty(lastName.Text.Trim()))
            {
                sb.AppendLine("Last Name");
            }
            if (string.IsNullOrEmpty(userName.Text.Trim()))
            {
                sb.AppendLine("Username");
            }
            if (manager.SelectedItem == null)
            {
                sb.AppendLine("Manager");
            }
            if (site.SelectedItem == null)
            {
                sb.AppendLine("Site");
            }
            if (department.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(department.Text))
                {
                    sb.AppendLine("Department");
                }
                ValidateTextBox(department);
            }
            if (userType.SelectedItem == null)
            {
                sb.AppendLine("User Type");
            }
            else if (userType.SelectedItem.ToString() == "MidLevel" || userType.SelectedItem.ToString() == "Doctor")
            {
                if (title.SelectedItem == null)
                {
                    sb.AppendLine("Title");
                }
            }

            return sb;
        }

        /// <summary>
        /// Checks combo values for specific account info.
        /// </summary>
        /// <param name="comboBox"></param>
        /// <returns></returns>
        internal bool ValidateNASComboBox(ComboBox comboBox)
        {
            if (comboBox.SelectedItem.ToString() != "Floater Pool" && comboBox.SelectedItem.ToString() != "NonSummit Accounts")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Validates each security group to verify it exists.
        /// </summary>
        /// <param name="securityGroup"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public ObservableCollection<ADObjectCheckList> ValidateSecurityGroupList(TextBox securityGroup, Task<ObservableCollection<ADObjectCheckList>> list)
        {
            if (!string.IsNullOrEmpty(securityGroup.Text))
            {
                string[] groups = securityGroup.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (string group in groups)
                {
                    if (!string.IsNullOrEmpty(group))
                    {
                        list.Result.Single(x => x.Name == group).Checked = true;
                    }
                }
            }

            return list.Result;
        }
    }
}
