namespace nosql.Entity
{
    using MongoDB.Bson;
    using NoSql.Identity;

    public interface IMongoEntityMap
    {
        T Map<T>(BsonDocument doc, IMongoIdentity user) where T : class;
    }
}