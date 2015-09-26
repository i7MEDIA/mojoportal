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
using System.Text;
using Npgsql;

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
            NpgsqlParameter[] arParams = new NpgsqlParameter[6];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderName;

            arParams[2] = new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = parentId;

            arParams[3] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = moduleGuid.ToString();

            arParams[4] = new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = folderGuid.ToString();

            arParams[5] = new NpgsqlParameter("parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = parentGuid.ToString();

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfilefolders_insert(:moduleid,:foldername,:parentid,:moduleguid,:folderguid,:parentguid)",
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[5];
            
            arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = folderName;

            arParams[3] = new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = parentId;

            arParams[4] = new NpgsqlParameter("parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = parentGuid.ToString();

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfilefolders_update(:folderid,:moduleid,:foldername,:parentid,:parentguid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteSharedFileFolder(int folderId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfilefolders_delete(:folderid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static IDataReader GetSharedFileFolder(int folderId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = folderId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfilefolders_selectonev2(:folderid)",
                arParams);

        }

        public static IDataReader GetSharedModuleFolders(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.sizeinkb),0) ");
            sqlCommand.Append("FROM mp_sharedfiles sf ");
            sqlCommand.Append("WHERE sf.folderid = sff.folderid) As sizeinkb ");

            sqlCommand.Append("FROM	mp_sharedfilefolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.moduleid = :moduleid ");
            sqlCommand.Append("ORDER BY sff.foldername ;");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_sharedfilefolders_selectallbymodulev2(:moduleid)",
            //    arParams);

        }

        public static IDataReader GetSharedFolders(int moduleId, int parentId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = parentId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT ");
            sqlCommand.Append("sff.*, ");
            sqlCommand.Append("(SELECT COALESCE(SUM(sf.sizeinkb),0) ");
            sqlCommand.Append("FROM mp_sharedfiles sf ");
            sqlCommand.Append("WHERE sf.folderid = sff.folderid) As sizeinkb ");

            sqlCommand.Append("FROM	mp_sharedfilefolders sff ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sff.moduleid = :moduleid ");
            sqlCommand.Append("AND sff.parentid = :parentid ");
            sqlCommand.Append("ORDER BY sff.foldername ;");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_sharedfilefolders_selectbymodulev2(:moduleid,:parentid)",
            //    arParams);

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
            sqlCommand.Append("INSERT INTO mp_sharedfiles (");
            sqlCommand.Append("moduleid, ");
            sqlCommand.Append("uploaduserid, ");
            sqlCommand.Append("friendlyname, ");
            sqlCommand.Append("originalfilename, ");
            sqlCommand.Append("serverfilename, ");
            sqlCommand.Append("sizeinkb, ");
            sqlCommand.Append("uploaddate, ");
            sqlCommand.Append("folderid, ");
            sqlCommand.Append("itemguid, ");
            sqlCommand.Append("moduleguid, ");
            sqlCommand.Append("userguid, ");
            sqlCommand.Append("folderguid, ");
            sqlCommand.Append("downloadcount, ");
            sqlCommand.Append("description )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":moduleid, ");
            sqlCommand.Append(":uploaduserid, ");
            sqlCommand.Append(":friendlyname, ");
            sqlCommand.Append(":originalfilename, ");
            sqlCommand.Append(":serverfilename, ");
            sqlCommand.Append(":sizeinkb, ");
            sqlCommand.Append(":uploaddate, ");
            sqlCommand.Append(":folderid, ");
            sqlCommand.Append(":itemguid, ");
            sqlCommand.Append(":moduleguid, ");
            sqlCommand.Append(":userguid, ");
            sqlCommand.Append(":folderguid, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append(":description )");
            sqlCommand.Append(";");
            sqlCommand.Append(" SELECT CURRVAL('mp_sharedfiles_itemid_seq');");

            NpgsqlParameter[] arParams = new NpgsqlParameter[13];
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = uploadUserId;

            arParams[2] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = folderId;

            arParams[8] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = itemGuid.ToString();

            arParams[9] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = moduleGuid.ToString();

            arParams[10] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = userGuid.ToString();

            arParams[11] = new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = folderGuid.ToString();

            arParams[12] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = description;

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
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
            sqlCommand.Append("UPDATE mp_sharedfiles ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("uploaduserid = :uploaduserid, ");
            sqlCommand.Append("friendlyname = :friendlyname, ");
            sqlCommand.Append("originalfilename = :originalfilename, ");
            sqlCommand.Append("serverfilename = :serverfilename, ");
            sqlCommand.Append("sizeinkb = :sizeinkb, ");
            sqlCommand.Append("uploaddate = :uploaddate, ");
            sqlCommand.Append("folderid = :folderid, ");
            sqlCommand.Append("userguid = :userguid, ");
            sqlCommand.Append("folderguid = :folderguid, ");
            sqlCommand.Append("description = :description ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[12];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = uploadUserId;

            arParams[3] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = friendlyName;

            arParams[4] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = originalFileName;

            arParams[5] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = serverFileName;

            arParams[6] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = sizeInKB;

            arParams[7] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadDate;

            arParams[8] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = folderId;

            arParams[9] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = userGuid.ToString();

            arParams[10] = new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = folderGuid.ToString();

            arParams[11] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = description;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);
			
        }

        public static bool IncrementDownloadCount(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_sharedfiles ");
            sqlCommand.Append("SET  ");

            sqlCommand.Append("downloadcount = downloadcount + 1 ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteSharedFile(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfiles_delete(:itemid)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sharedfileshistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_sharedfiles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_sharedfilefolders ");
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
            sqlCommand.Append("DELETE FROM mp_sharedfileshistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_sharedfiles ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid IN (SELECT moduleid FROM mp_modules WHERE siteid = :siteid) ");
            sqlCommand.Append(";");

            sqlCommand.Append("DELETE FROM mp_sharedfilefolders ");
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

        public static IDataReader GetSharedFile(int itemId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sf.itemid, ");
            sqlCommand.Append("sf.moduleid, ");
            sqlCommand.Append("sf.uploaduserid, ");
            sqlCommand.Append("sf.friendlyname, ");
            sqlCommand.Append("sf.originalfilename, ");
            sqlCommand.Append("sf.serverfilename, ");
            sqlCommand.Append("sf.sizeinkb, ");
            sqlCommand.Append("sf.uploaddate, ");
            sqlCommand.Append("sf.folderid, ");
            sqlCommand.Append("sf.itemguid, ");
            sqlCommand.Append("sf.folderguid, ");
            sqlCommand.Append("sf.userguid, ");
            sqlCommand.Append("sf.moduleguid, ");
            sqlCommand.Append("sf.downloadcount, ");
            sqlCommand.Append("sf.description ");

            sqlCommand.Append("FROM	mp_sharedfiles sf ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.itemid = :itemid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);
            
            //NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            //arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            //arParams[0].Direction = ParameterDirection.Input;
            //arParams[0].Value = itemId;

            //return NpgsqlHelper.ExecuteReader(
            //    GetConnectionString(),
            //    CommandType.StoredProcedure,
            //    "mp_sharedfiles_selectone(:itemid)",
            //    arParams);

        }

        public static IDataReader GetSharedFiles(int moduleId, int folderId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = folderId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sf.itemid, ");
            sqlCommand.Append("sf.moduleid, ");
            sqlCommand.Append("sf.uploaduserid, ");
            sqlCommand.Append("sf.friendlyname, ");
            sqlCommand.Append("sf.originalfilename, ");
            sqlCommand.Append("sf.serverfilename, ");
            sqlCommand.Append("sf.sizeinkb, ");
            sqlCommand.Append("sf.uploaddate, ");
            sqlCommand.Append("sf.folderid, ");
            sqlCommand.Append("sf.itemguid, ");
            sqlCommand.Append("sf.folderguid, ");
            sqlCommand.Append("sf.userguid, ");
            sqlCommand.Append("sf.moduleguid, ");
            sqlCommand.Append("sf.description, ");
            sqlCommand.Append("sf.downloadcount, ");
            sqlCommand.Append("u.name As username ");

            sqlCommand.Append("FROM	mp_sharedfiles sf ");

            sqlCommand.Append("LEFT OUTER JOIN	mp_users u ");
            sqlCommand.Append("ON sf.uploaduserid = u.userid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.moduleid = :moduleid ");
            sqlCommand.Append("AND sf.folderid = :folderid ");

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.friendlyname ");
            sqlCommand.Append(";");



            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

           
        }

        public static IDataReader GetSharedFiles(int moduleId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT   ");
            sqlCommand.Append("sf.itemid, ");
            sqlCommand.Append("sf.moduleid, ");
            sqlCommand.Append("sf.uploaduserid, ");
            sqlCommand.Append("sf.friendlyname, ");
            sqlCommand.Append("sf.originalfilename, ");
            sqlCommand.Append("sf.serverfilename, ");
            sqlCommand.Append("sf.sizeinkb, ");
            sqlCommand.Append("sf.uploaddate, ");
            sqlCommand.Append("sf.folderid, ");
            sqlCommand.Append("sf.itemguid, ");
            sqlCommand.Append("sf.folderguid, ");
            sqlCommand.Append("sf.userguid, ");
            sqlCommand.Append("sf.moduleguid, ");
            sqlCommand.Append("sf.description, ");
            sqlCommand.Append("sf.downloadcount ");
            
            sqlCommand.Append("FROM	mp_sharedfiles sf ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sf.moduleid = :moduleid ");
            

            sqlCommand.Append("ORDER BY ");
            sqlCommand.Append("sf.friendlyname ");
            sqlCommand.Append(";");

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetSharedFilesByPage(int siteId, int pageId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageId;

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  ");
            sqlCommand.Append("ce.itemid, ");
            sqlCommand.Append("ce.moduleid, ");
            sqlCommand.Append("ce.uploaduserid, ");
            sqlCommand.Append("ce.friendlyname, ");
            sqlCommand.Append("ce.originalfilename, ");
            sqlCommand.Append("ce.serverfilename, ");
            sqlCommand.Append("ce.sizeinkb, ");
            sqlCommand.Append("ce.uploaddate, ");
            sqlCommand.Append("ce.folderid, ");
            sqlCommand.Append("ce.itemguid, ");
            sqlCommand.Append("ce.folderguid, ");
            sqlCommand.Append("ce.userguid, ");
            sqlCommand.Append("ce.moduleguid, ");
            sqlCommand.Append("ce.description, ");
            sqlCommand.Append("ce.downloadcount, ");
            sqlCommand.Append("m.moduletitle, ");
            sqlCommand.Append("m.viewroles, ");
            sqlCommand.Append("md.featurename ");

            sqlCommand.Append("FROM	mp_sharedFiles ce ");

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
            sqlCommand.Append(" ; ");
            
            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            NpgsqlParameter[] arParams = new NpgsqlParameter[12];
            
            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;

            arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleId;

            arParams[2] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = friendlyName;

            arParams[3] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Text, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = originalFileName;

            arParams[4] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Text, 50);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = serverFileName;

            arParams[5] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = sizeInKB;

            arParams[6] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = uploadDate;

            arParams[7] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = uploadUserId;

            arParams[8] = new NpgsqlParameter("archivedate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = archiveDate;

            arParams[9] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = itemGuid.ToString();

            arParams[10] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = moduleGuid.ToString();

            arParams[11] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = userGuid.ToString();

            int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfileshistory_insert(:itemid,:moduleid,:friendlyname,:originalfilename,:serverfilename,:sizeinkb,:uploaddate,:uploaduserid,:archivedate,:itemguid,:moduleguid,:userguid)",
                arParams));

            return (newID > 0);

        }

        public static bool DeleteHistory(int id)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
                ConnectionString.GetWriteConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfileshistory_delete(:id)",
                arParams));

            return (rowsAffected > -1);

        }

        public static bool DeleteHistoryByItemID(int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = itemId;


            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_sharedfileshistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("itemid = :itemid ");
            sqlCommand.Append(";");

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static IDataReader GetHistory(int moduleId, int itemId)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[2];
            
            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            arParams[1] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = itemId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfileshistory_select(:moduleid,:itemid)",
                arParams);

        }

        public static IDataReader GetHistoryByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_sharedfileshistory ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("moduleid = :moduleid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetHistoryFile(int id)
        {
            NpgsqlParameter[] arParams = new NpgsqlParameter[1];
            
            arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = id;

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "mp_sharedfileshistory_selectone(:id)",
                arParams);

        }

    }
}
