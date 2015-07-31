namespace nosql.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using NoSql;
    using NoSql.Identity;
    using ReadWrite;

    public class MongoEntity<TEntity> : IMongoDbContext, IMongoEntityMap where TEntity : class, new()
    {
        private IMongoQuery _query;

        private readonly string _database;
        private readonly string _collection;
        private Func<BsonDocument, IMongoIdentity, TEntity> _mapper;

        public MongoEntity() { }

        public MongoEntity(string db, string coll)
        {
            _database = db; _collection = coll;
        }

        public MongoEntity<TEntity> AddMap(
            IMongoQuery queryIn,
            Func<BsonDocument, IMongoIdentity, TEntity> mapFn)
        {
            _query = queryIn;
            _mapper = mapFn;
            return this;
        }

        public TEntity Map(BsonDocument doc, IMongoIdentity user)
        {
            return _mapper(doc, user);
        }

        public T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class
        {
            return Map(doc, user) as T;
        }

        public IEnumerable<TEntity> MapCollection(IMongoIdentity user)
        {
            return GetDocuments().Select(doc => _mapper(doc, user));
        }

        public IEnumerable<BsonDocument> GetDocuments()
        {
            var reader = new NoSqlReader();
            reader.ChangeDatabase(_database, _collection);

            return reader.LoadCursor(_query);
        }

        public void SetQuery(IMongoQuery query)
        {
            _query = query;
        }

        public TEntity FirstOrDefault(IMongoIdentity user)
        {
            var doc = GetDocuments().ToList().FirstOrDefault();
            return _mapper(doc, user);
        }
    }
}
