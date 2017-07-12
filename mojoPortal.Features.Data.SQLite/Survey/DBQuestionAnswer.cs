/// Author:				
/// Created:			2008-08-29
/// Last Modified:		2008-08-30
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Configuration;
using Mono.Data.Sqlite;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    
    public static class DBQuestionAnswer
    {
        private static string GetConnectionString()
        {
            string connectionString = ConfigurationManager.AppSettings["SqliteConnectionString"];
            if (connectionString == "defaultdblocation")
            {

                connectionString = "version=3,URI=file:"
                    + System.Web.Hosting.HostingEnvironment.MapPath("~/Data/sqlitedb/mojo.db.config");

            }
            return connectionString;
        }

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
            sqlCommand.Append("INSERT INTO mp_SurveyQuestionAnswers (");
            sqlCommand.Append("AnswerGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("AnsweredDate )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":AnswerGuid, ");
            sqlCommand.Append(":QuestionGuid, ");
            sqlCommand.Append(":ResponseGuid, ");
            sqlCommand.Append(":Answer, ");
            sqlCommand.Append(":AnsweredDate )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":AnswerGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new SqliteParameter(":Answer", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            arParams[4] = new SqliteParameter(":AnsweredDate", DbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = DateTime.UtcNow;

            int rowsAffected = 0;
            rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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

            sqlCommand.Append("UPDATE mp_SurveyQuestionAnswers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid, ");
            sqlCommand.Append("ResponseGuid = :ResponseGuid, ");
            sqlCommand.Append("Answer = :Answer ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("AnswerGuid = :AnswerGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[4];

            arParams[0] = new SqliteParameter(":AnswerGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new SqliteParameter(":Answer", DbType.Object);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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
            sqlCommand.Append("FROM	mp_SurveyQuestionAnswers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid ");
            sqlCommand.Append("AND ResponseGuid = :ResponseGuid; ");

            SqliteParameter[] arParams = new SqliteParameter[2];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
