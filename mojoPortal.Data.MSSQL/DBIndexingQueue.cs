/// Author:					
/// Created:				2008-06-18
/// Last Modified:			2010-07-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

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
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_IndexingQueue_Insert", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
            sph.DefineSqlParameter("@SerializedItem", SqlDbType.NVarChar, -1, ParameterDirection.Input, serializedItem);
            sph.DefineSqlParameter("@ItemKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemKey);
            sph.DefineSqlParameter("@RemoveOnly", SqlDbType.Bit, ParameterDirection.Input, removeOnly);
            Int64 newID = Convert.ToInt64(sph.ExecuteScalar());
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
        //    SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_IndexingQueue_Update", 5);
        //    sph.DefineSqlParameter("@RowId", SqlDbType.BigInt, ParameterDirection.Input, rowId);
        //    sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
        //    sph.DefineSqlParameter("@SerializedItem", SqlDbType.NVarChar, -1, ParameterDirection.Input, serializedItem);
        //    sph.DefineSqlParameter("@ItemKey", SqlDbType.NVarChar, 255, ParameterDirection.Input, itemKey);
        //    sph.DefineSqlParameter("@RemoveOnly", SqlDbType.Bit, ParameterDirection.Input, removeOnly);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);

        //}

        /// <summary>
        /// Deletes a row from the mp_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(Int64 rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_IndexingQueue_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.BigInt, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes all rows from the mp_IndexingQueue table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool DeleteAll()
        {
            int rowsAffected = SqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_IndexingQueue_DeleteAll",
                null);

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets a count of rows in the mp_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_IndexingQueue_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_IndexingQueue table.
        /// </summary>
        public static DataTable GetIndexPaths()
        {

            IDataReader reader = SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_IndexingQueue_SelectDistinctPaths",
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        public static DataTable GetSiteIDs()
        {

            IDataReader reader = SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_IndexingQueue_SelectDistinctSiteID",
                null);

            return DBPortal.GetTableFromDataReader(reader);

        }

        /// <summary>
        /// Gets an DataTable with rows from the mp_IndexingQueue table with the passed path.
        /// </summary>
        public static DataTable GetByPath(string indexPath)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_IndexingQueue_SelectByPath", 1);
            sph.DefineSqlParameter("@IndexPath", SqlDbType.NVarChar, 255, ParameterDirection.Input, indexPath);
            IDataReader reader = sph.ExecuteReader();

            return DBPortal.GetTableFromDataReader(reader);

        }

        public static DataTable GetBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_IndexingQueue_SelectBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            IDataReader reader = sph.ExecuteReader();

            return DBPortal.GetTableFromDataReader(reader);

        }

        

        

        

    }

}
