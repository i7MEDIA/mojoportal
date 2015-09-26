// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2010-07-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBGallery
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GalleryImages ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("DisplayOrder, ");
            sqlCommand.Append("Caption, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("MetaDataXml, ");
            sqlCommand.Append("ImageFile, ");
            sqlCommand.Append("WebImageFile, ");
            sqlCommand.Append("ThumbnailFile, ");
            sqlCommand.Append("UploadDate, ");
            sqlCommand.Append("UploadUser, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@DisplayOrder, ");
            sqlCommand.Append("@Caption, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@MetaDataXml, ");
            sqlCommand.Append("@ImageFile, ");
            sqlCommand.Append("@WebImageFile, ");
            sqlCommand.Append("@ThumbnailFile, ");
            sqlCommand.Append("@UploadDate, ");
            sqlCommand.Append("@UploadUser, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[13];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@DisplayOrder", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = displayOrder;

            arParams[2] = new SqlCeParameter("@Caption", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = caption;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@MetaDataXml", SqlDbType.NText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = metaDataXml;

            arParams[5] = new SqlCeParameter("@ImageFile", SqlDbType.NVarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = imageFile;

            arParams[6] = new SqlCeParameter("@WebImageFile", SqlDbType.NVarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = webImageFile;

            arParams[7] = new SqlCeParameter("@ThumbnailFile", SqlDbType.NVarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = thumbnailFile;

            arParams[8] = new SqlCeParameter("@UploadDate", SqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadDate;

            arParams[9] = new SqlCeParameter("@UploadUser", SqlDbType.NVarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadUser;

            arParams[10] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid;

            arParams[11] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid;

            arParams[12] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_GalleryImages ");
            sqlCommand.Append("SET  ");
           
            sqlCommand.Append("DisplayOrder = @DisplayOrder, ");
            sqlCommand.Append("Caption = @Caption, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("MetaDataXml = @MetaDataXml, ");
            sqlCommand.Append("ImageFile = @ImageFile, ");
            sqlCommand.Append("WebImageFile = @WebImageFile, ");
            sqlCommand.Append("ThumbnailFile = @ThumbnailFile, ");
            sqlCommand.Append("UploadDate = @UploadDate, ");
            sqlCommand.Append("UploadUser = @UploadUser ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[11];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@DisplayOrder", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = displayOrder;

            arParams[3] = new SqlCeParameter("@Caption", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = caption;

            arParams[4] = new SqlCeParameter("@Description", SqlDbType.NVarChar);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqlCeParameter("@MetaDataXml", SqlDbType.NVarChar);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = metaDataXml;

            arParams[6] = new SqlCeParameter("@ImageFile", SqlDbType.NVarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = imageFile;

            arParams[7] = new SqlCeParameter("@WebImageFile", SqlDbType.NVarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = webImageFile;

            arParams[8] = new SqlCeParameter("@ThumbnailFile", SqlDbType.NVarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = thumbnailFile;

            arParams[9] = new SqlCeParameter("@UploadDate", SqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadDate;

            arParams[10] = new SqlCeParameter("@UploadUser", SqlDbType.NVarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = uploadUser;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteGalleryImage(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetGalleryImage(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAllImages(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetImagesByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

            sqlCommand.Append("FROM	mp_GalleryImages ce ");

            sqlCommand.Append("JOIN	mp_Modules m ");
            sqlCommand.Append("ON ce.ModuleID = m.ModuleID ");

            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("JOIN	mp_PageModules pm ");
            sqlCommand.Append("ON m.ModuleID = pm.ModuleID ");

            sqlCommand.Append("JOIN	mp_Pages p ");
            sqlCommand.Append("ON p.PageID = pm.PageID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append("AND pm.PublishBeginDate < @CurrentTime ");
            sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > @CurrentTime)  ;");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            arParams[2] = new SqlCeParameter("@CurrentTime", SqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = DateTime.UtcNow;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetThumbsByPage(int moduleId, int pageNumber, int thumbsPerPage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int totalRows = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            int totalPages = totalRows / thumbsPerPage;
            if (totalRows <= thumbsPerPage)
            {
                totalPages = 1;
            }
            else
            {
                int remainder = 0;
                Math.DivRem(totalRows, thumbsPerPage, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            int offset = 0;
            if (pageNumber > 1) { offset = thumbsPerPage * (pageNumber - 1); }

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append(" i.ItemID,  ");
            sqlCommand.Append("i.Caption,  ");
            sqlCommand.Append("i.ThumbnailFile,  ");
            sqlCommand.Append("i.WebImageFile,  ");
            sqlCommand.Append("i.ImageFile,  ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");

            sqlCommand.Append("WHERE i.ModuleID = @ModuleID   ");

            sqlCommand.Append("ORDER BY	i.DisplayOrder, i.ItemID  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + thumbsPerPage.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");
            sqlCommand.Append(" ; ");

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("Caption", typeof(String));
            dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("WebImageFile", typeof(String));
            //dt.Columns.Add("ImageFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    row["Caption"] = reader["Caption"];
                    row["ThumbnailFile"] = reader["ThumbnailFile"];
                    row["WebImageFile"] = reader["WebImageFile"];
                    //row["ImageFile"] = reader["ImageFile"];
                    row["TotalPages"] = reader["TotalPages"];

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static DataTable GetWebImageByPage(int moduleId, int pageNumber)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int totalRows = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));


            int pageSize = 1;

            int totalPages = totalRows / pageSize;
            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder = 0;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            int offset = 0;

            if (pageNumber > 1) { offset = pageSize * (pageNumber - 1); }

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("i.ItemID,  ");
            sqlCommand.Append("i.ModuleID,  ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");
            sqlCommand.Append("WHERE i.ModuleID = @ModuleID   ");
            sqlCommand.Append("ORDER BY	i.DisplayOrder, i.ItemID  ");
            sqlCommand.Append("OFFSET " + offset.ToString(CultureInfo.InvariantCulture) + " ROWS ");
            sqlCommand.Append("FETCH NEXT " + pageSize.ToString(CultureInfo.InvariantCulture) + "ROWS ONLY ");
            
            sqlCommand.Append(" ; ");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            //dt.Columns.Add("ModuleID", typeof(int));
            //dt.Columns.Add("Caption", typeof(String));
            //dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["ItemID"] = reader["ItemID"];
                    // row["ModuleID"] = reader["ModuleID"];
                    //row["Caption"] = reader["Caption"];

                    row["TotalPages"] = reader["TotalPages"];

                    dt.Rows.Add(row);
                }

            }

            return dt;

        }

    }
}
