using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.AD
{
    public class UserInformation
    {
        public string Name { get; set; }
        public List<ADPrincipalObject> SecurityGroups { get; set; }
    }
}
