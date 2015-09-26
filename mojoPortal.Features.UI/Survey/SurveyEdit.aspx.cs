/// Author:					Rob Henry
/// Created:				2007-02-10
/// Last Modified:			2012-05-08
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Editor;
using SurveyFeature.Business;
using Resources;

namespace SurveyFeature.UI
{
    public partial class SurveyEditPage : NonCmsBasePage
    {
        private Guid surveyGuid = Guid.Empty;
        private int pageId;
        private int moduleId;

        #region public properties

        public int PageId
        {
            get
            {
                return pageId;
            }
        }

        public int ModuleId
        {
            get
            {
                return moduleId;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();

            if (!UserCanEditModule(moduleId))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();

            PopulateControls();
                
            
        }

        private void PopulateControls()
        {

            if (Page.IsPostBack) return;

            if (surveyGuid != Guid.Empty)
            {
                Survey survey = new Survey(surveyGuid);
                txtSurveyName.Text = survey.SurveyName;
                edWelcomeMessage.Text = survey.StartPageText;
                edThankyouMessage.Text = survey.EndPageText;
            }
        }

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkSurveys.Text = SurveyResources.SurveyBreadCrumbText;
            lnkSurveyEdit.Text = SurveyResources.SurveyEditBreadCrumbText;
            lnkSurveys.NavigateUrl = SiteRoot + "/Survey/Surveys.aspx?pageid=" 
                + pageId.ToInvariantString() 
                + "&mid=" + moduleId.ToInvariantString();

            btnSave.Text = SurveyResources.SurveyEditSaveButton;
            btnSave.ToolTip = SurveyResources.SurveyEditSaveToolTip;
            btnCancel.Text = SurveyResources.SurveyEditCancelButton;
            btnCancel.ToolTip = SurveyResources.SurveyEditCancelButtonToolTip;

            edWelcomeMessage.WebEditor.ToolBar = ToolBar.Full;
            edWelcomeMessage.WebEditor.Height = Unit.Pixel(200);
            edThankyouMessage.WebEditor.ToolBar = ToolBar.Full;
            edThankyouMessage.WebEditor.Height = Unit.Pixel(200);
        }

        private void LoadParams()
        {
            surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            AddClassToBody("surveyedit");
        }

        

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            SuppressPageMenu();
            SiteUtils.SetupEditor(edWelcomeMessage, AllowSkinOverride, this);
            SiteUtils.SetupEditor(edThankyouMessage, AllowSkinOverride, this);
        }

        #endregion

        #region Events

        void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("survey");

            if (Page.IsValid)
            {
                Survey survey = new Survey(surveyGuid);
                survey.SurveyName = txtSurveyName.Text;
                survey.SiteGuid = siteSettings.SiteGuid;
                survey.StartPageText = edWelcomeMessage.Text;
                survey.EndPageText = edThankyouMessage.Text;
                survey.Save();

                WebUtils.SetupRedirect(this, SiteRoot + "/Survey/Surveys.aspx?pageid=" 
                    + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString());
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/Surveys.aspx?pageid=" 
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString());
        }

        #endregion
    }
}