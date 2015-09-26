/// Author:				Joe Audette
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
    public static class DBQuestion
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
        /// Inserts a row in the mp_SurveyQuestions table. Returns rows affected count.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="answerIsRequired"> answerIsRequired </param>
        /// <param name="questionOrder"> questionOrder </param>
        /// <param name="validationMessage"> validationMessage </param>
        /// <returns>int</returns>
        public static int Add(
            Guid questionGuid,
            Guid pageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            string validationMessage)
        {
            #region Bit Conversion

            int intAnswerIsRequired;
            if (answerIsRequired)
            {
                intAnswerIsRequired = 1;
            }
            else
            {
                intAnswerIsRequired = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyQuestions (");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("PageGuid, ");
            sqlCommand.Append("QuestionText, ");
            sqlCommand.Append("QuestionTypeId, ");
            sqlCommand.Append("AnswerIsRequired, ");
            sqlCommand.Append("QuestionOrder, ");
            sqlCommand.Append("ValidationMessage )");

            sqlCommand.Append("SELECT :QuestionGuid, :PageGuid, :QuestionText, ");
            sqlCommand.Append(":QuestionTypeId, :AnswerIsRequired, Count(*), :ValidationMessage ");
            sqlCommand.Append("FROM mp_SurveyPages; ");

            SqliteParameter[] arParams = new SqliteParameter[6];

            arParams[0] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            arParams[1] = new SqliteParameter(":PageGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageGuid.ToString();

            arParams[2] = new SqliteParameter(":QuestionText", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new SqliteParameter(":QuestionTypeId", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new SqliteParameter(":AnswerIsRequired", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intAnswerIsRequired;

            arParams[5] = new SqliteParameter(":ValidationMessage", DbType.String, 256);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = validationMessage;

            int rowsAffected = 0;
            rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return rowsAffected;

        }

        /// <summary>
        /// Updates a row in the mp_SurveyQuestions table. Returns true if row updated.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="questionText"> questionText </param>
        /// <param name="questionTypeId"> questionTypeId </param>
        /// <param name="answerIsRequired"> answerIsRequired </param>
        /// <param name="questionOrder"> questionOrder </param>
        /// <param name="validationMessage"> validationMessage </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid questionGuid,
            Guid pageGuid,
            string questionText,
            int questionTypeId,
            bool answerIsRequired,
            int questionOrder,
            string validationMessage)
        {
            #region Bit Conversion

            int intAnswerIsRequired;
            if (answerIsRequired)
            {
                intAnswerIsRequired = 1;
            }
            else
            {
                intAnswerIsRequired = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("UPDATE mp_SurveyQuestions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("PageGuid = :PageGuid, ");
            sqlCommand.Append("QuestionText = :QuestionText, ");
            sqlCommand.Append("QuestionTypeId = :QuestionTypeId, ");
            sqlCommand.Append("AnswerIsRequired = :AnswerIsRequired, ");
            sqlCommand.Append("QuestionOrder = :QuestionOrder, ");
            sqlCommand.Append("ValidationMessage = :ValidationMessage ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[7];

            arParams[0] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            arParams[1] = new SqliteParameter(":PageGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageGuid.ToString();

            arParams[2] = new SqliteParameter(":QuestionText", DbType.Object);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = questionText;

            arParams[3] = new SqliteParameter(":QuestionTypeId", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = questionTypeId;

            arParams[4] = new SqliteParameter(":AnswerIsRequired", DbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intAnswerIsRequired;

            arParams[5] = new SqliteParameter(":QuestionOrder", DbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = questionOrder;

            arParams[6] = new SqliteParameter(":ValidationMessage", DbType.String, 256);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = validationMessage;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyQuestions table. Returns true if row deleted.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid questionGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid; ");

            //now delete the question
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();


            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyQuestions table.
        /// </summary>
        /// <param name="questionGuid"> questionGuid </param>
        public static IDataReader GetOne(
            Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = :QuestionGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":QuestionGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyQuestions table.
        /// </summary>
        public static IDataReader GetAllByPage(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE PageGuid = :PageGuid ");
            sqlCommand.Append("ORDER BY QuestionOrder; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":PageGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
