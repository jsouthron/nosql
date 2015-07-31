using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using NoSql.Identity;


namespace NoSql.Aggregate
{
    public class NoSqlReportAggregator : NoSqlAggregator
    {
        protected IMongoIdentity _user;
        protected INoSqlConnect _connector;

        public NoSqlReportAggregator(INoSqlConnect connector, IMongoIdentity user) : base(connector) 
        { 
            _user = user; 
            _connector = connector; 
        }
    
    }
}
