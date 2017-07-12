/// Author:					
/// Created:				2008-05-15
/// Last Modified:			2013-07-15
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.BlogUI;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web
{
    /// <summary>
    /// Purpose: Renders a SiteMap as xml 
    /// in google site map protocol format
    /// https://www.google.com/webmasters/tools/docs/en/protocol.html
    /// for blog posts that have friendly urls
    /// 
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class BlogSiteMap : IHttpHandler
    {
        

        public void ProcessRequest(HttpContext context)
        {
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

                // add blog post urls
                if (WebConfigSettings.EnableBlogSiteMap)
                    AddBlogUrls(context, xmlTextWriter);


                xmlTextWriter.WriteEndElement(); //urlset

                //end of document
                xmlTextWriter.WriteEndDocument();
                
            }



        }

        private void AddBlogUrls(HttpContext context, XmlTextWriter xmlTextWriter)
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
            

            using(IDataReader reader = Blog.GetBlogsForSiteMap(siteSettings.SiteId))
            {
                while (reader.Read())
                {
                    int pageId = Convert.ToInt32(reader["PageID"]);
                    int moduleId = Convert.ToInt32(reader["ModuleID"]);
                    int itemId = Convert.ToInt32(reader["ItemID"]);
                    string itemUrl = reader["ItemUrl"].ToString();

                    string urlToUse = FormatBlogUrl(baseUrl, itemUrl, pageId, moduleId, itemId);

                    xmlTextWriter.WriteStartElement("url");
                    //xmlTextWriter.WriteElementString("loc", baseUrl + reader["ItemUrl"].ToString().Replace("~", string.Empty));
                    xmlTextWriter.WriteElementString("loc", urlToUse);
                    
                    
                    xmlTextWriter.WriteElementString(
                            "lastmod",
                            Convert.ToDateTime(reader["LastModUtc"]).ToString("u", CultureInfo.InvariantCulture).Replace(" ", "T"));

                    // maybe should use never for blog posts but in case it changes we do want to be re-indexed
                    xmlTextWriter.WriteElementString("changefreq", "monthly");

                    xmlTextWriter.WriteElementString("priority", "0.5");

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
