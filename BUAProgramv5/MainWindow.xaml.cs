using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Data;
using BUAProgramv5.Helper;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Notifications;
using BUAProgramv5.Pages.Administrator_Functions;
using BUAProgramv5.Pages.CreateAccount;
using BUAProgramv5.Pages.CreateNetworkFolder;
using BUAProgramv5.Pages.DisableAccount;
using BUAProgramv5.Pages.MoveExistingAccount;
using BUAProgramv5.Pages.Re_EnableAccount;
using BUAProgramv5.Pages.UnlockAccount;
using BUAProgramv5.Resolver;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.Screenshot;
using ControlzEx.Theming;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace BUAProgramv5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        CreateUserAccount _createUserAccountPage;
        CreateNetworkFolder _createNetworkFolderPage;
        DisableAccount _disableAccountPage;
        MoveUserAccount _moveUserAccountPage;
        Re_EnableAccount _re_EnableAccountPage;
        UnlockAccount _unlockAccountPage;
        EditSite _editSitePage;
        EditUser _editUserPage;
        UIControls _controls;
        public static INotificationBar notification = new NotificationBar();
        private ILogger _logger;
        private IClientService _service;

        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() =>
            {
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            Task loadControls = Task.Run(() => { _controls = new UIControls(); InitializePages(); SetChildPageSize(); });
            await Task.WhenAll(loadDependencies, loadControls);
            Task<bool> authorized = Task.Run(() => { return _service.UserADGroup(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName), "IT Dept"); });
            Task<bool> devAuthorized = Task.Run(() => { return _service.UserADGroup(string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName), "Dev Users"); });
            await Task.WhenAll(authorized, devAuthorized);
            if (!authorized.Result)
            {
                NotificationBar_Control.DataContext = notification;
                notification.StatusText = string.Format(GeneralMessages.NonAuthorizedNotification, Environment.UserName);
                _logger.LogInfo(string.Format(GeneralMessages.NonAuthorizedNotification, Environment.UserName));
                Application.Current.Shutdown();
            }
            if(devAuthorized.Result)
            {
                _controls.RestoreAdminButtons(EditSiteInfo_Menu);
                _controls.RestoreAdminButtons(EditUser_Menu);
            }

            if (loadDependencies.IsCompleted && authorized.Result)
            {
                NotificationBar_Control.DataContext = notification;
                notification.StatusText = string.Format(GeneralMessages.ReadyStateNotification, string.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName));
            }
        }

        private async void InitializePages()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
             {
                 _createUserAccountPage = new CreateUserAccount(mainWindow);
                 _createNetworkFolderPage = new CreateNetworkFolder(mainWindow);
                 _disableAccountPage = new DisableAccount(mainWindow);
                 _moveUserAccountPage = new MoveUserAccount(mainWindow);
                 _re_EnableAccountPage = new Re_EnableAccount(mainWindow);
                 _unlockAccountPage = new UnlockAccount(mainWindow);
                 _editSitePage = new EditSite(mainWindow);
                 _editUserPage = new EditUser(mainWindow);
             }));
        }

        private async void SetChildPageSize()
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                _NavigationFrame.Width = 825;
                _NavigationFrame.Height = 650;
            }));
        }

        private void DisableButtons()
        {
            _controls.HideButtons(MainWindow_Grid);
        }

        private void CreateNetworkFolder_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_createNetworkFolderPage);
        }

        private void CreateUserAccount_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_createUserAccountPage);
        }

        private void DisableUserAccount_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_disableAccountPage);
        }

        private void MoveUserAccount_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_moveUserAccountPage);
        }

        private void ReEnableUserAccount_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_re_EnableAccountPage);
        }

        private void UnlockUserAccount_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_unlockAccountPage);
        }

        private void EditSiteInfo_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_editSitePage);
        }

        private void EditUser_Menu_Click(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            _NavigationFrame.Navigate(_editUserPage);
        }

        private void Exit_Menu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
