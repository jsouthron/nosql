using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace NoSql.Aggregate
{
    public class NoSqlPipeline
    {
        private bool _fieldsKnown;
        
        public string Database { get; set; }
        public string Collection { get; set; }

        public List<BsonDocument> pipeline { get; set; }
        public List<string> Fields { get; set; }
        public string Explain { get { return string.Join(", ", pipeline).Replace('"', '\''); } }

        public Func<BsonDocument, BsonDocument> PostFilter { get; set; }

        public NoSqlPipeline()
        {
            pipeline = new List<BsonDocument>();
            _fieldsKnown = false;
        }

        public NoSqlPipeline(string db, string coll)
        {
            pipeline = new List<BsonDocument>();
            Database = db;
            Collection = coll;
            _fieldsKnown = false;
        }

        public NoSqlPipeline Match(IMongoQuery query)
        {
            var match = new BsonDocument()
                .Add(new BsonElement("$match", query.ToBsonDocument()));

            pipeline.Add(match);

            return this;
        }

        public NoSqlPipeline Group(string id, params KeyValuePair<string, BsonDocument>[] funcs)
        {
            var id_label = 
                String.IsNullOrEmpty(id) ? 
                BsonNull.Value : 
                BsonValue.Create("$" + id);

            var aggregators = new BsonDocument().Add(new BsonElement("_id", id_label));
            foreach (KeyValuePair<string, BsonDocument> item in funcs)
                aggregators.Add(item.Key, item.Value);

            var group = new BsonDocument{{ "$group", aggregators }};
            pipeline.Add(group);

            return this;
        }

        public NoSqlPipeline Group(List<NoSqlGroupField> fields, params KeyValuePair<string, BsonDocument>[] funcs)
        {
            var groupby = new BsonDocument();
            fields.ForEach(f => groupby.Add(f.ToBsonElement()));

            var aggregators = new BsonDocument().Add(new BsonElement("_id", groupby));
            foreach (KeyValuePair<string, BsonDocument> item in funcs)
                aggregators.Add(item.Key, item.Value);

            var group = new BsonDocument { { "$group", aggregators } };
            pipeline.Add(group);

            return this;
        }

        public NoSqlPipeline Sort(NoSqlAggregateSort order, params string[] fields)
        {
            var doc = new BsonDocument();
            foreach (var field in fields)
            {
                doc.Add(field, order);
            }

            pipeline.Add(new BsonDocument { { "$sort", doc } });
            return this;
        }

        public NoSqlPipeline Sort(params NoSqlSortedField[] fields)
        {
            var doc = new BsonDocument();
            foreach (var field in fields)
            {
                doc.Add(field.Name, field.SortOrder);
            }

            pipeline.Add(new BsonDocument { { "$sort", doc } });
            return this;
        }

        public NoSqlPipeline Sort(string field, NoSqlAggregateSort order = NoSqlAggregateSort.Ascending)
        {
            pipeline.Add(new BsonDocument { { "$sort", new BsonDocument { { field, order } } } });
            return this;
        }

        public NoSqlPipeline Project(NoSqlProjection projection)
        {
            pipeline.Add(new BsonDocument {{ "$project", projection.GetProjection() }});
            Fields = projection.GetFields().ToList();
            _fieldsKnown = true;

            return this;
        }

        public NoSqlPipeline Project(params string[] args)
        {
            var projection = new NoSqlProjection(args);
            pipeline.Add(new BsonDocument { { "$project", projection.GetProjection() } });

            Fields = projection.GetFields().ToList();
            _fieldsKnown = true;

            return this;
        }

        public NoSqlPipeline Limit(int limit)
        {
            pipeline.Add(new BsonDocument {{ "$limit", limit }});
            return this;
        }

        public NoSqlPipeline Skip(int skip)
        {
            pipeline.Add(new BsonDocument { { "$skip", skip } });
            return this;
        }

        public NoSqlPipeline Unwind(string field)
        {
            pipeline.Add(new BsonDocument {{ "$unwind", NoSqlField.Create(field) }});
            return this;
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, params BsonElement[] opts)
        {
            if (pipeline.Count > 0)
            {
                throw new InvalidOperationException("You can only use $geoNear as the first stage of a pipeline.");
            }

            if (location.Length != 2)
            {
                throw new InvalidOperationException("location[] must contain two double values.");
            }

            var doc = new BsonDocument
            {
                { "near", new BsonArray(location) },
                { "distanceField", distanceField }
            };

            doc.AddRange(opts);

            pipeline.Add(new BsonDocument { { "$geoNear", doc } });
            Fields.Add("dist");
            return this;
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, IMongoQuery query)
        {
            return GeoNear(location, distanceField, new BsonElement("query", query.ToBsonDocument()));
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance)
        {
            return GeoNear(location, distanceField, new BsonElement("maxDistance", maxDistance));
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, int num)
        {
            return GeoNear(location, distanceField, new BsonElement("num", num));
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance, int num)
        {
            return GeoNear(location, distanceField, new BsonElement[] 
            { 
                new BsonElement("maxDistance", maxDistance), 
                new BsonElement("num", num) 
            });
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance, int num, IMongoQuery query)
        {
            return GeoNear(location, distanceField, new BsonElement[] 
            { 
                new BsonElement("maxDistance", maxDistance), 
                new BsonElement("num", num), 
                new BsonElement("query", query.ToBsonDocument()) 
            });
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance, int num, IMongoQuery query, string includeDocs)
        {
            return GeoNear(location, distanceField, new BsonElement[] 
            { 
                new BsonElement("maxDistance", maxDistance), 
                new BsonElement("num", num), 
                new BsonElement("query", query.ToBsonDocument()),
                new BsonElement("includeDocs", includeDocs)
            });
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance, int num, IMongoQuery query, bool uniqueDocs)
        {
            return GeoNear(location, distanceField, new BsonElement[] 
            { 
                new BsonElement("maxDistance", maxDistance), 
                new BsonElement("num", num), 
                new BsonElement("query", query.ToBsonDocument()),
                new BsonElement("uniqueDocs", uniqueDocs)
            });
        }

        public NoSqlPipeline GeoNear(double[] location, string distanceField, double maxDistance, int num, IMongoQuery query, string includeDocs, bool uniqueDocs)
        {
            return GeoNear(location, distanceField, new BsonElement[] 
            { 
                new BsonElement("maxDistance", maxDistance), 
                new BsonElement("num", num), 
                new BsonElement("query", query.ToBsonDocument()),
                new BsonElement("includeDocs", includeDocs), 
                new BsonElement("uniqueDocs", uniqueDocs)
            });
        }
    }

    

   

}
