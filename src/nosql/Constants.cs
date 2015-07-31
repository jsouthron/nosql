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

        public const string UserId= "UserId";
        public const string RemoteAddress = "RemoteAddress";
        public const string Subdomain= "Subdomain";
        public const string EventTS= "EventTS";
        public const string Username = "Username";
        public const string RequestedUrl = "RequestedUrl";
        public const string OrganizationId = "OrganizationId";
        public const string UserLockedByMaxNoAttempts = "User Locked By Exceeding Number of Login Attempts";
        public const string UserLockedByAdmin = "User Locked By Admin";

        public const string PublicRoleGuid = "00000000-0000-0000-0000-000000000000";
        public const string UserRoleGuid = "506DAA03-6B92-44CB-80D7-D04D4CB15673";
        public const string AdminRoleGuid = "0BA9D1D7-9DF1-48B3-BE09-7FEE875B94C8";
        public const string SuperAdminRoleGuid = "885F3208-B441-4E51-98B1-443D87749A3B";
        public const string RootRoleGuid = "99999999-9999-9999-9999-999999999999";
        public const string LoggingRoleGuid = "8A10260D-FAAC-4E11-B7E0-DBF4EB0FD5DE";
        public const string SystemRoleGuid = "F36FF998-1ACD-4696-B722-C33A5AE09631";

        public static List<string> AdminRolesList { get { return new List<string> { AdminRoleGuid, SuperAdminRoleGuid }; } }

        public const string LoginSuccess = "Login Success";
        public const string LoginFailure = "Login Failure";
        public const string SSOLoginSuccess = "SSO Login Success";
        public const string SSOLoginFailure = "SSO Login Failure";

        public const long RootOrganizationID = 1;
        public const string AspNetApplicationName = "/";

        public static List<string> ApprovedFileTypes = new List<string> { "txt", "pdf", "doc", "docx", "rtf", "xlsx", "xls", "jpg", "jpeg", "png", "csv" };
        public static string RootDomain { get { return ConfigurationManager.AppSettings["RootDomain"]; } }

        public const string InvalidMessage = "The characters < > & \\ /\n are invalid";
        public const string RegexSpecialChars = @"([^<>&\\/]+)";
        public const string zipcode = @"(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)";
        public const string RegexEmail = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

        //User Voice
        public const string UserVoiceSubDomain = "benefithub";
        public const string UserVoiceSSOToken = "33a5b70b7db7f124460453be2318f380";
        public const string UserVoiceRedirectUrl = "http://support.benefithub.com?sso={0}";

        public const string DefaultSearchTerms = "DefaultSearchTerms";

        public const string PayPalUser = "PayPalUser";
        public const string PayPalPassword = "PayPalPassword";
        public const string PayPalSig = "PayPalSig";
        public const string PayPalUrl = "PayPalUrl";
        public const string PapPalVersion = "63";
    }

    public enum ActivityLogTypes
    {
        Registered = 1,
        Login = 10,
        Logout = 20,
        InvitationSent = 30
    }
}
