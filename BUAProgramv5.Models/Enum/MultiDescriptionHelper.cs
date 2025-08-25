using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.Enum
{
    [AttributeUsageAttribute(AttributeTargets.All)]
    public class MultiDescriptionHelper : DescriptionAttribute
    {
        public MultiDescriptionHelper(string description, string value) : base(description)
        {
            this.Value = value;
        }

        public string Value { get; private set; }
    }
}
