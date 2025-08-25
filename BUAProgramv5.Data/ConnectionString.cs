using BUAProgramv5.Models.Config;
using BUAProgramv5.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Data
{
    internal class ConnectionString
    {
        internal string Value { get; set; }
        private BUAConfiguration _section;

        internal ConnectionString(DbType dbtype)
        {
            _section = Config.GetConnectionInfo("DatabaseConnection");
            if (dbtype == DbType.Tsql)
            {
                Value = _section.MssqlConnection;
            }
        }
    }
}
