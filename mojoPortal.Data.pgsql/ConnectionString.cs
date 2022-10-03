using System;
using System.Configuration;


namespace mojoPortal.Data
{
	public static class ConnectionString
	{
		private const string connectionString = "PostgreSQLConnectionString";
		private const string writeString = "PostgreSQLWriteConnectionString";


		public static String GetReadConnectionString() => ConfigurationManager.AppSettings[connectionString];


		public static String GetWriteConnectionString()
		{
			if (ConfigurationManager.AppSettings[writeString] != null)
			{
				return ConfigurationManager.AppSettings[writeString];
			}

			return ConfigurationManager.AppSettings[connectionString];
		}
	}
}