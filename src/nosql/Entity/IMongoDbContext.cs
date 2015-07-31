namespace nosql.Entity
{
    using System.Collections.Generic;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public interface IMongoDbContext
    {
        IEnumerable<BsonDocument> GetDocuments();
        void SetQuery(IMongoQuery query);
    }
}