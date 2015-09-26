/// Author:					Rob Henry
/// Created:				2007-05-09
/// Last Modified:			2009-12-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using SurveyFeature.Business;

namespace SurveyFeature.UI
{

    public partial class QuestionsPage : NonCmsBasePage
    {
        private Guid surveyGuid = Guid.Empty;
        private Guid surveyPageGuid = Guid.Empty;
        private Survey survey = null;
        private SurveyFeature.Business.Page surveyPage = null;
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

            

            if (currentModule != null)
            {

                lnkSurveys.Text = string.Format(CultureInfo.InvariantCulture,
                    SurveyResources.ChooseActiveSurveyFormatString,
                    currentModule.ModuleTitle);
            }

            if (surveyPage != null)
            {
                lnkQuestions.Text = string.Format(CultureInfo.InvariantCulture,
                    SurveyResources.QuestionsPageFormatString,
                    surveyPage.PageTitle);

                heading.Text = lnkQuestions.Text;

            }

            if (Page.IsPostBack) return;

            BindGrid();

            ddQuestionType.DataSource = EnumHelper<QuestionType>.GetValues();
            ddQuestionType.DataBind();
	    }      
            
        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkSurveys.Text = SurveyResources.ChooseActiveSurveyLink;
            lnkSurveys.NavigateUrl = SiteRoot + "/Survey/Surveys.aspx?pageid="
                + pageId.ToInvariantString() + "&mid=" + ModuleId.ToInvariantString();


            lnkPages.Text = SurveyResources.SurveyPagesBreadCrumbText;
            lnkPages.NavigateUrl = SiteRoot 
                + "/Survey/SurveyPages.aspx?SurveyGuid=" 
                + surveyGuid.ToString()  + "&pageid=" + pageId.ToInvariantString()
                + "&mid=" + ModuleId.ToInvariantString();

            lnkQuestions.Text = SurveyResources.SurveyQuestionsBreadCrumbText;
            lnkQuestions.NavigateUrl = Request.RawUrl;

            btnNewQuestion.Text = SurveyResources.QuestionAddNewButtonText;
            btnNewQuestion.ToolTip = SurveyResources.QuestionAddNewButtonText;

