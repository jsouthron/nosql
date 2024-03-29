﻿namespace nosql.Aggregation
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Connectors;
    using Interfaces;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Builders;
    using NoSql;

    public class NoSqlAggregator : INoSqlAggregate
    {
        private readonly INoSqlConnect _connection;

        public NoSqlAggregator()
        {
            _connection = new NoSqlDefaultConnect();
        }

        public NoSqlAggregator(string database, string collection)
        {
            _connection = new NoSqlDefaultConnect();
            _connection.ChangeDatabase(database, collection);
        }

        public NoSqlAggregator(INoSqlConnect connection)
        {
            _connection = connection;
            if (_connection == null)
                throw new ArgumentNullException("Connection must not be null");

        }

        public INoSqlConnect GetCurrentConnection() { return _connection; }

        public NoSqlAggregator ChangeCollection(string collectionName)
        {
            _connection.ChangeCollection(collectionName);
            return this;
        }

        public NoSqlAggregator ChangeDatabase(string databaseName, string collectionName = null)
        {
            _connection.ChangeDatabase(databaseName, collectionName);
            return this;
        } 

        public long Count() { return _connection.GetCurrentCollection().Count(); }
        public long Count(IMongoQuery query) { return _connection.GetCurrentCollection().Count(query); }
        public IEnumerable<BsonValue> Distinct(string key) { return _connection.GetCurrentCollection().Distinct(key); }
        public IEnumerable<BsonValue> Distinct(string key, IMongoQuery query) { return _connection.GetCurrentCollection().Distinct(key, query); }
        public IEnumerable<TResult> Distinct<TResult>(string key) { return _connection.GetCurrentCollection().Distinct<TResult>(key); }
        public IEnumerable<TResult> Distinct<TResult>(string key, IMongoQuery query) { return _connection.GetCurrentCollection().Distinct<TResult>(key, query); }

        public IEnumerable<BsonDocument> Aggregate(params BsonDocument[] operations)
        {
            return _connection.GetCurrentCollection().Aggregate(operations).ResultDocuments;
        }

        public IEnumerable<BsonDocument> Aggregate(NoSqlPipeline pipeline)
        {
            return _connection.GetCurrentCollection().Aggregate(pipeline.Pipeline.ToArray()).ResultDocuments;
        }

        public IEnumerable<BsonDocument> Aggregate(NoSqlPipeline pipeline, Func<BsonDocument, BsonDocument> filter)
        {
            return _connection.GetCurrentCollection().Aggregate(pipeline.Pipeline.ToArray()).ResultDocuments;
        }

        public IEnumerable<IDictionary> AsDictionary(NoSqlPipeline pipeline)
        {
            return _connection.GetCurrentCollection().Aggregate(pipeline.Pipeline.ToArray()).ResultDocuments.Select(x => x.ToDictionary());
        } 


        public IEnumerable<BsonDocument> MapReduce(string map, string reduce)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResults();
        }

        public IEnumerable<TResult> MapReduce<TResult>(string map, string reduce)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResultsAs<TResult>();
        }

        public IEnumerable<BsonDocument> MapReduce(string map, string reduce, IMongoQuery query)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);
            options.SetQuery(query);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResults();
        }

        public IEnumerable<TResult> MapReduce<TResult>(string map, string reduce, IMongoQuery query)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetOutput(MapReduceOutput.Inline);
            options.SetQuery(query);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResultsAs<TResult>();
        } 

        public IEnumerable<BsonDocument> MapReduce(string map, string reduce, string finalize)
        {
            var options = new MapReduceOptionsBuilder();
                options.SetFinalize(finalize);
                options.SetOutput(MapReduceOutput.Inline);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResults();
        }

        public IEnumerable<TResult> MapReduce<TResult>(string map, string reduce, string finalize)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetFinalize(finalize);
            options.SetOutput(MapReduceOutput.Inline);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResultsAs<TResult>();
        }

        public IEnumerable<BsonDocument> MapReduce(string map, string reduce, string finalize, IMongoQuery query)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetFinalize(finalize);
            options.SetOutput(MapReduceOutput.Inline);
            options.SetQuery(query);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResults();
        }

        public IEnumerable<TResult> MapReduce<TResult>(string map, string reduce, string finalize, IMongoQuery query)
        {
            var options = new MapReduceOptionsBuilder();
            options.SetFinalize(finalize);
            options.SetOutput(MapReduceOutput.Inline);
            options.SetQuery(query);

            return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResultsAs<TResult>();
        }

        public IEnumerable<BsonDocument> MapReduce(string map, string reduce, IMongoMapReduceOptions options) { return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResults(); }
        public IEnumerable<TResult> MapReduce<TResult>(string map, string reduce, IMongoMapReduceOptions options) { return _connection.GetCurrentCollection().MapReduce(map, reduce, options).GetResultsAs<TResult>(); }

    }
}
