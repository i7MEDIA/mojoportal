// Author:					
// Created:				    2009-10-27
// Last Modified:			2014-03-18
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
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Net;
using Resources;

namespace mojoPortal.Web.ELetterUI
{
    public partial class Subscribe : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Subscribe));

        private SiteSettings siteSettings = null;
        private SubscriberRepository subscriptions = new SubscriberRepository();
        private List<LetterInfo> siteAvailableSubscriptions = null;
        protected string siteRoot = string.Empty;
        private SiteUser currentUser = null;

        private string buttonText = string.Empty;
        public string ButtonText
        {
            get { return buttonText; }
            set { buttonText = value; }
        }

        private string watermarkText = "none";
        public string WatermarkText
        {
            get { return watermarkText; }
            set { watermarkText = value; }
        }

        private int overrideInputWidth = 0;
        public int OverrideInputWidth
        {
            get { return overrideInputWidth; }
            set { overrideInputWidth = value; }
        }

        private string thankYouMessage = "none";
        public string ThankYouMessage
        {
            get { return thankYouMessage; }
            set { thankYouMessage = value; }
        }

        private bool showList = true;
        public bool ShowList
        {
            get { return showList; }
            set { showList = value; }
        }

        private bool includeDescriptionInList = true;
        public bool IncludeDescriptionInList
        {
            get { return includeDescriptionInList; }
            set { includeDescriptionInList = value; }
        }

        private bool showFormatOptions = true;
        public bool ShowFormatOptions
        {
            get { return showFormatOptions; }
            set { showFormatOptions = value; }
        }

        private bool showmoreInfoLink = false;
        public bool ShowMoreInfoLink
        {
            get { return showmoreInfoLink; }
            set { showmoreInfoLink = value; }
        }

        private bool showPreviousEditionsLink = true;
        public bool ShowPreviousEditionsLink
        {
            get { return showPreviousEditionsLink; }
            set { showPreviousEditionsLink = value; }
        }

        private string moreInfoText = "none";
        public string MoreInfoText
        {
            get { return moreInfoText; }
            set { moreInfoText = value; }
        }

        private bool htmlIsDefault = true;
        public bool HtmlIsDefault
        {
            get { return htmlIsDefault; }
            set { htmlIsDefault = value; }
        }

        private string validationGroup = "subscribe";

        public string ValidationGroup
        {
            get { return validationGroup; }
            set { validationGroup = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
           
            LoadSettings();
            SetupScript();
            PopulateLabels();

            if (!Page.IsPostBack)
            {
                BindList();
            }

        }

        private void BindList()
        {
            if (siteSettings == null) { return; }
            if (!showList) { return; }
            if (siteAvailableSubscriptions == null) { return; }

            rptLetters.DataSource = siteAvailableSubscriptions;
            rptLetters.DataBind();
            if (rptLetters.Items.Count == 0) { this.Visible = false; }

        }

        void btnSubscribe_Click(object sender, EventArgs e)
        {
            Page.Validate(validationGroup);
            if (!Page.IsValid) { return; }

            if (siteAvailableSubscriptions == null) { siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForCurrentUser(siteSettings.SiteGuid); }

            if ((siteAvailableSubscriptions != null) && (siteAvailableSubscriptions.Count == 0))
            {
                pnlSubscribe.Visible = false;
                pnlNoNewsletters.Visible = true;
                return;

            }

            currentUser = SiteUtils.GetCurrentSiteUser();
            DoSubscribe();
            if (hdnJs.Value != "js")
            {
                // this means javascript was not enabled so this was a full postback not an ajax postback
                // to get out of postback we will redirect to a thank you page
                WebUtils.SetupRedirect(this, siteRoot + "/eletter/ThankYou.aspx");
                return;
            }
            else
            {
                pnlSubscribe.Visible = false;
                pnlThanks.Visible = true;
                UpdatePanel1.Update();
            }
            
        }

        private void DoSubscribe()
        {
            if (showList)
            {
                foreach (LetterInfo available in siteAvailableSubscriptions)
                {
                    string controlID = "chk" + available.LetterInfoGuid.ToString();

                    if (Request.Params.Get(controlID) != null) //only found if checked
                    {
                        DoSubscribe(available, txtEmail.Text);
                    }
                }

            }
            else
            {
                // just subscribe to the default first letter
                if ((siteAvailableSubscriptions != null) && (siteAvailableSubscriptions.Count > 0))
                {
                    DoSubscribe(siteAvailableSubscriptions[0], txtEmail.Text);
                }
            }


        }

        private void DoSubscribe(LetterInfo letter, string email)
        {
            if (email == "email@gmail.com") { return; } //I've been seeing a lot of this from a bot

            LetterSubscriber s = subscriptions.Fetch(siteSettings.SiteGuid, letter.LetterInfoGuid, email);

            bool needToSendVerification = false;

            if (s == null)
            {
                s = new LetterSubscriber();
                s.SiteGuid = siteSettings.SiteGuid;
                s.EmailAddress = email;
                s.LetterInfoGuid = letter.LetterInfoGuid;
                if (showFormatOptions)
                {
                    s.UseHtml = rbHtmlFormat.Checked;
                }
                else
                {
                    s.UseHtml = htmlIsDefault;
                }

                if ((currentUser != null) && (string.Equals(currentUser.Email, email, StringComparison.InvariantCultureIgnoreCase)))
                {
                    s.UserGuid = currentUser.UserGuid;
                    s.IsVerified = true;
                }
                else
                {
                    // user is not authenticated but may still exist
                    // attach userguid but don't flag as verified
                    // because we don't know that the user who submited the form is the account owner
                    SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);
                    if (siteUser != null) { s.UserGuid = siteUser.UserGuid; }


                }
                s.IpAddress = SiteUtils.GetIP4Address();
                subscriptions.Save(s);

                LetterInfo.UpdateSubscriberCount(s.LetterInfoGuid);

                if (WebConfigSettings.LogNewsletterSubscriptions)
                {
                    log.Info(s.EmailAddress + " just subscribed to newsletter " + letter.Title);
                }
                    

                if(!s.IsVerified)
                {
                    needToSendVerification = true;
                }

            }
            else
            {
                // we found an existing subscription

                if (!s.IsVerified)
                {
                    // if the current authenticated user has the same email mark it as verified
                    if ((currentUser != null) && (string.Equals(currentUser.Email, email, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        s.UserGuid = currentUser.UserGuid;
                        if (showFormatOptions)
                        {
                            s.UseHtml = rbHtmlFormat.Checked;
                        }
                        subscriptions.Save(s);
                        subscriptions.Verify(s.SubscribeGuid, true, Guid.Empty);
                    }
                    else if (s.BeginUtc < DateTime.UtcNow.AddDays(-WebConfigSettings.NewsletterReVerifcationAfterDays))
                    {
                        // if the user never verifed before and its been at least x days go ahead and send another chance to verify
                        needToSendVerification = true;
                        // TODO: maybe we should log this in case some spam script is using the same email over and over
                        // or maybe we should add a verification sent count on subscription
                    }
                }
            }

            //added 2012-05-16 to support intranet scenarios where verification is not required
            if (!WebConfigSettings.NewsletterRequireVerification)
            {
                if (!s.IsVerified)
                {
                    s.IsVerified = true;
                    subscriptions.Save(s);
                }
                needToSendVerification = false;
            }

            if (needToSendVerification)
            {
                NewsletterHelper.SendSubscriberVerificationEmail(
                    siteRoot,
                    email,
                    s.SubscribeGuid,
                    letter,
                    siteSettings);

              

            }

        }


        private void PopulateLabels()
        {
            btnSubscribe.Text = buttonText.Coalesce(Resource.NewsletterSubscribeButton);
            if (watermarkText != "none")
            {
                txtEmail.Watermark = watermarkText;
            }
            else
            {
                txtEmail.Watermark = Resource.NewsletterEmailWatermark;
            }

            txtEmail.ToolTip = Resource.RegisterEmailHint;

            if (overrideInputWidth > 0)
            {
                txtEmail.Attributes.Add("style", "width:" + overrideInputWidth.ToInvariantString() + "px;");
            }

            if (thankYouMessage != "none")
            {
                litThankYou.Text = thankYouMessage;
            }
            else
            {
                litThankYou.Text = Resource.NewsletterThankYouMessage;
            }

            if (moreInfoText != "none")
            {
                lnkMoreInfo.Text = moreInfoText;
            }
            else
            {
                lnkMoreInfo.Text = Resource.NewsletterMoreInfoLink;
            }

            rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;
            rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;

            reqEmail.ErrorMessage = Resource.NewsletterEmailRequiredMessage;
            reqEmail.ValidationGroup = validationGroup;
            regexEmail.ErrorMessage = Resource.NewsletterEmailRegexxMessage;
            regexEmail.ValidationGroup = validationGroup;
            vSummary.ValidationGroup = validationGroup;

            if (!IsPostBack)
            {
                if (htmlIsDefault)
                {
                    rbHtmlFormat.Checked = true;
                }
                else
                {
                    rbPlainText.Checked = true;
                }
            }
            

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteRoot = SiteUtils.GetNavigationSiteRoot();
            spnFormat.Visible = showFormatOptions;
            
            lnkMoreInfo.NavigateUrl = siteRoot + "/eletter/Default.aspx";
            lnkMoreInfo.Visible = showmoreInfoLink;
            if (showList)
            {
                siteAvailableSubscriptions = NewsletterHelper.GetAvailableNewslettersForCurrentUser(siteSettings.SiteGuid);
            }
        }

        private void SetupScript()
        {
            if (WebConfigSettings.DisablejQuery)
            {
                hdnJs.Value = "js";
                return;
            }

            // this is merely to give us an indicator tha javascript was enabled
            // if not then we will do a redirect after the postback since the ajax postback won't work
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");
            script.Append("$('#" + hdnJs.ClientID + "').val('js');");
            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            btnSubscribe.Click += new EventHandler(btnSubscribe_Click);
            
        }

        

        

        


    }
}