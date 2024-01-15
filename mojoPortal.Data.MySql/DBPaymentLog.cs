using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBPaymentLog
{


	/// <summary>
	/// Inserts a row in the mp_PaymentLog table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="provider"> provider </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="responseCode"> responseCode </param>
	/// <param name="responseReasonCode"> responseReasonCode </param>
	/// <param name="reason"> reason </param>
	/// <param name="avsCode"> avsCode </param>
	/// <param name="ccvCode"> ccvCode </param>
	/// <param name="cavCode"> cavCode </param>
	/// <param name="transactionId"> transactionId </param>
	/// <param name="transactionType"> transactionType </param>
	/// <param name="method"> method </param>
	/// <param name="authCode"> authCode </param>
	/// <param name="amount"> amount </param>
	/// <param name="tax"> tax </param>
	/// <param name="duty"> duty </param>
	/// <param name="freight"> freight </param>
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		DateTime createdUtc,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string provider,
		string rawResponse,
		string responseCode,
		string responseReasonCode,
		string reason,
		string avsCode,
		string ccvCode,
		string cavCode,
		string transactionId,
		string transactionType,
		string method,
		string authCode,
		decimal amount,
		decimal tax,
		decimal duty,
		decimal freight)
	{


		string sqlCommand = @"
INSERT INTO 
    mp_PaymentLog (
        RowGuid, 
        CreatedUtc, 
        SiteGuid, 
        UserGuid, 
        StoreGuid, 
        CartGuid, 
        Provider, 
        RawResponse, 
        ResponseCode, 
        ResponseReasonCode, 
        Reason, 
        AvsCode, 
        CcvCode, 
        CavCode, 
        TransactionId, 
        TransactionType, 
        Method, 
        AuthCode, 
        Amount, 
        Tax, 
        Duty, 
        Freight 
    )
VALUES (
    ?RowGuid, 
    ?CreatedUtc, 
    ?SiteGuid, 
    ?UserGuid, 
    ?StoreGuid, 
    ?CartGuid, 
    ?Provider, 
    ?RawResponse, 
    ?ResponseCode, 
    ?ResponseReasonCode, 
    ?Reason, 
    ?AvsCode, 
    ?CcvCode, 
    ?CavCode, 
    ?TransactionId, 
    ?TransactionType, 
    ?Method, 
    ?AuthCode, 
    ?Amount, 
    ?Tax, 
    ?Duty, 
    ?Freight 
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

			new("?RawResponse", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rawResponse
			},

			new("?ResponseCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = responseCode
			},

			new("?ResponseReasonCode", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = responseReasonCode
			},

			new("?Reason", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = reason
			},

			new("?AvsCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = avsCode
			},

			new("?CcvCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = ccvCode
			},

			new("?CavCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = cavCode
			},

			new("?TransactionId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionId
			},

			new("?TransactionType", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionType
			},

			new("?Method", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = method
			},

			new("?AuthCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = authCode
			},

			new("?Amount", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = amount
			},

			new("?Tax", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = tax
			},

			new("?Duty", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = duty
			},

			new("?Freight", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = freight
			},

			new("?Provider", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = provider
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected;

	}

	/// <summary>
	/// Updates a row in the mp_PaymentLog table. Returns true if row updated.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="siteGuid"> siteGuid </param>
	/// <param name="userGuid"> userGuid </param>
	/// <param name="storeGuid"> storeGuid </param>
	/// <param name="cartGuid"> cartGuid </param>
	/// <param name="rawResponse"> rawResponse </param>
	/// <param name="responseCode"> responseCode </param>
	/// <param name="responseReasonCode"> responseReasonCode </param>
	/// <param name="reason"> reason </param>
	/// <param name="avsCode"> avsCode </param>
	/// <param name="ccvCode"> ccvCode </param>
	/// <param name="cavCode"> cavCode </param>
	/// <param name="transactionId"> transactionId </param>
	/// <param name="transactionType"> transactionType </param>
	/// <param name="method"> method </param>
	/// <param name="authCode"> authCode </param>
	/// <param name="amount"> amount </param>
	/// <param name="tax"> tax </param>
	/// <param name="duty"> duty </param>
	/// <param name="freight"> freight </param>
	/// <returns>bool</returns>
	public static bool Update(
		Guid rowGuid,
		Guid siteGuid,
		Guid userGuid,
		Guid storeGuid,
		Guid cartGuid,
		string rawResponse,
		string responseCode,
		string responseReasonCode,
		string reason,
		string avsCode,
		string ccvCode,
		string cavCode,
		string transactionId,
		string transactionType,
		string method,
		string authCode,
		decimal amount,
		decimal tax,
		decimal duty,
		decimal freight)
	{
		string sqlCommand = @"
UPDATE 
    mp_PaymentLog 
SET  
    SiteGuid = ?SiteGuid, 
    UserGuid = ?UserGuid, 
    StoreGuid = ?StoreGuid, 
    CartGuid = ?CartGuid, 
    RawResponse = ?RawResponse, 
    ResponseCode = ?ResponseCode, 
    ResponseReasonCode = ?ResponseReasonCode, 
    Reason = ?Reason, 
    AvsCode = ?AvsCode, 
    CcvCode = ?CcvCode, 
    CavCode = ?CavCode, 
    TransactionId = ?TransactionId, 
    TransactionType = ?TransactionType, 
    Method = ?Method, 
    AuthCode = ?AuthCode, 
    Amount = ?Amount, 
    Tax = ?Tax, 
    Duty = ?Duty, 
    Freight = ?Freight 
WHERE 
    RowGuid = ?RowGuid 
;";

		var arParams = new List<MySqlParameter>
			{
			new("?RowGuid",     MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
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

			new("?RawResponse", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = rawResponse
			},

			new("?ResponseCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = responseCode
			},

			new("?ResponseReasonCode", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = responseReasonCode
			},

			new("?Reason", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = reason
			},

			new("?AvsCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = avsCode
			},

			new("?CcvCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = ccvCode
			},

			new("?CavCode", MySqlDbType.VarChar, 1)
			{
				Direction = ParameterDirection.Input,
				Value = cavCode
			},

			new("?TransactionId", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionId
			},

			new("?TransactionType", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = transactionType
			},

			new("?Method", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = method
			},

			new("?AuthCode", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = authCode
			},

			new("?Amount", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = amount
			},

			new("?Tax", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = tax
			},

			new("?Duty", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = duty
			},

			new("?Freight", MySqlDbType.Decimal)
			{
				Direction = ParameterDirection.Input,
				Value = freight
			},
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);

		return rowsAffected > -1;
	}

	/// <summary>
	/// Deletes a row from the mp_PaymentLog table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"
DELETE FROM mp_PaymentLog 
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
			ConnectionString.GetWriteConnectionString(),
			sqlCommand.ToString(),
			arParams);
		return rowsAffected > 0;

	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_PaymentLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PaymentLog 
WHERE RowGuid = ?RowGuid;";

		var arParams = new List<MySqlParameter>
		{
			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			},

			new("?RowGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = rowGuid.ToString()
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_PaymentLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetByCart(Guid cartGuid)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_PaymentLog 
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
			ConnectionString.GetReadConnectionString(),
			sqlCommand.ToString(),
			arParams);

	}


}
