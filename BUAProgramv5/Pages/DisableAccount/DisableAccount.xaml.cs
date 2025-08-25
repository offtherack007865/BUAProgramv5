using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Helper;
using BUAProgramv5.Logging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.Resolver;
using BUAProgramv5.ServerFunctions.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unity;

namespace BUAProgramv5.Pages.DisableAccount
{
    /// <summary>
    /// Interaction logic for DisableAccount.xaml
    /// </summary>
    public partial class DisableAccount : Page
    {
        private MainWindow _mainWindow;
        private IClientService _service;
        private IEmailer _emailer;
        private ILogger _logger;
        private INotificationBar _notification;
        private UIControls _controls;
        private WPFHelper _helper;
        private bool _deactivateUser;

        public DisableAccount(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void DisableAccount_PageLoad(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _emailer = DependencyResolver.Container.Resolve<IEmailer>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadTask = Task.Run(() => { _controls = new UIControls(); _helper = new WPFHelper(); _deactivateUser = false; _notification = new NotificationBar(); });
            await Task.WhenAll(loadDependencies, loadTask);
        }

        private async void TextBoxSearch_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (_deactivateUser || _service == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(TextBoxSearch.Text))
                {
                    return;
                }

                _deactivateUser = true;

                if (!string.IsNullOrEmpty(TextBoxSearch.Text))
                {
                    await Task.Delay(10);
                    List<ADPrincipalObject> searchResults = _service.SearchByLastName(TextBoxSearch.Text);
                    UserName_Listbox.Items.Clear();

                    foreach (ADPrincipalObject user in searchResults)
                    {
                        UserName_Listbox.Items.Add(user.Name);
                    }

                    UserNotFound_Label.Visibility = UserName_Listbox.Items.Count == 0 ? Visibility.Visible : Visibility.Hidden;
                    _deactivateUser = false;
                }
            }));
        }

        private async void UserName_Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                UserDetails_Textbox.Clear();
                if (UserName_Listbox.Items.Count != 0)
                {
                    User userToDeactivate = _service.GetUserByDisplayName(UserName_Listbox.SelectedItem.ToString());

                    foreach (System.Reflection.PropertyInfo item in typeof(User).GetProperties())
                    {
                        if (item.GetValue(userToDeactivate, null) != null)
                        {
                            if (!item.GetValue(userToDeactivate, null).GetType().IsGenericType && !item.GetValue(userToDeactivate, null).GetType().IsGenericTypeDefinition)
                            {
                                if (item.Name != "UserPassword")
                                {
                                    UserDetails_Textbox.AppendText(string.Format("{0}: {1}{2}", item.Name, item.GetValue(userToDeactivate, null), Environment.NewLine));
                                }
                            }
                        }
                    }

                    UserDetails_Textbox.AppendText(string.Format("{0}{1}GROUPS{2}", Environment.NewLine, Environment.NewLine, Environment.NewLine));

                    foreach (ADPrincipalObject item in userToDeactivate.Groups.OrderBy(x => x.Name))
                    {
                        UserDetails_Textbox.AppendText(item.Name + Environment.NewLine);
                    }
                }
            }));
        }

        private async void Disable_Account_Button_Click(object sender, RoutedEventArgs e)
        {
            Disable_Account_Button.IsEnabled = false;
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBoxResult messageResult = MessageBox.Show(string.Format("Are you sure you want to disable {0}?", UserName_Listbox.SelectedItem), "Are you sure?", MessageBoxButton.YesNo);
                if (messageResult == MessageBoxResult.Yes)
                {
                    if(_service != null && _emailer != null && _logger != null)
                    {
                        User deactivatingAccount = _service.GetUserByDisplayName(UserName_Listbox.SelectedItem.ToString());
                        if(deactivatingAccount != null)
                        {
                            string userStatus = _helper.DisablePageSetUserValues(deactivatingAccount).Result;
                            _helper.DisablePageDisableAccount(deactivatingAccount, UserName_Listbox, _service, _emailer, _logger, userStatus, _notification, _mainWindow);
                            Disable_Account_Button.IsEnabled = true;
                        }
                    }
                }
            }));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _controls.ResetDisableUserForm(DisableAccount_Grid, UserName_Listbox);
        }
    }
}
