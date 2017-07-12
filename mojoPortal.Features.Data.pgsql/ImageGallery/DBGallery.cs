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
            NpgsqlParameter[] arParams = new NpgsqlParameter[13];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("displayorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = displayOrder;

            arParams[2] = new NpgsqlParameter("caption", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = caption;

            arParams[3] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new NpgsqlParameter("metadataxml", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = metaDataXml;

            arParams[5] = new NpgsqlParameter("imagefile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = imageFile;

            arParams[6] = new NpgsqlParameter("webimagefile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = webImageFile;

            arParams[7] = new NpgsqlParameter("thumbnailfile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = thumbnailFile;

            arParams[8] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadDate;

            arParams[9] = new NpgsqlParameter("uploaduser", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadUser;

            arParams[10] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = itemGuid.ToString();

            arParams[11] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = moduleGuid.ToString();

            arParams[12] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = userGuid.ToString();

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_insert(:moduleid,:displayorder,:caption,:description,:metadataxml,:imagefile,:webimagefile,:thumbnailfile,:uploaddate,:uploaduser,:itemguid,:moduleguid,:userguid)",
                arParams));

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
            NpgsqlParameter[] arParams = new NpgsqlParameter[11];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("displayorder", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = displayOrder;

            arParams[3] = new NpgsqlParameter("caption", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = caption;

            arParams[4] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new NpgsqlParameter("metadataxml", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = metaDataXml;

            arParams[6] = new NpgsqlParameter("imagefile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = imageFile;

            arParams[7] = new NpgsqlParameter("webimagefile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = webImageFile;

            arParams[8] = new NpgsqlParameter("thumbnailfile", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = thumbnailFile;

            arParams[9] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = uploadDate;

            arParams[10] = new NpgsqlParameter("uploaduser", NpgsqlTypes.NpgsqlDbType.Text, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = uploadUser;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_update(:itemid,:moduleid,:displayorder,:caption,:description,:metadataxml,:imagefile,:webimagefile,:thumbnailfile,:uploaddate,:uploaduser)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteGalleryImage(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_galleryimages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
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
            sqlCommand.Append("DELETE FROM mp_galleryimages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
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

        public static IDataReader GetGalleryImage(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_selectone(:itemid)",
                arParams);

        }

        public static IDataReader GetAllImages(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_select(:moduleid)",
                arParams);

        }

        public static IDataReader GetImagesByPage(int siteId, int pageId)
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

            sqlCommand.Append("FROM	mp_galleryimages ce ");

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
            
            sqlCommand.Append(";");

            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }



        public static DataTable GetThumbsByPage(int moduleId, int pageNumber, int thumbsPerPage)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[3];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            arParams[2] = new NpgsqlParameter("pagesize", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = thumbsPerPage;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            dt.Columns.Add("Caption", typeof(String));
            dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("WebImageFile", typeof(String));
           // dt.Columns.Add("ImageFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_selectthumbsbypage(:moduleid,:pagenumber,:pagesize)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
           
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("pagenumber", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageNumber;

            DataTable dt = new DataTable();
            dt.Columns.Add("ItemID", typeof(int));
            //dt.Columns.Add("ModuleID", typeof(int));
            //dt.Columns.Add("Caption", typeof(String));
            //dt.Columns.Add("ThumbnailFile", typeof(String));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_galleryimages_selectwebimagebypage(:moduleid,:pagenumber)",
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
