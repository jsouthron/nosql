using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoSql.Aggregation
{
    public class NoSqlMapReduce
    {

        public string map = @"
            function() {
                var movie = this;
                emit(movie.Category, { count: 1, totalMinutes: movie.Minutes });
            }";

        public string reduce = @"        
            function(key, values) {
                var result = {count: 0, totalMinutes: 0 };

                values.forEach(function(value){               
                    result.count += value.count;
                    result.totalMinutes += value.totalMinutes;
                });

                return result;
            }";

        public string finalize = @"
            function(key, value){
      
              value.average = value.totalMinutes / value.count;
              return value;

            }";
    }
}
