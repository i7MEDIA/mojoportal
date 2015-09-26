/// Author:				Joe Audette
/// Created:			2007-11-15
/// Last Modified:		2007-11-15
/// 
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
using mojoPortal.Data;


namespace WebStore.Data
{
    /// <summary>
    
    ///  
    /// </summary>
    public static class DBProductFile
    {
        
        

        public static int Add(
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_ProductFile_Insert", 7);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileName);
            sph.DefineSqlParameter("@FileImage", SqlDbType.Image, ParameterDirection.Input, fileImage);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@ByteLength", SqlDbType.Int, ParameterDirection.Input, byteLength);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

            

        }


        public static bool Update(
            Guid productGuid,
            string fileName,
            byte[] fileImage,
            string serverFileName,
            int byteLength,
            DateTime created,
            Guid createdBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_ProductFile_Update", 7);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@FileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileName);
            sph.DefineSqlParameter("@FileImage", SqlDbType.Image, ParameterDirection.Input, fileImage);
            sph.DefineSqlParameter("@ServerFileName", SqlDbType.NVarChar, 255, ParameterDirection.Input, serverFileName);
            sph.DefineSqlParameter("@ByteLength", SqlDbType.Int, ParameterDirection.Input, byteLength);
            sph.DefineSqlParameter("@Created", SqlDbType.DateTime, ParameterDirection.Input, created);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.UniqueIdentifier, ParameterDirection.Input, createdBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
            
            

        }

        public static bool Delete(Guid productGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetWriteConnectionString(), "ws_ProductFile_Delete", 1);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

            
        }

        public static IDataReader Get(Guid productGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(WebStoreConnectionString.GetReadConnectionString(), "ws_ProductFile_SelectOne", 1);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            return sph.ExecuteReader();
            

        }

        


    }
}
