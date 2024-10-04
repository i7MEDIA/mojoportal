using log4net;
using System;
using System.Web.UI;

namespace mojoPortal.Web.Services;

public partial class SessionKeepAlive : Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SessionKeepAlive));

	/// <summary>
	/// This page does a simple job of keeping a session alive 
	/// by calling this page from an iframe in edit pages, it will keep the seesion from
	/// timing out before saving your changes
	/// Just add <portal:SessionKeepAliveControl id="ka1" runat="server" /> to you page.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	protected void Page_Load(object sender, EventArgs e)
	{
		if (log.IsDebugEnabled)
		{
			log.Debug("Requested SessionKeepAlive.aspx");
		}

		var timeoutLength = (Session.Timeout * 60) - 60;

		if (WebConfigSettings.SessionKeepAliveFrequencyOverrideMinutes != -1)
		{
			timeoutLength = WebConfigSettings.SessionKeepAliveFrequencyOverrideMinutes * 60;
		}

		litMetaRefresh.Text = Invariant($@"<meta http-equiv=""refresh"" content=""{timeoutLength}"" />");
	}
}
