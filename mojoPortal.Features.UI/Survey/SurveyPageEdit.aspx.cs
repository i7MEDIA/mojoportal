/// Author:					Rob Henry
/// Created:				2007-09-09
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using SurveyFeature.Business;
using Resources;

namespace SurveyFeature.UI
{
    public partial class PageEditPage : NonCmsBasePage
    {
        private Guid surveyGuid = Guid.Empty;
        private Survey survey = null;
        private Guid surveyPageGuid = Guid.Empty;
        private SurveyFeature.Business.Page surveyPage;
        
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

            LoadSettings();

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
            
            if (survey != null)
            {
                if (surveyPage != null)
                {
                    heading.Text = string.Format(CultureInfo.InvariantCulture,
                        SurveyResources.PageEditFormatString,
                        survey.SurveyName);
                }
                else
                {
                     heading.Text = string.Format(CultureInfo.InvariantCulture,
                    SurveyResources.PageEditNewPageFormatString,
                    survey.SurveyName);

                }

                lnkPages.Text = string.Format(CultureInfo.InvariantCulture,
                SurveyResources.SurveyPagesLabelFormatString,
                survey.SurveyName);
            }

            
            
            
            if (Page.IsPostBack) return;

            if (surveyPage != null)
            {
                txtPageTitle.Text = surveyPage.PageTitle;
                chkPageEnabled.Checked = surveyPage.PageEnabled;
            }
            

        }

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkSurveys.Text = SurveyResources.SurveyBreadCrumbText;
            lnkSurveys.NavigateUrl = SiteRoot +"/Survey/Surveys.aspx?pageid=" 
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            lnkPages.NavigateUrl = SiteRoot
                + "/Survey/SurveyPages.aspx?SurveyGuid="
                + surveyGuid.ToString() + "&pageid=" + pageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

            lnkPageEdit.Text = SurveyResources.SurveyPageEditBreadCrumbText;
            lnkPageEdit.NavigateUrl = Request.RawUrl;

            

            btnSave.Text = SurveyResources.PageEditSaveButton;
            btnSave.ToolTip = SurveyResources.PageEditSaveButtonToolTip;
            btnCancel.Text = SurveyResources.PageEditCancelButton;
            btnCancel.ToolTip = SurveyResources.PageEditCancelButtonToolTip;
        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            surveyPageGuid = WebUtils.ParseGuidFromQueryString("SurveyPageGuid", Guid.Empty);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);
            
            if (surveyPageGuid == Guid.Empty)
            {
                surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
            }
            else
            {
                surveyPage = new SurveyFeature.Business.Page(surveyPageGuid);
                surveyGuid = surveyPage.SurveyGuid;
            }



            if (surveyGuid != Guid.Empty)
            {
                survey = new Survey(surveyGuid);
                if (survey.SiteGuid != siteSettings.SiteGuid)
                {
                    surveyGuid = Guid.Empty;
                    survey = null;

                    surveyPageGuid = Guid.Empty;
                    surveyPage = null;
                    
                }
            }

            AddClassToBody("surveypageedit");
        }

        

        private void SavePage(SurveyFeature.Business.Page page)
        {
            page.PageTitle = txtPageTitle.Text;
            page.PageEnabled = chkPageEnabled.Checked;
            
            if (surveyPageGuid == Guid.Empty)
            {
                //new question so we need to know the page it will live on
                page.SurveyGuid = surveyGuid;
            }

            page.Save();
        }

        #region Events

        void BtnCancel_Click(object sender, EventArgs e)
        {
            Guid redirectSurveyGuid = Guid.Empty;

            if (surveyPageGuid == Guid.Empty)
            {
                redirectSurveyGuid = surveyGuid;
            }
            else
            {
                SurveyFeature.Business.Page page = new SurveyFeature.Business.Page(surveyPageGuid);
                redirectSurveyGuid = page.SurveyGuid;
            }

            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" 
                + redirectSurveyGuid.ToString() + "&pageid=" + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString());
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("survey");
            if (Page.IsValid)
            {
                SurveyFeature.Business.Page page = new SurveyFeature.Business.Page(surveyPageGuid);
                SavePage(page);

                WebUtils.SetupRedirect(this, SiteRoot + "/Survey/SurveyPages.aspx?SurveyGuid=" 
                    + page.SurveyGuid.ToString() + "&pageid=" + pageId.ToInvariantString() + "&mid=" + moduleId.ToInvariantString());
            }
        }

        #endregion

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(BtnSave_Click);
            this.btnCancel.Click +=new EventHandler(BtnCancel_Click);

            SuppressPageMenu();
            
        }

        #endregion

    }
}

