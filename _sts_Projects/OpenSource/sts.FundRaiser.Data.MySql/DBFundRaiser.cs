// Author:					
// Created:					2013-09-07
// Last Modified:			2013-09-22
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
    public static class DBFundRaiser
    {
        public static int Create(
            Guid moduleGuid,
            Guid siteGuid,
            string title,
            string summary,
            string description,
            decimal revenueGoal,
            decimal stretchGoal,
            decimal revenueMaxAllowed,
            decimal minContribAmt,
            decimal maxContribAmt,
            bool allowUserAmt,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            Guid createdBy,
            DateTime createdUtc)
        {
            #region Bit Conversion

            int intAllowUserAmt = 0;
            if (allowUserAmt) { intAllowUserAmt = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO sts_FundRaiser (");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("Title, ");
            sqlCommand.Append("Summary, ");
            sqlCommand.Append("Description, ");
            sqlCommand.Append("RevenueGoal, ");
            sqlCommand.Append("StretchGoal, ");
            sqlCommand.Append("RevenueMaxAllowed, ");
            sqlCommand.Append("MinContribAmt, ");
            sqlCommand.Append("MaxContribAmt, ");
            sqlCommand.Append("AllowUserAmt, ");
            sqlCommand.Append("BeginDateUtc, ");
            sqlCommand.Append("EndDateUtc, ");
            sqlCommand.Append("CreatedBy, ");
            sqlCommand.Append("CreatedUtc, ");
            sqlCommand.Append("LastModBy, ");
            sqlCommand.Append("LastModUtc, ");
            sqlCommand.Append("TotalRevenue, ");
            sqlCommand.Append("TotalContributors, ");
            sqlCommand.Append("TotalComments, ");
            sqlCommand.Append("TotalUpdates )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?ModuleGuid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?Title, ");
            sqlCommand.Append("?Summary, ");
            sqlCommand.Append("?Description, ");
            sqlCommand.Append("?RevenueGoal, ");
            sqlCommand.Append("?StretchGoal, ");
            sqlCommand.Append("?RevenueMaxAllowed, ");
            sqlCommand.Append("?MinContribAmt, ");
            sqlCommand.Append("?MaxContribAmt, ");
            sqlCommand.Append("?AllowUserAmt, ");
            sqlCommand.Append("?BeginDateUtc, ");
            sqlCommand.Append("?EndDateUtc, ");
            sqlCommand.Append("?CreatedBy, ");
            sqlCommand.Append("?CreatedUtc, ");
            sqlCommand.Append("?LastModBy, ");
            sqlCommand.Append("?LastModUtc, ");
            sqlCommand.Append("?TotalRevenue, ");
            sqlCommand.Append("?TotalContributors, ");
            sqlCommand.Append("?TotalComments, ");
            sqlCommand.Append("?TotalUpdates )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[21];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = title;

            arParams[3] = new MySqlParameter("?Summary", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = summary;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?RevenueGoal", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = revenueGoal;

            arParams[6] = new MySqlParameter("?StretchGoal", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = stretchGoal;

            arParams[7] = new MySqlParameter("?RevenueMaxAllowed", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = revenueMaxAllowed;

            arParams[8] = new MySqlParameter("?MinContribAmt", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = minContribAmt;

            arParams[9] = new MySqlParameter("?MaxContribAmt", MySqlDbType.Decimal);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = maxContribAmt;

            arParams[10] = new MySqlParameter("?AllowUserAmt", MySqlDbType.Int32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = intAllowUserAmt;

            arParams[11] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = beginDateUtc;

            arParams[12] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[12].Direction = ParameterDirection.Input;

            if (endDateUtc == DateTime.MaxValue)
            {
                arParams[12].Value = DBNull.Value;
            }
            else
            {
                arParams[12].Value = endDateUtc;
            }

            arParams[13] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = createdBy.ToString();

            arParams[14] = new MySqlParameter("?CreatedUtc", MySqlDbType.DateTime);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = createdUtc;

            arParams[15] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = createdBy.ToString();

            arParams[16] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = createdUtc;

            arParams[17] = new MySqlParameter("?TotalRevenue", MySqlDbType.Decimal);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = 0;

            arParams[18] = new MySqlParameter("?TotalContributors", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = 0;

            arParams[19] = new MySqlParameter("?TotalComments", MySqlDbType.Int32);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = 0;

            arParams[20] = new MySqlParameter("?TotalUpdates", MySqlDbType.Int32);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = 0;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);
            return rowsAffected;
        }

        public static bool Update(
            Guid moduleGuid,
            string title,
            string summary,
            string description,
            decimal revenueGoal,
            decimal stretchGoal,
            decimal revenueMaxAllowed,
            decimal minContribAmt,
            decimal maxContribAmt,
            bool allowUserAmt,
            DateTime beginDateUtc,
            DateTime endDateUtc,
            Guid lastModBy,
            DateTime lastModUtc)
        {

            #region Bit Conversion

            int intAllowUserAmt = 0;
            if (allowUserAmt) { intAllowUserAmt = 1; }

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundRaiser ");
            sqlCommand.Append("SET  ");
  
            sqlCommand.Append("Title = ?Title, ");
            sqlCommand.Append("Summary = ?Summary, ");
            sqlCommand.Append("Description = ?Description, ");
            sqlCommand.Append("RevenueGoal = ?RevenueGoal, ");
            sqlCommand.Append("StretchGoal = ?StretchGoal, ");
            sqlCommand.Append("RevenueMaxAllowed = ?RevenueMaxAllowed, ");
            sqlCommand.Append("MinContribAmt = ?MinContribAmt, ");
            sqlCommand.Append("MaxContribAmt = ?MaxContribAmt, ");
            sqlCommand.Append("AllowUserAmt = ?AllowUserAmt, ");
            sqlCommand.Append("BeginDateUtc = ?BeginDateUtc, ");

            sqlCommand.Append("EndDateUtc = ?EndDateUtc, ");
         
            sqlCommand.Append("LastModBy = ?LastModBy, ");
            sqlCommand.Append("LastModUtc = ?LastModUtc ");
            

            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[14];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?Title", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = title;

            arParams[2] = new MySqlParameter("?Summary", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = summary;

            arParams[3] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = description;

            arParams[4] = new MySqlParameter("?RevenueGoal", MySqlDbType.Decimal);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = revenueGoal;

            arParams[5] = new MySqlParameter("?StretchGoal", MySqlDbType.Decimal);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = stretchGoal;

            arParams[6] = new MySqlParameter("?RevenueMaxAllowed", MySqlDbType.Decimal);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = revenueMaxAllowed;

            arParams[7] = new MySqlParameter("?MinContribAmt", MySqlDbType.Decimal);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = minContribAmt;

            arParams[8] = new MySqlParameter("?MaxContribAmt", MySqlDbType.Decimal);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = maxContribAmt;

            arParams[9] = new MySqlParameter("?AllowUserAmt", MySqlDbType.Int32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = intAllowUserAmt;

            arParams[10] = new MySqlParameter("?BeginDateUtc", MySqlDbType.DateTime);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = beginDateUtc;

            arParams[11] = new MySqlParameter("?EndDateUtc", MySqlDbType.DateTime);
            arParams[11].Direction = ParameterDirection.Input;
            if (endDateUtc == DateTime.MaxValue)
            {
                arParams[11].Value = DBNull.Value;
            }
            else
            {
                arParams[11].Value = endDateUtc;
            }

            arParams[12] = new MySqlParameter("?LastModBy", MySqlDbType.VarChar, 36);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = lastModBy.ToString();

            arParams[13] = new MySqlParameter("?LastModUtc", MySqlDbType.DateTime);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = lastModUtc;

           
            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);


        }

        public static bool UpdateStats(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundRaiser ");
            sqlCommand.Append("SET TotalRevenue = COALESCE((SELECT SUM(Amount) FROM sts_FundContrib WHERE ModuleGuid = sts_FundRaiser.ModuleGuid AND CompletedUtc IS NOT NULL), 0), ");
            sqlCommand.Append("TotalContributors = COALESCE((SELECT COUNT(*) FROM sts_ContribProfile WHERE ModuleGuid = sts_FundRaiser.ModuleGuid), 0) ");


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


        public static bool UpdateUpdateStats(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundRaiser ");
            sqlCommand.Append("SET TotalUpdates = COALESCE((SELECT COUNT(*) FROM sts_FundRaiserUpdates WHERE  ModuleGuid = sts_FundRaiser.ModuleGuid), 0) ");
           
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

        public static bool UpdateCommentStats(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE sts_FundRaiser ");
            sqlCommand.Append("TotalComments = COALESCE((SELECT COUNT(*) FROM mp_Comments WHERE FeatureGuid = '9214f31b-610a-41a8-a67d-28d2c3d605db' AND  ModuleGuid = sts_FundRaiser.ModuleGuid), 0) ");

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

        public static bool Delete(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM sts_FundRaiser ");
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
            sqlCommand.Append("DELETE FROM sts_FundRaiser ");
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

        public static IDataReader GetOne(Guid moduleGuid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  * ");
            sqlCommand.Append("FROM	sts_FundRaiser ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }

        public static bool Exists(Guid moduleGuid)
        {

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  Count(*) ");
            sqlCommand.Append("FROM	sts_FundRaiser ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            int count = Convert.ToInt32(MySqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams));

            return (count > 0);


        }

        public static void EnsureSalesReportData(Guid moduleGuid, int pageId, int moduleId, string orderDescription)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReport ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("UserGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("ModuleTitle, ");
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("ItemGuid, ");
            sqlCommand.Append("ItemName, ");
            sqlCommand.Append("Quantity, ");
            sqlCommand.Append("Price, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("IPAddress, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.Guid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("s.ModuleGuid, ");
            sqlCommand.Append("m.ModuleTitle, ");
            sqlCommand.Append("s.Guid, ");
            sqlCommand.Append("s.LevelGuid AS ItemGuid, ");
            sqlCommand.Append("?OrderDescription AS ItemName, ");
            sqlCommand.Append("1, ");
            sqlCommand.Append("s.Amount AS Price, ");
            sqlCommand.Append("s.Amount AS SubTotal, ");
            sqlCommand.Append("s.CompletedUtc AS OrderDateUtc, ");
            sqlCommand.Append("s.PaymentMethod, ");
            sqlCommand.Append("'' AS IPAddress, ");

            sqlCommand.Append("CONCAT('/FundRaiser/EditContribution.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;c=',s.Guid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/FundRaiser/Contributors.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;c=' , s.Guid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_FundContrib s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleGuid = m.Guid ");


            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReport cr ");
            sqlCommand.Append("ON cr.RowGuid = s.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("m.Guid = ?ModuleGuid ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("s.Amount > 0 ");
            sqlCommand.Append("AND s.OrderStatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' ");
            sqlCommand.Append("AND s.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' ");
            sqlCommand.Append("AND s.CompletedUtc IS NOT NULL ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[2];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            arParams[1] = new MySqlParameter("?OrderDescription", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = orderDescription;

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

        public static void EnsureSalesReportOrderData(Guid moduleGuid, int pageId, int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO mp_CommerceReportOrders ");
            sqlCommand.Append("( ");
            sqlCommand.Append("RowGuid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("FeatureGuid, ");
            sqlCommand.Append("ModuleGuid, ");
            sqlCommand.Append("UserGuid, "); ;
            sqlCommand.Append("OrderGuid, ");
            sqlCommand.Append("BillingFirstName, ");
            sqlCommand.Append("BillingLastName, ");
            sqlCommand.Append("BillingCompany, ");
            sqlCommand.Append("BillingAddress1, ");
            sqlCommand.Append("BillingAddress2, ");
            sqlCommand.Append("BillingSuburb, ");
            sqlCommand.Append("BillingCity, ");
            sqlCommand.Append("BillingPostalCode, ");
            sqlCommand.Append("BillingState, ");
            sqlCommand.Append("BillingCountry, ");
            sqlCommand.Append("PaymentMethod, ");
            sqlCommand.Append("SubTotal, ");
            sqlCommand.Append("TaxTotal, ");
            sqlCommand.Append("ShippingTotal, ");
            sqlCommand.Append("OrderTotal, ");
            sqlCommand.Append("OrderDateUtc, ");
            sqlCommand.Append("AdminOrderLink, ");
            sqlCommand.Append("UserOrderLink, ");
            sqlCommand.Append("RowCreatedUtc ");
            sqlCommand.Append(") ");

            sqlCommand.Append("SELECT ");
            sqlCommand.Append("s.Guid AS RowGuid, ");
            sqlCommand.Append("s.SiteGuid, ");
            sqlCommand.Append("m.FeatureGuid, ");
            sqlCommand.Append("s.ModuleGuid, ");
            sqlCommand.Append("s.UserGuid, ");
            sqlCommand.Append("s.Guid, ");
            sqlCommand.Append("s.CustomerFirstName, ");
            sqlCommand.Append("s.CustomerLastName, ");
            sqlCommand.Append("s.CustomerCompany, ");
            sqlCommand.Append("s.CustomerAddress1, ");
            sqlCommand.Append("s.CustomerAddress2, ");
            sqlCommand.Append("s.CustomerSuburb, ");
            sqlCommand.Append("s.CustomerCity, ");
            sqlCommand.Append("s.CustomerPostalCode, ");
            sqlCommand.Append("s.CustomerState, ");
            sqlCommand.Append("s.CustomerCountry, ");
            sqlCommand.Append("s.PaymentMethod, ");
            sqlCommand.Append("s.Amount, ");
            sqlCommand.Append("s.Tax, ");
            sqlCommand.Append("0, ");
            sqlCommand.Append("s.Amount, ");
            sqlCommand.Append("s.CompletedUtc, ");

            sqlCommand.Append("CONCAT('/FundRaiser/EditContribution.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;c=',s.Guid) AS AdminOrderLink, ");

            sqlCommand.Append("CONCAT('/FundRaiser/Contributors.aspx?pageid=' ");
            sqlCommand.Append(", " + pageId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(",'&amp;mid=', " + moduleId.ToString(CultureInfo.InvariantCulture));
            sqlCommand.Append(", '&amp;c=' , s.Guid) AS UserOrderLink, ");

            sqlCommand.Append("now() As RowCreatedUtc ");

            sqlCommand.Append("FROM ");
            sqlCommand.Append("sts_FundContrib s ");

            sqlCommand.Append("JOIN ");
            sqlCommand.Append("mp_Modules m ");
            sqlCommand.Append("ON s.ModuleGuid = m.Guid ");



            sqlCommand.Append("LEFT OUTER JOIN ");
            sqlCommand.Append("mp_CommerceReportOrders cr ");
            sqlCommand.Append("ON cr.RowGuid = s.Guid ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("cr.RowGuid IS NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("s.ModuleGuid = ?ModuleGuid ");
            sqlCommand.Append("AND s.OrderStatusGuid <> 'de3b9331-b98f-493f-be5e-926ffe5003bc' "); //cancelled
            sqlCommand.Append("AND s.OrderStatusGuid <> '0db28432-d9a9-423e-84f2-8a94db434643' "); //received
            sqlCommand.Append("AND s.OrderStatusGuid <> '00000000-0000-0000-0000-000000000000' "); //none
            sqlCommand.Append("AND s.CompletedUtc IS NOT NULL ");
            sqlCommand.Append("AND ");
            sqlCommand.Append("s.Amount > 0 ");

            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleGuid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleGuid.ToString();

            MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);


        }

    }
}
