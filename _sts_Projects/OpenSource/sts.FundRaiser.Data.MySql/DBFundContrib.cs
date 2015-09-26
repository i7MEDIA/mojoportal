// Author:					Joe Audette
// Created:					2013-09-05
// Last Modified:			2013-09-05
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
    public static class DBFundContrib
    {


        public static int Create(
            Guid guid,
            Guid moduleGuid,
            Guid siteGuid,
            Guid userGuid,
            Guid levelGuid,
            Guid orderStatusGuid,
            decimal amount,
            decimal tax,
            decimal processingFee,
            decimal netAmount,
            string paymentMethod,
            Guid currencyGuid,
            string transactionId,
            bool analyticsTracked,
            bool isAnonymous,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerEmail,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            string billingFirstName,
            string billingLastName,
            string billingCompany,
            string billingPhone,
            string billingAddress1,
            string billingAddress2,
            string billingSuburb,
            string billingCity,
            string billingState,
            string billingPostalCode,
            string billingCountry,
            string createdFromIP,
            string lastModifiedFromIP,
            DateTime createdUtc,
            DateTime lastModifiedUtc,

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
            bool subscribeToNews)
        {

            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }
            int intIsAnonymous = 0;
            if (isAnonymous) { intIsAnonymous = 1; }
            //int intBillingSame = 0;
            //if (billingSame) { intBillingSame = 1; }
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
            int intSubscribeToNews = 0;
            if (subscribeToNews) { intSubscribeToNews = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_FundContrib (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("LevelGuid, ");
            sqlCommand.Append("OrderStatusGuid, ");
            sqlCommand.Append("Amount, ");
            sqlCommand.Append("Tax, ");
            sqlCommand.Append("ProcessingFee, ");
            sqlCommand.Append("NetAmount, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("CurrencyGuid, ");
            sqlCommand.Append("TransactionId, ");
            sqlCommand.Append("AnalyticsTracked, ");
            sqlCommand.Append("IsAnonymous, ");
            sqlCommand.Append("CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName, ");
            sqlCommand.Append("CustomerCompany, ");
            sqlCommand.Append("CustomerEmail, ");
            sqlCommand.Append("CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb, ");
            sqlCommand.Append("CustomerCity, ");
            sqlCommand.Append("CustomerState, ");
            sqlCommand.Append("CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingPhone, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("CreatedFromIP, ");
            sqlCommand.Append("LastModifiedFromIP, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModifiedUtc, ");
            sqlCommand.Append("CompletedUtc, ");
            sqlCommand.Append("BillingSame, ");
            sqlCommand.Append("ContributorGuid, ");
            sqlCommand.Append("ContribComment, ");
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
            sqlCommand.Append("SubscribeToNews )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?UserGuid, ");
            sqlCommand.Append("?LevelGuid, ");
            sqlCommand.Append("?OrderStatusGuid, ");
            sqlCommand.Append("?Amount, ");
            sqlCommand.Append("?Tax, ");
            sqlCommand.Append("?ProcessingFee, ");
            sqlCommand.Append("?NetAmount, ");
            sqlCommand.Append("?PaymentMethod, ");
            sqlCommand.Append("?CurrencyGuid, ");
            sqlCommand.Append("?TransactionId, ");
            sqlCommand.Append("?AnalyticsTracked, ");
            sqlCommand.Append("?IsAnonymous, ");
            sqlCommand.Append("?CustomerFirstName, ");
            sqlCommand.Append("?CustomerLastName, ");
            sqlCommand.Append("?CustomerCompany, ");
            sqlCommand.Append("?CustomerEmail, ");
            sqlCommand.Append("?CustomerPhone, ");
            sqlCommand.Append("?CustomerAddress1, ");
            sqlCommand.Append("?CustomerAddress2, ");
            sqlCommand.Append("?CustomerSuburb, ");
            sqlCommand.Append("?CustomerCity, ");
            sqlCommand.Append("?CustomerState, ");
            sqlCommand.Append("?CustomerPostalCode, ");
            sqlCommand.Append("?CustomerCountry, ");
            sqlCommand.Append("?BillingFirstName, ");
            sqlCommand.Append("?BillingLastName, ");
            sqlCommand.Append("?BillingCompany, ");
            sqlCommand.Append("?BillingPhone, ");
            sqlCommand.Append("?BillingAddress1, ");
            sqlCommand.Append("?BillingAddress2, ");
            sqlCommand.Append("?BillingSuburb, ");
            sqlCommand.Append("?BillingCity, ");
            sqlCommand.Append("?BillingState, ");
            sqlCommand.Append("?BillingPostalCode, ");
            sqlCommand.Append("?BillingCountry, ");
            sqlCommand.Append("?CreatedFromIP, ");
            sqlCommand.Append("?LastModifiedFromIP, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModifiedUtc, ");
            sqlCommand.Append("?CompletedUtc, ");
            sqlCommand.Append("?BillingSame, ");
            sqlCommand.Append("?ContributorGuid, ");
            sqlCommand.Append("?ContribComment, ");
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
            sqlCommand.Append("?SubscribeToNews )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[62];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            arParams[2] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = siteGuid.ToString();

            arParams[3] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = userGuid.ToString();

            arParams[4] = new MySqlParameter("?LevelGuid", MySqlDbType.VarChar, 36);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = levelGuid.ToString();

            arParams[5] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = orderStatusGuid.ToString();

            arParams[6] = new MySqlParameter("?Amount", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = amount;

            arParams[7] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = tax;

            arParams[8] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = processingFee;

            arParams[9] = new MySqlParameter("?NetAmount", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = netAmount;

            arParams[10] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = paymentMethod;

            arParams[11] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = currencyGuid.ToString();

            arParams[12] = new MySqlParameter("?TransactionId", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = transactionId;

            arParams[13] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = intAnalyticsTracked;

            arParams[14] = new MySqlParameter("?IsAnonymous", MySqlDbType.Int32);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = intIsAnonymous;

            arParams[15] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerFirstName;

            arParams[16] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerLastName;

            arParams[17] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerCompany;

            arParams[18] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerEmail;

            arParams[19] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerPhone;

            arParams[20] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerAddress1;

            arParams[21] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerAddress2;

            arParams[22] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = customerSuburb;

            arParams[23] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = customerCity;

            arParams[24] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = customerState;

            arParams[25] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = customerPostalCode;

            arParams[26] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = customerCountry;

            arParams[27] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = billingFirstName;

            arParams[28] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingLastName;

            arParams[29] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = billingCompany;

            arParams[30] = new MySqlParameter("?BillingPhone", MySqlDbType.VarChar, 32);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = billingPhone;

            arParams[31] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = billingAddress1;

            arParams[32] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = billingAddress2;

            arParams[33] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = billingSuburb;

            arParams[34] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = billingCity;

            arParams[35] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = billingState;

            arParams[36] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = billingPostalCode;

            arParams[37] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = billingCountry;

            arParams[38] = new MySqlParameter("?CreatedFromIP", MySqlDbType.VarChar, 255);
            arParams[38].Direction = ParameterDirection.Input;
            arParams[38].Value = createdFromIP;

            arParams[39] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = lastModifiedFromIP;

            arParams[40] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = createdUtc;

            arParams[41] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = lastModifiedUtc;

            arParams[42] = new MySqlParameter("?CompletedUtc", MySqlDbType.DateTime);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = DBNull.Value;

            arParams[43] = new MySqlParameter("?BillingSame", MySqlDbType.Int32);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = 0;

            arParams[44] = new MySqlParameter("?ContributorGuid", MySqlDbType.VarChar, 36);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = Guid.Empty.ToString();

            arParams[45] = new MySqlParameter("?ContribComment", MySqlDbType.Text);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = string.Empty;

            arParams[46] = new MySqlParameter("?HideFirstName", MySqlDbType.Int32);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = intHideFirstName;

            arParams[47] = new MySqlParameter("?HideLastName", MySqlDbType.Int32);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = intHideLastName;

            arParams[48] = new MySqlParameter("?HideCompanyName", MySqlDbType.Int32);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = intHideCompanyName;

            arParams[49] = new MySqlParameter("?HideAddress1", MySqlDbType.Int32);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = intHideAddress1;

            arParams[50] = new MySqlParameter("?HideAddress2", MySqlDbType.Int32);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = intHideAddress2;

            arParams[51] = new MySqlParameter("?HideCity", MySqlDbType.Int32);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = intHideCity;

            arParams[52] = new MySqlParameter("?HideSuburb", MySqlDbType.Int32);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = intHideSuburb;

            arParams[53] = new MySqlParameter("?HideState", MySqlDbType.Int32);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = intHideState;

            arParams[54] = new MySqlParameter("?HideCountry", MySqlDbType.Int32);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = intHideCountry;

            arParams[55] = new MySqlParameter("?HidePostalCode", MySqlDbType.Int32);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = intHidePostalCode;

            arParams[56] = new MySqlParameter("?HideEmail", MySqlDbType.Int32);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = intHideEmail;

            arParams[57] = new MySqlParameter("?HidePhone", MySqlDbType.Int32);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = intHidePhone;

            arParams[58] = new MySqlParameter("?HideComment", MySqlDbType.Int32);
            arParams[58].Direction = ParameterDirection.Input;
            arParams[58].Value = intHideComment;

            arParams[59] = new MySqlParameter("?HideAmount", MySqlDbType.Int32);
            arParams[59].Direction = ParameterDirection.Input;
            arParams[59].Value = intHideAmount;

            arParams[60] = new MySqlParameter("?HideLevel", MySqlDbType.Int32);
            arParams[60].Direction = ParameterDirection.Input;
            arParams[60].Value = intHideLevel;

            arParams[61] = new MySqlParameter("?SubscribeToNews", MySqlDbType.Int32);
            arParams[61].Direction = ParameterDirection.Input;
            arParams[61].Value = intSubscribeToNews;

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
            Guid orderStatusGuid,
            Guid contributorGuid,
            decimal amount,
            decimal tax,
            decimal processingFee,
            decimal netAmount,
            string paymentMethod,
            Guid currencyGuid,
            string transactionId,
            bool analyticsTracked,
            bool isAnonymous,
            string contribComment,
            string customerFirstName,
            string customerLastName,
            string customerCompany,
            string customerEmail,
            string customerPhone,
            string customerAddress1,
            string customerAddress2,
            string customerSuburb,
            string customerCity,
            string customerState,
            string customerPostalCode,
            string customerCountry,
            bool billingSame,
            string billingFirstName,
            string billingLastName,
            string billingCompany,
            string billingPhone,
            string billingAddress1,
            string billingAddress2,
            string billingSuburb,
            string billingCity,
            string billingState,
            string billingPostalCode,
            string billingCountry,
            string lastModifiedFromIP,
            DateTime lastModifiedUtc,
            DateTime completedUtc,

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
            bool subscribeToNews)
        {
            #region Bit Conversion

            int intAnalyticsTracked = 0;
            if (analyticsTracked) { intAnalyticsTracked = 1; }
            int intIsAnonymous = 0;
            if (isAnonymous) { intIsAnonymous = 1; }
            int intBillingSame = 0;
            if (billingSame) { intBillingSame = 1; }
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
            int intSubscribeToNews = 0;
            if (subscribeToNews) { intSubscribeToNews = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundContrib ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("UserGuid = ?UserGuid, ");
            sqlCommand.Append("LevelGuid = ?LevelGuid, ");
            sqlCommand.Append("OrderStatusGuid = ?OrderStatusGuid, ");
            sqlCommand.Append("Amount = ?Amount, ");
            sqlCommand.Append("Tax = ?Tax, ");
            sqlCommand.Append("ProcessingFee = ?ProcessingFee, ");
            sqlCommand.Append("NetAmount = ?NetAmount, ");
            sqlCommand.Append("PaymentMethod = ?PaymentMethod, ");
            sqlCommand.Append("CurrencyGuid = ?CurrencyGuid, ");
            sqlCommand.Append("TransactionId = ?TransactionId, ");
            sqlCommand.Append("AnalyticsTracked = ?AnalyticsTracked, ");
            sqlCommand.Append("IsAnonymous = ?IsAnonymous, ");
            sqlCommand.Append("CustomerFirstName = ?CustomerFirstName, ");
            sqlCommand.Append("CustomerLastName = ?CustomerLastName, ");
            sqlCommand.Append("CustomerCompany = ?CustomerCompany, ");
            sqlCommand.Append("CustomerEmail = ?CustomerEmail, ");
            sqlCommand.Append("CustomerPhone = ?CustomerPhone, ");
            sqlCommand.Append("CustomerAddress1 = ?CustomerAddress1, ");
            sqlCommand.Append("CustomerAddress2 = ?CustomerAddress2, ");
            sqlCommand.Append("CustomerSuburb = ?CustomerSuburb, ");
            sqlCommand.Append("CustomerCity = ?CustomerCity, ");
            sqlCommand.Append("CustomerState = ?CustomerState, ");
            sqlCommand.Append("CustomerPostalCode = ?CustomerPostalCode, ");
            sqlCommand.Append("CustomerCountry = ?CustomerCountry, ");
            sqlCommand.Append("BillingFirstName = ?BillingFirstName, ");
            sqlCommand.Append("BillingLastName = ?BillingLastName, ");
            sqlCommand.Append("BillingCompany = ?BillingCompany, ");
            sqlCommand.Append("BillingPhone = ?BillingPhone, ");
            sqlCommand.Append("BillingAddress1 = ?BillingAddress1, ");
            sqlCommand.Append("BillingAddress2 = ?BillingAddress2, ");
            sqlCommand.Append("BillingSuburb = ?BillingSuburb, ");
            sqlCommand.Append("BillingCity = ?BillingCity, ");
            sqlCommand.Append("BillingState = ?BillingState, ");
            sqlCommand.Append("BillingPostalCode = ?BillingPostalCode, ");
            sqlCommand.Append("BillingCountry = ?BillingCountry, ");
  
            sqlCommand.Append("LastModifiedFromIP = ?LastModifiedFromIP, ");
           
            sqlCommand.Append("LastModifiedUtc = ?LastModifiedUtc, ");
            sqlCommand.Append("CompletedUtc = ?CompletedUtc, ");
            sqlCommand.Append("BillingSame = ?BillingSame, ");
            sqlCommand.Append("ContributorGuid = ?ContributorGuid, ");
            sqlCommand.Append("ContribComment = ?ContribComment, ");
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
            sqlCommand.Append("SubscribeToNews = ?SubscribeToNews ");

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[58];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = userGuid.ToString();

            arParams[2] = new MySqlParameter("?LevelGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = levelGuid.ToString();

            arParams[3] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = orderStatusGuid.ToString();

            arParams[4] = new MySqlParameter("?Amount", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = amount;

            arParams[5] = new MySqlParameter("?Tax", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = tax;

            arParams[6] = new MySqlParameter("?ProcessingFee", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = processingFee;

            arParams[7] = new MySqlParameter("?NetAmount", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = netAmount;

            arParams[8] = new MySqlParameter("?PaymentMethod", MySqlDbType.VarChar, 50);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = paymentMethod;

            arParams[9] = new MySqlParameter("?CurrencyGuid", MySqlDbType.VarChar, 36);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = currencyGuid.ToString();

            arParams[10] = new MySqlParameter("?TransactionId", MySqlDbType.VarChar, 255);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = transactionId;

            arParams[11] = new MySqlParameter("?AnalyticsTracked", MySqlDbType.Int32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = intAnalyticsTracked;

            arParams[12] = new MySqlParameter("?IsAnonymous", MySqlDbType.Int32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = intIsAnonymous;

            arParams[13] = new MySqlParameter("?CustomerFirstName", MySqlDbType.VarChar, 100);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = customerFirstName;

            arParams[14] = new MySqlParameter("?CustomerLastName", MySqlDbType.VarChar, 100);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = customerLastName;

            arParams[15] = new MySqlParameter("?CustomerCompany", MySqlDbType.VarChar, 255);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = customerCompany;

            arParams[16] = new MySqlParameter("?CustomerEmail", MySqlDbType.VarChar, 100);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = customerEmail;

            arParams[17] = new MySqlParameter("?CustomerPhone", MySqlDbType.VarChar, 32);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = customerPhone;

            arParams[18] = new MySqlParameter("?CustomerAddress1", MySqlDbType.VarChar, 255);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = customerAddress1;

            arParams[19] = new MySqlParameter("?CustomerAddress2", MySqlDbType.VarChar, 255);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = customerAddress2;

            arParams[20] = new MySqlParameter("?CustomerSuburb", MySqlDbType.VarChar, 255);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = customerSuburb;

            arParams[21] = new MySqlParameter("?CustomerCity", MySqlDbType.VarChar, 255);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = customerCity;

            arParams[22] = new MySqlParameter("?CustomerState", MySqlDbType.VarChar, 255);
            arParams[22].Direction = ParameterDirection.Input;
            arParams[22].Value = customerState;

            arParams[23] = new MySqlParameter("?CustomerPostalCode", MySqlDbType.VarChar, 20);
            arParams[23].Direction = ParameterDirection.Input;
            arParams[23].Value = customerPostalCode;

            arParams[24] = new MySqlParameter("?CustomerCountry", MySqlDbType.VarChar, 255);
            arParams[24].Direction = ParameterDirection.Input;
            arParams[24].Value = customerCountry;

            arParams[25] = new MySqlParameter("?BillingFirstName", MySqlDbType.VarChar, 100);
            arParams[25].Direction = ParameterDirection.Input;
            arParams[25].Value = billingFirstName;

            arParams[26] = new MySqlParameter("?BillingLastName", MySqlDbType.VarChar, 100);
            arParams[26].Direction = ParameterDirection.Input;
            arParams[26].Value = billingLastName;

            arParams[27] = new MySqlParameter("?BillingCompany", MySqlDbType.VarChar, 255);
            arParams[27].Direction = ParameterDirection.Input;
            arParams[27].Value = billingCompany;

            arParams[28] = new MySqlParameter("?BillingPhone", MySqlDbType.VarChar, 32);
            arParams[28].Direction = ParameterDirection.Input;
            arParams[28].Value = billingPhone;

            arParams[29] = new MySqlParameter("?BillingAddress1", MySqlDbType.VarChar, 255);
            arParams[29].Direction = ParameterDirection.Input;
            arParams[29].Value = billingAddress1;

            arParams[30] = new MySqlParameter("?BillingAddress2", MySqlDbType.VarChar, 255);
            arParams[30].Direction = ParameterDirection.Input;
            arParams[30].Value = billingAddress2;

            arParams[31] = new MySqlParameter("?BillingSuburb", MySqlDbType.VarChar, 255);
            arParams[31].Direction = ParameterDirection.Input;
            arParams[31].Value = billingSuburb;

            arParams[32] = new MySqlParameter("?BillingCity", MySqlDbType.VarChar, 255);
            arParams[32].Direction = ParameterDirection.Input;
            arParams[32].Value = billingCity;

            arParams[33] = new MySqlParameter("?BillingState", MySqlDbType.VarChar, 255);
            arParams[33].Direction = ParameterDirection.Input;
            arParams[33].Value = billingState;

            arParams[34] = new MySqlParameter("?BillingPostalCode", MySqlDbType.VarChar, 20);
            arParams[34].Direction = ParameterDirection.Input;
            arParams[34].Value = billingPostalCode;

            arParams[35] = new MySqlParameter("?BillingCountry", MySqlDbType.VarChar, 255);
            arParams[35].Direction = ParameterDirection.Input;
            arParams[35].Value = billingCountry;

            arParams[36] = new MySqlParameter("?LastModifiedFromIP", MySqlDbType.VarChar, 255);
            arParams[36].Direction = ParameterDirection.Input;
            arParams[36].Value = lastModifiedFromIP;

            arParams[37] = new MySqlParameter("?LastModifiedUtc", MySqlDbType.DateTime);
            arParams[37].Direction = ParameterDirection.Input;
            arParams[37].Value = lastModifiedUtc;

            arParams[38] = new MySqlParameter("?CompletedUtc", MySqlDbType.DateTime);
            arParams[38].Direction = ParameterDirection.Input;

            if (completedUtc != DateTime.MaxValue)
            {
                arParams[38].Value = completedUtc;
            }
            else
            {
                arParams[38].Value = DBNull.Value;
            }

            arParams[39] = new MySqlParameter("?BillingSame", MySqlDbType.Int32);
            arParams[39].Direction = ParameterDirection.Input;
            arParams[39].Value = intBillingSame;

            arParams[40] = new MySqlParameter("?ContributorGuid", MySqlDbType.VarChar, 36);
            arParams[40].Direction = ParameterDirection.Input;
            arParams[40].Value = contributorGuid.ToString();

            arParams[41] = new MySqlParameter("?ContribComment", MySqlDbType.Text);
            arParams[41].Direction = ParameterDirection.Input;
            arParams[41].Value = contribComment;

            arParams[42] = new MySqlParameter("?HideFirstName", MySqlDbType.Int32);
            arParams[42].Direction = ParameterDirection.Input;
            arParams[42].Value = intHideFirstName;

            arParams[43] = new MySqlParameter("?HideLastName", MySqlDbType.Int32);
            arParams[43].Direction = ParameterDirection.Input;
            arParams[43].Value = intHideLastName;

            arParams[44] = new MySqlParameter("?HideCompanyName", MySqlDbType.Int32);
            arParams[44].Direction = ParameterDirection.Input;
            arParams[44].Value = intHideCompanyName;

            arParams[45] = new MySqlParameter("?HideAddress1", MySqlDbType.Int32);
            arParams[45].Direction = ParameterDirection.Input;
            arParams[45].Value = intHideAddress1;

            arParams[46] = new MySqlParameter("?HideAddress2", MySqlDbType.Int32);
            arParams[46].Direction = ParameterDirection.Input;
            arParams[46].Value = intHideAddress2;

            arParams[47] = new MySqlParameter("?HideCity", MySqlDbType.Int32);
            arParams[47].Direction = ParameterDirection.Input;
            arParams[47].Value = intHideCity;

            arParams[48] = new MySqlParameter("?HideSuburb", MySqlDbType.Int32);
            arParams[48].Direction = ParameterDirection.Input;
            arParams[48].Value = intHideSuburb;

            arParams[49] = new MySqlParameter("?HideState", MySqlDbType.Int32);
            arParams[49].Direction = ParameterDirection.Input;
            arParams[49].Value = intHideState;

            arParams[50] = new MySqlParameter("?HideCountry", MySqlDbType.Int32);
            arParams[50].Direction = ParameterDirection.Input;
            arParams[50].Value = intHideCountry;

            arParams[51] = new MySqlParameter("?HidePostalCode", MySqlDbType.Int32);
            arParams[51].Direction = ParameterDirection.Input;
            arParams[51].Value = intHidePostalCode;

            arParams[52] = new MySqlParameter("?HideEmail", MySqlDbType.Int32);
            arParams[52].Direction = ParameterDirection.Input;
            arParams[52].Value = intHideEmail;

            arParams[53] = new MySqlParameter("?HidePhone", MySqlDbType.Int32);
            arParams[53].Direction = ParameterDirection.Input;
            arParams[53].Value = intHidePhone;

            arParams[54] = new MySqlParameter("?HideComment", MySqlDbType.Int32);
            arParams[54].Direction = ParameterDirection.Input;
            arParams[54].Value = intHideComment;

            arParams[55] = new MySqlParameter("?HideAmount", MySqlDbType.Int32);
            arParams[55].Direction = ParameterDirection.Input;
            arParams[55].Value = intHideAmount;

            arParams[56] = new MySqlParameter("?HideLevel", MySqlDbType.Int32);
            arParams[56].Direction = ParameterDirection.Input;
            arParams[56].Value = intHideLevel;

            arParams[57] = new MySqlParameter("?SubscribeToNews", MySqlDbType.Int32);
            arParams[57].Direction = ParameterDirection.Input;
            arParams[57].Value = intSubscribeToNews;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid, Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContrib ");
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

        public static bool DeleteByModule(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContrib ");
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

        public static bool DeleteByStatus(
           Guid moduleGuid,
           Guid orderStatusGuid,
           DateTime beginDateUtc,
           DateTime endDateUtc)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ( (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111') OR (OrderStatusGuid = ?OrderStatusGuid) ) ");
            sqlCommand.Append("AND CreatedUtc >= ?BeginDateUtc ");
            sqlCommand.Append("AND CreatedUtc <= ?EndDateUtc ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginDateUtc;

            arParams[3] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = endDateUtc;


            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return (rowsAffected > 0);


        }

        public static IDataReader GetOne(Guid guid, Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();


            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static IDataReader GetByContributor(Guid contributorGuid, Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContributorGuid = ?ContributorGuid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND CompletedUtc IS NOT NULL ");
            sqlCommand.Append("ORDER BY CompletedUtc ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ContributorGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contributorGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();


            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static IDataReader GetMostRecentByUser(Guid userGuid, Guid moduleGuid, Guid orderStatusGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("UserGuid = ?UserGuid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ( (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111') OR (OrderStatusGuid = ?OrderStatusGuid) ) ");
            sqlCommand.Append("ORDER BY CreatedUtc DESC ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[3];

            arParams[0] = new MySqlParameter("?UserGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = userGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();

            arParams[2] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = orderStatusGuid.ToString();


            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static decimal GetSumOfContributions(
            Guid contributorGuid,
            Guid moduleGuid
            )
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT COALESCE(SUM(Amount), 0) ");
            sqlCommand.Append("FROM	sts_FundContrib ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ContributorGuid = ?ContributorGuid ");
            sqlCommand.Append(" AND ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ContributorGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = contributorGuid.ToString();

            arParams[1] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = moduleGuid.ToString();


            return Convert.ToDecimal(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));


        }


        public static int GetCount(
            Guid moduleGuid,
            Guid orderStatusGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_FundContrib ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ( (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111') OR (OrderStatusGuid = ?OrderStatusGuid) ) ");
            sqlCommand.Append("AND CreatedUtc >= ?BeginDateUtc ");
            sqlCommand.Append("AND CreatedUtc <= ?EndDateUtc ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginDateUtc;

            arParams[3] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = endDateUtc;

            return Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

        }

        public static IDataReader GetPage(
            Guid moduleGuid,
            Guid orderStatusGuid,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCount(moduleGuid, orderStatusGuid, beginDateUtc, endDateUtc);

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
            sqlCommand.Append("SELECT	* ");

            sqlCommand.Append("FROM	sts_FundContrib  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND ( (?OrderStatusGuid = '11111111-1111-1111-1111-111111111111') OR (OrderStatusGuid = ?OrderStatusGuid) ) ");
            sqlCommand.Append("AND CreatedUtc >= ?BeginDateUtc ");
            sqlCommand.Append("AND CreatedUtc <= ?EndDateUtc ");

            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
            //sqlCommand.Append("  ");
            sqlCommand.Append("LIMIT ?PageSize ");

            if (pageNumber > 1)
            {
                sqlCommand.Append("OFFSET ?OffsetRows ");
            }

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[4];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderStatusGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderStatusGuid.ToString();

            arParams[2] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = beginDateUtc;

            arParams[3] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = endDateUtc;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static int GetCartCount(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_FundContrib ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");

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

        public static IDataReader GetCartPage(
            Guid moduleGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            int pageLowerBound = (pageSize * pageNumber) - pageSize;
            totalPages = 1;
            int totalRows = GetCartCount(moduleGuid);

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
            sqlCommand.Append("SELECT	* ");
            sqlCommand.Append("FROM	sts_FundContrib  ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND OrderStatusGuid = '00000000-0000-0000-0000-000000000000' ");


            sqlCommand.Append("ORDER BY CreatedUtc DESC  ");
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
