// Author:					Joe Audette
// Created:					2010-04-06
// Last Modified:			2010-04-20
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Data.SqlServerCe;

namespace mojoPortal.Data
{
    public static class DBWebPartContent
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }


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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_WebParts ");
            sqlCommand.Append("(");
            sqlCommand.Append("WebPartID, ");
            sqlCommand.Append("SiteID, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("ImageUrl, ");
            sqlCommand.Append("ClassName, ");
            sqlCommand.Append("AssemblyName, ");
            sqlCommand.Append("AvailableForMyPage, ");
            sqlCommand.Append("AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("AvailableForContentSystem, ");
            sqlCommand.Append("CountOfUseOnMyPage, ");
            sqlCommand.Append("SiteGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@WebPartID, ");
            sqlCommand.Append("@SiteID, ");
            sqlCommand.Append("@Title, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@ImageUrl, ");
            sqlCommand.Append("@ClassName, ");
            sqlCommand.Append("@AssemblyName, ");
            sqlCommand.Append("@AvailableForMyPage, ");
            sqlCommand.Append("@AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("@AvailableForContentSystem, ");
            sqlCommand.Append("@CountOfUseOnMyPage, ");
            sqlCommand.Append("@SiteGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@WebPartID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@ImageUrl", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new SqlCeParameter("@ClassName", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new SqlCeParameter("@AssemblyName", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new SqlCeParameter("@AvailableForMyPage", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = availableForMyPage;

            arParams[8] = new SqlCeParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultipleInstancesOnMyPage;

            arParams[9] = new SqlCeParameter("@AvailableForContentSystem", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = availableForContentSystem;

            arParams[10] = new SqlCeParameter("@CountOfUseOnMyPage", SqlDbType.Int);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = 0;

            arParams[11] = new SqlCeParameter("@SiteGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = siteGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_WebParts ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteID = @SiteID, ");
            sqlCommand.Append("Title = @Title, ");
            sqlCommand.Append("Description = @Description, ");
            sqlCommand.Append("ImageUrl = @ImageUrl, ");
            sqlCommand.Append("ClassName = @ClassName, ");
            sqlCommand.Append("AssemblyName = @AssemblyName, ");
            sqlCommand.Append("AvailableForMyPage = @AvailableForMyPage, ");
            sqlCommand.Append("AllowMultipleInstancesOnMyPage = @AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("AvailableForContentSystem = @AvailableForContentSystem ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("WebPartID = @WebPartID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[10];

            arParams[0] = new SqlCeParameter("@WebPartID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId;

            arParams[1] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new SqlCeParameter("@Title", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new SqlCeParameter("@Description", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new SqlCeParameter("@ImageUrl", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new SqlCeParameter("@ClassName", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new SqlCeParameter("@AssemblyName", SqlDbType.NVarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new SqlCeParameter("@AvailableForMyPage", SqlDbType.Bit);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = availableForMyPage;

            arParams[8] = new SqlCeParameter("@AllowMultipleInstancesOnMyPage", SqlDbType.Bit);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = allowMultipleInstancesOnMyPage;

            arParams[9] = new SqlCeParameter("@AvailableForContentSystem", SqlDbType.Bit);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = availableForContentSystem;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_WebParts ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("CountOfUseOnMyPage = CountOfUseOnMyPage + @Increment ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("WebPartID = @WebPartID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@WebPartID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId;

            arParams[1] = new SqlCeParameter("@Increment", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = increment;

            

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteWebPart(Guid webPartId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WebPartID = @WebPartID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@WebPartID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetWebPart(Guid webPartId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WebPartID = @WebPartID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@WebPartID", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SelectBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AvailableForContentSystem = 1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static DataTable SelectPage(
            int siteId,
            int pageNumber,
            int pageSize,
            bool sortByClassName,
            bool sortByAssemblyName)
        {
            
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

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID ");
            sqlCommand.Append("AND AvailableForContentSystem = 1 ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            using (IDataReader reader = SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams))
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
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM ");
            sqlCommand.Append("( ");
            sqlCommand.Append("SELECT m.ModuleID ");
            sqlCommand.Append("FROM mp_Modules m ");
            sqlCommand.Append("JOIN mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("m.SiteID = @SiteID ");
            sqlCommand.Append("AND m.AvailableForMyPage = 1 ");

            sqlCommand.Append("UNION ");

            sqlCommand.Append("SELECT 0 As ModuleID ");
            sqlCommand.Append("FROM	mp_WebParts w ");
            sqlCommand.Append("WHERE w.SiteID = @SiteID ");
            sqlCommand.Append("AND w.AvailableForMyPage = 1 ");

            sqlCommand.Append(") dt ");
            

            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetMostPopular(int siteId, int numberToGet)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("SELECT TOP (" + numberToGet.ToString(CultureInfo.InvariantCulture) + ") dt.* FROM  ");
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
            sqlCommand.Append("'00000000-0000-0000-0000-000000000000' As WebPartID ");

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
            //sqlCommand.Append("CONVERT(varchar(36),w.WebPartID) As WebPartID ");
            sqlCommand.Append("w.WebPartID ");
            sqlCommand.Append("FROM  mp_WebParts w ");
            sqlCommand.Append("WHERE w.SiteID = @SiteID AND w.AvailableForMyPage = 1 ");
            sqlCommand.Append(" ) dt   ");
            sqlCommand.Append("ORDER BY dt.CountOfUseOnMyPage DESC, dt.ModuleTitle ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
        }

        public static int Count(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE SiteID = @SiteID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));
        }

    }
}
