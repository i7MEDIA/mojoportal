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
    public static class DBSurveyResponse
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
            //sqlCommand.Append("SubmissionDate, ");
            sqlCommand.Append("Annonymous, ");
            sqlCommand.Append("Complete, ");
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append(":ResponseGuid, ");
            sqlCommand.Append(":SurveyGuid, ");
            //sqlCommand.Append(":SubmissionDate, ");
            sqlCommand.Append(":Annonymous, ");
            sqlCommand.Append(":Complete, ");
            sqlCommand.Append(":UserGuid )");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[5];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            //arParams[2] = new SqliteParameter(":SubmissionDate", DbType.DateTime);
            //arParams[2].Direction = ParameterDirection.Input;
            //arParams[2].Value = submissionDate;

            arParams[2] = new SqliteParameter(":Annonymous", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intAnnonymous;

            arParams[3] = new SqliteParameter(":Complete", DbType.Int32);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intComplete;

            arParams[4] = new SqliteParameter(":UserGuid", DbType.String, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            int rowsAffected = 0;
            rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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

            sqlCommand.Append("UPDATE mp_SurveyResponses ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SubmissionDate = :SubmissionDate, ");
            sqlCommand.Append("Complete = :Complete ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ResponseGuid = :ResponseGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[3];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new SqliteParameter(":SubmissionDate", DbType.DateTime);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = submissionDate;

            arParams[2] = new SqliteParameter(":Complete", DbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intComplete;

            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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
            sqlCommand.Append("ResponseGuid = :ResponseGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            int rowsAffected = SqliteHelper.ExecuteNonQuery(GetConnectionString(), sqlCommand.ToString(), arParams);
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
            sqlCommand.Append("ResponseGuid = :ResponseGuid ");
            sqlCommand.Append(";");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE SurveyGuid = :SurveyGuid; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("WHERE SurveyGuid = :SurveyGuid ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("ORDER BY SubmissionDate, ResponseGuid ");
            sqlCommand.Append("LIMIT 1; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":SurveyGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("ResponseGuid = :ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = :ResponseGuid) ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("Order By SubmissionDate, ResponseGuid ");
            sqlCommand.Append("Limit 1; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
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
            sqlCommand.Append("ResponseGuid = :ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = :ResponseGuid) ");
            sqlCommand.Append("Order By SubmissionDate DESC, ResponseGuid; ");

            SqliteParameter[] arParams = new SqliteParameter[1];

            arParams[0] = new SqliteParameter(":ResponseGuid", DbType.String, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return SqliteHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
