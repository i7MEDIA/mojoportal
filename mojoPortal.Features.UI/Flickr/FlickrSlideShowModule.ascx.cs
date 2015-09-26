// Author:					Joe Audette
// Created:					2009-09-03
// Last Modified:			2009-10-24
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
using System.Data;
using System.Configuration;
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
using Resources;

namespace mojoPortal.Features.UI
{
	
    public partial class FlickrSlideShowModule : SiteModuleControl
    {
		// FeatureGuid 6e2c97e2-31f5-47a6-9611-c129b8cca554

        private string FlickrUserName = string.Empty;
        private string FlickrApiKey = string.Empty;
        private string SlideShowTheme = "LightTheme";
        private int SlideShowWidth = 640;
        private int SlideShowHeight = 480;
        private bool SlideShowWindowlessMode = false;
		
		#region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion
		
        protected void Page_Load(object sender, EventArgs e)
        {
			
            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
			//TitleControl.EditUrl = SiteRoot + "/FlickrSlideShow/FlickrSlideShowEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            slideShow.FlickrUserName = FlickrUserName;
            slideShow.FlickrApiKey = FlickrApiKey;
            slideShow.Height = SlideShowHeight;
            slideShow.Width = SlideShowWidth;
            slideShow.Theme = SlideShowTheme;
            slideShow.Windowless = SlideShowWindowlessMode;

            if ((FlickrUserName.Length == 0) || (FlickrApiKey.Length == 0))
            {
                pnlInnerBody.Visible = false;
                pnlNotConfig.Visible = true;
            }

        }


        private void PopulateLabels()
        {
			//TitleControl.EditText = "Edit";
            lblNotConfigured.Text = FlickrResources.FlickrNotConfiguredWarning;
        }

        private void LoadSettings()
        {
            if (Settings.Contains("FlickrUserName"))
            {
                FlickrUserName = Settings["FlickrUserName"].ToString();
            }

            if (Settings.Contains("FlickrApiKey"))
            {
                FlickrApiKey = Settings["FlickrApiKey"].ToString();
            }

            if (Settings.Contains("SlideShowTheme"))
            {
                SlideShowTheme = Settings["SlideShowTheme"].ToString();
            }

            SlideShowHeight = WebUtils.ParseInt32FromHashtable(
                Settings, "SlideShowHeight", SlideShowHeight);

            SlideShowWidth = WebUtils.ParseInt32FromHashtable(
                Settings, "SlideShowWidth", SlideShowWidth);

            SlideShowWindowlessMode = WebUtils.ParseBoolFromHashtable(
                Settings, "SlideShowWindowlessMode", SlideShowWindowlessMode);

            

        }

		
    }
}
