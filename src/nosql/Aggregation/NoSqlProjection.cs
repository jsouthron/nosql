namespace nosql.Aggregation
{
    using System.Collections.Generic;
    using System.Linq;
    using MongoDB.Bson;

    public class NoSqlProjection
    {
        private readonly List<string> _includefields;
        private readonly List<string> _removefields;
        private readonly IDictionary<string, object> _computedfields;
        private readonly List<BsonElement> _renamefields;
        private readonly List<BsonElement> _insertfields;
        private readonly BsonDocument _projection;

        public NoSqlProjection()
        {
            _includefields = new List<string>();
            _removefields = new List<string>();
            _computedfields = new Dictionary<string, object>();
            _renamefields = new List<BsonElement>();
            _insertfields = new List<BsonElement>();
            _projection = new BsonDocument();
        }

        public NoSqlProjection(params string[] fields)
        {
            _includefields = new List<string>();
            _includefields.AddRange(fields);

            _removefields = new List<string>();
            _computedfields = new Dictionary<string, object>();
            _renamefields = new List<BsonElement>();
            _insertfields = new List<BsonElement>();
            _projection = new BsonDocument();
        }

        public IEnumerable<string> GetFields()
        {
            return _projection.Select(elem => elem.Name).Where(rem => !_removefields.Contains(rem));
        }

        public NoSqlProjection Include(params string[] field)
        {
            _includefields.AddRange(field);
            return this;
        }

        public NoSqlProjection Remove(params string[] field)
        {
            _removefields.AddRange(field);
            return this;
        }

        public NoSqlProjection InsertComputed(KeyValuePair<string, object> computed)
        {
            _computedfields.Add(computed);
            return this;
        }

        public NoSqlProjection Rename(string from, string to)
        {
            _renamefields.Add(new BsonElement(to, NoSqlField.Create(from)));
            _removefields.Add(from);
            return this;
        }

        public NoSqlProjection Copy(string from, string to)
        {
            _renamefields.Add(new BsonElement(to, NoSqlField.Create(from)));
            return this;
        }

        public NoSqlProjection Insert(string key, object value)
        {
            _insertfields.Add(new BsonElement(key, BsonValue.Create(value)));
            return this;
        }

        public BsonDocument GetProjection()
        {
            _projection.AddRange(_computedfields);
            _removefields.ForEach(x => _projection.Add(new BsonElement(x, 0)));
            _includefields.ForEach(x => _projection.Add(new BsonElement(x, 1)));
            _insertfields.ForEach(x => _projection.Add(x));
            _projection.AddRange(_renamefields);

            return _projection;
        }
    }
}
