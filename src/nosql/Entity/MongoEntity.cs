using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NoSql.Aggregate;
using NoSql.Identity;

namespace NoSql.WebFrontEnd.Repository
{
    public interface IMongoDbContext
    {
        IEnumerable<BsonDocument> GetDocuments();
        void SetQuery(IMongoQuery query);
    }

    public interface IMongoEntityMap
    {
        T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class;
    }

    public class MongoEntity<TEntity> : IMongoDbContext, IMongoEntityMap where TEntity : class, new()
    {
        private IMongoQuery _query;

        private string _database;
        private string _collection;
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
            return (TEntity) _mapper(doc, user);
        }

        public T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class
        {
            return Map(doc, user) as T;
        }

        public IEnumerable<TEntity> MapCollection(IMongoIdentity user)
        {
            return GetDocuments().Select(doc => (TEntity) _mapper(doc, user));
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

    public class MongoAggregateEntity<TEntity> : IMongoDbContext, IMongoEntityMap
    {
        private Func<BsonDocument, IMongoIdentity, TEntity> _mapper;
        private NoSqlPipeline _pipeline;

        public MongoAggregateEntity() { }

        public MongoAggregateEntity(
            NoSqlPipeline pipeline,
            Func<BsonDocument, IMongoIdentity, TEntity> mapFn)
        {
            _pipeline = pipeline;
            _mapper = mapFn;
        }

        public MongoAggregateEntity<TEntity> AddMap(
            NoSqlPipeline pipeline,
            Func<BsonDocument, IMongoIdentity, TEntity> mapFn)
        {
            _pipeline = pipeline;
            _mapper = mapFn;
            return this;
        }

        public TEntity Map(BsonDocument doc, IMongoIdentity user)
        {
            return (TEntity)_mapper(doc, user);
        }

        public MongoAggregateEntity<TEntity> AddPipeline(NoSqlPipeline pipeline)
        {
            _pipeline = pipeline;
            return this;
        }

        public IEnumerable<TEntity> MapCollection(IMongoIdentity user)
        {
            return GetDocuments().Select(doc => (TEntity)_mapper(doc, user));
        }

        public IEnumerable<BsonDocument> GetDocuments()
        {
            return new NoSqlAggregator().Aggregate(_pipeline);
        }

        public T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class
        {
            return Map(doc, user) as T;
        }

        public void SetQuery(IMongoQuery pipeline)
        {
            _pipeline = pipeline as NoSqlPipeline;
        }
    }

    public class MongoCompositeEntity<TEntity> : IMongoDbContext, IMongoEntityMap where TEntity : class
    {
        Func<BsonDocument, IMongoIdentity, TEntity> _mapper;
        IMongoDbContext _left;
        IMongoDbContext _right;

        public MongoCompositeEntity() { }

        public MongoCompositeEntity(Func<BsonDocument, IMongoIdentity, TEntity> mapFn)
        {
            _mapper = mapFn;
        }

        public MongoCompositeEntity(
            IMongoDbContext left,
            IMongoDbContext right,
            Func<BsonDocument, IMongoIdentity, TEntity> mapFn)
        {
            _left = left;
            _right = right;
            _mapper = mapFn;
        }

        public TEntity Map(BsonDocument doc, IMongoIdentity user)
        {
            return (TEntity)_mapper(doc, user);
        }

        public IEnumerable<TEntity> MapCollection(IMongoIdentity user)
        {
            return GetDocuments().Select(doc => (TEntity)_mapper(doc, user));
        }

        public IEnumerable<BsonDocument> GetDocuments()
        {
            var left = _left.GetDocuments();

            _right.SetQuery(Query.And(
                            Query.In("_id", left.Select(doc => doc["_id"])),
                            Query.EQ("IsActive", true)));

            var docs = left.Join(_right.GetDocuments(),
                lkey => lkey["_id"],
                rkey => rkey["_id"],
                (lkey, rkey) => { return lkey.Merge(rkey, false); });

            return docs;
        }

        public T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class
        {
            return Map(doc, user) as T;
        }


        public void SetQuery(IMongoQuery query)
        {
            throw new NotImplementedException();
        }
    }

    public class MongoEntityComparer : IEqualityComparer<BsonDocument>
    {
        private List<string> _keys;

        public MongoEntityComparer() 
        {
            _keys = new List<string>() { "_id" };
        }

        public MongoEntityComparer(params string[] keys) 
        {
            _keys = new List<string>(keys);
        }

        public bool Equals(BsonDocument x, BsonDocument y)
        {
            var result = false;
            
            if (Object.ReferenceEquals(x, y))
            {
                return true;
            }

            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            {
                return false;
            }

            if (_keys.Count < 1)
            {
                return false;
            }

            for (int i = 0; i < _keys.Count; i++)
            {
                if (!x.Contains(_keys[i]) || !y.Contains(_keys[i]))
                {
                    break;
                }

                result = x[_keys[i]].AsString == y[_keys[i]].AsString;
            }

            return result;
        }

        public int GetHashCode(BsonDocument doc)
        {
            if (Object.ReferenceEquals(doc, null))
            {
                return 0;
            }

            int hash = doc.GetDocumentId() == null ? 0 : doc.GetDocumentId().GetHashCode();

            return hash;
        }
    }
}
