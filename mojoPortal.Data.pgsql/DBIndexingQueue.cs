/// Author:					
/// Created:				2008-06-18
/// Last Modified:			2012-08-11
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
			NpgsqlParameter[] arParams = new NpgsqlParameter[5];
			arParams[0] = new NpgsqlParameter(":indexpath", NpgsqlTypes.NpgsqlDbType.Varchar,255);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = indexPath;
			
			arParams[1] = new NpgsqlParameter(":serializeditem", NpgsqlTypes.NpgsqlDbType.Text);
			arParams[1].Direction = ParameterDirection.Input;
			arParams[1].Value = serializedItem;
			
			arParams[2] = new NpgsqlParameter(":itemkey", NpgsqlTypes.NpgsqlDbType.Varchar,255);
			arParams[2].Direction = ParameterDirection.Input;
			arParams[2].Value = itemKey;
			
			arParams[3] = new NpgsqlParameter(":removeonly", NpgsqlTypes.NpgsqlDbType.Boolean);
			arParams[3].Direction = ParameterDirection.Input;
			arParams[3].Value = removeOnly;

            arParams[4] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = siteId;
			
			
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("INSERT INTO mp_indexingqueue (");
            sqlCommand.Append("siteid, ");
			sqlCommand.Append("indexpath, ");
			sqlCommand.Append("serializeditem, ");
			sqlCommand.Append("itemkey, ");
			sqlCommand.Append("removeonly )"); 
			
			sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":siteid, ");
			sqlCommand.Append(":indexpath, ");
			sqlCommand.Append(":serializeditem, ");
			sqlCommand.Append(":itemkey, ");
			sqlCommand.Append(":removeonly )"); 
			sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_indexingqueueid_seq');");
			
			Int64 newID = Convert.ToInt64(NpgsqlHelper.ExecuteScalar(ConnectionString.GetWriteConnectionString(),
				CommandType.Text, 
				sqlCommand.ToString(),
				arParams));
			
	
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
        //    Int64  rowId, 
        //    string indexPath, 
        //    string serializedItem, 
        //    string itemKey, 
        //    bool removeOnly) 
        //{
        //    NpgsqlParameter[] arParams = new NpgsqlParameter[5];
			
        //    arParams[0] = new NpgsqlParameter(":rowid", NpgsqlTypes.NpgsqlDbType.Bigint); 
        //    arParams[0].Direction = ParameterDirection.Input;
        //    arParams[0].Value = rowId;
			
        //    arParams[1] = new NpgsqlParameter(":indexpath", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
        //    arParams[1].Direction = ParameterDirection.Input;
        //    arParams[1].Value = indexPath;
			
        //    arParams[2] = new NpgsqlParameter(":serializeditem", NpgsqlTypes.NpgsqlDbType.Text); 
        //    arParams[2].Direction = ParameterDirection.Input;
        //    arParams[2].Value = serializedItem;
			
        //    arParams[3] = new NpgsqlParameter(":itemkey", NpgsqlTypes.NpgsqlDbType.Varchar,255); 
        //    arParams[3].Direction = ParameterDirection.Input;
        //    arParams[3].Value = itemKey;
			
        //    arParams[4] = new NpgsqlParameter(":removeonly", NpgsqlTypes.NpgsqlDbType.Boolean); 
        //    arParams[4].Direction = ParameterDirection.Input;
        //    arParams[4].Value = removeOnly;
			
			
			
        //    StringBuilder sqlCommand = new StringBuilder();
        //    sqlCommand.Append("UPDATE mp_indexingqueue ");
        //    sqlCommand.Append("SET  ");
        //    sqlCommand.Append("indexpath = :indexpath, ");
        //    sqlCommand.Append("serializeditem = :serializeditem, ");
        //    sqlCommand.Append("itemkey = :itemkey, ");
        //    sqlCommand.Append("removeonly = :removeonly "); 
			
        //    sqlCommand.Append("WHERE  ");
        //    sqlCommand.Append("rowid = :rowid "); 
        //    sqlCommand.Append(";");

        //    int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
        //        CommandType.Text, 
        //        sqlCommand.ToString(), 
        //        arParams);
	
        //    return (rowsAffected > -1);
			
        //}
		
		/// <summary>
		/// Deletes a row from the mp_IndexingQueue table. Returns true if row deleted.
		/// </summary>
		/// <param name="rowId"> rowId </param>
		/// <returns>bool</returns>
		public static bool Delete(Int64  rowId) 
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];
			
			arParams[0] = new NpgsqlParameter(":rowid", NpgsqlTypes.NpgsqlDbType.Bigint);
			arParams[0].Direction = ParameterDirection.Input;
			arParams[0].Value = rowId;
				
			
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("DELETE FROM mp_indexingqueue ");
			sqlCommand.Append("WHERE ");
			sqlCommand.Append("rowid = :rowid "); 
			sqlCommand.Append(";");
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_indexingqueue ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an DataTable with rows from the mp_IndexingQueue table with the passed path.
        /// </summary>
        public static DataTable GetByPath(string indexPath)
        {
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":indexpath", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
			arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = indexPath;
			
			StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.Append("SELECT  * ");
			sqlCommand.Append("FROM	mp_indexingqueue ");
			sqlCommand.Append("WHERE ");
            sqlCommand.Append("indexpath = :indexpath "); 
			sqlCommand.Append(";");

            DataTable dt = new DataTable();
            dt.Columns.Add("RowId", typeof(int));
            dt.Columns.Add("IndexPath", typeof(String));
            dt.Columns.Add("SerializedItem", typeof(String));
            dt.Columns.Add("ItemKey", typeof(String));
            dt.Columns.Add("RemoveOnly", typeof(bool));

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("SELECT  DISTINCT indexpath ");
			sqlCommand.Append("FROM	mp_indexingqueue ");
            sqlCommand.Append("ORDER BY indexpath ");
			sqlCommand.Append(";");
			
			IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
				CommandType.Text, 
				sqlCommand.ToString(),
				null);

            return DBPortal.GetTableFromDataReader(reader);
	
		}

        public static DataTable GetSiteIDs()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  DISTINCT siteid ");
            sqlCommand.Append("FROM	mp_indexingqueue ");
            sqlCommand.Append("ORDER BY siteid ");
            sqlCommand.Append(";");

            IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        public static DataTable GetBySite(int siteId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_indexingqueue ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("siteid = :siteid ");
            sqlCommand.Append(";");

            DataTable dt = new DataTable();
            dt.Columns.Add("RowId", typeof(int));
            dt.Columns.Add("IndexPath", typeof(String));
            dt.Columns.Add("SerializedItem", typeof(String));
            dt.Columns.Add("ItemKey", typeof(String));
            dt.Columns.Add("RemoveOnly", typeof(bool));
            dt.Columns.Add("SiteID", typeof(int));

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
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
                    row["SiteID"] = Convert.ToInt32(reader["SiteID"]);

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        /// <summary>
        /// Gets a count of rows in the mp_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_indexingqueue ");
            sqlCommand.Append(";");

            return Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                null));


        }
		
        
	}
}
