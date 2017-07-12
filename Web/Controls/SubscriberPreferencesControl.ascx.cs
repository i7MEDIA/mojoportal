// Author:					
// Created:				    2007-12-21
// Last Modified:			2009-10-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ELetterUI
{
    
    public partial class SubscriberPreferencesControl : UserControl
    {
        //private ScriptManager scriptController;
        private SiteSettings siteSettings = null;
        private SiteUser siteUser = null;
        private List<LetterSubscriber> userSubscriptions = null;
        private SubscriberRepository subscriptions = new SubscriberRepository();
        private List<LetterInfo> siteAvailableSubscriptions = null;
        private Guid userGuid = Guid.Empty;
        protected string siteRoot = string.Empty;
        private bool hasUnverifiedSubscriptions = false;

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();


        }


        private void PopulateControls()
        {
            if (!Request.IsAuthenticated) return;
            if (siteUser == null) return;
            if (siteAvailableSubscriptions == null) return;
            if (Page.IsPostBack) return;

            BindPreferences();

            if (hasUnverifiedSubscriptions)
            {
                lblUnverifiedWarning.Visible = true;

            }


        }

        private void BindPreferences()
        {
            rbHtmlFormat.Checked = UseHtml();
            rbPlainText.Checked = !rbHtmlFormat.Checked;

            rptLetterPrefs.DataSource = siteAvailableSubscriptions;
            rptLetterPrefs.DataBind();

        }

        void btnSavePreferences_Click(object sender, EventArgs e)
        {
            foreach (LetterInfo availableSubscription in siteAvailableSubscriptions)
            {
                string controlID = "chk" + availableSubscription.LetterInfoGuid.ToString();

                if (Request.Params.Get(controlID) != null)
                {
                    
                    if (!IsSubscribed(availableSubscription.LetterInfoGuid))
                    {
                        //subscribe
                        LetterSubscriber subscriber = new LetterSubscriber();
                        subscriber.SiteGuid = siteSettings.SiteGuid;
                        subscriber.LetterInfoGuid = availableSubscription.LetterInfoGuid;
                        subscriber.UserGuid = siteUser.UserGuid;
                        subscriber.EmailAddress = siteUser.Email.ToLower();
                        subscriber.IsVerified = true;
                        subscriber.UseHtml = rbHtmlFormat.Checked;
                        subscriber.IpAddress = SiteUtils.GetIP4Address();
                        subscriptions.Save(subscriber);

                        LetterInfo.UpdateSubscriberCount(availableSubscription.LetterInfoGuid);

                    }
                    else
                    {
                        // user is subscribed already
                        foreach (LetterSubscriber s in userSubscriptions)
                        {
                            if (s.LetterInfoGuid == availableSubscription.LetterInfoGuid)
                            {
                                if (s.UseHtml != rbHtmlFormat.Checked)
                                {
                                    s.UseHtml = rbHtmlFormat.Checked;
                                    subscriptions.Save(s);  
                                }
                                if (!s.IsVerified)
                                {
                                    subscriptions.Verify(s.SubscribeGuid, true, Guid.Empty);
                                    LetterInfo.UpdateSubscriberCount(availableSubscription.LetterInfoGuid);
                                }
                            }

                        }

                    }
                }
                else
                {
                    if (IsSubscribed(availableSubscription.LetterInfoGuid))
                    {
                        // unsubscribe
                        foreach (LetterSubscriber s in userSubscriptions)
                        {
                            if (s.LetterInfoGuid == availableSubscription.LetterInfoGuid)
                            {
                                subscriptions.Delete(s);
                            }
                        }
                        
                        
                        LetterInfo.UpdateSubscriberCount(availableSubscription.LetterInfoGuid);

                    }
                }


            }
            
            GetSubscriptions();
            BindPreferences();
            lblUnverifiedWarning.Visible = false;
            //btnSavePreferences.Text = Resource.NewsletterSavePreferencesButton;
            System.Threading.Thread.Sleep(1000);

            UpdatePanel1.Update();
            


        }

        private bool UseHtml()
        {

            foreach (LetterSubscriber subscription in userSubscriptions)
            {
                return subscription.UseHtml;
            }

            return true;

        }

        protected string GetChecked(string strLetterInfoGuid)
        {
            if(IsSubscribed(strLetterInfoGuid))
            return " checked ";

            return string.Empty;

        }

        protected bool IsSubscribed(string strLetterInfoGuid)
        {
            if (strLetterInfoGuid.Length == 36)
            {
                Guid letterInfoGuid = new Guid(strLetterInfoGuid);
                return IsSubscribed(letterInfoGuid);

            }

            return false;

        }

        protected bool IsSubscribed(Guid letterInfoGuid)
        {
            if (siteUser == null) { return false; }

            
            foreach (LetterSubscriber subscription in userSubscriptions)
            {
                if (subscription.LetterInfoGuid == letterInfoGuid) return true;
            }

            return false;

        }



        private void PopulateLabels()
        {
            litHeader.Text = Resource.NewsletterPreferencesHeader;
            rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;
            rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;
            btnSavePreferences.Text = Resource.NewsletterSavePreferencesButton;
            btnSavePreferences.ToolTip = Resource.NewsletterSavePreferencesButton;

            //btnSavePreferences.Attributes.Add("style", "display:inline;");
            //if (!Page.IsPostBack)
            //{
            //    UIHelper.DisableButtonAfterClick(
            //        btnSavePreferences,
            //        Resource.ButtonDisabledPleaseWait,
            //        Page.ClientScript.GetPostBackEventReference(this.btnSavePreferences, string.Empty)
            //        );
            //}



        }


        private void LoadSettings()
        {
            siteRoot = SiteUtils.GetNavigationSiteRoot();
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (userGuid == Guid.Empty)
            {
                siteUser = SiteUtils.GetCurrentSiteUser();
            }
            else
            {
                siteUser = new SiteUser(siteSettings, userGuid);
            }

            //if (scriptController == null)
            //{
            //    scriptController = (ScriptManager)Page.Master.FindControl("ScriptManager1");
            //}

            //if (scriptController != null)
            //{
            //    scriptController.RegisterAsyncPostBackControl(this.btnSavePreferences);
            //}

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }


            GetSubscriptions();


        }

        private void GetSubscriptions()
        {
            if ((siteSettings != null) && (siteUser != null))
            {
                siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForCurrentUser(siteSettings.SiteGuid);
                userSubscriptions = subscriptions.GetListByUser(siteSettings.SiteGuid, siteUser.UserGuid);
                foreach (LetterSubscriber s in userSubscriptions)
                {
                    if (!s.IsVerified) { hasUnverifiedSubscriptions = true; }
                }
                
            }


        }





        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.btnSavePreferences.Click += new EventHandler(btnSavePreferences_Click);
#if NET35
            if (WebConfigSettings.DisablePageViewStateByDefault) {Page.EnableViewState = true; }
#endif
        }

        
    }
}