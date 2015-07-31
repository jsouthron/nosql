using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace NoSql
{
    public interface INoSqlRead
    {
        MongoCursor<BsonDocument> LoadCursor(IMongoQuery query);
        IEnumerable<BsonDocument> LoadDoc(IMongoQuery query);
        INoSqlConnect GetCurrentConnection();
    }

    public interface INoSqlWrite
    {
        BsonDocument NoSqlUpdateDoc(UpdateBuilder update, IMongoQuery query);
        BsonDocument NoSqlUpdateDoc(List<UpdateBuilder> updates, IMongoQuery query);
        BsonDocument NoSQLSaveDoc(BsonDocument doc);
        INoSqlConnect GetCurrentConnection();        
    }

    public interface INoSqlRemove
    {
        bool Remove(IMongoQuery query);
        INoSqlConnect GetCurrentConnection();
    }

    public interface INoSqlArchive : INoSqlRemove
    {
        bool Archive(IMongoQuery query, long userId);
    }

    public interface INoSqlConnect
    {
        INoSqlConnect ChangeCollection(string collectionName);
        INoSqlConnect ChangeDatabase(string databaseName, string collectionName = null);
        MongoCollection GetCurrentCollection();
    }

    public interface INoSqlAggregate
    {
        IEnumerable<BsonDocument> Aggregate(params BsonDocument[] operations);
    }

    public class NoSqlComparor : IEqualityComparer
    {

        bool IEqualityComparer.Equals(object x, object y)
        {
            throw new NotImplementedException();
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
