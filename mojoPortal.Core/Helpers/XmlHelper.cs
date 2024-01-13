using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using log4net;

namespace mojoPortal.Core.Helpers;

public static class XmlHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(XmlHelper));

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

	/// <summary>
	/// transforms xml from a given url using xsl from a given url
	/// returns an empty string and logs an error if an error occurs
	/// </summary>
	/// <param name="xmlUrl"></param>
	/// <param name="xslUrl"></param>
	/// <returns></returns>
	public static string TransformXML(string xmlUrl, string xslUrl)
	{
		if (string.IsNullOrEmpty(xmlUrl)) { throw new ArgumentException("xmlUrl is required"); }
		if (string.IsNullOrEmpty(xslUrl)) { throw new ArgumentException("xslUrl is required"); }

		var stringWriter = new StringWriter();
		try
		{
			var xmlReader = new XmlTextReader(xmlUrl);
			var xslTransform = new XslCompiledTransform();
			xslTransform.Load(xslUrl);

			var xPathDocument = new XPathDocument(xmlReader);

			xslTransform.Transform(xPathDocument, null, stringWriter);
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
					string opValue = " ";
					if (attribs["value"] != null && !string.IsNullOrWhiteSpace(attribs["value"].Value))
					{
						opValue = attribs["value"].Value;
					}

					string optGroup = string.Empty;
					if (attribs["optgroup"] != null && !string.IsNullOrWhiteSpace(attribs["optgroup"].Value))
					{
						optGroup = $"^{attribs["optgroup"].Value}";
					}
					string option = $"{attribs["name"].Value}|{opValue}{optGroup}";
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
					string opValue = " ";
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