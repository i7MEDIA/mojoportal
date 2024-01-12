using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;

public static class DBIndexingQueue
{
	/// <summary>
	/// Inserts a row in the mp_IndexingQueue table. Returns new integer id.
	/// </summary>
	/// <param name="indexPath"> indexPath </param>
	/// <param name="serializedItem"> serializedItem </param>
	/// <param name="itemKey"> itemKey </param>
	/// <param name="removeOnly"> removeOnly </param>
	/// <returns>int</returns>
	public static Int64 Create(
		int siteId,
		string indexPath,
		string serializedItem,
		string itemKey,
		bool removeOnly)
	{
		#region Bit Conversion
		int intRemoveOnly;
		if (removeOnly)
		{
			intRemoveOnly = 1;
		}
		else
		{
			intRemoveOnly = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_IndexingQueue (
    SiteID, 
    IndexPath, 
    SerializedItem, 
    ItemKey, 
    RemoveOnly 
    )
VALUES (
    ?SiteID, 
    ?IndexPath, 
    ?SerializedItem, 
    ?ItemKey, 
    ?RemoveOnly 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?IndexPath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = indexPath
			},

			new("?SerializedItem", MySqlDbType.LongText)
			{
				Direction = ParameterDirection.Input,
				Value = serializedItem
			},

			new("?ItemKey", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = itemKey
			},

			new("?RemoveOnly", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intRemoveOnly
			},

			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		Int64 newID = -1;

		try
		{
			newID = Convert.ToInt64(CommandHelper.ExecuteScalar(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams).ToString());
		}
		catch (MySqlException) { }

		return newID;

	}

	/// <summary>
	/// Deletes a row from the mp_IndexingQueue table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowId"> rowId </param>
	/// <returns>bool</returns>
	public static bool Delete(Int64 rowId)
	{
		string sqlCommand = @"
DELETE FROM mp_IndexingQueue 
WHERE RowId = ?RowId;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowId", MySqlDbType.Int64)
			{
				Direction = ParameterDirection.Input,
				Value = rowId
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Deletes all rows from the mp_IndexingQueue table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowId"> rowId </param>
	/// <returns>bool</returns>
	public static bool DeleteAll()
	{
		string sqlCommand = @"
DELETE FROM mp_IndexingQueue;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString());
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets a count of rows in the mp_IndexingQueue table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_IndexingQueue;";

		return Convert.ToInt32(
			CommandHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString()
			)
		);
	}


	/// <summary>
	/// Gets an DataTable with rows from the mp_IndexingQueue table with the passed path.
	/// </summary>
	public static DataTable GetByPath(string indexPath)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_IndexingQueue 
WHERE IndexPath = ?IndexPath 
ORDER BY RowId;";

		var arParams = new List<MySqlParameter>
		{
			new("?IndexPath", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = indexPath
			}
		};

		DataTable dt = new DataTable();
		dt.Columns.Add("RowId", typeof(int));
		dt.Columns.Add("IndexPath", typeof(String));
		dt.Columns.Add("SerializedItem", typeof(String));
		dt.Columns.Add("ItemKey", typeof(String));
		dt.Columns.Add("RemoveOnly", typeof(bool));

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{

			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["RowId"] = reader["RowId"];
				row["IndexPath"] = reader["IndexPath"];
				row["SerializedItem"] = reader["SerializedItem"];
				row["ItemKey"] = reader["ItemKey"];
				row["RemoveOnly"] = Convert.ToBoolean(reader["RemoveOnly"]);

				dt.Rows.Add(row);

			}
		}

		return dt;

	}

	public static DataTable GetBySite(int siteId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_IndexingQueue 
WHERE SiteID = ?SiteID 
ORDER BY RowId;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			}
		};

		DataTable dt = new DataTable();
		dt.Columns.Add("RowId", typeof(int));
		dt.Columns.Add("IndexPath", typeof(String));
		dt.Columns.Add("SerializedItem", typeof(String));
		dt.Columns.Add("ItemKey", typeof(String));
		dt.Columns.Add("RemoveOnly", typeof(bool));

		using (IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams))
		{

			while (reader.Read())
			{
				DataRow row = dt.NewRow();
				row["RowId"] = reader["RowId"];
				row["IndexPath"] = reader["IndexPath"];
				row["SerializedItem"] = reader["SerializedItem"];
				row["ItemKey"] = reader["ItemKey"];
				row["RemoveOnly"] = Convert.ToBoolean(reader["RemoveOnly"]);

				dt.Rows.Add(row);

			}
		}

		return dt;

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_IndexingQueue table.
	/// </summary>
	public static DataTable GetIndexPaths()
	{
		string sqlCommand = @"
SELECT DISTINCT IndexPath 
FROM mp_IndexingQueue 
ORDER BY IndexPath;";

		IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());

		return DBPortal.GetTableFromDataReader(reader);
	}

	public static DataTable GetSiteIDs()
	{
		string sqlCommand = @"
SELECT DISTINCT SiteID 
FROM mp_IndexingQueue 
ORDER BY SiteID;";

		IDataReader reader = CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());

		return DBPortal.GetTableFromDataReader(reader);

	}




}
