using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBPayPalLog
{


	/// <summary>
	/// Inserts a row in the mp_PayPalLog table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="requestType"> requestType </param>
	/// <param name="apiVersion"> apiVersion </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="token"> token </param>
	/// <param name="payerId"> payerId </param>
	/// <param name="transactionId"> transactionId </param>
	/// <param name="paymentType"> paymentType </param>
	/// <param name="paymentStatus"> paymentStatus </param>
	/// <param name="pendingReason"> pendingReason </param>
	/// <param name="reasonCode"> reasonCode </param>
	/// <param name="currencyCode"> currencyCode </param>
	/// <param name="exchangeRate"> exchangeRate </param>
	/// <param name="cartTotal"> cartTotal </param>
	/// <param name="payPalAmt"> payPalAmt </param>
	/// <param name="taxAmt"> taxAmt </param>
	/// <param name="feeAmt"> feeAmt </param>
	/// <param name="settleAmt"> settleAmt </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		DateTime createdUtc,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string requestType,
		string apiVersion,
		string rawResponse,
		string token,
		string payerId,
		string transactionId,
		string paymentType,
		string paymentStatus,
		string pendingReason,
		string reasonCode,
		string currencyCode,
		decimal exchangeRate,
		decimal cartTotal,
		decimal payPalAmt,
		decimal taxAmt,
		decimal feeAmt,
		decimal settleAmt,
		string providerName,
		string returnUrl,
		string serializedObject,
		string pdtProviderName,
		string ipnProviderName,
		string response)
	{

		string sqlCommand = @"
INSERT INTO 
    mp_PayPalLog (
        RowGuid, 
        CreatedUtc, 
        SiteGuid, 
        UserGuid, 
        StoreGuid, 
        CartGuid, 
        RequestType, 
        ApiVersion, 
        RawResponse, 
        Token, 
        PayerId, 
        TransactionId, 
        PaymentType, 
        PaymentStatus, 
        PendingReason, 
        ReasonCode, 
        CurrencyCode, 
        ExchangeRate, 
        CartTotal, 
        PayPalAmt, 
        TaxAmt, 
        FeeAmt, 
        ProviderName, 
        ReturnUrl, 
        SerializedObject, 
        PDTProviderName, 
        IPNProviderName, 
        Response, 
        SettleAmt 
    )
VALUES (
    ?RowGuid, 
    ?CreatedUtc, 
    ?SiteGuid, 
    ?UserGuid, 
    ?StoreGuid, 
    ?CartGuid, 
    ?RequestType, 
    ?ApiVersion, 
    ?RawResponse, 
    ?Token, 
    ?PayerId, 
    ?TransactionId, 
    ?PaymentType, 
    ?PaymentStatus, 
    ?PendingReason, 
    ?ReasonCode, 
    ?CurrencyCode, 
    ?ExchangeRate, 
    ?CartTotal, 
    ?PayPalAmt, 
    ?TaxAmt, 
    ?FeeAmt, 
    ?ProviderName, 
    ?ReturnUrl, 
    ?SerializedObject, 
    ?PDTProviderName, 
    ?IPNProviderName, 
    ?Response, 
    ?SettleAmt 
);";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new ("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new ("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?StoreGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			},

			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			},

			new("?RequestType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = requestType
			},

			new("?ApiVersion", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = apiVersion
			},

			new("?RawResponse", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rawResponse
			},

			new("?Token", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = token
			},

			new("?PayerId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = payerId
			},

			new("?TransactionId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionId
			},

			new("?PaymentType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = paymentType
			},

			new("?PaymentStatus", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paymentStatus
			},

			new("?PendingReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pendingReason
			},

			new("?ReasonCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = reasonCode
			},

			new("?CurrencyCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = currencyCode
			},

			new("?ExchangeRate", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = exchangeRate
			},

			new("?CartTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = cartTotal
			},

			new("?PayPalAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = payPalAmt
			},

			new("?TaxAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = taxAmt
			},

			new("?FeeAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = feeAmt
			},

			new("?SettleAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = settleAmt
			},

			new("?ProviderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = providerName
			},

			new("?ReturnUrl", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = returnUrl
			},

			new("?SerializedObject", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = serializedObject
			},

			new("?PDTProviderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pdtProviderName
			},

			new("?IPNProviderName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = ipnProviderName
			},

			new("?Response", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = response
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected;

	}


	/// <summary>
	/// Updates a row in the mp_PayPalLog table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="requestType"> requestType </param>
	/// <param name="apiVersion"> apiVersion </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="token"> token </param>
	/// <param name="payerId"> payerId </param>
	/// <param name="transactionId"> transactionId </param>
	/// <param name="paymentType"> paymentType </param>
	/// <param name="paymentStatus"> paymentStatus </param>
	/// <param name="pendingReason"> pendingReason </param>
	/// <param name="reasonCode"> reasonCode </param>
	/// <param name="currencyCode"> currencyCode </param>
	/// <param name="exchangeRate"> exchangeRate </param>
	/// <param name="cartTotal"> cartTotal </param>
	/// <param name="payPalAmt"> payPalAmt </param>
	/// <param name="taxAmt"> taxAmt </param>
	/// <param name="feeAmt"> feeAmt </param>
	/// <param name="settleAmt"> settleAmt </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		DateTime createdUtc,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string requestType,
		string apiVersion,
		string rawResponse,
		string token,
		string payerId,
		string transactionId,
		string paymentType,
		string paymentStatus,
		string pendingReason,
		string reasonCode,
		string currencyCode,
		decimal exchangeRate,
		decimal cartTotal,
		decimal payPalAmt,
		decimal taxAmt,
		decimal feeAmt,
		decimal settleAmt)
	{

		string sqlCommand = @"
UPDATE 
    mp_PayPalLog 
SET  
    CreatedUtc = ?CreatedUtc, 
    SiteGuid = ?SiteGuid, 
    UserGuid = ?UserGuid, 
    StoreGuid = ?StoreGuid, 
    CartGuid = ?CartGuid, 
    RequestType = ?RequestType, 
    ApiVersion = ?ApiVersion, 
    RawResponse = ?RawResponse, 
    Token = ?Token, 
    PayerId = ?PayerId, 
    TransactionId = ?TransactionId, 
    PaymentType = ?PaymentType, 
    PaymentStatus = ?PaymentStatus, 
    PendingReason = ?PendingReason, 
    ReasonCode = ?ReasonCode, 
    CurrencyCode = ?CurrencyCode, 
    ExchangeRate = ?ExchangeRate, 
    CartTotal = ?CartTotal, 
    PayPalAmt = ?PayPalAmt, 
    TaxAmt = ?TaxAmt, 
    FeeAmt = ?FeeAmt, 
    SettleAmt = ?SettleAmt 
WHERE 
    RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?CreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?UserGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = userGuid.ToString()
			},

			new("?StoreGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = storeGuid.ToString()
			},

			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			},

			new("?RequestType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = requestType
			},

			new("?ApiVersion", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = apiVersion
			},

			new("?RawResponse", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rawResponse
			},

			new("?Token", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = token
			},

			new("?PayerId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = payerId
			},

			new("?TransactionId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionId
			},

			new("?PaymentType", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = paymentType
			},

			new("?PaymentStatus", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = paymentStatus
			},

			new("?PendingReason", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pendingReason
			},

			new("?ReasonCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = reasonCode
			},

			new("?CurrencyCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = currencyCode
			},

			new("?ExchangeRate", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = exchangeRate
			},

			new("?CartTotal", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = cartTotal
			},

			new("?PayPalAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = payPalAmt
			},

			new("?TaxAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = taxAmt
			},

			new("?FeeAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = feeAmt
			},

			new("?SettleAmt", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = settleAmt
			}
		};



		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;

	}

	/// <summary>
	/// Deletes a row from the mp_PayPalLog table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_PayPalLog 
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
DELETE FROM mp_PayPalLog 
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
DELETE FROM mp_PayPalLog 
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
DELETE FROM mp_PayPalLog 
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
	/// Gets an IDataReader with one row from the mp_PayPalLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PayPalLog 
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
	/// Gets an IDataReader with rows from the mp_PayPalLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetByCart(Guid cartGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PayPalLog 
