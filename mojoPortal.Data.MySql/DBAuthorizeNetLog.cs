using System;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBAuthorizeNetLog
{
	/// <summary>
	/// Inserts a row in the mp_AuthorizeNetLog table. Returns rows affected count.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <param name="createdUtc"> createdUtc </param>
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
	/// <returns>int</returns>
	public static int Create(
		Guid rowGuid,
		DateTime createdUtc,
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
		var sqlCommand = @"
INSERT INTO mp_AuthorizeNetLog (
	RowGuid
	,CreatedUtc
	,SiteGuid
	,UserGuid
	,StoreGuid
	,CartGuid
	,RawResponse
	,ResponseCode
	,ResponseReasonCode
	,Reason
	,AvsCode
	,CcvCode
	,CavCode
	,TransactionId
	,TransactionType
	,Method
	,AuthCode
	,Amount
	,Tax
	,Duty
	,Freight 
)
VALUES (
	?RowGuid
	,?CreatedUtc
	,?SiteGuid
	,?UserGuid
	,?StoreGuid
	,?CartGuid
	,?RawResponse
	,?ResponseCode
	,?ResponseReasonCode
	,?Reason
	,?AvsCode
	,?CcvCode
	,?CavCode
	,?TransactionId
	,?TransactionType
	,?Method
	,?AuthCode
	,?Amount
	,?Tax
	,?Duty
	,?Freight 
);";

		var arParams = new MySqlParameter[]
		{
			new("?RowGuid", MySqlDbType.VarChar, 36) { Value = rowGuid },
			new("?CreatedUtc", MySqlDbType.DateTime) { Value = createdUtc },
			new("?SiteGuid", MySqlDbType.VarChar, 36) { Value = siteGuid },
			new("?UserGuid", MySqlDbType.VarChar, 36) { Value = userGuid },
			new("?StoreGuid", MySqlDbType.VarChar, 36) { Value = storeGuid },
			new("?CartGuid", MySqlDbType.VarChar, 36) { Value = cartGuid },
			new("?RawResponse", MySqlDbType.Text) { Value = rawResponse },
			new("?ResponseCode", MySqlDbType.VarChar, 1) { Value = responseCode },
			new("?ResponseReasonCode", MySqlDbType.VarChar, 20) { Value = responseReasonCode },
			new("?Reason", MySqlDbType.Text) { Value = reason },
			new("?AvsCode", MySqlDbType.VarChar, 50) { Value = avsCode },
			new("?CcvCode", MySqlDbType.VarChar, 1) { Value = ccvCode },
			new("?CavCode", MySqlDbType.VarChar, 1) { Value = cavCode },
			new("?TransactionId", MySqlDbType.VarChar, 50) { Value = transactionId },
			new("?TransactionType", MySqlDbType.VarChar, 50) { Value = transactionType },
			new("?Method", MySqlDbType.VarChar, 20) { Value = method },
			new("?AuthCode", MySqlDbType.VarChar, 50) { Value = authCode },
			new("?Amount", MySqlDbType.Decimal) { Value = amount },
			new("?Tax", MySqlDbType.Decimal) { Value = tax },
			new("?Duty", MySqlDbType.Decimal) { Value = duty },
			new("?Freight", MySqlDbType.Decimal) { Value = freight }
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWrite(), sqlCommand, arParams);
		return rowsAffected;
	}

	/// <summary>
	/// Updates a row in the mp_AuthorizeNetLog table. Returns true if row updated.
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
	mp_AuthorizeNetLog 
SET  
	SiteGuid = ?SiteGuid 
	,UserGuid = ?UserGuid 
	,StoreGuid = ?StoreGuid 
	,CartGuid = ?CartGuid 
	,RawResponse = ?RawResponse 
	,ResponseCode = ?ResponseCode 
	,ResponseReasonCode = ?ResponseReasonCode 
	,Reason = ?Reason 
	,AvsCode = ?AvsCode 
	,CcvCode = ?CcvCode 
	,CavCode = ?CavCode 
	,TransactionId = ?TransactionId 
	,TransactionType = ?TransactionType 
	,Method = ?Method 
	,AuthCode = ?AuthCode 
	,Amount = ?Amount 
	,Tax = ?Tax 
	,Duty = ?Duty 
	,Freight = ?Freight 
WHERE  
	RowGuid = ?RowGuid;";

		var arParams = new MySqlParameter[]
		{
			new("?RowGuid", MySqlDbType.VarChar, 36) { Value = rowGuid.ToString() },
			new("?SiteGuid", MySqlDbType.VarChar, 36) { Value = siteGuid.ToString() },
			new("?UserGuid", MySqlDbType.VarChar, 36) { Value = userGuid.ToString() },
			new("?StoreGuid", MySqlDbType.VarChar, 36) { Value = storeGuid.ToString() },
			new("?CartGuid", MySqlDbType.VarChar, 36) { Value = cartGuid.ToString() },
			new("?RawResponse", MySqlDbType.Text) { Value = rawResponse },
			new("?ResponseCode", MySqlDbType.VarChar, 1) { Value = responseCode },
			new("?ResponseReasonCode", MySqlDbType.VarChar, 20) { Value = responseReasonCode },
			new("?Reason", MySqlDbType.Text) { Value = reason },
			new("?AvsCode", MySqlDbType.VarChar, 50) { Value = avsCode },
			new("?CcvCode", MySqlDbType.VarChar, 1) { Value = ccvCode },
			new("?CavCode", MySqlDbType.VarChar, 1) { Value = cavCode },
			new("?TransactionId", MySqlDbType.VarChar, 50) { Value = transactionId },
			new("?TransactionType", MySqlDbType.VarChar, 50) { Value = transactionType },
			new("?Method", MySqlDbType.VarChar, 20) { Value = method },
			new("?AuthCode", MySqlDbType.VarChar, 50) { Value = authCode },
			new("?Amount", MySqlDbType.Decimal) { Value = amount },
			new("?Tax", MySqlDbType.Decimal) { Value = tax },
			new("?Duty", MySqlDbType.Decimal) { Value = duty },
			new("?Freight", MySqlDbType.Decimal) { Value = freight }
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWrite(), sqlCommand, arParams);

		return rowsAffected > -1;
	}

	/// <summary>
	/// Deletes a row from the mp_AuthorizeNetLog table. Returns true if row deleted.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	/// <returns>bool</returns>
	public static bool Delete(Guid rowGuid)
	{
		string sqlCommand = @"DELETE FROM mp_AuthorizeNetLog WHERE RowGuid = ?RowGuid;";

		var arParams = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36) { Value = rowGuid.ToString() };

		int rowsAffected = CommandHelper.ExecuteNonQuery(ConnectionString.GetWrite(), sqlCommand, arParams);

		return rowsAffected > 0;
	}

	/// <summary>
	/// Gets an IDataReader with one row from the mp_AuthorizeNetLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetOne(Guid rowGuid)
	{
		string sqlCommand = @"SELECT * FROM mp_AuthorizeNetLog WHERE RowGuid = ?RowGuid;";

		var param = new MySqlParameter("?RowGuid", MySqlDbType.VarChar, 36) { Value = rowGuid.ToString() };

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand, param);
	}

	/// <summary>
	/// Gets an IDataReader with rows from the mp_AuthorizeNetLog table.
	/// </summary>
	/// <param name="rowGuid"> rowGuid </param>
	public static IDataReader GetByCart(Guid cartGuid)
	{
		string sqlCommand = @"SELECT * FROM mp_AuthorizeNetLog WHERE CartGuid = ?CartGuid ORDER BY CreatedUtc;";

		var param = new MySqlParameter("?CartGuid", MySqlDbType.VarChar, 36) { Value = cartGuid.ToString() };

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand, param);
	}

	/// <summary>
	/// Gets an IDataReader with all rows in the mp_AuthorizeNetLog table.
	/// </summary>
	public static IDataReader GetAll() => CommandHelper.ExecuteReader(ConnectionString.GetRead(), "SELECT * FROM mp_AuthorizeNetLog;");

	/// <summary>
	/// Gets a count of rows in the mp_AuthorizeNetLog table.
	/// </summary>
	public static int GetCount() => Convert.ToInt32(CommandHelper.ExecuteScalar(ConnectionString.GetRead(), "SELECT Count(*) FROM mp_AuthorizeNetLog;"));

	/// <summary>
	/// Gets a page of data from the mp_AuthorizeNetLog table.
	/// </summary>
	/// <param name="pageNumber">The page number.</param>
	/// <param name="pageSize">Size of the page.</param>
	/// <param name="totalPages">total pages</param>
	public static IDataReader GetPage(int pageNumber, int pageSize, out int totalPages)
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
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"SELECT * FROM	mp_AuthorizeNetLog LIMIT ?Offset, ?PageSize;";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?Offset", MySqlDbType.Int32) { Value = pageLowerBound },
			new MySqlParameter("?PageSize", MySqlDbType.Int32) { Value = pageSize },
		];

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand, arParams);
	}
}