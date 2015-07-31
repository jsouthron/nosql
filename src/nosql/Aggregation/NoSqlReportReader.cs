namespace nosql.Aggregation
{
    using Interfaces;
    using NoSql;
    using NoSql.Identity;

    public class NoSqlReportAggregator : NoSqlAggregator
    {
        protected IMongoIdentity User;
        protected INoSqlConnect Connector;

        public NoSqlReportAggregator(INoSqlConnect connector, IMongoIdentity user) : base(connector) 
        { 
            User = user; 
            Connector = connector;  
        }
    
    }
}
