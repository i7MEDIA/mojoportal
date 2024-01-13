using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using mojoPortal.Core.Helpers;

namespace mojoPortal.Core.Extensions;

public static class XmlExtensions
{
	public static int ParseInt32FromAttribute(this XmlAttributeCollection attribs, string attrib, int defaultIfNotFound)
	{
		int returnValue = defaultIfNotFound;
		if (attribs is null) { return returnValue; }

		if (attrib is not null
			&& attribs[attrib] is not null
			&& !int.TryParse(attribs[attrib].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out returnValue))
		{
			returnValue = defaultIfNotFound;
		}

		return returnValue;
	}

	public static bool ParseBoolFromAttribute(this XmlAttributeCollection attribs, string attrib, bool defaultIfNotFound)
	{
		bool returnValue = defaultIfNotFound;

		if (attribs is not null
			&& attrib is not null
			&& attribs[attrib] is not null)
		{
			if (string.Equals(attribs[attrib].Value, "true", StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}

			return false;
		}

		return returnValue;
	}

	public static string ParseStringFromAttribute(this XmlAttributeCollection attribs, string attrib, string defaultIfNotFound)
	{
		//string returnValue = defaultIfNotFound;
		if (attribs is null) { return defaultIfNotFound; }

		if (attrib is not null
			&& attribs[attrib] is not null)
		{
			return attribs[attrib].Value;
		}

		return defaultIfNotFound;
	}

	public static Guid ParseGuidFromAttribute(this XmlAttributeCollection attribs, string attrib, Guid defaultIfNotFound)
	{
		if (attribs is null) { return defaultIfNotFound; }

		if (attrib is not null && attribs[attrib] is not null)
		{
			if (Guid.TryParse(attribs[attrib].Value, out Guid result))
			{
				return result;
			}
		}

		return defaultIfNotFound;
	}

	// 2013-04-15 added these helper extensions 
	//http://weblogs.asp.net/bleroy/archive/2013/04/14/a-c-helper-to-read-and-write-xml-from-and-to-objects.aspx
	//https://gist.github.com/bleroy/5384405
	public static string Attr(this XElement el, string name)
	{
		var attr = el.Attribute(name);
		return attr?.Value;
	}

	public static XElement Attr<T>(this XElement el, string name, T value)
	{
		el.SetAttributeValue(name, value);
		return el;
	}

	public static XElement FromAttr<TTarget, TProperty>(this XElement el, TTarget target,
														Expression<Func<TTarget, TProperty>> targetExpression)
	{
		if (targetExpression.Body is not MemberExpression memberExpression)
		{
			throw new InvalidOperationException("Expression is not a member expression.");
		}

		var propertyInfo = memberExpression.Member as PropertyInfo ?? throw new InvalidOperationException("Expression is not for a property.");
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

	public static XElement ToAttr<TTarget, TProperty>(
		this XElement el,
		TTarget target,
		Expression<Func<TTarget, TProperty>> targetExpression)
	{
		if (targetExpression.Body is not MemberExpression memberExpression)
		{
			throw new InvalidOperationException("Expression is not a member expression.");
		}

		var propertyInfo = memberExpression.Member as PropertyInfo ?? throw new InvalidOperationException("Expression is not for a property.");
		var name = propertyInfo.Name;
		var val = propertyInfo.GetValue(target, null);
		if (typeof(TProperty) == typeof(string))
		{
			el.Attr(name, (string)val);
			return el;
		}
		if (val is null)
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
}
