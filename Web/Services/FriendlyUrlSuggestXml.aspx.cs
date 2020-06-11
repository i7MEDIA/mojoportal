using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;
using System;
using System.Text;
using System.Web;
using System.Xml;

namespace mojoPortal.Web.Services
{
	public partial class FriendlyUrlSuggestXml : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.Expires = -1;
			Response.ContentType = "application/xml";
			Encoding encoding = new UTF8Encoding();

			XmlTextWriter xmlTextWriter = new XmlTextWriter(Response.OutputStream, encoding)
			{
				Formatting = Formatting.Indented
			};

			xmlTextWriter.WriteStartDocument();
			xmlTextWriter.WriteStartElement("DATA");

			string warning = string.Empty;

			if (Request.Params.Get("pn") != null)
			{
				string pageName = Request.Params.Get("pn");
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

				if (siteSettings != null)
				{
					string friendlyUrl = SiteUtils.SuggestFriendlyUrl(pageName.Trim(), siteSettings);

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
					string enteredUrl = HttpContext.Current.Server.UrlDecode(Request.Params.Get("cu").Trim());

					if (WebPageInfo.IsPhysicalWebPage(enteredUrl))
					{
						warning = Resource.PageSettingsPhysicalUrlWarning;
					}

					xmlTextWriter.WriteStartElement("wn");
					xmlTextWriter.WriteString(warning);
					xmlTextWriter.WriteEndElement();
				}
			}

			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();

			xmlTextWriter.Close();
		}
	}
}
