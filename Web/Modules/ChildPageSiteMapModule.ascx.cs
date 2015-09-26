/// Author:					Joe Davis (i7MEDIA)
/// Created:				2011-02-09
/// Last Modified:		    2011-02-09
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
using System;
using System.Web.UI;

namespace mojoPortal.Web.UI
{
    public partial class ChildPageSiteMapModule : SiteModuleControl
    {
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
            //TitleControl.EditUrl = SiteRoot + "/Empty/EmptyEdit.aspx";

            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }


        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {
            ChildPageMenu1.ForceDisplay = !WebConfigSettings.EnforcePageSettingsInChildPageSiteMapModule;
            if (WebConfigSettings.HideMasterPageChildSiteMapWhenUsingModule)
            {
                Control c = Page.Master.FindControl("ChildPageMenu");
                if (c != null) { c.Visible = false; }

            }

            // TODO:? do we need to make this configurable separate from the standard child pae site map by using a different skin id?
            //ChildPageMenu1.MenuSkinID = "ChildSiteMap";

        }
    }
}