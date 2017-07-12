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
            if (attribs == null) { return returnValue; }

            if (
                (attrib != null)
                && (attribs[attrib] != null)
                && (!Int32.TryParse(attribs[attrib].Value, NumberStyles.Any, CultureInfo.InvariantCulture, out returnValue))
                )
            {
                returnValue = defaultIfNotFound;
            }

            return returnValue;
        }

        public static bool ParseBoolFromAttribute(XmlAttributeCollection attribs, String attrib, bool defaultIfNotFound)
        {
            bool returnValue = defaultIfNotFound;

            if (
                (attribs != null)
                && (attrib != null)
                && (attribs[attrib] != null)
                )
            {
                if (string.Equals(attribs[attrib].Value, "true", StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }

                return false;
            }

            return returnValue;

        }
    }
}