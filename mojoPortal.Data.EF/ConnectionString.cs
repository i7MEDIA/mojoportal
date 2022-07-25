//https://www.pmichaels.net/2018/09/09/readonly-entity-framework/

using Newtonsoft.Json;
using System.IO;
using System.Web;
using mojoPortal.Data;

namespace mojoPortal.Data.EF
{
	public static class ConnectionString
	{
		public static string GetReadConnectionString()
		{
			var strings = GetConnectionStrings();

			return !string.IsNullOrWhiteSpace(strings.ReadOnlyConnectionString) ?
				strings.ReadOnlyConnectionString :
				strings.ConnectionString;
		}


		/// <summary>
		/// Gets the connection string for write.
		/// </summary>
		/// <returns>string</returns>
		public static string GetWriteConnectionString()
		{
			var strings = GetConnectionStrings();

			return strings.ConnectionString;
		}


		private static ConnectionStringsJson GetConnectionStrings()
		{
			var path = HttpContext.Current.Server.MapPath("~/App_Data/ConnectionStrings.json");

			if (File.Exists(path))
			{
				var file = File.ReadAllText(path);

				return JsonConvert.DeserializeObject<ConnectionStringsJson>(file);
			}
			else
			{
				var write = Data.ConnectionString.GetReadConnectionString();
				var read = Data.ConnectionString.GetWriteConnectionString();

				var connectionStrings = new ConnectionStringsJson
				{
					ConnectionString = write,
					ReadOnlyConnectionString = read == write ? string.Empty : read
				};

				File.WriteAllText(path, JsonConvert.SerializeObject(connectionStrings, Formatting.Indented));

				return connectionStrings;
			}
		}
	}

	public class ConnectionStringsJson
	{
		public string ConnectionString { get; set; }
		public string ReadOnlyConnectionString { get; set; } = null;
	}
}