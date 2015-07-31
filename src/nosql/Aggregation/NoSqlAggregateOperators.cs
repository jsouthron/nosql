namespace nosql.Aggregation
{
    using System.Collections.Generic;
    using MongoDB.Bson;

    public class NoSqlGroupOperators
    {
        public static KeyValuePair<string, BsonDocument> AddToSet(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$addToSet", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Push(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$push", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> First(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$first", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Last(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$last", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Min(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$min", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Max(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$max", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Average(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$avg", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Sum(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$sum", NoSqlField.Create(field))); }
        public static KeyValuePair<string, BsonDocument> Count(string label, int value = 1) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$sum", value)); }
        public static KeyValuePair<string, BsonDocument> Count(string label, string field) { return new KeyValuePair<string, BsonDocument>(label, new BsonDocument("$sum", NoSqlField.Create(field))); }
    }

    public class NoSqlBoolean
    {
        public static KeyValuePair<string, object> And(string label, params object[] items) { return new KeyValuePair<string, object>(label, new BsonDocument("$and", new BsonArray(items))); }
        public static KeyValuePair<string, object> Or(string label, params object[] items) { return new KeyValuePair<string, object>(label, new BsonDocument("$or", new BsonArray(items))); }
        public static KeyValuePair<string, object> Not(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$not", BsonValue.Create(value))); }
    }

    public class NoSqlComparison
    {
        public static KeyValuePair<string, object> Compare(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$cmp", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> EQ(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$eq", new BsonArray(new List<object> { left, right }))); }
        public static BsonDocument EQ(object left, object right) { return new BsonDocument("$eq", new BsonArray(new List<object> { left, right })); }
        public static KeyValuePair<string, object> GT(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$gt", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> GTE(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$gte", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> LT(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$lt", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> LTE(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$lte", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> NE(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$ne", new BsonArray(new List<object> { left, right }))); }
    }

    public class NoSqlMath
    {
        public static KeyValuePair<string, object> Add(string label, params object[] items) { return new KeyValuePair<string, object>(label, new BsonDocument("$add", new BsonArray(items))); }
        public static KeyValuePair<string, object> Subtract(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$subtract", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> Multiply(string label, params object[] items) { return new KeyValuePair<string, object>(label, new BsonDocument("$multiply", new BsonArray(items))); }
        public static KeyValuePair<string, object> Divide(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$divide", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> Mod(string label, object left, object right) { return new KeyValuePair<string, object>(label, new BsonDocument("$mod", new BsonArray(new List<object> { left, right }))); }
    }

    public class NoSqlString
    {
        public static KeyValuePair<string, object> Concat(string label, params string[] items) { return new KeyValuePair<string, object>(label, new BsonDocument("$concat", new BsonArray(items))); }
        public static KeyValuePair<string, object> StringCaseCompare(string label, string left, string right) { return new KeyValuePair<string, object>(label, new BsonDocument("$mod", new BsonArray(new List<object> { left, right }))); }
        public static KeyValuePair<string, object> Substr(string label, object value, int start, int length) { return new KeyValuePair<string, object>(label, new BsonDocument("$substr", new BsonArray(new List<object> { BsonValue.Create(value), start, length }))); }
        public static KeyValuePair<string, object> ToLower(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$toLower", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> ToUpper(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$toUpper", BsonValue.Create(value))); }
    }

    public class NoSqlDate
    {
        public static KeyValuePair<string, object> DayOfYear(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$dayOfYear", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> DayOfMonth(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$dayOfMonth", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> DayOfWeek(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$dayOfWeek", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Year(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$year", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Month(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$month", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Week(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$week", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Hour(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$hour", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Minute(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$minute", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> Second(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$second", BsonValue.Create(value))); }
        public static KeyValuePair<string, object> MilliSecond(string label, object value) { return new KeyValuePair<string, object>(label, new BsonDocument("$millisecond", BsonValue.Create(value))); }
    }

    public class NoSqlConditional
    {
        public static KeyValuePair<string, object> InlineCondition(string label, object condition, object trueResult, object falseResult)
        {
            return new KeyValuePair<string, object>(label, new BsonDocument("$cond", new BsonArray(new List<object> { condition, trueResult, falseResult })));
        }

        public static KeyValuePair<string, object> IfNull(string label, object left, object right)
        {
            return new KeyValuePair<string, object>(label, new BsonDocument("$ifNull", new BsonArray(new List<object> { left, right })));
        }
    }

    public static class NoSqlField
    {
        public static string Create(string field) { return "$" + field; }
        public static NoSqlSortedField Create(string field, NoSqlAggregateSort order) { return new NoSqlSortedField(field, order); }
        public static NoSqlSortedField Create(string field, int order) { return new NoSqlSortedField(field, order); }
    }

    public class NoSqlSortedField
    {
        public string Name { get; set; }
        public NoSqlAggregateSort SortOrder { get; set; }

        public NoSqlSortedField() { }

        public NoSqlSortedField(string field, NoSqlAggregateSort order)
        {
            Name = field;
            SortOrder = order;
        }

        public NoSqlSortedField(string field, int order)
        {
            Name = field;
            SortOrder = order > -1 ? NoSqlAggregateSort.Ascending : NoSqlAggregateSort.Descending;
        }
    }

    public enum NoSqlAggregateSort
    {
        Descending = -1,
        Ascending = 1
    }

    public class NoSqlGroupField
    {
        private string _field;
        public string _as;

        public NoSqlGroupField() { }
        public NoSqlGroupField(string field) { _field = field; }

        public NoSqlGroupField As(string label)
        {
            _as = label;
            return this;
        }

        public BsonElement ToBsonElement()
        {
            return new BsonElement(_as, "$" + _field);
        }

        public string Create(string field) { _field = field; return "$" + _field; }
    }
}
