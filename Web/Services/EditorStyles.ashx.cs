/// Author:		        
/// Created:            2008-01-17
/// Last Modified:      2018-10-31

using System;
using System.Data;
using System.IO;
using System.Web;
using System.Collections;
using System.Text;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Services
{
    
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class EditorStyles : IHttpHandler
    {

        private string libraryfolder = string.Empty;


        public void ProcessRequest(HttpContext context)
        {

            SendStyleTemplateXml(context);

         
        }

        private void SendStyleTemplateXml(HttpContext context)
        {
            
            context.Response.Expires = -1;
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("Styles");

                if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates)
                {
                    RenderSystemStyles(context, xmlTextWriter);
                }

                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if(siteSettings != null)
                {
                    using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
                    {
                        while (reader.Read())
                        {
                            xmlTextWriter.WriteStartElement("Style");
                            xmlTextWriter.WriteAttributeString("name", reader["Name"].ToString());
                            xmlTextWriter.WriteAttributeString("element", reader["Element"].ToString());
                            xmlTextWriter.WriteStartElement("Attribute");
                            xmlTextWriter.WriteAttributeString("name", "class");
                            xmlTextWriter.WriteAttributeString("value", reader["CssClass"].ToString());
                            xmlTextWriter.WriteEndElement(); //Attribute
                            xmlTextWriter.WriteEndElement(); //Style
                        }

                    }


                }

                if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates)
                {
                    RenderSystemStyles(context, xmlTextWriter);
                }

                xmlTextWriter.WriteEndElement(); //Styles

                //end of document
                xmlTextWriter.WriteEndDocument();
            }


        }

        private void RenderSystemStyles(HttpContext context, XmlTextWriter xmlTextWriter)
        {
            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Heading 1");
            xmlTextWriter.WriteAttributeString("element", WebConfigSettings.CKEditorH1Mapping); //we start at h3 because site title is h1 and module title is h2
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Heading 2");
            xmlTextWriter.WriteAttributeString("element", WebConfigSettings.CKEditorH2Mapping);
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Heading 3");
            xmlTextWriter.WriteAttributeString("element", WebConfigSettings.CKEditorH3Mapping);
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Paragraph");
            xmlTextWriter.WriteAttributeString("element", "p");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Image on Right");
            xmlTextWriter.WriteAttributeString("element", "img");
            xmlTextWriter.WriteStartElement("Attribute");
            xmlTextWriter.WriteAttributeString("name", "class");
            xmlTextWriter.WriteAttributeString("value", "image-right");
            xmlTextWriter.WriteEndElement(); //Attribute
            xmlTextWriter.WriteEndElement(); //Style

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Image on Left");
            xmlTextWriter.WriteAttributeString("element", "img");
            xmlTextWriter.WriteStartElement("Attribute");
            xmlTextWriter.WriteAttributeString("name", "class");
            xmlTextWriter.WriteAttributeString("value", "image-left");
            xmlTextWriter.WriteEndElement(); //Attribute
            xmlTextWriter.WriteEndElement(); //Style

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Document Block aka div");
            xmlTextWriter.WriteAttributeString("element", "div");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Preformatted Text");
            xmlTextWriter.WriteAttributeString("element", "pre");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Address");
            xmlTextWriter.WriteAttributeString("element", "address");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Inline Quotation");
            xmlTextWriter.WriteAttributeString("element", "q");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Cited Work");
            xmlTextWriter.WriteAttributeString("element", "cite");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Emphasis");
            xmlTextWriter.WriteAttributeString("element", "em");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Code");
            xmlTextWriter.WriteAttributeString("element", "code");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Keyboard Input");
            xmlTextWriter.WriteAttributeString("element", "kbd");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Sample Text");
            xmlTextWriter.WriteAttributeString("element", "samp");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Term Definition");
            xmlTextWriter.WriteAttributeString("element", "dfn");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Variable");
            xmlTextWriter.WriteAttributeString("element", "var");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Deleted Text");
            xmlTextWriter.WriteAttributeString("element", "del");
            xmlTextWriter.WriteEndElement();

            xmlTextWriter.WriteStartElement("Style");
            xmlTextWriter.WriteAttributeString("name", "Inserted Text");
            xmlTextWriter.WriteAttributeString("element", "ins");
            xmlTextWriter.WriteEndElement();

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
