using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data
{
	public static class DBEmailSendQueue
	{


		/// <summary>
		/// Inserts a row in the mp_EmailSendQueue table. Returns rows affected count.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <param name="siteGuid"> siteGuid </param>
		/// <param name="moduleGuid"> moduleGuid </param>
		/// <param name="userGuid"> userGuid </param>
		/// <param name="specialGuid1"> specialGuid1 </param>
		/// <param name="specialGuid2"> specialGuid2 </param>
		/// <param name="fromAddress"> fromAddress </param>
		/// <param name="replyTo"> replyTo </param>
		/// <param name="toAddress"> toAddress </param>
		/// <param name="ccAddress"> ccAddress </param>
		/// <param name="bccAddress"> bccAddress </param>
		/// <param name="subject"> subject </param>
		/// <param name="textBody"> textBody </param>
		/// <param name="htmlBody"> htmlBody </param>
		/// <param name="type"> type </param>
		/// <param name="dateToSend"> dateToSend </param>
		/// <param name="createdUtc"> createdUtc </param>
		/// <returns>int</returns>
		public static int Create(
			Guid guid,
			Guid siteGuid,
			Guid moduleGuid,
			Guid userGuid,
			Guid specialGuid1,
			Guid specialGuid2,
			string fromAddress,
			string replyTo,
			string toAddress,
			string ccAddress,
			string bccAddress,
			string subject,
			string textBody,
			string htmlBody,
			string type,
			DateTime dateToSend,
			DateTime createdUtc)
		{

			#region Bit Conversion


			#endregion

			string sqlCommand = @"
INSERT INTO 
    mp_EmailSendQueue (
        Guid, 
        SiteGuid, 
        ModuleGuid, 
        UserGuid, 
        SpecialGuid1, 
        SpecialGuid2, 
        FromAddress, 
        ReplyTo, 
        ToAddress, 
        CcAddress, 
        BccAddress, 
        Subject, 
        TextBody, 
        HtmlBody, 
        Type, 
        DateToSend, 
        CreatedUtc 
    )
VALUES (
    ?Guid, 
    ?SiteGuid, 
    ?ModuleGuid, 
    ?UserGuid, 
    ?SpecialGuid1, 
    ?SpecialGuid2, 
    ?FromAddress, 
    ?ReplyTo, 
    ?ToAddress, 
    ?CcAddress, 
    ?BccAddress, 
    ?Subject, 
    ?TextBody, 
    ?HtmlBody, 
    ?Type, 
    ?DateToSend, 
    ?CreatedUtc 
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

				new("?UserGuid", MySqlDbType.VarChar, 36)
				{
					Direction = ParameterDirection.Input,
					Value = userGuid.ToString()
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

				new("?DateToSend", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = dateToSend
				},

				new("?CreatedUtc", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = createdUtc
				}
			};


			int rowsAffected = CommandHelper.ExecuteNonQuery(
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);

			return rowsAffected;

		}

		/// <summary>
		/// Deletes a row from the mp_EmailSendQueue table. Returns true if row deleted.
		/// </summary>
		/// <param name="guid"> guid </param>
		/// <returns>bool</returns>
		public static bool Delete(Guid guid)
		{
			string sqlCommand = @"
DELETE FROM mp_EmailSendQueue 
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
				ConnectionString.GetWriteConnectionString(),
				sqlCommand.ToString(),
				arParams);
			return rowsAffected > 0;

		}

		/// <summary>
		/// Gets an IDataReader with rows from the mp_EmailSendQueue table where DateToSend >= CurrentTime.
		/// </summary>
		/// <param name="currentTime"> currentTime </param>
		public static IDataReader GetEmailToSend(DateTime currentTime)
		{
			string sqlCommand = @"
SELECT * 
FROM mp_EmailSendQueue 
WHERE DateToSend >= ?CurrentTime;";

			var arParams = new List<MySqlParameter>
			{
				new("?CurrentTime", MySqlDbType.DateTime)
				{
					Direction = ParameterDirection.Input,
					Value = currentTime
				}
			};


			return CommandHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				sqlCommand.ToString(),
				arParams);

		}


	}
}
