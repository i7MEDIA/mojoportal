using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace mojoPortal.Data
{
	public static class ConnectionString
	{
		private const string connectionString = "PostgreSQLConnectionString";
		private const string writeString = "PostgreSQLWriteConnectionString";

		public static string CleanConnectionString(string connStr)
		{
			if (string.IsNullOrWhiteSpace(connStr))
			{
				return connStr;
			}

			// Remove unsupported 'Encoding=...' parameter so legacy connection strings in user.config don't fail in Npgsql 8.x
			return Regex.Replace(connStr, @"(?i)\bencoding\s*=\s*[^;]+;?", string.Empty).Trim();
		}

		public static string GetReadConnectionString() => CleanConnectionString(ConfigurationManager.AppSettings[connectionString]);

		public static string GetWriteConnectionString()
		{
			if (ConfigurationManager.AppSettings[writeString] != null)
			{
				return CleanConnectionString(ConfigurationManager.AppSettings[writeString]);
			}

			return CleanConnectionString(ConfigurationManager.AppSettings[connectionString]);
		}
	}
}