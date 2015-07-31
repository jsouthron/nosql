using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace NoSql
{
    public class Constants
    {
        public const string NoSqlConnectionString = "NoSqlConnectionString";
        public const string SolrConnectionString = "SolrConnectionString";
        public const string NoSqlArchiveConnectionString = "NoSqlArchiveConnectionString";

        public const string AspNetApplicationName = "/";

        public static List<string> ApprovedFileTypes = new List<string> { "txt", "pdf", "doc", "docx", "rtf", "xlsx", "xls", "jpg", "jpeg", "png", "csv" };

        public const string InvalidMessage = "The characters < > & \\ /\n are invalid";
        public const string RegexSpecialChars = @"([^<>&\\/]+)";
        public const string Zipcode = @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)";
        public const string RegexEmail = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        public const string DefaultSearchTerms = "DefaultSearchTerms";

        public const string PayPalUser = "PayPalUser";
        public const string PayPalPassword = "PayPalPassword";
        public const string PayPalSig = "PayPalSig";
        public const string PayPalUrl = "PayPalUrl";
        public const string PapPalVersion = "63";
    }
}
