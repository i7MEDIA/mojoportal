// Author:					Joe Audette
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
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using FirebirdSql.Data.FirebirdClient;

namespace mojoPortal.Data
{
    public static class DBLetterSubscription
    {
        private static String GetReadConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

        private static String GetWriteConnectionString()
        {
            if (ConfigurationManager.AppSettings["FirebirdWriteConnectionString"] != null)
            {
                return ConfigurationManager.AppSettings["FirebirdWriteConnectionString"];
            }

            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[10];


            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new FbParameter("@IsVerified", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsVerified;

            arParams[6] = new FbParameter("@VerifyGuid", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new FbParameter("@BeginUtc", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new FbParameter("@UseHtml", FbDbType.SmallInt);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intUseHtml;

            arParams[9] = new FbParameter("@IpAddress", FbDbType.VarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ipAddress;


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
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@IsVerified, ");
            sqlCommand.Append("@VerifyGuid, ");
            sqlCommand.Append("@IpAddress, ");
            sqlCommand.Append("@BeginUtc, ");
            sqlCommand.Append("@UseHtml )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[9];


            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new FbParameter("@IsVerified", FbDbType.SmallInt);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intIsVerified;

            arParams[6] = new FbParameter("@VerifyGuid", FbDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new FbParameter("@BeginUtc", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new FbParameter("@UseHtml", FbDbType.SmallInt);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intUseHtml;

           

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
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@IsVerified, ");
            sqlCommand.Append("@VerifyGuid, ");
   
            sqlCommand.Append("@BeginUtc, ");
            sqlCommand.Append("@UseHtml )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            sqlCommand.Append("UseHtml = @UseHtml, ");
            sqlCommand.Append("UserGuid = @UserGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new FbParameter("@UseHtml", FbDbType.SmallInt);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intUseHtml;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("Email = @Email ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            int count =  Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
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
            sqlCommand.Append("IsVerified = @IsVerified, ");
            sqlCommand.Append("VerifyGuid = @VerifyGuid ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@IsVerified", FbDbType.SmallInt);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intIsVerified;

            arParams[2] = new FbParameter("@VerifyGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = verifyGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@OlderThanUtc", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThanUtc;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_LetterSubscribe table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ls.UserGuid = @UserGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        

        public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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
            sqlCommand.Append("ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("(");
            sqlCommand.Append("ls.Email CONTAINING @EmailOrIpAddress ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append("u.Email CONTAINING @EmailOrIpAddress ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append("ls.IpAddress CONTAINING @EmailOrIpAddress ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@EmailOrIpAddress", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = emailOrIpAddress;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.Email = @Email) OR (u.Email = @Email)) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
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
            sqlCommand.Append("ls.SiteGuid = @SiteGuid ");
            sqlCommand.Append("AND ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.Email = @Email) OR (u.Email = @Email)) ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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


            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new FbParameter("@ExcludeIfAnyUnsubscribeHx", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intExcludeIfAnyUnsubscribeHx;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
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
            sqlCommand.Append("SELECT FIRST 1000 ");
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


            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new FbParameter("@ExcludeIfAnyUnsubscribeHx", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intExcludeIfAnyUnsubscribeHx;

            

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            //sqlCommand.Append("u.Email,  ");
            sqlCommand.Append("u.Name,  ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");


            sqlCommand.Append("FROM	mp_LetterSubscribe ls ");
            sqlCommand.Append("LEFT OUTER JOIN  ");
            sqlCommand.Append("mp_Users u  ");
            sqlCommand.Append("ON  ");
            sqlCommand.Append("u.UserGuid = ls.UserGuid  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append(" ls.LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append("AND ls.IsVerified = 1 ");
            sqlCommand.Append("AND ls.Guid   ");
            sqlCommand.Append("NOT IN (  ");
            sqlCommand.Append("SELECT SubscribeGuid  ");
            sqlCommand.Append("FROM	mp_LetterSendLog  ");
            sqlCommand.Append("WHERE LetterGuid = @LetterGuid  ");
            sqlCommand.Append(")  ");

            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@LetterGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_LetterSubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("LetterInfoGuid = @LetterInfoGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetReadConnectionString(),
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
            sqlCommand.Append("SELECT FIRST " + pageSize.ToString() + " ");
            sqlCommand.Append("	SKIP " + pageLowerBound.ToString() + " ");
            sqlCommand.Append("ls.Guid, ");
            sqlCommand.Append("ls.SiteGuid, ");
            sqlCommand.Append("ls.LetterInfoGuid, ");
            sqlCommand.Append("ls.UserGuid, ");
            sqlCommand.Append("ls.IsVerified, ");
            sqlCommand.Append("ls.VerifyGuid, ");
            sqlCommand.Append("ls.BeginUtc, ");
            sqlCommand.Append("ls.UseHtml, ");
            sqlCommand.Append("ls.IpAddress, ");
            sqlCommand.Append("COALESCE(u.Email, ls.Email) As Email, ");
            sqlCommand.Append("COALESCE(u.Name, ls.Email) AS Name, ");
            sqlCommand.Append("	u.Email AS UserEmail, ");
            sqlCommand.Append("u.FirstName, ");
            sqlCommand.Append("u.LastName ");

            sqlCommand.Append("FROM	mp_LetterSubscribe ls  ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u  ");
            sqlCommand.Append("ON u.UserGuid = ls.UserGuid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append(" ls.LetterInfoGuid = @LetterInfoGuid ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("ls.BeginUtc DESC ");
            sqlCommand.Append("	; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new FbParameter("@PageNumber", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new FbParameter("@PageSize", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            return FBSqlHelper.ExecuteReader(
                GetReadConnectionString(),
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

            FbParameter[] arParams = new FbParameter[11];

            arParams[0] = new FbParameter("@RowGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@SubscribeGuid", FbDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subscribeGuid.ToString();

            arParams[3] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = letterInfoGuid.ToString();

            arParams[4] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new FbParameter("@Email", FbDbType.VarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = email;

            arParams[6] = new FbParameter("@IsVerified", FbDbType.SmallInt);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = intIsVerified;

            arParams[7] = new FbParameter("@UseHtml", FbDbType.SmallInt);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intUseHtml;

            arParams[8] = new FbParameter("@BeginUtc", FbDbType.TimeStamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = beginUtc;

            arParams[9] = new FbParameter("@EndUtc", FbDbType.TimeStamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endUtc;

            arParams[10] = new FbParameter("@IpAddress", FbDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ipAddress;

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
            sqlCommand.Append("@RowGuid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@SubscribeGuid, ");
            sqlCommand.Append("@LetterInfoGuid, ");
            sqlCommand.Append("@UserGuid, ");
            sqlCommand.Append("@Email, ");
            sqlCommand.Append("@IsVerified, ");
            sqlCommand.Append("@IpAddress, ");
            sqlCommand.Append("@UseHtml, ");
            sqlCommand.Append("@BeginUtc, ");
            sqlCommand.Append("@EndUtc )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
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
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@LetterInfoGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


    }
}
