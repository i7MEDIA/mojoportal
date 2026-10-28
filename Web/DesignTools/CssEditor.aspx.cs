using log4net;
using System;

namespace mojoPortal.Web.AdminUI;

public partial class CssEditorPage : NonCmsBasePage
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(CssEditorPage));

	protected void Page_Load(object sender, EventArgs e)
	{
		_log.Info("CssEditor.aspx is deprecated, redirecting to admin menu.");

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
