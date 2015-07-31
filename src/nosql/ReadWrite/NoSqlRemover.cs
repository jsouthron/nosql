using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace NoSql
{
    public class NoSqlRemover:INoSqlRemove
    {
        private INoSqlConnect _connection;
        public NoSqlRemover(INoSqlConnect connection)
        {
            _connection = connection;
            if (_connection == null)
                throw new ArgumentNullException("Collection must not be null");
        }

        public bool Remove(IMongoQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("Collection must not be null");

            return _connection.GetCurrentCollection().Remove(query).Ok;
        }

        public INoSqlConnect GetCurrentConnection()
        {
            return _connection;
        }
    }
}
