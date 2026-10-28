using log4net;
using System;

namespace mojoPortal.Web.AdminUI;

public partial class ManageSkinPage : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(ManageSkinPage));

	protected void Page_Load(object sender, EventArgs e)
	{
		_log.Info("ManageSkin.aspx is deprecated, redirecting to admin menu.");

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
