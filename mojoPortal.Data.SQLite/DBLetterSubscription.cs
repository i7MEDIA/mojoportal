// Author:					
// Created:					2009-10-11
// Last Modified:			2013-08-23
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Web;
using Mono.Data.Sqlite;
using mojoPortal.Data; // add a project reference to mojoPortal.Data.SQLite to get this

namespace mojoPortal.Data
{
    public static class DBLetterSubscription
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }


        /// <summary>
        /// Inserts a row in the mp_LetterSubscribe table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="email"> email </param>
        /// <param name="isVerified"> isVerified </param>
        /// <param name="verifyGuid"> verifyGuid </param>
        /// <param name="beginUtc"> beginUtc </param>
        /// <param name="useHtml"> useHtml </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid letterInfoGuid,
            Guid userGuid,
            string email,
            bool isVerified,
            Guid verifyGuid,
            DateTime beginUtc,
            bool useHtml,
            string ipAddress)
        {
            #region Bit Conversion

            int intIsVerified = 0;
            if (isVerified) { intIsVerified = 1; }
            int intUseHtml = 0;
            if (useHtml) { intUseHtml = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSubscribe (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("IsVerified, ");
            sqlCommand.Append("VerifyGuid, ");
            sqlCommand.Append("IpAddress, ");
            sqlCommand.Append("BeginUtc, ");
            sqlCommand.Append("UseHtml )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":LetterInfoGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":Email, ");
            sqlCommand.Append(":IsVerified, ");
            sqlCommand.Append(":VerifyGuid, ");
            sqlCommand.Append(":IpAddress, ");
            sqlCommand.Append(":BeginUtc, ");
            sqlCommand.Append(":UseHtml )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[10];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":Email", DbType.String, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new SqliteParameter(":IsVerified", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsVerified;

            arParams[6] = new SqliteParameter(":VerifyGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new SqliteParameter(":BeginUtc", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new SqliteParameter(":UseHtml", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intUseHtml;

            arParams[9] = new SqliteParameter(":IpAddress", DbType.String, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ipAddress;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Inserts a row in the mp_LetterSubscribe table. Returns rows affected count.
        /// This method is a legacy method only needed to retain the correct signature for the
        /// DatabaseHelperDoVersion2320PostUpgradeTasks
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="email"> email </param>
        /// <param name="isVerified"> isVerified </param>
        /// <param name="verifyGuid"> verifyGuid </param>
        /// <param name="beginUtc"> beginUtc </param>
        /// <param name="useHtml"> useHtml </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid letterInfoGuid,
            Guid userGuid,
            string email,
            bool isVerified,
            Guid verifyGuid,
            DateTime beginUtc,
            bool useHtml)
        {
            #region Bit Conversion

            int intIsVerified = 0;
            if (isVerified) { intIsVerified = 1; }
            int intUseHtml = 0;
            if (useHtml) { intUseHtml = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSubscribe (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("IsVerified, ");
            sqlCommand.Append("VerifyGuid, ");
 
            sqlCommand.Append("BeginUtc, ");
            sqlCommand.Append("UseHtml )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":Guid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":LetterInfoGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":Email, ");
            sqlCommand.Append(":IsVerified, ");
            sqlCommand.Append(":VerifyGuid, ");

            sqlCommand.Append(":BeginUtc, ");
            sqlCommand.Append(":UseHtml )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[9];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new SqliteParameter(":Email", DbType.String, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new SqliteParameter(":IsVerified", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsVerified;

            arParams[6] = new SqliteParameter(":VerifyGuid", DbType.String, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new SqliteParameter(":BeginUtc", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new SqliteParameter(":UseHtml", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intUseHtml;

           
            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid userGuid,
            bool useHtml)
        {
            #region Bit Conversion

            int intUseHtml = 0;
            if (useHtml) { intUseHtml = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_LetterSubscribe ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("UseHtml = :UseHtml, ");
            sqlCommand.Append("UserGuid = :UserGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new SqliteParameter(":UseHtml", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intUseHtml;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Exists(Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Email = :Email ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":Email", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool Verify(
            Guid guid,
            bool isVerified,
            Guid verifyGuid)
        {
            #region Bit Conversion

            int intIsVerified = 0;
            if (isVerified) { intIsVerified = 1; }
           
            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_LetterSubscribe ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("IsVerified = :IsVerified, ");
            sqlCommand.Append("VerifyGuid = :VerifyGuid ");
          
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new SqliteParameter(":IsVerified", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIsVerified;

            arParams[2] = new SqliteParameter(":VerifyGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = verifyGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsVerified = 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("BeginUtc < :OlderThanUtc ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":OlderThanUtc", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThanUtc;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.Guid = :Guid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":Guid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ls.UserGuid = :UserGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("(");

            sqlCommand.Append(" (ls.Email LIKE :EmailOrIpAddress) ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append(" (u.Email LIKE :EmailOrIpAddress) ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append(" (ls.IpAddress LIKE :EmailOrIpAddress) ");
           
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":EmailOrIpAddress", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = emailOrIpAddress;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.Email = :Email) OR (u.Email = :Email)) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":Email", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.Email = :Email) OR (u.Email = :Email)) ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new SqliteParameter(":Email", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            int intExcludeIfAnyUnsubscribeHx = 0;
            if (excludeIfAnyUnsubscribeHx)
            {
                intExcludeIfAnyUnsubscribeHx = 1;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*)  ");
           
            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND u.IsDeleted = 0 ");
            sqlCommand.Append("AND u.ProfileApproved = 1 ");
            sqlCommand.Append("AND u.IsLockedOut = 0 ");
            sqlCommand.Append("AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT ls.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribe ls ");
            sqlCommand.Append("WHERE ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT lsx.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribeHx lsx ");
            sqlCommand.Append("WHERE ((:ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = :LetterInfoGuid)) ");
            sqlCommand.Append(") ");


            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new SqliteParameter(":ExcludeIfAnyUnsubscribeHx", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intExcludeIfAnyUnsubscribeHx;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;
        }

        public static IDataReader GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            int intExcludeIfAnyUnsubscribeHx = 0;
            if(excludeIfAnyUnsubscribeHx)
            {
                intExcludeIfAnyUnsubscribeHx = 1;
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.UserGuid, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteGuid = :SiteGuid ");
            sqlCommand.Append("AND u.IsDeleted = 0 ");
            sqlCommand.Append("AND u.ProfileApproved = 1 ");
            sqlCommand.Append("AND u.IsLockedOut = 0 ");
            sqlCommand.Append("AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') ");
            
            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT ls.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribe ls ");
            sqlCommand.Append("WHERE ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT lsx.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribeHx lsx ");
            sqlCommand.Append("WHERE ((:ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = :LetterInfoGuid)) ");
            sqlCommand.Append(") ");
            
            
            sqlCommand.Append("ORDER BY u.UserID ");
            sqlCommand.Append("LIMIT 1000 ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new SqliteParameter(":ExcludeIfAnyUnsubscribeHx", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intExcludeIfAnyUnsubscribeHx;

            
            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("u.Name, As Name,  ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("u.FirstName AS FirstName, ");
            sqlCommand.Append("u.LastName AS LastName ");


            sqlCommand.Append("FROM	mp_LetterSubscribe ls  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u  ");
            sqlCommand.Append("ON u.UserGuid = ls.UserGuid  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append("AND ls.IsVerified = 1 ");
            sqlCommand.Append("AND ls.Guid   ");
            sqlCommand.Append("NOT IN (  ");
            sqlCommand.Append("SELECT SubscribeGuid  ");
            sqlCommand.Append("FROM	mp_LetterSendLog  ");
            sqlCommand.Append("WHERE LetterGuid = :LetterGuid  ");
            sqlCommand.Append(")  ");
            sqlCommand.Append(" ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }

        public static IDataReader GetPage(
            Guid letterInfoGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCountByLetter(letterInfoGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid AS Guid, ");
            sqlCommand.Append("ls.SiteGuid AS SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid AS LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid AS UserGuid, ");
            sqlCommand.Append("ls.IsVerified AS IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid AS VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc AS BeginUtc, ");
            sqlCommand.Append("ls.UseHtml AS UseHtml, ");
            sqlCommand.Append("ls.IpAddress AS IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("u.Email AS UserEmail, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");


            sqlCommand.Append("FROM	mp_LetterSubscribe ls  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u  ");
            sqlCommand.Append("ON u.UserGuid = ls.UserGuid  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append("ORDER BY ls.BeginUtc DESC ");
            sqlCommand.Append("LIMIT " + pageLowerBound.ToString()
                + ", :PageSize  ; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new SqliteParameter(":PageSize", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Inserts a row in the mp_LetterSubscribeHx table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="subscribeGuid"> subscribeGuid </param>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="email"> email </param>
        /// <param name="isVerified"> isVerified </param>
        /// <param name="useHtml"> useHtml </param>
        /// <param name="beginUtc"> beginUtc </param>
        /// <param name="endUtc"> endUtc </param>
        /// <returns>int</returns>
        public static int CreateHistory(
            Guid rowGuid,
            Guid siteGuid,
            Guid subscribeGuid,
            Guid letterInfoGuid,
            Guid userGuid,
            string email,
            bool isVerified,
            bool useHtml,
            DateTime beginUtc,
            DateTime endUtc,
            string ipAddress)
        {
            #region Bit Conversion

            int intIsVerified = 0;
            if (isVerified) { intIsVerified = 1; }
            int intUseHtml = 0;
            if (useHtml) { intUseHtml = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSubscribeHx (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SubscribeGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("IsVerified, ");
            sqlCommand.Append("IpAddress, ");
            sqlCommand.Append("UseHtml, ");
            sqlCommand.Append("BeginUtc, ");
            sqlCommand.Append("EndUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":RowGuid, ");
            sqlCommand.Append(":SiteGuid, ");
            sqlCommand.Append(":SubscribeGuid, ");
            sqlCommand.Append(":LetterInfoGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":Email, ");
            sqlCommand.Append(":IsVerified, ");
            sqlCommand.Append(":IpAddress, ");
            sqlCommand.Append(":UseHtml, ");
            sqlCommand.Append(":BeginUtc, ");
            sqlCommand.Append(":EndUtc )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[11];

            arParams[0] = new SqliteParameter(":RowGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new SqliteParameter(":SubscribeGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subscribeGuid.ToString();

            arParams[3] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = letterInfoGuid.ToString();

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new SqliteParameter(":Email", DbType.String, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = email;

            arParams[6] = new SqliteParameter(":IsVerified", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsVerified;

            arParams[7] = new SqliteParameter(":UseHtml", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intUseHtml;

            arParams[8] = new SqliteParameter(":BeginUtc", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = beginUtc;

            arParams[9] = new SqliteParameter(":EndUtc", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endUtc;

            arParams[10] = new SqliteParameter(":IpAddress", DbType.String, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ipAddress;


            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool DeleteHistoryBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribeHx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = :SiteGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribeHx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = :LetterInfoGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":LetterInfoGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }


    }
}
