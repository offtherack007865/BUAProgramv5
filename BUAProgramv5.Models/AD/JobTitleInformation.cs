using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.AD
{
    public class JobTitleInformation
    {
        public JobTitleInformation(string inputPermanentOrContractor, string inputJobTitle)
        {
            PermanentOrContractor = inputPermanentOrContractor;
            JobTitle = inputJobTitle;
        }
        public string PermanentOrContractor { get; set; }
        public string JobTitle { get; set; }
    }
}
