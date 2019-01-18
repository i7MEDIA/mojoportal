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
		public string SiteSettingsNoticeMarkup { get; set; } = "<div class='alert alert-info'>{0}</div>";
		public string SiteSettingsPanelHeadingMarkup { get; set; } = "<h3>{0} <small>{1}</small></h3>";
		public string SiteSettingsSubPanelHeadingMarkup { get; set; } = "<h4>{0} <small>{1}</small></h4>";
		public string SecurityProtocolCheckResponseMarkup { get; set; } = "<div>{0}</div>";
		public string ModuleSettingsSettingPanelElement { get; set; } = "div";
		public string ModuleSettingsSettingLabelMarkup { get; set; } = "<label class=\"{0}\" for=\"{1}\">{2}</label>";
		public string ModuleSettingsSettingPanelClass { get; set; } = "settingrow";
		public string ModuleSettingsSettingLabelClass { get; set; } = "settinglabel";
		public string ModuleSettingsSettingControlClass { get; set; } = "forminput";
		public string RestartButtonClass { get; set; } = "btn btn-danger btn-sm ";
		public string UpdateAvailableLinkMarkup { get; set; } = "<a href=\"{0}\">{1}</a>";
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