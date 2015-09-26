// Author:                     Joe Audette
// Created:                    2009-05-13
// Last Modified:              2009-05-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using WebStore.Business;

namespace WebStore.UI.WebStore
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class WebStoreSiteMap : IHttpHandler
    {
        private SiteSettings siteSettings = null;

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            GenerateSiteMap(context);
        }

        private void GenerateSiteMap(HttpContext context)
        {
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(20));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {

                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("urlset");
                xmlTextWriter.WriteStartAttribute("xmlns");
                xmlTextWriter.WriteValue("http://www.sitemaps.org/schemas/sitemap/0.9");
                xmlTextWriter.WriteEndAttribute();


                AddUrls(context, xmlTextWriter);


                xmlTextWriter.WriteEndElement(); //urlset

                //end of document
                xmlTextWriter.WriteEndDocument();
                

            }



        }

        private void AddUrls(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            if (siteSettings == null) { return; }

            string baseUrl = SiteUtils.GetNavigationSiteRoot();
            if ((siteSettings.UseSslOnAllPages) && (SiteUtils.SslIsAvailable()))
            {
                baseUrl = baseUrl.Replace("http:", "https:");
            }
            else
            {
                baseUrl = baseUrl.Replace("https:", "http:");
            }

            using (IDataReader reader = Product.GetForSiteMap(siteSettings.SiteGuid, Guid.Empty))
            {
                while (reader.Read())
                {
                    xmlTextWriter.WriteStartElement("url");
                    xmlTextWriter.WriteElementString("loc", baseUrl + reader["Url"].ToString().Replace("~", string.Empty));
                    xmlTextWriter.WriteElementString(
                            "lastmod",
                            Convert.ToDateTime(reader["LastModified"]).ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T"));

                    
                    xmlTextWriter.WriteElementString("changefreq", "monthly");

                    xmlTextWriter.WriteElementString("priority", "0.6");

                    xmlTextWriter.WriteEndElement(); //url
                }


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
