using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data
{
	public static class DBEmailSendLog
	{

		/// <summary>
		/// Inserts a row in the mp_EmailSendLog table. Returns rows affected count.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <param name="specialGuid1"> specialGuid1 </param>
		/// <param name="specialGuid2"> specialGuid2 </param>
		/// <param name="toAddress"> toAddress </param>
		/// <param name="ccAddress"> ccAddress </param>
		/// <param name="bccAddress"> bccAddress </param>
		/// <param name="subject"> subject </param>
		/// <param name="textBody"> textBody </param>
		/// <param name="htmlBody"> htmlBody </param>
		/// <param name="type"> type </param>
		/// <param name="sentUtc"> sentUtc </param>
		/// <param name="fromAddress"> fromAddress </param>
		/// <param name="replyTo"> replyTo </param>
		/// <param name="userGuid"> userGuid </param>
		/// <returns>int</returns>
		public static void Create(
			Guid guid,
			Guid siteGuid,
			Guid moduleGuid,
			Guid specialGuid1,
			Guid specialGuid2,
			string toAddress,
			string ccAddress,
			string bccAddress,
			string subject,
			string textBody,
			string htmlBody,
			string type,
			DateTime sentUtc,
			string fromAddress,
			string replyTo,
			Guid userGuid)
		{
			string sqlCommand = @"
INSERT INTO 
    mp_EmailSendLog (
        Guid, 
        SiteGuid, 
        ModuleGuid, 
        SpecialGuid1, 
        SpecialGuid2, 
        ToAddress, 
        CcAddress, 
        BccAddress, 
        Subject, 
        TextBody, 
        HtmlBody, 
        Type, 
        SentUtc, 
        FromAddress, 
        ReplyTo, 
        UserGuid 
    ) 
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?SpecialGuid1, 
    ?SpecialGuid2, 
    ?ToAddress, 
    ?CcAddress, 
    ?BccAddress, 
    ?Subject, 
    ?TextBody, 
    ?HtmlBody, 
    ?Type, 
    ?SentUtc, 
    ?FromAddress, 
    ?ReplyTo, 
    ?UserGuid 
);";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				},

				new("?SiteGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = siteGuid.ToString()
				},

				new("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				},

				new("?SpecialGuid1", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = specialGuid1.ToString()
				},

				new("?SpecialGuid2", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = specialGuid2.ToString()
				},

				new("?ToAddress", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = toAddress
				},

				new("?CcAddress", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = ccAddress
				},

				new("?BccAddress", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = bccAddress
				},

				new("?Subject", MySqlDbType.VarChar, 255)
				{
					Direction = ParameterDirection.Input,
					Value = subject
				},

				new("?TextBody", MySqlDbType.LongText)
				{
					Direction = ParameterDirection.Input,
					Value = textBody
				},

				new("?HtmlBody", MySqlDbType.LongText)
				{
					Direction = ParameterDirection.Input,
					Value = htmlBody
				},

				new("?Type", MySqlDbType.VarChar, 50)
				{
					Direction = ParameterDirection.Input,
					Value = type
				},

				new("?SentUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = sentUtc
				},

				new("?FromAddress", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = fromAddress
				},

				new("?ReplyTo", MySqlDbType.VarChar, 100)
				{
					Direction = ParameterDirection.Input,
					Value = replyTo
				},

				new("?UserGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = userGuid.ToString()
				}
			};


			CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);


		}

		/// <summary>
		/// Deletes a row from the mp_EmailSendLog table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			string sqlCommand = @"
DELETE FROM mp_EmailSendLog 
WHERE Guid = ?Guid;";

			var arParams = new List<MySqlParameter>
			{
				new("?Guid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = guid.ToString()
				}
			};


			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;
		}

		/// <summary>
		/// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteBySite(Guid siteGuid)
		{
			string sqlCommand = @"
DELETE FROM mp_EmailSendLog 
WHERE SiteGuid = ?SiteGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?SiteGuid", MySqlDbType.VarChar, 36){
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

		/// <summary>
		/// Deletes rows from the mp_EmailSendLog table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool DeleteByModule(Guid moduleGuid)
		{
			string sqlCommand = @"
DELETE FROM mp_EmailSendLog 
WHERE ModuleGuid = ?ModuleGuid;";

			var arParams = new List<MySqlParameter>
			{
				new("?ModuleGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = moduleGuid.ToString()
				}
			};


			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWrite(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected > 0;

		}


	}
}
