using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace NoSql.Identity
{
    using nosql.Extensions;

    public abstract class MongoIdentity : IMongoIdentity
    {
        private readonly object _identity;
        private readonly long _ownerId;
        private string _displayName;
        
        protected MongoIdentity(object obj, string displayName) 
        {
            var t = obj.GetType();
            var prop = t.GetProperty("Id");
            _identity = obj;
            _displayName = displayName;

            long.TryParse(prop.GetValue(obj, null).ToSafeString(), out _ownerId);
        }

        public virtual BsonDocument GetIdentity()
        {
            return _identity.ReflectedBsonDocument();
        }

        public virtual long GetOwnerId()
        {
            return _ownerId;
        }

        public virtual bool HasOwnerId()
        {
            return _ownerId > 0;
        }

        public abstract string GetDisplayName();
    }
}
