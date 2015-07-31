namespace nosql.Writers
{
    using System.Collections.Generic;

    public class MongoWorksheetCollection 
    {
        public List<MongoWorksheet> Worksheets { get; set; }

        public MongoWorksheetCollection()
        {
            Worksheets = new List<MongoWorksheet>();
        }

        public MongoWorksheetCollection(params MongoWorksheet[] items)
        {
            Worksheets = new List<MongoWorksheet>();
            Worksheets.AddRange(items);
        }

        public MongoWorksheetCollection Add(MongoWorksheet item)
        {
            Worksheets.Add(item);
            return this;
        }

        public MongoWorksheetCollection Add(params MongoWorksheet[] items)
        {
            Worksheets.AddRange(items);
            return this;
        }
    }
}