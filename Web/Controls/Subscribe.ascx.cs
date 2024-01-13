using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ELetterUI;

public partial class Subscribe : UserControl
{
        private static readonly ILog log = LogManager.GetLogger(typeof(Subscribe));

        private SiteSettings siteSettings = null;
	private SubscriberRepository subscriptions = new();
        private List<LetterInfo> siteAvailableSubscriptions = null;
        protected string siteRoot = string.Empty;
        private SiteUser currentUser = null;

	public string ButtonText { get; set; } = string.Empty;

	public string WatermarkText { get; set; } = "none";

	public int OverrideInputWidth { get; set; } = 0;

	public string ThankYouMessage { get; set; } = "none";

	public bool ShowList { get; set; } = true;

	public bool IncludeDescriptionInList { get; set; } = true;

	public bool ShowFormatOptions { get; set; } = true;

	public bool ShowMoreInfoLink { get; set; } = false;

	public bool ShowPreviousEditionsLink { get; set; } = true;

	public string MoreInfoText { get; set; } = "none";

	public bool HtmlIsDefault { get; set; } = true;

	public string ValidationGroup { get; set; } = "subscribe";


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
		if (!ShowList) { return; }
            if (siteAvailableSubscriptions == null) { return; }

            rptLetters.DataSource = siteAvailableSubscriptions;
            rptLetters.DataBind();
            if (rptLetters.Items.Count == 0) { this.Visible = false; }

        }

        void btnSubscribe_Click(object sender, EventArgs e)
        {
		Page.Validate(ValidationGroup);
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
		if (ShowList)
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
			if (ShowFormatOptions)
                {
                    s.UseHtml = rbHtmlFormat.Checked;
                }
                else
                {
				s.UseHtml = HtmlIsDefault;
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
                    

			if (!s.IsVerified)
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
					if (ShowFormatOptions)
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
		btnSubscribe.Text = ButtonText.Coalesce(Resource.NewsletterSubscribeButton);
		if (WatermarkText != "none")
            {
			txtEmail.Attributes.Add("placeholder", WatermarkText);
            }
            else
            {
			txtEmail.Attributes.Add("placeholder", Resource.NewsletterEmailWatermark);
            }

            txtEmail.ToolTip = Resource.RegisterEmailHint;

		if (OverrideInputWidth > 0)
            {
			txtEmail.Attributes.Add("style", "width:" + OverrideInputWidth.ToInvariantString() + "px;");
            }

		if (ThankYouMessage != "none")
            {
			litThankYou.Text = ThankYouMessage;
            }
            else
            {
                litThankYou.Text = Resource.NewsletterThankYouMessage;
            }

		if (MoreInfoText != "none")
            {
			lnkMoreInfo.Text = MoreInfoText;
            }
            else
            {
                lnkMoreInfo.Text = Resource.NewsletterMoreInfoLink;
            }

            rbHtmlFormat.Text = Resource.NewsletterHtmlFormatLabel;
            rbPlainText.Text = Resource.NewsletterPlainTextFormatLabel;

            reqEmail.ErrorMessage = Resource.NewsletterEmailRequiredMessage;
		reqEmail.ValidationGroup = ValidationGroup;
            regexEmail.ErrorMessage = Resource.NewsletterEmailRegexxMessage;
		regexEmail.ValidationGroup = ValidationGroup;
		vSummary.ValidationGroup = ValidationGroup;

            if (!IsPostBack)
            {
			if (HtmlIsDefault)
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
		spnFormat.Visible = ShowFormatOptions;
            
            lnkMoreInfo.NavigateUrl = siteRoot + "/eletter/Default.aspx";
		lnkMoreInfo.Visible = ShowMoreInfoLink;
		if (ShowList)
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