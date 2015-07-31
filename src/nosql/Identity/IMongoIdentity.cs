using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace NoSql.Identity
{
    public interface IMongoIdentity
    {
        BsonDocument GetIdentity();
        long GetOwnerId();
        string GetDisplayName();
    }
}
