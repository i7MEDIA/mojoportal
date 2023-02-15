using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace mojoPortal.Core.Helpers
{
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
			XmlDocument xmlDoc = new();

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
			XmlDocument xmlDoc = new();

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
			XmlDocument xmlDoc = new();

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
			XmlElement elem = xmlDoc.CreateElement(name);
			XmlText text = xmlDoc.CreateTextNode(content);
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

			StringWriter stringWriter = new StringWriter();
			try
			{
				XmlTextReader xmlReader = new XmlTextReader(xmlUrl);
				XslCompiledTransform xslTransform = new XslCompiledTransform();
				xslTransform.Load(xslUrl);

				XPathDocument xPathDocument = new XPathDocument(xmlReader);

				xslTransform.Transform(xPathDocument, null, stringWriter);
				return stringWriter.ToString();
			}
			catch (XsltCompileException ex)
			{
				log.Info("swallowed exception for xml path " + xmlUrl + " and xsl path " + xslUrl, ex);
			}
			catch (System.Security.SecurityException ex)
			{
				log.Info("swallowed exception for xml path " + xmlUrl + " and xsl path " + xslUrl, ex);
			}

			return string.Empty;

		}

		// 2013-04-15 added these helper extensions 
		//http://weblogs.asp.net/bleroy/archive/2013/04/14/a-c-helper-to-read-and-write-xml-from-and-to-objects.aspx
		//https://gist.github.com/bleroy/5384405
		public static string Attr(this XElement el, string name)
		{
			var attr = el.Attribute(name);
			return attr == null ? null : attr.Value;
		}

		public static XElement Attr<T>(this XElement el, string name, T value)
		{
			el.SetAttributeValue(name, value);
			return el;
		}

		public static XElement FromAttr<TTarget, TProperty>(this XElement el, TTarget target,
															Expression<Func<TTarget, TProperty>> targetExpression)
		{
			var memberExpression = targetExpression.Body as MemberExpression;
			if (memberExpression == null) throw new InvalidOperationException("Expression is not a member expression.");
			var propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null) throw new InvalidOperationException("Expression is not for a property.");
			var name = propertyInfo.Name;
			var attr = el.Attribute(name);
			if (attr == null) return el;
			if (typeof(TProperty) == typeof(string))
			{
				propertyInfo.SetValue(target, (string)attr, null);
				return el;
			}
			if (attr.Value == "null")
			{
				propertyInfo.SetValue(target, null, null);
			}
			else if (typeof(TProperty) == typeof(int))
			{
				propertyInfo.SetValue(target, (int)attr, null);
			}
			else if (typeof(TProperty) == typeof(bool))
			{
				propertyInfo.SetValue(target, (bool)attr, null);
			}
			else if (typeof(TProperty) == typeof(DateTime))
			{
				propertyInfo.SetValue(target, (DateTime)attr, null);
			}
			else if (typeof(TProperty) == typeof(double))
			{
				propertyInfo.SetValue(target, (double)attr, null);
			}
			else if (typeof(TProperty) == typeof(float))
			{
				propertyInfo.SetValue(target, (float)attr, null);
			}
			else if (typeof(TProperty) == typeof(decimal))
			{
				propertyInfo.SetValue(target, (decimal)attr, null);
			}
			else if (typeof(TProperty) == typeof(int?))
			{
				propertyInfo.SetValue(target, (int?)attr, null);
			}
			else if (typeof(TProperty) == typeof(bool?))
			{
				propertyInfo.SetValue(target, (bool?)attr, null);
			}
			else if (typeof(TProperty) == typeof(DateTime?))
			{
				propertyInfo.SetValue(target, (DateTime?)attr, null);
			}
			else if (typeof(TProperty) == typeof(double?))
			{
				propertyInfo.SetValue(target, (double?)attr, null);
			}
			else if (typeof(TProperty) == typeof(float?))
			{
				propertyInfo.SetValue(target, (float?)attr, null);
			}
			else if (typeof(TProperty) == typeof(decimal?))
			{
				propertyInfo.SetValue(target, (decimal?)attr, null);
			}
			return el;
		}

		public static XElement ToAttr<TTarget, TProperty>(this XElement el, TTarget target,
														  Expression<Func<TTarget, TProperty>> targetExpression)
		{
			var memberExpression = targetExpression.Body as MemberExpression;
			if (memberExpression == null) throw new InvalidOperationException("Expression is not a member expression.");
			var propertyInfo = memberExpression.Member as PropertyInfo;
			if (propertyInfo == null) throw new InvalidOperationException("Expression is not for a property.");
			var name = propertyInfo.Name;
			var val = propertyInfo.GetValue(target, null);
			if (typeof(TProperty) == typeof(string))
			{
				el.Attr(name, (string)val);
				return el;
			}
			if (val == null)
			{
				el.Attr(name, "null");
			}
			else if (typeof(TProperty) == typeof(int))
			{
				el.Attr(name, (int)val);
			}
			else if (typeof(TProperty) == typeof(bool))
			{
				el.Attr(name, (bool)val);
			}
			else if (typeof(TProperty) == typeof(DateTime))
			{
				el.Attr(name, (DateTime)val);
			}
			else if (typeof(TProperty) == typeof(double))
			{
				el.Attr(name, (double)val);
			}
			else if (typeof(TProperty) == typeof(float))
			{
				el.Attr(name, (float)val);
			}
			else if (typeof(TProperty) == typeof(decimal))
			{
				el.Attr(name, (decimal)val);
			}
			else if (typeof(TProperty) == typeof(int?))
			{
				el.Attr(name, (int?)val);
			}
			else if (typeof(TProperty) == typeof(bool?))
			{
				el.Attr(name, (bool?)val);
			}
			else if (typeof(TProperty) == typeof(DateTime?))
			{
				el.Attr(name, (DateTime?)val);
			}
			else if (typeof(TProperty) == typeof(double?))
			{
				el.Attr(name, (double?)val);
			}
			else if (typeof(TProperty) == typeof(float?))
			{
				el.Attr(name, (float?)val);
			}
			else if (typeof(TProperty) == typeof(decimal?))
			{
				el.Attr(name, (decimal?)val);
			}
			return el;
		}

		public static XElementWithContext<TContext> With<TContext>(this XElement el, TContext context)
		{
			return new XElementWithContext<TContext>(el, context);
		}


		public static StringBuilder GetKeyValuePairsAsStringBuilder(XmlNodeList nodes)
		{
			StringBuilder sb = new StringBuilder();
			foreach (XmlNode node in nodes)
			{
				XmlAttributeCollection attribs = node.Attributes;
				if (attribs["name"] != null)
				{
					if (!String.IsNullOrWhiteSpace(attribs["name"].Value))
					{
						string opValue = " ";
						if (attribs["value"] != null && !String.IsNullOrWhiteSpace(attribs["value"].Value))
						{
							opValue = attribs["value"].Value;
						}
						string option = attribs["name"].Value + "|" + opValue;
						sb.Append(option + ";");
					}
				}
			}

			return sb;
		}

		public static Dictionary<string, string> GetKeyValuePairs(XmlNodeList nodes)
		{
			Dictionary<string, string> dic = new Dictionary<string, string>();

			foreach (XmlNode node in nodes)
			{
				XmlAttributeCollection attribs = node.Attributes;
				if (attribs["name"] != null)
				{
					if (!String.IsNullOrWhiteSpace(attribs["name"].Value))
					{
						string opValue = " ";
						if (attribs["value"] != null && !String.IsNullOrWhiteSpace(attribs["value"].Value))
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

	public class XElementWithContext<TContext>
	{
		public XElementWithContext(XElement element, TContext context)
		{
			Element = element;
			Context = context;
		}

		public XElement Element { get; private set; }
		public TContext Context { get; private set; }

		public XElementWithContext<TNewContext> With<TNewContext>(TNewContext context)
		{
			return new XElementWithContext<TNewContext>(Element, context);
		}

		public XElementWithContext<TContext> ToAttr<TProperty>(
			Expression<Func<TContext, TProperty>> targetExpression)
		{
			Element.ToAttr(Context, targetExpression);
			return this;
		}

		public XElementWithContext<TContext> FromAttr<TProperty>(
			Expression<Func<TContext, TProperty>> targetExpression)
		{
			Element.FromAttr(Context, targetExpression);
			return this;
		}
	}


	


}
