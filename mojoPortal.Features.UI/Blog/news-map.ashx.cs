// Author:					
// Created:				    2014-01-17
// Last Modified:			2014-02-10
//		
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
using mojoPortal.Web.BlogUI;
using mojoPortal.Web.Framework;
using log4net;

namespace mojoPortal.Features.UI
{
    /// <summary>
    /// SiteMap for google news
    /// https://support.google.com/news/publisher/topic/2527688?hl=en
    /// 
    /// </summary>
    public class BlogNewsMap : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BlogNewsMap));


        public void ProcessRequest(HttpContext context)
        {
            GenerateSiteMap(context);
        }

        private void GenerateSiteMap(HttpContext context)
        {
            //context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(20));
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);

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

                xmlTextWriter.WriteStartAttribute("xmlns:news");
                xmlTextWriter.WriteValue("http://www.google.com/schemas/sitemap-news/0.9");
                xmlTextWriter.WriteEndAttribute();

                xmlTextWriter.WriteStartAttribute("xmlns:image");
                xmlTextWriter.WriteValue("http://www.google.com/schemas/sitemap-image/1.1");
                xmlTextWriter.WriteEndAttribute();




                // add blog post urls
                //if (WebConfigSettings.EnableBlogSiteMap)
                    AddNewsItems(context, xmlTextWriter);


                xmlTextWriter.WriteEndElement(); //urlset

                //end of document
                xmlTextWriter.WriteEndDocument();

            }



        }

        private void AddNewsItems(HttpContext context, XmlTextWriter xmlTextWriter)
        {


            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings == null) { return; }

            if (siteSettings.SiteGuid == Guid.Empty) { return; }

            string baseUrl = SiteUtils.GetNavigationSiteRoot();
            if ((siteSettings.UseSslOnAllPages) && (SiteUtils.SslIsAvailable()))
            {
                baseUrl = baseUrl.Replace("http:", "https:");
            }
            else
            {
                baseUrl = baseUrl.Replace("https:", "http:");
            }


            using (IDataReader reader = Blog.GetBlogsForNewsMap(siteSettings.SiteId, BlogConfiguration.NewsMapMaxHoursOld))
            {
                while (reader.Read())
                {
                    string pubName = reader["PubName"].ToString();
                    string itemUrl = reader["ItemUrl"].ToString();

                    if (string.IsNullOrEmpty(pubName)) 
                    {
                        log.Error("Blog News Map skipping item " + itemUrl + " because publication name is missing and is required.");

                        continue; 
                    }

                    int pageId = Convert.ToInt32(reader["PageID"]);
                    int moduleId = Convert.ToInt32(reader["ModuleID"]);
                    int itemId = Convert.ToInt32(reader["ItemID"]);

                    string headlineImageUrl = reader["HeadlineImageUrl"].ToString();
                    string pubAccess = reader["PubAccess"].ToString();
                    string pubGenres = reader["PubGenres"].ToString();
                    string pubGeoLocations = reader["PubGeoLocations"].ToString();
                    string pubKeyWords = reader["PubKeyWords"].ToString();
                    string pubLanguage = reader["PubLanguage"].ToString();
                    string pubStockTickers = reader["PubStockTickers"].ToString();
                    string title = reader["Heading"].ToString();
                    
                    

                    string urlToUse = FormatBlogUrl(baseUrl, itemUrl, pageId, moduleId, itemId);

                    xmlTextWriter.WriteStartElement("url");
                    //xmlTextWriter.WriteElementString("loc", baseUrl + reader["ItemUrl"].ToString().Replace("~", string.Empty));
                    xmlTextWriter.WriteElementString("loc", urlToUse);

                    xmlTextWriter.WriteStartElement("news:news");

                    xmlTextWriter.WriteStartElement("news:publication");
                    xmlTextWriter.WriteElementString("news:name", pubName);
                    xmlTextWriter.WriteElementString("news:language", pubLanguage);
                    xmlTextWriter.WriteEndElement(); //news:publication

                    if (pubAccess.Length > 0)
                    {
                        xmlTextWriter.WriteElementString("news:access", pubAccess);
                    }

                    if (pubGenres.Length > 0)
                    {
                        xmlTextWriter.WriteElementString("news:genres", pubGenres);
                    }


                    xmlTextWriter.WriteElementString("news:publication_date", 
                        Convert.ToDateTime(reader["StartDate"]).ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T")
                        );

                    xmlTextWriter.WriteElementString("news:title", title);

                    if (pubKeyWords.Length > 0)
                    {
                        xmlTextWriter.WriteElementString("news:keywords", pubKeyWords);
                    }

                    if (pubStockTickers.Length > 0)
                    {
                        xmlTextWriter.WriteElementString("news:stock_tickers", pubStockTickers);
                    }
                    


                    xmlTextWriter.WriteEndElement(); //news:news

                    if (headlineImageUrl.Length > 0)
                    {
                        xmlTextWriter.WriteStartElement("image:image");
                        xmlTextWriter.WriteElementString("image:loc", WebUtils.ResolveServerUrl(headlineImageUrl));
                        xmlTextWriter.WriteEndElement();

                    }
                   

                   
                   

                    xmlTextWriter.WriteEndElement(); //url
                }


            }


        }


        private string FormatBlogUrl(string baseUrl, string itemUrl, int pageId, int moduleId, int itemId)
        {
            bool useFriendlyUrls = BlogConfiguration.UseFriendlyUrls(moduleId);

            if (useFriendlyUrls && (itemUrl.Length > 0))
                return baseUrl + itemUrl.Replace("~", string.Empty);

            return baseUrl + "/Blog/ViewPost.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString()
                + "&ItemID=" + itemId.ToInvariantString()
                ;

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