namespace nosql.Entity
{
    using System;
    using System.Collections.Generic;
    using Extensions;
    using MongoDB.Bson;
    using NoSql;

    public class MongoEntityComparer : IEqualityComparer<BsonDocument>
    {
        private readonly List<string> _keys;

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