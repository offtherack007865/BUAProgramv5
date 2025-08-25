using BUAProgramv5.Logging;
using BUAProgramv5.Resolver;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.Screenshot;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace BUAProgramv5
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            DependencyResolver.RegisterComponents();
            DependencyResolver.Container.Resolve<MainWindow>().Show();
        }
    }
}
