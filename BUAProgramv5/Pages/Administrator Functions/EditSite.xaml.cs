using BUAProgramv5.Data;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Unity;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BUAProgramv5.API.ClientWorker.Service;
using System.Collections.ObjectModel;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Helper;
using BUAProgramv5.Pages.Administrator_Functions.Windows;

namespace BUAProgramv5.Pages.Administrator_Functions
{
    /// <summary>
    /// Interaction logic for EditSite.xaml
    /// </summary>
    public partial class EditSite : Page
    {
        private MainWindow _mainWindow;

        private ITsqlQuery _query;
        private IClientService _service;
        private ILogger _logger;


        private IAccountInformation _acctInfo;
        private DataValidation _validation;
        private INotificationBar _notification;
        public SiteInformation _siteInfo;
        private List<SiteInformation> _allSites;

        public EditSite(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task<ObservableCollection<ADObjectCheckList>> list = Task.Run(() => { return _service.GetAllSecurityGroups(); });
                await Task.WhenAll(list);
                ObservableCollection<ADObjectCheckList> validatedList = _validation.ValidateSecurityGroupList(SecurityGroup_Textbox, list);

                SecurityGroups securityGroupWindow = new SecurityGroups(validatedList)
                {
                    Owner = _mainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };
                securityGroupWindow.ShowDialog();

                List<ADObjectCheckList> updatedGroupMembership = securityGroupWindow.Groups.ToList();

                if (_siteInfo.SecurityGroups != null)
                {
                    _siteInfo.SecurityGroups.Clear();

                    foreach (ADObjectCheckList group in updatedGroupMembership.Where(x => x.Checked == true))
                    {
                        _siteInfo.SecurityGroups.Add(new ADPrincipalObject
                        {
                            Name = group.Name,
                            DistinguishedName = group.DistinguishedName
                        });
                    }

                    StringBuilder sb = new StringBuilder();

                    foreach (ADPrincipalObject group in _siteInfo.SecurityGroups)
                    {
                        sb.AppendLine(group.Name);
                    }

                    SecurityGroup_Textbox.Text = sb.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(ErrorMessages.EditSiteError, ex.Message, ex.StackTrace));
                throw;
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ExistingSites_Combobox.SelectedItem.ToString() == "New Site")
            {

                SiteInformation newSiteDetails = new SiteInformation()
                {
                    Name = Name_Textbox.Text,
                    Phone = PhoneNumber_Textbox.Text,
                    OU = Org_Combobox.SelectedItem.ToString(),
                    SecurityGroups = _siteInfo.SecurityGroups
                };
                _notification.StatusText = "Creating Site";
                _mainWindow.StatusBar_TextBlock.Refresh();

                _query.AddSite(newSiteDetails);

                _notification.StatusText = "Site Saved";
                _mainWindow.StatusBar_TextBlock.Refresh();
                MessageBox.Show("Site Created!");
            }

            else
            {
                SiteInformation newSiteDetails = new SiteInformation()
                {
                    ID = _siteInfo.ID,
                    Name = Name_Textbox.Text,
                    Phone = PhoneNumber_Textbox.Text,
                    OU = Org_Combobox.SelectedItem.ToString(),
                    SecurityGroups = _siteInfo.SecurityGroups
                };

                _notification.StatusText = "Editing Site";
                _mainWindow.StatusBar_TextBlock.Refresh();

                _query.EditSite(newSiteDetails);

                _notification.StatusText = "Site Saved";
                _mainWindow.StatusBar_TextBlock.Refresh();
            }
            MessageBox.Show("Site Saved!");
        }

        private void ExistingSites_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _siteInfo = _allSites.Single(x => x.Name == ExistingSites_Combobox.SelectedItem.ToString());

            Name_Textbox.Text = _siteInfo.Name;
            PhoneNumber_Textbox.Text = _siteInfo.Phone;
            Org_Combobox.SelectedItem = _siteInfo.OU;

            StringBuilder sb = new StringBuilder();

            foreach (ADPrincipalObject group in _siteInfo.SecurityGroups)
            {
                sb.AppendLine(group.Name);
            }

            SecurityGroup_Textbox.Text = sb.ToString();
        }

        private async void Edit_Site_Loaded(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _query = DependencyResolver.Container.Resolve<ITsqlQuery>();
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
                _acctInfo = new AccountInformation();
                _validation = new DataValidation();
                _notification = new NotificationBar();
                _allSites = new List<SiteInformation>();
                _siteInfo = new SiteInformation();
            });
            await Task.WhenAll(loadDependencies);

            if (_query.DoesTableExist("Site") && _query.DoesTableExist("Site_SecurityGroups"))
            {
                Task loadData = Task.Run(() =>
                {
                    _allSites = _query.GetSiteInfo().Result;
                });
                await Task.WhenAll(loadData);
                _allSites.Add(new SiteInformation { Name = "New Site", SecurityGroups = new List<ADPrincipalObject>() });
                ExistingSites_Combobox.ItemsSource = _allSites.Select(x => x.Name);
                Task<List<string>> loadUserData = Task.Run(() => { return _service.GetUserOUs(); });
                await Task.WhenAll(loadUserData);
                Org_Combobox.ItemsSource = loadUserData.Result;
            }
            else
            {
                _allSites.Add(new SiteInformation { Name = "New Site", SecurityGroups = new List<ADPrincipalObject>() });
                ExistingSites_Combobox.ItemsSource = _allSites.Select(x => x.Name);
                Task<List<string>> loadUserData = Task.Run(() => { return _service.GetUserOUs(); });
                await Task.WhenAll(loadUserData);
                Org_Combobox.ItemsSource = loadUserData.Result;
            }
        }
    }
}
