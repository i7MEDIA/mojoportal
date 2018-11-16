// Author:		        
// Created:            2009-08-31
// Last Modified:      2014-04-08
//
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.IO;
using System.Web;
using System.Collections;

//using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Editor;
using Resources;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// Returns content templates in a json format consumable by CKeditor
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CKeditorTemplates : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string siteRoot = string.Empty;
        private string comma = string.Empty;

        
        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {

                return;
            }

            RenderJsonList(context);

        }

        private void RenderJsonList(HttpContext context)
        {
            context.Response.ContentType = "text/javascript";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            // Register a templates definition set named "mojo".
            context.Response.Write("CKEDITOR.addTemplates( 'mojo',{");

            context.Response.Write("\"imagesPath\":\"" + siteRoot + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/htmltemplateimages/" + "\"");
            context.Response.Write(",\"templates\":");
            context.Response.Write("[");

			//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
            if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            
            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);
            foreach (ContentTemplate t in templates)
            {
                if (!WebUser.IsInRoles(t.AllowedRoles)) { continue; }

                context.Response.Write(comma);
                context.Response.Write("{");

                context.Response.Write("\"title\":\"" + t.Title.JsonEscape() + "\"");
                context.Response.Write(",\"image\":\"" + t.ImageFileName.JsonEscape() + "\"");
                context.Response.Write(",\"description\":\"" + t.Description.RemoveLineBreaks().JsonEscape() + "\"");
                // is this going to work?
                context.Response.Write(",\"html\":\"" + t.Body.RemoveLineBreaks().JsonEscape() + "\"");

                context.Response.Write("}");

                comma = ",";

            }

			//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
			if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            context.Response.Write("]");

            context.Response.Write("});");


        }

        private void RenderSystemTemplateItems(HttpContext context)
        {
            List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();
            foreach (ContentTemplate t in templates)
            {
                context.Response.Write(comma);
                context.Response.Write("{");

                context.Response.Write("\"title\":\"" + t.Title.JsonEscape() + "\"");
                context.Response.Write(",\"image\":\"" + t.ImageFileName.JsonEscape() + "\"");
                context.Response.Write(",\"description\":\"" + t.Description.RemoveLineBreaks().JsonEscape() + "\"");
                // is this going to work?
                context.Response.Write(",\"html\":\"" + t.Body.RemoveLineBreaks().JsonEscape() + "\"");

                context.Response.Write("}");

                comma = ",";

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
