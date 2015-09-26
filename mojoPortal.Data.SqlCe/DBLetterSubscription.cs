// Author:					Joe Audette
// Created:					2010-04-05
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
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBLetterSubscription
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSubscribe ");
            sqlCommand.Append("(");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("IsVerified, ");
            sqlCommand.Append("VerifyGuid, ");
            sqlCommand.Append("BeginUtc, ");
            sqlCommand.Append("UseHtml, ");
            sqlCommand.Append("IpAddress ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@IsVerified, ");
            sqlCommand.Append("@VerifyGuid, ");
            sqlCommand.Append("@BeginUtc, ");
            sqlCommand.Append("@UseHtml, ");
            sqlCommand.Append("@IpAddress ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid;

            arParams[3] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid;

            arParams[4] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new SqlCeParameter("@IsVerified", SqlDbType.Bit);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isVerified;

            arParams[6] = new SqlCeParameter("@VerifyGuid", SqlDbType.UniqueIdentifier);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid;

            arParams[7] = new SqlCeParameter("@BeginUtc", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new SqlCeParameter("@UseHtml", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = useHtml;

            arParams[9] = new SqlCeParameter("@IpAddress", SqlDbType.NVarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ipAddress;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_LetterSubscribe table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="useHtml"> useHtml </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            bool useHtml)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterSubscribe ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("UseHtml = @UseHtml ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            arParams[2] = new SqlCeParameter("@UseHtml", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = useHtml;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("IsVerified = 0 ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("BeginUtc < @OlderThanUtc ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@OlderThanUtc", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThanUtc;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes a row from the mp_LetterSubscribe table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Verify(
            Guid guid,
            bool isVerified,
            Guid verifyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_LetterSubscribe ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("IsVerified = @IsVerified, ");
            sqlCommand.Append("VerifyGuid = @VerifyGuid ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            arParams[1] = new SqlCeParameter("@IsVerified", SqlDbType.Bit);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = isVerified;

            arParams[2] = new SqlCeParameter("@VerifyGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = verifyGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static bool Exists(Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Email = @Email ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
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
            sqlCommand.Append("ls.Guid = @Guid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@Guid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");
            
            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");
            
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ls.UserGuid = @UserGuid ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByEmail(Guid siteGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ((ls.Email = @Email) OR (u.Email = @Email)) ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ((ls.Email = @Email) OR (u.Email = @Email)) ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            arParams[2] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*) ");
            
            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND u.IsDeleted = 0 ");
            sqlCommand.Append("AND u.ProfileApproved = 1 ");
            sqlCommand.Append("AND u.IsLockedOut = 0 ");
            sqlCommand.Append("AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT ls.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribe ls ");
            sqlCommand.Append("WHERE ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT lsx.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribeHx lsx ");
            sqlCommand.Append("WHERE ((@ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = @LetterInfoGuid)) ");
            sqlCommand.Append(") ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            arParams[2] = new SqlCeParameter("@ExcludeIfAnyUnsubscribeHx", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excludeIfAnyUnsubscribeHx;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
               GetConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams));
        }

        public static IDataReader GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP (1000) ");
            sqlCommand.Append("u.UserID, ");
            sqlCommand.Append("u.UserGuid, ");
            sqlCommand.Append("u.Email ");

            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND u.IsDeleted = 0 ");
            sqlCommand.Append("AND u.ProfileApproved = 1 ");
            sqlCommand.Append("AND u.IsLockedOut = 0 ");
            sqlCommand.Append("AND (u.RegisterConfirmGuid IS NULL OR u.RegisterConfirmGuid = '00000000-0000-0000-0000-000000000000') ");
            
            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT ls.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribe ls ");
            sqlCommand.Append("WHERE ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.UserGuid NOT IN ");
            sqlCommand.Append("(SELECT lsx.UserGuid ");
            sqlCommand.Append("FROM mp_LetterSubscribeHx lsx ");
            sqlCommand.Append("WHERE ((@ExcludeIfAnyUnsubscribeHx = 1) OR (lsx.LetterInfoGuid = @LetterInfoGuid)) ");
            sqlCommand.Append(") ");
            
            
            sqlCommand.Append("ORDER BY u.UserID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            arParams[2] = new SqlCeParameter("@ExcludeIfAnyUnsubscribeHx", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excludeIfAnyUnsubscribeHx;

            
            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND (");
            sqlCommand.Append("(ls.Email LIKE '%' + @EmailOrIpAddress + '%') ");
            sqlCommand.Append("OR (u.Email LIKE '%' + @EmailOrIpAddress + '%') ");
            sqlCommand.Append("OR (ls.[IpAddress]  LIKE '%' + @EmailOrIpAddress + '%') ");
            sqlCommand.Append(") ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            arParams[1] = new SqlCeParameter("@EmailOrIpAddress", SqlDbType.NVarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = emailOrIpAddress;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader from the mp_LetterSubscriber table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterInfoGuid </param>
        public static IDataReader GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("u.[Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ls.IsVerified = 1 ");
           
            sqlCommand.Append("AND ls.[Guid] NOT IN ( SELECT [SubscribeGuid] ");
            sqlCommand.Append("FROM	[mp_LetterSendLog] ");
            sqlCommand.Append("WHERE [LetterGuid] = @LetterGuid ) ");

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@LetterGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid;

            arParams[1] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets a count of rows in the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="letterGuid"> letterGuid </param>
        public static int GetCountByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        /// <summary>
        /// Gets a page of data from the mp_LetterSubscriber table.
        /// </summary>
        /// <param name="letterInfoGuid"> letterGuid </param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
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
            sqlCommand.Append("SELECT * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageSize.ToString(CultureInfo.InvariantCulture) + ") * FROM ");
            sqlCommand.Append("(");
            sqlCommand.Append("SELECT TOP (" + pageNumber.ToString(CultureInfo.InvariantCulture) + " * " + pageSize.ToString(CultureInfo.InvariantCulture) + ")  ");

            sqlCommand.Append("ls.[Guid], ");
            sqlCommand.Append("ls.[SiteGuid], ");
            sqlCommand.Append("ls.[LetterInfoGuid], ");
            sqlCommand.Append("ls.[UserGuid], ");
            sqlCommand.Append("ls.[IsVerified], ");
            sqlCommand.Append("ls.[VerifyGuid], ");
            sqlCommand.Append("ls.[BeginUtc], ");
            sqlCommand.Append("ls.[UseHtml], ");
            sqlCommand.Append("ls.[IpAddress], ");
            sqlCommand.Append("COALESCE(u.Email, ls.[Email]) As Email, ");
            sqlCommand.Append("u.[Email] AS UserEmail, ");
            sqlCommand.Append("COALESCE(u.[Name], ls.[Email]) AS [Name], ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("[mp_Users] u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.[UserGuid] = ls.[UserGuid] ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");

            
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("ls.BeginUtc DESC  ");

            sqlCommand.Append(") AS t1 ");
            //sqlCommand.Append("ORDER BY  ");

            sqlCommand.Append(") AS t2 ");

            //sqlCommand.Append("WHERE   ");
            //sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append(";");


            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_LetterSubscribeHx ");
            sqlCommand.Append("(");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("SubscribeGuid, ");
            sqlCommand.Append("LetterInfoGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Email, ");
            sqlCommand.Append("IsVerified, ");
            sqlCommand.Append("UseHtml, ");
            sqlCommand.Append("BeginUtc, ");
            sqlCommand.Append("EndUtc, ");
            sqlCommand.Append("IpAddress ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SubscribeGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@IsVerified, ");
            sqlCommand.Append("@UseHtml, ");
            sqlCommand.Append("@BeginUtc, ");
            sqlCommand.Append("@EndUtc, ");
            sqlCommand.Append("@IpAddress ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@RowGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid;

            arParams[1] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid;

            arParams[2] = new SqlCeParameter("@SubscribeGuid", SqlDbType.UniqueIdentifier);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subscribeGuid;

            arParams[3] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = letterInfoGuid;

            arParams[4] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid;

            arParams[5] = new SqlCeParameter("@Email", SqlDbType.NVarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = email;

            arParams[6] = new SqlCeParameter("@IsVerified", SqlDbType.Bit);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isVerified;

            arParams[7] = new SqlCeParameter("@UseHtml", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = useHtml;

            arParams[8] = new SqlCeParameter("@BeginUtc", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = beginUtc;

            arParams[9] = new SqlCeParameter("@EndUtc", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endUtc;

            arParams[10] = new SqlCeParameter("@IpAddress", SqlDbType.NVarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ipAddress;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool DeleteHistoryBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribeHx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_LetterSubscribeHx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@LetterInfoGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
