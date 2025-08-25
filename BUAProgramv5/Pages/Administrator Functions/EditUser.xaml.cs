using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Data;
using BUAProgramv5.Helper;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.Pages.Administrator_Functions.Windows;
using BUAProgramv5.Resolver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Unity;

namespace BUAProgramv5.Pages.Administrator_Functions
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Page
    {
        private MainWindow _mainWindow;

        private ITsqlQuery _query;
        private IClientService _service;
        private IAccountInformation _acctInfo;
        private DataValidation _validation;

        private List<UserInformation> _userInfo;
        private UserInformation _newUser;

        public EditUser(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void Edit_User_Loaded(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _query = DependencyResolver.Container.Resolve<ITsqlQuery>();
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _acctInfo = new AccountInformation();
                _validation = new DataValidation();
                _newUser = new UserInformation();
                _userInfo = new List<UserInformation>();
            });
            await Task.WhenAll(loadDependencies);
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if(_query.DoesTableExist("UserTypes"))
                {
                    _userInfo = await _query.GetUserTypes();
                    _userInfo.Add(new UserInformation
                    {
                        Name = "NEW TEMPLATE",
                        SecurityGroups = new List<ADPrincipalObject>()
                    });
                    ExistingTemplates_Combobox.ItemsSource = _userInfo.Select(x => x.Name);
                }
                else
                {
                    _userInfo.Add(new UserInformation
                    {
                        Name = "NEW TEMPLATE",
                        SecurityGroups = new List<ADPrincipalObject>()
                    });
                    ExistingTemplates_Combobox.ItemsSource = _userInfo.Select(x => x.Name);
                }
            }));
        }

        private void ExistingTemplates_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ExistingTemplates_Combobox.SelectedItem.ToString() == "NEW TEMPLATE")
            {
                Name_Textbox.Visibility = Visibility.Visible;
                Name_Label.Visibility = Visibility.Visible;
            }
            else
            {
                Name_Textbox.Visibility = Visibility.Hidden;
                Name_Label.Visibility = Visibility.Hidden;
            }

            _newUser = _userInfo.SingleOrDefault(x => x.Name == ExistingTemplates_Combobox.SelectedItem.ToString());
            Name_Textbox.Text = _newUser.Name;

            StringBuilder sb = new StringBuilder();

            foreach (ADPrincipalObject group in _newUser.SecurityGroups)
            {
                sb.AppendLine(group.Name);
            }

            SecurityGroups_Textbox.Text = sb.ToString();
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            List<string> secGroups = new List<string>() { "OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int", "OU=Security Groups,DC=ad,DC=sumg,DC=int", "OU=EMail,DC=ad,DC=sumg,DC=int", "OU=EMAIL-OLD-DL,DC=ad,DC=sumg,DC=int" };
            ObservableCollection<ADObjectCheckList> list = _service.GetListSecurityGroups(secGroups);
            ObservableCollection<ADObjectCheckList> validatedList = _validation.ValidateSecurityGroupList(list, _acctInfo);

            SecurityGroups secGroupsWindow = new SecurityGroups(validatedList)
            {
                Owner = _mainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            secGroupsWindow.ShowDialog();

            List<ADObjectCheckList> updatedGroupMembership = secGroupsWindow.Groups.ToList();
            if (_newUser.Name != null && _newUser.SecurityGroups != null)
            {
                _newUser.SecurityGroups.Clear();

                foreach (ADObjectCheckList group in updatedGroupMembership.Where(x => x.Checked == true))
                {
                    _newUser.SecurityGroups.Add(new ADPrincipalObject { Name = group.Name, DistinguishedName = group.DistinguishedName });
                }

                StringBuilder sb = new StringBuilder();

                foreach (ADPrincipalObject group in _newUser.SecurityGroups)
                {
                    sb.AppendLine(group.Name);
                }

                SecurityGroups_Textbox.Text = sb.ToString();
            }
        }

        private void Save_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ExistingTemplates_Combobox.SelectedItem.ToString() == "NEW TEMPLATE")
            {
                UserInformation userTemplate = new UserInformation()
                {
                    Name = Name_Textbox.Text,
                    SecurityGroups = _newUser.SecurityGroups
                };
                MainWindow.notification.StatusText = "Creating Template";
                _query.AddUserTemplate(userTemplate);
                MainWindow.notification.StatusText = "Template Saved";
            }
            else
            {
                UserInformation userTemplate = new UserInformation()
                {
                    Name = ExistingTemplates_Combobox.SelectedItem.ToString(),
                    SecurityGroups = _newUser.SecurityGroups
                };

                MainWindow.notification.StatusText = "Editing Template";
                _query.EditUserTemplate(userTemplate);
                MainWindow.notification.StatusText = "Template Saved";
            }

            MessageBox.Show("Template Saved!");
        }
    }
}
