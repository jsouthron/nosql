namespace nosql.Connectors
{
    using System.Configuration;
    using Interfaces;
    using MongoDB.Driver;
    using NoSql;

    public class NoSqlDefaultConnect: INoSqlConnect
    {
        protected MongoCollection Collection;
        protected MongoDatabase Database;
        protected MongoServer Server;

        public NoSqlDefaultConnect() : this(ConfigurationManager.ConnectionStrings[Constants.NoSqlConnectionString].ConnectionString) { }

        public NoSqlDefaultConnect(string connectionString)
        {
            Server = new MongoClient(connectionString).GetServer();
        }

        public NoSqlDefaultConnect(string database, string collection, string connectionString)
        {
            Server = new MongoClient(connectionString).GetServer();
            Database = Server.GetDatabase(database);
            Collection = Database.GetCollection(collection);
        }

        public virtual INoSqlConnect ChangeCollection(string collectionName)
        {
            Collection = Database.GetCollection(collectionName);
            return this;
        }

        public virtual INoSqlConnect ChangeDatabase(string databaseName, string collectionName = null)
        {
            Database = Server.GetDatabase(databaseName);
            Collection = Database.GetCollection(collectionName ?? Collection.Name);
            return this;
        }


        public MongoCollection GetCurrentCollection()
        {
            return Collection;
        }
    }
}
