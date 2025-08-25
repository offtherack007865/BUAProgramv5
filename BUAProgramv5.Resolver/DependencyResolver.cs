using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Data;
using BUAProgramv5.Logging;
using BUAProgramv5.ServerFunctions.Azure;
using BUAProgramv5.ServerFunctions.Exchange;
using BUAProgramv5.ServerFunctions.Mail;
using BUAProgramv5.ServerFunctions.O365;
using BUAProgramv5.ServerFunctions.Screenshot;
using Unity;
using Unity.log4net;

namespace BUAProgramv5.Resolver
{
    public static class DependencyResolver
    {
        public static IUnityContainer Container { get; set; }

        static DependencyResolver()
        {
            Container = new UnityContainer();
            RegisterComponents(Container);
        }

        public static void RegisterComponents(IUnityContainer container)
        {
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IEmailer, Emailer>();
            container.RegisterType<ITsqlQuery, TsqlQuery>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IExchangeWorker, ExchangeWorker>();
            container.RegisterType<IO365Worker, O365Worker>();
            container.RegisterType<IAzureWorker, AzureWorker>();
            container.RegisterType<IScreenshotGenerator, ScreenshotGenerator>();
            container.AddNewExtension<Log4NetExtension>();
        }

        public static void RegisterComponents()
        {
            UnityContainer container = new UnityContainer();
            container.RegisterType<ILogger, Logger>();
            container.RegisterType<IEmailer, Emailer>();
            container.RegisterType<ITsqlQuery, TsqlQuery>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IExchangeWorker, ExchangeWorker>();
            container.RegisterType<IO365Worker, O365Worker>();
            container.RegisterType<IAzureWorker, AzureWorker>();
            container.RegisterType<IScreenshotGenerator, ScreenshotGenerator>();
            container.AddNewExtension<Log4NetExtension>();
        }
    }
}
