using log4net;
using System;

namespace mojoPortal.Web.DesignTools;

public partial class DownloadSkin : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(DownloadSkin));

	protected void Page_Load(object sender, EventArgs e)
	{
		_log.Info("DownloadSkin.aspx is depricated, redirecting to admin menu.");

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
