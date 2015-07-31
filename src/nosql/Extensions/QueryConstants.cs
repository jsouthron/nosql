namespace nosql.Extensions
{
    using System;
    using Location;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

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

        public static IMongoQuery OwnerOnly(string displayName)
        {
            return Query.EQ("UserGuid", displayName.ToUpper());
        }

        public static IMongoQuery OwnerOnly(string displayName, IMongoQuery query)
        {
            return Query.And(Query.EQ("UserGuid", displayName.ToUpper()), query);
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
}