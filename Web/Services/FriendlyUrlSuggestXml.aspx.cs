/// Author:						
/// Created:					9/10/2006
/// Last Modified:				4/14/2007
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Web;
using System.Text;
using System.Xml;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.Services
{
    
    public partial class FriendlyUrlSuggestXml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Expires = -1;
            Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();

            XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding);
            xmlTextWriter.Formatting = Formatting.Indented;

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("DATA");
            string warning = string.Empty;

            if (Request.Params.Get("pn") != null)
            {
                String pageName = Request.Params.Get("pn");
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

                if (siteSettings != null)
                {
                    String friendlyUrl = SiteUtils.SuggestFriendlyUrl(pageName, siteSettings);
                    
                    if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrl))
                    {
                        warning = Resource.PageSettingsPhysicalUrlWarning;
                    }

                    xmlTextWriter.WriteStartElement("fn");
                    
                    xmlTextWriter.WriteString("~/" + friendlyUrl);
                    
                    xmlTextWriter.WriteEndElement();

                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();

                }

            }
            else
            {
                if (Request.Params.Get("cu") != null)
                {
                    String enteredUrl = HttpContext.Current.Server.UrlDecode(Request.Params.Get("cu"));
                    if (WebPageInfo.IsPhysicalWebPage(enteredUrl))
                    {
                        warning = Resource.PageSettingsPhysicalUrlWarning;
                    }
                    
                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();
                }


            }
            
            //end of document
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Close();
            //Response.End();

        }
    }
}
