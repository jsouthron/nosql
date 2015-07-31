using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace NoSql.Identity
{
    public abstract class MongoIdentity : IMongoIdentity
    {
        private object _identity;
        private long _owner_id;
        private string _display_name;
        
        protected MongoIdentity(object obj) 
        {
            var t = obj.GetType();
            var prop = t.GetProperty("Id");
            _identity = obj;

            long.TryParse(prop.GetValue(obj, null).ToSafeString(), out _owner_id);
        }

        public virtual BsonDocument GetIdentity()
        {
            return _identity.ReflectedBsonDocument();
        }

        public virtual long GetOwnerId()
        {
            return _owner_id;
        }

        public virtual bool HasOwnerId()
        {
            return _owner_id > 0;
        }

        public abstract string GetDisplayName();
    }
}
