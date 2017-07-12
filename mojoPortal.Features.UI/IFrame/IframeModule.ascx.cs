// Author:					
// Created:					2009-12-19
// Last Modified:			2009-12-19
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
using System.Text;
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

    public partial class IframeModule : SiteModuleControl
    {
        // FeatureGuid 36d66cea-a047-4411-ab29-1344493b5a33

        private string frameSrc = string.Empty;
        private string frameName = string.Empty;
        private string frameTitle = string.Empty;

        // deprecated - use styles instead
        private string frameAlign = string.Empty; // left, right, top, middle, bottom
        private string frameCss = string.Empty;

        private string frameBorder = "0"; // 0 or 1
        private string frameHeight = "100%";
        private string frameWidth = "100%";
        private string frameMarginHeight = "0";
        private string frameMarginWidth = "0";
        private string frameScrolling = "auto"; // yes, no, auto

        



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
            //TitleControl.EditUrl = SiteRoot + "/IframeModule/IframeModuleEdit.aspx";
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (frameSrc.Length == 0) { return; }

            StringBuilder markup = new StringBuilder();
            markup.Append("<iframe ");

            if (frameName.Length > 0) { markup.Append("name=\"" + frameName + "\" "); }
            if (frameTitle.Length > 0) { markup.Append("title=\"" + frameTitle + "\" "); }
            markup.Append("src=\"" + frameSrc + "\" ");
            if (frameAlign.Length > 0) { markup.Append("align=\"" + frameAlign + "\" "); }
            if (frameCss.Length > 0) { markup.Append("class=\"" + frameCss + "\" "); }
            markup.Append("frameborder=\"" + frameBorder + "\" ");
            markup.Append("height=\"" + frameHeight + "\" ");
            markup.Append("width=\"" + frameWidth + "\" ");
            if (frameMarginHeight.Length > 0) { markup.Append("marginheight=\"" + frameMarginHeight + "\" "); }
            if (frameMarginWidth.Length > 0) { markup.Append("marginwidth=\"" + frameMarginWidth + "\" "); }
            markup.Append("scrolling=\"" + frameScrolling + "\" ");

            markup.Append("></iframe>");

            litFrame.Text = markup.ToString();
            pnlWrapper.Controls.Add(litFrame);

        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {
            if (Settings.Contains("frameSrc"))
            {
                frameSrc = Settings["frameSrc"].ToString();
            }

            if (Settings.Contains("frameName"))
            {
                frameName = Settings["frameName"].ToString();
            }

            if (Settings.Contains("frameTitle"))
            {
                frameTitle = Settings["frameTitle"].ToString();
            }

            if (Settings.Contains("frameAlign"))
            {
                frameAlign = Settings["frameAlign"].ToString();
            }

            if (Settings.Contains("frameCss"))
            {
                frameCss = Settings["frameCss"].ToString();
            }

            if (Settings.Contains("frameBorder"))
            {
                frameBorder = Settings["frameBorder"].ToString();
            }

            if (Settings.Contains("frameHeight"))
            {
                frameHeight = Settings["frameHeight"].ToString();
            }

            if (Settings.Contains("frameWidth"))
            {
                frameWidth = Settings["frameWidth"].ToString();
            }

            if (Settings.Contains("frameMarginHeight"))
            {
                frameMarginHeight = Settings["frameMarginHeight"].ToString();
            }

            if (Settings.Contains("frameMarginWidth"))
            {
                frameMarginWidth = Settings["frameMarginWidth"].ToString();
            }

            if (Settings.Contains("frameScrolling"))
            {
                frameScrolling = Settings["frameScrolling"].ToString();
            }

        }
        


    }
}
