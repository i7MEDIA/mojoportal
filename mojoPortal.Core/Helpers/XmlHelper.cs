using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace mojoPortal.Core.Helpers;

public static class XmlHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(XmlHelper));

	public static XmlDocument CreateXmlDocument(bool EnableResolver = false)
	{
		var xmlDoc = new XmlDocument();

		if (!EnableResolver)
		{
			//prevent XXE (https://portswigger.net/web-security/xxe)
			xmlDoc.XmlResolver = null;
		}

		return xmlDoc;
	}


	/// <summary>
	/// Loads XML Document from stream and disables XmlResolver by default to prevent XXE
	/// </summary>
	/// <param name="stream"></param>
	/// <returns>System.Xml.XmlDocument</returns>
	public static XmlDocument GetXmlDocument(Stream stream, bool EnableResolver = false)
	{
		var xmlDoc = new XmlDocument();

		if (!EnableResolver)
		{
			//prevent XXE (https://portswigger.net/web-security/xxe)
			xmlDoc.XmlResolver = null;
		}

		using (stream)
		{
			xmlDoc.Load(stream);
		}

		return xmlDoc;
	}


	/// <summary>
	/// Loads XML Document from file path and disables XmlResolver by default to prevent XXE
	/// </summary>
	/// <param name="fileName"></param>
	/// <param name="EnableResolver"></param>
	/// <returns>System.Xml.XmlDocument</returns>
	public static XmlDocument GetXmlDocument(string fileName, bool EnableResolver = false)
	{
		var xmlDoc = new XmlDocument();

		if (!EnableResolver)
		{
			//prevent XXE (https://portswigger.net/web-security/xxe)
			xmlDoc.XmlResolver = null;
		}

		xmlDoc.Load(fileName);

		return xmlDoc;
	}


	/// <summary>
	/// Loads XML Document from string of XML and disables XmlResolver by default to prevent XXE
	/// </summary>
	/// <param name="xmlString"></param>
	/// <param name="EnableResolver"></param>
	/// <returns>System.Xml.XmlDocument</returns>
	public static XmlDocument GetXmlDocumentFromString(string xmlString, bool EnableResolver = false)
	{
		var xmlDoc = new XmlDocument();

		if (!EnableResolver)
		{
			//prevent XXE (https://portswigger.net/web-security/xxe)
			xmlDoc.XmlResolver = null;
		}

		xmlDoc.LoadXml(xmlString);

		return xmlDoc;
	}


	public static void AddNode(XmlDocument xmlDoc, string name, string content)
	{
		var elem = xmlDoc.CreateElement(name);
		var text = xmlDoc.CreateTextNode(content);

		xmlDoc.DocumentElement.AppendChild(elem);
		xmlDoc.DocumentElement.LastChild.AppendChild(text);
	}


	private static bool IsSafeIpAddress(IPAddress ip)
	{
		// Joe queries as to what should be done when we do want to explicitly use a local resource intentionally?
		var bytes = ip.GetAddressBytes();

		// IPv4 Checks
		if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
		{
			// Allow for development testing
			#if !DEBUG
			// Loopback: 127.0.0.0/8
			if (bytes[0] == 127)
			{
				return false;
			}
			#endif

			// Link-Local / Cloud Metadata: 169.254.0.0/16
			if (bytes[0] == 169 && bytes[1] == 254)
			{
				return false;
			}

			// RFC 1918 Private: 10.0.0.0/8
			if (bytes[0] == 10)
			{
				return false;
			}

			// RFC 1918 Private: 172.16.0.0/12
			if (bytes[0] == 172 && bytes[1] >= 16 && bytes[1] <= 31)
			{
				return false;
			}

			// RFC 1918 Private: 192.168.0.0/16
			if (bytes[0] == 192 && bytes[1] == 168)
			{
				return false;
			}
		}

		// IPv6 Checks
		else if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
		{
			// Loopback: ::1
			if (IPAddress.IsLoopback(ip))
			{
				return false;
			}

			// Link-local: fe80::/10
			if (bytes[0] == 0xfe && (bytes[1] & 0xc0) == 0x80)
			{
				return false;
			}

			// Unique Local (Private IPv6): fc00::/7
			if ((bytes[0] & 0xfe) == 0xfc)
			{
				return false;
			}
		}

		return true;
	}


	public static string SecureFetch(string url, int redirectCount = 0)
	{
		if (redirectCount > 5)
		{
			throw new Exception("Too many redirects.");
		}

		var uri = new Uri(url);

		if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
		{
			throw new Exception("Invalid URL scheme. Only HTTP and HTTPS are allowed.");
		}

		var ips = Dns.GetHostAddresses(uri.DnsSafeHost);
		// This will return true for loopback (localhost) in the debug environment to allow for development testing
		var targetIp = ips.FirstOrDefault(IsSafeIpAddress);

		if (targetIp == null)
		{
			throw new Exception("Target resolves to a restricted internal or private IP address.");
		}

		// Prevent DNS Rebinding by targeting the IP directly
		var directUri = new UriBuilder(uri)
		{
			Host = targetIp.ToString()
		};

		var request = (HttpWebRequest)WebRequest.Create(directUri.Uri);

		request.Host = uri.Host; // Preserve original Host header
		request.AllowAutoRedirect = false;
		request.Proxy = null; // Ensure local proxy fallbacks are bypassed

		try
		{
			using var response = (HttpWebResponse)request.GetResponse();

			// Handle Redirects securely
			var statusCode = (int)response.StatusCode;

			if (statusCode >= 300 && statusCode <= 399)
			{
				var location = response.Headers["Location"];

				if (string.IsNullOrEmpty(location))
				{
					throw new Exception("Redirect missing Location header.");
				}

				var nextUri = new Uri(uri, location);

				return SecureFetch(nextUri.ToString(), redirectCount + 1);
			}

			using var reader = new StreamReader(response.GetResponseStream());

			return reader.ReadToEnd();
		}
		catch (WebException)
		{
			throw new Exception("Failed to securely fetch the remote resource.");
		}
	}


	/// <summary>
	/// transforms xml from a given url using xsl from a given url
	/// returns an empty string and logs an error if an error occurs
	/// </summary>
	/// <param name="xmlUrl"></param>
	/// <param name="xslUrl"></param>
	/// <returns></returns>
	public static string TransformXML(string xmlUrl, string xslUrl)
	{
		if (string.IsNullOrEmpty(xmlUrl))
		{
			throw new ArgumentException("xmlUrl is required");
		}

		if (string.IsNullOrEmpty(xslUrl))
		{
			throw new ArgumentException("xslUrl is required");
		}

		var stringWriter = new StringWriter();

		try
		{
			var safeXmlContent = SecureFetch(xmlUrl);
			var safeXslContent = SecureFetch(xslUrl);

			var settings = new XmlReaderSettings
			{
				DtdProcessing = DtdProcessing.Prohibit,
				XmlResolver = null
			};

			using (var stringReader = new StringReader(safeXmlContent))
			using (var xmlReader = XmlReader.Create(stringReader, settings))
			{
				var xslSettings = XsltSettings.Default;
				var xslTransform = new XslCompiledTransform();

				using (var xslStringReader = new StringReader(safeXslContent))
				using (var xslReader = XmlReader.Create(xslStringReader, settings))
				{
					xslTransform.Load(xslReader, xslSettings, null);
				}

				var xPathDocument = new XPathDocument(xmlReader);

				xslTransform.Transform(xPathDocument, null, stringWriter);
			}

			return stringWriter.ToString();
		}
		catch (XsltCompileException ex)
		{
			log.Info($"swallowed exception for xml path {xmlUrl} and xsl path {xslUrl}", ex);
		}
		catch (System.Security.SecurityException ex)
		{
			log.Info($"swallowed exception for xml path {xmlUrl} and xsl path {xslUrl}", ex);
		}

		return string.Empty;
	}


	public static StringBuilder GetKeyValuePairsAsStringBuilder(XmlNodeList nodes)
	{
		var sb = new StringBuilder();

		foreach (XmlNode node in nodes)
		{
			var attribs = node.Attributes;

			if (attribs["name"] != null)
			{
				if (!string.IsNullOrWhiteSpace(attribs["name"].Value))
				{
					var opValue = " ";

					if (attribs["value"] != null && !string.IsNullOrWhiteSpace(attribs["value"].Value))
					{
						opValue = attribs["value"].Value;
					}

					var optGroup = string.Empty;

					if (attribs["optgroup"] != null && !string.IsNullOrWhiteSpace(attribs["optgroup"].Value))
					{
						optGroup = $"^{attribs["optgroup"].Value}";
					}

					var option = $"{attribs["name"].Value}|{opValue}{optGroup}";

					sb.Append(option + ";");
				}
			}
		}

		return sb;
	}


	public static Dictionary<string, string> GetKeyValuePairs(XmlNodeList nodes)
	{
		var dic = new Dictionary<string, string>();

		foreach (XmlNode node in nodes)
		{
			var attribs = node.Attributes;

			if (attribs["name"] != null)
			{
				if (!string.IsNullOrWhiteSpace(attribs["name"].Value))
				{
					var opValue = " ";

					if (attribs["value"] != null && !string.IsNullOrWhiteSpace(attribs["value"].Value))
					{
						opValue = attribs["value"].Value;
					}

					dic.Add(attribs["name"].Value, opValue);
				}
			}
		}

		return dic;
	}
}
