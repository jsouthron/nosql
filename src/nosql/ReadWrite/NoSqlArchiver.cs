namespace nosql.ReadWrite
{
    using System;
    using Interfaces;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NoSql;
    using nosql.Connectors;

    public class NoSqlArchiver:NoSqlRemover, INoSqlArchive
    {
        protected INoSqlConnect OriginServer;
        protected INoSqlConnect ArchiveServer;

        public NoSqlArchiver(INoSqlConnect originServer) : this(originServer, new NoSqlArchiveConnect()) { }

        public NoSqlArchiver(INoSqlConnect originServer, INoSqlConnect archiveServer):base(originServer)
        {
            OriginServer = originServer;
            ArchiveServer = archiveServer;
            if (OriginServer == null || ArchiveServer == null)
                throw new ArgumentNullException("Servers must not be null");
        }

        public bool Archive(IMongoQuery query, long userId)
        {
            if (query == null)
                throw new ArgumentNullException("Query must not be null");
            var collection = OriginServer.GetCurrentCollection();
            ArchiveServer.ChangeDatabase(collection.Database.Name, collection.Name);

            var result = false;
            var document = OriginServer.GetCurrentCollection().FindAndRemove(query, SortBy.Null).ModifiedDocument;
            if (document != null)
            {
                document["DeletedOn"] = DateTime.Now;
                document["DeletedBy"] = userId;
                ArchiveServer.GetCurrentCollection().Save(document);
            }
            return result;
        }
    }
}
