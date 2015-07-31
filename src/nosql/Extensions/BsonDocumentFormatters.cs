namespace nosql.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;
    using NoSql;
    using NoSql.Identity;

    public static class BsonDocumentFormatters
    {
        public static string GetDocumentId(this BsonDocument doc)
        {
            return doc["_id"].ToString();
        }

        public static string GetString(this BsonDocument doc, string key)
        {
            return doc.Contains(key) ? doc[key].ToString() : string.Empty;
        }

        public static int GetInt32(this BsonDocument doc, string key)
        {
            return doc.Contains(key) ? doc[key].AsInt32 : -1;
        }

        public static long GetInt64(this BsonDocument doc, string key)
        {
            return doc.Contains(key) ? doc[key].AsInt64 : -1;
        }

        public static string GetStringFormat(this BsonDocument doc, string format, string key)
        {
            return string.Format(format, doc.GetString(key));
        }

        public static string GetStringFormat(this BsonDocument doc, string format, string key0, string key1)
        {
            return string.Format(format, doc.GetString(key0), doc.GetString(key1));
        }

        public static string Prefix(this BsonDocument doc, string key, string prefix)
        {
            return doc.Contains(key) ? prefix + doc[key].ToString() : prefix + string.Empty;
        }

        public static string CascadeOnFlags(this BsonDocument doc, string left, string middle, string leftvalue, string middlevalue, string rightvalue)
        {
            if (!doc.Contains(left) || !doc.Contains(middle)) return rightvalue;

            return doc[left].AsBoolean ? leftvalue : doc[middle].AsBoolean ? middlevalue : rightvalue;
        }

        public static string GetFriendlyDate(this BsonDocument doc, string key)
        {
            return doc.Contains(key) ? DateTime.Parse(doc["CreatedOn"].ToString()).ToFriendlyDateString() : string.Empty;
        }

        public static bool IsOwner(this BsonDocument doc, IMongoIdentity user)
        {
            return doc.Contains("OwnerId") && (doc["OwnerId"].AsInt64 == user.GetOwnerId());
        }

        public static bool GetFlag(this BsonDocument doc, string flag)
        {
            return doc.Contains(flag) && doc[flag].AsBoolean;
        }

        public static void ToConsole(this IEnumerable<BsonDocument> docs, int limit = 0)
        {
            if (limit == 0) limit = docs.Count();

            docs.Take(limit).ToList().ForEach(doc => doc.ToConsole());
        }

        public static void ToConsole(this BsonDocument doc)
        {
            Console.WriteLine("{");
            foreach (BsonElement el in doc)
            {
                Console.Write("\t");
                Console.WriteLine(el.Name + ": " + el.Value);
            }
            Console.WriteLine("}");
        }
    }
}