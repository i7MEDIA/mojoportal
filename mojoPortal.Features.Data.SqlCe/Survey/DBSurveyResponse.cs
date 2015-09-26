// Author:				Joe Audette
// Created:			    2010-07-02
// Last Modified:		2010-07-09
// 
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.SqlServerCe;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    public static class DBSurveyResponse
    {
        private static String GetConnectionString()
        {
            return DBPortal.GetConnectionString();
        }

        /// <summary>
        /// Inserts a row in the mp_SurveyResponses table. Returns rows affected count.
        /// </summary>
        /// <param name="responseGuid"> responseGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="userId"> userId </param>
        /// <param name="submissionDate"> submissionDate </param>
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
            sqlCommand.Append("INSERT INTO mp_SurveyResponses ");
            sqlCommand.Append("(");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("SurveyGuid, ");
            //sqlCommand.Append("SubmissionDate, ");
            sqlCommand.Append("Annonymous, ");
            sqlCommand.Append("Complete, ");
            sqlCommand.Append("UserGuid ");
            sqlCommand.Append(")");

            sqlCommand.Append(" VALUES ");
            sqlCommand.Append("(");
            sqlCommand.Append("@ResponseGuid, ");
            sqlCommand.Append("@SurveyGuid, ");
            //sqlCommand.Append("@SubmissionDate, ");
            sqlCommand.Append("@Annonymous, ");
            sqlCommand.Append("@Complete, ");
            sqlCommand.Append("@UserGuid ");
            sqlCommand.Append(")");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[5];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            arParams[1] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid;

            arParams[2] = new SqlCeParameter("@Annonymous", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = annonymous;

            arParams[3] = new SqlCeParameter("@Complete", SqlDbType.Bit);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = complete;

            arParams[4] = new SqlCeParameter("@UserGuid", SqlDbType.UniqueIdentifier);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("UPDATE mp_SurveyResponses ");
            sqlCommand.Append("SET  ");
          
            sqlCommand.Append("SubmissionDate = @SubmissionDate, ");
            
            sqlCommand.Append("Complete = @Complete ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append(";");

            SqlCeParameter[] arParams = new SqlCeParameter[3];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            arParams[1] = new SqlCeParameter("@SubmissionDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = submissionDate;

            arParams[2] = new SqlCeParameter("@Complete", SqlDbType.Bit);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = complete;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("DELETE FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            int rowsAffected = SqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ;");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SurveyGuid = @SurveyGuid; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT TOP(1) * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SurveyGuid = @SurveyGuid ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("ORDER BY SubmissionDate, ResponseGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@SurveyGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT SubmissionDate ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            DateTime responseDate = Convert.ToDateTime(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE Complete = 1 AND SubmissionDate > @ResponseDate ");

            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid IN (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");

            sqlCommand.Append("Order By SubmissionDate, ResponseGuid; ");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            arParams[1] = new SqlCeParameter("@ResponseDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseDate;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("SELECT SubmissionDate ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append("; ");

            SqlCeParameter[] arParams = new SqlCeParameter[1];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            DateTime responseDate = Convert.ToDateTime(SqlHelper.ExecuteScalar(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams));

            sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT TOP(1) * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE Complete = 1 AND SubmissionDate < @ResponseDate ");
            
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid IN (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");

            sqlCommand.Append("Order By SubmissionDate DESC, ResponseGuid; ");

            arParams = new SqlCeParameter[2];

            arParams[0] = new SqlCeParameter("@ResponseGuid", SqlDbType.UniqueIdentifier);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid;

            arParams[1] = new SqlCeParameter("@ResponseDate", SqlDbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = responseDate;

            return SqlHelper.ExecuteReader(
                GetConnectionString(),
                CommandType.Text,
                sqlCommand.ToString(),
                arParams);

        }

    }
}
