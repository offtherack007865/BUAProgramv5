using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.AD
{
    public class SiteInformation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string OU { get; set; }
        public List<ADPrincipalObject> SecurityGroups { get; set; }
    }
}
