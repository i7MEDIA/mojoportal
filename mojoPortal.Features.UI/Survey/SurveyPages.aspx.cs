/// Author:					Rob Henry
/// Created:				2007-09-25
/// Last Modified:			2010-11-08
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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using SurveyFeature.Business;
using Resources;

namespace SurveyFeature.UI
{
    public partial class SurveyPagesPage : mojoBasePage
    {

        private Guid surveyGuid = Guid.Empty;
        private Survey survey = null;
        private string editContentImage = WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
        private int pageId;
        private int moduleId;
        private mojoPortal.Business.Module currentModule = null;

        #region protected properties

        protected int PageId
        {
            get { return pageId; }
        }

        protected int ModuleId
        {
            get { return moduleId; }
        }

        protected string EditContentImage
        {
            get { return editContentImage; }
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

            if (!UserCanEditModule(moduleId, Survey.FeatureGuid))
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
            }

            PopulateLabels();
            PopulateControls();

            
        }

        private void PopulateControls()
        {

            if (survey == null) return;

            lnkPages.Text = string.Format(CultureInfo.InvariantCulture,
                SurveyResources.SurveyPagesLabelFormatString,
                survey.SurveyName);

            Title = SiteUtils.FormatPageTitle(siteSettings, lnkPages.Text);

            heading.Text = lnkPages.Text;

            if (currentModule != null)
            {

                lnkSurveys.Text = string.Format(CultureInfo.InvariantCulture,
                    SurveyResources.ChooseActiveSurveyFormatString,
                    currentModule.ModuleTitle);
            }



            if (Page.IsPostBack) return;
            

            BindGrid();
               
        }

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkSurveys.Text = SurveyResources.ChooseActiveSurveyLink;
            lnkSurveys.NavigateUrl = SiteRoot + "/Survey/Surveys.aspx?pageid="
                + pageId.ToInvariantString() + "&mid="
                + ModuleId.ToInvariantString();


            lnkPages.Text = SurveyResources.SurveyPagesBreadCrumbText;
            lnkPages.NavigateUrl = Request.RawUrl;

            lnkAddNew.Text = Resources.SurveyResources.PageAddButton;
            //lnkAddNew.ToolTip = Resources.SurveyResources.PageAddButtonToolTip;

            
            lnkAddNew.NavigateUrl = SiteRoot + "/Survey/SurveyPageEdit.aspx?SurveyGuid=" + surveyGuid.ToString() 
                + "&pageid=" + pageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

