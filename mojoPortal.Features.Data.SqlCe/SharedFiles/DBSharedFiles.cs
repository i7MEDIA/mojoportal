// Author:					Joe Audette
// Created:				    2010-07-02
// Last Modified:			2013-02-18
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 
// Note moved into separate class file from dbPortal 2007-11-03

using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
//using log4net;

namespace mojoPortal.Data
{
    public static class DBSharedFiles
    {
        //private static readonly ILog log = LogManager.GetLogger(typeof(DBSharedFiles));

        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
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
            sqlCommand.Append("INSERT INTO mp_SharedFileFolders ");
            sqlCommand.Append("(");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("FolderName, ");
            sqlCommand.Append("ParentID, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("FolderGuid, ");
            sqlCommand.Append("ParentGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@FolderName, ");
            sqlCommand.Append("@ParentID, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@FolderGuid, ");
            sqlCommand.Append("@ParentGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[6];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@FolderName", SqlDbType.NVarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderName;

            arParams[2] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = parentId;

            arParams[3] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid;

            arParams[4] = new SqlCeParameter("@FolderGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = folderGuid;

            arParams[5] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = parentGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            //sqlCommand.Append("ModuleID = @ModuleID, ");
            sqlCommand.Append("FolderName = @FolderName, ");
            sqlCommand.Append("ParentID = @ParentID, ");
            sqlCommand.Append("ParentGuid = @ParentGuid ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("FolderID = @FolderID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@FolderName", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;

            arParams[3] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = parentId;

            arParams[4] = new SqlCeParameter("@ParentGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = parentGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSharedFileFolder(int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderID = @FolderID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID = @ModuleID;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID = @ModuleID;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID = @ModuleID;");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            

            return (rowsAffected > -1);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFileFolders WHERE ModuleID IN (SELECT ModuleID FROM mp_Modules WHERE SiteID = @SiteID);");

            arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetSharedFileFolder(int folderId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFileFolders ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("FolderID = @FolderID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSharedModuleFolders(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append(" COALESCE(s2.SizeInKB,0) As SizeInKB ");

            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT FolderID, COALESCE(SUM(SizeInKB),0) As SizeInKB ");
            sqlCommand.Append("FROM mp_SharedFiles ");
            sqlCommand.Append("GROUP BY FolderID ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.FolderID = sff.FolderID ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = @ModuleID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetSharedFolders(int moduleId, int parentId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append(" COALESCE(s2.SizeInKB,0) As SizeInKB ");
            
            sqlCommand.Append("FROM	mp_SharedFileFolders sff ");

            sqlCommand.Append("LEFT OUTER JOIN ( ");
            sqlCommand.Append("SELECT FolderID, COALESCE(SUM(SizeInKB),0) As SizeInKB ");
            sqlCommand.Append("FROM mp_SharedFiles ");
            sqlCommand.Append("GROUP BY FolderID ");
            sqlCommand.Append(") s2  ");
            sqlCommand.Append("ON s2.FolderID = sff.FolderID ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.ModuleID = @ModuleID ");
            sqlCommand.Append("AND sff.ParentID = @ParentID ");
            sqlCommand.Append("ORDER BY sff.FolderName ;");

            //log.Info(sqlCommand.ToString());

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@ParentID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("INSERT INTO mp_SharedFiles ");
            sqlCommand.Append("(");
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
     
            sqlCommand.Append("FolderGuid, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("DownloadCount ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@UploadUserID, ");
            sqlCommand.Append("@FriendlyName, ");
            sqlCommand.Append("@OriginalFileName, ");
            sqlCommand.Append("@ServerFileName, ");
            sqlCommand.Append("@SizeInKB, ");
            sqlCommand.Append("@UploadDate, ");
            sqlCommand.Append("@FolderID, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid, ");

            sqlCommand.Append("@FolderGuid, ");
            sqlCommand.Append("@Description, ");
            sqlCommand.Append("@DownloadCount ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[14];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@UploadUserID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = uploadUserId;

            arParams[2] = new SqlCeParameter("@FriendlyName", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new SqlCeParameter("@OriginalFileName", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new SqlCeParameter("@ServerFileName", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new SqlCeParameter("@SizeInKB", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new SqlCeParameter("@UploadDate", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = folderId;

            arParams[8] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid;

            arParams[9] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid;

            arParams[10] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid;

            arParams[11] = new SqlCeParameter("@FolderGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = folderGuid;

            arParams[12] = new SqlCeParameter("@Description", SqlDbType.NText);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            arParams[13] = new SqlCeParameter("@DownloadCount", SqlDbType.Int);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = 0;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return newId;

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
            //sqlCommand.Append("ModuleID = @ModuleID, ");
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
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@UploadUserID", SqlDbType.Int);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uploadUserId;

            arParams[3] = new SqlCeParameter("@FriendlyName", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = friendlyName;

            arParams[4] = new SqlCeParameter("@OriginalFileName", SqlDbType.NVarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = originalFileName;

            arParams[5] = new SqlCeParameter("@ServerFileName", SqlDbType.NVarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = serverFileName;

            arParams[6] = new SqlCeParameter("@SizeInKB", SqlDbType.Int);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sizeInKB;

            arParams[7] = new SqlCeParameter("@UploadDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadDate;

            arParams[8] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = folderId;

            arParams[9] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid;

            arParams[10] = new SqlCeParameter("@FolderGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = folderGuid;

            arParams[11] = new SqlCeParameter("@Description", SqlDbType.NVarChar);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = description;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFiles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

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
            sqlCommand.Append("ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("sf.ModuleID = @ModuleID ");
            sqlCommand.Append("AND sf.FolderID = @FolderID ");
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new SqlCeParameter("@FolderID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("sf.ModuleID = @ModuleID ");
            
            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.FriendlyName ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("p.SiteID = @SiteID ");
            sqlCommand.Append("AND pm.PageID = @PageID ");
            sqlCommand.Append(" ; ");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@SiteID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new SqlCeParameter("@PageID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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
            sqlCommand.Append("INSERT INTO mp_SharedFilesHistory ");
            sqlCommand.Append("(");
            sqlCommand.Append("ItemID, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("FriendlyName, ");
            sqlCommand.Append("OriginalFileName, ");
            sqlCommand.Append("ServerFileName, ");
            sqlCommand.Append("SizeInKB, ");
            sqlCommand.Append("UploadDate, ");
            sqlCommand.Append("ArchiveDate, ");
            sqlCommand.Append("UploadUserID, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ItemID, ");
            sqlCommand.Append("@ModuleID, ");
            sqlCommand.Append("@FriendlyName, ");
            sqlCommand.Append("@OriginalFileName, ");
            sqlCommand.Append("@ServerFileName, ");
            sqlCommand.Append("@SizeInKB, ");
            sqlCommand.Append("@UploadDate, ");
            sqlCommand.Append("@ArchiveDate, ");
            sqlCommand.Append("@UploadUserID, ");
            sqlCommand.Append("@ItemGuid, ");
            sqlCommand.Append("@ModuleGuid, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[12];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new SqlCeParameter("@FriendlyName", SqlDbType.NVarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new SqlCeParameter("@OriginalFileName", SqlDbType.NVarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new SqlCeParameter("@ServerFileName", SqlDbType.NVarChar, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new SqlCeParameter("@SizeInKB", SqlDbType.Int);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new SqlCeParameter("@UploadDate", SqlDbType.DateTime);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new SqlCeParameter("@ArchiveDate", SqlDbType.DateTime);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = archiveDate;

            arParams[8] = new SqlCeParameter("@UploadUserID", SqlDbType.Int);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = uploadUserId;

            arParams[9] = new SqlCeParameter("@ItemGuid", SqlDbType.UniqueIdentifier);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid;

            arParams[10] = new SqlCeParameter("@ModuleGuid", SqlDbType.UniqueIdentifier);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid;

            arParams[11] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid;


            int newId = Convert.ToInt32(SqlHelper.DoInsertGetIdentitiy(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            return (newId > -1);

        }

        public static bool DeleteHistory(int id)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ID = @ID ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteHistoryByItemID(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
        }

        public static IDataReader GetHistory(int moduleId, int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SharedFilesHistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = @ModuleID ");
            sqlCommand.Append("AND ItemID = @ItemID ;");

            SqlCeParameter[] arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ItemID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ModuleID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
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

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ID", SqlDbType.Int);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
