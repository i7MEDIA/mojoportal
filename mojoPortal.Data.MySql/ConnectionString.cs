using System;
using System.Configuration;

namespace mojoPortal.Data;

public static class ConnectionString
{
	private const string connectionString = "MySqlConnectionString";
	private const string writeString = "MySqlWriteConnectionString";

	public static string GetRead() => GetString();
	public static string GetWrite() => GetString("write");

	[Obsolete("Use GetRead()")]
	public static string GetReadConnectionString() => GetRead();
	[Obsolete("Use GetWrite()")]
	public static string GetWriteConnectionString() => GetWrite();


	private static string GetString(string type = "")
	{
		if (type == "write" && ConfigurationManager.AppSettings[writeString] is not null)
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