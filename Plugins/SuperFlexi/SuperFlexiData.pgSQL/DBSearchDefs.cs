using System;
using System.Data;
using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;

namespace SuperFlexiData;

public static class DBSearchDefs
{
	public static bool Create(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
	{
		var sqlCommand = """
			insert into i7_sflexi_searchdefs (
				guid
				,siteguid
				,featureguid
				,fielddefinitionguid
				,title
				,keywords
				,description
				,link
				,linkqueryaddendum
			) values (
				:guid
				,:siteguid
				,:featureguid
				,:fielddefinitionguid
				,:title
				,:keywords
				,:description
				,:link
				,:linkqueryaddendum
			);
			""";

		var sqlParams = new NpgsqlParameter[]
			{
				new(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid },
				new(":title", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = title },
				new(":keywords", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new(":description", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = description },
				new(":link", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = link },
				new(":linkqueryaddendum", NpgsqlDbType.Varchar, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

		int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParams).ToString());

		return rowsAffected > 0;
	}

	public static bool Update(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
	{
		var sqlCommand = """
			update i7_sflexi_searchdefs
			set
				siteguid = :siteguid
				,featureguid = :featureguid
				,fielddefinitionguid = :fielddefinitionguid
				,title = :title
				,keywords = :keywords
				,description = :description
				,link = :link
				,linkqueryaddendum = :linkqueryaddendum
			where guid = :guid;
			""";

		var sqlParams = new NpgsqlParameter[]
			{
				new(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid },
				new(":title", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = title },
				new(":keywords", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new(":description", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = description },
				new(":link", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = link },
				new(":linkqueryaddendum", NpgsqlDbType.Varchar, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

		int rowsAffected = Convert.ToInt32(
			NpgsqlHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				CommandType.Text,
				sqlCommand,
				sqlParams).ToString()
				);

		return rowsAffected > 0;
	}

	public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where fielddefinitionguid = :fielddefinitionguid;";
		var sqlParam = new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldDefGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where siteguid = :siteguid;";
		var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
	}

	public static bool Delete(Guid guid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where guid = :guid;";
		var sqlParam = new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid };

		int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
	}

	public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
	{
		var sqlCommand = "select * from i7_sflexi_searchdefs where fielddefinitionguid = :fielddefinitionguid;";
		var sqlParam = new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldDefinitionGuid };

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			sqlParam);
	}

	public static IDataReader GetOne(Guid guid)
	{
		var sqlCommand = "select * from i7_sflexi_searchdefs where guid = :guid;";
		var sqlParam = new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid };

		return NpgsqlHelper.ExecuteReader(
			ConnectionString.GetWriteConnectionString(),
			CommandType.Text,
			sqlCommand.ToString(),
			sqlParam);
	}
}
