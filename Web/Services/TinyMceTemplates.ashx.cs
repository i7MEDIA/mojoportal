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
using Newtonsoft.Json;
using mojoPortal.Core.Configuration;
using mojoPortal.Web.Controls.Editors;

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
        //private Guid templateGuid = Guid.Empty;
        //private string templateGuidString = string.Empty;
        //private string siteRoot = string.Empty;
        //private string comma = string.Empty;
		private string skinRootFolder = string.Empty;
		private string currentSkin = string.Empty;
		private FileInfo skinTemplatesFile = null;
		private FileInfo systemTemplatesFile = null;


		//private const string jQueryAccordionGuid = "e110400d-c92d-4d78-a830-236f584af115";
  //      private const string jQueryAccordionNoHeightGuid = "08e2a92f-d346-416b-b37b-bd82acf51514";
  //      private const string jQueryTabsGuid = "7efaeb03-a1f9-4b08-9ffd-46237ba993b0";
  //      private const string YuiTabsGuid = "046dae46-5301-45a5-bcbf-0b87c2d9e919";
  //      private const string faqGuid = "ad5f5b63-d07a-4e6b-bbd5-2b6201743dab";
  //      private const string Columns2Over1Guid = "cfb9e9c4-b740-42f5-8c16-1957b536b8e9";
  //      private const string Columns3Over1Guid = "9ac79a8d-7dfd-4485-af3c-b8fdf256bbb8";
  //      private const string Columns4Over1Guid = "28ae8c68-b619-4e23-8dde-17d0a34ee7c6";

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {  
                return;
            }

			//siteRoot = SiteUtils.GetNavigationSiteRoot();
			skinRootFolder = SiteUtils.GetSiteSkinFolderPath();
			currentSkin = siteSettings.Skin;
			
			if (HttpContext.Current.Request.Params.Get("skin") != null)
			{
				currentSkin = SiteUtils.SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));
			}

			skinTemplatesFile = new FileInfo($"{skinRootFolder + currentSkin}\\config\\editortemplates.json");
			systemTemplatesFile = new FileInfo(HttpContext.Current.Server.MapPath("~/data/style/editortemplates.json"));

			RenderJsonList(context);

        }

        private void RenderJsonList(HttpContext context)
        {          
            context.Response.ContentEncoding = new UTF8Encoding();
			context.Response.ContentType = "application/json";

			var templatesOrder = AppConfig.EditorTemplatesOrder.SplitOnCharAndTrim(',');
            var collection = new EditorTemplateCollection();

			foreach (var i in templatesOrder)
			{

				switch (i)
				{
					case "site":

						//collection.ImagesPath = $"{siteRoot}/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/htmltemplateimages/";

						//add site content templates to list
						foreach (var t in ContentTemplate.GetAll(siteSettings.SiteGuid))
						{
							if (!WebUser.IsInRoles(t.AllowedRoles)) { continue; }

							collection.Templates.Add(new EditorTemplate
							{
								Title = t.Title,
								Image = t.ImageFileName,
								Description = t.Description,
								Html = t.Body.RemoveLineBreaks()
							});
						}

						break;
					case "system":
						if (!systemTemplatesFile.Exists) break;

						collection.Templates.AddRange(new EditorTemplateCollection(systemTemplatesFile).Templates);
                        
						break;
					case "skin":
						if (!skinTemplatesFile.Exists) break;

						collection.Templates.AddRange(new EditorTemplateCollection(skinTemplatesFile).Templates);

						break;
					default:
						break;
				}
			}

			context.Response.Write(JsonConvert.SerializeObject(new TinyMceTemplateCollection(collection).Templates));
            
        }


        //private void RenderSystemTemplateItems(HttpContext context)
        //{
        //    List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();
        //    foreach (ContentTemplate t in templates)
        //    {
        //        if (WebConfigSettings.TinyMceUseV4)
        //        {
        //            context.Response.Write(comma);
        //            context.Response.Write("{\"title\":\"" + t.Title.JsonEscape() + "\",url:\"" 
        //                + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\"}");
        //        }
        //        else
        //        {
        //            context.Response.Write(comma);
        //            context.Response.Write("[\"" + t.Title.JsonEscape() + "\",\"" 
        //                + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "\",\"\"]");
        //        }

        //        //context.Response.Write(comma);
        //        //context.Response.Write("['" + t.Title + "','" + siteRoot + "/Services/TinyMceTemplates.ashx?g=" + t.Guid.ToString() + "','']");
        //        comma = ",";

        //    }


        //}


        //private void RenderTemplate(HttpContext context)
        //{
        //    context.Response.ContentType = "text/html";
        //    Encoding encoding = new UTF8Encoding();
        //    context.Response.ContentEncoding = encoding;

        //    if (IsSystemTemplateGuid())
        //    {
        //        context.Response.Write(GetSystemTemplate());
        //    }
        //    else
        //    {
        //        ContentTemplate template = ContentTemplate.Get(templateGuid);
        //        if (template != null)
        //        {
        //            context.Response.Write(template.Body);

        //        }

        //    }

        //}

        //private string GetSystemTemplate()
        //{
        //    List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();
        //    Guid templateGuid = new Guid(templateGuidString);
        //    foreach (ContentTemplate t in templates)
        //    {
        //        if (t.Guid == templateGuid) { return t.Body; }
        //    }

        //    return string.Empty;

        //}


        //private bool IsSystemTemplateGuid()
        //{
        //    if (templateGuidString == jQueryAccordionGuid) { return true; }
        //    if (templateGuidString == jQueryAccordionNoHeightGuid) { return true; }
        //    if (templateGuidString == jQueryTabsGuid) { return true; }
        //    if (templateGuidString == YuiTabsGuid) { return true; }
        //    if (templateGuidString == Columns2Over1Guid) { return true; }
        //    if (templateGuidString == Columns3Over1Guid) { return true; }
        //    if (templateGuidString == Columns4Over1Guid) { return true; }
        //    if (templateGuidString == faqGuid) { return true;}


        //    return false;
        //}

		public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }


	public class TinyMceTemplateCollection
	{
		[JsonProperty(PropertyName = "imagesPath")]
		public string ImagesPath { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "templates")]
        public List<TinyMceTemplate> Templates { get; set; }
		public TinyMceTemplateCollection()
		{
			Templates = new List<TinyMceTemplate>();
		}

        public TinyMceTemplateCollection(EditorTemplateCollection collection)
        {
            ImagesPath = collection.ImagesPath ?? "";
            Templates = new List<TinyMceTemplate>();
            foreach (var template in collection.Templates)
            {
                Templates.Add(new TinyMceTemplate {
                    Title = template.Title ?? "",
                    Description = template.Description ?? "",
					Content = template.Html ?? "",
                });
            }
        }
	}

	public class TinyMceTemplate
	{
		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; } = string.Empty;

		[JsonProperty(PropertyName = "content")]
		public string Content { get; set; } = string.Empty;

		public bool ShouldSerializeDescription()
		{
			return Description != null;
		}
        public TinyMceTemplate() { }
        public TinyMceTemplate(EditorTemplate template)
        {
            Title = template.Title;
            Description = template.Description;
            Content = template.Html;
        }
	}


}
