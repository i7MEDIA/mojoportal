using log4net;
using mojoPortal.Web.Framework;
using System;

namespace mojoPortal.Web.Services
{
	public partial class SessionKeepAlive : System.Web.UI.Page
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SessionKeepAlive));
        private static bool debugLog = log.IsDebugEnabled;

        /// <summary>
        ///     This page does a simple job of keeping a session alive 
        ///     by calling this page from an iframe in edit pages, it will keep the seesion from
        ///     timing out before saving your changes
        ///     Just add
        ///     <portal:SessionKeepAliveControl id="ka1" runat="server" />
        ///     to you page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (debugLog) { log.Debug("Requested SessionKeepAlive.aspx"); }

//#if NET35
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) - 60));
            if (WebConfigSettings.SessionKeepAliveFrequencyOverrideMinutes > -1)
            {
                litMetaRefresh.Text = "<meta http-equiv=\"refresh\"  content=\"" + WebConfigSettings.SessionKeepAliveFrequencyOverrideMinutes.ToInvariantString() + "\" />";
            }
            else
            {
                litMetaRefresh.Text = "<meta http-equiv=\"refresh\"  content=\"" + Convert.ToString((Session.Timeout * 60) - 60) + "\" />";
            }
//#else
            
            //litMetaRefresh.Text = "<meta http-equiv=\"refresh\"  content=\"" + Convert.ToString((FormsAuthentication.Timeout.Minutes * 60) - 60) + "\" />";
//#endif
            

        }
    }
}
