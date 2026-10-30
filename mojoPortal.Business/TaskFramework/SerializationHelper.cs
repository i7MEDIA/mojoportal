using System.IO;
using System.Xml.Serialization;

namespace mojoPortal.Business;

/// <summary>
///
/// </summary>
public class SerializationHelper
{
	public const string XmlDeclarationHeader = """<?xml version="1.0" encoding="utf-16"?>""";

	private SerializationHelper() { }

	public static string RemoveXmlDeclaration(string inputXml)
	{
		if (inputXml == null)
		{
			return null;
		}

		return inputXml.Replace(XmlDeclarationHeader, string.Empty);
	}

	public static string RestoreXmlDeclaration(string inputXmlWithNoHeader)
	{
		if (inputXmlWithNoHeader == null)
		{
			return null;
		}

		if (inputXmlWithNoHeader.StartsWith(XmlDeclarationHeader))
		{
			return inputXmlWithNoHeader;
		}

		return XmlDeclarationHeader + inputXmlWithNoHeader;
	}

	/// <summary>
	/// Deserializes an object from an xml string
	/// </summary>
	/// <param name="type"></param>
	/// <param name="serializedObject"></param>
	/// <returns></returns>
	public static object DeserializeFromString(Type type, string serializedObject)
	{
		if (type == null)
		{
			throw new ArgumentNullException(nameof(type));
		}

		if (string.IsNullOrEmpty(serializedObject))
		{
			return null;
		}

		using var streamReader = new StringReader(serializedObject);
		using var reader = XmlHelper.CreateXmlReader(streamReader);
		var serializer = new XmlSerializer(type);

		return serializer.Deserialize(reader);
	}

	/// <summary>
	/// Serializes a serializable object to xml string
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public static string SerializeToString(object obj)
	{
		// should we use an XmlWriter here?
		using StringWriter writer = new StringWriter();
		var serializer = new XmlSerializer(obj.GetType());
		var ns = new XmlSerializerNamespaces();

		ns.Add("", "");

		serializer.Serialize(writer, obj, ns);

		return writer.ToString();
	}
}
