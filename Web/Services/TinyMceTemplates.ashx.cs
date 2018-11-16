// Author:		        
// Created:            2009-08-14
// Last Modified:      2018-10-31
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
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.Services
{
    /// <summary>
    /// When a template guid is passed returns html contentof the template,
    /// otherwise returns a json list of templates
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Index
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Custom_filebrowser
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/template
    /// http://wiki.moxiecode.com/index.php/TinyMCE:Configuration/theme_advanced_blockformats
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class TinyMceTemplates : IHttpHandler
    {
        private SiteSettings siteSettings = null;
        private Guid templateGuid = Guid.Empty;
        private string templateGuidString = string.Empty;
        private string siteRoot = string.Empty;
        private string comma = string.Empty;

        private const string jQueryAccordionGuid = "e110400d-c92d-4d78-a830-236f584af115";
        private const string jQueryAccordionNoHeightGuid = "08e2a92f-d346-416b-b37b-bd82acf51514";
        private const string jQueryTabsGuid = "7efaeb03-a1f9-4b08-9ffd-46237ba993b0";
        private const string YuiTabsGuid = "046dae46-5301-45a5-bcbf-0b87c2d9e919";
        private const string faqGuid = "ad5f5b63-d07a-4e6b-bbd5-2b6201743dab";
        private const string Columns2Over1Guid = "cfb9e9c4-b740-42f5-8c16-1957b536b8e9";
        private const string Columns3Over1Guid = "9ac79a8d-7dfd-4485-af3c-b8fdf256bbb8";
        private const string Columns4Over1Guid = "28ae8c68-b619-4e23-8dde-17d0a34ee7c6";

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                
                return;
            }

            templateGuid = WebUtils.ParseGuidFromQueryString("g", templateGuid);
            if (templateGuid != Guid.Empty) { templateGuidString = templateGuid.ToString(); }

            if (templateGuid != Guid.Empty)
            {
                RenderTemplate(context);

            }
            else
            {
                RenderJsonList(context);

            }

        }

        private void RenderJsonList(HttpContext context)
        {
            if (WebConfigSettings.TinyMceUseV4)
            {
                context.Response.ContentType = "application/json";
            }
            else
            {
                context.Response.ContentType = "text/javascript";
            }
            
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            siteRoot = SiteUtils.GetNavigationSiteRoot();

            if (WebConfigSettings.TinyMceUseV4)
            {
                context.Response.Write("[");
            }
            else
            {
                context.Response.Write("var tinyMCETemplateList = [");
            }

			//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
			if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);
            foreach (ContentTemplate t in templates)
            {
                if (WebConfigSettings.TinyMceUseV4)
                {
                    context.Response.Write(comma);
                    context.Response.Write("{\"title\":\"" + t.Title.JsonEscape() + "\",\"url\":\"" + siteRoot 
                        + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\"}");
                }
                else
                {
                    context.Response.Write(comma);
                    context.Response.Write("[\"" + t.Title.JsonEscape() + "\",\"" 
                        + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\",\"\"]");
                }
                
                comma = ",";

            }

			//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
			if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
            {
                RenderSystemTemplateItems(context);
            }

            if (WebConfigSettings.TinyMceUseV4)
            {
                context.Response.Write("]");
            }
            else
            {
                context.Response.Write("];");
            }
            


        }


        private void RenderSystemTemplateItems(HttpContext context)
        {
            List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();
            foreach (ContentTemplate t in templates)
            {
                if (WebConfigSettings.TinyMceUseV4)
                {
                    context.Response.Write(comma);
                    context.Response.Write("{\"title\":\"" + t.Title.JsonEscape() + "\",url:\"" 
                        + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\"}");
                }
                else
                {
                    context.Response.Write(comma);
                    context.Response.Write("[\"" + t.Title.JsonEscape() + "\",\"" 
                        + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\",\"\"]");
                }

                //context.Response.Write(comma);
                //context.Response.Write("['" + t.Title + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "','']");
                comma = ",";

            }


        }


        private void RenderTemplate(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            if (IsSystemTemplateGuid())
            {
                context.Response.Write(GetSystemTemplate());
            }
            else
            {
                ContentTemplate template = ContentTemplate.Get(templateGuid);
                if (template != null)
                {
                    context.Response.Write(template.Body);

                }

            }

        }

        private string GetSystemTemplate()
        {
            List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();
            Guid templateGuid = new Guid(templateGuidString);
            foreach (ContentTemplate t in templates)
            {
                if (t.Guid == templateGuid) { return t.Body; }
            }

            return string.Empty;

        }


        private bool IsSystemTemplateGuid()
        {
            if (templateGuidString == jQueryAccordionGuid) { return true; }
            if (templateGuidString == jQueryAccordionNoHeightGuid) { return true; }
            if (templateGuidString == jQueryTabsGuid) { return true; }
            if (templateGuidString == YuiTabsGuid) { return true; }
            if (templateGuidString == Columns2Over1Guid) { return true; }
            if (templateGuidString == Columns3Over1Guid) { return true; }
            if (templateGuidString == Columns4Over1Guid) { return true; }
            if (templateGuidString == faqGuid) { return true;}


            return false;
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
