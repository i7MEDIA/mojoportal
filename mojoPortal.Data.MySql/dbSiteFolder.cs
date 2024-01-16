using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBSiteFolder
{

	public static int Add(
		Guid guid,
		Guid siteGuid,
		string folderName)
	{

		string sqlCommand = @"
INSERT INTO mp_SiteFolders (
Guid, 
SiteGuid, 
FolderName )
 VALUES (
?Guid, 
?SiteGuid, 
?FolderName );";


		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			}
		};

		int rowsAffected = 0;
		rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	public static bool Update(
		Guid guid,
		Guid siteGuid,
		string folderName)
	{

		string sqlCommand = @"
UPDATE 
    mp_SiteFolders 
SET  
    SiteGuid = ?SiteGuid, 
    FolderName = ?FolderName 
WHERE  
    Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}


	public static bool Delete(Guid guid)
	{
		string sqlCommand = @"
DELETE FROM mp_SiteFolders 
WHERE Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > 0;

	}


	public static IDataReader GetOne(Guid guid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SiteFolders 
WHERE Guid = ?Guid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Guid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static IDataReader GetBySite(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SiteFolders 
WHERE SiteGuid = ?SiteGuid ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};


		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	public static Guid GetSiteGuid(string folderName)
	{


		var arParams = new List<MySqlParameter>
		{
			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			}
		};

		Guid siteGuid = Guid.Empty;

		string sqlCommand = @"
SELECT SiteGuid 
FROM mp_SiteFolders 
WHERE FolderName = ?FolderName ;";

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
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

			string sqlCommand1 = @"
SELECT SiteGuid     
FROM mp_Sites    
ORDER BY SiteID  
LIMIT 1 ;";

			using IDataReader reader = CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand1.ToString());

			if (reader.Read())
			{
				siteGuid = new Guid(reader["SiteGuid"].ToString());
			}

		}

		return siteGuid;

	}

	public static bool Exists(string folderName)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_SiteFolders 
WHERE FolderName = ?FolderName ;";

		var arParams = new List<MySqlParameter>
		{
			new("?FolderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			}
		};

		int count = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams));

		return count > 0;

	}





}
