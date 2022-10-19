// Author:					
// Created:					2009-07-19
// Last Modified:			2013-04-23
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
using System.Configuration;
using Npgsql;

namespace mojoPortal.Data
{
    public static class DBContentWorkflow
    {
        
        /// <summary>
        /// Inserts a row in the mp_ContentWorkflow table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="moduleGuid"> moduleGuid </param>
        /// <param name="createdDateUtc"> createdDateUtc </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="status"> status </param>
        /// <param name="contentText"> contentText </param>
        /// <param name="customData"> customData </param>
        /// <param name="customReferenceNumber"> customReferenceNumber </param>
        /// <param name="customReferenceGuid"> customReferenceGuid </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid userGuid,
            DateTime createdDateUtc,
            string contentText,
            string customData,
            int customReferenceNumber,
            Guid customReferenceGuid,
            string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_contentworkflow (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("siteguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("createddateutc, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("lastmoduserguid, ");
            sqlCommand.Append("lastmodutc, ");
            sqlCommand.Append("status, ");
            sqlCommand.Append("contenttext, ");
            sqlCommand.Append("customdata, ");
            sqlCommand.Append("customreferencenumber, ");
            sqlCommand.Append("customreferenceguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":siteguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":createddateutc, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":lastmoduserguid, ");
            sqlCommand.Append(":lastmodutc, ");
            sqlCommand.Append(":status, ");
            sqlCommand.Append(":contenttext, ");
            sqlCommand.Append(":customdata, ");
            sqlCommand.Append(":customreferencenumber, ");
            sqlCommand.Append(":customreferenceguid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[12];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new NpgsqlParameter(":createddateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = createdDateUtc;

            arParams[4] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            arParams[5] = new NpgsqlParameter(":lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = userGuid.ToString();

            arParams[6] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDateUtc;

            arParams[7] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = status;

            arParams[8] = new NpgsqlParameter(":contenttext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = contentText;

            arParams[9] = new NpgsqlParameter(":customdata", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = customData;

            arParams[10] = new NpgsqlParameter(":customreferencenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = customReferenceNumber;

            arParams[11] = new NpgsqlParameter(":customreferenceguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = customReferenceGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates a row in the mp_ContentWorkflow table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="lastModUserGuid"> lastModUserGuid </param>
        /// <param name="lastModUtc"> lastModUtc </param>
        /// <param name="status"> status </param>
        /// <param name="contentText"> contentText </param>
        /// <param name="customData"> customData </param>
        /// <param name="customReferenceNumber"> customReferenceNumber </param>
        /// <param name="customReferenceGuid"> customReferenceGuid </param>
        /// <returns>bool</returns>
        public static int Update(
            Guid guid,
            Guid lastModUserGuid,
            DateTime lastModUtc,
            string contentText,
            string customData,
            int customReferenceNumber,
            Guid customReferenceGuid,
            string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_contentworkflow ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("lastmoduserguid = :lastmoduserguid, ");
            sqlCommand.Append("lastmodutc = :lastmodutc, ");
            sqlCommand.Append("status = :status, ");
            sqlCommand.Append("contenttext = :contenttext, ");
            sqlCommand.Append("customdata = :customdata, ");
            sqlCommand.Append("customreferencenumber = :customreferencenumber, ");
            sqlCommand.Append("customreferenceguid = :customreferenceguid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("guid = :guid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[8];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":lastmoduserguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = lastModUserGuid.ToString();

            arParams[2] = new NpgsqlParameter(":lastmodutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = lastModUtc;

            arParams[3] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = status;

            arParams[4] = new NpgsqlParameter(":contenttext", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = contentText;

            arParams[5] = new NpgsqlParameter(":customdata", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = customData;

            arParams[6] = new NpgsqlParameter(":customreferencenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = customReferenceNumber;

            arParams[7] = new NpgsqlParameter(":customreferenceguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = customReferenceGuid.ToString();


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_contentworkflowaudithistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleguid = :moduleguid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_contentworkflow ");
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
        /// Deletes rows from the mp_ContentWorkflow table. Returns true if rows deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM mp_contentworkflowaudithistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("contentworkflowguid IN (SELECT guid FROM mp_contentworkflow WHERE siteguid = :siteguid)  ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_contentworkflow ");
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

        public static IDataReader GetWorkInProgress(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  cw.*, ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.moduletitle, ");

            sqlCommand.Append("createdby.name as createdbyusername, ");
            sqlCommand.Append("createdby.loginname as createdbyuserlogin, ");
            sqlCommand.Append("createdby.email as createdbyuseremail, ");
            sqlCommand.Append("createdby.firstname as createdbyfirstname, ");
            sqlCommand.Append("createdby.lastname as createdbylastname, ");
            sqlCommand.Append("createdby.userid as createdbyuserid, ");
            sqlCommand.Append("createdby.avatarurl as createdbyavatar, ");
            sqlCommand.Append("createdby.authorbio as createdbyauthorbio, ");

            sqlCommand.Append("modifiedby.name as modifiedbyusername, ");
            sqlCommand.Append("modifiedby.firstname as modifiedbyfirstname, ");
            sqlCommand.Append("modifiedby.lastname as modifiedbylastname, ");
            sqlCommand.Append("modifiedby.loginname as modifiedbyuserlogin, ");
            sqlCommand.Append("modifiedby.email as modifiedbyuseremail, ");


            sqlCommand.Append("cwah.notes as notes, ");
            sqlCommand.Append("cwah.createddateutc as recentactionon, ");
            sqlCommand.Append("recentactionby.name as recentactionbyusername, ");
            sqlCommand.Append("recentactionby.loginname as recentactionbyuserlogin, ");
            sqlCommand.Append("recentactionby.email as recentactionbyuseremail ");

            sqlCommand.Append("FROM	mp_contentworkflow cw ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.moduleguid = m.guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users createdby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdby.userguid = cw.userguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users modifiedby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedby.userguid = cw.lastmoduserguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_contentworkflowaudithistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.contentworkflowguid = cw.guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.active = true ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users recentactionby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentactionby.userguid = cwah.userguid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.moduleguid = :moduleguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.status NOT IN ('Cancelled','Approved') ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("createddateutc DESC ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetWorkInProgress(Guid moduleGuid, string status)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  cw.*, ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.moduletitle, ");

            sqlCommand.Append("createdby.name as createdbyusername, ");
            sqlCommand.Append("createdby.loginname as createdbyuserlogin, ");
            sqlCommand.Append("createdby.email as createdbyuseremail, ");
            sqlCommand.Append("createdby.firstname as createdbyfirstname, ");
            sqlCommand.Append("createdby.lastname as createdbylastname, ");
            sqlCommand.Append("createdby.userid as createdbyuserid, ");
            sqlCommand.Append("createdby.avatarurl as createdbyavatar, ");
            sqlCommand.Append("createdby.authorbio as createdbyauthorbio, ");

            sqlCommand.Append("modifiedby.name as modifiedbyusername, ");
            sqlCommand.Append("modifiedby.firstname as modifiedbyfirstname, ");
            sqlCommand.Append("modifiedby.lastname as modifiedbylastname, ");
            sqlCommand.Append("modifiedby.loginname as modifiedbyuserlogin, ");
            sqlCommand.Append("modifiedby.email as modifiedbyuseremail, ");


            sqlCommand.Append("cwah.notes as notes, ");
            sqlCommand.Append("cwah.createddateutc as recentactionon, ");
            sqlCommand.Append("recentactionby.name as recentactionbyusername, ");
            sqlCommand.Append("recentactionby.loginname as recentactionbyuserlogin, ");
            sqlCommand.Append("recentactionby.email as recentactionbyuseremail ");

            sqlCommand.Append("FROM	mp_contentworkflow cw ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.moduleguid = m.guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users createdby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdby.userguid = cw.userguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users modifiedby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("modifiedby.userguid = cw.lastmoduserguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_contentworkflowaudithistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.contentworkflowguid = cw.guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.active = true ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users recentactionby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentactionby.userguid = cwah.userguid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.moduleguid = :moduleguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.status = :status ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("createddateutc DESC ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetWorkInProgressCountByPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");

            sqlCommand.Append("FROM	mp_contentworkflow cw ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_pagemodules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.moduleguid = cw.moduleguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("pm.pageguid = :pageguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.status Not In ('Cancelled','Approved') ");

            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":pageguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static Guid GetDraftSubmitter(Guid contentWorkflowGuid)
        {
            Guid result = Guid.Empty;

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":contentworkflowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contentWorkflowGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT userguid ");
            sqlCommand.Append("FROM	mp_contentworkflowaudithistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("contentworkflowguid = :contentworkflowguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("newstatus = 'AwaitingApproval' ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("createddateutc DESC ");
            sqlCommand.Append("LIMIT 1 ");
            sqlCommand.Append(";");

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    result = new Guid(reader[0].ToString());
                }
            }

            return result;
        }

        public static int GetCount(Guid siteGuid, string status)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_contentworkflow ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteguid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("status = :status ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
               ConnectionString.GetReadConnectionString(),
               CommandType.Text,
               sqlCommand.ToString(),
               arParams));

        }

        public static IDataReader GetPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {

            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(siteGuid, status);

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT cw.*, ");
            sqlCommand.Append("m.moduleid, ");
            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("createdby.name as createdbyusername, ");
            sqlCommand.Append("createdby.loginname as createdByUserLogin, ");
            sqlCommand.Append("createdBy.Email as Createdbyuseremail, ");
            sqlCommand.Append("cwah.notes as notes, ");
            sqlCommand.Append("cwah.createddateutc as recentactionon, ");
            sqlCommand.Append("recentactionby.name as recentactionbyusername, ");
            sqlCommand.Append("recentactionby.loginname as recentactionbyuserlogin, ");
            sqlCommand.Append("recentactionby.email as recentactionbyuseremail ");

            sqlCommand.Append("FROM	mp_contentworkflow cw  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.moduleguid = m.guid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users createdby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("createdby.userguid = cw.userguid ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_contentworkflowaudithistory cwah ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cwah.contentworkflowguid = cw.guid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cwah.active = true ");

            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_users recentactionby ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("recentactionby.userguid = cwah.userguid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.siteGuid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.status = :status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.CreatedDateUtc DESC ");


            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetPageInfoForPage(
            Guid siteGuid,
            string status,
            int pageNumber,
            int pageSize)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new NpgsqlParameter(":status", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = status;

            arParams[2] = new NpgsqlParameter(":pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageSize;

            arParams[3] = new NpgsqlParameter(":pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("p.pageid, ");
            sqlCommand.Append("p.pageguid, ");
            sqlCommand.Append("p.pagename, ");
            sqlCommand.Append("p.useurl, ");
            sqlCommand.Append("p.url As pageurl, ");
            sqlCommand.Append("cw.guid as workflowguid ");

            sqlCommand.Append("FROM	mp_contentworkflow cw  ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_modules m ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("cw.moduleguid = m.guid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_pagemodules pm ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_pages p ");
            sqlCommand.Append("ON ");
            sqlCommand.Append("pm.pageid = p.pageid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cw.siteguid = :siteguid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("cw.status = :status ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("cw.createddateutc DESC ");


            sqlCommand.Append("LIMIT  :pagesize");

            if (pageNumber > 1)
                sqlCommand.Append(" OFFSET :pageoffset ");

            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int CreateAuditHistory(
            Guid guid,
            Guid workflowGuid,
            Guid moduleGuid,
            Guid userGuid,
            DateTime createdDateUtc,
            string status,
            string notes,
            bool active)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_contentworkflowaudithistory (");
            sqlCommand.Append("guid, ");
            sqlCommand.Append("contentworkflowguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("createddateutc, ");
            sqlCommand.Append("newstatus, ");
            sqlCommand.Append("notes, ");
            sqlCommand.Append("active )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":guid, ");
            sqlCommand.Append(":contentworkflowguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":createddateutc, ");
            sqlCommand.Append(":newstatus, ");
            sqlCommand.Append(":notes, ");
            sqlCommand.Append(":active ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[8];

            arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new NpgsqlParameter(":contentworkflowguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = workflowGuid.ToString();

            arParams[2] = new NpgsqlParameter(":moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new NpgsqlParameter(":userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new NpgsqlParameter(":createddateutc", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = createdDateUtc;

            arParams[5] = new NpgsqlParameter(":newstatus", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = status;

            arParams[6] = new NpgsqlParameter(":notes", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = notes;

            arParams[7] = new NpgsqlParameter(":active", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = active;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        public static bool DeactivateAudit(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_contentworkflowaudithistory ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("active = false ");

            sqlCommand.Append("WHERE  ");
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



    }
}
