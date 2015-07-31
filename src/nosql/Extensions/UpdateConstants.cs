namespace nosql.Extensions
{
    using MongoDB.Driver.Builders;

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