// Created:       2007-11-03
// Last Modified: 2018-07-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mojoPortal.Data
{
	public static class DBSharedFiles
	{
		/// <summary>
		/// Add a new folder to the module.
		/// </summary>
		/// <param name="folderGuid">folderGuid</param>
		/// <param name="moduleGuid">moduleGuid</param>
		/// <param name="parentGuid">parentGuid</param>
		/// <param name="moduleId">moduleId</param>
		/// <param name="folderName">folderName</param>
		/// <param name="parentId">parentId</param>
		/// <param name="viewRoles">viewRoles</param>
		/// <returns></returns>
		public static int AddSharedFileFolder(
			Guid folderGuid,
			Guid moduleGuid,
			Guid parentGuid,
			int moduleId,
			string folderName,
			int parentId,
			string viewRoles
		)
		{
			string sqlCommand = @"
				mp_sharedfilefolders_insert(
					:moduleid,
					:foldername,
					:parentid,
					:moduleguid,
					:folderguid,
					:parentguid,
					:viewroles
				)";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 255)
				{
					Direction = ParameterDirection.Input,
					Value = folderName
				},
				new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = parentId
				},
				new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},
				new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = folderGuid.ToString()
				},
				new NpgsqlParameter("parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
				{
					Direction = ParameterDirection.Input,
					Value = parentGuid.ToString()
				},
				new NpgsqlParameter("viewroles", NpgsqlTypes.NpgsqlDbType.Text)
				{
					Direction = ParameterDirection.Input,
					Value = viewRoles
				}
			};

			int newID = Convert.ToInt32(
				NpgsqlHelper.ExecuteScalar(
					ConnectionString.GetWriteConnectionString(),
					CommandType.StoredProcedure,
					sqlCommand,
					arParams.ToArray()
				)
			);

			return newID;
		}


		/// <summary>
		/// Update a folder
		/// </summary>
		/// <param name="folderId">folderId</param>
		/// <param name="moduleId">moduleId</param>
		/// <param name="folderName">folderName</param>
		/// <param name="parentId">parentId</param>
		/// <param name="parentGuid">parentGuid</param>
		/// <param name="viewRoles">viewRoles</param>
		/// <returns></returns>
		public static bool UpdateSharedFileFolder(
			int folderId,
			int moduleId,
			string folderName,
			int parentId,
			Guid parentGuid,
			string viewRoles
		)
		{
			string sqlCommand = @"
				mp_sharedfilefolders_update(
					:folderid,
					:moduleid,
					:foldername,
					:parentid,
					:parentguid,
					:viewroles
				)";

			NpgsqlParameter[] arParams = new NpgsqlParameter[6];

			arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			};

			arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[2] = new NpgsqlParameter("foldername", NpgsqlTypes.NpgsqlDbType.Text, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			};

			arParams[3] = new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			};

			arParams[4] = new NpgsqlParameter("parentguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			};

			arParams[5] = new NpgsqlParameter("viewroles", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			};

			int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				CommandType.StoredProcedure,
				sqlCommand,
				arParams));

			return (rowsAffected > -1);
		}


		public static bool DeleteSharedFileFolder(int folderId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			};

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

			arParams[0] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_sharedfilefolders_selectonev2(:folderid)",
				arParams);
		}


		public static IDataReader GetSharedModuleFolders(int moduleId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

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
		}


		public static IDataReader GetSharedFolders(int moduleId, int parentId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[2];

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[1] = new NpgsqlParameter("parentid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			};

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
			string description,
			string viewRoles
		)
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
			sqlCommand.Append("description, ");
			sqlCommand.Append("viewroles)");

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
			sqlCommand.Append(":description, ");
			sqlCommand.Append(":viewroles)");
			sqlCommand.Append(";");
			sqlCommand.Append(" SELECT CURRVAL('mp_sharedfiles_itemid_seq');");

			NpgsqlParameter[] arParams = new NpgsqlParameter[14];

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[1] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			};

			arParams[2] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			};

			arParams[3] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			};

			arParams[4] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			};

			arParams[5] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			};

			arParams[6] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			};

			arParams[7] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			};

			arParams[8] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			};

			arParams[9] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			};

			arParams[10] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			};

			arParams[11] = new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = folderGuid.ToString()
			};

			arParams[12] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			};

			arParams[13] = new NpgsqlParameter("viewroles", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			};

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
			string description,
			string viewRoles
		)
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
			sqlCommand.Append("description = :description, ");
			sqlCommand.Append("viewroles = :viewroles ");
			sqlCommand.Append("WHERE  ");
			sqlCommand.Append("itemid = :itemid ");
			sqlCommand.Append(";");

			NpgsqlParameter[] arParams = new NpgsqlParameter[13];

			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

			arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[2] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			};

			arParams[3] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			};

			arParams[4] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			};

			arParams[5] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			};

			arParams[6] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			};

			arParams[7] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			};

			arParams[8] = new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = folderId
			};

			arParams[9] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			};

			arParams[10] = new NpgsqlParameter("folderguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = folderGuid.ToString()
			};

			arParams[11] = new NpgsqlParameter("description", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = description
			};

			arParams[12] = new NpgsqlParameter("viewroles", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			};

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

			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

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

			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

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

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

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

			arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			};

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams);

			return (rowsAffected > -1);
		}


		/// <summary>
		/// Get a single shared file
		/// </summary>
		/// <param name="itemId">ID of Shared File</param>
		/// <returns></returns>
		public static IDataReader GetSharedFile(int itemId)
		{
			string sqlCommand = @"
				SELECT
					sf.itemid,
					sf.moduleid,
					sf.uploaduserid,
					sf.friendlyname,
					sf.originalfilename,
					sf.serverfilename,
					sf.sizeinkb,
					sf.uploaddate,
					sf.folderid,
					sf.itemguid,
					sf.folderguid,
					sf.userguid,
					sf.moduleguid,
					sf.downloadcount,
					sf.description,
					sf.viewroles 
				FROM
					mp_sharedfiles sf
				WHERE
					sf.itemid = :itemid;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = itemId
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		/// <summary>
		/// Get shared files for current folder
		/// </summary>
		/// <param name="moduleId">The Module ID</param>
		/// <param name="folderId">The current folder ID</param>
		/// <returns></returns>
		public static IDataReader GetSharedFiles(int moduleId, int folderId)
		{
			string sqlCommand = @"
				SELECT
					sf.itemid,
					sf.moduleid,
					sf.uploaduserid,
					sf.friendlyname,
					sf.originalfilename,
					sf.serverfilename,
					sf.sizeinkb,
					sf.uploaddate,
					sf.folderid,
					sf.itemguid,
					sf.folderguid,
					sf.userguid,
					sf.moduleguid,
					sf.description,
					sf.downloadcount,
					sf.viewroles,
					u.name As username
				FROM
					mp_sharedfiles sf
				LEFT OUTER JOIN
					mp_users u
				ON
					sf.uploaduserid = u.userid
				WHERE
					sf.moduleid = :moduleid
				AND
					sf.folderid = :folderid
				ORDER BY
					sf.friendlyname;";

			var arParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = moduleId
				},
				new NpgsqlParameter("folderid", NpgsqlTypes.NpgsqlDbType.Integer)
				{
					Direction = ParameterDirection.Input,
					Value = folderId
				}
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand,
				arParams.ToArray()
			);
		}


		public static IDataReader GetSharedFiles(int moduleId)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

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
			sqlCommand.Append("sf.viewroles ");

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

			arParams[0] = new NpgsqlParameter("siteid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			};

			arParams[1] = new NpgsqlParameter("pageid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			};

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
			sqlCommand.Append("ce.viewroles, ");
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
			DateTime archiveDate,
			string viewRoles
		)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[13];

			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

			arParams[1] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[2] = new NpgsqlParameter("friendlyname", NpgsqlTypes.NpgsqlDbType.Text, 255)
			{
				Direction = ParameterDirection.Input,
				Value = friendlyName
			};

			arParams[3] = new NpgsqlParameter("originalfilename", NpgsqlTypes.NpgsqlDbType.Text, 255)
			{
				Direction = ParameterDirection.Input,
				Value = originalFileName
			};

			arParams[4] = new NpgsqlParameter("serverfilename", NpgsqlTypes.NpgsqlDbType.Text, 50)
			{
				Direction = ParameterDirection.Input,
				Value = serverFileName
			};

			arParams[5] = new NpgsqlParameter("sizeinkb", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = sizeInKB
			};

			arParams[6] = new NpgsqlParameter("uploaddate", NpgsqlTypes.NpgsqlDbType.Timestamp)
			{
				Direction = ParameterDirection.Input,
				Value = uploadDate
			};

			arParams[7] = new NpgsqlParameter("uploaduserid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = uploadUserId
			};

			arParams[8] = new NpgsqlParameter("archivedate", NpgsqlTypes.NpgsqlDbType.Timestamp)
			{
				Direction = ParameterDirection.Input,
				Value = archiveDate
			};

			arParams[9] = new NpgsqlParameter("itemguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = itemGuid.ToString()
			};

			arParams[10] = new NpgsqlParameter("moduleguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = moduleGuid.ToString()
			};

			arParams[11] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			};

			arParams[12] = new NpgsqlParameter("viewroles", NpgsqlTypes.NpgsqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = viewRoles
			};

			int newID = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				CommandType.StoredProcedure,
				"mp_sharedfileshistory_insert(:itemid,:moduleid,:friendlyname,:originalfilename,:serverfilename,:sizeinkb,:uploaddate,:uploaduserid,:archivedate,:itemguid,:moduleguid,:userguid,:viewroles)",
				arParams));

			return (newID > 0);
		}


		public static bool DeleteHistory(int id)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = id
			};

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

			arParams[0] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

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

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			arParams[1] = new NpgsqlParameter("itemid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = itemId
			};

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

			arParams[0] = new NpgsqlParameter("moduleid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = moduleId
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.Text,
				sqlCommand.ToString(),
				arParams);
		}


		public static IDataReader GetHistoryFile(int id)
		{
			NpgsqlParameter[] arParams = new NpgsqlParameter[1];

			arParams[0] = new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = id
			};

			return NpgsqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"mp_sharedfileshistory_selectone(:id)",
				arParams);
		}
	}
}
