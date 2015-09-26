// Author:					Joe Audette
// Created:					2010-04-04
// Last Modified:			2013-01-11
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
    public static class DBIndexingQueue
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


        /// <summary>
        /// Inserts a row in the mp_IndexingQueue table. Returns new integer id.
        /// </summary>
        /// <param name="indexPath"> indexPath </param>
        /// <param name="serializedItem"> serializedItem </param>
        /// <param name="itemKey"> itemKey </param>
        /// <param name="removeOnly"> removeOnly </param>
        /// <returns>int</returns>
        public static Int64 Create(
            int siteId,
            string indexPath,
            string serializedItem,
            string itemKey,
            bool removeOnly)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_IndexingQueue ");
            sqlCommand.Append("(");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("IndexPath, ");
            sqlCommand.Append("SerializedItem, ");
            sqlCommand.Append("ItemKey, ");
            sqlCommand.Append("RemoveOnly ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@IndexPath, ");
            sqlCommand.Append("@SerializedItem, ");
            sqlCommand.Append("@ItemKey, ");
            sqlCommand.Append("@RemoveOnly ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@IndexPath", SqlDbType.NVarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            arParams[1] = new SqlCeParameter("@SerializedItem", SqlDbType.NText);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = serializedItem;

            arParams[2] = new SqlCeParameter("@ItemKey", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = itemKey;

            arParams[3] = new SqlCeParameter("@RemoveOnly", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = removeOnly;

            arParams[4] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteId;


            Int64 newId = Convert.ToInt64(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;


        }

        /// <summary>
        /// Deletes a row from the mp_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(Int64 rowId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("RowId = @RowId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@RowId", SqlDbType.BigInt);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes all rows from the mp_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool DeleteAll()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_IndexingQueue ");
            
            sqlCommand.Append(";");

            
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets a count of rows in the mp_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append(";");

            //SqlCeParameter[] arParams = new SqlCeParameter[1];

            //arParams[0] = new SqlCeParameter("@ApplicationID", SqlDbType.UniqueIdentifier);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = applicationId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_IndexingQueue table.
        /// </summary>
        public static DataTable GetIndexPaths()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  DISTINCT IndexPath ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("IndexPath ");
            sqlCommand.Append(";");

            return DBPortal.GetTableFromDataReader(SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        public static DataTable GetSiteIDs()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  DISTINCT SiteID ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("SiteID ");
            sqlCommand.Append(";");

            return DBPortal.GetTableFromDataReader(SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));

        }

        /// <summary>
        /// Gets an DataTable with rows from the mp_IndexingQueue table with the passed path.
        /// </summary>
        public static DataTable GetByPath(string indexPath)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[IndexPath] = @IndexPath ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("RowId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@IndexPath", SqlDbType.NVarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            return DBPortal.GetTableFromDataReader(SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }

        public static DataTable GetBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("[SiteID] = @SiteID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("RowId ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return DBPortal.GetTableFromDataReader(SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

        }


    }
}
