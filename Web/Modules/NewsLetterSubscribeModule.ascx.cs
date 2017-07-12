// Author:					
// Created:					2009-10-27
// Last Modified:			2013-01-17
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.ELetterUI
{

    public partial class NewsLetterSubscribeModuleModule : SiteModuleControl
    {
        // FeatureGuid 6c358bd7-6b78-4b3f-a56a-b1146dfa4c34

        private string NewsletterButtonTextSetting = string.Empty;
        private string NewsletterWatermarkTextSetting = string.Empty;
        private string NewsletterThankYouMessageSetting = string.Empty;
        private bool NewsletterShowListSetting = true;
        private bool NewsletterIncludeDescriptionInListSetting = false;
        private bool NewsletterShowFormatOptionsSetting = false;
        private bool NewsletterHtmlIsDefaultSetting = true;
        private bool NewsletterShowmoreInfoLinkSetting = false;
        private string NewsletterMoreInfoTextSetting = string.Empty;
        private bool NewsletterShowPreviousEditionLinksSetting = false;
        private int NewsletterOverrideInputWidthSetting = 0;
        private string CustomCssClassSetting = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            // make this usique in case multiple instances on the same page
            subscribe1.ValidationGroup = "subscribe" + ModuleId.ToInvariantString();

            if (NewsletterButtonTextSetting.Length > 0)
            {
                subscribe1.ButtonText = NewsletterButtonTextSetting;
            }

            if (NewsletterWatermarkTextSetting.Length > 0)
            {
                subscribe1.WatermarkText = NewsletterWatermarkTextSetting;
            }

            if (NewsletterThankYouMessageSetting.Length > 0)
            {
                subscribe1.ThankYouMessage = NewsletterThankYouMessageSetting;
            }

            if (NewsletterMoreInfoTextSetting.Length > 0)
            {
                subscribe1.MoreInfoText = NewsletterMoreInfoTextSetting;
            }

            subscribe1.ShowList = NewsletterShowListSetting;
            subscribe1.IncludeDescriptionInList = NewsletterIncludeDescriptionInListSetting;
            subscribe1.ShowFormatOptions = NewsletterShowFormatOptionsSetting;
            subscribe1.HtmlIsDefault = NewsletterHtmlIsDefaultSetting;
            subscribe1.ShowMoreInfoLink = NewsletterShowmoreInfoLinkSetting;
            subscribe1.ShowPreviousEditionsLink = NewsletterShowPreviousEditionLinksSetting;
            subscribe1.OverrideInputWidth = NewsletterOverrideInputWidthSetting;


        }


        private void LoadSettings()
        {
            if (Settings.Contains("NewsletterButtonTextSetting"))
            {
                NewsletterButtonTextSetting = Settings["NewsletterButtonTextSetting"].ToString();
            }

            if (Settings.Contains("NewsletterWatermarkTextSetting"))
            {
                NewsletterWatermarkTextSetting = Settings["NewsletterWatermarkTextSetting"].ToString();
            }

            if (Settings.Contains("NewsletterThankYouMessageSetting"))
            {
                NewsletterThankYouMessageSetting = Settings["NewsletterThankYouMessageSetting"].ToString();
            }

            if (Settings.Contains("NewsletterMoreInfoTextSetting"))
            {
                NewsletterMoreInfoTextSetting = Settings["NewsletterMoreInfoTextSetting"].ToString();
            }

            if (Settings.Contains("CustomCssClassSetting"))
            {
                CustomCssClassSetting = Settings["CustomCssClassSetting"].ToString();
            }


            if (CustomCssClassSetting.Length > 0)
            {
                pnlOuterWrap.SetOrAppendCss(CustomCssClassSetting); 
            }

            NewsletterShowListSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterShowListSetting", NewsletterShowListSetting);

            NewsletterIncludeDescriptionInListSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterIncludeDescriptionInListSetting", NewsletterIncludeDescriptionInListSetting);

            NewsletterShowFormatOptionsSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterShowFormatOptionsSetting", NewsletterShowFormatOptionsSetting);

            NewsletterHtmlIsDefaultSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterHtmlIsDefaultSetting", NewsletterHtmlIsDefaultSetting);

            NewsletterShowmoreInfoLinkSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterShowmoreInfoLinkSetting", NewsletterShowmoreInfoLinkSetting);

            NewsletterShowPreviousEditionLinksSetting = WebUtils.ParseBoolFromHashtable(
                Settings, "NewsletterShowPreviousEditionLinksSetting", NewsletterShowPreviousEditionLinksSetting);

            NewsletterOverrideInputWidthSetting = WebUtils.ParseInt32FromHashtable(
                Settings, "NewsletterOverrideInputWidthSetting", NewsletterOverrideInputWidthSetting);

            

        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion


    }
}
