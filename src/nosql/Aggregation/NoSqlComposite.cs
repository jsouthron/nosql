using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace NoSql.Aggregation
{
    public class NoSqlComposite
    {
        public NoSqlComposite() { }

        #region Join Operators

        public void Join() { }

        //public static IEnumerable<BsonDocument> Join(
        //    this IEnumerable<BsonDocument> outer, 
        //    IEnumerable<BsonDocument> inner)
        //{
        //    return outer.Join(inner, 
        //        orow => orow["_id"], 
        //        irow => irow["_id"], 
        //        (orow, irow) => orow.Merge(irow));
        //}

        public void GroupJoin() { }

        //public static IEnumerable<BsonDocument> GroupJoin(
        //    this IEnumerable<BsonDocument> outer,
        //    IEnumerable<BsonDocument> inner)
        //{
        //    return outer.GroupJoin(inner,
        //        orow => orow["_id"],
        //        irow => irow["_id"],
        //        (orow, irow) => orow.Merge(irow));
        //}

        #endregion

        private void PlinqCustomAggregate()
        {
            int[] source = new int[100000];
            Random rand = new Random();
            for (int x = 0; x < source.Length; x++)
            {
                source[x] = rand.Next(10, 20);
            }

            double mean = source.AsParallel().Average();


            // We use the overload that is unique to ParallelEnumerable. The 
            // third Func parameter combines the results from each thread.
            double standardDev = source.AsParallel().Aggregate(
                // initialize subtotal. Use decimal point to tell 
                // the compiler this is a type double. Can also use: 0d.
                0.0,

                // do this on each thread
                 (subtotal, item) => subtotal + Math.Pow((item - mean), 2),

                 // aggregate results after all threads are done.
                 (total, thisThread) => total + thisThread,

                // perform standard deviation calc on the aggregated result.
                (finalSum) => Math.Sqrt((finalSum / (source.Length - 1)))
            );

            Console.WriteLine("Mean value is = {0}", mean);
            Console.WriteLine("Standard deviation is {0}", standardDev);
            Console.ReadLine();

        }

        #region Set Operators

        public void Distinct() { }

        public void Except() { }

        public void Intersect() { }

        public void Union() { }

        #endregion

    }
}

/// <summary>
/// Merges a dictionary against an array of other dictionaries.
/// </summary>
/// <typeparam name="TResult">The type of the resulting dictionary.</typeparam>
/// <typeparam name="TKey">The type of the key in the resulting dictionary.</typeparam>
/// <typeparam name="TValue">The type of the value in the resulting dictionary.</typeparam>
/// <param name="source">The source dictionary.</param>
/// <param name="mergeBehavior">A delegate returning the merged value. (Parameters in order: The current key, The current value, The previous value)</param>
/// <param name="mergers">Dictionaries to merge against.</param>
/// <returns>The merged dictionary.</returns>
//public static TResult MergeLeft<TResult, TKey, TValue>(
//    this TResult source,
//    Func<TKey, TValue, TValue, TValue> mergeBehavior,
//    params IDictionary<TKey, TValue>[] mergers)
//    where TResult : IDictionary<TKey, TValue>, new()
//{
//    var result = new TResult();
//    var sources = new List<IDictionary<TKey, TValue>> { source }
//        .Concat(mergers);

//    foreach (var kv in sources.SelectMany(src => src))
//    {
//        TValue previousValue;
//        result.TryGetValue(kv.Key, out previousValue);
//        result[kv.Key] = mergeBehavior(kv.Key, kv.Value, previousValue);
//    }

//    return result;
//}

//private static BsonDocument MergeLeft(this BsonDocument left, BsonDocument right)
//{
//    left.Merge(right, false);

//    return left;
//}