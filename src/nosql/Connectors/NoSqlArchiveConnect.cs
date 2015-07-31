using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NoSql
{
	public class NoSqlArchiveConnect:NoSqlDefaultConnect
	{
        public NoSqlArchiveConnect() : base(ConfigurationManager.ConnectionStrings[Constants.NoSqlArchiveConnectionString].ConnectionString) { }
	}
}
