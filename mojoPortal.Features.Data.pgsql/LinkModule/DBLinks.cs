/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2012-08-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using Npgsql;

namespace mojoPortal.Data
{
    
    public static class DBLinks
    {
        public static int AddLink(
            Guid itemGuid,
            Guid moduleGuid,
            int moduleId,
            string title,
            string url,
            int viewOrder,
            string description,
            DateTime createdDate,
            int createdBy,
            string target,
            Guid userGuid)
        {
            
            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = url;

            arParams[3] = new NpgsqlParameter("vieworder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = viewOrder;

            arParams[4] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = createdDate;

            arParams[6] = new NpgsqlParameter("createdby", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdBy;

            arParams[7] = new NpgsqlParameter("target", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = target;

            arParams[8] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_links_insert(:moduleid,:title,:url,:vieworder,:description,:createddate,:createdby,:target,:itemguid,:moduleguid,:userguid)",
                arParams));

            return newID;

        }

        public static bool UpdateLink(
            int itemId,
            int moduleId,
            string title,
            string url,
            int viewOrder,
            string description,
            DateTime createdDate,
            string target,
            int createdBy)
        {

            NpgsqlParameter[] arParams = new NpgsqlParameter[9];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("title", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new NpgsqlParameter("url", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = url;

            arParams[4] = new NpgsqlParameter("vieworder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = viewOrder;

            arParams[5] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = description;

            arParams[6] = new NpgsqlParameter("createddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = createdDate;

            arParams[7] = new NpgsqlParameter("createdby", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = createdBy;

            arParams[8] = new NpgsqlParameter("target", NpgsqlTypes.NpgsqlDbType.Varchar, 20);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = target;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_links_update(:itemid,:moduleid,:title,:url,:vieworder,:description,:createddate,:createdby,:target)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteLink(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_links_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_links ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WHERE moduleid = :moduleid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_links ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WHERE moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetLink(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_links_selectone(:itemid)",
                arParams);

        }



        public static IDataReader GetLinks(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_links_select(:moduleid)",
                arParams);
        }


        public static IDataReader GetLinksByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("md.featurename ");

            sqlCommand.Append("FROM	mp_links ce ");

            sqlCommand.Append("JOIN	mp_modules m ");
            sqlCommand.Append("ON ce.moduleid = m.moduleid ");

            sqlCommand.Append("JOIN	mp_moduledefinitions md ");
            sqlCommand.Append("ON m.moduledefid = md.moduledefid ");

            sqlCommand.Append("JOIN	mp_pagemodules pm ");
            sqlCommand.Append("ON m.moduleid = pm.moduleid ");

            sqlCommand.Append("JOIN	mp_pages p ");
            sqlCommand.Append("ON p.pageid = pm.pageid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.siteid = :siteid ");
            sqlCommand.Append("AND pm.pageid = :pageid ");
            sqlCommand.Append(" ; ");
           
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_links ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            int moduleId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleId);

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

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new NpgsqlParameter("pageoffset", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	mp_links  ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append("ORDER BY  ");
            sqlCommand.Append("vieworder, title  ");
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

    }
}
