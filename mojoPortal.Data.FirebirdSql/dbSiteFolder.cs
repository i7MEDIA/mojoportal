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
    
    public static class DBSiteFolder
    {
        
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }

       

        public static int Add(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SiteFolders (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FolderName )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@Guid, ");
            sqlCommand.Append("@SiteGuid, ");
            sqlCommand.Append("@FolderName );");


            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;


            int rowsAffected = 0;
            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(), arParams);

            return rowsAffected;

        }


        public static bool Update(
            Guid guid,
            Guid siteGuid,
            string folderName)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SiteFolders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SiteGuid = @SiteGuid, ");
            sqlCommand.Append("FolderName = @FolderName ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[3];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }


        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = @Guid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@Guid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = @SiteGuid ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteGuid", FbDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static Guid GetSiteGuid(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderName;

            Guid siteGuid = Guid.Empty;

            sqlCommand.Append("SELECT SiteGuid ");
            sqlCommand.Append("FROM mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = @FolderName ;");

            using (IDataReader reader = FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams))
            {
                if (reader.Read())
                {
                    siteGuid = new Guid(reader["SiteGuid"].ToString());
                }
            }

            if (siteGuid == Guid.Empty)
            {

                sqlCommand = new StringBuilder();
                sqlCommand.Append("SELECT FIRST 1 SiteGuid ");
                sqlCommand.Append("FROM	mp_Sites ");
                sqlCommand.Append("ORDER BY	SiteID ");
                sqlCommand.Append(" ;");


                using (IDataReader reader = FBSqlHelper.ExecuteReader(
                    GetConnectionString(),
                    sqlCommand.ToString(),
                    null))
                {
                    if (reader.Read())
                    {
                        siteGuid = new Guid(reader["SiteGuid"].ToString());
                    }
                }

            }

            return siteGuid;

        }

        public static bool Exists(string folderName)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT Count(*) ");
            sqlCommand.Append("FROM	mp_SiteFolders ");
            sqlCommand.Append("WHERE FolderName = @FolderName ; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderName;

            int count = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);

        }


        


    }
}
