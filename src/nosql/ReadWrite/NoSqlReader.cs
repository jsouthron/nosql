using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using MongoDB.Driver;
using MongoDB.Bson;

namespace NoSql
{
    public class NoSqlReader : INoSqlRead
    {
        private INoSqlConnect _connection;

        public NoSqlReader()
        {
            _connection = new NoSqlDefaultConnect();
        }

        public NoSqlReader(string database, string connection)
        {
            _connection = new NoSqlDefaultConnect();
            _connection.ChangeDatabase(database, connection);
        }

        public NoSqlReader(INoSqlConnect connection)
        {
            _connection = connection;
            if (_connection == null)
                throw new ArgumentNullException("Connection must not be null");
        }

        public MongoCursor<TEntity> FindAs<TEntity>(IMongoQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("Query must not be null");

            return _connection.GetCurrentCollection().FindAs<TEntity>(query);
        }

        public MongoCursor<BsonDocument> LoadCursor(IMongoQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("Query must not be null");

            return _connection.GetCurrentCollection().FindAs<BsonDocument>(query);
        }

        public IEnumerable<BsonDocument> LoadDoc(IMongoQuery query)
        {
            return LoadCursor(query).Select(x => x);
        }

        public NoSqlReader ChangeDatabase(string database, string collection)
        {
            GetCurrentConnection().ChangeDatabase(database, collection);
            return this;
        }

        public INoSqlConnect GetCurrentConnection()
        {
            return _connection;
        }

        public long Count()
        {
            return _connection.GetCurrentCollection().Count();
        }

        public long Count(IMongoQuery query)
        {
            return _connection.GetCurrentCollection().Count(query);
        }

    }
}
