using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

namespace NoSql
{
    public class NoSqlArchiver:NoSqlRemover, INoSqlArchive
    {
        protected INoSqlConnect _originServer;
        protected INoSqlConnect _archiveServer;

        public NoSqlArchiver(INoSqlConnect originServer) : this(originServer, new NoSqlArchiveConnect()) { }

        public NoSqlArchiver(INoSqlConnect originServer, INoSqlConnect archiveServer):base(originServer)
        {
            _originServer = originServer;
            _archiveServer = archiveServer;
            if (_originServer == null || _archiveServer == null)
                throw new ArgumentNullException("Servers must not be null");
        }

        public bool Archive(IMongoQuery query, long userId)
        {
            if (query == null)
                throw new ArgumentNullException("Query must not be null");
            var collection = _originServer.GetCurrentCollection();
            _archiveServer.ChangeDatabase(collection.Database.Name, collection.Name);

            var result = false;
            var document = _originServer.GetCurrentCollection().FindAndRemove(query, SortBy.Null).ModifiedDocument;
            if (document != null)
            {
                document["DeletedOn"] = DateTime.Now;
                document["DeletedBy"] = userId;
                _archiveServer.GetCurrentCollection().Save(document);
            }
            return result;
        }
    }
}
