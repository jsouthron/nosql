namespace nosql.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NoSql.Identity;

    public class MongoCompositeEntity<TEntity> : IMongoDbContext, IMongoEntityMap where TEntity : class
    {
        readonly Func<BsonDocument, IMongoIdentity, TEntity> _mapper;
        readonly IMongoDbContext _left;
        readonly IMongoDbContext _right;

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
                (lkey, rkey) => lkey.Merge(rkey, false));

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
}