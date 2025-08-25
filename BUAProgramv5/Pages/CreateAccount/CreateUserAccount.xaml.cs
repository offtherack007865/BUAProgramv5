using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.Screenshot;
using BUAProgramv5.Logging;
using BUAProgramv5.Notifications;
using BUAProgramv5.Data;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Helper;
using System.Collections.ObjectModel;
using BUAProgramv5.Pages.Administrator_Functions.Windows;
using BUAProgramv5.Messaging;
using System.IO;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Utilities;
using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Resolver;
using Unity;

namespace BUAProgramv5.Pages.CreateAccount
{
    /// <summary>
    /// Interaction logic for CreateUserAccount.xaml
    /// </summary>
    public partial class CreateUserAccount : Page
    {
        private MainWindow _mainWindow;
        private ITsqlQuery _query;
        private IClientService _service;
        private IEmailer _emailer;
        private IScreenshotGenerator _screenshot;
        private ILogger _logger;

        private INotificationBar _notification;
        private IAccountInformation _acctInfo;
        private DataValidation _validation;
        private UIControls _controls;
        private WPFHelper _helper;
        private PageMessages _messages;
        private List<SiteInformation> _siteInfo;
        private List<UserInformation> _userInfo;
        private List<JobTitleInformation> _jobTitleInfo;
        private List<ADPrincipalObject> _groupMembership;
        private StringBuilder _groups;
        private BUAConfiguration _networkConfig;

