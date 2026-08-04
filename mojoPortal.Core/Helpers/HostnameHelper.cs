using System;
using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Core.Helpers;

public static class HostnameHelper
{
	/// <summary>
	/// Checks to see if a domain string has a valid DNS, IPv4 and IPv6 hostname
	/// </summary>
	/// <param name="value">The domain to parse</param>
	/// <returns></returns>
	public static bool IsValidHostName(string value)
	{
		var hostname = value.TrimEnd('/');

		if (hostname.Contains(":/"))
		{
			var index = hostname.IndexOf("://");

			if (index == -1)
			{
				index = hostname.IndexOf(":/") + 2;
			}
			else
			{
				index += 3;
			}

			hostname = hostname.Substring(index);
		}

		return Uri.CheckHostName(hostname) != UriHostNameType.Unknown;
	}


	public static List<string> ParseMultilineHostnameList(string domains) =>
		[.. domains
			.Split(['\r', '\n', ',', ';', ' '], StringSplitOptions.RemoveEmptyEntries)
			.Select(x => x.Trim())];


	/// <summary>
	/// Parses a list of domains by newline, comma, semicolon, or space, validates the hostname, and returns true if all items are valid.
	/// </summary>
	/// <param name="hostnameList">The string list of domain</param>
	/// <param name="validHostnames"></param>
	/// <param name="invalidHostnames"></param>
	/// <returns></returns>
	public static bool TryParseHostnameList(
		string hostnameList,
		out List<string> validHostnames,
		out List<string> invalidHostnames)
	{
		var hostnames = ParseMultilineHostnameList(hostnameList);
		var hasInvalidHostnames = false;

		validHostnames = [];
		invalidHostnames = [];

		foreach (var hostname in hostnames)
		{
			if (IsValidHostName(hostname))
			{
				validHostnames.Add(hostname);
			}
			else
			{
				invalidHostnames.Add(hostname);
				hasInvalidHostnames = true;
			}
		}

		if (hasInvalidHostnames)
		{
			return false;
		}

		return true;
	}
}
