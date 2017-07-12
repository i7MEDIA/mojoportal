using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Configuration;

namespace mojoPortal.Data
{
    /// <summary>
    /// Author:					
    /// Created:				2007-11-03
    /// Last Modified:			2008-01-28
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
    /// 
    /// </summary>
    public static class DBWebPartContent
    {
        

        public static int AddWebPart(
            Guid webPartId,
            Guid siteGuid,
            int siteId,
            string title,
            string description,
            string imageUrl,
            string className,
            string assemblyName,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            bool availableForContentSystem)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_WebParts_Insert", 11);
            sph.DefineSqlParameter("@WebPartID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, webPartId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@ImageUrl", SqlDbType.NVarChar, ParameterDirection.Input, imageUrl);
            sph.DefineSqlParameter("@ClassName", SqlDbType.NVarChar, ParameterDirection.Input, className);
            sph.DefineSqlParameter("@AssemblyName", SqlDbType.NVarChar, ParameterDirection.Input, assemblyName);
            sph.DefineSqlParameter("@AvailableForMyPage", SqlDbType.Bit, ParameterDirection.Input, availableForMyPage);
            sph.DefineSqlParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit, ParameterDirection.Input, allowMultipleInstancesOnMyPage);
            sph.DefineSqlParameter("@AvailableForContentSystem", SqlDbType.Bit, ParameterDirection.Input, availableForContentSystem);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool UpdateWebPart(
            Guid webPartId,
            int siteId,
            string title,
            string description,
            string imageUrl,
            string className,
            string assemblyName,
            bool availableForMyPage,
            bool allowMultipleInstancesOnMyPage,
            bool availableForContentSystem)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_WebParts_Update", 10);
            sph.DefineSqlParameter("@WebPartID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, webPartId);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@ImageUrl", SqlDbType.NVarChar, ParameterDirection.Input, imageUrl);
            sph.DefineSqlParameter("@ClassName", SqlDbType.NVarChar, ParameterDirection.Input, className);
            sph.DefineSqlParameter("@AssemblyName", SqlDbType.NVarChar, ParameterDirection.Input, assemblyName);
            sph.DefineSqlParameter("@AvailableForMyPage", SqlDbType.Bit, ParameterDirection.Input, availableForMyPage);
            sph.DefineSqlParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit, ParameterDirection.Input, allowMultipleInstancesOnMyPage);
            sph.DefineSqlParameter("@AvailableForContentSystem", SqlDbType.Bit, ParameterDirection.Input, availableForContentSystem);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_WebParts_UpdateCountOfUseOnMyPage", 2);
            sph.DefineSqlParameter("@WebPartID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, webPartId);
            sph.DefineSqlParameter("@Increment", SqlDbType.Int, ParameterDirection.Input, increment);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteWebPart(Guid webPartId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_WebParts_Delete", 1);
            sph.DefineSqlParameter("@WebPartID", SqlDbType.Int, ParameterDirection.Input, webPartId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetWebPart(Guid webPartId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_WebParts_SelectOne", 1);
            sph.DefineSqlParameter("@WebPartID", SqlDbType.UniqueIdentifier, ParameterDirection.Input, webPartId);
            return sph.ExecuteReader();
        }

        public static IDataReader SelectBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_WebParts_SelectBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();
        }

        public static DataTable SelectPage(
            int siteId,
            int pageNumber,
            int pageSize,
            bool sortByClassName,
            bool sortByAssemblyName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_WebParts_SelectPage", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@SortByClassName", SqlDbType.Bit, ParameterDirection.Input, sortByClassName);
            sph.DefineSqlParameter("@SortByAssemblyName", SqlDbType.Bit, ParameterDirection.Input, sortByAssemblyName);
            
            DataTable dt = new DataTable();
            dt.Columns.Add("WebPartID", typeof(String));
            dt.Columns.Add("Title", typeof(String));
            dt.Columns.Add("Description", typeof(String));
            dt.Columns.Add("ImageUrl", typeof(String));
            dt.Columns.Add("ClassName", typeof(String));
            dt.Columns.Add("AssemblyName", typeof(String));
            dt.Columns.Add("AvailableForMyPage", typeof(bool));
            dt.Columns.Add("AllowMultipleInstancesOnMyPage", typeof(bool));
            dt.Columns.Add("AvailableForContentSystem", typeof(bool));
            dt.Columns.Add("TotalPages", typeof(int));

            using (IDataReader reader = sph.ExecuteReader())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["WebPartID"] = reader["WebPartID"].ToString();
                    row["Title"] = reader["Title"];
                    row["Description"] = reader["Description"];
                    row["ImageUrl"] = reader["ImageUrl"];
                    row["ClassName"] = reader["ClassName"];
                    row["AssemblyName"] = reader["AssemblyName"];
                    row["AvailableForMyPage"] = reader["AvailableForMyPage"];
                    row["AllowMultipleInstancesOnMyPage"] = reader["AllowMultipleInstancesOnMyPage"];
                    row["AvailableForContentSystem"] = reader["AvailableForContentSystem"];
                    row["TotalPages"] = reader["TotalPages"];

                    dt.Rows.Add(row);

                }

            }

            return dt;

        }

        public static bool Exists(Int32 siteId, String className, String assemblyName)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_WebParts_WebPartExists", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ClassName", SqlDbType.NVarChar, 255, ParameterDirection.Input, className);
            sph.DefineSqlParameter("@AssemblyName", SqlDbType.NVarChar, 255, ParameterDirection.Input, assemblyName);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return (count > 0);
        }

        public static IDataReader GetWebPartsForMyPage(int siteId)
        {
            //SqlParameterHelper sph = new SqlParameterHelper(GetConnectionString(), "mp_WebParts_GetWebPartsForMyPage", 1);
            //sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            //return sph.ExecuteReader();

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT  dt.* FROM  ");
            sqlCommand.Append(" (   ");
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.SiteID, ");
            sqlCommand.Append("m.ModuleDefID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("m.CountOfUseOnMyPage , ");
            sqlCommand.Append("m.Icon As ModuleIcon, ");
            sqlCommand.Append("md.Icon As FeatureIcon, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("0 As IsAssembly, ");
            sqlCommand.Append("'' As WebPartID ");

            sqlCommand.Append("FROM  mp_Modules m ");
            sqlCommand.Append("JOIN  mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ");


            sqlCommand.Append(" UNION ");

            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("-1 As ModuleID, ");
            sqlCommand.Append("w.SiteID, ");
            sqlCommand.Append("0 As ModuleDefID, ");
            sqlCommand.Append("w.Title As ModuleTitle, ");
            sqlCommand.Append("w.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("w.CountOfUseOnMyPage , ");
            sqlCommand.Append("w.ImageUrl As ModuleIcon, ");
            sqlCommand.Append("w.ImageUrl As FeatureIcon, ");
            sqlCommand.Append("w.Description As FeatureName, ");
            sqlCommand.Append("'Resource' As ResourceFile, ");
            sqlCommand.Append("1 As IsAssembly, ");
            sqlCommand.Append("CONVERT(varchar(36),w.WebPartID) As WebPartID ");
            sqlCommand.Append("FROM  mp_WebParts w ");
            sqlCommand.Append("WHERE w.SiteID = @SiteID AND w.AvailableForMyPage = 1 ");
            sqlCommand.Append(" ) dt   ");
            sqlCommand.Append("ORDER BY  dt.ModuleTitle ");

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), sqlCommand.ToString(), CommandType.Text, 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();

        }

        public static IDataReader GetMostPopular(int siteId, int numberToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT TOP " + numberToGet + " dt.* FROM  ");
            sqlCommand.Append(" (   ");
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("m.ModuleID, ");
            sqlCommand.Append("m.SiteID, ");
            sqlCommand.Append("m.ModuleDefID, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("m.CountOfUseOnMyPage , ");
            sqlCommand.Append("m.Icon As ModuleIcon, ");
            sqlCommand.Append("md.Icon As FeatureIcon, ");
            sqlCommand.Append("md.FeatureName, ");
            sqlCommand.Append("md.ResourceFile, ");
            sqlCommand.Append("0 As IsAssembly, ");
            sqlCommand.Append("'' As WebPartID ");

            sqlCommand.Append("FROM  mp_Modules m ");
            sqlCommand.Append("JOIN  mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ");


            sqlCommand.Append(" UNION ");

            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("-1 As ModuleID, ");
            sqlCommand.Append("w.SiteID, ");
            sqlCommand.Append("0 As ModuleDefID, ");
            sqlCommand.Append("w.Title As ModuleTitle, ");
            sqlCommand.Append("w.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("w.CountOfUseOnMyPage , ");
            sqlCommand.Append("w.ImageUrl As ModuleIcon, ");
            sqlCommand.Append("w.ImageUrl As FeatureIcon, ");
            sqlCommand.Append("w.Description As FeatureName, ");
            sqlCommand.Append("'Resource' As ResourceFile, ");
            sqlCommand.Append("1 As IsAssembly, ");
            sqlCommand.Append("CONVERT(varchar(36),w.WebPartID) As WebPartID ");
            sqlCommand.Append("FROM  mp_WebParts w ");
            sqlCommand.Append("WHERE w.SiteID = @SiteID AND w.AvailableForMyPage = 1 ");
            sqlCommand.Append(" ) dt   ");
            sqlCommand.Append("ORDER BY dt.CountOfUseOnMyPage DESC, dt.ModuleTitle ");

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), sqlCommand.ToString(), CommandType.Text, 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return sph.ExecuteReader();


        }

        public static int Count(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_WebParts_GetCount", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int count = Convert.ToInt32(sph.ExecuteScalar());
            return count;
        }



        


    }
}
