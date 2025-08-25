using Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUAProgramv5.Resolver;
using BUAProgramv5.ConsoleApp.Helper;
using BUAProgramv5.Data;
using BUAProgramv5.API.ClientWorker.Service;

namespace BUAProgramv5.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IClientService service = DependencyResolver.Container.Resolve<IClientService>();
            ITsqlQuery tsql = DependencyResolver.Container.Resolve<ITsqlQuery>();
            ConsoleHelper helper = new ConsoleHelper(service, tsql);
            helper.SeedTables();
        }
    }
}
