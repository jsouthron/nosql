using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using System.Configuration;

namespace NoSql
{
    public class NoSqlDefaultConnect: INoSqlConnect
    {
        protected MongoCollection _collection;
        protected MongoDatabase _database;
        protected MongoServer _server;

        public NoSqlDefaultConnect() : this(ConfigurationManager.ConnectionStrings[Constants.NoSqlConnectionString].ConnectionString) { }

        public NoSqlDefaultConnect(string connectionString)
        {
            _server = new MongoClient(connectionString).GetServer();
        }

        public NoSqlDefaultConnect(string database, string collection, string connectionString)
        {
            _server = new MongoClient(connectionString).GetServer();
            _database = _server.GetDatabase(database);
            _collection = _database.GetCollection(collection);
        }

        public virtual INoSqlConnect ChangeCollection(string collectionName)
        {
            _collection = _database.GetCollection(collectionName);
            return this;
        }

        public virtual INoSqlConnect ChangeDatabase(string databaseName, string collectionName = null)
        {
            _database = _server.GetDatabase(databaseName);
            _collection = _database.GetCollection(collectionName ?? _collection.Name);
            return this;
        }


        public MongoCollection GetCurrentCollection()
        {
            return _collection;
        }
    }
}
