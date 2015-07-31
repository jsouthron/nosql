using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NoSql
{
    public class MongoLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        private double _proximity;
        public double Proximity
        {
            get { return this._proximity * 1609.34; }
            set { this._proximity = value; }
        }

        public MongoLocation(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public MongoLocation(double[] loc)
        {
            Longitude = loc[0];
            Latitude = loc[1];
        }
    }
}
