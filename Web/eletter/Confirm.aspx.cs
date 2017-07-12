// Author:					
// Created:					2009-10-28
// Last Modified:			2010-03-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Resources;



namespace mojoPortal.Web.ELetterUI
{

    public partial class ConfirmPage : NonCmsBasePage
    {
        private SubscriberRepository subscriptions = new SubscriberRepository();
        private Guid subscriptionGuid = Guid.Empty;
        private LetterInfo letterInfo = null;
        private LetterSubscriber subscription = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if (subscription == null)
            {
                pnlNotFound.Visible = true;
                pnlConfirmed.Visible = false;
            }
            else
            {
                letterInfo = new LetterInfo(subscription.LetterInfoGuid);
                litConfrimDetails.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsletterConfirmedFormat, letterInfo.Title);
                pnlNotFound.Visible = false;
                pnlConfirmed.Visible = true;
            }

        }


        private void PopulateLabels()
        {
            lnkThisPage.Text = Resource.NewslettersLink;
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/Default.aspx";

        }

        private void LoadSettings()
        {
            subscriptionGuid = WebUtils.ParseGuidFromQueryString("s", subscriptionGuid);
            if (subscriptionGuid != Guid.Empty)
            {
                subscription = subscriptions.Fetch(subscriptionGuid);
                if ((subscription != null) && (subscription.SiteGuid == siteSettings.SiteGuid))
                {
                    subscriptions.Verify(subscription.SubscribeGuid, true, Guid.Empty);
                    if (subscription.UserGuid == Guid.Empty)
                    {
                        SiteUser user = SiteUser.GetByEmail(siteSettings, subscription.EmailAddress);
                        if (user != null)
                        {
                            subscription.UserGuid = user.UserGuid;
                            subscriptions.Save(subscription);
                        }
                    }
                    
                    LetterInfo.UpdateSubscriberCount(subscription.LetterInfoGuid);
                }
                else
                {
                    subscription = null;
                }

            }

            AddClassToBody("administration");
            AddClassToBody("eletterconfirm");
        }

        


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();


        }

        #endregion
    }
}
