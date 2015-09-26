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
    
    public static class DBSharedFiles
    {
        
        public static String DBPlatform()
        {
            return "FirebirdSql";
        }

        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

        }



        public static int AddSharedFileFolder(
            Guid folderGuid,
            Guid moduleGuid,
            Guid parentGuid,
            int moduleId,
            string folderName,
            int parentId)
        {
            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":FolderName", FbDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderName;

            arParams[2] = new FbParameter(":ParentID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = parentId;

            arParams[3] = new FbParameter(":ModuleGuid", FbDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new FbParameter(":FolderGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = folderGuid.ToString();

            arParams[5] = new FbParameter(":ParentGuid", FbDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = parentGuid.ToString();

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SHAREDFILEFOLDERS_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

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
            sqlCommand.Append("ModuleID = @ModuleID, ");
            sqlCommand.Append("FolderName = @FolderName, ");
            sqlCommand.Append("ParentID = @ParentID, ");
            sqlCommand.Append("ParentGuid = @ParentGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("FolderID = @FolderID ;");

            FbParameter[] arParams = new FbParameter[5];

            arParams[0] = new FbParameter("@FolderID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new FbParameter("@FolderName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;

            arParams[3] = new FbParameter("@ParentID", FbDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = parentId;

            arParams[4] = new FbParameter("@ParentGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = parentGuid.ToString();

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("FolderID = @FolderID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("FolderID = @FolderID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@FolderID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSharedModuleFolders(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.SizeInKB),0) ");
            sqlCommand.Append("FROM mp_SharedFiles sf ");
            sqlCommand.Append("WHERE sf.FolderID = sff.FolderID) As SizeInKB ");

            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFolders(int moduleId, int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.SizeInKB),0) ");
            sqlCommand.Append("FROM mp_SharedFiles sf ");
            sqlCommand.Append("WHERE sf.FolderID = sff.FolderID) As SizeInKB ");

            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = @ModuleID ");
            sqlCommand.Append("AND sff.ParentID = @ParentID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@ParentID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return FBSqlHelper.ExecuteReader(
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
            FbParameter[] arParams = new FbParameter[13];

            arParams[0] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter(":UploadUserID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = uploadUserId;

            arParams[2] = new FbParameter(":FriendlyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new FbParameter(":OriginalFileName", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new FbParameter(":ServerFileName", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new FbParameter(":SizeInKB", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new FbParameter(":UploadDate", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new FbParameter(":FolderID", FbDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = folderId;

            arParams[8] = new FbParameter(":ItemGuid", FbDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new FbParameter(":ModuleGuid", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new FbParameter(":FolderGuid", FbDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = folderGuid.ToString();

            arParams[12] = new FbParameter(":Description", FbDbType.VarChar);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SHAREDFILES_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));

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
            sqlCommand.Append("ModuleID = @ModuleID, ");
            sqlCommand.Append("UploadUserID = @UploadUserID, ");
            sqlCommand.Append("FriendlyName = @FriendlyName, ");
            sqlCommand.Append("OriginalFileName = @OriginalFileName, ");
            sqlCommand.Append("ServerFileName = @ServerFileName, ");
            sqlCommand.Append("SizeInKB = @SizeInKB, ");
            sqlCommand.Append("UploadDate = @UploadDate, ");
            sqlCommand.Append("FolderID = @FolderID, ");
            sqlCommand.Append("UserGuid = @UserGuid, ");
            sqlCommand.Append("FolderGuid = @FolderGuid, ");
            sqlCommand.Append("Description = @Description ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[12];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new FbParameter("@UploadUserID", FbDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uploadUserId;

            arParams[3] = new FbParameter("@FriendlyName", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = friendlyName;

            arParams[4] = new FbParameter("@OriginalFileName", FbDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = originalFileName;

            arParams[5] = new FbParameter("@ServerFileName", FbDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = serverFileName;

            arParams[6] = new FbParameter("@SizeInKB", FbDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sizeInKB;

            arParams[7] = new FbParameter("@UploadDate", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadDate;

            arParams[8] = new FbParameter("@FolderID", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = folderId;

            arParams[9] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            arParams[10] = new FbParameter("@FolderGuid", FbDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = folderGuid.ToString();

            arParams[11] = new FbParameter("@Description", FbDbType.VarChar);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = description;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(int moduleId)
        {
          
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID = @ModuleID;");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID = @ModuleID;");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID = @ModuleID;");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

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
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetSharedFiles(int moduleId, int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

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
            sqlCommand.Append("sf.ModuleID = @ModuleID ");
            sqlCommand.Append("AND sf.FolderID = @FolderID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@FolderID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFiles(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");

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
            sqlCommand.Append("sf.ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");

            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");

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
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("m.ViewRoles, ");
            sqlCommand.Append("md.FeatureName ");

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
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@SiteID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new FbParameter("@PageID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            return FBSqlHelper.ExecuteReader(
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
            FbParameter[] arParams = new FbParameter[12];

            arParams[0] = new FbParameter(":ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new FbParameter(":ModuleID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new FbParameter(":FriendlyName", FbDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new FbParameter(":OriginalFileName", FbDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new FbParameter(":ServerFileName", FbDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new FbParameter(":SizeInKB", FbDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new FbParameter(":UploadDate", FbDbType.TimeStamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new FbParameter(":ArchiveDate", FbDbType.TimeStamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = archiveDate;

            arParams[8] = new FbParameter(":UploadUserID", FbDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadUserId;

            arParams[9] = new FbParameter(":ItemGuid", FbDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid.ToString();

            arParams[10] = new FbParameter(":ModuleGuid", FbDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new FbParameter(":UserGuid", FbDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid.ToString();

            int newID = Convert.ToInt32(FBSqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.StoredProcedure,
                "EXECUTE PROCEDURE MP_SHAREDFILESHISTORY_INSERT ("
                + FBSqlHelper.GetParamString(arParams.Length) + ")",
                arParams));


            return (newID > 0);

        }



        public static bool DeleteHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
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
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND ItemID = @ItemID ;");

            FbParameter[] arParams = new FbParameter[2];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new FbParameter("@ItemID", FbDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ModuleID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ID = @ID ;");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ID", FbDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }






    }
}
