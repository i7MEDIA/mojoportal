using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBGoogleCheckoutLog
{

	/// <summary>
	/// Inserts a row in the mp_GoogleCheckoutLog table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="notificationType"> notificationType </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="serialNumber"> serialNumber </param>
	/// <param name="gTimestamp"> gTimestamp </param>
	/// <param name="orderNumber"> orderNumber </param>
	/// <param name="buyerId"> buyerId </param>
	/// <param name="fullfillState"> fullfillState </param>
	/// <param name="financeState"> financeState </param>
	/// <param name="emailListOptIn"> emailListOptIn </param>
	/// <param name="avsResponse"> avsResponse </param>
	/// <param name="cvnResponse"> cvnResponse </param>
	/// <param name="authExpDate"> authExpDate </param>
	/// <param name="authAmt"> authAmt </param>
	/// <param name="discountTotal"> discountTotal </param>
	/// <param name="shippingTotal"> shippingTotal </param>
	/// <param name="taxTotal"> taxTotal </param>
	/// <param name="orderTotal"> orderTotal </param>
	/// <param name="latestChgAmt"> latestChgAmt </param>
	/// <param name="totalChgAmt"> totalChgAmt </param>
	/// <param name="latestRefundAmt"> latestRefundAmt </param>
	/// <param name="totalRefundAmt"> totalRefundAmt </param>
	/// <param name="latestChargeback"> latestChargeback </param>
	/// <param name="totalChargeback"> totalChargeback </param>
	/// <param name="cartXml"> cartXml </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		DateTime createdUtc,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string notificationType,
		string rawResponse,
		string serialNumber,
		DateTime gTimestamp,
		string orderNumber,
		string buyerId,
		string fullfillState,
		string financeState,
		bool emailListOptIn,
		string avsResponse,
		string cvnResponse,
		DateTime authExpDate,
		decimal authAmt,
		decimal discountTotal,
		decimal shippingTotal,
		decimal taxTotal,
		decimal orderTotal,
		decimal latestChgAmt,
		decimal totalChgAmt,
		decimal latestRefundAmt,
		decimal totalRefundAmt,
		decimal latestChargeback,
		decimal totalChargeback,
		string cartXml,
		string providerName)
	{

		#region Bit Conversion

		int intEmailListOptIn;
		if (emailListOptIn)
		{
			intEmailListOptIn = 1;
		}
		else
		{
			intEmailListOptIn = 0;
		}


		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_GoogleCheckoutLog (
        RowGuid, 
        CreatedUtc, 
        SiteGuid, 
        UserGuid, 
        StoreGuid, 
        CartGuid, 
        NotificationType, 
        RawResponse, 
        SerialNumber, 
        GTimestamp, 
        OrderNumber, 
        BuyerId, 
        FullfillState, 
        FinanceState, 
        EmailListOptIn, 
        AvsResponse, 
        CvnResponse, 
        AuthExpDate, 
        AuthAmt, 
        DiscountTotal, 
        ShippingTotal, 
        TaxTotal, 
        OrderTotal, 
        LatestChgAmt, 
        TotalChgAmt, 
        LatestRefundAmt, 
        TotalRefundAmt, 
        LatestChargeback, 
        TotalChargeback, 
        CartXml, 
        ProviderName 
    )
VALUES (
    ?RowGuid, 
    ?CreatedUtc, 
    ?SiteGuid, 
    ?UserGuid, 
    ?StoreGuid, 
    ?CartGuid, 
    ?NotificationType, 
    ?RawResponse, 
    ?SerialNumber, 
    ?GTimestamp, 
    ?OrderNumber, 
    ?BuyerId, 
    ?FullfillState, 
    ?FinanceState, 
    ?EmailListOptIn, 
    ?AvsResponse, 
    ?CvnResponse, 
    ?AuthExpDate, 
    ?AuthAmt, 
    ?DiscountTotal, 
    ?ShippingTotal, 
    ?TaxTotal, 
    ?OrderTotal, 
    ?LatestChgAmt, 
    ?TotalChgAmt, 
    ?LatestRefundAmt, 
    ?TotalRefundAmt, 
    ?LatestChargeback, 
    ?TotalChargeback, 
    ?CartXml, 
    ?ProviderName 
)
;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?CreatedUtc", MySqlDbType.DateTime) {
				Direction = ParameterDirection.Input,
				Value = createdUtc
				},

			new("?SiteGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?StoreGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			},

			new("?CartGuid", MySqlDbType.VarChar, 36) {
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			},

			new("?NotificationType", MySqlDbType.VarChar, 255) {
				Direction = ParameterDirection.Input,
				Value = notificationType
			},

			new("?RawResponse", MySqlDbType.Text) {
				Direction = ParameterDirection.Input,
				Value = rawResponse
			},

			new ("?SerialNumber", MySqlDbType.VarChar, 50) {
				Direction = ParameterDirection.Input,
				Value = serialNumber
			},

			new ("?GTimestamp", MySqlDbType.DateTime) {
				Direction = ParameterDirection.Input,
				Value = gTimestamp
			},

			new("?OrderNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = orderNumber
			},

			new("?BuyerId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = buyerId
			},

			new("?FullfillState", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = fullfillState
			},

			new("?FinanceState", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = financeState
			},

			new("?EmailListOptIn", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intEmailListOptIn
			},

			new("?AvsResponse", MySqlDbType.VarChar, 5)
			{
				Direction = ParameterDirection.Input,
				Value = avsResponse
			},

			new("?CvnResponse", MySqlDbType.VarChar, 5)
			{
				Direction = ParameterDirection.Input,
				Value = cvnResponse
			},

			new("?AuthExpDate", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = authExpDate
			},

			new("?AuthAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = authAmt
			},

			new("?DiscountTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = discountTotal
			},

			new("?ShippingTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = shippingTotal
			},

			new("?TaxTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = taxTotal
			},

			new("?OrderTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = orderTotal
			},

			new("?LatestChgAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = latestChgAmt
			},

			new("?TotalChgAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = totalChgAmt
			},

			new("?LatestRefundAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = latestRefundAmt
			},

			new("?TotalRefundAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = totalRefundAmt
			},

			new("?LatestChargeback", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = latestChargeback
			},

			new("?TotalChargeback", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = totalChargeback
			},

			new("?CartXml", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = cartXml
			},

			new("?ProviderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = providerName
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_GoogleCheckoutLog table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="notificationType"> notificationType </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="serialNumber"> serialNumber </param>
	/// <param name="gTimestamp"> gTimestamp </param>
	/// <param name="orderNumber"> orderNumber </param>
	/// <param name="buyerId"> buyerId </param>
	/// <param name="fullfillState"> fullfillState </param>
	/// <param name="financeState"> financeState </param>
	/// <param name="emailListOptIn"> emailListOptIn </param>
	/// <param name="avsResponse"> avsResponse </param>
	/// <param name="cvnResponse"> cvnResponse </param>
	/// <param name="authExpDate"> authExpDate </param>
	/// <param name="authAmt"> authAmt </param>
	/// <param name="discountTotal"> discountTotal </param>
	/// <param name="shippingTotal"> shippingTotal </param>
	/// <param name="taxTotal"> taxTotal </param>
	/// <param name="orderTotal"> orderTotal </param>
	/// <param name="latestChgAmt"> latestChgAmt </param>
	/// <param name="totalChgAmt"> totalChgAmt </param>
	/// <param name="latestRefundAmt"> latestRefundAmt </param>
	/// <param name="totalRefundAmt"> totalRefundAmt </param>
	/// <param name="latestChargeback"> latestChargeback </param>
	/// <param name="totalChargeback"> totalChargeback </param>
	/// <param name="cartXml"> cartXml </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		DateTime createdUtc,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string notificationType,
		string rawResponse,
		string serialNumber,
		DateTime gTimestamp,
		string orderNumber,
		string buyerId,
		string fullfillState,
		string financeState,
		bool emailListOptIn,
		string avsResponse,
		string cvnResponse,
		DateTime authExpDate,
		decimal authAmt,
		decimal discountTotal,
		decimal shippingTotal,
		decimal taxTotal,
		decimal orderTotal,
		decimal latestChgAmt,
		decimal totalChgAmt,
		decimal latestRefundAmt,
		decimal totalRefundAmt,
		decimal latestChargeback,
		decimal totalChargeback,
		string cartXml)
	{
		#region Bit Conversion

		int intEmailListOptIn;
		if (emailListOptIn)
		{
			intEmailListOptIn = 1;
		}
		else
		{
			intEmailListOptIn = 0;
		}


		#endregion

		string sqlCommand = @"
UPDATE 
    mp_GoogleCheckoutLog 
SET  
    CreatedUtc = ?CreatedUtc, 
    SiteGuid = ?SiteGuid, 
    UserGuid = ?UserGuid, 
    StoreGuid = ?StoreGuid, 
    CartGuid = ?CartGuid, 
    NotificationType = ?NotificationType, 
    RawResponse = ?RawResponse, 
    SerialNumber = ?SerialNumber, 
    GTimestamp = ?GTimestamp, 
    OrderNumber = ?OrderNumber, 
    BuyerId = ?BuyerId, 
    FullfillState = ?FullfillState, 
    FinanceState = ?FinanceState, 
    EmailListOptIn = ?EmailListOptIn, 
    AvsResponse = ?AvsResponse, 
    CvnResponse = ?CvnResponse, 
    AuthExpDate = ?AuthExpDate, 
    AuthAmt = ?AuthAmt, 
    DiscountTotal = ?DiscountTotal, 
    ShippingTotal = ?ShippingTotal, 
    TaxTotal = ?TaxTotal, 
    OrderTotal = ?OrderTotal, 
    LatestChgAmt = ?LatestChgAmt, 
    TotalChgAmt = ?TotalChgAmt, 
    LatestRefundAmt = ?LatestRefundAmt, 
    TotalRefundAmt = ?TotalRefundAmt, 
    LatestChargeback = ?LatestChargeback, 
    TotalChargeback = ?TotalChargeback, 
    CartXml = ?CartXml 
WHERE  
    RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?CreatedUtc", MySqlDbType.DateTime) {
			Direction = ParameterDirection.Input,
			Value = createdUtc
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = siteGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = userGuid.ToString()
			},

			new("?StoreGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = storeGuid.ToString()
			},

			new("?CartGuid", MySqlDbType.VarChar, 36) {
			Direction = ParameterDirection.Input,
			Value = cartGuid.ToString()
			},

			new("?NotificationType", MySqlDbType.VarChar, 255) {
			Direction = ParameterDirection.Input,
			Value = notificationType
			},

			new("?RawResponse", MySqlDbType.Text) {
			Direction = ParameterDirection.Input,
			Value = rawResponse
			},

			new("?SerialNumber", MySqlDbType.VarChar, 50) {
			Direction = ParameterDirection.Input,
			Value = serialNumber
			},

			new("?GTimestamp", MySqlDbType.DateTime) {
			Direction = ParameterDirection.Input,
			Value = gTimestamp
			},

			new("?OrderNumber", MySqlDbType.VarChar, 50) {
			Direction = ParameterDirection.Input,
			Value = orderNumber
			},

			new("?BuyerId", MySqlDbType.VarChar, 50) {
			Direction = ParameterDirection.Input,
			Value = buyerId
			},

			new("?FullfillState", MySqlDbType.VarChar, 50) {
			Direction = ParameterDirection.Input,
			Value = fullfillState
			},

			new("?FinanceState", MySqlDbType.VarChar, 50) {
			Direction = ParameterDirection.Input,
			Value = financeState
			},

			new("?EmailListOptIn", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = intEmailListOptIn
			},

			new("?AvsResponse", MySqlDbType.VarChar, 5) {
			Direction = ParameterDirection.Input,
			Value = avsResponse
			},

			new("?CvnResponse", MySqlDbType.VarChar, 5) {
			Direction = ParameterDirection.Input,
			Value = cvnResponse
			},

			new("?AuthExpDate", MySqlDbType.DateTime) {
			Direction = ParameterDirection.Input,
			Value = authExpDate
			},

			new("?AuthAmt", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = authAmt
			},

			new("?DiscountTotal", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = discountTotal
			},

			new("?ShippingTotal", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = shippingTotal
			},

			new("?TaxTotal", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = taxTotal
			},

			new("?OrderTotal", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = orderTotal
			},

			new("?LatestChgAmt", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = latestChgAmt
			},

			new("?TotalChgAmt", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = totalChgAmt
			},

			new("?LatestRefundAmt", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = latestRefundAmt
			},

			new("?TotalRefundAmt", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = totalRefundAmt
			},

			new("?LatestChargeback", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = latestChargeback
			},

			new("?TotalChargeback", MySqlDbType.Decimal) {
			Direction = ParameterDirection.Input,
			Value = totalChargeback
			},

			new("?CartXml", MySqlDbType.Text) {
			Direction = ParameterDirection.Input,
			Value = cartXml
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_GoogleCheckoutLog table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_GoogleCheckoutLog 
WHERE RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteByCart(Guid cartGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_GoogleCheckoutLog 
WHERE CartGuid = ?CartGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteBySite(Guid siteGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_GoogleCheckoutLog 
WHERE SiteGuid = ?SiteGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	public static bool DeleteByStore(Guid storeGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_GoogleCheckoutLog 
WHERE StoreGuid = ?StoreGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?StoreGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GoogleCheckoutLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT  * 
FROM mp_GoogleCheckoutLog 
WHERE RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_GoogleCheckoutLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetMostRecentByOrder(string googleOrderId)
	{
		string sqlCommand = @"
SELECT  * 
FROM mp_GoogleCheckoutLog 
WHERE OrderNumber = ?OrderNumber 
AND CartGuid <> '00000000-0000-0000-0000-000000000000' 
AND NotificationType = 'NewOrderNotification' 
ORDER BY CreatedUtc DESC;";

		var arParams = new List<MySqlParameter>
		{
			new("?OrderNumber", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = googleOrderId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_GoogleCheckoutLog table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_GoogleCheckoutLog;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets a count of rows in the mp_GoogleCheckoutLog table.
	/// </summary>
	public static int GetCountByCart(Guid cartGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_GoogleCheckoutLog 
WHERE CartGuid = ?CartGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a count of rows in the mp_GoogleCheckoutLog table.
	/// </summary>
	public static int GetCountByStore(Guid storeGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_GoogleCheckoutLog 
WHERE StoreGuid = ?StoreGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?StoreGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams));
	}

	/// <summary>
	/// Gets a page of data from the ws_GoogleCheckoutLog table.
	/// </summary>
	/// <param name="cartGuid">The cartGuid</param>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPageByCart(
		Guid cartGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByCart(cartGuid);

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

		string sqlCommand = @"
SELECT	* 
FROM	mp_GoogleCheckoutLog  
WHERE 
CartGuid = ?CartGuid 
ORDER BY CreatedUtc  
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}
		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32) {
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32) {
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);


	}

	/// <summary>
	/// Gets a page of data from the mp_GoogleCheckoutLog table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPageByStore(
		Guid storeGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCountByStore(storeGuid);

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

		string sqlCommand = @"
SELECT	* 
FROM	mp_GoogleCheckoutLog  
WHERE 
StoreGuid = ?StoreGuid 
ORDER BY CreatedUtc DESC 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}
		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?StoreGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			},

			new("?PageSize", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32) {
			Direction = ParameterDirection.Input,
			Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);
	}
}
