/// Author:				Rob Henry
/// Created:			2007-11-26
/// Last Modified:		2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyQuestionAnswers (");
            sqlCommand.Append("AnswerGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("AnsweredDate ");
            sqlCommand.Append(") ");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?AnswerGuid, ");
            sqlCommand.Append("?QuestionGuid, ");
            sqlCommand.Append("?ResponseGuid, ");
            sqlCommand.Append("?Answer, ");
            sqlCommand.Append("?AnsweredDate ");
            sqlCommand.Append("); ");
            
            
            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?AnswerGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new MySqlParameter("?Answer", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            arParams[4] = new MySqlParameter("?AnsweredDate", MySqlDbType.DateTime);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = DateTime.UtcNow;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            #region Bit Conversion


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyQuestionAnswers ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionGuid = ?QuestionGuid, ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid, ");
            sqlCommand.Append("Answer = ?Answer ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("AnswerGuid = ?AnswerGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?AnswerGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = answerGuid.ToString();

            arParams[1] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = responseGuid.ToString();

            arParams[3] = new MySqlParameter("?Answer", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = answer;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyQuestionAnswers ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid = ?QuestionGuid ");
            sqlCommand.Append("AND ResponseGuid = ?ResponseGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();
            arParams[1] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

    }
}
