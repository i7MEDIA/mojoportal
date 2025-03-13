using System;
using System.Data;
using mojoPortal.Data;
using Mono.Data.Sqlite;

namespace SuperFlexiData;

public static class DBSearchDefs
{
	public static bool Create(
		Guid guid,
		Guid siteGuid,
		Guid featureGuid,
		Guid definitionGuid,
		string title,
		string keywords,
		string description,
		string link,
		string linkQueryAddendum)
	{
		var sqlCommand = """
			insert into 
				i7_sflexi_searchdefs (
					guid
					,SiteGuid
					,FeatureGuid
					,FieldDefinitionGuid
					,title
					,keywords
					,description
					,link
					,linkqueryaddendum
				) values (
					:guid
					,:SiteGuid
					,:FeatureGuid
					,:FieldDefinitionGuid
					,:Title
					,:Keywords
					,:Description
					,:link
					,:LinkQueryAddendum
				);
			""";

		var sqlParams = new SqliteParameter[]
			{
				new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
				new(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
				new(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
				new(":Title", DbType.String, 50) { Direction = ParameterDirection.Input, Value = title },
				new(":Keywords", DbType.String, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new(":Description", DbType.String, 255) { Direction = ParameterDirection.Input, Value = description },
				new(":link", DbType.Object) { Direction = ParameterDirection.Input, Value = link },
				new(":LinkQueryAddendum", DbType.String, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

		int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams).ToString());

		return rowsAffected > 0;
	}

	public static bool Update(
		Guid guid,
		Guid siteGuid,
		Guid featureGuid,
		Guid definitionGuid,
		string title,
		string keywords,
		string description,
		string link,
		string linkQueryAddendum)
	{
		var sqlCommand = """
			update i7_sflexi_searchdefs 
			set 
				SiteGuid = :SiteGuid
				,FeatureGuid = :FeatureGuid
				,FieldDefinitionGuid = :FieldDefinitionGuid
				,Title = :Title
				,Keywords = :Keywords
				,Description = :Description
				,Link = :Link
				,LinkQueryAddendum = :LinkQueryAddendum
			where Guid = :Guid;
			""";

		var sqlParams = new SqliteParameter[]
		{
			new(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
			new(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
			new(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
			new(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
			new(":Title", DbType.String, 50) { Direction = ParameterDirection.Input, Value = title },
			new(":Keywords", DbType.String, 50) { Direction = ParameterDirection.Input, Value = keywords },
			new(":Description", DbType.String, 255) { Direction = ParameterDirection.Input, Value = description },
			new(":Link", DbType.Object) { Direction = ParameterDirection.Input, Value = link },
			new(":LinkQueryAddendum", DbType.String, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
		};

		int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParams).ToString());

		return rowsAffected > 0;
	}

	public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where FieldDefinitionGuid = :FieldDefinitionGuid;";
		var sqlParam = new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldDefGuid.ToString() };

		int rowsAffected = SqliteHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			sqlParam);

		return rowsAffected > 0;
	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where SiteGuid = :SiteGuid;";
		var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };
		int rowsAffected = SqliteHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand, sqlParam);
		return rowsAffected > 0;
	}

	public static bool Delete(Guid guid)
	{
		var sqlCommand = "delete from i7_sflexi_searchdefs where Guid = :Guid;";
		var sqlParam = new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() };
		int rowsAffected = SqliteHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(), sqlCommand, sqlParam);
		return rowsAffected > 0;
	}

	public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
	{
		var sqlCommand = "select * from i7_sflexi_searchdefs where FieldDefinitionGuid = :FieldDefinitionGuid;";
		var sqlParam = new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldDefinitionGuid.ToString() };
		return SqliteHelper.ExecuteReader(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), sqlParam);
	}

	public static IDataReader GetOne(Guid guid)
	{
		var sqlCommand = "select * from i7_sflexi_searchdefs where Guid = :Guid;";
		var sqlParam = new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() };
		return SqliteHelper.ExecuteReader(ConnectionString.GetWriteConnectionString(), sqlCommand.ToString(), sqlParam);
	}
}
