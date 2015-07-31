using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using NoSql.Identity;

namespace NoSql
{
    public static class NoSqlExtensions
    {
        public static BsonDocument ReflectedBsonDocument(this object obj)
        {
            BsonDocument doc = new BsonDocument();

            obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => prop.GetValue(obj, null) != null)
                .ToList()
                .ForEach(prop =>
                {
                    doc.Add(prop.Name, BsonValue.Create(prop.GetValue(obj, null)));
                });

            return doc;
        }

        public static BsonDocument SetCurrentSchema(this BsonDocument doc, double schema)
        {
            doc["__schemaVersion"] = schema;
            return doc;
        }

        public static BsonDocument AddBsonIdString(this BsonDocument doc)
        {
            doc["_id"] = ObjectId.GenerateNewId().ToString();
            doc.Remove("Id");

            return doc;
        }

        public static List<UpdateBuilder> AttachUpdateMetaData(this BsonDocument doc, List<UpdateBuilder> builders, IMongoIdentity user)
        {
            builders = builders == null ? new List<UpdateBuilder>() : builders;
            foreach (var name in doc.Names)
            {
                if (name != "Id")
                    builders.Add(Update.Set(name, doc.GetValue(name)));
            }

            builders.Add(Update.Set("LastUpdatedBy", user.GetDisplayName()));
            builders.Add(Update.Set("LastModifiedOn", DateTime.Now));

            return builders;
        }

        public static List<UpdateBuilder> AttachUpdateMetaData(this object obj, IMongoIdentity user, double schema)
        {
            var builders = new List<UpdateBuilder>();

            obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetValue(obj, null) != null)
                .ToList()
                .ForEach(p =>
                {
                    BsonValue val = BsonValue.Create(p.GetValue(obj, null));
                    builders.Add(Update.Set(p.Name, val));
                });

            builders.Add(Update.Set("__schemaVersion", schema));
            builders.Add(Update.Set("LastUpdatedBy", user.GetDisplayName()));
            builders.Add(Update.Set("LastModifiedOn", DateTime.Now));

            return builders;
        }

        public static BsonDocument AttachCreationMetaData(this BsonDocument doc, IMongoIdentity user)
        {
            doc["OwnerId"] = user.GetOwnerId();
            doc["CreatedOn"] = DateTime.Now;
            doc["LastModifiedOn"] = DateTime.Now;
            doc["LastUpdatedBy"] = user.GetOwnerId();
            doc["UTCOffset"] = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString();

            return doc;
        }

        public static BsonValue GetBsonValue(this object value)
        {
            try
            {
                return BsonValue.Create(value);
            }
            catch
            {
                var message = string.Format(".NET type {0} cannot be mapped to a BsonValue", value.GetType().FullName);
                throw new ArgumentException(message);
            }
        }

        public static BsonDocument ConvertToMonthYear(this BsonDocument doc, string field)
        {
            if (doc.Contains(field))
            {
                doc[field] = doc[field].AsInt32.AsMonthYear();
            }

            return doc;
        }
    }

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

            docs.Take(limit).ToList().ForEach(doc =>
            {
                doc.ToConsole();
            });
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

    public static class QueryConstants
    {
        public static IMongoQuery GeoQuery(MongoLocation loc, double prox)
        {
            if (prox == 0) throw new ArgumentException("Proximity can not be zero");
            prox = prox / 69;
            return Query.Near("loc", loc.Longitude, loc.Latitude, prox);
        }

        public static IMongoQuery GeoQuery(MongoLocation loc, double prox, IMongoQuery query)
        {
            var locQuery = QueryConstants.GeoQuery(loc, prox);

            var q = new QueryDocument();
            q.AddRange(BsonDocument.Parse(locQuery.ToString()));
            if (query != null) q.AddRange(BsonDocument.Parse(query.ToString()));

            return q;
        }

        public static IMongoQuery OwnerOnly(long userId)
        {
            return Query.EQ("OwnerId", userId);
        }

        public static IMongoQuery OwnerOnly(long userId, IMongoQuery query)
        {
            return Query.And(Query.EQ("OwnerId", userId), query);
        }

        public static IMongoQuery OwnerOnly(string display_name)
        {
            return Query.EQ("UserGuid", display_name.ToUpper());
        }

        public static IMongoQuery OwnerOnly(string display_name, IMongoQuery query)
        {
            return Query.And(Query.EQ("UserGuid", display_name.ToUpper()), query);
        }

        public static IMongoQuery GetAll()
        {
            return Query.Exists("_id");
        }

        public static IMongoQuery ById(string id)
        {
            if (id == null) id = String.Empty;

            return Query.EQ("_id", id);
        }

        public static IMongoQuery ById(string id, IMongoQuery query)
        {
            return (Query.And(Query.EQ("_id", id), query));
        }

        public static IMongoQuery NotExpired
        {
            get
            {
                return (Query.And(
                  Query.Or(
                      Query.NotExists("StartDate"),
                      Query.EQ("StartDate", BsonNull.Value),
                      Query.LTE("StartDate", DateTime.Now)),
                  Query.Or(
                      Query.NotExists("EndDate"),
                      Query.EQ("EndDate", BsonNull.Value),
                      Query.GTE("EndDate", DateTime.Now))));
            }
        }

        public static IMongoQuery ActiveByDate(string startField, string endField)
        {
            return (Query.And(
                    Query.Or(
                        Query.NotExists(startField),
                        Query.EQ(startField, BsonNull.Value),
                        Query.LTE(startField, DateTime.Now)),
                    Query.Or(
                        Query.NotExists(endField),
                        Query.EQ(endField, BsonNull.Value),
                        Query.GTE(endField, DateTime.Now))));
        }

        public static IMongoQuery IsPartner
        {
            get { return Query.EQ("IsPartner", true); }
        }


        public static IMongoQuery IsGroup
        {
            get { return Query.EQ("IsGroup", true); }
        }

        public static IMongoQuery IsActive
        {
            get { return Query.EQ("IsActive", true); }
        }
    }

    public static class UpdateConstants
    {
        public static UpdateBuilder Increment(string field, int incrementor = 1)
        {
            return Update.Inc(field, incrementor);
        }

        public static UpdateBuilder TogglerActivation(bool isActive)
        {
            return Update.Set("IsActive", isActive);
        }

        public static UpdateBuilder TogglerIndexing(bool isIndexed)
        {
            return Update.Set("IndexMe", isIndexed);
        }

        public static UpdateBuilder UpdateByField(string field, object value)
        {
            return Update.Set(field, value.GetBsonValue());
        }
    }
}
