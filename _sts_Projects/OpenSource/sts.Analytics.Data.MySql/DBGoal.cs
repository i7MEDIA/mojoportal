// Author:					Joe Audette
// Created:					2010-09-08
// Last Modified:			2010-09-08
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace sts.Analytics.Data
{
    public static class DBGoal
    {
        /// <summary>
        /// Inserts a row in the sts_ga_Goal table. Returns rows affected count.
        /// </summary>
        /// <param name="rowGuid"> rowGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="goal1Name"> goal1Name </param>
        /// <param name="goal1Value"> goal1Value </param>
        /// <param name="goal1IsActtive"> goal1IsActtive </param>
        /// <param name="goal2Name"> goal2Name </param>
        /// <param name="goal2Value"> goal2Value </param>
        /// <param name="goal2IsActive"> goal2IsActive </param>
        /// <param name="goal3Name"> goal3Name </param>
        /// <param name="goal3Value"> goal3Value </param>
        /// <param name="goal3IsActive"> goal3IsActive </param>
        /// <param name="goal4Name"> goal4Name </param>
        /// <param name="goal4Value"> goal4Value </param>
        /// <param name="gaol4IsActive"> gaol4IsActive </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>int</returns>
        public static int Create(
            Guid rowGuid,
            Guid siteGuid,
            string profileId,
            string goal1Name,
            decimal goal1Value,
            bool goal1IsActtive,
            string goal2Name,
            decimal goal2Value,
            bool goal2IsActive,
            string goal3Name,
            decimal goal3Value,
            bool goal3IsActive,
            string goal4Name,
            decimal goal4Value,
            bool gaol4IsActive,
            DateTime capturedUtc)
        {
            #region Bit Conversion

            int intGoal1IsActtive = 0;
            if (goal1IsActtive) { intGoal1IsActtive = 1; }
            int intGoal2IsActive = 0;
            if (goal2IsActive) { intGoal2IsActive = 1; }
            int intGoal3IsActive = 0;
            if (goal3IsActive) { intGoal3IsActive = 1; }
            int intGaol4IsActive = 0;
            if (gaol4IsActive) { intGaol4IsActive = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ga_Goal (");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ProfileId, ");
            sqlCommand.Append("Goal1Name, ");
            sqlCommand.Append("Goal1Value, ");
            sqlCommand.Append("Goal1IsActtive, ");
            sqlCommand.Append("Goal2Name, ");
            sqlCommand.Append("Goal2Value, ");
            sqlCommand.Append("Goal2IsActive, ");
            sqlCommand.Append("Goal3Name, ");
            sqlCommand.Append("Goal3Value, ");
            sqlCommand.Append("Goal3IsActive, ");
            sqlCommand.Append("Goal4Name, ");
            sqlCommand.Append("Goal4Value, ");
            sqlCommand.Append("Gaol4IsActive, ");
            sqlCommand.Append("CapturedUtc )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?RowGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ProfileId, ");
            sqlCommand.Append("?Goal1Name, ");
            sqlCommand.Append("?Goal1Value, ");
            sqlCommand.Append("?Goal1IsActtive, ");
            sqlCommand.Append("?Goal2Name, ");
            sqlCommand.Append("?Goal2Value, ");
            sqlCommand.Append("?Goal2IsActive, ");
            sqlCommand.Append("?Goal3Name, ");
            sqlCommand.Append("?Goal3Value, ");
            sqlCommand.Append("?Goal3IsActive, ");
            sqlCommand.Append("?Goal4Name, ");
            sqlCommand.Append("?Goal4Value, ");
            sqlCommand.Append("?Gaol4IsActive, ");
            sqlCommand.Append("?CapturedUtc )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[16];

            arParams[0] = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = rowGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = profileId;

            arParams[3] = new MySqlParameter("?Goal1Name", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = goal1Name;

            arParams[4] = new MySqlParameter("?Goal1Value", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = goal1Value;

            arParams[5] = new MySqlParameter("?Goal1IsActtive", MySqlDbType.Int32);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = intGoal1IsActtive;

            arParams[6] = new MySqlParameter("?Goal2Name", MySqlDbType.VarChar, 255);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = goal2Name;

            arParams[7] = new MySqlParameter("?Goal2Value", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = goal2Value;

            arParams[8] = new MySqlParameter("?Goal2IsActive", MySqlDbType.Int32);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = intGoal2IsActive;

            arParams[9] = new MySqlParameter("?Goal3Name", MySqlDbType.VarChar, 255);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = goal3Name;

            arParams[10] = new MySqlParameter("?Goal3Value", MySqlDbType.Decimal);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = goal3Value;

            arParams[11] = new MySqlParameter("?Goal3IsActive", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intGoal3IsActive;

            arParams[12] = new MySqlParameter("?Goal4Name", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = goal4Name;

            arParams[13] = new MySqlParameter("?Goal4Value", MySqlDbType.Decimal);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = goal4Value;

            arParams[14] = new MySqlParameter("?Gaol4IsActive", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intGaol4IsActive;

            arParams[15] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = capturedUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the sts_ga_Goal table. Returns true if row updated.
        /// </summary>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="profileId"> profileId </param>
        /// <param name="goal1Name"> goal1Name </param>
        /// <param name="goal1Value"> goal1Value </param>
        /// <param name="goal1IsActtive"> goal1IsActtive </param>
        /// <param name="goal2Name"> goal2Name </param>
        /// <param name="goal2Value"> goal2Value </param>
        /// <param name="goal2IsActive"> goal2IsActive </param>
        /// <param name="goal3Name"> goal3Name </param>
        /// <param name="goal3Value"> goal3Value </param>
        /// <param name="goal3IsActive"> goal3IsActive </param>
        /// <param name="goal4Name"> goal4Name </param>
        /// <param name="goal4Value"> goal4Value </param>
        /// <param name="gaol4IsActive"> gaol4IsActive </param>
        /// <param name="capturedUtc"> capturedUtc </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid siteGuid,
            string profileId,
            string goal1Name,
            decimal goal1Value,
            bool goal1IsActtive,
            string goal2Name,
            decimal goal2Value,
            bool goal2IsActive,
            string goal3Name,
            decimal goal3Value,
            bool goal3IsActive,
            string goal4Name,
            decimal goal4Value,
            bool gaol4IsActive,
            DateTime capturedUtc)
        {
            #region Bit Conversion

            int intGoal1IsActtive = 0;
            if (goal1IsActtive) { intGoal1IsActtive = 1; }
            int intGoal2IsActive = 0;
            if (goal2IsActive) { intGoal2IsActive = 1; }
            int intGoal3IsActive = 0;
            if (goal3IsActive) { intGoal3IsActive = 1; }
            int intGaol4IsActive = 0;
            if (gaol4IsActive) { intGaol4IsActive = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_ga_Goal ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Goal1Name = ?Goal1Name, ");
            sqlCommand.Append("Goal1Value = ?Goal1Value, ");
            sqlCommand.Append("Goal1IsActtive = ?Goal1IsActtive, ");
            sqlCommand.Append("Goal2Name = ?Goal2Name, ");
            sqlCommand.Append("Goal2Value = ?Goal2Value, ");
            sqlCommand.Append("Goal2IsActive = ?Goal2IsActive, ");
            sqlCommand.Append("Goal3Name = ?Goal3Name, ");
            sqlCommand.Append("Goal3Value = ?Goal3Value, ");
            sqlCommand.Append("Goal3IsActive = ?Goal3IsActive, ");
            sqlCommand.Append("Goal4Name = ?Goal4Name, ");
            sqlCommand.Append("Goal4Value = ?Goal4Value, ");
            sqlCommand.Append("Gaol4IsActive = ?Gaol4IsActive, ");
            sqlCommand.Append("CapturedUtc = ?CapturedUtc ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[15];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            arParams[2] = new MySqlParameter("?Goal1Name", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = goal1Name;

            arParams[3] = new MySqlParameter("?Goal1Value", MySqlDbType.Decimal);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = goal1Value;

            arParams[4] = new MySqlParameter("?Goal1IsActtive", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intGoal1IsActtive;

            arParams[5] = new MySqlParameter("?Goal2Name", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = goal2Name;

            arParams[6] = new MySqlParameter("?Goal2Value", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = goal2Value;

            arParams[7] = new MySqlParameter("?Goal2IsActive", MySqlDbType.Int32);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = intGoal2IsActive;

            arParams[8] = new MySqlParameter("?Goal3Name", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = goal3Name;

            arParams[9] = new MySqlParameter("?Goal3Value", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = goal3Value;

            arParams[10] = new MySqlParameter("?Goal3IsActive", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intGoal3IsActive;

            arParams[11] = new MySqlParameter("?Goal4Name", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = goal4Name;

            arParams[12] = new MySqlParameter("?Goal4Value", MySqlDbType.Decimal);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = goal4Value;

            arParams[13] = new MySqlParameter("?Gaol4IsActive", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intGaol4IsActive;

            arParams[14] = new MySqlParameter("?CapturedUtc", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = capturedUtc;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool DeleteByProfile(Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ga_Goal ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader GetByProfile(Guid siteGuid, string profileId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_ga_Goal ");
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("ProfileId = ?ProfileId");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            arParams[1] = new MySqlParameter("?ProfileId", MySqlDbType.VarChar, 20);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = profileId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

    }
}
