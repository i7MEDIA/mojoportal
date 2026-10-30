using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace mojoPortal.Core.Helpers;

public static class XmlSanitizer
{
	private static readonly HashSet<string> DangerousTags = new(StringComparer.OrdinalIgnoreCase)
	{
		"script",
		"foreignObject",
		"iframe",
		"object",
		"embed",
		"applet",
		"meta",
		"link"
	};

	#region Public Methods

	public static string RemoveScripts(string xmlString)
	{
		if (string.IsNullOrWhiteSpace(xmlString))
		{
			return string.Empty;
		}

		using var stringReader = new StringReader(xmlString);
		using var xmlReader = XmlHelper.CreateXmlReader(stringReader);
		var document = XDocument.Load(xmlReader);

		var elementsToRemove = document
			.Descendants()
			.Where(el => DangerousTags.Contains(el.Name.LocalName))
			.ToList();

		foreach (var el in elementsToRemove)
		{
			el.Remove();
		}

		foreach (var element in document.Descendants())
		{
			var attributesToRemove = element.Attributes()
				.Where(IsDangerousAttribute)
				.ToList();

			foreach (var attr in attributesToRemove)
			{
				attr.Remove();
			}
		}

		return document.ToString();
	}


	public static Stream RemoveScripts(Stream inputStream)
	{
		var fileText = new StreamReader(inputStream).ReadToEnd();
		var processed = RemoveScripts(fileText);
		var stream = new MemoryStream();
		var writer = new StreamWriter(stream);

		writer.Write(processed);
		writer.Flush();

		stream.Position = 0;

		return stream;
	}


	public static Stream RemoveScripts(HttpPostedFile file)
	{
		return RemoveScripts(file.InputStream);
	}

	#endregion


	#region Private Methods

	private static bool IsDangerousAttribute(XAttribute attribute)
	{
		var localName = attribute.Name.LocalName;

		if (localName.StartsWith("on", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}

		var normalizedValue = new string(attribute.Value.Where(c => !char.IsWhiteSpace(c) && !char.IsControl(c)).ToArray());

		if (
			localName.Equals("href", StringComparison.OrdinalIgnoreCase) ||
			localName.Equals("src", StringComparison.OrdinalIgnoreCase) ||
			localName.Equals("from", StringComparison.OrdinalIgnoreCase) ||
			localName.Equals("to", StringComparison.OrdinalIgnoreCase) ||
			localName.Equals("values", StringComparison.OrdinalIgnoreCase))
		{
			if (
				normalizedValue.StartsWith("javascript:", StringComparison.OrdinalIgnoreCase) ||
				normalizedValue.StartsWith("vbscript:", StringComparison.OrdinalIgnoreCase) ||
				normalizedValue.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}

		if (
			normalizedValue.IndexOf("javascript:", StringComparison.OrdinalIgnoreCase) >= 0 ||
			normalizedValue.IndexOf("vbscript:", StringComparison.OrdinalIgnoreCase) >= 0)
		{
			return true;
		}

		return false;
	}

	#endregion
}
