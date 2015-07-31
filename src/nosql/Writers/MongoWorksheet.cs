namespace nosql.Writers
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public class MongoWorksheet
    {
        public IEnumerable<BsonDocument> Data { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Headers { get; set; }
    }
}