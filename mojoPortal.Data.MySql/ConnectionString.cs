using System.Configuration;

namespace mojoPortal.Data
{
	public static class ConnectionString
	{
		private const string StringName = "MySqlConnectionString";
		private const string WriteStringName = "MySqlWriteConnectionString";


		public static string GetReadConnectionString()
		{
			return GetString();
		}


		public static string GetWriteConnectionString()
		{
			return GetString("write") ?? GetString();
		}


		private static string GetString(string type = "get")
		{
			switch (type)
			{
				default:
					return ConfigurationManager.AppSettings[StringName].AddSslMode();
				case "write":
					return ConfigurationManager.AppSettings[WriteStringName].AddSslMode();
			}
		}


		private static string AddSslMode(this string cstring)
		{
			return cstring.Contains("SslMode")
				? cstring
				: cstring.TrimEnd(';') + ";SslMode=none;";
		}
	}
}