        public CreateUserAccount(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void CreateAccount_PageLoad(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _query = DependencyResolver.Container.Resolve<ITsqlQuery>();
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _emailer = DependencyResolver.Container.Resolve<IEmailer>();
                _screenshot = DependencyResolver.Container.Resolve<IScreenshotGenerator>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadControls = Task.Run(() =>
            {
                _notification = new NotificationBar();
                _acctInfo = new AccountInformation();
                _validation = new DataValidation();
                _controls = new UIControls();
                _helper = new WPFHelper();
                _groupMembership = new List<ADPrincipalObject>();
                _networkConfig = Config.GetNetworkInfo("Directories");
                _messages = new PageMessages();
            });
            await Task.WhenAll(loadDependencies, loadControls);
            Task loadData = Task.Run(() =>
            {
                _userInfo = _query.GetUserTypes().Result;
                _jobTitleInfo = _query.GetJobTitles().Result;
                _siteInfo = _query.GetSiteInfo().Result;
                PopulateControls(_siteInfo, _userInfo, _jobTitleInfo);
            });
            await Task.WhenAll(loadData);
            Task<IOrderedEnumerable<string>> loadUsersTask = Task.Run(() => { return _service.GetAllUsers().Select(x => x.Name).OrderBy(x => x); });
            await Task.WhenAll(loadUsersTask);
            Manager_Combobox.ItemsSource = loadUsersTask.Result;
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SiteInformation_Textbox.DataContext = _acctInfo;
                GroupInformation_Textbox.DataContext = _acctInfo;
            }));
        }

        private async void Site_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                _helper.DetermineSiteCombobox(Site_Combobox, _siteInfo, _acctInfo, Department_Label, Department_Textbox);
                _helper.BuildGroupInformation(Site_Combobox, _siteInfo, UserType_Combobox, _userInfo, _groupMembership, _acctInfo);
            }));
        }

        private async void UserType_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                _helper.DetermineUserType(UserType_Combobox, Title_Label, Title_Textbox);
                _groupMembership.Clear();
                _groupMembership = _helper.GenerateGroupMembership(Site_Combobox, _siteInfo, UserType_Combobox, _userInfo, _groupMembership);
                _groups = _helper.BuildGroupInformation(_groupMembership);
                _acctInfo.GroupInformation = _groups.ToString();
            }));
        }

        private async void AddGroups_Button_Click(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() => 
            {
                ObservableCollection<ADObjectCheckList> list = _service.GetListSecurityGroups(new List<string> { "OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int", "OU=Security Groups,DC=ad,DC=sumg,DC=int", "OU=EMail,DC=ad,DC=sumg,DC=int", "OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int" });
                ObservableCollection<ADObjectCheckList> validatedList = _validation.ValidateSecurityGroupList(list, _acctInfo);

                SecurityGroups securityGroupWindow = new SecurityGroups(validatedList)
                {
                    Owner = _mainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                securityGroupWindow.ShowDialog();

                List<ADObjectCheckList> updatedGroupMembership = securityGroupWindow.Groups.ToList();
                _groupMembership.Clear();

                foreach (ADObjectCheckList group in updatedGroupMembership.Where(x => x.Checked == true))
                {
                    _groupMembership.Add(new ADPrincipalObject { Name = group.Name, DistinguishedName = group.DistinguishedName });
                }

                _groupMembership = _helper.GenerateGroupMembership(Site_Combobox, _siteInfo, UserType_Combobox, _userInfo, _groupMembership);
                _groups = _helper.BuildGroupInformation(_groupMembership);
                _acctInfo.GroupInformation = _groups.ToString();
            }));
        }

        private async void Reset_Button_Click(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _controls.ResetCreateAccountForm(CreateAccount_Grid, _groupMembership, _acctInfo);
            }));
            
        }

        private async void CreateAccount_Button_Click(object sender, RoutedEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (_validation.ValidateForm(FirstName_Textbox, LastName_Textbox, UserName_Textbox, Manager_Combobox, Site_Combobox, Department_Textbox, UserType_Combobox, Description_Combobox, UsernameAvailability_Label, _service))
                {
                    int errors = 0;
                    //Does not send emails since email addresses are not created anymore.
                    //bool mailboxResult = false;
                    bool mailboxResult = true;
                    MessageBoxResult result = MessageBox.Show("Do you wish to create an account?", "Are you sure?", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        CreateAccount_Button.IsEnabled = false;

                        User newUser = new User()
                        {
                            Department = Department_Textbox.Text,
                            EmployeeId = EmployeeID_Textbox.Text,
                            DisplayName = _helper.GenerateDisplayName(Title_Textbox, MiddleInitial_Textbox, LastName_Textbox, FirstName_Textbox).Trim(),
                            EmailAddress = UserName_Textbox.Text.Trim() + GeneralMessages.CompanyEmail,
                            FirstName = FirstName_Textbox.Text.Trim(),
                            JobDescription = Description_Combobox.SelectedItem.ToString(),
                            LastName = LastName_Textbox.Text.Trim(),
                            MiddleInitial = MiddleInitial_Textbox.Text.Trim(),
                            Manager = _service.SearchADByName(Manager_Combobox.SelectedItem.ToString()),
                            Username = UserName_Textbox.Text.Replace(" ", ""),
                            OU = (UserType_Combobox.SelectedItem.ToString() == "Doctor") ? "OU=Physicians--Email,OU=EMail,DC=ad,DC=sumg,DC=int" : _siteInfo.SingleOrDefault(x => x.Name == Site_Combobox.SelectedItem.ToString()).OU,
                            PhoneNumber = _siteInfo.SingleOrDefault(x => x.Name == Site_Combobox.SelectedItem.ToString()).Phone,
                            SiteName = Site_Combobox.SelectedItem.ToString().Contains("099") ? "099 - Central" : _siteInfo.SingleOrDefault(x => x.Name == Site_Combobox.SelectedItem.ToString()).Name,
                            Groups = _helper.GenerateGroupMembership(Site_Combobox, _siteInfo, UserType_Combobox, _userInfo, _groupMembership),
                            Company = "Summit Medical Group",
                            UserPassword = PasswordGeneration.GenerateUniquePassword()
                        };
                        try
                        {
                            MainWindow.notification.StatusText = GeneralMessages.ActiveDirectoryAccount;
                            _mainWindow.StatusBar_TextBlock.Refresh();
                            _service.CreateNewUser(newUser);
                            _service.AddUserToGroups(newUser);
                            try
                            {
                                if (_validation.ValidateNASComboBox(Site_Combobox))
                                {
                                    IEnumerable<string> directories = Directory.EnumerateDirectories(_networkConfig.NetworkPath);

                                    if (!directories.Any(x => x == _networkConfig.NetworkPath + newUser.Username))
                                    {
                                        MainWindow.notification.StatusText = GeneralMessages.CreateNetworkFolder;
                                        _mainWindow.StatusBar_TextBlock.Refresh();
                                        bool nasResult = _service.CreateNasFolder(newUser.Username);
                                        if (!nasResult)
                                        {
                                            MessageBox.Show(GeneralMessages.CreateNasSyncMessageNotification);
                                            _service.CreateNasFolder(newUser.Username);
                                        }
                                        bool mapdriveResult = _service.MapDriveLetter(newUser.Username, "U:");
                                        if (!mapdriveResult)
                                        {
                                            MessageBox.Show(GeneralMessages.CreateMapDriveSyncMessageNotification);
                                            _service.MapDriveLetter(newUser.Username, "U:");
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MainWindow.notification.StatusText = ErrorMessages.NetworkStorageError;
                                _mainWindow.StatusBar_TextBlock.Refresh();
                                _logger.LogError(string.Format(ErrorMessages.CreatingNetworkFolderError, ex.Message, ex.StackTrace));
                                errors++;
                                mailboxResult = true;
                            }
                            if (errors > 0)
                            {
                                _logger.LogError(string.Format(ErrorMessages.PartialAccountCreationError, errors, newUser.DisplayName));
                                MessageBox.Show(string.Format(GeneralMessages.PartialAccountCreation, errors, newUser.DisplayName), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            MainWindow.notification.StatusText = GeneralMessages.AccountCreationSuccessful + newUser.DisplayName;
                            _mainWindow.StatusBar_TextBlock.Refresh();

                            MessageBox.Show(GeneralMessages.EmailNotification);

                            StringBuilder stringBuilder = _messages.CreateEmailMessage(newUser, mailboxResult, errors);
                            string htmlBody = stringBuilder.ToString().Replace(Environment.NewLine, "<br>");

                            using (_emailer.memStream = _screenshot.CreateBitmapFromVisual(_mainWindow))
                            {
                                _emailer.SendEmail(GeneralMessages.NewAccountCreated + newUser.Username, htmlBody);
                            }
                        }
                        catch (Exception ex)
                        {
                            MainWindow.notification.StatusText = ErrorMessages.ActiveDirectoryError;
                            _mainWindow.StatusBar_TextBlock.Refresh();

                            _logger.LogError(string.Format(ErrorMessages.AccountCreationError, ex.Message, ex.StackTrace));
                            return;
                        }
                    }
                }
                CreateAccount_Button.IsEnabled = true;
                _controls.ResetCreateAccountForm(CreateAccount_Grid, _groupMembership, _acctInfo);
            }));
        }

        private async void CreateAccount_Grid_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if(_service != null)
                {
                    await Task.Delay(10);
                    UserName_Textbox.Text = _validation.ValidateGeneratedUserName(FirstName_Textbox.Text, MiddleInitial_Textbox.Text, LastName_Textbox.Text);
                    _validation.ValidateUserName(UserName_Textbox.Text.Replace(" ", ""), UsernameAvailability_Label, _service);
                }
            }));
        }

        private async void PopulateControls(List<SiteInformation> siteInfo, List<UserInformation>userTypeInfo, List<JobTitleInformation> jobTitleInformationList)
        {
            Task populateControlsTask = Task.Run(async () =>
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

                    List<string> jobTitleList = new List<string>();
                    jobTitleInformationList.ForEach(x => jobTitleList.Add(x.JobTitle));
                    Description_Combobox.ItemsSource = jobTitleList;
                }));
            });
            await Task.WhenAll(populateControlsTask);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            _controls.ResetCreateAccountForm(CreateAccount_Grid, _groupMembership, _acctInfo);
        }
    }
}
