using log4net;
using System;

namespace mojoPortal.Web.AdminUI;

public partial class DesignerToolsPage : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(DesignerToolsPage));


	protected void Page_Load(object sender, EventArgs e)
	{
		_log.Info("Default.aspx is deprecated, redirecting to admin menu.");

		SiteUtils.RedirectToAdminMenu(this);
		return;
	}


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion
}
