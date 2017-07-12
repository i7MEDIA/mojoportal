
#region using statements

using System;
using System.Data;
using System.Configuration;
using System.Collections;
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
using mojoPortal.Business;
using Resources;

#endregion

namespace mojoPortal.Web.ContentUI
{
    /// <summary>
    /// Author:					
    /// Created:				4/1/2007
    /// Last Modified:			4/1/2007
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// This module can be installed an used just to force a column to be 
    /// displayed even if no other content is in it
    /// </summary>
    public partial class EmptyModule : SiteModuleControl
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
            //TitleControl.Visible = !this.RenderInWebPartMode;
            //if (this.ModuleConfiguration != null)
            //{
            //    this.Title = this.ModuleConfiguration.ModuleTitle;
            //    this.Description = this.ModuleConfiguration.FeatureName;
            //}


        }


        private void PopulateLabels()
        {
            //TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {


        }


    }
}