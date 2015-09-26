/// Author:				Joe Audette
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
    
    public static class DBQuestionAnswer
    {
        /// <summary>
        /// Inserts a row in the mp_SurveyQuestionAnswers table. Returns rows affected count.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="answer"> answer </param>
        /// <returns>int</returns>
        public static int Add(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_surveyquestionanswers (");
            sqlCommand.Append("answerguid, ");
            sqlCommand.Append("questionguid, ");
            sqlCommand.Append("responseguid, ");
            sqlCommand.Append("answer, ");
            sqlCommand.Append("answereddate )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":answerguid, ");
            sqlCommand.Append(":questionguid, ");
            sqlCommand.Append(":responseguid, ");
            sqlCommand.Append(":answer, ");
            sqlCommand.Append(":answereddate ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[5];

            arParams[0] = new NpgsqlParameter("answerguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            arParams[4] = new NpgsqlParameter("answereddate", NpgsqlTypes.NpgsqlDbType.Timestamp);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = DateTime.UtcNow;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_SurveyQuestionAnswers table. Returns true if row updated.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="answer"> answer </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid answerGuid,
            Guid questionGuid,
            Guid responseGuid,
            string answer)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_surveyquestionanswers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("questionguid = :questionguid, ");
            sqlCommand.Append("responseguid = :responseguid, ");
            sqlCommand.Append("answer = :answer ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("answerguid = :answerguid ");
            sqlCommand.Append(";");

            NpgsqlParameter[] arParams = new NpgsqlParameter[4];

            arParams[0] = new NpgsqlParameter("answerguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new NpgsqlParameter("answer", NpgsqlTypes.NpgsqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            int rowsAffected = NpgsqlHelper.ExecuteNonQuery(ConnectionString.GetWriteConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestionAnswers table.
        /// </summary>
        /// <param name="answerGuid"> answerGuid </param>
        public static IDataReader GetOne(Guid responseGuid, Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_surveyquestionanswers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("questionguid = :questionguid ");
            sqlCommand.Append("AND responseguid = :responseguid; ");

            NpgsqlParameter[] arParams = new NpgsqlParameter[2];

            arParams[0] = new NpgsqlParameter("questionguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            arParams[1] = new NpgsqlParameter("responseguid", NpgsqlTypes.NpgsqlDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseGuid.ToString();

            return NpgsqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }


    }
}
