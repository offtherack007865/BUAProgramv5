using BUAProgramv5.Helper;
using BUAProgramv5.Logging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.Screenshot;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Linq;
using System.Windows;
using System;
using BUAProgramv5.Messaging;
using System.Text;
using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Resolver;
using Unity;
using BUAProgramv5.Data;

namespace BUAProgramv5.Pages.Re_EnableAccount
{
    /// <summary>
    /// Interaction logic for Re_EnableAccount.xaml
    /// </summary>
    public partial class Re_EnableAccount : Page
    {
        private MainWindow _mainWindow;
        private ITsqlQuery _query;
        private IClientService _service;
        private IEmailer _emailer;
        private IScreenshotGenerator _screenshot;
        private ILogger _logger;
        private INotificationBar _notification;
        private WPFHelper _helper;
        private UIControls _controls;
        private DataValidation _validation;
        private PageMessages _messages;
        private List<SiteInformation> _siteInfo;
        private List<UserInformation> _userInfo;
        private List<ADPrincipalObject> _groups;
        private bool _processing;
        public Re_EnableAccount(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void Re_Enable_PageLoad(object sender, System.Windows.RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _query = DependencyResolver.Container.Resolve<ITsqlQuery>();
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _emailer = DependencyResolver.Container.Resolve<IEmailer>();
                _screenshot = DependencyResolver.Container.Resolve<IScreenshotGenerator>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadTask = Task.Run(() => 
            {
                _controls = new UIControls();
                _helper = new WPFHelper();
                _notification = new NotificationBar();
                _processing = false;
                _validation = new DataValidation();
                _groups = new List<ADPrincipalObject>();
                _messages = new PageMessages();
            });
            await Task.WhenAll(loadDependencies, loadTask);
            Task loadData = Task.Run(() => 
            {
                _userInfo = _query.GetUserTypes().Result;
                _siteInfo = _query.GetSiteInfo().Result;
                PopulateControls(_siteInfo, _userInfo);
            });
            await Task.WhenAll(loadData);
            Task<IOrderedEnumerable<string>> loadUsersTask = Task.Run(() => { return _service.GetAllUsers().Select(x => x.Name).OrderBy(x => x); });
            await Task.WhenAll(loadUsersTask);
            Manager_Combobox.ItemsSource = loadUsersTask.Result;
        }

        private async void UserSearch_Textbox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (_processing || _service == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(UserSearch_Textbox.Text))
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

                    if (User_Listbox.Items.Count == 0)
                    {
                        UserNotFound_Label.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        UserNotFound_Label.Visibility = Visibility.Hidden;
                    }

                    _processing = false;
                }
            }));
        }

        private async void User_Listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _helper.DetermineUserListbox(User_Listbox, User_Details_Textbox, _service, Re_Enable_Account_Button);
            }));
        }

        private async void Re_Enable_Account_Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                bool ready = _validation.ValidateReEnableForm(User_Listbox, UserSearch_Textbox, JobTitle_Textbox, Manager_Combobox, Site_Combobox, UserType_Combobox);
                if (!ready)
                {
                    return;
                }
                else
                {
                    SiteInformation site = _siteInfo.SingleOrDefault(x => x.Name == Site_Combobox.SelectedItem.ToString());
                    UserInformation user = _userInfo.SingleOrDefault(x => x.Name == UserType_Combobox.SelectedItem.ToString());

                    _groups.AddRange(site.SecurityGroups);
                    _groups.AddRange(user.SecurityGroups);

                    User userName = new User()
                    {
                        DistinguishedName = _service.SearchADByName(User_Listbox.SelectedItem.ToString()).DistinguishedName,
                        JobDescription = JobTitle_Textbox.Text,
                        Username = _service.GetUserByDisplayName(User_Listbox.SelectedItem.ToString()).Username,
                        PhoneNumber = site.Phone,
                        SiteName = site.Name,
                        Manager = _service.SearchADByName(Manager_Combobox.SelectedItem.ToString()),
                        Groups = _groups,
                        UserPassword = PasswordGeneration.GenerateUniquePassword()
                    };

                    MessageBoxResult Result = MessageBox.Show("Are you sure?", "Are you sure?", MessageBoxButton.YesNo);
                    if (Result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            _mainWindow.NotificationBar_Control.DataContext = _notification;
                            _notification.StatusText = GeneralMessages.ReEnableAccountNotification;
                            _mainWindow.StatusBar_TextBlock.Refresh();
                            _service.ReEnableExistingUser(userName);
                            try
                            {
                                _mainWindow.NotificationBar_Control.DataContext = _notification;
                                _notification.StatusText = GeneralMessages.RelocateUserAccountNotification + site.OU;
                                _mainWindow.StatusBar_TextBlock.Refresh();
                                _service.MoveOUs(_service.GetUserByDisplayName(User_Listbox.SelectedItem.ToString()).DistinguishedName, site.OU);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(string.Format(ErrorMessages.MovingUserAccountError, ex.Message, ex.StackTrace));
                                MessageBox.Show(ErrorMessages.MovingUserAccountNotification);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(string.Format(ErrorMessages.ReEnablingUserAccountError, ex.Message, ex.StackTrace));
                            MessageBox.Show(string.Format(ErrorMessages.ReEnablingUserAccountNotification), ex.Message);
                            return;
                        }

                        StringBuilder sb = _messages.CreateEmailReEnableMessage(userName);
                        string htmlbody = sb.ToString().Replace(Environment.NewLine, "<br>");
                        using (_emailer.memStream = _screenshot.CreateBitmapFromVisual(_mainWindow))
                        {
                            _emailer.SendEmail(subject: GeneralMessages.AccountReEnabledEmailNotification + userName.Username, body: htmlbody, copyArchitecture: true);
                        }

                        _mainWindow.NotificationBar_Control.DataContext = _notification;
                        _notification.StatusText = GeneralMessages.AccountReEnabledNotification;
                        _mainWindow.StatusBar_TextBlock.Refresh();
                        _controls.ResetReEnableUserForm(Re_EnableAccount_Grid, User_Listbox);
                    }
                }
            }));
        }

        private async void PopulateControls(List<SiteInformation> siteInfo, List<UserInformation> userTypeInfo)
        {
            Task controlsTask = Task.Run(async () =>
            {
                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    List<string> userTypeList = new List<string>();
                    userTypeInfo.ForEach(x => userTypeList.Add(x.Name));
                    UserType_Combobox.ItemsSource = userTypeList;
                    UserType_Combobox.SelectedIndex = 2;

                    List<string> siteList = new List<string>();
                    siteInfo.ForEach(x => siteList.Add(x.Name));
                    Site_Combobox.ItemsSource = siteList;
                }));
            });
            await Task.WhenAll(controlsTask);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _controls.ResetReEnableUserForm(Re_EnableAccount_Grid, User_Listbox);
        }
    }
}