            grdSurveyPages.Columns[0].HeaderText = SurveyResources.PagesGridEditDeleteHeader;
            grdSurveyPages.Columns[1].HeaderText = SurveyResources.PagesGridPageTitleHeader;
            grdSurveyPages.Columns[2].HeaderText = SurveyResources.PagesGridPageEnabledHeader;
            grdSurveyPages.Columns[3].HeaderText = SurveyResources.PagesGridQuestionCountHeader;
            grdSurveyPages.Columns[4].HeaderText = SurveyResources.PagesGridMoveUpOrDownHeader;
            

        }

        private void LoadSettings()
        {
            surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

            currentModule = GetModule(moduleId, Survey.FeatureGuid);
               
            if(surveyGuid != Guid.Empty)
            {
                survey = new Survey(surveyGuid);
                if(survey.SiteGuid != siteSettings.SiteGuid)
                {
                    surveyGuid = Guid.Empty;
                    survey = null;
                }
            }

            AddClassToBody("surveypages");
        }

        //private void SetupBreadCrumb()
        //{
        //    lnkSurveys.NavigateUrl = "~/Survey/Surveys.aspx?PageId=" + pageId + "&mid=" + ModuleId ;
        //}

        private void BindGrid()
        {
            List<SurveyFeature.Business.Page> pages = SurveyFeature.Business.Page.GetAll(surveyGuid);
            grdSurveyPages.DataSource = pages;
            grdSurveyPages.DataBind();

            //hide the grid and display a message if no questions have been added.

            if (pages.Count == 0)
            {
                grdSurveyPages.Visible = false;
                lblMessages.Text = Resources.SurveyResources.SurveyHasNoPagesWarning;
            }
            else
            {
                grdSurveyPages.Visible = true;
            }

        }

        public Guid SurveyGuid
        {
            get { return surveyGuid; }
        }

        #region Events

        void grdSurveyPages_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
            {
                GridViewRow row = e.Row;
                ImageButton deleteButton = (ImageButton)row.FindControl("btnDelete");
                UIHelper.AddConfirmationDialog(deleteButton, Resources.SurveyResources.PageDeleteDeletesAllQuestionsWarning);

                
            }
            

        }

        void grdSurveyPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            

        }

        

        void grdSurveyPages_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            List<SurveyFeature.Business.Page> pages = SurveyFeature.Business.Page.GetAll(surveyGuid);
            Guid currentPageGuid = new Guid(e.CommandArgument.ToString());

            SurveyFeature.Business.Page currentPage = null;

            SurveyFeature.Business.Page swapPage;

            int currentItemIndex = -1;
            int i = 0;

            foreach (SurveyFeature.Business.Page p in pages)
            {
                if (p.SurveyPageGuid == currentPageGuid)
                {
                    currentPage = p;
                    currentItemIndex = i;
                }

                i += 1;
                    

            }

            if(currentPage == null)return;
           
            switch (e.CommandName)
            {
                case "up":

                    if (currentItemIndex > 0)
                    {
                        
                        swapPage = pages[currentItemIndex - 1];

                        currentPage.PageOrder = currentItemIndex - 1;
                        swapPage.PageOrder = currentItemIndex;

                        currentPage.Save();
                        swapPage.Save();
                    }

                    break;

                case "down":

                    if (currentItemIndex < pages.Count - 1)
                    {
                     
                        swapPage = pages[currentItemIndex + 1];

                        currentPage.PageOrder = currentItemIndex + 1;
                        swapPage.PageOrder = currentItemIndex;

                        currentPage.Save();
                        swapPage.Save();
                    }

                    break;
                case "delete":
                    //ImageButton button = (ImageButton)e.CommandSource;
                    SurveyFeature.Business.Page.Delete(currentPageGuid);
                    break;
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
            //BindGrid();

        }
        
        //protected void RptPages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e == null)
        //    {
        //        throw new ArgumentNullException("e");
        //    }

        //    if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Footer)
        //    {
        //        RepeaterItem item = e.Item;
        //        ImageButton deleteButton = (ImageButton)item.FindControl("btnDelete");
        //        UIHelper.AddConfirmationDialog(deleteButton, Resources.SurveyResources.PageDeleteDeletesAllQuestionsWarning);

        //        // Not good to make a db hit for each row here - JA
        //        //HyperLink link = (HyperLink)item.FindControl("questionsLink");
        //        //link.Text = SurveyFeature.Business.Page.QuestionCount(new Guid(deleteButton.CommandArgument)).ToString(CultureInfo.CurrentCulture);
            
        //    }
        //}

        //protected void RptPages_ItemCommand(object sender, RepeaterCommandEventArgs e)
        //{
        //    if (e == null)
        //    {
        //        throw new ArgumentNullException("e");
        //    }

        //    //deal with up, down or delete statements
        //    int currentItemIndex = e.Item.ItemIndex;

        //    switch (e.CommandName)
        //    {
        //        case "up":

        //            if (currentItemIndex > 0)
        //            {
        //                SurveyFeature.Business.Page currentPage = new SurveyFeature.Business.Page(new Guid(e.CommandArgument.ToString()));

        //                RepeaterItem aboveRowItem = rptPages.Items[currentItemIndex - 1];
        //                ImageButton upImageButton = (ImageButton)aboveRowItem.FindControl("btnUp");
        //                SurveyFeature.Business.Page swapPage = new SurveyFeature.Business.Page(new Guid(upImageButton.CommandArgument));

        //                currentPage.PageOrder = currentItemIndex - 1;
        //                swapPage.PageOrder = currentItemIndex;

        //                currentPage.Save();
        //                swapPage.Save();
        //            }

        //            break;
        //        case "down":

        //            if (currentItemIndex < rptPages.Items.Count - 1)
        //            {
        //                SurveyFeature.Business.Page currentPage = new SurveyFeature.Business.Page(new Guid(e.CommandArgument.ToString()));

        //                RepeaterItem belowRowItem = rptPages.Items[currentItemIndex + 1];
        //                ImageButton downImageButton = (ImageButton)belowRowItem.FindControl("btnDown");
        //                SurveyFeature.Business.Page swapPage = new SurveyFeature.Business.Page(new Guid(downImageButton.CommandArgument));

        //                currentPage.PageOrder = currentItemIndex + 1;
        //                swapPage.PageOrder = currentItemIndex;

        //                currentPage.Save();
        //                swapPage.Save();
        //            }

        //            break;
        //        case "delete":
        //            ImageButton button = (ImageButton)e.CommandSource;
        //            SurveyFeature.Business.Page.Delete(new Guid(button.CommandArgument));
        //            break;
        //    }

        //    BindGrid();
        //}

        //protected void BtnAddNewSurveyPage_Click(object sender, ImageClickEventArgs e)
        //{
        //    WebUtils.SetupRedirect(this, "~/Survey/SurveyPageEdit.aspx?SurveyGuid=" + surveyGuid + "&PageId=" + pageId + "&mid=" + ModuleId);
        //}

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

            this.Load += new EventHandler(this.Page_Load);
            this.grdSurveyPages.RowCommand += new GridViewCommandEventHandler(grdSurveyPages_RowCommand);
            this.grdSurveyPages.RowDeleting += new GridViewDeleteEventHandler(grdSurveyPages_RowDeleting);
            this.grdSurveyPages.RowDataBound += new GridViewRowEventHandler(grdSurveyPages_RowDataBound);
            
            //btnAddNewSurveyPage.Click += new ImageClickEventHandler(BtnAddNewSurveyPage_Click);
            //rptPages.ItemDataBound += new RepeaterItemEventHandler(RptPages_ItemDataBound);
            //rptPages.ItemCommand += new RepeaterCommandEventHandler(RptPages_ItemCommand);
            SuppressPageMenu();
            
        }

        

        

        #endregion
    }
}
