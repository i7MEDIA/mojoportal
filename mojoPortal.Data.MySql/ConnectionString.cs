using System.Configuration;


namespace mojoPortal.Data
{
	public static class ConnectionString
	{
		private const string connectionString = "MySqlConnectionString";
		private const string writeString = "MySqlWriteConnectionString";


		public static string GetReadConnectionString() => GetString();


		public static string GetWriteConnectionString() => GetString("write");


		private static string GetString(string type = "")
		{
			if (type == "write" && ConfigurationManager.AppSettings[writeString] != null)
			{
				return ConfigurationManager.AppSettings[writeString].AddSslMode();
			}

			return ConfigurationManager.AppSettings[connectionString].AddSslMode();
		}


		private static string AddSslMode(this string cstring)
		{
			return cstring.Contains("SslMode")
				? cstring
				: cstring.TrimEnd(';') + ";SslMode=none;";
		}
	}
}