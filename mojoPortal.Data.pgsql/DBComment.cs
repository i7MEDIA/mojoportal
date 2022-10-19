// Author:					
// Created:					2012-08-11
// Last Modified:			2012-08-25
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
using System.IO;
using Npgsql;
using NpgsqlTypes;
using mojoPortal.Data; // add a project reference to mojoPortal.Data.pgsql to get this
	
namespace mojoPortal.Data
{
	public static class DBComments
    {
    
        /// <summary>
        /// Inserts a row in the mp_Comments table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="parentGuid"> parentGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="featureGuid"> featureGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="contentGuid"> contentGuid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid parentGuid,
            Guid siteGuid,
            Guid featureGuid,
            Guid moduleGuid,
            Guid contentGuid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime createdUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_comments (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("parentguid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("featureguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("contentguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("title, ");
            sqlCommand.Append("usercomment, ");
            sqlCommand.Append("username, ");
            sqlCommand.Append("useremail, ");
            sqlCommand.Append("userurl, ");
            sqlCommand.Append("userip, ");
            sqlCommand.Append("createdutc, ");
            sqlCommand.Append("lastmodutc, ");
            sqlCommand.Append("moderationstatus, ");
            sqlCommand.Append("moderatedby, ");
            sqlCommand.Append("moderationreason )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":parentguid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":featureguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":contentguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":title, ");
            sqlCommand.Append(":usercomment, ");
            sqlCommand.Append(":username, ");
            sqlCommand.Append(":useremail, ");
            sqlCommand.Append(":userurl, ");
            sqlCommand.Append(":userip, ");
            sqlCommand.Append(":createdutc, ");
            sqlCommand.Append(":lastmodutc, ");
            sqlCommand.Append(":moderationstatus, ");
            sqlCommand.Append(":moderatedby, ");
            sqlCommand.Append(":moderationreason ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[18];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentGuid.ToString();

            arParams[2] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = featureGuid.ToString();

            arParams[4] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = moduleGuid.ToString();

            arParams[5] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = contentGuid.ToString();

            arParams[6] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userGuid.ToString();

            arParams[7] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = title;

            arParams[8] = new NpgsqlParameter(":usercomment", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = userComment;

            arParams[9] = new NpgsqlParameter(":username", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userName;

            arParams[10] = new NpgsqlParameter(":useremail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userEmail;

            arParams[11] = new NpgsqlParameter(":userurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userUrl;

            arParams[12] = new NpgsqlParameter(":userip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userIp;

            arParams[13] = new NpgsqlParameter(":createdutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdUtc;

            arParams[14] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdUtc;

            arParams[15] = new NpgsqlParameter(":moderationstatus", NpgsqlTypes.NpgsqlDbType.Smallint);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = moderationStatus;

            arParams[16] = new NpgsqlParameter(":moderatedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = moderatedBy.ToString();

            arParams[17] = new NpgsqlParameter(":moderationreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = moderationReason;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the mp_Comments table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="title"> title </param>
        /// <param name="userComment"> userComment </param>
        /// <param name="userName"> userName </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="userUrl"> userUrl </param>
        /// <param name="userIp"> userIp </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="moderationStatus"> moderationStatus </param>
        /// <param name="moderatedBy"> moderatedBy </param>
        /// <param name="moderationReason"> moderationReason </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            Guid userGuid,
            string title,
            string userComment,
            string userName,
            string userEmail,
            string userUrl,
            string userIp,
            DateTime lastModUtc,
            byte moderationStatus,
            Guid moderatedBy,
            string moderationReason)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_comments ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("title = :title, ");
            sqlCommand.Append("usercomment = :usercomment, ");
            sqlCommand.Append("username = :username, ");
            sqlCommand.Append("useremail = :useremail, ");
            sqlCommand.Append("userurl = :userurl, ");
            sqlCommand.Append("userip = :userip, ");
            sqlCommand.Append("lastmodutc = :lastmodutc, ");
            sqlCommand.Append("moderationstatus = :moderationstatus, ");
            sqlCommand.Append("moderatedby = :moderatedby, ");
            sqlCommand.Append("moderationreason = :moderationreason ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[12];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new NpgsqlParameter(":title", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter(":usercomment", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userComment;

            arParams[4] = new NpgsqlParameter(":username", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userName;

            arParams[5] = new NpgsqlParameter(":useremail", NpgsqlTypes.NpgsqlDbType.Varchar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userEmail;

            arParams[6] = new NpgsqlParameter(":userurl", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = userUrl;

            arParams[7] = new NpgsqlParameter(":userip", NpgsqlTypes.NpgsqlDbType.Varchar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = userIp;

            arParams[8] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = lastModUtc;

            arParams[9] = new NpgsqlParameter(":moderationstatus", NpgsqlTypes.NpgsqlDbType.Smallint);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moderationStatus;

            arParams[10] = new NpgsqlParameter(":moderatedby", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moderatedBy.ToString();

            arParams[11] = new NpgsqlParameter(":moderationreason", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moderationReason;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
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

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByContent(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("contentguid = :contentguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
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

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByFeature(Guid featureGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("featureguid = :featureguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":featureguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = featureGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Deletes rows from the mp_Comments table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByParent(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("parentguid = :parentguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
            sqlCommand.Append("COALESCE(u.name, c.username) AS postauthor, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("COALESCE(u.email, c.useremail) AS authoremail, ");
            sqlCommand.Append("COALESCE(u.totalrevenue, 0) AS userrevenue, ");
            sqlCommand.Append("COALESCE(u.trusted, false) AS trusted, ");
            sqlCommand.Append("u.avatarurl AS postauthoravatar, ");
            sqlCommand.Append("COALESCE(c.userurl, u.websiteurl) AS postauthorwebsiteurl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON c.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.guid = :guid ");
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

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentAsc(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
            sqlCommand.Append("COALESCE(u.name, c.username) AS postauthor, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("COALESCE(u.email, c.useremail) AS authoremail, ");
            sqlCommand.Append("COALESCE(u.totalrevenue, 0) AS userrevenue, ");
            sqlCommand.Append("COALESCE(u.trusted, false) AS trusted, ");
            sqlCommand.Append("u.avatarurl AS postauthoravatar, ");
            sqlCommand.Append("COALESCE(c.userurl, u.websiteurl) AS postauthorwebsiteurl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON c.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.contentguid = :contentguid ");
            sqlCommand.Append("ORDER BY c.createdutc ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByContentDesc(Guid contentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
            sqlCommand.Append("COALESCE(u.name, c.username) AS postauthor, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("COALESCE(u.email, c.useremail) AS authoremail, ");
            sqlCommand.Append("COALESCE(u.totalrevenue, 0) AS userrevenue, ");
            sqlCommand.Append("COALESCE(u.trusted, false) AS trusted, ");
            sqlCommand.Append("u.avatarurl AS postauthoravatar, ");
            sqlCommand.Append("COALESCE(c.userurl, u.websiteurl) AS postauthorwebsiteurl ");
            

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON c.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.contentguid = :contentguid ");
            sqlCommand.Append("ORDER BY c.createdutc DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentAsc(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
            sqlCommand.Append("COALESCE(u.name, c.username) AS postauthor, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("COALESCE(u.email, c.useremail) AS authoremail, ");
            sqlCommand.Append("COALESCE(u.totalrevenue, 0) AS userrevenue, ");
            sqlCommand.Append("COALESCE(u.trusted, false) AS trusted, ");
            sqlCommand.Append("u.avatarurl AS postauthoravatar, ");
            sqlCommand.Append("COALESCE(c.userurl, u.websiteurl) AS postauthorwebsiteurl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON c.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.parentguid = :parentguid ");
            sqlCommand.Append("ORDER BY c.createdutc ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets an IDataReader with rows from the mp_Comments table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetByParentDesc(Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  c.*, ");
            sqlCommand.Append("COALESCE(u.name, c.username) AS postauthor, ");
            sqlCommand.Append("COALESCE(u.userid, -1) AS userid, ");
            sqlCommand.Append("COALESCE(u.email, c.useremail) AS authoremail, ");
            sqlCommand.Append("COALESCE(u.totalrevenue, 0) AS userrevenue, ");
            sqlCommand.Append("COALESCE(u.trusted, false) AS trusted, ");
            sqlCommand.Append("u.avatarurl AS postauthoravatar, ");
            sqlCommand.Append("COALESCE(c.userurl, u.websiteurl) AS postauthorwebsiteurl ");

            sqlCommand.Append("FROM	mp_Comments c ");

            sqlCommand.Append("LEFT OUTER JOIN mp_users u ");
            sqlCommand.Append("ON c.userguid = u.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("c.parentguid = :parentguid ");
            sqlCommand.Append("ORDER BY c.createdutc DESC ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = parentGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_Comments table.
        /// </summary>
        public static int GetCount(Guid contentGuid, int moderationStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("contentguid = :contentguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("moderationstatus = :moderationstatus ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":contentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentGuid.ToString();

            arParams[1] = new NpgsqlParameter(":moderationstatus", NpgsqlTypes.NpgsqlDbType.Smallint);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountByModule(Guid moduleGuid, int moderationStatus)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("moderationstatus = :moderationstatus ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter(":moderationstatus", NpgsqlTypes.NpgsqlDbType.Smallint);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moderationStatus;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

        public static int GetCountBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_comments ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append(":siteguid = '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("OR ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

    }
}
