using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;


namespace NoSql
{
    using System.Web.Configuration;

    public static class ExtensionMethods
    {
        public static string ToSafeString(this object obj)
        {
            return (obj ?? string.Empty).ToString();
        }

        public static string ToFriendlyDateString(this DateTime t)
        {
            return t.ToString("MMMM d, yyyy");
        }

        public static string AsMonthYear(this int m)
        {
            string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

            if (m > 0 && m <= 12)
            {
                return string.Format("{0}-{1}", m.ToString(), DateTime.Now.Year); 
            }

            return string.Format("unknown"); 
        }

        public static string ExtractSafeString(this object o, string key)
        {
            var obj = o.GetType();
            var prop = obj.GetProperty(key);

            string value = prop.GetValue(o, null).ToSafeString();
            value = (value == "") ? null : value;

            return value;
        }

    }

    public static class StateDictionary
    {
        public static string GetState(string abbr = null, string state = null)
        {
            Dictionary<string, string> states = new Dictionary<string, string>();
            states.Add("AL", "alabama");
            states.Add("AK", "alaska");
            states.Add("AZ", "arizona");
            states.Add("AR", "arkansas");
            states.Add("CA", "california");
            states.Add("CO", "colorado");
            states.Add("CT", "connecticut");
            states.Add("DE", "delaware");
            states.Add("DC", "district of columbia");
            states.Add("FL", "florida");
            states.Add("GA", "georgia");
            states.Add("HI", "hawaii");
            states.Add("ID", "idaho");
            states.Add("IL", "illinois");
            states.Add("IN", "indiana");
            states.Add("IA", "iowa");
            states.Add("KS", "kansas");
            states.Add("KY", "kentucky");
            states.Add("LA", "louisiana");
            states.Add("ME", "maine");
            states.Add("MD", "maryland");
            states.Add("MA", "massachusetts");
            states.Add("MI", "michigan");
            states.Add("MN", "minnesota");
            states.Add("MS", "mississippi");
            states.Add("MO", "missouri");
            states.Add("MT", "montana");
            states.Add("NE", "nebraska");
            states.Add("NV", "nevada");
            states.Add("NH", "new hampshire");
            states.Add("NJ", "new jersey");
            states.Add("NM", "new mexico");
            states.Add("NY", "new york");
            states.Add("NC", "north carolina");
            states.Add("ND", "north dakota");
            states.Add("OH", "ohio");
            states.Add("OK", "oklahoma");
            states.Add("OR", "oregon");
            states.Add("PA", "pennsylvania");
            states.Add("RI", "rhode island");
            states.Add("SC", "south carolina");
            states.Add("SD", "south dakota");
            states.Add("TN", "tennessee");
            states.Add("TX", "texas");
            states.Add("UT", "utah");
            states.Add("VT", "vermont");
            states.Add("VA", "virginia");
            states.Add("WA", "washington");
            states.Add("WV", "west virginia");
            states.Add("WI", "wisconsin");
            states.Add("WY", "wyoming");


            if (abbr != null && states.ContainsKey(abbr.ToUpper()))
                return (states[abbr.ToUpper()]);

            if (state != null && states.Values.Contains(state.ToLower()))
                return states.FirstOrDefault(x => x.Value == state).Key;

            /* error handler is to return an empty string rather than throwing an exception */
            return "";
        }

        public static HttpBrowserCapabilities GetClientBrowser(this string useragent)
        {
            var browser = new HttpBrowserCapabilities
            {
                Capabilities = new Hashtable { { string.Empty, useragent } }
            };
            var factory = new BrowserCapabilitiesFactory();
            factory.ConfigureBrowserCapabilities(new NameValueCollection(), browser);

          
            return browser;
        }
    }
}
