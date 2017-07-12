/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2010-07-02
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
    
    public static class DBGallery
    {
        
       
        public static int AddGalleryImage(
            Guid itemGuid,
            Guid moduleGuid,
            int moduleId,
            int displayOrder,
            string caption,
            string description,
            string metaDataXml,
            string imageFile,
            string webImageFile,
            string thumbnailFile,
            DateTime uploadDate,
            string uploadUser,
            Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GalleryImages_Insert", 13);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ModuleGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, moduleGuid);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Caption", SqlDbType.NVarChar, 255, ParameterDirection.Input, caption);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@MetaDataXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, metaDataXml);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@WebImageFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, webImageFile);
            sph.DefineSqlParameter("@ThumbnailFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, thumbnailFile);
            sph.DefineSqlParameter("@UploadDate", SqlDbType.DateTime, ParameterDirection.Input, uploadDate);
            sph.DefineSqlParameter("@UploadUser", SqlDbType.NVarChar, 100, ParameterDirection.Input, uploadUser);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool UpdateGalleryImage(
          int itemId,
          int moduleId,
          int displayOrder,
          string caption,
          string description,
          string metaDataXml,
          string imageFile,
          string webImageFile,
          string thumbnailFile,
          DateTime uploadDate,
          string uploadUser)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GalleryImages_Update", 11);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Caption", SqlDbType.NVarChar, 255, ParameterDirection.Input, caption);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@MetaDataXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, metaDataXml);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@WebImageFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, webImageFile);
            sph.DefineSqlParameter("@ThumbnailFile", SqlDbType.NVarChar, 100, ParameterDirection.Input, thumbnailFile);
            sph.DefineSqlParameter("@UploadDate", SqlDbType.DateTime, ParameterDirection.Input, uploadDate);
            sph.DefineSqlParameter("@UploadUser", SqlDbType.NVarChar, 100, ParameterDirection.Input, uploadUser);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteGalleryImage(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GalleryImages_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GalleryImages_DeleteByModule", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_GalleryImages_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);

        }

        public static IDataReader GetGalleryImage(int itemId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GalleryImages_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetAllImages(int moduleId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GalleryImages_Select", 1);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetImagesByPage(int siteId, int pageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GalleryImages_SelectByPage", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageId);
            return sph.ExecuteReader();
        }

        public static DataTable GetThumbsByPage(int moduleId, int pageNumber, int thumbsPerPage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GalleryImages_SelectThumbsByPage", 3);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, thumbsPerPage);

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("Caption", typeof(String));
            dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("WebImageFile", typeof(String));
            //dt.Columns.Add("ImageFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    row["Caption"] = reader["Caption"];
                    row["ThumbnailFile"] = reader["ThumbnailFile"];
                    row["WebImageFile"] = reader["WebImageFile"];
                   // row["ImageFile"] = reader["ImageFile"];
                    row["TotalPages"] = reader["TotalPages"];

                    dt.Rows.Add(row);

                }

            }
            return dt;
        }



        public static DataTable GetWebImageByPage(int moduleId, int pageNumber)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_GalleryImages_SelectWebImageByPage", 2);
            sph.DefineSqlParameter("@ModuleID", SqlDbType.Int, ParameterDirection.Input, moduleId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    row["TotalPages"] = reader["TotalPages"];
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }




    }
}
