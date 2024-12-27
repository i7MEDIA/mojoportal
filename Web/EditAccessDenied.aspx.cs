using System;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class EditAccessDenied : NonCmsBasePage
{
	override protected void OnInit(EventArgs e)
	{
		this.Load += new System.EventHandler(this.Page_Load);
		base.OnInit(e);
	}

	private void Page_Load(object sender, System.EventArgs e)
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AccessDenied);
		lnkHome.Text = Resource.ReturnHomeLabel;
		lnkHome.ToolTip = Resource.ReturnHomeLabel;
		lnkHome.NavigateUrl = "~/Default.aspx".ToLinkBuilder().ToString();
		SiteUtils.AddNoIndexMeta(this);
		//lblEditAccessDeniedMessage.Text = ResourceHelper.GetMessageTemplate("EditAccessDeniedMessage.config");

		AddClassToBody("accessdenied");
	}
}