/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2013-02-18
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
    
    public static class DBSharedFiles
    {
        
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



        public static int AddSharedFileFolder(
            Guid folderGuid,
            Guid moduleGuid,
            Guid parentGuid,
            int moduleId,
            string folderName,
            int parentId)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SharedFileFolders (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("FolderName, ");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("FolderGuid, ");
            sqlCommand.Append("ParentGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":FolderName, ");
            sqlCommand.Append(":ParentID, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":FolderGuid, ");
            sqlCommand.Append(":ParentGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":FolderName", DbType.String, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderName;

            arParams[2] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = parentId;

            arParams[3] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new SqliteParameter(":FolderGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = folderGuid.ToString();

            arParams[5] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = parentGuid.ToString();

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool UpdateSharedFileFolder(
            int folderId,
            int moduleId,
            string folderName,
            int parentId,
            Guid parentGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SharedFileFolders ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = :ModuleID, ");
            sqlCommand.Append("FolderName = :FolderName, ");
            sqlCommand.Append("ParentID = :ParentID, ");
            sqlCommand.Append("ParentGuid = :ParentGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("FolderID = :FolderID ;");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqliteParameter(":FolderName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;

            arParams[3] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = parentId;

            arParams[4] = new SqliteParameter(":ParentGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = parentGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteSharedFileFolder(int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderID = :FolderID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetSharedFileFolder(int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFileFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderID = :FolderID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSharedModuleFolders(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.SizeInKB),0) ");
            sqlCommand.Append("FROM mp_SharedFiles sf ");
            sqlCommand.Append("WHERE sf.FolderID = sff.FolderID) As SizeInKB ");

            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = :ModuleID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFolders(int moduleId, int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.SizeInKB),0) ");
            sqlCommand.Append("FROM mp_SharedFiles sf ");
            sqlCommand.Append("WHERE sf.FolderID = sff.FolderID) As SizeInKB ");

            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = :ModuleID ");
            sqlCommand.Append("AND sff.ParentID = :ParentID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ParentID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int AddSharedFile(
            Guid itemGuid,
            Guid moduleGuid,
            Guid userGuid,
            Guid folderGuid,
            int moduleId,
            int uploadUserId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int folderId,
            string description)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SharedFiles (");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("UploadUserID, ");
            sqlCommand.Append("FriendlyName, ");
            sqlCommand.Append("OriginalFileName, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("SizeInKB, ");
            sqlCommand.Append("UploadDate, ");
            sqlCommand.Append("FolderID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("DownloadCount, ");
            sqlCommand.Append("FolderGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":UploadUserID, ");
            sqlCommand.Append(":FriendlyName, ");
            sqlCommand.Append(":OriginalFileName, ");
            sqlCommand.Append(":ServerFileName, ");
            sqlCommand.Append(":SizeInKB, ");
            sqlCommand.Append(":UploadDate, ");
            sqlCommand.Append(":FolderID, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid, ");
            sqlCommand.Append(":Description, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":FolderGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[13];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":UploadUserID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = uploadUserId;

            arParams[2] = new SqliteParameter(":FriendlyName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new SqliteParameter(":OriginalFileName", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new SqliteParameter(":ServerFileName", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new SqliteParameter(":SizeInKB", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new SqliteParameter(":UploadDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = folderId;

            arParams[8] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new SqliteParameter(":FolderGuid", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = folderGuid.ToString();

            arParams[12] = new SqliteParameter(":Description", DbType.Object);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            int newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return newID;

        }


        public static bool UpdateSharedFile(
            int itemId,
            int moduleId,
            int uploadUserId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int folderId,
            Guid folderGuid,
            Guid userGuid,
            string description)
        {

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SharedFiles ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("ModuleID = :ModuleID, ");
            sqlCommand.Append("UploadUserID = :UploadUserID, ");
            sqlCommand.Append("FriendlyName = :FriendlyName, ");
            sqlCommand.Append("OriginalFileName = :OriginalFileName, ");
            sqlCommand.Append("ServerFileName = :ServerFileName, ");
            sqlCommand.Append("SizeInKB = :SizeInKB, ");
            sqlCommand.Append("UploadDate = :UploadDate, ");
            sqlCommand.Append("FolderID = :FolderID, ");
            sqlCommand.Append("UserGuid = :UserGuid, ");
            sqlCommand.Append("Description = :Description, ");
            sqlCommand.Append("FolderGuid = :FolderGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[12];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqliteParameter(":UploadUserID", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uploadUserId;

            arParams[3] = new SqliteParameter(":FriendlyName", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = friendlyName;

            arParams[4] = new SqliteParameter(":OriginalFileName", DbType.String, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = originalFileName;

            arParams[5] = new SqliteParameter(":ServerFileName", DbType.String, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = serverFileName;

            arParams[6] = new SqliteParameter(":SizeInKB", DbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sizeInKB;

            arParams[7] = new SqliteParameter(":UploadDate", DbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadDate;

            arParams[8] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = folderId;

            arParams[9] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            arParams[10] = new SqliteParameter(":FolderGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = folderGuid.ToString();

            arParams[11] = new SqliteParameter(":Description", DbType.Object);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = description;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool IncrementDownloadCount(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SharedFiles ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("DownloadCount = DownloadCount + 1 ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }


        public static bool DeleteSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles ");
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
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID = :ModuleID;");
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID = :ModuleID;");
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID = :ModuleID;");

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
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID);");
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID);");
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = :SiteID);");

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

        public static IDataReader GetSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("UploadUserID, ");
            sqlCommand.Append("FriendlyName, ");
            sqlCommand.Append("OriginalFileName, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("SizeInKB, ");
            sqlCommand.Append("UploadDate, ");
            sqlCommand.Append("FolderID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("FolderGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("DownloadCount, ");
            sqlCommand.Append("ModuleGuid ");

            sqlCommand.Append("FROM	mp_SharedFiles ");
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


        public static IDataReader GetSharedFiles(int moduleId, int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sf.ItemID, ");
            sqlCommand.Append("sf.ModuleID, ");
            sqlCommand.Append("sf.UploadUserID, ");
            sqlCommand.Append("sf.FriendlyName, ");
            sqlCommand.Append("sf.OriginalFileName, ");
            sqlCommand.Append("sf.ServerFileName, ");
            sqlCommand.Append("sf.SizeInKB, ");
            sqlCommand.Append("sf.UploadDate, ");
            sqlCommand.Append("sf.FolderID, ");
            sqlCommand.Append("sf.ItemGuid, ");
            sqlCommand.Append("sf.FolderGuid, ");
            sqlCommand.Append("sf.UserGuid, ");
            sqlCommand.Append("sf.ModuleGuid, ");
            sqlCommand.Append("sf.Description, ");
            sqlCommand.Append("sf.DownloadCount, ");
            sqlCommand.Append("u.Name As UserName ");

            sqlCommand.Append("FROM	mp_SharedFiles sf ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_Users u ");
            sqlCommand.Append("ON sf.UploadUserID = u.UserID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.ModuleID = :ModuleID ");
            sqlCommand.Append("AND sf.FolderID = :FolderID ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":FolderID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSharedFiles(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sf.ItemID, ");
            sqlCommand.Append("sf.ModuleID, ");
            sqlCommand.Append("sf.UploadUserID, ");
            sqlCommand.Append("sf.FriendlyName, ");
            sqlCommand.Append("sf.OriginalFileName, ");
            sqlCommand.Append("sf.ServerFileName, ");
            sqlCommand.Append("sf.SizeInKB, ");
            sqlCommand.Append("sf.UploadDate, ");
            sqlCommand.Append("sf.FolderID, ");
            sqlCommand.Append("sf.ItemGuid, ");
            sqlCommand.Append("sf.FolderGuid, ");
            sqlCommand.Append("sf.UserGuid, ");
            sqlCommand.Append("sf.ModuleGuid, ");
            sqlCommand.Append("sf.Description, ");
            sqlCommand.Append("sf.DownloadCount ");

            sqlCommand.Append("FROM	mp_SharedFiles sf ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.ModuleID = :ModuleID ");
            
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ce.ItemID, ");
            sqlCommand.Append("ce.ModuleID, ");
            sqlCommand.Append("ce.UploadUserID, ");
            sqlCommand.Append("ce.FriendlyName, ");
            sqlCommand.Append("ce.OriginalFileName, ");
            sqlCommand.Append("ce.ServerFileName, ");
            sqlCommand.Append("ce.SizeInKB, ");
            sqlCommand.Append("ce.UploadDate, ");
            sqlCommand.Append("ce.FolderID, ");
            sqlCommand.Append("ce.ItemGuid, ");
            sqlCommand.Append("ce.FolderGuid, ");
            sqlCommand.Append("ce.UserGuid, ");
            sqlCommand.Append("ce.ModuleGuid, ");
            sqlCommand.Append("ce.Description, ");
            sqlCommand.Append("ce.DownloadCount, ");
            sqlCommand.Append("m.ModuleTitle As ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles As ViewRoles, ");
            sqlCommand.Append("md.FeatureName As FeatureName ");

            sqlCommand.Append("FROM	mp_SharedFiles ce ");

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
            sqlCommand.Append(" ; ");

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


        public static bool AddHistory(
            Guid itemGuid,
            Guid moduleGuid,
            Guid userGuid,
            int itemId,
            int moduleId,
            string friendlyName,
            string originalFileName,
            string serverFileName,
            int sizeInKB,
            DateTime uploadDate,
            int uploadUserId,
            DateTime archiveDate)
        {


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SharedFilesHistory (");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("FriendlyName, ");
            sqlCommand.Append("OriginalFileName, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("SizeInKB, ");
            sqlCommand.Append("UploadDate, ");
            sqlCommand.Append("UploadUserID, ");
            sqlCommand.Append("ArchiveDate, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ItemID, ");
            sqlCommand.Append(":ModuleID, ");
            sqlCommand.Append(":FriendlyName, ");
            sqlCommand.Append(":OriginalFileName, ");
            sqlCommand.Append(":ServerFileName, ");
            sqlCommand.Append(":SizeInKB, ");
            sqlCommand.Append(":UploadDate, ");
            sqlCommand.Append(":UploadUserID, ");
            sqlCommand.Append(":ArchiveDate, ");
            sqlCommand.Append(":ItemGuid, ");
            sqlCommand.Append(":ModuleGuid, ");
            sqlCommand.Append(":UserGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ROWID();");

            SqliteParameter[] arParams = new SqliteParameter[12];

            arParams[0] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqliteParameter(":FriendlyName", DbType.String, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new SqliteParameter(":OriginalFileName", DbType.String, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new SqliteParameter(":ServerFileName", DbType.String, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new SqliteParameter(":SizeInKB", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new SqliteParameter(":UploadDate", DbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new SqliteParameter(":UploadUserID", DbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadUserId;

            arParams[8] = new SqliteParameter(":ArchiveDate", DbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = archiveDate;

            arParams[9] = new SqliteParameter(":ItemGuid", DbType.String, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid.ToString();

            arParams[10] = new SqliteParameter(":ModuleGuid", DbType.String, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid.ToString();

            int newID = 0;

            newID = Convert.ToInt32(SqliteHelper.ExecuteScalar(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return (newID > 0);

        }



        public static bool DeleteHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteHistoryByItemID(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
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

        public static IDataReader GetHistory(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ");
            sqlCommand.Append("AND ItemID = :ItemID ;");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqliteParameter(":ItemID", DbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }

        public static IDataReader GetHistoryByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = :ModuleID ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ModuleID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);



        }


        public static IDataReader GetHistoryFile(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = :ID ;");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ID", DbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }






    }

}
