// Author:					
// Created:					2009-10-11
// Last Modified:			2013-09-26
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
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    public static class DBLetterSubscription
    {
        
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
            sqlCommand.Append("INSERT INTO mp_lettersubscribe (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("letterinfoguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("email, ");
            sqlCommand.Append("isverified, ");
            sqlCommand.Append("verifyguid, ");
            sqlCommand.Append("ipaddress, ");
            sqlCommand.Append("beginutc, ");
            sqlCommand.Append("usehtml )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":letterinfoguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":email, ");
            sqlCommand.Append(":isverified, ");
            sqlCommand.Append(":verifyguid, ");
            sqlCommand.Append(":ipaddress, ");
            sqlCommand.Append(":beginutc, ");
            sqlCommand.Append(":usehtml ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[10];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new NpgsqlParameter(":isverified", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isVerified;

            arParams[6] = new NpgsqlParameter(":verifyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new NpgsqlParameter(":beginutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new NpgsqlParameter(":usehtml", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = useHtml;

            arParams[9] = new NpgsqlParameter(":ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = ipAddress;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_lettersubscribe (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("letterinfoguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("email, ");
            sqlCommand.Append("isverified, ");
            sqlCommand.Append("verifyguid, ");
      
            sqlCommand.Append("beginutc, ");
            sqlCommand.Append("usehtml )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":letterinfoguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":email, ");
            sqlCommand.Append(":isverified, ");
            sqlCommand.Append(":verifyguid, ");
            
            sqlCommand.Append(":beginutc, ");
            sqlCommand.Append(":usehtml ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[9];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = letterInfoGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = email;

            arParams[5] = new NpgsqlParameter(":isverified", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = isVerified;

            arParams[6] = new NpgsqlParameter(":verifyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = verifyGuid.ToString();

            arParams[7] = new NpgsqlParameter(":beginutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = beginUtc;

            arParams[8] = new NpgsqlParameter(":usehtml", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = useHtml;

            

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid userGuid,
            bool useHtml)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_lettersubscribe ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("usehtml = :usehtml, ");
            sqlCommand.Append("userguid = :userguid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter(":usehtml", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = useHtml;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Exists(Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("email = :email ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static bool Verify(
            Guid guid,
            bool isVerified,
            Guid verifyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_lettersubscribe ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("isverified = :isverified, ");
            sqlCommand.Append("verifyguid = :verifyguid ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":isverified", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = isVerified;

            arParams[2] = new NpgsqlParameter(":verifyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = verifyGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteUnverified(Guid letterInfoGuid, DateTime olderThanUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("isverified = false ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("beginutc < :olderthanutc ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":olderthanutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = olderThanUtc;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByUser(Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");

            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByUser(Guid siteGuid, Guid userGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.siteguid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ls.userguid = :userguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader Search(Guid letterInfoGuid, string emailOrIpAddress)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("(");

            sqlCommand.Append(" (ls.email LIKE :emailoripaddress) ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append(" (u.email LIKE :emailoripaddress) ");
            sqlCommand.Append(" OR ");
            sqlCommand.Append(" (ls.ipaddress LIKE :emailoripaddress) ");
            
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":emailoripaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = emailOrIpAddress;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.siteguid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.email = :email) OR (u.email = :email)) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = email;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByEmail(Guid siteGuid, Guid letterInfoGuid, string email)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");

            sqlCommand.Append("FROM	mp_lettersubscribe ls ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("u.userguid = ls.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.siteguid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("((ls.email = :email) OR (u.email = :email)) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = email;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int CountUsersNotSubscribedByLetter(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COUNT(*)  ");
            
            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.siteguid = :siteguid ");
            sqlCommand.Append("AND u.isdeleted = false ");
            sqlCommand.Append("AND u.profileapproved = true ");
            sqlCommand.Append("AND u.islockedout = false ");
            sqlCommand.Append("AND (u.registerconfirmguid IS NULL OR u.registerconfirmguid = '00000000-0000-0000-0000-000000000000') ");

            sqlCommand.Append("AND u.userguid NOT IN ");
            sqlCommand.Append("(SELECT ls.userguid ");
            sqlCommand.Append("FROM mp_lettersubscribe ls ");
            sqlCommand.Append("WHERE ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.userguid NOT IN ");
            sqlCommand.Append("(SELECT lsx.userguid ");
            sqlCommand.Append("FROM mp_lettersubscribehx lsx ");
            sqlCommand.Append("WHERE ((:excludeifanyunsubscribehx = true) OR (lsx.letterinfoguid = :letterinfoguid)) ");
            sqlCommand.Append(") ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new NpgsqlParameter(":excludeifanyunsubscribehx", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excludeIfAnyUnsubscribeHx;

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;
        }

        public static IDataReader GetTop1000UsersNotSubscribed(Guid siteGuid, Guid letterInfoGuid, bool excludeIfAnyUnsubscribeHx)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("u.userid, ");
            sqlCommand.Append("u.userguid, ");
            sqlCommand.Append("u.email ");

            sqlCommand.Append("FROM mp_Users u ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("u.siteguid = :siteguid ");
            sqlCommand.Append("AND u.isdeleted = false ");
            sqlCommand.Append("AND u.profileapproved = true ");
            sqlCommand.Append("AND u.islockedout = false ");
            sqlCommand.Append("AND (u.registerconfirmguid IS NULL OR u.registerconfirmguid = '00000000-0000-0000-0000-000000000000') ");
            
            sqlCommand.Append("AND u.userguid NOT IN ");
            sqlCommand.Append("(SELECT ls.userguid ");
            sqlCommand.Append("FROM mp_lettersubscribe ls ");
            sqlCommand.Append("WHERE ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(") ");

            sqlCommand.Append("AND u.userguid NOT IN ");
            sqlCommand.Append("(SELECT lsx.userguid ");
            sqlCommand.Append("FROM mp_lettersubscribehx lsx ");
            sqlCommand.Append("WHERE ((:excludeifanyunsubscribehx = true) OR (lsx.letterinfoguid = :letterinfoguid)) ");
            sqlCommand.Append(") ");
            
            
            sqlCommand.Append("ORDER BY u.userid ");
            sqlCommand.Append("LIMIT 1000 ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            arParams[2] = new NpgsqlParameter(":excludeifanyunsubscribehx", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = excludeIfAnyUnsubscribeHx;
            

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSubscribersNotSentYet(
            Guid letterGuid,
            Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("u.name,  ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");


            sqlCommand.Append("FROM	mp_lettersubscribe ls  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users u  ");
            sqlCommand.Append("ON u.userguid = ls.userguid  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ls.letterinfoguid = :letterinfoguid ");
            sqlCommand.Append("AND ls.isverified = true ");
            sqlCommand.Append("AND ls.guid   ");
            sqlCommand.Append("NOT IN (  ");
            sqlCommand.Append("SELECT subscribeguid  ");
            sqlCommand.Append("FROM	mp_lettersendlog  ");
            sqlCommand.Append("WHERE letterguid = :letterguid  ");
            sqlCommand.Append(")  ");
            sqlCommand.Append(" ;");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":letterguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterGuid.ToString();

            arParams[1] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = letterInfoGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCountByLetter(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_lettersubscribe ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return count;

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
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("ls.guid, ");
            sqlCommand.Append("ls.siteguid, ");
            sqlCommand.Append("ls.letterinfoguid, ");
            sqlCommand.Append("ls.userguid, ");
            sqlCommand.Append("ls.isverified, ");
            sqlCommand.Append("ls.verifyguid, ");
            sqlCommand.Append("ls.beginutc, ");
            sqlCommand.Append("ls.usehtml, ");
            sqlCommand.Append("ls.ipaddress, ");
            sqlCommand.Append("COALESCE(u.email, ls.email) As email, ");
            sqlCommand.Append("COALESCE(u.name, ls.email) AS name, ");
            sqlCommand.Append("u.email AS useremail, ");
            sqlCommand.Append("u.firstname, ");
            sqlCommand.Append("u.lastname ");


            sqlCommand.Append("FROM	mp_lettersubscribe ls  ");
            sqlCommand.Append("LEFT OUTER JOIN	mp_users u  ");
            sqlCommand.Append("ON u.userguid = ls.userguid  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");

            sqlCommand.Append("ORDER BY ls.BeginUtc DESC ");

            sqlCommand.Append("LIMIT :pagesize ");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            arParams[1] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("INSERT INTO mp_lettersubscribehx (");
            sqlCommand.Append("rowguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("subscribeguid, ");
            sqlCommand.Append("letterinfoguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("email, ");
            sqlCommand.Append("isverified, ");
            sqlCommand.Append("ipaddress, ");
            sqlCommand.Append("usehtml, ");
            sqlCommand.Append("beginutc, ");
            sqlCommand.Append("endutc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":rowguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":subscribeguid, ");
            sqlCommand.Append(":letterinfoguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":email, ");
            sqlCommand.Append(":isverified, ");
            sqlCommand.Append(":ipaddress, ");
            sqlCommand.Append(":usehtml, ");
            sqlCommand.Append(":beginutc, ");
            sqlCommand.Append(":endutc ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[11];

            arParams[0] = new NpgsqlParameter(":rowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":subscribeguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = subscribeGuid.ToString();

            arParams[3] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = letterInfoGuid.ToString();

            arParams[4] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new NpgsqlParameter(":email", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = email;

            arParams[6] = new NpgsqlParameter(":isverified", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = isVerified;

            arParams[7] = new NpgsqlParameter(":usehtml", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = useHtml;

            arParams[8] = new NpgsqlParameter(":beginutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = beginUtc;

            arParams[9] = new NpgsqlParameter(":endutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = endUtc;

            arParams[10] = new NpgsqlParameter(":ipaddress", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = ipAddress;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool DeleteHistoryBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribehx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteHistoryByLetterInfo(Guid letterInfoGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_lettersubscribehx ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("letterinfoguid = :letterinfoguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":letterinfoguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = letterInfoGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }



    }
}
