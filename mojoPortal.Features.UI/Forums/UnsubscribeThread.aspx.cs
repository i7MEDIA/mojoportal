/// Last Modified:      2013-08-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ForumUI
{
    public partial class UnsubscribeForumThread : mojoBasePage
	{
		// Create a logger for use in this class
		private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeForumThread));

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion


        private Guid threadSubGuid = Guid.Empty;

        private void Page_Load(object sender, System.EventArgs e)
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.UnSubscribeLink);
            AddClassToBody("forumthreadunsubscribe");
            
            //if (!Request.IsAuthenticated)
            //{
            //    lblUnsubscribe.Text = ResourceHelper.GetMessageTemplate("AccessDeniedMessage.config");
            //    return;
            //}

            

            threadSubGuid = WebUtils.ParseGuidFromQueryString("ts", threadSubGuid);

            if (threadSubGuid != Guid.Empty)
            {
                ForumThread.Unsubscribe(threadSubGuid);
                lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeCompleted;
                return;
            }

            

            int threadID = WebUtils.ParseInt32FromQueryString("threadid", -1);

            if (threadID > -1)
            {
                UnsubscribeUser(threadID);
            }

					
		}

        private void UnsubscribeUser(int threadId)
        {
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null) return;
            if (!ForumThread.Unsubscribe(threadId, siteUser.UserId))
            {
                log.ErrorFormat("ForumThread.UnSubscribe({0}, {1}) failed", threadId, siteUser.UserId);
                lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeFailed;
                return;
            }
            lblUnsubscribe.Text = ForumResources.ForumThreadUnsubscribeCompleted;	

        }


		
	}
}
