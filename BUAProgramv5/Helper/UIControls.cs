using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BUAProgramv5.Helper
{
    internal class UIControls
    {
        /// <summary>
        /// Dynamically reset Create Account Form data.
        /// </summary>
        /// <param name="parent"></param>
        internal void ResetCreateAccountForm(Grid parent, List<ADPrincipalObject> adPrincipalObject, IAccountInformation accountInformation)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        ((TextBox)(ctrl)).Text = string.Empty;
                    }
                }

                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)(ctrl)).Text = string.Empty;
                    }
                }

                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(Label))
                    {
                        if (((Label)(ctrl)).Name == "UsernameAvailability_Label")
                        {
                            ((Label)(ctrl)).Visibility = Visibility.Hidden;
                        }
                    }
                }

                accountInformation.GroupInformation = string.Empty;
                accountInformation.SiteInformation = string.Empty;
                adPrincipalObject = new List<ADPrincipalObject>();
            }));
        }

        /// <summary>
        /// Dynamically reset Move Existing User Form data.
        /// </summary>
        internal void ResetMoveExistingUserForm(Grid parent, ListBox listBox)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        if (((TextBox)(ctrl)).Name == "UserSearch_Textbox")
                        {
                            ((TextBox)(ctrl)).Text = "Last name of User";
                        }
                        else
                        {
                            ((TextBox)(ctrl)).Text = string.Empty;
                        }
                    }
                }

                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)(ctrl)).Text = string.Empty;
                    }
                }

                listBox.Items.Clear();
            }));
        }

        /// <summary>
        /// Dynamically reset Disable User form data.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="textbox"></param>
        /// <param name="listbox"></param>
        internal void ResetDisableUserForm(Grid parent, ListBox listbox)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        if (((TextBox)(ctrl)).Name == "TextBoxSearch")
                        {
                            ((TextBox)(ctrl)).Text = "Last Name of User";
                        }
                        else if (((TextBox)(ctrl)).Name == "UserDetails_Textbox")
                        {
                            ((TextBox)(ctrl)).Text = string.Empty;
                        }
                    }

                    if (ctrl.GetType() == typeof(Button))
                    {
                        if (((Button)(ctrl)).Name == "Disable_Account_Button")
                        {
                            ((Button)(ctrl)).IsEnabled = true;
                        }
                    }

                    if (ctrl.GetType() == typeof(Label))
                    {
                        if (((Label)(ctrl)).Name == "UserNotFound_Label")
                        {
                            ((Label)(ctrl)).Visibility = Visibility.Hidden;
                        }
                    }
                }

                listbox.Items.Clear();
            }));
        }

        /// <summary>
        /// Dynamically reset ReEnable User form data.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="listbox"></param>
        internal void ResetReEnableUserForm(Grid parent, ListBox listbox)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (Control ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        if (((TextBox)(ctrl)).Name == "UserSearch_Textbox")
                        {
                            ((TextBox)(ctrl)).Text = "Last Name of User";
                        }
                        else if (((TextBox)(ctrl)).Name == "JobTitle_Textbox")
                        {
                            ((TextBox)(ctrl)).Text = string.Empty;
                        }
                    }

                    if (ctrl.GetType() == typeof(ComboBox))
                    {
                        ((ComboBox)(ctrl)).Text = string.Empty;
                    }
                }

                listbox.Items.Clear();
            }));
        }

        /// <summary>
        /// Hides objects dynamically based on type.
        /// </summary>
        /// <param name="parent"></param>
        internal void HideButtons(Grid parent)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                foreach (object ctrl in parent.Children)
                {
                    if (ctrl.GetType() == typeof(Button))
                    {
                        ((Button)(ctrl)).Visibility = Visibility.Hidden;
                    }
                    if (ctrl.GetType() == typeof(Image))
                    {
                        ((Image)(ctrl)).Visibility = Visibility.Hidden;
                    }
                }
            }));
        }

        /// <summary>
        /// If not apart of the Dev User group, those options are hidden by default, restores pages as long as user is a developer.
        /// </summary>
        /// <param name="menuItem"></param>
        internal void RestoreAdminButtons(MenuItem menuItem)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                menuItem.Visibility = Visibility.Visible;
            }));
        }
    }
}
