using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;


namespace mojoPortal.Data
{
    /// <summary>
    /// Author:					Joe Audette
    /// Created:				2007-11-03
    /// Last Modified:			2008-02-02
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
        
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            #region Bit Conversion

            int intAvailableForMyPage;
            if (availableForMyPage)
            {
                intAvailableForMyPage = 1;
            }
            else
            {
                intAvailableForMyPage = 0;
            }

            int intAllowMultipleInstancesOnMyPage;
            if (allowMultipleInstancesOnMyPage)
            {
                intAllowMultipleInstancesOnMyPage = 1;
            }
            else
            {
                intAllowMultipleInstancesOnMyPage = 0;
            }

            int intAvailableForContentSystem;
            if (availableForContentSystem)
            {
                intAvailableForContentSystem = 1;
            }
            else
            {
                intAvailableForContentSystem = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_WebParts (");
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
            sqlCommand.Append("SiteGuid )"); 

            sqlCommand.Append(" VALUES (");
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
            sqlCommand.Append("@SiteGuid )");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[12];

            arParams[0] = new FbParameter("@WebPartID", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            arParams[1] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@ImageUrl", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new FbParameter("@ClassName", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new FbParameter("@AssemblyName", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new FbParameter("@AvailableForMyPage", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAvailableForMyPage;

            arParams[8] = new FbParameter("@AllowMultipleInstancesOnMyPage", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intAllowMultipleInstancesOnMyPage;

            arParams[9] = new FbParameter("@AvailableForContentSystem", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intAvailableForContentSystem;

            arParams[10] = new FbParameter("@CountOfUseOnMyPage", FbDbType.Integer);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = 0;

            arParams[11] = new FbParameter("@SiteGuid", FbDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = siteGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            #region Bit Conversion

            int intAvailableForMyPage;
            if (availableForMyPage)
            {
                intAvailableForMyPage = 1;
            }
            else
            {
                intAvailableForMyPage = 0;
            }

            int intAllowMultipleInstancesOnMyPage;
            if (allowMultipleInstancesOnMyPage)
            {
                intAllowMultipleInstancesOnMyPage = 1;
            }
            else
            {
                intAllowMultipleInstancesOnMyPage = 0;
            }

            int intAvailableForContentSystem;
            if (availableForContentSystem)
            {
                intAvailableForContentSystem = 1;
            }
            else
            {
                intAvailableForContentSystem = 0;
            }


            #endregion

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
            sqlCommand.Append("WebPartID = @WebPartID ;");

            FbParameter[] arParams = new FbParameter[10];

            arParams[0] = new FbParameter("@WebPartID", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            arParams[1] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            arParams[2] = new FbParameter("@Title", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new FbParameter("@Description", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new FbParameter("@ImageUrl", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = imageUrl;

            arParams[5] = new FbParameter("@ClassName", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = className;

            arParams[6] = new FbParameter("@AssemblyName", FbDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = assemblyName;

            arParams[7] = new FbParameter("@AvailableForMyPage", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intAvailableForMyPage;

            arParams[8] = new FbParameter("@AllowMultipleInstancesOnMyPage", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intAllowMultipleInstancesOnMyPage;

            arParams[9] = new FbParameter("@AvailableForContentSystem", FbDbType.Integer);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intAvailableForContentSystem;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_WebParts ");
            sqlCommand.Append("SET CountOfUseOnMyPage = CountOfUseOnMyPage  + " + increment.ToString() + " ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WebPartID = @WebPartID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@WebPartID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static bool DeleteWebPart(Guid webPartId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WebPartID = @WebPartID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@WebPartID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetWebPart(Guid webPartId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("WebPartID = @WebPartID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@WebPartID", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = webPartId.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader SelectBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteID = @SiteID AND AvailableForContentSystem = 1 ");
            sqlCommand.Append("ORDER BY Title, ClassName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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

            int totalRows = OnlyCount(siteId);
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

            int skip = pageSize * (pageNumber - 1);

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT	 ");
            sqlCommand.Append(" FIRST " + pageSize.ToString() + " ");
            if (pageNumber > 1)
            {
                sqlCommand.Append(" SKIP " + skip.ToString() + "  ");
            }
            sqlCommand.Append("m.*,  ");
            sqlCommand.Append(totalPages.ToString() + " As TotalPages  ");
            sqlCommand.Append("FROM	mp_WebParts m  ");

            if (sortByClassName)
            {

                sqlCommand.Append("   ORDER BY 	m.ClassName, m.Title ;");

            }
            else if (sortByAssemblyName)
            {

                sqlCommand.Append("   ORDER BY 	m.AssemblyName, m.Title ;");

            }
            else
            {

                sqlCommand.Append("   ORDER BY 	m.Title, m.ClassName ;");

            }


            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;


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


            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_WebParts ");
            sqlCommand.Append("WHERE SiteID = @SiteID  ");
            sqlCommand.Append(" AND ClassName = @ClassName  ");
            sqlCommand.Append(" AND AssemblyName = @AssemblyName ; ");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@ClassName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = className;

            arParams[2] = new FbParameter("@AssemblyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = assemblyName;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }

        public static IDataReader GetWebPartsForMyPage(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("(SELECT   ");
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

            sqlCommand.Append("FROM	mp_Modules m ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 )");

            sqlCommand.Append(" UNION ");

            sqlCommand.Append("(SELECT   ");
            sqlCommand.Append("-1 As ModuleID, ");
            sqlCommand.Append("w.SiteID, ");
            sqlCommand.Append("0 As MuduleDefID, ");
            sqlCommand.Append("w.Title As ModuleTitle, ");
            sqlCommand.Append("w.AllowMultipleInstancesOnMyPage, ");
            sqlCommand.Append("w.CountOfUseOnMyPage , ");
            sqlCommand.Append("w.ImageUrl As ModuleIcon, ");
            sqlCommand.Append("w.ImageUrl As FeatureIcon, ");
            sqlCommand.Append("w.Description As FeatureName, ");
            sqlCommand.Append("'Resource' As ResourceFile, ");
            sqlCommand.Append("1 As IsAssembly, ");
            sqlCommand.Append("w.WebPartID ");

            sqlCommand.Append("FROM	mp_WebParts w ");

            sqlCommand.Append("WHERE w.SiteID = @SiteID AND w.AvailableForMyPage = 1 )");

            sqlCommand.Append("ORDER BY ModuleTitle ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetMostPopular(
            int siteId,
            int numberToGet)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT FIRST " + numberToGet.ToString() + " *  ");
            sqlCommand.Append("FROM (SELECT   ");
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

            sqlCommand.Append("FROM	mp_Modules m ");
            sqlCommand.Append("JOIN	mp_ModuleDefinitions md ");
            sqlCommand.Append("ON m.ModuleDefID = md.ModuleDefID ");

            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ");

            sqlCommand.Append(" UNION ");

            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("0 As ModuleID, ");
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
            sqlCommand.Append("w.WebPartID ");

            sqlCommand.Append("FROM	mp_WebParts w ");

            sqlCommand.Append("WHERE w.SiteID = @SiteID AND w.AvailableForMyPage = 1 ");

            sqlCommand.Append("ORDER BY 4) ");
            sqlCommand.Append(" ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static int Count(int siteId)
        {
            int count = OnlyCount(siteId);
            count += DBModule.CountForMyPage(siteId);
            return count;

        }

        public static int OnlyCount(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM mp_WebParts m  ");
            sqlCommand.Append("WHERE m.SiteID = @SiteID AND m.AvailableForMyPage = 1 ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return count;

        }





    }
}
