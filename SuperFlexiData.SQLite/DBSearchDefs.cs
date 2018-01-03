// Created:					2018-01-02
// Last Modified:   2018-01-03

using mojoPortal.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Mono.Data.Sqlite;

namespace SuperFlexiData
{
	public static class DBSearchDefs
    {
        public static bool Create(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.AppendFormat("insert into i7_sflexi_searchdefs ({0}) values ({1});",
				@"guid
                 ,SiteGuid
                 ,FeatureGuid
                 ,FieldDefinitionGuid
                 ,title
                 ,keywords
                 ,description
                 ,link
                 ,linkqueryaddendum"
				, @":guid
				   ,:SiteGuid
				   ,:FeatureGuid
				   ,:FieldDefinitionGuid
				   ,:Title
				   ,:Keywords
				   ,:Description
				   ,:link
				   ,:LinkQueryAddendum"
			);

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
				new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
				new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
				new SqliteParameter(":Title", DbType.String, 50) { Direction = ParameterDirection.Input, Value = title },
				new SqliteParameter(":Keywords", DbType.String, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new SqliteParameter(":Description", DbType.String, 255) { Direction = ParameterDirection.Input, Value = description },
				new SqliteParameter(":link", DbType.Object) { Direction = ParameterDirection.Input, Value = link },
				new SqliteParameter(":LinkQueryAddendum", DbType.String, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

            int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());

            return rowsAffected > 0;
        }

        public static bool Update(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("update i7_sflexi_searchdefs set {0} where Guid = :Guid;",
                "SiteGuid = :SiteGuid,"
                + "FeatureGuid = :FeatureGuid,"
                + "FieldDefinitionGuid = :FieldDefinitionGuid,"
                + "Title = :Title,"
                + "Keywords = :Keywords,"
                + "Description = :Description,"
                + "Link = :Link,"
                + "LinkQueryAddendum = :LinkQueryAddendum");

			var sqlParams = new List<SqliteParameter>
			{
				new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() },
				new SqliteParameter(":FeatureGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = featureGuid.ToString() },
				new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = definitionGuid.ToString() },
				new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() },
				new SqliteParameter(":Title", DbType.String, 50) { Direction = ParameterDirection.Input, Value = title },
				new SqliteParameter(":Keywords", DbType.String, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new SqliteParameter(":Description", DbType.String, 255) { Direction = ParameterDirection.Input, Value = description },
				new SqliteParameter(":Link", DbType.Object) { Direction = ParameterDirection.Input, Value = link },
				new SqliteParameter(":LinkQueryAddendum", DbType.String, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

			int rowsAffected = Convert.ToInt32(SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParams.ToArray()).ToString());

            return rowsAffected > 0;
        }

        public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where FieldDefinitionGuid = :FieldDefinitionGuid;");

			var sqlParam = new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldDefGuid.ToString() };

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where SiteGuid = :SiteGuid;");

			var sqlParam = new SqliteParameter(":SiteGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = siteGuid.ToString() };

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where Guid = :Guid;");

			var sqlParam = new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() };

			int rowsAffected = SqliteHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_searchdefs where FieldDefinitionGuid = :FieldDefinitionGuid;");

			var sqlParam = new SqliteParameter(":FieldDefinitionGuid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = fieldDefinitionGuid.ToString() };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_searchdefs where Guid = :Guid;");

			var sqlParam = new SqliteParameter(":Guid", DbType.String, 36) { Direction = ParameterDirection.Input, Value = guid.ToString() };

            return SqliteHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				sqlParam);
        }
    }
}
