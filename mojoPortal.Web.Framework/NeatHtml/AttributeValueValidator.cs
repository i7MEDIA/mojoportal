/*

NeatHtml- Helps prevent XSS attacks by validating HTML against a subset of XHTML.
Copyright (C) 2006  Dean Brettle

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Schema;
using System.Collections;
using System.Text.RegularExpressions;
using System.Diagnostics;

//namespace Brettle.Web.NeatHtml
namespace mojoPortal.Web.Framework
{
    internal class AttributeValueValidator
    {
        internal AttributeValueValidator() { }

        internal void Add(XmlDocument schemaDoc, XmlSchema schema, string typeName, string namespaceUri)
        {
            XmlSchemaSimpleType simpleType = schema.SchemaTypes[new XmlQualifiedName(typeName, namespaceUri)] as XmlSchemaSimpleType;
            XmlSchemaSimpleTypeRestriction typeRestriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;
            XmlSchemaPatternFacet pattern = typeRestriction.Facets[0] as XmlSchemaPatternFacet;
            Regex re = new Regex(pattern.Value);

            XmlNodeList attributeNameNodes = schemaDoc.SelectNodes("//*[@type='" + typeName + "']/@name");
            for (int i = 0; i < attributeNameNodes.Count; i++)
            {
                string attrName = attributeNameNodes.Item(i).Value;
                if (RegexForAttrName[attrName] != null)
                {
                    continue;
                }
                else
                {
                    RegexForAttrName[attrName] = re;
                    if (XPathPredicateOfAttributes == null)
                    {
                        XPathPredicateOfAttributes = "";
                    }
                    else
                    {
                        XPathPredicateOfAttributes += " or ";
                    }
                    XPathPredicateOfAttributes += "local-name() = '" + attrName + "'";
                }
            }
        }


        internal void Validate(XmlDocument doc)
        {
            string xpathOfAttributes = "/" + "/@*[" + XPathPredicateOfAttributes + "]";

            Debug.WriteLine("XPath = " + xpathOfAttributes);
            XmlNodeList attrs = doc.SelectNodes(xpathOfAttributes);
            foreach (XmlAttribute attr in attrs)
            {
                Regex re = (Regex)RegexForAttrName[attr.LocalName];
                Debug.WriteLine("Checking " + attr.Value + " against " + re.ToString());
                if (!re.IsMatch(attr.Value))
                {
                    // TODO: i18n
                    throw new XmlSchemaException("Invalid value for " + attr.LocalName + " attribute: " + attr.Value, null);
                }
            }
        }

        private Hashtable RegexForAttrName = new Hashtable();
        private string XPathPredicateOfAttributes = null;
    }
}
