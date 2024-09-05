using System.Net;

namespace mojoPortal.Business;

public static class IPAddressHelper
{
	/// <summary>
	/// Takes an ip address in standard ipv4 notation and converts to its long representation
	/// </summary>
	/// <param name="ipv4Address"></param>
	/// <returns></returns>
	public static Int64 ConvertToLong(string ipv4Address)
	{
		Int64 result = 0;
		if (ipv4Address.Contains(":")) { return result; } // an ipv6 address was passed instead

		if (IPAddress.TryParse(ipv4Address, out IPAddress ipAddress))
		{
			byte[] b = ipAddress.GetAddressBytes();
			if (b.Length >= 4) // prevent index out of range error
			{
				result = (long)(b[0] * 16777216);
				result += (long)(b[1] * 65536);
				result += (long)(b[2] * 256);
				result += (long)(b[3] * 1);
			}
		}

		return result;
	}

	/// <summary>
	/// Takes a long value and converts it to standard ipv4 notation
	/// </summary>
	public static string ConvertToIPAddressString(long ipAddressAsLong) => new IPAddress(ipAddressAsLong).ToString();
}