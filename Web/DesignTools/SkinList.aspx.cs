using log4net;
using System;

namespace mojoPortal.Web.AdminUI;

public partial class SkinListPage : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(SkinListPage));


	protected void Page_Load(object sender, EventArgs e)
	{
		_log.Info("SkinList.aspx is deprecated, redirecting to admin menu.");

		SiteUtils.RedirectToAdminMenu(this);
		return;
	}


	#region OnInit

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}

	#endregion
}
