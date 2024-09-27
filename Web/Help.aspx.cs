using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.UI.Pages;

public partial class Help : Page
{
	private string helpKey = string.Empty;

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}

	protected void Page_Load(object sender, EventArgs e)
	{

		helpKey = WebUtils.ParseStringFromQueryString("helpkey", helpKey);

		if (Request.Params.Get("helpkey") != null)
		{
			helpKey = Request.Params.Get("helpkey");
		}

		if (Request.Params.Get("e") == null)
		{
			if (WebUser.IsAdminOrContentAdmin)
			{
				if (helpKey != string.Empty)
				{
					litEditLink.Text = $"<a href=\"{"HelpEdit.aspx".ToLinkBuilder().AddParam("helpkey", helpKey)}\">{Resource.HelpEditLink}</a>";
				}
			}
		}

		if (!string.IsNullOrWhiteSpace(helpKey))
		{
			ShowHelp();
		}
	}

	protected void ShowHelp()
	{
		var helpText = ResourceHelper.GetHelpFileText(helpKey);

		if (string.IsNullOrWhiteSpace(helpText))
		{
			if (WebUser.IsAdminOrContentAdmin)
			{
				helpText = string.Format(Resource.HelpNoHelpAvailableAdminUser, HttpUtility.HtmlDecode(helpKey.RemoveMarkup()));
			}
			else
			{
				helpText = Resource.HelpNoHelpAvailable;
			}
		}

		litHelp.Text = helpText;
	}
}