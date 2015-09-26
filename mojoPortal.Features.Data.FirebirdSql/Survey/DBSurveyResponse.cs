using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using FirebirdSql.Data.FirebirdClient;
//using log4net;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    /// <summary>
    /// Author:				Joe Audette
    /// Created:			2008-08-29
    /// Last Modified:		2008-08-29
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    ///  
    /// </summary>
    public static class DBSurveyResponse
    {
        private static String GetConnectionString()
        {
            return ConfigurationManager.AppSettings["FirebirdConnectionString"];

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

            FbParameter[] arParams = new FbParameter[5];


            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = surveyGuid.ToString();

            //arParams[2] = new FbParameter("@SubmissionDate", FbDbType.TimeStamp);
            //arParams[2].Direction = ParameterDirection.Input;
            //arParams[2].Value = submissionDate;

            arParams[2] = new FbParameter("@Annonymous", FbDbType.SmallInt);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intAnnonymous;

            arParams[3] = new FbParameter("@Complete", FbDbType.SmallInt);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = intComplete;

            arParams[4] = new FbParameter("@UserGuid", FbDbType.Char, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = userGuid.ToString();

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_SurveyResponses (");
            sqlCommand.Append("ResponseGuid, ");
            sqlCommand.Append("SurveyGuid, ");
            //sqlCommand.Append("SubmissionDate, ");
            sqlCommand.Append("Annonymous, ");
            sqlCommand.Append("Complete, ");
            sqlCommand.Append("UserGuid )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("@ResponseGuid, ");
            sqlCommand.Append("@SurveyGuid, ");
            //sqlCommand.Append("@SubmissionDate, ");
            sqlCommand.Append("@Annonymous, ");
            sqlCommand.Append("@Complete, ");
            sqlCommand.Append("@UserGuid )");
            sqlCommand.Append(";");

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("UPDATE mp_SurveyResponses ");
            sqlCommand.Append("SET  ");
            sqlCommand.Append("SubmissionDate = @SubmissionDate, ");
            sqlCommand.Append("Complete = @Complete ");
            
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[6];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            arParams[1] = new FbParameter("@SubmissionDate", FbDbType.TimeStamp);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = submissionDate;

            arParams[2] = new FbParameter("@Complete", FbDbType.SmallInt);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intComplete;

            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append(";");
            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();


            int rowsAffected = FBSqlHelper.ExecuteNonQuery(
                GetConnectionString(),
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
            sqlCommand.Append("ResponseGuid = @ResponseGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SurveyGuid = @SurveyGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT FIRST 1  * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SurveyGuid = @SurveyGuid ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("ORDER BY SubmissionDate, ResponseGuid ");
            sqlCommand.Append(";");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@SurveyGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = surveyGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("SELECT FIRST 1 * ");
            sqlCommand.Append("FROM	mp_SurveyResponses ");
            sqlCommand.Append("WHERE SubmissionDate > (");
            sqlCommand.Append("SELECT SubmissionDate ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");
            sqlCommand.Append("AND Complete = 1 ");
            sqlCommand.Append("Order By SubmissionDate, ResponseGuid ");
            sqlCommand.Append("; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return FBSqlHelper.ExecuteReader(
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
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("SurveyGuid = (");
            sqlCommand.Append("SELECT SurveyGuid ");
            sqlCommand.Append("FROM mp_SurveyResponses ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ResponseGuid = @ResponseGuid) ");
            sqlCommand.Append("Order By SubmissionDate DESC, ResponseGuid; ");

            FbParameter[] arParams = new FbParameter[1];

            arParams[0] = new FbParameter("@ResponseGuid", FbDbType.Char, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = responseGuid.ToString();

            return FBSqlHelper.ExecuteReader(
                GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
