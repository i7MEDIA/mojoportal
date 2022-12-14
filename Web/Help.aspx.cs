using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web.UI.Pages
{
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
						litEditLink.Text = $"<a href=\"{SiteUtils.GetNavigationSiteRoot()}/HelpEdit.aspx?helpkey={SecurityHelper.RemoveMarkup(helpKey)}\">{Resource.HelpEditLink}</a>";
					}
				}
			}

			if (helpKey != string.Empty)
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
					helpText = string.Format(Resource.HelpNoHelpAvailableAdminUser, HttpUtility.HtmlDecode(SecurityHelper.RemoveMarkup(helpKey)));
				}
				else
				{
					helpText = Resource.HelpNoHelpAvailable;
				}
			}

			litHelp.Text = helpText;
		}
	}
}