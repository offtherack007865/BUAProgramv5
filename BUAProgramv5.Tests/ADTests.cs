using BUAProgramv5.API.ClientWorker.Service;
using BUAProgramv5.Logging;
using BUAProgramv5.Models.AD;
using BUAProgramv5.Models.Enum;
using BUAProgramv5.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace BUAProgramv5.Tests
{
    [TestFixture]
    public class ADTests
    {
        private IClientService _service;
        private ILogger _logger;
        public ADTests()
        {
            _service = DependencyResolver.Container.Resolve<IClientService>();
            _logger = DependencyResolver.Container.Resolve<ILogger>();
        }

        [TestCase]
        public void GetAllSiteSecurityGroupsFromAD()
        {
            List<string> SMGOUs = new List<string>() { "OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int", "OU=Security Groups,DC=ad,DC=sumg,DC=int", "OU=EMail,DC=ad,DC=sumg,DC=int" };
            //List<string> securityGroups = new List<string>() { "OU=Security Groups,DC=ad,DC=sumg,DC=int" };
            //List<string> mainOUGroups = new List<string>() { "OU=EMail,DC=ad,DC=sumg,DC=int" };
            ObservableCollection<ADObjectCheckList> results = _service.GetListSecurityGroups(SMGOUs);

        }


        [TestCase]
        public void GetSecurityGroupsFromAD()
        {
            //string orgGroup = "OU=Security Groups,DC=ad,DC=sumg,DC=int";
            //List<string> securityGroups = new List<string>() { "OU=Security Groups,DC=ad,DC=sumg,DC=int" };
            //List<string> mainOUGroups = new List<string>() { "OU=EMail,DC=ad,DC=sumg,DC=int" };
            //ObservableCollection<ADObjectCheckList> results = _service.(orgGroup);

        }

        [TestCase]
        public void GetAdditionalSiteInformation()
        {
            //var test = _activeDirectory.GetAllSecurityGroups("OU=Security Groups - Remote Offices,DC=ad,DC=sumg,DC=int");
        }
    }
}
