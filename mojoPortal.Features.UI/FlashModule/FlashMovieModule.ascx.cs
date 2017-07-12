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

namespace mojoPortal.Web.FlashUI
{
    /// <summary>
    /// Author:					
    /// Created:				2007-05-01
    /// Last Modified:			2007-05-01
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public partial class FlashMovieModule : SiteModuleControl
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
            // TODO: finish implenting, need db to store settings and params for movie

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            //TitleControl.EditUrl = SiteRoot + "/FlashMovie/FlashMovieEdit.aspx";
            //TitleControl.Visible = !this.RenderInWebPartMode;
            //if (this.ModuleConfiguration != null)
            //{
            //    this.Title = this.ModuleConfiguration.ModuleTitle;
            //    this.Description = this.ModuleConfiguration.FeatureName;
            //}


        }


        private void PopulateLabels()
        {
            TitleControl.EditText = "Edit";
        }

        private void LoadSettings()
        {


        }


    }
}