            grdSurveyQuestions.Columns[0].HeaderText = SurveyResources.QuestionsGridEditDeleteHeader;
            grdSurveyQuestions.Columns[1].HeaderText = SurveyResources.QuestionsGridTextHeader;
            grdSurveyQuestions.Columns[2].HeaderText = SurveyResources.QuestionsGridTypeHeader;
            grdSurveyQuestions.Columns[3].HeaderText = SurveyResources.QuestionsGridRequiredHeader;
            grdSurveyQuestions.Columns[4].HeaderText = SurveyResources.QuestionsGridMoveUpOrDownHeader;

        }

        private void LoadSettings()
        {
            surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
            surveyPageGuid = WebUtils.ParseGuidFromQueryString("SurveyPageGuid", Guid.Empty);
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            currentModule = GetModule(moduleId, Survey.FeatureGuid);
            
            if (surveyGuid != Guid.Empty)
            {
                survey = new Survey(surveyGuid);
                if (survey.SiteGuid != siteSettings.SiteGuid)
                {
                    surveyGuid = Guid.Empty;
                    survey = null;
                }
                else
                {
                    if (surveyPageGuid != Guid.Empty)
                    {
                        surveyPage = new SurveyFeature.Business.Page(surveyPageGuid);
                        if (surveyPage.SurveyGuid != survey.SurveyGuid)
                        {
                            surveyPageGuid = Guid.Empty;
                            surveyPage = null;

                        }
                    }
                }
            }

            AddClassToBody("surveyquestions");


        }

        //private void SetupBreadCrumb()
        //{
        //    lnkSurveys.NavigateUrl = "~/Survey/Surveys.aspx?PageId=" + PageId + "&mid=" + ModuleId ;
        //    lnkPages.NavigateUrl = "~/Survey/SurveyPages.aspx?SurveyGuid=" + surveyGuid + "&PageId=" + PageId + "&mid=" + ModuleId;
        //}

        private void BindGrid()
        {
            List<Question> questions = Question.GetAll(surveyPageGuid);
            grdSurveyQuestions.DataSource = questions;
            grdSurveyQuestions.DataBind();

            //hide the grid and display a message if no questions have been added.

            if (questions.Count == 0)
            {
                grdSurveyQuestions.Visible = false;
                lblMessages.Text = Resources.SurveyResources.PageHasNoQuestionsWarning;
            }
            else
            {
                grdSurveyQuestions.Visible = true;
            }

        }

        protected string GetQuestionTypeText(string questionTypeId)
        {
            int id = int.Parse(questionTypeId, CultureInfo.CurrentCulture);
            return ((QuestionType)id).ToString();
        }

        protected string FormatQuestionTextForDisplay(string questionText)
        {
            //remove all tags and trim to 40 characters
            int trimSize = 40;
            string questionTextWithoutTags = System.Text.RegularExpressions.Regex.Replace(questionText, "<[^>]*>", string.Empty);

            if (questionTextWithoutTags.Length > trimSize)
            {
                return questionTextWithoutTags.Substring(0, trimSize) + "..";
            }

            return questionTextWithoutTags;
        }

        #region Events

        void grdSurveyQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            

        }

        void grdSurveyQuestions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Guid currentQuestionGuid = new Guid(e.CommandArgument.ToString());
            List<Question> questions = Question.GetAll(surveyPageGuid);
            Question currentQuestion = null;
            Question swapQuestion;

            int currentItemIndex = -1;
            int i = 0;

            foreach (Question q in questions)
            {
                if (q.QuestionGuid == currentQuestionGuid)
                {
                    currentQuestion = q;
                    currentItemIndex = i;
                }

                i += 1;


            }

            if (currentQuestion == null) return;

            switch (e.CommandName)
            {
                case "up":

                    if (currentItemIndex > 0)
                    {
                        swapQuestion = questions[currentItemIndex - 1];

                        currentQuestion.QuestionOrder = currentItemIndex - 1;
                        swapQuestion.QuestionOrder = currentItemIndex;

                        currentQuestion.Save();
                        swapQuestion.Save();
                    }

                    break;
                case "down":

                    if (currentItemIndex < questions.Count - 1)
                    {
                        swapQuestion = questions[currentItemIndex + 1];

                        currentQuestion.QuestionOrder = currentItemIndex + 1;
                        swapQuestion.QuestionOrder = currentItemIndex;

                        currentQuestion.Save();
                        swapQuestion.Save();
                    }

                    break;
                case "delete":
                    //ImageButton button = (ImageButton)e.CommandSource;
                    Question.Delete(currentQuestionGuid);
                    break;
            }


            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        //private void BtnAddNewQuestion_Click(object sender, EventArgs e)
        //{
        //    WebUtils.SetupRedirect(this, "~/Survey/SurveyQuestionEdit.aspx?NewQuestionType=" + ddQuestionType.SelectedValue
        //        + "&SurveyPageGuid=" + surveyPageGuid + "&PageId=" + PageId + "&mid=" + ModuleId);
        //}

        void btnNewQuestion_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/SurveyQuestionEdit.aspx?NewQuestionType=" + ddQuestionType.SelectedValue
                + "&SurveyPageGuid=" + surveyPageGuid + "&pageId=" + PageId.ToInvariantString() + "&mid=" + ModuleId.ToInvariantString());
        }

        //private void rptQuestions_ItemCommand(object sender, RepeaterCommandEventArgs e)
        //{
        //    //deal with up, down or delete statements
        //    int currentItemIndex = e.Item.ItemIndex;

        //    switch (e.CommandName)
        //    {
        //        case "up":
                    
        //            if (currentItemIndex > 0)
        //            {
        //                Question currentQuestion = new Question(new Guid(e.CommandArgument.ToString()));

        //                RepeaterItem aboveRowItem = rptQuestions.Items[currentItemIndex - 1];
        //                ImageButton upImageButton = (ImageButton) aboveRowItem.FindControl("btnUp");
        //                Question swapQuestion = new Question(new Guid(upImageButton.CommandArgument));

        //                currentQuestion.QuestionOrder = currentItemIndex - 1;
        //                swapQuestion.QuestionOrder = currentItemIndex;

        //                currentQuestion.Save();
        //                swapQuestion.Save();
        //            }
                                        
        //            break;
        //        case "down":

        //            if (currentItemIndex < rptQuestions.Items.Count - 1)
        //            {
        //                Question currentQuestion = new Question(new Guid(e.CommandArgument.ToString()));

        //                RepeaterItem belowRowItem = rptQuestions.Items[currentItemIndex + 1];
        //                ImageButton downImageButton = (ImageButton)belowRowItem.FindControl("btnDown");
        //                Question swapQuestion = new Question(new Guid(downImageButton.CommandArgument));

        //                currentQuestion.QuestionOrder = currentItemIndex + 1;
        //                swapQuestion.QuestionOrder = currentItemIndex;

        //                currentQuestion.Save();
        //                swapQuestion.Save();
        //            }

        //            break;
        //        case "delete":
        //            ImageButton button = (ImageButton)e.CommandSource;
        //            Question.Delete(new Guid(button.CommandArgument));
        //            break;
        //    }
            
        //    BindGrid();
        //}

        protected void DdQuestionsPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void BtnAddNewSurveyQuestion_Click(object sender, ImageClickEventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/SurveyQuestionEdit.aspx?pageId=" + PageId.ToInvariantString() + "&SurveyPageGuid=" + surveyPageGuid 
                + "&NewQuestionType=" + ddQuestionType.SelectedValue + "&mid=" + ModuleId.ToInvariantString());
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

            this.Load += new EventHandler(this.Page_Load);
            //this.btnAddNewSurveyQuestion.Click += new ImageClickEventHandler(BtnAddNewSurveyQuestion_Click);
            this.btnNewQuestion.Click += new EventHandler(btnNewQuestion_Click);
            //rptQuestions.ItemCommand += new RepeaterCommandEventHandler(rptQuestions_ItemCommand);
            this.grdSurveyQuestions.RowCommand += new GridViewCommandEventHandler(grdSurveyQuestions_RowCommand);
            this.grdSurveyQuestions.RowDeleting += new GridViewDeleteEventHandler(grdSurveyQuestions_RowDeleting);

            SuppressPageMenu();
        }

        

        

        #endregion

    }
}
