using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace mojoPortal.Web.Framework
{
	public static class XmlHelper
	{
		//private static readonly ILog log = LogManager.GetLogger(typeof(XmlHelper));

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static void AddNode(XmlDocument xmlDoc, string name, string content)
		{
			Core.Helpers.XmlHelper.AddNode(xmlDoc, name, content);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static string TransformXML(string xmlUrl, string xslUrl)
		{
			return Core.Helpers.XmlHelper.TransformXML(xmlUrl, xslUrl);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]

		public static string Attr(this XElement el, string name)
		{
			return Core.Helpers.XmlHelper.Attr(el, name);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static XElement Attr<T>(this XElement el, string name, T value)
		{
			return Core.Helpers.XmlHelper.Attr<T>(el, name, value);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static XElement FromAttr<TTarget, TProperty>(this XElement el, TTarget target,
															Expression<Func<TTarget, TProperty>> targetExpression)
		{
			return Core.Helpers.XmlHelper.FromAttr<TTarget, TProperty>(el, target, targetExpression);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static XElement ToAttr<TTarget, TProperty>(this XElement el, TTarget target,
														  Expression<Func<TTarget, TProperty>> targetExpression)
		{
			return Core.Helpers.XmlHelper.ToAttr<TTarget, TProperty>(el,target, targetExpression);
		}

		//public static XElementWithContext<TContext> With<TContext>(this XElement el, TContext context)
		//{
		//	return new XElementWithContext<TContext>(el, context);
		//}

		//public class XElementWithContext<TContext>
		//{
		//	public XElementWithContext(XElement element, TContext context)
		//	{
		//		Element = element;
		//		Context = context;
		//	}

		//	public XElement Element { get; private set; }
		//	public TContext Context { get; private set; }

		//	public XElementWithContext<TNewContext> With<TNewContext>(TNewContext context)
		//	{
		//		return new XElementWithContext<TNewContext>(Element, context);
		//	}

		//	public XElementWithContext<TContext> ToAttr<TProperty>(
		//		Expression<Func<TContext, TProperty>> targetExpression)
		//	{
		//		Element.ToAttr(Context, targetExpression);
		//		return this;
		//	}

		//	public XElementWithContext<TContext> FromAttr<TProperty>(
		//		Expression<Func<TContext, TProperty>> targetExpression)
		//	{
		//		Element.FromAttr(Context, targetExpression);
		//		return this;
		//	}
		//}
		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static StringBuilder GetKeyValuePairsAsStringBuilder(XmlNodeList nodes)
		{
			return Core.Helpers.XmlHelper.GetKeyValuePairsAsStringBuilder(nodes);
		}

		[Obsolete("Use mojoPortal.Core.Helpers.XmlHelper")]
		public static Dictionary<string, string> GetKeyValuePairs(XmlNodeList nodes)
		{
			return Core.Helpers.XmlHelper.GetKeyValuePairs(nodes);
		}
	}
}
