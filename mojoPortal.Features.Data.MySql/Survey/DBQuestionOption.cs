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
            sqlCommand.Append("INSERT INTO mp_SurveyQuestionOptions (");
            sqlCommand.Append("QuestionOptionGuid, ");
            sqlCommand.Append("QuestionGuid, ");
            sqlCommand.Append("Answer, ");
            sqlCommand.Append("`Order`) ");

            sqlCommand.Append("VALUES(");
            sqlCommand.Append("?QuestionOptionGuid, ");
            sqlCommand.Append("?QuestionGuid, ");
            sqlCommand.Append("?Answer, ");
            sqlCommand.Append("?Order); ");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?QuestionOptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new MySqlParameter("?Answer", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new MySqlParameter("?Order", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("UPDATE mp_SurveyQuestionOptions ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("QuestionGuid = ?QuestionGuid, ");
            sqlCommand.Append("Answer = ?Answer, ");
            sqlCommand.Append("`Order` = ?Order ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionOptionGuid = ?QuestionOptionGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?QuestionOptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            arParams[1] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = questionGuid.ToString();

            arParams[2] = new MySqlParameter("?Answer", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = answer;

            arParams[3] = new MySqlParameter("?Order", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = order;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionOptionGuid = ?QuestionOptionGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QuestionOptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyQuestionOptions table.
        /// </summary>
        public static IDataReader GetAll(Guid questionGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE QuestionGuid = ?QuestionGuid ");
            sqlCommand.Append("ORDER BY `Order` ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QuestionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionOptionGuid = ?QuestionOptionGuid ");
            sqlCommand.Append("ORDER BY `Order`; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?QuestionOptionGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = questionOptionGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
