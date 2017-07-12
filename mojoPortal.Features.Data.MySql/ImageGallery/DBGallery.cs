/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2012-07-20
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
/// Note moved into separate class file from dbPortal 2007-11-03

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


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_GalleryImages (");
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
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?DisplayOrder, ");
            sqlCommand.Append("?Caption, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?MetaDataXml, ");
            sqlCommand.Append("?ImageFile, ");
            sqlCommand.Append("?WebImageFile, ");
            sqlCommand.Append("?ThumbnailFile, ");
            sqlCommand.Append("?UploadDate, ");
            sqlCommand.Append("?UploadUser, ");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?DisplayOrder", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = displayOrder;

            arParams[2] = new MySqlParameter("?Caption", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = caption;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?MetaDataXml", MySqlDbType.MediumText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = metaDataXml;

            arParams[5] = new MySqlParameter("?ImageFile", MySqlDbType.VarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = imageFile;

            arParams[6] = new MySqlParameter("?WebImageFile", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = webImageFile;

            arParams[7] = new MySqlParameter("?ThumbnailFile", MySqlDbType.VarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = thumbnailFile;

            arParams[8] = new MySqlParameter("?UploadDate", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadDate;

            arParams[9] = new MySqlParameter("?UploadUser", MySqlDbType.VarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadUser;

            arParams[10] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid.ToString();

            arParams[11] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid.ToString();

            arParams[12] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(), 
                sqlCommand.ToString(), 
                arParams).ToString());

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

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_GalleryImages ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = ?ModuleID, ");
            sqlCommand.Append("DisplayOrder = ?DisplayOrder, ");
            sqlCommand.Append("Caption = ?Caption, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("MetaDataXml = ?MetaDataXml, ");
            sqlCommand.Append("ImageFile = ?ImageFile, ");
            sqlCommand.Append("WebImageFile = ?WebImageFile, ");
            sqlCommand.Append("ThumbnailFile = ?ThumbnailFile, ");
            sqlCommand.Append("UploadDate = ?UploadDate, ");
            sqlCommand.Append("UploadUser = ?UploadUser ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[11];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?DisplayOrder", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = displayOrder;

            arParams[3] = new MySqlParameter("?Caption", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = caption;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?MetaDataXml", MySqlDbType.MediumText);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = metaDataXml;

            arParams[6] = new MySqlParameter("?ImageFile", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = imageFile;

            arParams[7] = new MySqlParameter("?WebImageFile", MySqlDbType.VarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = webImageFile;

            arParams[8] = new MySqlParameter("?ThumbnailFile", MySqlDbType.VarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = thumbnailFile;

            arParams[9] = new MySqlParameter("?UploadDate", MySqlDbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadDate;

            arParams[10] = new MySqlParameter("?UploadUser", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = uploadUser;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteGalleryImage(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages WHERE ModuleID = ?ModuleID;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetGalleryImage(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAllImages(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");
            sqlCommand.Append("AND pm.PublishBeginDate < now() ");
            sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > now())  ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new MySqlParameter("?PageID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetThumbsByPage(
            int moduleId,
            int pageNumber,
            int thumbsPerPage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int totalRows = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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

            int offset = thumbsPerPage * (pageNumber - 1);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append(" i.ItemID,  ");
            sqlCommand.Append("i.Caption,  ");
            sqlCommand.Append("i.ThumbnailFile,  ");
            sqlCommand.Append("i.WebImageFile,  ");
            sqlCommand.Append("i.ImageFile,  ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");

            sqlCommand.Append("WHERE i.ModuleID = ?ModuleID   ");

            sqlCommand.Append("ORDER BY	i.DisplayOrder, i.ItemID  ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
                    + ", " + thumbsPerPage.ToString(CultureInfo.InvariantCulture) + " ");
            }
            else
            {
                sqlCommand.Append("LIMIT "
                    + thumbsPerPage.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append(" ; ");


            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("Caption", typeof(String));
            dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("WebImageFile", typeof(String));
            //dt.Columns.Add("ImageFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));


            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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

        public static DataTable GetWebImageByPage(
            int moduleId,
            int pageNumber)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int totalRows = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
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

            int offset = pageSize * (pageNumber - 1);

            sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("i.ItemID,  ");
            sqlCommand.Append("i.ModuleID,  ");
            sqlCommand.Append(totalPages.ToString(CultureInfo.InvariantCulture) + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");
            sqlCommand.Append("WHERE i.ModuleID = ?ModuleID   ");
            sqlCommand.Append("ORDER BY	i.DisplayOrder, i.ItemID  ");
            if (pageNumber > 1)
            {
                sqlCommand.Append("LIMIT " + offset.ToString(CultureInfo.InvariantCulture)
                    + ", " + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            }
            else
            {
                sqlCommand.Append("LIMIT "
                    + pageSize.ToString(CultureInfo.InvariantCulture) + " ");
            }
            sqlCommand.Append(" ; ");

            arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            //dt.Columns.Add("ModuleID", typeof(int));
            //dt.Columns.Add("Caption", typeof(String));
            //dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
