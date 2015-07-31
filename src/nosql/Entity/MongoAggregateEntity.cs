namespace nosql.Entity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Aggregation;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using NoSql.Identity;

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
}