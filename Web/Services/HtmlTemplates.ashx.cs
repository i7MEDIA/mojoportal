// Author:		        
// Created:            2008-01-17
// Last Modified:      2011-01-05
// 


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
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class HtmlTemplates : IHttpHandler
    {

        private SiteSettings siteSettings = null;
        private string imagebaseUrl = string.Empty;

        

        public void ProcessRequest(HttpContext context)
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                //TODO: should we return some xml with an error message?
                return;
            }

       
            imagebaseUrl = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) +"/htmltemplateimages/";

            RenderXml(context);
               
        }

        private void RenderXml(HttpContext context)
        {
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("Templates");
                xmlTextWriter.WriteAttributeString("imagesBasePath", imagebaseUrl);

				//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
				if (WebConfigSettings.AddSystemContentTemplatesAboveSiteTemplates) //false by default
                {
                    RenderSystemTemplates(context, xmlTextWriter);
                }

                RenderSiteTemplates(context, xmlTextWriter);

				//2018/10/31 -- we don't really want to use these anymore. we're adding the ability to have templates in the skin but not system wide templates
				if (WebConfigSettings.AddSystemContentTemplatesBelowSiteTemplates) //false by default
                {
                    RenderSystemTemplates(context, xmlTextWriter);
                }
                
                xmlTextWriter.WriteEndElement(); //Templates
     

            }

        }

        private void RenderSiteTemplates(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            List<ContentTemplate> templates = ContentTemplate.GetAll(siteSettings.SiteGuid);

            foreach (ContentTemplate template in templates)
            {
                if (!WebUser.IsInRoles(template.AllowedRoles)) { continue; }
                
                xmlTextWriter.WriteStartElement("Template");
                xmlTextWriter.WriteAttributeString("title", template.Title);
                xmlTextWriter.WriteAttributeString("image", template.ImageFileName);
                xmlTextWriter.WriteStartElement("Description");
                xmlTextWriter.WriteValue(template.Description);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("Html");
                xmlTextWriter.WriteCData(template.Body);
                xmlTextWriter.WriteEndElement(); //Html
                xmlTextWriter.WriteEndElement(); //Template
                
            }

        }

        private void RenderSystemTemplates(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            List<ContentTemplate> templates = SiteUtils.GetSystemContentTemplates();

            foreach (ContentTemplate template in templates)
            {
                xmlTextWriter.WriteStartElement("Template");
                xmlTextWriter.WriteAttributeString("title", template.Title);
                xmlTextWriter.WriteAttributeString("image", template.ImageFileName);
                xmlTextWriter.WriteStartElement("Description");
                xmlTextWriter.WriteValue(template.Description);
                xmlTextWriter.WriteEndElement();

                xmlTextWriter.WriteStartElement("Html");
                xmlTextWriter.WriteCData(template.Body);
                xmlTextWriter.WriteEndElement(); //Html
                xmlTextWriter.WriteEndElement(); //Template

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
