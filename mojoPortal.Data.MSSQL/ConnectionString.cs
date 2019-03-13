using System.Configuration;


namespace mojoPortal.Data
{
	public static class ConnectionString
	{
		private const string connectionString = "MSSQLConnectionString";
		private const string writeString = "MSSQLWriteConnectionString";

		public static string GetReadConnectionString()
		{
			if (UseConnectionStringSection())
			{
				return GetReadConnectionStringFromConnectionStringSection();
			}

			return ConfigurationManager.AppSettings[connectionString];
		}


		/// <summary>
		/// Gets the connection string for write.
		/// </summary>
		/// <returns>string</returns>
		public static string GetWriteConnectionString()
		{
			if (UseConnectionStringSection())
			{
				return GetWriteConnectionStringFromConnectionStringSection();
			}

			if (ConfigurationManager.AppSettings[writeString] != null)
			{
				return ConfigurationManager.AppSettings[writeString];
			}

			return ConfigurationManager.AppSettings[connectionString];
		}


		private static string GetWriteConnectionStringFromConnectionStringSection()
		{
			if (ConfigurationManager.ConnectionStrings[writeString] != null)
			{
				return ConfigurationManager.ConnectionStrings[writeString].ConnectionString;
			}

			return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
		}


		private static string GetReadConnectionStringFromConnectionStringSection()
		{
			return ConfigurationManager.ConnectionStrings[connectionString].ConnectionString;
		}


		private static bool UseConnectionStringSection()
		{
			var connectionStringSection = ConfigurationManager.AppSettings["UseConnectionStringSection"];

			if (connectionStringSection != null && connectionStringSection == "true")
			{
				return true;
			}

			return false;
		}
	}
}