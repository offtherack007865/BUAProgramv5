using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Logging;
using BUAProgramv5.Messaging;
using BUAProgramv5.Resolver;
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
using System.Windows.Media;
using Unity;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BUAProgramv5.Pages.UnlockAccount
{
    /// <summary>
    /// Interaction logic for UnlockAccount.xaml
    /// </summary>
    public partial class UnlockAccount : Page
    {
        private MainWindow _mainWindow;
        private IClientService _service;
        private ILogger _logger;
        private bool processing = false;
        public UnlockAccount(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        private async void Username_Textbox_KeyUp(object sender, KeyEventArgs e)
        {
            await Application.Current.Dispatcher.BeginInvoke(new Action(async () => 
            {
                if (string.IsNullOrEmpty(Username_Textbox.Text) || _service == null)
                {
                    return;
                }

                await Task.Delay(10);
                bool userNameExists = _service.DoesNameExist(Username_Textbox.Text);
                if (!processing)
                {
                    UnlockAcount_Button.IsEnabled = false;
                    UnlockAcount_Button.Content = GeneralMessages.UnlockAccountNotification;

                    if (string.IsNullOrEmpty(Username_Textbox.Text))
                    {
                        return;
                    }

                    processing = true;

                    if (!userNameExists)
                    {
                        alert_Label.Content = GeneralMessages.UsernameDoesNotExistNotification;
                        alert_Label.Foreground = Brushes.Red;
                        processing = false;
                        return;
                    }

                    if (_service.IsAccountLocked(Username_Textbox.Text))
                    {
                        alert_Label.Content = GeneralMessages.AccountIsLockedNotification;
                        alert_Label.Foreground = Brushes.Red;
                        UnlockAcount_Button.IsEnabled = true;
                        UnlockAcount_Button.Content = GeneralMessages.AccountUnlockNotification;
                    }
                    else
                    {
                        alert_Label.Content = GeneralMessages.AccountIsUnlockedNotification;
                        alert_Label.Foreground = Brushes.Green;
                        UnlockAcount_Button.IsEnabled = false;
                    }

                    processing = false;
                }
            }));
        }

        private void UnlockAcount_Button_Click(object sender, RoutedEventArgs e)
        {
            UnlockAcount_Button.IsEnabled = false;
            _service.UnlockAccount(Username_Textbox.Text);
            UnlockAcount_Button.IsEnabled = true;
        }

        private async void Unlock_PageLoad(object sender, RoutedEventArgs e)
        {
            Task loadDependencies = Task.Run(() => 
            {
                _service = DependencyResolver.Container.Resolve<IClientService>();
                _logger = DependencyResolver.Container.Resolve<ILogger>();
            });
            await Task.WhenAll(loadDependencies);
        }
    }
}
