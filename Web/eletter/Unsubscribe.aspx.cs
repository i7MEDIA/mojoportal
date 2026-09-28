/// Author:					
/// Created:				2007-09-23
/// Last Modified:			2010-01-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using mojoPortal.Web.Framework;
using mojoPortal.Web.ExternalNewsletter;
using mojoPortal.Business;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

    public partial class UnsubscribePage : NonCmsBasePage
    {
        private Guid letterInfoGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private Guid subscriptionGuid = Guid.Empty;
        private SubscriberRepository subscriptions = new SubscriberRepository();


        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            if ((letterInfoGuid != Guid.Empty) && (userGuid != Guid.Empty))
            {
                if (WebConfigSettings.PromptBeforeUnsubscribeNewsletter)
                {
                    ShowUnsubscribePrompt();
                }
                else
                {
                    DoUnsubscribe();
                }
            }
            else
            {
                if (subscriptionGuid != Guid.Empty)
                {
                    DoUnsubscribe();
                }
                else
                {
                    ShowNotFoundMessge();
                }

            }


        }

        void btnUnsubscribeConfirm_Click(object sender, EventArgs e)
        {
            
            DoUnsubscribe();
        }

        private void DoUnsubscribe()
        {
            if (subscriptionGuid != Guid.Empty)
            {
                LetterSubscriber s = subscriptions.Fetch(subscriptionGuid);
                if (s == null)
                {
                    ShowNotFoundMessge();
                    return;
                }
                subscriptions.Delete(s);
                if (ExternalNewsletterProviderManager.IsExternal(s.LetterInfoGuid))
                {
                    ExternalNewsletterProviderManager.Unsubscribe(s.LetterInfoGuid, s.EmailAddress);
                }
                else
                {
                    LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);
                }
                lblUnsubscribe.Text = Resource.NewsletterUnsubscribeSuccess;

                btnUnsubscribeConfirm.Visible = false;
                lblUnsubscribe.Visible = true;
                return;
            }

            bool isExternal = ExternalNewsletterProviderManager.IsExternal(letterInfoGuid);
            LetterInfo letterInfo = new LetterInfo(letterInfoGuid);
            if (letterInfo.LetterInfoGuid == Guid.Empty && !isExternal)
            {
                ShowNotFoundMessge();
                return;
            }

            List<LetterSubscriber> userSubscriptions = subscriptions.GetListByUser(siteSettings.SiteGuid, userGuid);

            bool unsubscribed = false;

            foreach (LetterSubscriber s in userSubscriptions)
            {
                if (s.LetterInfoGuid == letterInfoGuid)
                {
                    subscriptions.Delete(s);
                    unsubscribed = true;
                    if (isExternal)
                    {
                        ExternalNewsletterProviderManager.Unsubscribe(s.LetterInfoGuid, s.EmailAddress);
                    }
                    else
                    {
                        LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);
                    }
                }

            }

            if (unsubscribed)
            {
                string title = !string.IsNullOrEmpty(letterInfo.Title) ? letterInfo.Title : Resource.UserProfileNewslettersTab;
                lblUnsubscribe.Text = string.Format(CultureInfo.InvariantCulture,
                    Resource.NewsletterUnsubscribeSuccessFormatString,
                    title);

                btnUnsubscribeConfirm.Visible = false;
                lblUnsubscribe.Visible = true;
            }
            else
            {
                ShowNotFoundMessge();
            }

        }

        private void ShowUnsubscribePrompt()
        {
            bool isExternal = ExternalNewsletterProviderManager.IsExternal(letterInfoGuid);
            LetterInfo letterInfo = new LetterInfo(letterInfoGuid);
            if (letterInfo.LetterInfoGuid == Guid.Empty && !isExternal)
            {
                ShowNotFoundMessge();
                return;
            }

            string title = !string.IsNullOrEmpty(letterInfo.Title) ? letterInfo.Title : Resource.UserProfileNewslettersTab;

            btnUnsubscribeConfirm.Visible = true;
            lblUnsubscribe.Visible = false;
            btnUnsubscribeConfirm.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.NewsletterUnsubscribeConfirmFormatString,
                Server.HtmlEncode(title));
        }

        private void ShowNotFoundMessge()
        {
            lblUnsubscribe.Text = Resource.NewsletterUnsubscribeInvalidParamsMessage;

        }


        private void PopulateLabels()
        {

        }

        private void LoadSettings()
        {
            letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", Guid.Empty);
            userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);

            subscriptionGuid = WebUtils.ParseGuidFromQueryString("s", Guid.Empty);

            AddClassToBody("administration");
            AddClassToBody("letterunsubscribe");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            btnUnsubscribeConfirm.Click += new EventHandler(btnUnsubscribeConfirm_Click);
        }

        

        #endregion
    }
}
