namespace nosql.ReadWrite
{
    using System;
    using MongoDB.Driver;
    using nosql.Interfaces;

    public class NoSqlRemover:INoSqlRemove
    {
        private readonly INoSqlConnect _connection;
        public NoSqlRemover(INoSqlConnect connection)
        {
            _connection = connection;
            if (_connection == null)
                throw new ArgumentNullException("Collection must not be null");
        }

        public bool Remove(IMongoQuery query)
        {
            if (query == null)
                throw new ArgumentNullException("Collection must not be null");

            return _connection.GetCurrentCollection().Remove(query).Ok;
        }

        public INoSqlConnect GetCurrentConnection()
        {
            return _connection;
        }
    }
}
