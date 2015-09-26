/// Last Modified:      2013-08-17
/// 
/// 
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
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web.ForumUI
{
	
    public partial class UnsubscribeForum : mojoBasePage
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(UnsubscribeForum));

        private Guid subGuid = Guid.Empty;
 
        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
            Title = SiteUtils.FormatPageTitle(siteSettings, ForumResources.UnSubscribeLink);
            AddClassToBody("forumunsubscribe");
            
            //if (!Request.IsAuthenticated)
            //{
            //    lblUnsubscribe.Text = ResourceHelper.GetMessageTemplate("AccessDeniedMessage.config");
            //    return;
            //}

            

            subGuid = WebUtils.ParseGuidFromQueryString("fs", subGuid);

            
            if (subGuid != Guid.Empty)
            {
                Forum.Unsubscribe(subGuid);
                lblUnsubscribe.Text = ForumResources.ForumUnsubscribeCompleted;	
                return;
            }

            int forumID = WebUtils.ParseInt32FromQueryString("itemid", -1);

            if (forumID > -1)
            {
                UnsubscribeUser(forumID);
                return;
            }


            if ((WebUser.IsAdmin)&&(Request.Params.Get("ue") != null))
            {
                UnsubscribeUserFromAll(Request.Params.Get("ue"));
            }

            
					
		}


        private void UnsubscribeUser(int forumId)
        {
            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null) return;
            Forum forum = new Forum(forumId);
            if (!forum.Unsubscribe(siteUser.UserId))
            {
                log.ErrorFormat("Forum.UnSubscribe({0}, {1}, ) failed", forumId, siteUser.UserId);
                lblUnsubscribe.Text = ForumResources.ForumUnsubscribeFailed;
                return;
            }
            lblUnsubscribe.Text = ForumResources.ForumUnsubscribeCompleted;	


        }

        private void UnsubscribeUserFromAll(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) { return; }
            if(!Email.IsValidEmailAddressSyntax(userEmail)){ return;}

            
            SiteUser user = SiteUser.GetByEmail(siteSettings, userEmail);
            if(user == null) { return;}
            if(user.UserGuid == Guid.Empty){ return;}

            ForumThread.UnsubscribeAll(user.UserId);
            Forum.UnsubscribeAll(user.UserId);

            lblUnsubscribe.Text = Resources.ForumResources.AdminUnsubscribeUserComplete;	

        }


		
	}
}
