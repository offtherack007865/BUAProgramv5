using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.Config;
using BUAProgramv5.Notifications;
using BUAProgramv5.Resolver;
using BUAProgramv5.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Unity;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BUAProgramv5.Pages.CreateNetworkFolder
{
    /// <summary>
    /// Interaction logic for CreateNetworkFolder.xaml
    /// </summary>
    public partial class CreateNetworkFolder : Page
    {
        private MainWindow _mainWindow;
        private IClientService _service;
        private ILogger _logger;
        private bool processing;
        private BUAConfiguration _networkConfig;

        public CreateNetworkFolder(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void User_Textbox_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () =>
            {
                if (!processing)
                {
                    CreateNASFolder_Button.IsEnabled = false;
                    CreateNASFolder_Button.Content = GeneralMessages.CreateNetworkFolderNotification;

                    if (string.IsNullOrEmpty(User_Textbox.Text) || _service == null)
                    {
                        return;
                    }

                    processing = true;

                    await Task.Delay(10);
                    bool userNameExists = _service.DoesNameExist(User_Textbox.Text);

                    if (!userNameExists)
                    {
                        Alert_Label.Content = ErrorMessages.DoesntExistError;
                        Alert_Label.Foreground = Brushes.Red;
                        processing = false;
                        return;
                    }

                    if (Directory.Exists(_networkConfig.NetworkPath + User_Textbox.Text))
                    {
                        Alert_Label.Content = string.Format(GeneralMessages.FolderFoundNotification, User_Textbox.Text);
                        Alert_Label.Foreground = Brushes.Green;
                    }
                    else
                    {
                        Alert_Label.Content = ErrorMessages.NetworkFolderError;
                        Alert_Label.Foreground = Brushes.Red;
                        CreateNASFolder_Button.IsEnabled = true;
                        CreateNASFolder_Button.Content = GeneralMessages.NetworkFolderNotification;
                    }
                    processing = false;
                }
            }));
        }

        private void CreateNASFolder_Button_Click(object sender, RoutedEventArgs e)
        {
            CreateNASFolder_Button.IsEnabled = false;
            _service.CreateNasFolder(User_Textbox.Text);
            _service.MapDriveLetter(User_Textbox.Text, "U:");

            MainWindow.notification.StatusText = string.Format(GeneralMessages.HomeFolderCreation, User_Textbox.Text);
            _mainWindow.StatusBar_TextBlock.Refresh();

            CreateNASFolder_Button.IsEnabled = true;
        }

        private async void NetworkFolder_PageLoad(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadTask = Task.Run(() => { processing = false; _networkConfig = Config.GetNetworkInfo("Directories"); });
            await Task.WhenAll(loadDependencies, loadTask);
        }
    }
}
