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
    public static class DBQuestionOption
    {
        
        /// <summary>
        /// Inserts a row in the mp_SurveyQuestionOptions table. Returns rows affected count.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="order"> order </param>
        /// <returns>int</returns>
        public static int Add(
            Guid questionOptionGuid,
            Guid questionGuid,
            string answer,
            int order)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveyquestionoptions (");
            sqlCommand.Append("questionoptionguid, ");
            sqlCommand.Append("questionguid, ");
            sqlCommand.Append("answer, ");
            sqlCommand.Append("\"order\" )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":questionoptionguid, ");
            sqlCommand.Append(":questionguid, ");
            sqlCommand.Append(":answer, ");
            sqlCommand.Append(":sort ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("questionoptionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new NpgsqlParameter("sort", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_SurveyQuestionOptions table. Returns true if row updated.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="answer"> answer </param>
        /// <param name="order"> order </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid questionOptionGuid,
            Guid questionGuid,
            string answer,
            int order)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveyquestionoptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("questionguid = :questionguid, ");
            sqlCommand.Append("answer = :answer, ");
            sqlCommand.Append("\"order\" = :sort ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("questionoptionguid = :questionoptionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("questionoptionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Varchar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new NpgsqlParameter("sort", NpgsqlTypes.NpgsqlDbType.Integer);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;


            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyQuestionOptions table. Returns true if row deleted.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid questionOptionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionoptionguid = :questionoptionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("questionoptionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();
            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyQuestionOptions table.
        /// </summary>
        public static IDataReader GetAll(Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid = :questionguid ");
            sqlCommand.Append("ORDER BY \"order\" ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestionOptions table.
        /// </summary>
        /// <param name="questionOptionGuid"> questionOptionGuid </param>
        public static IDataReader GetOne(
            Guid questionOptionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyquestionoptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionoptionguid = :questionoptionguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[1];

            arParams[0] = new NpgsqlParameter("questionoptionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
