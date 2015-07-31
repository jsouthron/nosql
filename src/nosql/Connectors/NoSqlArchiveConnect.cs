namespace nosql.Connectors
{
    using System.Configuration;
    using NoSql;

    public class NoSqlArchiveConnect:NoSqlDefaultConnect
	{
        public NoSqlArchiveConnect() : base(ConfigurationManager.ConnectionStrings[Constants.NoSqlArchiveConnectionString].ConnectionString) { }
	}
}
