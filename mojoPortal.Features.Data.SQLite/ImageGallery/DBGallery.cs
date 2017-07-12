/// Author:					
/// Created:				2007-11-03
/// Last Modified:			2009-09-03
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
using System.Web;
using Mono.Data.Sqlite;

namespace mojoPortal.Data
{
    
    public static class DBGallery
    {
        
        public static String DBPlatform()
        {
            return "SQLite";
        }

        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {
                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
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
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":DisplayOrder, ");
            sqlCommand.Append(":Caption, ");
            sqlCommand.Append(":Description, ");
            sqlCommand.Append(":MetaDataXml, ");
            sqlCommand.Append(":ImageFile, ");
            sqlCommand.Append(":WebImageFile, ");
            sqlCommand.Append(":ThumbnailFile, ");
            sqlCommand.Append(":UploadDate, ");
            sqlCommand.Append(":UploadUser, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[13];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":DisplayOrder", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = displayOrder;

            arParams[2] = new SqliteParameter(":Caption", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = caption;

            arParams[3] = new SqliteParameter(":Description", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqliteParameter(":MetaDataXml", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = metaDataXml;

            arParams[5] = new SqliteParameter(":ImageFile", DbType.String, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = imageFile;

            arParams[6] = new SqliteParameter(":WebImageFile", DbType.String, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = webImageFile;

            arParams[7] = new SqliteParameter(":ThumbnailFile", DbType.String, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = thumbnailFile;

            arParams[8] = new SqliteParameter(":UploadDate", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadDate;

            arParams[9] = new SqliteParameter(":UploadUser", DbType.String, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadUser;

            arParams[10] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid.ToString();

            arParams[11] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid.ToString();

            arParams[12] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
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
            sqlCommand.Append("ModuleID = :ModuleID, ");
            sqlCommand.Append("DisplayOrder = :DisplayOrder, ");
            sqlCommand.Append("Caption = :Caption, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("MetaDataXml = :MetaDataXml, ");
            sqlCommand.Append("ImageFile = :ImageFile, ");
            sqlCommand.Append("WebImageFile = :WebImageFile, ");
            sqlCommand.Append("ThumbnailFile = :ThumbnailFile, ");
            sqlCommand.Append("UploadDate = :UploadDate, ");
            sqlCommand.Append("UploadUser = :UploadUser ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[11];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqliteParameter(":DisplayOrder", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = displayOrder;

            arParams[3] = new SqliteParameter(":Caption", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = caption;

            arParams[4] = new SqliteParameter(":Description", DbType.Object);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new SqliteParameter(":MetaDataXml", DbType.Object);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = metaDataXml;

            arParams[6] = new SqliteParameter(":ImageFile", DbType.String, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = imageFile;

            arParams[7] = new SqliteParameter(":WebImageFile", DbType.String, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = webImageFile;

            arParams[8] = new SqliteParameter(":ThumbnailFile", DbType.String, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = thumbnailFile;

            arParams[9] = new SqliteParameter(":UploadDate", DbType.DateTime);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadDate;

            arParams[10] = new SqliteParameter(":UploadUser", DbType.String, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = uploadUser;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteGalleryImage(
            int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages WHERE ModuleID = :ModuleID;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_GalleryImages WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID);");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetAllImages(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetImagesByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ce.*, ");

            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName ");

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
            sqlCommand.Append("p.SiteID = :SiteID ");
            sqlCommand.Append("AND pm.PageID = :PageID ");
            sqlCommand.Append("AND pm.PublishBeginDate < datetime('now','localtime') ");
            sqlCommand.Append("AND (pm.PublishEndDate IS NULL OR pm.PublishEndDate > datetime('now','localtime'))  ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":SiteID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqliteParameter(":PageID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable GetThumbsByPage(int moduleId, int pageNumber, int thumbsPerPage)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_GalleryImages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();

            int pageLowerBound = (thumbsPerPage * pageNumber) - thumbsPerPage;

            int totalPages = (int)Math.Ceiling((double)count / (double)thumbsPerPage);

            sqlCommand.Append("SELECT i.ItemID As ItemID,  ");
            sqlCommand.Append("i.Caption As Caption,  ");
            sqlCommand.Append("i.ThumbnailFile As ThumbnailFile,  ");
            sqlCommand.Append("i.WebImageFile As WebImageFile,  ");
            sqlCommand.Append("i.ImageFile As ImageFile,  ");
            sqlCommand.Append(":TotalPages As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");

            sqlCommand.Append("WHERE i.ModuleID = :ModuleID   ");
            sqlCommand.Append("ORDER BY DisplayOrder    ");
            sqlCommand.Append("LIMIT " + thumbsPerPage + " ");
            sqlCommand.Append("OFFSET " + pageLowerBound + " ; ");


            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":TotalPages", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = totalPages;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("Caption", typeof(String));
            dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("WebImageFile", typeof(String));
            //dt.Columns.Add("ImageFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
                   // row["ImageFile"] = reader["ImageFile"];
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
            sqlCommand.Append("ModuleID = :ModuleID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int count = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();

            int pageLowerBound = (1 * pageNumber) - 1;

            int totalPages = (int)Math.Ceiling((double)count / (double)1);

            sqlCommand.Append("SELECT i.ItemID As ItemID,  ");
            sqlCommand.Append("i.ModuleID As ModuleID,  ");
            sqlCommand.Append(":TotalPages As TotalPages  ");
            sqlCommand.Append("FROM	mp_GalleryImages i  ");

            sqlCommand.Append("WHERE i.ModuleID = :ModuleID   ");

            sqlCommand.Append("ORDER BY 	DisplayOrder, ItemID    ");
            sqlCommand.Append("LIMIT		1 ");
            sqlCommand.Append("OFFSET		" + pageLowerBound + " ; ");



            arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":TotalPages", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = totalPages;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("TotalPages", typeof(int));


            using (IDataReader reader = SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
