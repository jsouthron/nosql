using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoSql.Aggregation
{
    public class NoSqlMapReduce
    {

        public string Map = @" 
            function() {
                var movie = this;
                emit(movie.Category, { count: 1, totalMinutes: movie.Minutes });
            }";

        public string Reduce = @"        
            function(key, values) {
                var result = {count: 0, totalMinutes: 0 };

                values.forEach(function(value){               
                    result.count += value.count;
                    result.totalMinutes += value.totalMinutes;
                });

                return result;
            }";

        public string Finalize = @"
            function(key, value){
      
              value.average = value.totalMinutes / value.count;
              return value;

            }";
    }
}
