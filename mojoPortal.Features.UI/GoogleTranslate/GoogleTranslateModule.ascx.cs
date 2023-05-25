// Author:					
// Created:					2009-12-19
// Last Modified:			2010-10-25
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
using Resources;

namespace mojoPortal.Features.UI
{

    public partial class GoogleTranslateModule : SiteModuleControl
    {
        // FeatureGuid 5cb1a666-3de9-47df-8b52-ec0df2deeacf

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

        }

        #endregion

        private bool UseSimpleLayout = false;
        private bool ShowToolbar = false;
        private bool TrackInGoogleAnalytics = false;
        private bool AllowOnSecurePages = true;
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            PopulateLabels();
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

            gt1.ShowToolbar = ShowToolbar;
            gt1.TrackInGoogleAnalytics = TrackInGoogleAnalytics;
            gt1.AllowSecurePageTranslation = AllowOnSecurePages;
            if (UseSimpleLayout)
            {
                gt1.Layout = "SIMPLE";
            }
            else
            {
                gt1.Layout = "HORIZONTAL";
            }

        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {
            UseSimpleLayout = WebUtils.ParseBoolFromHashtable(Settings, "UseSimpleLayout", UseSimpleLayout);
            ShowToolbar = WebUtils.ParseBoolFromHashtable(Settings, "ShowToolbar", ShowToolbar);
            TrackInGoogleAnalytics = WebUtils.ParseBoolFromHashtable(Settings, "TrackInGoogleAnalytics", TrackInGoogleAnalytics);
            AllowOnSecurePages = WebUtils.ParseBoolFromHashtable(Settings, "AllowOnSecurePages", AllowOnSecurePages);
		}


    }
}
