// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
// Author:					
// Created:				    2009-05-22
// Last Modified:			2009-06-22
// 

using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Collections;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// Returns xml describing the site search engine in OpenSearchDescription format 
    /// https://developer.mozilla.org/en/Creating_OpenSearch_plugins_for_Firefox
    /// http://www.opensearch.org/Specifications/OpenSearch/1.1#OpenSearch_description_document
    /// </summary>
    public class SearchEngineInfo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SendResponse(context);
        }

        private void SendResponse(HttpContext context)
        {

            string siteRoot = SiteUtils.GetNavigationSiteRoot();


            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            string imageSiteRoot = WebUtils.GetSiteRoot();
            string shortName = "mojoPortal Search";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings != null)
            {
                if (siteSettings.OpenSearchName.Length > 0)
                {
                    shortName = siteSettings.OpenSearchName;
                }
                else
                {
                    shortName = string.Format(CultureInfo.InvariantCulture, Resource.SearchDiscoveryTitleFormat, siteSettings.SiteName);
                }
            }

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("OpenSearchDescription");
                xmlTextWriter.WriteAttributeString("xmlns", "http://a9.com/-/spec/opensearch/1.1/");
                xmlTextWriter.WriteAttributeString("xmlns:moz", "http://www.mozilla.org/2006/browser/search/");

                xmlTextWriter.WriteStartElement("ShortName");
                xmlTextWriter.WriteValue(shortName);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("Description");
                xmlTextWriter.WriteValue("The search engine included in mojoPortal Content Managment System");
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("InputEncoding");
                xmlTextWriter.WriteValue("UTF-8");
                xmlTextWriter.WriteEndElement();
                
                
                //URI to an icon representative of the search engine. When possible, search engines should offer a 16x16 image of type "image/x-icon" 
                //and a 64x64 image of type "image/jpeg" or "image/png". 
                //context.Response.Write("<Image width=\"16\" height=\"16\" type=\"image/x-icon\">data:image/x-icon;base64,imageData</Image>");
                //context.Response.Write("<Image height="\16\" width=\"16\" type=\"image/x-icon\">http://example.com/favicon.ico</Image>");
                xmlTextWriter.WriteStartElement("Image");
                xmlTextWriter.WriteAttributeString("type", "image/x-icon");
                xmlTextWriter.WriteAttributeString("width", "16");
                xmlTextWriter.WriteAttributeString("height", "16");
                xmlTextWriter.WriteValue(SiteUtils.GetSkinBaseUrl(false, null) + "favicon.ico");
                xmlTextWriter.WriteEndElement();

                //self update url
                xmlTextWriter.WriteStartElement("Url");
                xmlTextWriter.WriteAttributeString("type", "application/opensearchdescription+xml");
                xmlTextWriter.WriteAttributeString("rel", "self");
                xmlTextWriter.WriteAttributeString("template", siteRoot + "/SearchEngineInfo.ashx");
                xmlTextWriter.WriteEndElement();
               

                //Atom url
                xmlTextWriter.WriteStartElement("Url");
                xmlTextWriter.WriteAttributeString("type", "application/atom+xml");
                xmlTextWriter.WriteAttributeString("template", siteRoot + "/SearchResults.ashx?q={searchTerms}&p={startPage?}");
                xmlTextWriter.WriteEndElement();
                //context.Response.Write("<Url type=\"application/atom+xml\" template=\"http://example.com/?q={searchTerms}&amp;pw={startPage?}&amp;format=atom\"/>");

                //search url
                xmlTextWriter.WriteStartElement("Url");
                xmlTextWriter.WriteAttributeString("type", "text/html");
                xmlTextWriter.WriteAttributeString("template", siteRoot + "/SearchResults.aspx?q={searchTerms}&p={startPage?}");
                xmlTextWriter.WriteEndElement();

                

                //autosuggest
                //context.Response.Write("<Url type=\"application/x-suggestions+json\" template=\"suggestionURL\"/>");

                //FF specific
                xmlTextWriter.WriteStartElement("moz:SearchForm");
                xmlTextWriter.WriteValue(siteRoot + "/SearchResults.aspx");
                xmlTextWriter.WriteEndElement();
                

                
                xmlTextWriter.WriteEndElement();

            }

        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
