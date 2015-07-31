using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;

namespace BenefithubPortal.Lib.NoSql.Mapping
{
    //public class MongoMultiMap<TEntity> where TEntity : class
    //{
    //    public List<MongoEntity<TEntity>> Mappings { get; set; }

    //    public MongoMultiMap()
    //    {
    //        Mappings = new List<MongoEntity<TEntity>>();
    //    }

    //    public MongoMultiMap(List<MongoEntity<TEntity>> mappings)
    //    {
    //        Mappings = mappings;
    //    }

    //    public IEnumerable<TEntity> GetAll(NoSqlReader nosql, UserContextModel user)
    //    {
    //        var result = new List<TEntity>();

    //        Mappings.ForEach(map =>
    //        {
    //            //nosql.GetCurrentConnection().ChangeDatabase(map.Database, map.Collection);
    //            //result.AddRange(nosql.LoadCursor(map.query).Select(doc => (TEntity)map.Mapper(doc, user)));
    //        });

    //        return result;
    //    }

    //    public void AddMapping(MongoEntity<TEntity> mapping)
    //    {
    //        Mappings.Add(mapping);
    //    }

    //    public void AddMapping(
    //        string db,
    //        string coll,
    //        IMongoQuery queryIn,
    //        Func<BsonDocument, UserContextModel, TEntity> mapFn)
    //    {
    //        //Mappings.Add(new MongoEntity<TEntity> { Database = db, Collection = coll, query = queryIn, Mapper = mapFn });
    //    }

    //    //delegate T Generator<T>();
    //    //IEnumerable<T> Generate<T>(int number, Generator<T> factory)
    //    //{
    //    //    for (int i = 0; i < number; i++)
    //    //        yield return factory();
    //    //}


    //    //delegate Tout MergeOne<Tin, Tout>(Tin a, Tin b);

    //    //IEnumerable<Tout> Merge<Tin, Tout>(IEnumerable<Tin> first,
    //    //    IEnumerable<Tin> second, MergeOne<Tin, Tout> factory)
    //    //{
    //    //    IEnumerator<Tin> firstIter = first.GetEnumerator();
    //    //    IEnumerator<Tin> secondIter = second.GetEnumerator();
    //    //    while (firstIter.MoveNext() && secondIter.MoveNext())
    //    //        yield return factory(firstIter.Current, secondIter.Current);
    //    //}
    //}
}
