using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace mojoPortal.Core.Helpers
{
	public static class XmlSanitizer
	{
		#region Public Methods

		public static string RemoveScripts(string xmlString)
		{
			var document = XDocument.Parse(xmlString);
			var children = document.Elements();

			RecursiveRemoveByTagName(children.FirstOrDefault(), "script");

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

		private static void RecursiveRemoveByTagName(XElement element, string tagName)
		{
			var hasChildren = element.Elements().Count() > 0;
			var firstChild = element.Elements().FirstOrDefault();
			var hasSiblings = element.ElementsAfterSelf().Count() > 0;
			var nextSibling = element.ElementsAfterSelf().FirstOrDefault();

			if (element == null)
			{
				return;
			}

			if (element.Name.LocalName == tagName)
			{
				element.Remove();
			}
			else
			{
				if (hasChildren)
				{
					RecursiveRemoveByTagName(firstChild, tagName);
				}
			}

			if (hasSiblings)
			{
				RecursiveRemoveByTagName(nextSibling, tagName);
			}
		}

		#endregion
	}
}
