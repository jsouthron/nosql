using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NoSql.Identity;

namespace NoSql
{
    public class NoSqlRepository 
    {
        protected IMongoIdentity _user;
        protected INoSqlConnect _connection;
        protected INoSqlArchive _noSqlArchive;
        protected double __schema_current;

        public NoSqlRepository(IMongoIdentity user, double schema) 
        { 
            _user = user;
            __schema_current = schema;
            _connection = new NoSqlDefaultConnect();
            _noSqlArchive = new NoSqlArchiver(_connection);
        }
        
        public MongoCursor<TEntity> FindAs<TEntity>(IMongoQuery query)
        {
            return _connection.GetCurrentCollection().FindAs<TEntity>(query);
        }

        public MongoCursor<BsonDocument> LoadCursor(IMongoQuery query)
        {
            return _connection.GetCurrentCollection().FindAs<BsonDocument>(query);
        }

        public BsonDocument Insert(object obj)
        {
            var doc = obj.ReflectedBsonDocument()
                .SetCurrentSchema(__schema_current)
                .AddBsonIdString();

            return doc;
        }

        public bool InsertAs<TEntity>(TEntity obj)
        {
            return _connection.GetCurrentCollection().Insert<TEntity>(obj).Ok;
        }

        public IEnumerable<BsonDocument> InsertBatch(IEnumerable<object> items)
        {
            var docs = items.Select(obj =>
            {
                return obj.ReflectedBsonDocument()
                        .SetCurrentSchema(__schema_current)
                        .AddBsonIdString();
            });
            
            _connection.GetCurrentCollection().InsertBatch(docs);

            return docs;
        }

        public int InsertBatchAs<TEntity>(IEnumerable<TEntity> items)
        {
            int rows_affected = 0;
            items.ToList().ForEach(item => {
                var result = _connection.GetCurrentCollection().Insert<TEntity>(item).Ok;
                if (result) rows_affected++;
            });

            return rows_affected;
        }

        public int SaveAllAs<TEntity>(IEnumerable<TEntity> items)
        {
            int rows_affected = 0;
            items.ToList().ForEach(item =>
            {
                var result = _connection.GetCurrentCollection().Save<TEntity>(item).Ok;
                if (result) rows_affected++;
            });

            return rows_affected;
        }

        public BsonDocument FindAndModify(object obj, IMongoQuery query)
        {
            var id = obj.ExtractSafeString("Id");
            var updates = obj.AttachUpdateMetaData(_user, __schema_current);

            var doc = _connection
                .GetCurrentCollection()
                .FindAndModify(
                    QueryConstants.ById(id, query),
                    SortBy.Null,
                    MongoDB.Driver.Builders.Update.Combine(updates),
                    true, false)
                .ModifiedDocument;

            return doc;

            throw new NotImplementedException();
        }

        public bool Update(IMongoUpdate update, IMongoQuery query)
        {
            return _connection
                .GetCurrentCollection()
                .Update(query, update).Ok;
        }

        public bool Remove(IMongoQuery query)
        {
            throw new NotImplementedException();
        }

        public NoSqlRepository ChangeDatabase(string database, string collection)
        {
            _connection.ChangeDatabase(database, collection);
            return this;
        }
    
        public INoSqlConnect GetCurrentConnection()
        {
            return _connection;
        }
    }
}
