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
    public static class DBSurveyPage
    {
        
        /// <summary>
        /// Inserts a row in the mp_SurveyPages table. Returns rows affected count.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <param name="pageEnabled"> pageEnabled </param>
        /// <returns>int</returns>
        public static int Add(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            bool pageEnabled)
        {
            #region Bit Conversion

            int intPageEnabled;

            if (pageEnabled)
            {
                intPageEnabled = 1;
            }
            else
            {
                intPageEnabled = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            //sqlCommand.Append("SELECT ?PageCount = COUNT(*) FROM mp_SurveyPages; ");
            sqlCommand.Append("INSERT INTO mp_SurveyPages ");
            sqlCommand.Append("(PageGuid, ");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("PageTitle, ");
            sqlCommand.Append("PageOrder, ");
            sqlCommand.Append("PageEnabled) ");
            sqlCommand.Append("SELECT ?PageGuid, ?SurveyGuid, ?PageTitle, ");
            sqlCommand.Append("Count(*), ?PageEnabled FROM mp_SurveyPages; ");
            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            arParams[1] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            arParams[2] = new MySqlParameter("?PageTitle", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new MySqlParameter("?PageEnabled", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intPageEnabled;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the mp_SurveyPages table. Returns true if row updated.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <param name="pageEnabled"> pageEnabled </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            int pageOrder,
            bool pageEnabled)
        {
            #region Bit Conversion

            int intPageEnabled;
            if (pageEnabled)
            {
                intPageEnabled = 1;
            }
            else
            {
                intPageEnabled = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyPages ");
            sqlCommand.Append("SET ");
            sqlCommand.Append("SurveyGuid = ?SurveyGuid, ");
            sqlCommand.Append("PageTitle = ?PageTitle, ");
            sqlCommand.Append("PageOrder = ?PageOrder, ");
            sqlCommand.Append("PageEnabled = ?PageEnabled ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            arParams[1] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            arParams[2] = new MySqlParameter("?PageTitle", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageTitle;

            arParams[3] = new MySqlParameter("?PageOrder", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = pageOrder;

            arParams[4] = new MySqlParameter("?PageEnabled", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intPageEnabled;
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        /// <summary>
        /// Deletes a row from the mp_SurveyPages table. Returns true if row deleted.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid pageGuid)
        {
            //first delete questionOptions
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM mp_SurveyQuestionOptions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("QuestionGuid IN (");
            sqlCommand.Append("SELECT QuestionGuid ");
            sqlCommand.Append("FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid); ");

            //now delete survey questions
            sqlCommand.Append("DELETE FROM mp_SurveyQuestions ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid; ");

            //now delete pages
            sqlCommand.Append("DELETE FROM mp_SurveyPages ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("PageGuid = ?PageGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyPages table.
        /// </summary>
        /// <param name="pageGuid"> pageGuid </param>
        public static IDataReader GetOne(
            Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  sp.*, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount ");
            sqlCommand.Append("FROM	mp_SurveyPages sp ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("sp.PageGuid = ?PageGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyPages table.
        /// </summary>
        public static IDataReader GetAll(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT sp.*, ");
            sqlCommand.Append("(SELECT COUNT(*) FROM mp_SurveyQuestions sq WHERE sp.PageGuid = sq.PageGuid) AS QuestionCount ");
            sqlCommand.Append("FROM	mp_SurveyPages sp ");
            sqlCommand.Append("WHERE sp.SurveyGuid = ?SurveyGuid ");
            sqlCommand.Append("ORDER BY sp.PageOrder; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        /// <summary>
        /// Gets a count of rows in the mp_SurveyPages table.
        /// </summary>
        public static int GetQuestionsCount(Guid pageGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	mp_SurveyQuestions ");
            sqlCommand.Append("WHERE PageGuid = ?PageGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = pageGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));
        }

        



    }
}
