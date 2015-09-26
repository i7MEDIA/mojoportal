/// Author:				Rob Henry
/// Created:			2007-11-26
/// Last Modified:		2009-02-23
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
using System.Globalization;
using System.Text;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

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
            #region Bit Conversion

            int intAnnonymous;
            if (annonymous)
            {
                intAnnonymous = 1;
            }
            else
            {
                intAnnonymous = 0;
            }

            int intComplete;
            if (complete)
            {
                intComplete = 1;
            }
            else
            {
                intComplete = 0;
            }


            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyResponses (");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("SurveyGuid, ");
            sqlCommand.Append("UserGuid, ");
            //sqlCommand.Append("SubmissionDate, ");
            sqlCommand.Append("Annonymous, ");
            sqlCommand.Append("Complete )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ResponseGuid, ");
            sqlCommand.Append("?SurveyGuid, ");
            sqlCommand.Append("?UserGuid, ");
            //sqlCommand.Append(DateTime.Now.ToShortDateString() + ", ");
            sqlCommand.Append("?Annonymous, ");
            sqlCommand.Append("?Complete );");

            MySqlParameter[] arParams = new MySqlParameter[5];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new MySqlParameter("?SurveyGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            arParams[2] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = userGuid.ToString();

            arParams[3] = new MySqlParameter("?Annonymous", MySqlDbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intAnnonymous;

            arParams[4] = new MySqlParameter("?Complete", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intComplete;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
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
            
            #region Bit Conversion

            int intComplete;

            if (complete)
            {
                intComplete = 1;
            }
            else
            {
                intComplete = 0;
            }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE mp_SurveyResponses SET ");
            sqlCommand.Append("Complete = ?Complete, ");
            sqlCommand.Append("SubmissionDate = ?SubmissionDate ");

            sqlCommand.Append("WHERE ResponseGuid = ?ResponseGuid");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new MySqlParameter("?Complete", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = intComplete;

            arParams[2] = new MySqlParameter("?SubmissionDate", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = submissionDate;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);
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
            sqlCommand.Append("DELETE FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid ;");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SurveyGuid = ?SurveyGuid; ");

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
        /// Gets an IDataReader with the first response to a survey
        /// </summary>
        public static IDataReader GetFirst(Guid surveyGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SurveyGuid = ?SurveyGuid ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("ORDER BY SubmissionDate, ResponseGuid ");
            sqlCommand.Append("LIMIT 1; ");

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
        /// Gets an IDataReader with the next response to a survey
        /// </summary>
        public static IDataReader GetNext(Guid responseGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SubmissionDate > (");
            sqlCommand.Append("SELECT SubmissionDate ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid) ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("Order By SubmissionDate, ResponseGuid ");
            sqlCommand.Append("Limit 1; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE Complete = 1 AND SubmissionDate < (");
            sqlCommand.Append("SELECT SubmissionDate ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = ?ResponseGuid) ");
            sqlCommand.Append("Order By SubmissionDate DESC, ResponseGuid; ");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ResponseGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }



    }
}
