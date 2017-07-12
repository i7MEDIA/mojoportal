// Author:					
// Created:				    2009-09-03
// Last Modified:			2010-02-12
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
using System.IO;
using System.Web;
using System.Collections;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Features.UI.Services
{
    /// <summary>
    /// Provides xml data in a format that can be consumed using the Vertigo Slideshow Silverlight control
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GalleryDataService : IHttpHandler
    {
        private SiteSettings siteSettings = null;
        private bool canRender = false;
        private int pageId = -1;
        private int moduleId = -1;
        private string siteRoot = string.Empty;
        private string thumbnailBaseUrl = string.Empty;
        private string fullSizeBaseUrl = string.Empty;
        private PageSettings galleryPage = null;
        private Module galleryModule = null;
        private Gallery gallery = null;
        private string transition = "CrossFadeTransition"; //CrossFadeTransition FadeTransition WipeTransition WipeRightTransition

        public void ProcessRequest(HttpContext context)
        {
            LoadSettings(context);

            if (canRender) { RenderXml(context); }
        }

        private void RenderXml(HttpContext context)
        {

            context.Response.Expires = -1;
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("data");
                xmlTextWriter.WriteAttributeString("transition", transition);

                int pageNumber = 1;
                int pageSize = 1000;

                DataTable dt = gallery.GetThumbsByPage(pageNumber, pageSize);

                if (dt.Rows.Count > 0)
                {
                    xmlTextWriter.WriteStartElement("album");
                    xmlTextWriter.WriteAttributeString("title", galleryModule.ModuleTitle);
                    xmlTextWriter.WriteAttributeString("description", "");
                    xmlTextWriter.WriteAttributeString("source", fullSizeBaseUrl + dt.Rows[0]["WebImageFile"].ToString());

                    foreach (DataRow row in dt.Rows)
                    {
                        xmlTextWriter.WriteStartElement("slide");
                        xmlTextWriter.WriteAttributeString("title", row["Caption"].ToString());
                        xmlTextWriter.WriteAttributeString("description", "");
                        xmlTextWriter.WriteAttributeString("source", fullSizeBaseUrl + row["WebImageFile"].ToString());
                        xmlTextWriter.WriteAttributeString("thumbnail", thumbnailBaseUrl + row["ThumbnailFile"].ToString());
                        xmlTextWriter.WriteEndElement();

                    }

                    xmlTextWriter.WriteEndElement(); //album

                }


                xmlTextWriter.WriteEndElement(); //data

                //end of document
                xmlTextWriter.WriteEndDocument();

            }
        }

        private void LoadSettings(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            pageId = WebUtils.ParseInt32FromQueryString("pageid", true, pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, moduleId);

            if (moduleId == -1) { return; }
            if (pageId == -1) { return; }

            galleryPage = new PageSettings(siteSettings.SiteId, pageId);
            if (galleryPage.PageId == -1) { return; }

            galleryModule = new Module(moduleId, pageId);
            if (galleryModule.ModuleId == -1) { return; }

            if ((!WebUser.IsInRoles(galleryPage.AuthorizedRoles)) && (!WebUser.IsInRoles(galleryModule.ViewRoles))) { return; }

            siteRoot = WebUtils.GetSiteRoot();

            //thumbnailBaseUrl = siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() 
            //    + "/GalleryImages/" + moduleId.ToInvariantString() + "/Thumbnails/";

            //fullSizeBaseUrl = siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString()
            //    + "/GalleryImages/" + moduleId.ToInvariantString() + "/WebImages/";

            string baseUrl;
            if (WebConfigSettings.ImageGalleryUseMediaFolder)
            {
                baseUrl = siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }
            else
            {
                baseUrl = siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/GalleryImages/" + moduleId.ToInvariantString() + "/";
            }

            thumbnailBaseUrl = baseUrl + "Thumbnails/";

            fullSizeBaseUrl = baseUrl + "WebImages/";

            gallery = new Gallery(moduleId);


            canRender = true;
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
