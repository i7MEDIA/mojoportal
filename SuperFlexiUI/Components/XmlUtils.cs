// Author:					i7MEDIA
// Created:					2015-3-6
// Last Modified:			2015-3-26
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Xml;

namespace SuperFlexiUI
{
	public class XmlUtils
	{
		public static int ParseInt32FromAttribute(XmlAttributeCollection attribs, string attrib, int defaultIfNotFound)
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

		public static bool ParseBoolFromAttribute(XmlAttributeCollection attribs, String attrib, bool defaultIfNotFound)
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

		public static string ParseStringFromAttribute(XmlAttributeCollection attribs, string attrib, string defaultIfNotFound)
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
	}
}