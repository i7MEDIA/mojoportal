/// Author:				
/// Created:			2004-12-24
///	Last Modified:		2010-12-06
///	
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.LinksUI
{
	public partial  class LinksModule : SiteModuleControl
    {
        protected ListConfiguration config = new ListConfiguration();

        
        protected void Page_Load(object sender, EventArgs e)
		{
            LoadSettings();
           
		}

        private void LoadSettings()
        {
            config = new ListConfiguration(Settings);

            //pnlContainer.ModuleId = ModuleId;
            
            Title1.EditUrl = SiteRoot + "/List/Edit.aspx";
            Title1.EditText = LinkResources.EditLinksAddLinkLabel;
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (IsEditable)
            {
                Title1.LiteralExtraMarkup =
                    "&nbsp;<a href='"
                    + SiteRoot
                    + "/List/EditIntro.aspx?pageid=" + PageId.ToInvariantString()
                    + "&amp;mid=" + ModuleId.ToInvariantString()
                    + "' class='ModuleEditLink' title='" + LinkResources.EditIntro + "'>" + LinkResources.EditIntro + "</a>";
            }

            if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }

            theList.Config = config;
            theList.PageId = PageId;
            theList.ModuleId = ModuleId;
            theList.IsEditable = IsEditable;
            theList.SiteRoot = SiteRoot;
            theList.ImageSiteRoot = ImageSiteRoot;
            

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