WHERE CartGuid = ?CartGuid 
ORDER BY CreatedUtc;";

		var arParams = new List<MySqlParameter>
		{
			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}


	/// <summary>
	/// Gets an IDataReader with one row from the mp_PayPalLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetSetExpressCheckout(string token)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PayPalLog 
WHERE Token = ?Token 
AND RequestType = 'SetExpressCheckout' 
ORDER BY CreatedUtc DESC 
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?Token", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = token
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_PayPalLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetMostRecentLog(Guid cartGuid, string requestType)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PayPalLog 
WHERE CartGuid = ?CartGuid 
AND (RequestType = ?RequestType OR ?RequestType = '') 
ORDER BY CreatedUtc DESC 
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?CartGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = cartGuid.ToString()
			},

			new("?RequestType", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = requestType
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_PayPalLog table.
	/// </summary>
	public static IDataReader GetAll()
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PayPalLog;";

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand.ToString());
	}

	/// <summary>
	/// Gets a count of rows in the mp_PayPalLog table.
	/// </summary>
	public static int GetCount()
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_PayPalLog;";

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand.ToString()));
	}

	/// <summary>
	/// Gets a page of data from the mp_PayPalLog table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount();

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
SELECT * 
FROM mp_PayPalLog 
LIMIT ?PageSize, ?OffsetRows;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},


			new("?OffsetRows", MySqlDbType.Int32)
			{
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
