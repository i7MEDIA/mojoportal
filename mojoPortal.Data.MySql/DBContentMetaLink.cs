using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data
{

	public static class DBContentMetaLink
	{

		/// <summary>
		/// Inserts a row in the mp_ContentMetaLink table. Returns rows affected count.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <param name="contentGuid"> contentGuid </param>
		/// <param name="rel"> rel </param>
		/// <param name="href"> href </param>
		/// <param name="hrefLang"> hrefLang </param>
		/// <param name="rev"> rev </param>
		/// <param name="type"> type </param>
		/// <param name="media"> media </param>
		/// <param name="sortRank"> sortRank </param>
		/// <param name="createdUtc"> createdUtc </param>
		/// <param name="createdBy"> createdBy </param>
		/// <param name="lastModUtc"> lastModUtc </param>
		/// <param name="lastModBy"> lastModBy </param>
		/// <returns>int</returns>
		public static int Create(
			Guid guid,
			Guid siteGuid,
			Guid moduleGuid,
			Guid contentGuid,
			string rel,
			string href,
			string hrefLang,
			string rev,
			string type,
			string media,
			int sortRank,
			DateTime createdUtc,
			Guid createdBy)
		{
			string sqlCommand = @"
INSERT INTO 
    mp_ContentMetaLink (
        Guid, 
        SiteGuid, 
        ModuleGuid, 
        ContentGuid, 
        Rel, 
        Href, 
        HrefLang, 
        Rev, 
        Type, 
        Media, 
        SortRank, 
        CreatedUtc, 
        CreatedBy, 
        LastModUtc, 
        LastModBy 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?ContentGuid, 
    ?Rel, 
    ?Href, 
    ?HrefLang, 
    ?Rev, 
    ?Type, 
    ?Media, 
    ?SortRank, 
    ?CreatedUtc, 
    ?CreatedBy, 
    ?LastModUtc, 
    ?LastModBy 
)";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36){
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},

				new("?SiteGuid", MySqlDbType.VarChar, 36){
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?ModuleGuid", MySqlDbType.VarChar, 36){
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},

				new("?ContentGuid", MySqlDbType.VarChar, 36){
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				},

				new("?Rel", MySqlDbType.VarChar, 255){
					Direction = ParameterDirection.Input,
					Value = rel
				},

				new("?Href", MySqlDbType.VarChar, 255){
					Direction = ParameterDirection.Input,
					Value = href
				},

				new("?HrefLang", MySqlDbType.VarChar, 10){
					Direction = ParameterDirection.Input,
					Value = hrefLang
				},

				new("?Rev", MySqlDbType.VarChar, 50){
					Direction = ParameterDirection.Input,
					Value = rev
				},

				new("?Type", MySqlDbType.VarChar, 50){
					Direction = ParameterDirection.Input,
					Value = type
				},

				new("?Media", MySqlDbType.VarChar, 50){
					Direction = ParameterDirection.Input,
					Value = media
				},

				new("?SortRank", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortRank
				},

				new("?CreatedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},

				new("?CreatedBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				},

				new("?LastModUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				},


				new("?LastModBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = createdBy.ToString()
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected;

		}

		/// <summary>
		/// Updates a row in the mp_ContentMetaLink table. Returns true if row updated.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="rel"> rel </param>
		/// <param name="href"> href </param>
		/// <param name="hrefLang"> hrefLang </param>
		/// <param name="rev"> rev </param>
		/// <param name="type"> type </param>
		/// <param name="media"> media </param>
		/// <param name="sortRank"> sortRank </param>
		/// <param name="lastModUtc"> lastModUtc </param>
		/// <param name="lastModBy"> lastModBy </param>
		/// <returns>bool</returns>
		public static bool Update(
			Guid guid,
			string rel,
			string href,
			string hrefLang,
			string rev,
			string type,
			string media,
			int sortRank,
			DateTime lastModUtc,
			Guid lastModBy)
		{
			string sqlCommand = @"
UPDATE 
    mp_ContentMetaLink 
SET  
    Rel = ?Rel, 
    Href = ?Href, 
    HrefLang = ?HrefLang, 
    Rev = ?Rev, 
    Type = ?Type, 
    Media = ?Media, 
    SortRank = ?SortRank, 
    LastModUtc = ?LastModUtc, 
    LastModBy = ?LastModBy 
WHERE  
    Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},

				new("?Rel", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = rel
				},

				new("?Href", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = href
				},

				new("?HrefLang", MySqlDbType.VarChar, 10)
				{
					Direction = ParameterDirection.Input,
					Value = hrefLang
				},

				new("?Rev", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = rev
				},

				new("?Type", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = type
				},

				new("?Media", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = media
				},

				new("?SortRank", MySqlDbType.Int32)
				{
					Direction = ParameterDirection.Input,
					Value = sortRank
				},

				new("?LastModUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = lastModUtc
				},

				new("?LastModBy", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = lastModBy.ToString()
				}

			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > -1;

		}

		/// <summary>
		/// Deletes a row from the mp_ContentMetaLink table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentMetaLink 
WHERE 
    Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
		/// </summary>
		/// <param name="siteGuid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentMetaLink 
WHERE 
    SiteGuid = ?SiteGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
		/// </summary>
		/// <param name="moduleGuid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentMetaLink 
WHERE 
    ModuleGuid = ?ModuleGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Deletes rows from the mp_ContentMetaLink table. Returns true if rows deleted.
		/// </summary>
		/// <param name="contentGuid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByContent(Guid contentGuid)
		{
			string sqlCommand = @"
DELETE FROM 
    mp_ContentMetaLink 
WHERE 
    ContentGuid = ?ContentGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?ContentGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};



			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}

		/// <summary>
		/// Gets an IDataReader with one row from the mp_ContentMetaLink table.
		/// </summary>
		/// <param name="guid"> guid </param>
		public static IDataReader GetOne(Guid guid)
		{
			string sqlCommand = @"
SELECT  
    * 
FROM 
    mp_ContentMetaLink 
WHERE 
    Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			};



			return CommandHelper.ExecuteReader(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);

		}

		/// <summary>
		/// Gets an IDataReader with rows from the mp_ContentMetaLink table.
		/// </summary>
		/// <param name="contentGuid"> guid </param>
		public static IDataReader GetByContent(Guid contentGuid)
		{
			string sqlCommand = @"
SELECT 
    *
FROM 
    mp_ContentMetaLink 
WHERE 
    ContentGuid = ?ContentGuid 
ORDER BY 
    SortRank;";

			var arParams = new List<MySqlParameter>
			{
				new("?ContentGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};



			return CommandHelper.ExecuteReader(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams);

		}

		public static int GetMaxSortRank(Guid contentGuid)
		{
			string sqlCommand = @"
SELECT  
    COALESCE(MAX(SortRank),1) 
FROM 
    mp_ContentMetaLink 
WHERE 
    ContentGuid = ?ContentGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?ContentGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = contentGuid.ToString()
				}
			};



			return Convert.ToInt32(CommandHelper.ExecuteScalar(
				ConnectionString.GetRead(),
				sqlCommand.ToString(),
				arParams));

		}

	}
}
