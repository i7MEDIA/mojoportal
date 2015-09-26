// Author:					Joe Audette
// Created:					2013-9-6
// Last Modified:			2013-9-6
// 
// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using System.IO;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace sts.FundRaiser.Data
{
    public static class DBContributor
    {

        public static int Create(
            Guid guid,
            Guid siteGuid,
            Guid moduleGuid,
            Guid userGuid,
            Guid levelGuid,
            bool isAnonymous,
            string firstName,
            string lastName,
            string companyName,
            string emailAddress,
            string phone,
            string webSiteUrl,
            string contribComment,
            string bio,
            string address1,
            string address2,
            string suburb,
            string city,
            string state,
            string country,
            string postalCode,
            decimal totalContribution,
            DateTime firstContributionUtc,
            DateTime lastContributionUtc,
            DateTime lastModUtc,
            Guid lastModBy,

            bool hideFirstName,
            bool hideLastName,
            bool hideCompanyName,
            bool hideAddress1,
            bool hideAddress2,
            bool hideCity,
            bool hideSuburb,
            bool hideState,
            bool hideCountry,
            bool hidePostalCode,
            bool hideEmail,
            bool hidePhone,
            bool hideComment,

            bool hideAmount,
            bool hideLevel,
            bool hidePhoto,
            bool subscribeToNews)
        {
            #region Bit Conversion

            int intIsAnonymous = 0;
            if (isAnonymous) { intIsAnonymous = 1; }
            int intHideFirstName = 0;
            if (hideFirstName) { intHideFirstName = 1; }
            int intHideLastName = 0;
            if (hideLastName) { intHideLastName = 1; }
            int intHideCompanyName = 0;
            if (hideCompanyName) { intHideCompanyName = 1; }
            int intHideAddress1 = 0;
            if (hideAddress1) { intHideAddress1 = 1; }
            int intHideAddress2 = 0;
            if (hideAddress2) { intHideAddress2 = 1; }
            int intHideCity = 0;
            if (hideCity) { intHideCity = 1; }
            int intHideSuburb = 0;
            if (hideSuburb) { intHideSuburb = 1; }
            int intHideState = 0;
            if (hideState) { intHideState = 1; }
            int intHideCountry = 0;
            if (hideCountry) { intHideCountry = 1; }
            int intHidePostalCode = 0;
            if (hidePostalCode) { intHidePostalCode = 1; }
            int intHideEmail = 0;
            if (hideEmail) { intHideEmail = 1; }
            int intHidePhone = 0;
            if (hidePhone) { intHidePhone = 1; }
            int intHideComment = 0;
            if (hideComment) { intHideComment = 1; }
            int intHideAmount = 0;
            if (hideAmount) { intHideAmount = 1; }
            int intHideLevel = 0;
            if (hideLevel) { intHideLevel = 1; }
            int intHidePhoto = 0;
            if (hidePhoto) { intHidePhoto = 1; }
            int intSubscribeToNews = 0;
            if (subscribeToNews) { intSubscribeToNews = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_ContribProfile (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("IsAnonymous, ");
            sqlCommand.Append("FirstName, ");
            sqlCommand.Append("LastName, ");
            sqlCommand.Append("CompanyName, ");
            sqlCommand.Append("EmailAddress, ");
            sqlCommand.Append("Phone, ");
            sqlCommand.Append("WebSiteUrl, ");
            sqlCommand.Append("ContribComment, ");
            sqlCommand.Append("Bio, ");
            sqlCommand.Append("Address1, ");
            sqlCommand.Append("Address2, ");
            sqlCommand.Append("Suburb, ");
            sqlCommand.Append("City, ");
            sqlCommand.Append("State, ");
            sqlCommand.Append("Country, ");
            sqlCommand.Append("PostalCode, ");
            sqlCommand.Append("TotalContribution, ");
            sqlCommand.Append("FirstContributionUtc, ");
            sqlCommand.Append("LastContributionUtc, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("HideFirstName, ");
            sqlCommand.Append("HideLastName, ");
            sqlCommand.Append("HideCompanyName, ");
            sqlCommand.Append("HideAddress1, ");
            sqlCommand.Append("HideAddress2, ");
            sqlCommand.Append("HideCity, ");
            sqlCommand.Append("HideSuburb, ");
            sqlCommand.Append("HideState, ");
            sqlCommand.Append("HideCountry, ");
            sqlCommand.Append("HidePostalCode, ");
            sqlCommand.Append("HideEmail, ");
            sqlCommand.Append("HidePhone, ");
            sqlCommand.Append("HideComment, ");
            sqlCommand.Append("HideAmount, ");
            sqlCommand.Append("HideLevel, ");
            sqlCommand.Append("LevelGuid, ");
            sqlCommand.Append("HidePhoto, ");
            sqlCommand.Append("SubscribeToNews )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?IsAnonymous, ");
            sqlCommand.Append("?FirstName, ");
            sqlCommand.Append("?LastName, ");
            sqlCommand.Append("?CompanyName, ");
            sqlCommand.Append("?EmailAddress, ");
            sqlCommand.Append("?Phone, ");
            sqlCommand.Append("?WebSiteUrl, ");
            sqlCommand.Append("?ContribComment, ");
            sqlCommand.Append("?Bio, ");
            sqlCommand.Append("?Address1, ");
            sqlCommand.Append("?Address2, ");
            sqlCommand.Append("?Suburb, ");
            sqlCommand.Append("?City, ");
            sqlCommand.Append("?State, ");
            sqlCommand.Append("?Country, ");
            sqlCommand.Append("?PostalCode, ");
            sqlCommand.Append("?TotalContribution, ");
            sqlCommand.Append("?FirstContributionUtc, ");
            sqlCommand.Append("?LastContributionUtc, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?HideFirstName, ");
            sqlCommand.Append("?HideLastName, ");
            sqlCommand.Append("?HideCompanyName, ");
            sqlCommand.Append("?HideAddress1, ");
            sqlCommand.Append("?HideAddress2, ");
            sqlCommand.Append("?HideCity, ");
            sqlCommand.Append("?HideSuburb, ");
            sqlCommand.Append("?HideState, ");
            sqlCommand.Append("?HideCountry, ");
            sqlCommand.Append("?HidePostalCode, ");
            sqlCommand.Append("?HideEmail, ");
            sqlCommand.Append("?HidePhone, ");
            sqlCommand.Append("?HideComment, ");
            sqlCommand.Append("?HideAmount, ");
            sqlCommand.Append("?HideLevel, ");
            sqlCommand.Append("?LevelGuid, ");
            sqlCommand.Append("?HidePhoto, ");
            sqlCommand.Append("?SubscribeToNews )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[43];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?IsAnonymous", MySqlDbType.Int32);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = intIsAnonymous;

            arParams[5] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = firstName;

            arParams[6] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = lastName;

            arParams[7] = new MySqlParameter("?CompanyName", MySqlDbType.VarChar, 255);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = companyName;

            arParams[8] = new MySqlParameter("?EmailAddress", MySqlDbType.VarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = emailAddress;

            arParams[9] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 50);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = phone;

            arParams[10] = new MySqlParameter("?WebSiteUrl", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = webSiteUrl;

            arParams[11] = new MySqlParameter("?ContribComment", MySqlDbType.Text);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = contribComment;

            arParams[12] = new MySqlParameter("?Bio", MySqlDbType.Text);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = bio;

            arParams[13] = new MySqlParameter("?Address1", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = address1;

            arParams[14] = new MySqlParameter("?Address2", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = address2;

            arParams[15] = new MySqlParameter("?Suburb", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = suburb;

            arParams[16] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = city;

            arParams[17] = new MySqlParameter("?State", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = state;

            arParams[18] = new MySqlParameter("?Country", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = country;

            arParams[19] = new MySqlParameter("?PostalCode", MySqlDbType.VarChar, 20);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = postalCode;

            arParams[20] = new MySqlParameter("?TotalContribution", MySqlDbType.Decimal);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = totalContribution;

            arParams[21] = new MySqlParameter("?FirstContributionUtc", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = firstContributionUtc;

            arParams[22] = new MySqlParameter("?LastContributionUtc", MySqlDbType.DateTime);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = lastContributionUtc;

            arParams[23] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = lastModUtc;

            arParams[24] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = lastModBy.ToString();

            arParams[25] = new MySqlParameter("?HideFirstName", MySqlDbType.Int32);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = intHideFirstName;

            arParams[26] = new MySqlParameter("?HideLastName", MySqlDbType.Int32);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = intHideLastName;

            arParams[27] = new MySqlParameter("?HideCompanyName", MySqlDbType.Int32);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intHideCompanyName;

            arParams[28] = new MySqlParameter("?HideAddress1", MySqlDbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intHideAddress1;

            arParams[29] = new MySqlParameter("?HideAddress2", MySqlDbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intHideAddress2;

            arParams[30] = new MySqlParameter("?HideCity", MySqlDbType.Int32);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = intHideCity;

            arParams[31] = new MySqlParameter("?HideSuburb", MySqlDbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intHideSuburb;

            arParams[32] = new MySqlParameter("?HideState", MySqlDbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intHideState;

            arParams[33] = new MySqlParameter("?HideCountry", MySqlDbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intHideCountry;

            arParams[34] = new MySqlParameter("?HidePostalCode", MySqlDbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intHidePostalCode;

            arParams[35] = new MySqlParameter("?HideEmail", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intHideEmail;

            arParams[36] = new MySqlParameter("?HidePhone", MySqlDbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intHidePhone;

            arParams[37] = new MySqlParameter("?HideComment", MySqlDbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intHideComment;

            arParams[38] = new MySqlParameter("?HideAmount", MySqlDbType.Int32);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = intHideAmount;

            arParams[39] = new MySqlParameter("?HideLevel", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intHideLevel;

            arParams[40] = new MySqlParameter("?LevelGuid", MySqlDbType.VarChar, 36);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = levelGuid.ToString();

            arParams[41] = new MySqlParameter("?HidePhoto", MySqlDbType.Int32);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = intHidePhoto;

            arParams[42] = new MySqlParameter("?SubscribeToNews", MySqlDbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intSubscribeToNews;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;

        }

        public static bool Update(
            Guid guid,
            Guid userGuid,
            Guid levelGuid,
            bool isAnonymous,
            string firstName,
            string lastName,
            string companyName,
            string emailAddress,
            string phone,
            string webSiteUrl,
            string contribComment,
            string bio,
            string address1,
            string address2,
            string suburb,
            string city,
            string state,
            string country,
            string postalCode,
            decimal totalContribution,
            DateTime firstContributionUtc,
            DateTime lastContributionUtc,
            DateTime lastModUtc,
            Guid lastModBy,

            bool hideFirstName,
            bool hideLastName,
            bool hideCompanyName,
            bool hideAddress1,
            bool hideAddress2,
            bool hideCity,
            bool hideSuburb,
            bool hideState,
            bool hideCountry,
            bool hidePostalCode,
            bool hideEmail,
            bool hidePhone,
            bool hideComment,

            bool hideAmount,
            bool hideLevel,
            bool hidePhoto,
            bool subscribeToNews)
        {
            #region Bit Conversion

            int intIsAnonymous = 0;
            if (isAnonymous) { intIsAnonymous = 1; }
            int intHideFirstName = 0;
            if (hideFirstName) { intHideFirstName = 1; }
            int intHideLastName = 0;
            if (hideLastName) { intHideLastName = 1; }
            int intHideCompanyName = 0;
            if (hideCompanyName) { intHideCompanyName = 1; }
            int intHideAddress1 = 0;
            if (hideAddress1) { intHideAddress1 = 1; }
            int intHideAddress2 = 0;
            if (hideAddress2) { intHideAddress2 = 1; }
            int intHideCity = 0;
            if (hideCity) { intHideCity = 1; }
            int intHideSuburb = 0;
            if (hideSuburb) { intHideSuburb = 1; }
            int intHideState = 0;
            if (hideState) { intHideState = 1; }
            int intHideCountry = 0;
            if (hideCountry) { intHideCountry = 1; }
            int intHidePostalCode = 0;
            if (hidePostalCode) { intHidePostalCode = 1; }
            int intHideEmail = 0;
            if (hideEmail) { intHideEmail = 1; }
            int intHidePhone = 0;
            if (hidePhone) { intHidePhone = 1; }
            int intHideComment = 0;
            if (hideComment) { intHideComment = 1; }
            int intHideAmount = 0;
            if (hideAmount) { intHideAmount = 1; }
            int intHideLevel = 0;
            if (hideLevel) { intHideLevel = 1; }
            int intHidePhoto = 0;
            if (hidePhoto) { intHidePhoto = 1; }
            int intSubscribeToNews = 0;
            if (subscribeToNews) { intSubscribeToNews = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_ContribProfile ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("IsAnonymous = ?IsAnonymous, ");
            sqlCommand.Append("FirstName = ?FirstName, ");
            sqlCommand.Append("LastName = ?LastName, ");
            sqlCommand.Append("CompanyName = ?CompanyName, ");
            sqlCommand.Append("EmailAddress = ?EmailAddress, ");
            sqlCommand.Append("Phone = ?Phone, ");
            sqlCommand.Append("WebSiteUrl = ?WebSiteUrl, ");
            sqlCommand.Append("ContribComment = ?ContribComment, ");
            sqlCommand.Append("Bio = ?Bio, ");
            sqlCommand.Append("Address1 = ?Address1, ");
            sqlCommand.Append("Address2 = ?Address2, ");
            sqlCommand.Append("Suburb = ?Suburb, ");
            sqlCommand.Append("City = ?City, ");
            sqlCommand.Append("State = ?State, ");
            sqlCommand.Append("Country = ?Country, ");
            sqlCommand.Append("PostalCode = ?PostalCode, ");
            sqlCommand.Append("TotalContribution = ?TotalContribution, ");
            sqlCommand.Append("FirstContributionUtc = ?FirstContributionUtc, ");
            sqlCommand.Append("LastContributionUtc = ?LastContributionUtc, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc, ");
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("HideFirstName = ?HideFirstName, ");
            sqlCommand.Append("HideLastName = ?HideLastName, ");
            sqlCommand.Append("HideCompanyName = ?HideCompanyName, ");
            sqlCommand.Append("HideAddress1 = ?HideAddress1, ");
            sqlCommand.Append("HideAddress2 = ?HideAddress2, ");
            sqlCommand.Append("HideCity = ?HideCity, ");
            sqlCommand.Append("HideSuburb = ?HideSuburb, ");
            sqlCommand.Append("HideState = ?HideState, ");
            sqlCommand.Append("HideCountry = ?HideCountry, ");
            sqlCommand.Append("HidePostalCode = ?HidePostalCode, ");
            sqlCommand.Append("HideEmail = ?HideEmail, ");
            sqlCommand.Append("HidePhone = ?HidePhone, ");
            sqlCommand.Append("HideComment = ?HideComment, ");
            sqlCommand.Append("HideAmount = ?HideAmount, ");
            sqlCommand.Append("HideLevel = ?HideLevel, ");
            sqlCommand.Append("LevelGuid = ?LevelGuid, ");
            sqlCommand.Append("HidePhoto = ?HidePhoto, ");
            sqlCommand.Append("SubscribeToNews = ?SubscribeToNews ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[41];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?IsAnonymous", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = intIsAnonymous;

            arParams[3] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = firstName;

            arParams[4] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = lastName;

            arParams[5] = new MySqlParameter("?CompanyName", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = companyName;

            arParams[6] = new MySqlParameter("?EmailAddress", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = emailAddress;

            arParams[7] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 50);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = phone;

            arParams[8] = new MySqlParameter("?WebSiteUrl", MySqlDbType.VarChar, 255);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = webSiteUrl;

            arParams[9] = new MySqlParameter("?ContribComment", MySqlDbType.Text);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = contribComment;

            arParams[10] = new MySqlParameter("?Bio", MySqlDbType.Text);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = bio;

            arParams[11] = new MySqlParameter("?Address1", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = address1;

            arParams[12] = new MySqlParameter("?Address2", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = address2;

            arParams[13] = new MySqlParameter("?Suburb", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = suburb;

            arParams[14] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = city;

            arParams[15] = new MySqlParameter("?State", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = state;

            arParams[16] = new MySqlParameter("?Country", MySqlDbType.VarChar, 255);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = country;

            arParams[17] = new MySqlParameter("?PostalCode", MySqlDbType.VarChar, 20);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = postalCode;

            arParams[18] = new MySqlParameter("?TotalContribution", MySqlDbType.Decimal);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = totalContribution;

            arParams[19] = new MySqlParameter("?FirstContributionUtc", MySqlDbType.DateTime);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = firstContributionUtc;

            arParams[20] = new MySqlParameter("?LastContributionUtc", MySqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = lastContributionUtc;

            arParams[21] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = lastModUtc;

            arParams[22] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = lastModBy.ToString();

            arParams[23] = new MySqlParameter("?HideFirstName", MySqlDbType.Int32);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = intHideFirstName;

            arParams[24] = new MySqlParameter("?HideLastName", MySqlDbType.Int32);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = intHideLastName;

            arParams[25] = new MySqlParameter("?HideCompanyName", MySqlDbType.Int32);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = intHideCompanyName;

            arParams[26] = new MySqlParameter("?HideAddress1", MySqlDbType.Int32);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = intHideAddress1;

            arParams[27] = new MySqlParameter("?HideAddress2", MySqlDbType.Int32);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = intHideAddress2;

            arParams[28] = new MySqlParameter("?HideCity", MySqlDbType.Int32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = intHideCity;

            arParams[29] = new MySqlParameter("?HideSuburb", MySqlDbType.Int32);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = intHideSuburb;

            arParams[30] = new MySqlParameter("?HideState", MySqlDbType.Int32);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = intHideState;

            arParams[31] = new MySqlParameter("?HideCountry", MySqlDbType.Int32);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = intHideCountry;

            arParams[32] = new MySqlParameter("?HidePostalCode", MySqlDbType.Int32);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = intHidePostalCode;

            arParams[33] = new MySqlParameter("?HideEmail", MySqlDbType.Int32);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = intHideEmail;

            arParams[34] = new MySqlParameter("?HidePhone", MySqlDbType.Int32);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = intHidePhone;

            arParams[35] = new MySqlParameter("?HideComment", MySqlDbType.Int32);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = intHideComment;

            arParams[36] = new MySqlParameter("?HideAmount", MySqlDbType.Int32);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = intHideAmount;

            arParams[37] = new MySqlParameter("?HideLevel", MySqlDbType.Int32);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = intHideLevel;

            arParams[38] = new MySqlParameter("?LevelGuid", MySqlDbType.VarChar, 36);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = levelGuid.ToString();

            arParams[39] = new MySqlParameter("?HidePhoto", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intHidePhoto;

            arParams[40] = new MySqlParameter("?SubscribeToNews", MySqlDbType.Int32);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = intSubscribeToNews;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ContribProfile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ContribProfile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_ContribProfile ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid = ?SiteGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);


        }

        public static IDataReader GetOne(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cp.*, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS UserAvatar, ");
            sqlCommand.Append("cl.Title, ");
            sqlCommand.Append("cl.Description ");

            sqlCommand.Append("FROM	sts_ContribProfile cp ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON cp.UserGuid = u.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN sts_FundContribLevel cl ");
            sqlCommand.Append("ON cp.LevelGuid = cl.Guid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cp.Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);
        }

        public static IDataReader GetByUser(Guid userGuid, Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cp.*, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS UserAvatar, ");
            sqlCommand.Append("cl.Title, ");
            sqlCommand.Append("cl.Description ");

            sqlCommand.Append("FROM	sts_ContribProfile cp ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON cp.UserGuid = u.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN sts_FundContribLevel cl ");
            sqlCommand.Append("ON cp.LevelGuid = cl.Guid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("cp.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND cp.UserGuid = ?UserGuid ");

            sqlCommand.Append("ORDER BY cp.LastContributionUtc DESC ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


        public static IDataReader Search(Guid moduleGuid, string firstName, string lastName, string emailAddress, string companyName, Guid excludeProfileGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cp.*, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS UserAvatar, ");
            sqlCommand.Append("cl.Title, ");
            sqlCommand.Append("cl.Description ");

            sqlCommand.Append("FROM	sts_ContribProfile cp ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON cp.UserGuid = u.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN sts_FundContribLevel cl ");
            sqlCommand.Append("ON cp.LevelGuid = cl.Guid ");

            sqlCommand.Append("WHERE ");

            sqlCommand.Append("cp.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ( (?ExcludeProfileGuid = '00000000-0000-0000-0000-000000000000') OR (cp.Guid <> ?ExcludeProfileGuid) ) ");

            sqlCommand.Append("AND (");

            sqlCommand.Append("(cp.FirstName  LIKE ?FirstName ) ");
            sqlCommand.Append("OR (cp.LastName  LIKE ?LastName ) ");
            sqlCommand.Append("OR (cp.EmailAddress  LIKE ?EmailAddress ) ");
            sqlCommand.Append("OR (cp.CompanyName  LIKE  ?CompanyName ) ");

            sqlCommand.Append(") ");

            sqlCommand.Append("ORDER BY cp.LastName, cp.FirstName ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[6];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?FirstName", MySqlDbType.VarChar, 100);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = "%" + firstName + "%";

            arParams[2] = new MySqlParameter("?LastName", MySqlDbType.VarChar, 100);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = "%" + lastName + "%";

            arParams[3] = new MySqlParameter("?EmailAddress", MySqlDbType.VarChar, 100);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = "%" + emailAddress + "%";

            arParams[4] = new MySqlParameter("?CompanyName", MySqlDbType.VarChar, 255);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = "%" + companyName + "%";

            arParams[5] = new MySqlParameter("?ExcludeProfileGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = excludeProfileGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_ContribProfile ");
            sqlCommand.Append("WHERE ");

            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPageByDateDesc(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cp.*, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS UserAvatar, ");
            sqlCommand.Append("cl.Title, ");
            sqlCommand.Append("cl.Description ");

            sqlCommand.Append("FROM	sts_ContribProfile cp ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON cp.UserGuid = u.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN sts_FundContribLevel cl ");
            sqlCommand.Append("ON cp.LevelGuid = cl.Guid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");

            sqlCommand.Append("ORDER BY cp.LastContributionUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetPageByLastFirstname(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleGuid);

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  cp.*, ");

            sqlCommand.Append("COALESCE(u.UserID, -1) AS UserID, ");
            sqlCommand.Append("COALESCE(u.TotalRevenue, 0) AS UserRevenue, ");
            sqlCommand.Append("COALESCE(u.Trusted, 0) AS Trusted, ");
            sqlCommand.Append("u.AvatarUrl AS UserAvatar, ");
            sqlCommand.Append("cl.Title, ");
            sqlCommand.Append("cl.Description ");

            sqlCommand.Append("FROM	sts_ContribProfile cp ");

            sqlCommand.Append("LEFT OUTER JOIN mp_Users u ");
            sqlCommand.Append("ON cp.UserGuid = u.UserGuid ");

            sqlCommand.Append("LEFT OUTER JOIN sts_FundContribLevel cl ");
            sqlCommand.Append("ON cp.LevelGuid = cl.Guid ");


            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");

            sqlCommand.Append("ORDER BY cp.LastName, cp.FirstName   ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?PageSize", MySqlDbType.Int32);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = pageSize;

            arParams[2] = new MySqlParameter("?OffsetRows", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = pageLowerBound;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
