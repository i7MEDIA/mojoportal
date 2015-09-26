/// Author:					Joe Audette
/// Created:				2007-11-03
/// Last Modified:			2013-02-13
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
    public static class DBSharedFiles
    {
        

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
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?FolderName, ");
            sqlCommand.Append("?ParentID, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?FolderGuid, ");
            sqlCommand.Append("?ParentGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderName;

            arParams[2] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = parentId;

            arParams[3] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new MySqlParameter("?FolderGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = folderGuid.ToString();

            arParams[5] = new MySqlParameter("?ParentGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = parentGuid.ToString();

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ModuleID = ?ModuleID, ");
            sqlCommand.Append("FolderName = ?FolderName, ");
            sqlCommand.Append("ParentID = ?ParentID, ");
            sqlCommand.Append("ParentGuid = ?ParentGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("FolderID = ?FolderID ;");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?FolderName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;

            arParams[3] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = parentId;

            arParams[4] = new MySqlParameter("?ParentGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = parentGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteSharedFileFolder(int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderID = ?FolderID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("FolderID = ?FolderID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("sff.ModuleID = ?ModuleID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("sff.ModuleID = ?ModuleID ");
            sqlCommand.Append("AND sff.ParentID = ?ParentID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?ParentID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?UploadUserID, ");
            sqlCommand.Append("?FriendlyName, ");
            sqlCommand.Append("?OriginalFileName, ");
            sqlCommand.Append("?ServerFileName, ");
            sqlCommand.Append("?SizeInKB, ");
            sqlCommand.Append("?UploadDate, ");
            sqlCommand.Append("?FolderID, ");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("?FolderGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[13];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?UploadUserID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = uploadUserId;

            arParams[2] = new MySqlParameter("?FriendlyName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new MySqlParameter("?OriginalFileName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new MySqlParameter("?ServerFileName", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;


            arParams[5] = new MySqlParameter("?SizeInKB", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new MySqlParameter("?UploadDate", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = folderId;

            arParams[8] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new MySqlParameter("?FolderGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = folderGuid.ToString();

            arParams[12] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ModuleID = ?ModuleID, ");
            sqlCommand.Append("UploadUserID = ?UploadUserID, ");
            sqlCommand.Append("FriendlyName = ?FriendlyName, ");
            sqlCommand.Append("OriginalFileName = ?OriginalFileName, ");
            sqlCommand.Append("ServerFileName = ?ServerFileName, ");
            sqlCommand.Append("SizeInKB = ?SizeInKB, ");
            sqlCommand.Append("UploadDate = ?UploadDate, ");
            sqlCommand.Append("FolderID = ?FolderID, ");
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("FolderGuid = ?FolderGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?UploadUserID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uploadUserId;

            arParams[3] = new MySqlParameter("?FriendlyName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = friendlyName;

            arParams[4] = new MySqlParameter("?OriginalFileName", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = originalFileName;

            arParams[5] = new MySqlParameter("?ServerFileName", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = serverFileName;

            arParams[6] = new MySqlParameter("?SizeInKB", MySqlDbType.Int32);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sizeInKB;

            arParams[7] = new MySqlParameter("?UploadDate", MySqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadDate;

            arParams[8] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = folderId;

            arParams[9] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            arParams[10] = new MySqlParameter("?FolderGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = folderGuid.ToString();

            arParams[11] = new MySqlParameter("?Description", MySqlDbType.LongText);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = description;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }


        public static bool DeleteSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles ");
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
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID = ?ModuleID;");
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID = ?ModuleID;");
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID = ?ModuleID;");

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
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);");
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);");
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = ?SiteID);");

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
            sqlCommand.Append("sf.ModuleID = ?ModuleID ");
            sqlCommand.Append("AND sf.FolderID = ?FolderID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?FolderID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("sf.ModuleID = ?ModuleID ");
            
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
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
            sqlCommand.Append("p.SiteID = ?SiteID ");
            sqlCommand.Append("AND pm.PageID = ?PageID ");
            sqlCommand.Append(" ; ");

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
            sqlCommand.Append("?ItemID, ");
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?FriendlyName, ");
            sqlCommand.Append("?OriginalFileName, ");
            sqlCommand.Append("?ServerFileName, ");
            sqlCommand.Append("?SizeInKB, ");
            sqlCommand.Append("?UploadDate, ");
            sqlCommand.Append("?UploadUserID, ");
            sqlCommand.Append("?ArchiveDate, ");
            sqlCommand.Append("?ItemGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid )");
            sqlCommand.Append(";");

            sqlCommand.Append("SELECT LAST_INSERT_ID();");

            MySqlParameter[] arParams = new MySqlParameter[12];

            arParams[0] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new MySqlParameter("?FriendlyName", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new MySqlParameter("?OriginalFileName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new MySqlParameter("?ServerFileName", MySqlDbType.VarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new MySqlParameter("?SizeInKB", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new MySqlParameter("?UploadDate", MySqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new MySqlParameter("?UploadUserID", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadUserId;

            arParams[8] = new MySqlParameter("?ArchiveDate", MySqlDbType.DateTime);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = archiveDate;

            arParams[9] = new MySqlParameter("?ItemGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid.ToString();

            arParams[10] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid.ToString();

            int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams).ToString());

            return (newID > 0);

        }



        public static bool DeleteHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = ?ID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteHistoryByItemID(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
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

        public static IDataReader GetHistory(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append("AND ItemID = ?ItemID ;");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new MySqlParameter("?ItemID", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetHistoryByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader GetHistoryFile(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = ?ID ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }






    }
}
