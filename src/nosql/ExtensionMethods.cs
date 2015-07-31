using System;


namespace NoSql
{
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
}
