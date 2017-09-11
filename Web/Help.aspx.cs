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
		private string helpKey = String.Empty;


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

			// if we pass this param suppress edit link doesn't work in cluetip
			if (Request.Params.Get("e") == null)
			{
				if (WebUser.IsAdminOrContentAdmin)
				{
					if (helpKey != String.Empty)
					{
						litEditLink.Text =
							"<a href='" +
							SiteUtils.GetNavigationSiteRoot() + "/HelpEdit.aspx?helpkey=" +
							SecurityHelper.RemoveMarkup(helpKey) + "'>" +
							Resource.HelpEditLink +
							"</a><br />"
						;
					}
				}
			}

			if (helpKey != String.Empty)
			{
				ShowHelp();
			}
		}


		protected void ShowHelp()
		{
			String helpText = ResourceHelper.GetHelpFileText(helpKey);

			if (helpText == String.Empty)
			{
				helpText = WebUser.IsAdminOrContentAdmin ?
					String.Format(Resource.HelpNoHelpAvailableAdminUser, HttpUtility.HtmlDecode(SecurityHelper.RemoveMarkup(helpKey))) :
					Resource.HelpNoHelpAvailable;
			}

			litHelp.Text = helpText;
		}
	}
}