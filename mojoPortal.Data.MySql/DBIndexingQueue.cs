///							DBIndexingQueue.cs
/// Author:					
/// Created:				2008-06-18
/// Last Modified:			2013-01-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data
{
    public static class DBIndexingQueue
    {
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
            #region Bit Conversion
            int intRemoveOnly;
            if (removeOnly)
            {
                intRemoveOnly = 1;
            }
            else
            {
                intRemoveOnly = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_IndexingQueue (");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("IndexPath, ");
            sqlCommand.Append("SerializedItem, ");
            sqlCommand.Append("ItemKey, ");
            sqlCommand.Append("RemoveOnly )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?SiteID, ");
            sqlCommand.Append("?IndexPath, ");
            sqlCommand.Append("?SerializedItem, ");
            sqlCommand.Append("?ItemKey, ");
            sqlCommand.Append("?RemoveOnly )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?IndexPath", MySqlDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            arParams[1] = new MySqlParameter("?SerializedItem", MySqlDbType.LongText);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = serializedItem;

            arParams[2] = new MySqlParameter("?ItemKey", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = itemKey;

            arParams[3] = new MySqlParameter("?RemoveOnly", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intRemoveOnly;

            arParams[4] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteId;

            Int64 newID = -1;

            try
            {
                newID = Convert.ToInt64(MySqlHelper.ExecuteScalar(
                    ConnectionString.GetWriteConnectionString(),
                    sqlCommand.ToString(),
                    arParams).ToString());
            }
            catch (MySqlException) { }

            return newID;

        }


        ///// <summary>
        ///// Updates a row in the mp_IndexingQueue table. Returns true if row updated.
        ///// </summary>
        ///// <param name="rowId"> rowId </param>
        ///// <param name="indexPath"> indexPath </param>
        ///// <param name="serializedItem"> serializedItem </param>
        ///// <param name="itemKey"> itemKey </param>
        ///// <param name="removeOnly"> removeOnly </param>
        ///// <returns>bool</returns>
        //public static bool Update(
        //    Int64 rowId,
        //    string indexPath,
        //    string serializedItem,
        //    string itemKey,
        //    bool removeOnly)
        //{
        //    #region Bit Conversion

        //    int intRemoveOnly;
        //    if (removeOnly)
        //    {
        //        intRemoveOnly = 1;
        //    }
        //    else
        //    {
        //        intRemoveOnly = 0;
        //    }


        //    #endregion

        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_IndexingQueue ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("IndexPath = ?IndexPath, ");
        //    sqlCommand.Append("SerializedItem = ?SerializedItem, ");
        //    sqlCommand.Append("ItemKey = ?ItemKey, ");
        //    sqlCommand.Append("RemoveOnly = ?RemoveOnly ");

        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("RowId = ?RowId ");
        //    sqlCommand.Append(";");

        //    MySqlParameter[] arParams = new MySqlParameter[5];

        //    arParams[0] = new MySqlParameter("?RowId", MySqlDbType.Int64);
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = rowId;

        //    arParams[1] = new MySqlParameter("?IndexPath", MySqlDbType.VarChar, 255);
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = indexPath;

        //    arParams[2] = new MySqlParameter("?SerializedItem", MySqlDbType.Text);
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = serializedItem;

        //    arParams[3] = new MySqlParameter("?ItemKey", MySqlDbType.VarChar, 255);
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = itemKey;

        //    arParams[4] = new MySqlParameter("?RemoveOnly", MySqlDbType.Int32);
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = intRemoveOnly;

        //    int rowsAffected = MySqlHelper.ExecuteNonQuery(
        //        ConnectionString.GetWriteConnectionString(),
        //        sqlCommand.ToString(),
        //        arParams);

        //    return (rowsAffected > -1);

        //}

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
            sqlCommand.Append("RowId = ?RowId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?RowId", MySqlDbType.Int64);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

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

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                null);
            return (rowsAffected > 0);

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

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("IndexPath = ?IndexPath ");
            sqlCommand.Append("ORDER BY RowId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?IndexPath", MySqlDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;

            DataTable dt = new DataTable();
            dt.Columns.Add("RowId", typeof(int));
            dt.Columns.Add("IndexPath", typeof(String));
            dt.Columns.Add("SerializedItem", typeof(String));
            dt.Columns.Add("ItemKey", typeof(String));
            dt.Columns.Add("RemoveOnly", typeof(bool));

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["RowId"] = reader["RowId"];
                    row["IndexPath"] = reader["IndexPath"];
                    row["SerializedItem"] = reader["SerializedItem"];
                    row["ItemKey"] = reader["ItemKey"];
                    row["RemoveOnly"] = Convert.ToBoolean(reader["RemoveOnly"]);

                    dt.Rows.Add(row);

                }
            }

            return dt;

        }

        public static DataTable GetBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = ?SiteID ");
            sqlCommand.Append("ORDER BY RowId ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            DataTable dt = new DataTable();
            dt.Columns.Add("RowId", typeof(int));
            dt.Columns.Add("IndexPath", typeof(String));
            dt.Columns.Add("SerializedItem", typeof(String));
            dt.Columns.Add("ItemKey", typeof(String));
            dt.Columns.Add("RemoveOnly", typeof(bool));

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {

                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["RowId"] = reader["RowId"];
                    row["IndexPath"] = reader["IndexPath"];
                    row["SerializedItem"] = reader["SerializedItem"];
                    row["ItemKey"] = reader["ItemKey"];
                    row["RemoveOnly"] = Convert.ToBoolean(reader["RemoveOnly"]);

                    dt.Rows.Add(row);

                }
            }

            return dt;

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_IndexingQueue table.
        /// </summary>
        public static DataTable GetIndexPaths()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT  IndexPath ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("ORDER BY IndexPath ");
            sqlCommand.Append(";");

            IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

            return DBPortal.GetTableFromDataReader(reader);
        }

        public static DataTable GetSiteIDs()
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT DISTINCT  SiteID ");
            sqlCommand.Append("FROM	mp_IndexingQueue ");
            sqlCommand.Append("ORDER BY SiteID ");
            sqlCommand.Append(";");

            IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        

        
    }
}
