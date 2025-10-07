using System;
using System.Data;
using Npgsql;

namespace mojoPortal.Data;

public static class DBSiteFolder
{
	public static int Add(Guid guid, Guid siteGuid, int siteId, string folderName)
	{
		NpgsqlParameter[] arParams =
		[
			new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},
			new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
			new NpgsqlParameter(":foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			},
			new NpgsqlParameter(":siteid", NpgsqlTypes.NpgsqlDbType.Integer)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},
		];

		var sqlCommand = """
                INSERT INTO mp_sitefolders (guid, siteguid, foldername, siteid)
                VALUES (:guid, :siteguid, :foldername, :siteid);
            """;

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			arParams);

		return rowsAffected;
	}

	public static bool Update(Guid guid, string folderName)
	{
		NpgsqlParameter[] arParams =
		[
			new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},
			new NpgsqlParameter(":foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = folderName
			},
		];

		var sqlCommand = """
                UPDATE mp_sitefolders 
                SET foldername = :foldername
                WHERE guid = :guid ;
                """;

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			arParams);

		return rowsAffected > -1;
	}

	public static bool Delete(Guid guid)
	{
		NpgsqlParameter[] arParams =
		[
			new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = guid.ToString()
			},
		];

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
			CommandType.StoredProcedure,
			"mp_sitefolders_delete(:guid)",
			arParams);

		return rowsAffected > -1;
	}

	public static IDataReader GetOne(Guid guid)
    {
        NpgsqlParameter[] arParams = new NpgsqlParameter[1];
        
        arParams[0] = new NpgsqlParameter(":guid", NpgsqlTypes.NpgsqlDbType.Varchar, 36);
        arParams[0].Direction = ParameterDirection.Input;
        arParams[0].Value = guid.ToString();

        return NpgsqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            CommandType.Text,
			"SELECT * FROM mp_sitefolders WHERE guid = :guid",
            arParams);

    }

    public static IDataReader GetBySite(Guid siteGuid)
    {
        NpgsqlParameter[] arParams =
		[
			new NpgsqlParameter(":siteguid", NpgsqlTypes.NpgsqlDbType.Varchar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},
		];

		return NpgsqlHelper.ExecuteReader(
            ConnectionString.GetReadConnectionString(),
            CommandType.Text,
            "SELECT * FROM mp_sitefolders WHERE siteguid = :siteguid",
            arParams);

    }

    public static bool Exists(string folderName)
    {
        NpgsqlParameter[] arParams = new NpgsqlParameter[1];
        
        arParams[0] = new NpgsqlParameter(":foldername", NpgsqlTypes.NpgsqlDbType.Text, 50);
        arParams[0].Direction = ParameterDirection.Input;
        arParams[0].Value = folderName;

        int count = Convert.ToInt32(NpgsqlHelper.ExecuteScalar(
            ConnectionString.GetReadConnectionString(),
            CommandType.StoredProcedure,
            "mp_sitefolders_exists(:foldername)",
            arParams));

        return (count > 0);

    }


    public static Guid GetSiteGuid(string folderName)
    {
        NpgsqlParameter[] arParams = new NpgsqlParameter[1];
        
        arParams[0] = new NpgsqlParameter(":foldername", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
        arParams[0].Direction = ParameterDirection.Input;
        arParams[0].Value = folderName;

        string strGuid = NpgsqlHelper.ExecuteScalar(
            ConnectionString.GetReadConnectionString(),
            CommandType.StoredProcedure,
            "mp_sitefolders_selectsiteguid(:foldername)",
            arParams).ToString();

        return new Guid(strGuid);

    }

}
