// Created:					2017-12-30
// Last Modified:			2018-01-02

using mojoPortal.Data;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SuperFlexiData
{
	public static class DBSearchDefs
    {
        public static bool Create(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
			sqlCommand.AppendFormat("insert into i7_sflexi_searchdefs ({0}) values ({1});",
				@"guid
                 ,siteguid
                 ,featureguid
                 ,fielddefinitionguid
                 ,title
                 ,keywords
                 ,description
                 ,link
                 ,linkqueryaddendum"
				, @":guid
				   ,:siteguid
				   ,:featureguid
				   ,:fielddefinitionguid
				   ,:title
				   ,:keywords
				   ,:description
				   ,:link
				   ,:linkqueryaddendum"
			);

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid },
				new NpgsqlParameter(":title", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = title },
				new NpgsqlParameter(":keywords", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new NpgsqlParameter(":description", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = description },
				new NpgsqlParameter(":link", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = link },
				new NpgsqlParameter(":linkqueryaddendum", NpgsqlDbType.Varchar, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

            int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
                sqlParams.ToArray()).ToString());

            return rowsAffected > 0;
        }

        public static bool Update(Guid guid, Guid siteGuid, Guid featureGuid, Guid definitionGuid, string title, string keywords, string description, string link, string linkQueryAddendum)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.AppendFormat("update i7_sflexi_searchdefs set {0} where guid = :guid;",
                "siteguid = :siteguid,"
                + "featureguid = :featureguid,"
                + "fielddefinitionguid = :fielddefinitionguid,"
                + "title = :title,"
                + "keywords = :keywords,"
                + "description = :description,"
                + "link = :link,"
                + "linkqueryaddendum = :linkqueryaddendum");

			var sqlParams = new List<NpgsqlParameter>
			{
				new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid },
				new NpgsqlParameter(":featureguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = featureGuid },
				new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = definitionGuid },
				new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid },
				new NpgsqlParameter(":title", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = title },
				new NpgsqlParameter(":keywords", NpgsqlDbType.Varchar, 50) { Direction = ParameterDirection.Input, Value = keywords },
				new NpgsqlParameter(":description", NpgsqlDbType.Varchar, 255) { Direction = ParameterDirection.Input, Value = description },
				new NpgsqlParameter(":link", NpgsqlDbType.Text) { Direction = ParameterDirection.Input, Value = link },
				new NpgsqlParameter(":linkqueryaddendum", NpgsqlDbType.Varchar, 16) { Direction = ParameterDirection.Input, Value = linkQueryAddendum }
			};

			int rowsAffected = Convert.ToInt32(NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParams.ToArray()).ToString());

            return rowsAffected > 0;
        }

        public static bool DeleteByFieldDefinition(Guid fieldDefGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where fielddefinitionguid = :fielddefinitionguid;");

			var sqlParam = new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldDefGuid };

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where siteguid = :siteguid;");

			var sqlParam = new NpgsqlParameter(":siteguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = siteGuid };

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("delete from i7_sflexi_searchdefs where guid = :guid;");

			var sqlParam = new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid };

			int rowsAffected = NpgsqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);

            return (rowsAffected > 0);
        }

        public static IDataReader GetByFieldDefinition(Guid fieldDefinitionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_searchdefs where fielddefinitionguid = :fielddefinitionguid;");

			var sqlParam = new NpgsqlParameter(":fielddefinitionguid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = fieldDefinitionGuid };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);
        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("select * from i7_sflexi_searchdefs where guid = :guid;");

			var sqlParam = new NpgsqlParameter(":guid", NpgsqlDbType.Uuid) { Direction = ParameterDirection.Input, Value = guid };

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
				sqlCommand.ToString(),
				sqlParam);
        }
    }
}
