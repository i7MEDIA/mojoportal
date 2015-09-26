/// Author:				Rob Henry
/// Created:			2007-08-31
/// Last Modified:		2008-01-21
/// 
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
using System.Data.SqlClient;
using System.Configuration;
using mojoPortal.Data;

namespace SurveyFeature.Data
{
    
    public static class DBSurveyPage
    {
       
        /// <summary>
        /// Inserts a row in the mp_SurveyPages table. Returns rows affected count.
        /// </summary>
        /// <param name="surveySurveyPageGuid"> surveySurveyPageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <returns>int</returns>
        public static int Add(
            Guid pageGuid,
            Guid surveyGuid,
            string pageTitle,
            bool pageEnabled)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyPages_Insert", 4);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@PageEnabled", SqlDbType.Bit, ParameterDirection.Input, pageEnabled);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }


        /// <summary>
        /// Updates a row in the mp_SurveyPages table. Returns true if row updated.
        /// </summary>
        /// <param name="surveyPageGuid"> surveyPageGuid </param>
        /// <param name="surveyGuid"> surveyGuid </param>
        /// <param name="pageTitle"> pageTitle </param>
        /// <param name="pageOrder"> pageOrder </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid surveyPageGuid,
            Guid surveyGuid,
            string pageTitle,
            int pageOrder,
            bool pageEnabled)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyPages_Update", 5);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            sph.DefineSqlParameter("@PageTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, pageTitle);
            sph.DefineSqlParameter("@PageOrder", SqlDbType.Int, ParameterDirection.Input, pageOrder);
            sph.DefineSqlParameter("@PageEnabled", SqlDbType.Bit, ParameterDirection.Input, pageEnabled);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the mp_SurveyPages table. Returns true if row deleted.
        /// </summary>
        /// <param name="surveyPageGuid"> surveyPageGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid surveyPageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "mp_SurveyPages_Delete", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the mp_SurveyPages table.
        /// </summary>
        /// <param name="surveyPageGuid"> surveyPageGuid </param>
        public static IDataReader GetOne(
            Guid surveyPageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyPages_SelectOne", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyPageGuid);
            return sph.ExecuteReader();
        }


        /// <summary>
        /// Gets an IDataReader with all rows in the mp_SurveyPages table.
        /// </summary>
        public static IDataReader GetAll(Guid surveyGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyPages_SelectAll", 1);
            sph.DefineSqlParameter("@SurveyGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, surveyGuid);
            return sph.ExecuteReader();
        }

        public static int GetQuestionsCount(Guid pageGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "mp_SurveyPages_CountQuestions", 1);
            sph.DefineSqlParameter("@PageGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, pageGuid);
            return (int)sph.ExecuteScalar();
        }



    }
}
