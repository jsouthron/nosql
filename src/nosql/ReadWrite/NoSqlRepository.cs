namespace nosql.ReadWrite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
    using Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NoSql;
    using nosql.Connectors;
    using NoSql.Identity;

    public class NoSqlRepository 
    {
        protected IMongoIdentity User;
        protected INoSqlConnect Connection;
        protected INoSqlArchive NoSqlArchive;
        protected double SchemaCurrent;

        public NoSqlRepository(IMongoIdentity user, double schema) 
        { 
            User = user;
            SchemaCurrent = schema;
            Connection = new NoSqlDefaultConnect();
            NoSqlArchive = new NoSqlArchiver(Connection);
        }
        
        public MongoCursor<TEntity> FindAs<TEntity>(IMongoQuery query)
        {
            return Connection.GetCurrentCollection().FindAs<TEntity>(query);
        }

        public MongoCursor<BsonDocument> LoadCursor(IMongoQuery query)
        {
            return Connection.GetCurrentCollection().FindAs<BsonDocument>(query);
        }

        public BsonDocument Insert(object obj)
        {
            var doc = obj.ReflectedBsonDocument()
                .SetCurrentSchema(SchemaCurrent)
                .AddBsonIdString();

            return doc;
        }

        public bool InsertAs<TEntity>(TEntity obj)
        {
            return Connection.GetCurrentCollection().Insert<TEntity>(obj).Ok;
        }

        public IEnumerable<BsonDocument> InsertBatch(IEnumerable<object> items)
        {
            var docs = items.Select(obj => obj.ReflectedBsonDocument()
                .SetCurrentSchema(SchemaCurrent)
                .AddBsonIdString());
            
            Connection.GetCurrentCollection().InsertBatch(docs);

            return docs;
        }

        public int InsertBatchAs<TEntity>(IEnumerable<TEntity> items)
        {
            var rowsAffected = 0;
            items.ToList().ForEach(item => {
                var result = Connection.GetCurrentCollection().Insert<TEntity>(item).Ok;
                if (result) rowsAffected++;
            });

            return rowsAffected;
        }

        public int SaveAllAs<TEntity>(IEnumerable<TEntity> items)
        {
            var rowsAffected = 0;
            items.ToList().ForEach(item =>
            {
                var result = Connection.GetCurrentCollection().Save<TEntity>(item).Ok;
                if (result) rowsAffected++;
            });

            return rowsAffected;
        }

        public BsonDocument FindAndModify(object obj, IMongoQuery query)
        {
            var id = obj.ExtractSafeString("Id");
            var updates = obj.AttachUpdateMetaData(User, SchemaCurrent);

            var doc = Connection
                .GetCurrentCollection()
                .FindAndModify(
                    QueryConstants.ById(id, query),
                    SortBy.Null,
                    MongoDB.Driver.Builders.Update.Combine(updates),
                    true, false)
                .ModifiedDocument;

            return doc;
        }

        public bool Update(IMongoUpdate update, IMongoQuery query)
        {
            return Connection
                .GetCurrentCollection()
                .Update(query, update).Ok;
        }

        public bool Remove(IMongoQuery query)
        {
            throw new NotImplementedException();
        }

        public NoSqlRepository ChangeDatabase(string database, string collection)
        {
            Connection.ChangeDatabase(database, collection);
            return this;
        }
    
        public INoSqlConnect GetCurrentConnection()
        {
            return Connection;
        }
    }
}
