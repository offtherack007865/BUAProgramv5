using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Models.RestSharp
{
    public class ApiAuthentication
    {
        public ApiAuthentication()
        {
            this.access_token = string.Empty;
            this.token_type = string.Empty;
            this.expires_in = 0;
            this.UserName = string.Empty;

        }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
        public string UserName { get; set; }
    }
}
