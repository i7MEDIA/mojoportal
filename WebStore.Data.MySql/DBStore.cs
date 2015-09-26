/// Author:					Joe Audette
/// Created:				2008-02-26
/// Last Modified:		    2012-07-20
/// 
/// This implementation is for MySQL. 
/// 
/// The use and distribution terms for this software are covered by the 
/// GPL (http://www.gnu.org/licenses/gpl.html)
/// which can be found in the file GPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Globalization;
using MySql.Data.MySqlClient;
using mojoPortal.Data;

namespace WebStore.Data
{
    
    public static class DBStore
    {
        

        public static int Add(
            Guid guid,
            Guid siteGuid,
            int moduleId,
            string name,
            string description,
            string ownerName,
            string ownerEmail,
            string salesEmail,
            string supportEmail,
            string emailFrom,
            string orderBccEmail,
            string phone,
            string fax,
            string address,
            string city,
            Guid zoneGuid,
            string postalCode,
            Guid countryGuid,
            bool isClosed,
            string closedMessage,
            DateTime created,
            Guid createdBy)
        {
            #region Bit Conversion

            

            int intIsClosed = 0;
            if (isClosed)
            {
                intIsClosed = 1;
            }
           
           
            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("INSERT INTO ws_Store (");
            sqlCommand.Append("Guid, ");
            sqlCommand.Append("SiteGuid, ");
            sqlCommand.Append("ModuleID, ");
            sqlCommand.Append("Name, ");
            sqlCommand.Append("Description, ");
            
            sqlCommand.Append("OwnerName, ");
            sqlCommand.Append("OwnerEmail, ");
            sqlCommand.Append("SalesEmail, ");
            sqlCommand.Append("SupportEmail, ");
            sqlCommand.Append("EmailFrom, ");
            sqlCommand.Append("OrderBCCEmail, ");
            sqlCommand.Append("Phone, ");
            sqlCommand.Append("Fax, ");
            sqlCommand.Append("Address, ");
            sqlCommand.Append("City, ");
            sqlCommand.Append("ZoneGuid, ");
            sqlCommand.Append("PostalCode, ");
            sqlCommand.Append("CountryGuid, ");
            sqlCommand.Append("IsClosed, ");
            sqlCommand.Append("ClosedMessage, ");
            
            sqlCommand.Append("Created, ");
            sqlCommand.Append("CreatedBy )");

            sqlCommand.Append(" VALUES (");
            sqlCommand.Append("?Guid, ");
            sqlCommand.Append("?SiteGuid, ");
            sqlCommand.Append("?ModuleID, ");
            sqlCommand.Append("?Name, ");
            sqlCommand.Append("?Description, ");
            
            sqlCommand.Append("?OwnerName, ");
            sqlCommand.Append("?OwnerEmail, ");
            sqlCommand.Append("?SalesEmail, ");
            sqlCommand.Append("?SupportEmail, ");
            sqlCommand.Append("?EmailFrom, ");
            sqlCommand.Append("?OrderBCCEmail, ");
            sqlCommand.Append("?Phone, ");
            sqlCommand.Append("?Fax, ");
            sqlCommand.Append("?Address, ");
            sqlCommand.Append("?City, ");
            sqlCommand.Append("?ZoneGuid, ");
            sqlCommand.Append("?PostalCode, ");
            sqlCommand.Append("?CountryGuid, ");
            sqlCommand.Append("?IsClosed, ");
            sqlCommand.Append("?ClosedMessage, ");
           
            sqlCommand.Append("?Created, ");
            sqlCommand.Append("?CreatedBy )");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[22];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = siteGuid.ToString();

            arParams[2] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = moduleId;

            arParams[3] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = name;

            arParams[4] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = description;

            arParams[5] = new MySqlParameter("?OwnerName", MySqlDbType.VarChar, 255);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = ownerName;

            arParams[6] = new MySqlParameter("?OwnerEmail", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = ownerEmail;

            arParams[7] = new MySqlParameter("?SalesEmail", MySqlDbType.VarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = salesEmail;

            arParams[8] = new MySqlParameter("?SupportEmail", MySqlDbType.VarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = supportEmail;

            arParams[9] = new MySqlParameter("?EmailFrom", MySqlDbType.VarChar, 100);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = emailFrom;

            arParams[10] = new MySqlParameter("?OrderBCCEmail", MySqlDbType.VarChar, 100);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = orderBccEmail;

            arParams[11] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 32);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = phone;

            arParams[12] = new MySqlParameter("?Fax", MySqlDbType.VarChar, 32);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = fax;

            arParams[13] = new MySqlParameter("?Address", MySqlDbType.VarChar, 255);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = address;

            arParams[14] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = city;

            arParams[15] = new MySqlParameter("?ZoneGuid", MySqlDbType.VarChar, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = zoneGuid.ToString();

            arParams[16] = new MySqlParameter("?PostalCode", MySqlDbType.VarChar, 50);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = postalCode;

            arParams[17] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = countryGuid.ToString();

            arParams[18] = new MySqlParameter("?IsClosed", MySqlDbType.Int32);
            arParams[18].Direction = ParameterDirection.Input;
            arParams[18].Value = intIsClosed;

            arParams[19] = new MySqlParameter("?ClosedMessage", MySqlDbType.Text);
            arParams[19].Direction = ParameterDirection.Input;
            arParams[19].Value = closedMessage;

            arParams[20] = new MySqlParameter("?Created", MySqlDbType.DateTime);
            arParams[20].Direction = ParameterDirection.Input;
            arParams[20].Value = created;

            arParams[21] = new MySqlParameter("?CreatedBy", MySqlDbType.VarChar, 36);
            arParams[21].Direction = ParameterDirection.Input;
            arParams[21].Value = createdBy.ToString();

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return rowsAffected;
           
        }

        public static bool Update(
            Guid guid,
            string name,
            string description,
            string ownerName,
            string ownerEmail,
            string salesEmail,
            string supportEmail,
            string emailFrom,
            string orderBccEmail,
            string phone,
            string fax,
            string address,
            string city,
            Guid zoneGuid,
            string postalCode,
            Guid countryGuid,
            bool isClosed,
            string closedMessage)
        {
            #region Bit Conversion

            int intIsClosed;
            if (isClosed)
            {
                intIsClosed = 1;
            }
            else
            {
                intIsClosed = 0;
            }

            

            #endregion

            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("UPDATE ws_Store ");
            sqlCommand.Append("SET  ");
            
            sqlCommand.Append("Name = ?Name, ");
            sqlCommand.Append("Description = ?Description, ");
            
            sqlCommand.Append("OwnerName = ?OwnerName, ");
            sqlCommand.Append("OwnerEmail = ?OwnerEmail, ");
            sqlCommand.Append("SalesEmail = ?SalesEmail, ");
            sqlCommand.Append("SupportEmail = ?SupportEmail, ");
            sqlCommand.Append("EmailFrom = ?EmailFrom, ");
            sqlCommand.Append("OrderBCCEmail = ?OrderBCCEmail, ");
            sqlCommand.Append("Phone = ?Phone, ");
            sqlCommand.Append("Fax = ?Fax, ");
            sqlCommand.Append("Address = ?Address, ");
            sqlCommand.Append("City = ?City, ");
            sqlCommand.Append("ZoneGuid = ?ZoneGuid, ");
            sqlCommand.Append("PostalCode = ?PostalCode, ");
            sqlCommand.Append("CountryGuid = ?CountryGuid, ");
            sqlCommand.Append("IsClosed = ?IsClosed, ");
            sqlCommand.Append("ClosedMessage = ?ClosedMessage ");
            
           
            sqlCommand.Append("WHERE  ");
            sqlCommand.Append("Guid = ?Guid ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[18];

            arParams[0] = new MySqlParameter("?Guid", MySqlDbType.VarChar, 36);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = guid.ToString();

            arParams[1] = new MySqlParameter("?Name", MySqlDbType.VarChar, 255);
            arParams[1].Direction = ParameterDirection.Input;
            arParams[1].Value = name;

            arParams[2] = new MySqlParameter("?Description", MySqlDbType.Text);
            arParams[2].Direction = ParameterDirection.Input;
            arParams[2].Value = description;

            arParams[3] = new MySqlParameter("?OwnerName", MySqlDbType.VarChar, 255);
            arParams[3].Direction = ParameterDirection.Input;
            arParams[3].Value = ownerName;

            arParams[4] = new MySqlParameter("?OwnerEmail", MySqlDbType.VarChar, 100);
            arParams[4].Direction = ParameterDirection.Input;
            arParams[4].Value = ownerEmail;

            arParams[5] = new MySqlParameter("?SalesEmail", MySqlDbType.VarChar, 100);
            arParams[5].Direction = ParameterDirection.Input;
            arParams[5].Value = salesEmail;

            arParams[6] = new MySqlParameter("?SupportEmail", MySqlDbType.VarChar, 100);
            arParams[6].Direction = ParameterDirection.Input;
            arParams[6].Value = supportEmail;

            arParams[7] = new MySqlParameter("?EmailFrom", MySqlDbType.VarChar, 100);
            arParams[7].Direction = ParameterDirection.Input;
            arParams[7].Value = emailFrom;

            arParams[8] = new MySqlParameter("?OrderBCCEmail", MySqlDbType.VarChar, 100);
            arParams[8].Direction = ParameterDirection.Input;
            arParams[8].Value = orderBccEmail;

            arParams[9] = new MySqlParameter("?Phone", MySqlDbType.VarChar, 32);
            arParams[9].Direction = ParameterDirection.Input;
            arParams[9].Value = phone;

            arParams[10] = new MySqlParameter("?Fax", MySqlDbType.VarChar, 32);
            arParams[10].Direction = ParameterDirection.Input;
            arParams[10].Value = fax;

            arParams[11] = new MySqlParameter("?Address", MySqlDbType.VarChar, 255);
            arParams[11].Direction = ParameterDirection.Input;
            arParams[11].Value = address;

            arParams[12] = new MySqlParameter("?City", MySqlDbType.VarChar, 255);
            arParams[12].Direction = ParameterDirection.Input;
            arParams[12].Value = city;

            arParams[13] = new MySqlParameter("?ZoneGuid", MySqlDbType.VarChar, 36);
            arParams[13].Direction = ParameterDirection.Input;
            arParams[13].Value = zoneGuid.ToString();

            arParams[14] = new MySqlParameter("?PostalCode", MySqlDbType.VarChar, 50);
            arParams[14].Direction = ParameterDirection.Input;
            arParams[14].Value = postalCode;

            arParams[15] = new MySqlParameter("?CountryGuid", MySqlDbType.VarChar, 36);
            arParams[15].Direction = ParameterDirection.Input;
            arParams[15].Value = countryGuid.ToString();

            arParams[16] = new MySqlParameter("?IsClosed", MySqlDbType.Int32);
            arParams[16].Direction = ParameterDirection.Input;
            arParams[16].Value = intIsClosed;

            arParams[17] = new MySqlParameter("?ClosedMessage", MySqlDbType.Text);
            arParams[17].Direction = ParameterDirection.Input;
            arParams[17].Value = closedMessage;

            

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > -1);

        }

        public static bool Delete(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("DELETE FROM ws_Store ");
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

        public static bool DeleteByModule(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM ws_CartOffers  ");
            sqlCommand.Append("WHERE CartGuid IN (SELECT CartGuid  ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_CartOrderInfo  ");
            sqlCommand.Append("WHERE CartGuid IN (SELECT CartGuid   ");
            sqlCommand.Append("FROM ws_Cart  ");
            sqlCommand.Append("WHERE StoreGuid IN (  ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadHistory  ");
            sqlCommand.Append("WHERE TicketGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_FullfillDownloadTicket ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTicket  ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTerms ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OfferHistory ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OrderOfferProduct ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_OrderOffers ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Order  ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OfferProduct ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT [Guid] ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_ProductFile ");
            sqlCommand.Append("WHERE ProductGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_ProductHistory ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE ModuleID = ?ModuleID ");
            sqlCommand.Append("); ");

            
            sqlCommand.Append("DELETE FROM ws_Store ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static bool DeleteBySite(int siteId)
        {
            StringBuilder sqlCommand = new StringBuilder();

            sqlCommand.Append("DELETE FROM ws_CartOffers  ");
            sqlCommand.Append("WHERE CartGuid IN (SELECT CartGuid  ");
            sqlCommand.Append("FROM ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID)  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_CartOrderInfo  ");
            sqlCommand.Append("WHERE CartGuid IN (SELECT CartGuid   ");
            sqlCommand.Append("FROM ws_Cart  ");
            sqlCommand.Append("WHERE StoreGuid IN (  ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID)  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_Cart ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadHistory  ");
            sqlCommand.Append("WHERE TicketGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_FullfillDownloadTicket ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTicket  ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_FullfillDownloadTerms ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OfferHistory ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM [dbo].mp_Sites WHERE SiteID = ?SiteID)  ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OrderOfferProduct ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid  ");
            sqlCommand.Append("FROM ws_Store  ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_OrderOffers ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Order  ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_OfferProduct ");
            sqlCommand.Append("WHERE OfferGuid IN (SELECT [Guid] ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE  ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM ws_ProductFile ");
            sqlCommand.Append("WHERE ProductGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_ProductHistory ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");

            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Product ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");


            sqlCommand.Append("DELETE FROM mp_FriendlyUrls ");
            sqlCommand.Append("WHERE PageGuid IN (SELECT Guid ");
            sqlCommand.Append("FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(")); ");

            sqlCommand.Append("DELETE FROM ws_Offer ");
            sqlCommand.Append("WHERE StoreGuid IN ( ");
            sqlCommand.Append("SELECT Guid ");
            sqlCommand.Append("FROM ws_Store ");
            sqlCommand.Append("WHERE SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append("); ");


            sqlCommand.Append("DELETE FROM ws_Store ");
            sqlCommand.Append("WHERE ");
            sqlCommand.Append("SiteGuid IN (SELECT SiteGuid FROM mp_Sites WHERE SiteID = ?SiteID) ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?SiteID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = siteId;

            int rowsAffected = MySqlHelper.ExecuteNonQuery(
                ConnectionString.GetWriteConnectionString(),
                sqlCommand.ToString(),
                arParams);

            return (rowsAffected > 0);

        }

        public static IDataReader Get(Guid guid)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  s.*, ");
            sqlCommand.Append("m.Guid As ModuleGuid ");

            sqlCommand.Append("FROM	ws_Store s ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON m.ModuleID = s.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.Guid = ?Guid ");
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

        public static IDataReader Get(int moduleId)
        {
            StringBuilder sqlCommand = new StringBuilder();
            sqlCommand.Append("SELECT  s.*, ");
            sqlCommand.Append("m.Guid As ModuleGuid ");

            sqlCommand.Append("FROM	ws_Store s ");

            sqlCommand.Append("JOIN mp_Modules m ");
            sqlCommand.Append("ON m.ModuleID = s.ModuleID ");

            sqlCommand.Append("WHERE ");
            sqlCommand.Append("s.ModuleID = ?ModuleID ");
            sqlCommand.Append(";");

            MySqlParameter[] arParams = new MySqlParameter[1];

            arParams[0] = new MySqlParameter("?ModuleID", MySqlDbType.Int32);
            arParams[0].Direction = ParameterDirection.Input;
            arParams[0].Value = moduleId;

            return MySqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                sqlCommand.ToString(),
                arParams);

        }


    }
}
