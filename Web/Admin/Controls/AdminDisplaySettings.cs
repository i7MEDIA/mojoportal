using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
	/// <summary>
	/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
	/// </summary>
	public class AdminDisplaySettings : WebControl
	{

		//private string siteSettingsFolderNamesListNoNamesMarkup = "<div class='alert alert-info'>{0}</div>";
		//public string SiteSettingsFolderNamesListNoNamesMarkup { get => siteSettingsFolderNamesListNoNamesMarkup; set => siteSettingsFolderNamesListNoNamesMarkup = value; }

		private string siteSettingsNoticeMarkup = "<div class='alert alert-info'>{0}</div>";
		public string SiteSettingsNoticeMarkup { get => siteSettingsNoticeMarkup; set => siteSettingsNoticeMarkup = value; }

		private string siteSettingsPanelHeadingMarkup = "<h3>{0} <small>{1}</small></h3>";
		public string SiteSettingsPanelHeadingMarkup { get => siteSettingsPanelHeadingMarkup; set => siteSettingsPanelHeadingMarkup = value; }

		private string siteSettingsSubPanelHeadingMarkup = "<h4>{0} <small>{1}</small></h4>";
		public string SiteSettingsSubPanelHeadingMarkup { get => siteSettingsSubPanelHeadingMarkup; set => siteSettingsSubPanelHeadingMarkup = value; }

		private string securityProtocolCheckResponseMarkup = "<div>{0}</div>";
		public string SecurityProtocolCheckResponseMarkup { get => securityProtocolCheckResponseMarkup; set => securityProtocolCheckResponseMarkup = value; }


		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			// nothing to render
		}
	}
}