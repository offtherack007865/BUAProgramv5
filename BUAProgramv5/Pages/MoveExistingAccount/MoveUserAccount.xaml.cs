using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Helper;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.Resolver;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.Screenshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using Unity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BUAProgramv5.Data;

namespace BUAProgramv5.Pages.MoveExistingAccount
{
    /// <summary>
    /// Interaction logic for MoveUserAccount.xaml
    /// </summary>
    public partial class MoveUserAccount : Page
    {
        private MainWindow _mainWindow;
        private ITsqlQuery _query;
        private IClientService _service;
        private IEmailer _emailer;
        private IScreenshotGenerator _screenshot;
        private ILogger _logger;
        private INotificationBar _notification;
        private DataValidation _validation;
        private PageMessages _messages;
        private WPFHelper _helper;
        private UIControls _controls;
        private List<SiteInformation> _siteInfo;
        private bool _processing;
        private string _oldDistName;

        public MoveUserAccount(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void MoveUser_PageLoad(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _query = DependencyResolver.Container.Resolve<ITsqlQuery>();
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _emailer = DependencyResolver.Container.Resolve<IEmailer>();
                _screenshot = DependencyResolver.Container.Resolve<IScreenshotGenerator>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadControlsTask = Task.Run(() => 
            {
                _validation = new DataValidation();
                _messages = new PageMessages();
                _helper = new WPFHelper();
                _controls = new UIControls();
                _notification = new NotificationBar();
                _processing = false;
                _oldDistName = string.Empty;
            });
            await Task.WhenAll(loadDependencies, loadControlsTask);
            Task loadData = Task.Run(() => 
            {
                _siteInfo = _query.GetSiteInfo().Result;
                PopulateControls(_siteInfo);
            });
            await Task.WhenAll(loadData);
            Task<IOrderedEnumerable<string>> loadUsersTask = Task.Run(() => { return _service.GetAllUsers().Select(x => x.Name).OrderBy(x => x); });
            await Task.WhenAll(loadUsersTask);
            Manager_Combobox.ItemsSource = loadUsersTask.Result;
        }

        private async void PopulateControls(List<SiteInformation> siteInfo)
        {
            Task populateControlsTask = Task.Run(async () =>
            {
                List<string> siteList = new List<string>();

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    siteInfo.ForEach(x => siteList.Add(x.Name));
                    Site_Combobox.ItemsSource = siteList;
                }));
            });
            await Task.WhenAll(populateControlsTask);
        }

        private async void UserSearch_Textbox_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (_processing || _service == null)
                {
                    return;
                }
                else if (string.IsNullOrEmpty(UserSearch_Textbox.Text))
                {
                    return;
                }

                _processing = true;

                if (!string.IsNullOrEmpty(UserSearch_Textbox.Text))
                {
                    await Task.Delay(10);
                    List<ADPrincipalObject> searchResults = _service.SearchByLastName(UserSearch_Textbox.Text);
                    User_Listbox.Items.Clear();

                    foreach (ADPrincipalObject user in searchResults)
                    {
                        User_Listbox.Items.Add(user.Name);
                    }
                    UserNotFound_Label.Visibility = User_Listbox.Items.Count == 0 ? Visibility.Visible : Visibility.Hidden;

                    _processing = false;
                }
            }));
        }

        private async void Description_Textbox_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _validation.ValidateMoveUserForm(Site_Combobox, User_Listbox, Manager_Combobox, Description_Textbox, MoveUserAccount_Button);
            }));
        }

        private async void Manager_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _validation.ValidateMoveUserForm(Site_Combobox, User_Listbox, Manager_Combobox, Description_Textbox, MoveUserAccount_Button);
            }));
        }

        private async void Site_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _validation.ValidateMoveUserForm(Site_Combobox, User_Listbox, Manager_Combobox, Description_Textbox, MoveUserAccount_Button);
            }));
        }

        private async void User_Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                UserDetails_Textbox.Clear();
                if (User_Listbox.Items.Count != 0)
                {
                    User userToDeactivate = _service.GetUserByDisplayName(User_Listbox.SelectedItem.ToString());

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
                _validation.ValidateMoveUserForm(Site_Combobox, User_Listbox, Manager_Combobox, Description_Textbox, MoveUserAccount_Button);
            }));
        }

        private async void MoveUSerAccount_Button_Click(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                User user = new User();

                SiteInformation currentSite = new SiteInformation();
                SiteInformation replacementSite = new SiteInformation();

                try
                {
                    replacementSite = _siteInfo.Single(x => x.Name == Site_Combobox.SelectedItem.ToString());
                    user = _service.GetUserByDisplayName(User_Listbox.SelectedItem.ToString());
                    _oldDistName = user.DistinguishedName;
                    currentSite = _siteInfo.SingleOrDefault(x => x.Name == user.SiteName);
                    try
                    {
                        
                        if (currentSite != null)
                        {
                            _service.RemoveUserFromGroups(_oldDistName, currentSite.SecurityGroups);
                            _service.ReplaceUsersCurrentGroupWithNewGroup(_oldDistName, replacementSite.SecurityGroups, user);
                        }
                        else
                        {
                            MessageBox.Show(string.Format(ErrorMessages.DistinguishedADNameError, currentSite.Name));
                        }
                        try
                        {
                            _service.UpdateUserJobDescription(_oldDistName, Description_Textbox.Text);
                            try
                            {
                                _service.UpdateUserSiteInfo(_oldDistName, replacementSite.Phone, replacementSite.Name, _service.GetUserByDisplayName(Manager_Combobox.SelectedItem.ToString()).DistinguishedName);
                                try
                                {
                                    _service.MoveOUs(_oldDistName, replacementSite.OU);
                                    MessageBox.Show(GeneralMessages.MovedAccount);

                                    StringBuilder emailBody = _messages.CreateEmailSiteMessage(user, currentSite, replacementSite);
                                    string htmlbody = emailBody.ToString().Replace(Environment.NewLine, "<br>");

                                    try
                                    {
                                        using (_emailer.memStream = _screenshot.CreateBitmapFromVisual(_mainWindow))
                                        {
                                            _emailer.SendEmail(subject: GeneralMessages.UserAccountMovedNotification + user.DisplayName, body: htmlbody);
                                        }

                                        _mainWindow.NotificationBar_Control.DataContext = _notification;
                                        _notification.StatusText = string.Format(GeneralMessages.UserAccountMovedSuccessful, user.DisplayName);
                                        _mainWindow.StatusBar_TextBlock.Refresh();
                                        _controls.ResetMoveExistingUserForm(MoveExistingAccount_Grid, User_Listbox);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(string.Format(ErrorMessages.EmailFailureNotification, ex.Message, ex.StackTrace));
                                        MessageBox.Show(string.Format(GeneralMessages.EmailFailureNotification, Environment.NewLine, ex.Message, Environment.NewLine, ex.StackTrace), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogError(string.Format(ErrorMessages.MovingOrganizationalUnitError, ex.Message, ex.StackTrace));
                                }
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(string.Format(ErrorMessages.UpdatingSiteError, ex.Message, ex.StackTrace));
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(string.Format(ErrorMessages.UpdatingJobDescriptionError, ex.Message, ex.StackTrace));
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(string.Format(ErrorMessages.RemoveOldSecurityGroupsError, ex.Message, ex.StackTrace));
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(string.Format(GeneralMessages.DataGatheringNotification, ex.Message, ex.StackTrace));
                    MessageBox.Show(string.Format(ErrorMessages.ProgramCrashError), ex.Message);
                }
            }));
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _controls.ResetMoveExistingUserForm(MoveExistingAccount_Grid, User_Listbox);
        }
    }
}
