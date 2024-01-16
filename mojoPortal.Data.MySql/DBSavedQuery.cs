using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;

namespace mojoPortal.Data;


public static class DBSavedQuery
{
	/// <summary>
	/// Inserts a row in the mp_SavedQuery table. Returns rows affected count.
	/// </summary>
	/// <param name="id"> id </param>
	/// <param name="name"> name </param>
	/// <param name="statement"> statement </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="createdBy"> createdBy </param>
	/// <returns>int</returns>
	public static int Create(
		Guid id,
		string name,
		string statement,
		DateTime createdUtc,
		Guid createdBy)
	{

		string sqlCommand = @"
INSERT INTO 
    mp_SavedQuery (
        Id, 
        Name, 
        Statement, 
        CreatedUtc, 
        CreatedBy, 
        LastModUtc, 
        LastModBy 
)
VALUES (
    ?Id, 
    ?Name, 
    ?Statement, 
    ?CreatedUtc, 
    ?CreatedBy, 
    ?LastModUtc, 
    ?LastModBy 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?Id", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = id.ToString()
			},

			new("?Name", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = name
			},

			new("?Statement", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = statement
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
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_SavedQuery table. Returns true if row updated.
	/// </summary>
	/// <param name="id"> id </param>
	/// <param name="statement"> statement </param>
	/// <param name="lastModUtc"> lastModUtc </param>
	/// <param name="lastModBy"> lastModBy </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid id,
		string statement,
		DateTime lastModUtc,
		Guid lastModBy)
	{

		string sqlCommand = @"
UPDATE mp_SavedQuery 
SET 
Statement = ?Statement, 
LastModUtc = ?LastModUtc, 
LastModBy = ?LastModBy 
WHERE 
Id = ?Id;";

		var arParams = new List<MySqlParameter>
		{
			new("?Id", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = id.ToString()
			},

			new("?Statement", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = statement
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
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_SavedQuery table. Returns true if row deleted.
	/// </summary>
	/// <param name="id"> id </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid id)
	{
		string sqlCommand = @"
DELETE FROM mp_SavedQuery 
WHERE Id = ?Id ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Id", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = id.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SavedQuery table.
	/// </summary>
	/// <param name="id"> id </param>
	public static IDataReader GetOne(Guid id)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SavedQuery 
WHERE Id = ?Id ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Id", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = id.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_SavedQuery table.
	/// </summary>
	/// <param name="name"> name </param>
	public static IDataReader GetOne(string name)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SavedQuery 
WHERE Name = ?Name ;";

		var arParams = new List<MySqlParameter>
		{
			new("?Name", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = name
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_SavedQuery table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_SavedQuery ;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString());
	}


}
