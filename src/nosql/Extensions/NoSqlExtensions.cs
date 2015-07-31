namespace nosql.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MongoDB.Bson;
    using MongoDB.Driver.Builders;
    using NoSql;
    using NoSql.Identity;

    public static class NoSqlExtensions
    {
        public static BsonDocument ReflectedBsonDocument(this object obj)
        {
            var doc = new BsonDocument();

            obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => prop.GetValue(obj, null) != null)
                .ToList()
                .ForEach(prop => doc.Add(prop.Name, BsonValue.Create(prop.GetValue(obj, null))));

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
            builders = builders ?? new List<UpdateBuilder>();
            builders.AddRange(from name in doc.Names where name != "Id" select Update.Set(name, doc.GetValue(name)));

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
                    var val = BsonValue.Create(p.GetValue(obj, null));
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
}
