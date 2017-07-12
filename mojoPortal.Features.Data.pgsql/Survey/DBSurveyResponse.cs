/// Author:				
/// Created:			2008-08-29
/// Last Modified:		2012-08-13
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Text;
using mojoPortal.Data;
using Npgsql;

namespace SurveyFeature.Data
{
    public static class DBSurveyResponse
    {
       
        /// <summary>
        /// Inserts a row in the mp_SurveyResponses table. Returns rows affected count.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="userId"> userId </param>
        /// <param name="annonymous"> annonymous </param>
        /// <param name="complete"> complete </param>
        /// <returns>int</returns>
        public static int Add(
            Guid responseGuid,
            Guid surveyGuid,
            Guid userGuid,
            bool annonymous,
            bool complete)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveyresponses (");
            sqlCommand.Append("responseguid, ");
            sqlCommand.Append("surveyguid, ");
            //sqlCommand.Append("submissiondate, ");
            sqlCommand.Append("annonymous, ");
            sqlCommand.Append("complete, ");
            sqlCommand.Append("userguid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":responseguid, ");
            sqlCommand.Append(":surveyguid, ");
            //sqlCommand.Append(":submissiondate, ");
            sqlCommand.Append(":annonymous, ");
            sqlCommand.Append(":complete, ");
            sqlCommand.Append(":userguid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            //arParams[2] = new NpgsqlParameter("submissiondate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            //arParams[2].Direction = ParameterDirection.Input;
            //arParams[2].Value = submissionDate;

            arParams[2] = new NpgsqlParameter("annonymous", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = annonymous;

            arParams[3] = new NpgsqlParameter("complete", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = complete;

            arParams[4] = new NpgsqlParameter("userguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;


        }

        /// <summary>
        /// Updates the status of a response. Returns true if row updated.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="complete"> complete </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid responseGuid,
            DateTime submissionDate,
            bool complete)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveyresponses ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("submissiondate = :submissiondate, ");
            sqlCommand.Append("complete = :complete ");
       
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("responseguid = :responseguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[3];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new NpgsqlParameter("submissiondate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = submissionDate;

            arParams[2] = new NpgsqlParameter("complete", NpgsqlTypes.NpgsqlDbType.Boolean);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = complete;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        /// <summary>
        /// Deletes a row from the mp_SurveyResponses table. Returns true if row deleted.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyResponses table.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        public static IDataReader GetOne(
            Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyResponses table.
        /// </summary>
        public static IDataReader GetAll(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("surveyguid = :surveyguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with the first response to a survey
        /// </summary>
        public static IDataReader GetFirst(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE surveyguid = :surveyguid ");
            sqlCommand.Append("AND complete = true ");
            sqlCommand.Append("ORDER BY submissiondate, responseguid ");
            sqlCommand.Append("LIMIT 1; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("surveyguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);


        }

        /// <summary>
        /// Gets an IDataReader with the next response to a survey
        /// </summary>
        public static IDataReader GetNext(Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE submissiondate > (");
            sqlCommand.Append("SELECT submissiondate ");
            sqlCommand.Append("FROM mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("surveyguid = (");
            sqlCommand.Append("SELECT surveyguid ");
            sqlCommand.Append("FROM mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid) ");
            sqlCommand.Append("AND complete = true ");
            sqlCommand.Append("Order By submissiondate, responseguid ");
            sqlCommand.Append("Limit 1; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with the next response to a survey
        /// </summary>
        public static IDataReader GetPrevious(Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_surveyresponses ");
            sqlCommand.Append("WHERE complete = true AND submissiondate < (");
            sqlCommand.Append("SELECT submissiondate ");
            sqlCommand.Append("FROM mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("surveyguid = (");
            sqlCommand.Append("SELECT surveyguid ");
            sqlCommand.Append("FROM mp_surveyresponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("responseguid = :responseguid) ");
            sqlCommand.Append("Order By submissiondate DESC, responseguid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
