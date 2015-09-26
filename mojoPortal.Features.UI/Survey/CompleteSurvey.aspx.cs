/// Author:					Rob Henry
/// Created:				2007-11-10
/// Last Modified:			2008-01-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using SurveyFeature.Business;
using Resources;

namespace SurveyFeature.UI
{
    public partial class CompleteSurveyPage : mojoBasePage
    {

        private int pageId;
        private int moduleId;

        private Guid surveyGuid = Guid.Empty;
        private Guid surveyPageGuid = Guid.Empty;
        private Survey survey;
        private SurveyFeature.Business.Page page;
        private List<Question> questions;
        private Guid previousSurveyPageGuid = Guid.Empty;
        private Guid nextSurveyPageGuid = Guid.Empty;
        private SurveyManager manager;


        #region protected properties

        protected int PageId
        {
            get { return pageId;}
        }

        protected int ModuleId
        {
            get { return moduleId; }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            LoadSettings();

           
            if (!UserHasPermissionToComplete())
            {
                SiteUtils.RedirectToAccessDeniedPage();
            }

           
            if ((survey.SurveyGuid == Guid.Empty)||(survey.SiteGuid != siteSettings.SiteGuid))
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
                return;
            }

            PopulateLabels();

            manager = new SurveyManager(surveyGuid);
            previousSurveyPageGuid = manager.GetPreviousSurveyPageGuid(surveyPageGuid);
            nextSurveyPageGuid = manager.GetNextSurveyPageGuid(surveyPageGuid);

            SetNavigationButtons();
           
            RenderQuestions();
            

        }

        private void RenderQuestions()
        {
            IQuestion control = null;
            Collection<QuestionOption> options;

            foreach (Question question in questions)
            {
                switch ((QuestionType)question.QuestionTypeId)
                {
                    case QuestionType.TextBox:
                        control = new TextBoxQuestion(question);
                        break;
                    case QuestionType.DropDownList:
                        options = QuestionOption.GetAll(question.QuestionGuid);
                        control = new DropDownListQuestion(question, options);
                        break;
                    case QuestionType.RadioButtonList:
                        options = QuestionOption.GetAll(question.QuestionGuid);
                        control = new RadioButtonListQuestion(question, options);
                        break;
                    case QuestionType.CheckBoxList:
                        options = QuestionOption.GetAll(question.QuestionGuid);
                        control = new CheckBoxListQuestion(question, options);
                        break;
                    case QuestionType.Date:
                        control = new DateQuestion(question);
                        break;
                    default:
                        throw new ArgumentException("Invalid question type");
                }

                PopulateAnswer(control);

                HtmlGenericControl row = new HtmlGenericControl("div");
                HtmlGenericControl spacer = new HtmlGenericControl("div");
                row.Attributes.Add("class", "settingrow");
                spacer.Attributes.Add("class", "clearpanel");
                row.Controls.Add((CompositeControl)control);

                pnlQuestions.Controls.Add(row);
                pnlQuestions.Controls.Add(spacer);
            }
        }

        private void SetNavigationButtons()
        {
            if (previousSurveyPageGuid == Guid.Empty)
            {
                btnSurveyBack.Visible = false;
            }

            if (nextSurveyPageGuid == Guid.Empty)
            {
                btnSurveyForward.Text = SurveyResources.SurveyButtonSubmit;
                btnSurveyForward.ToolTip = SurveyResources.SurveyButtonSubmitToolTip;
                btnSurveyForward.CommandName = "Submit";
            }
        }

        private void SaveAnswers()
        {

            foreach (Control control in pnlQuestions.Controls)
            {
                HtmlGenericControl convertedControl = control as HtmlGenericControl;

                //check this is a question container and not a spacer
                if (convertedControl as HtmlGenericControl != null && convertedControl.Attributes["class"] == "settingrow")
                {
                    IQuestion question = (IQuestion)control.Controls[0];
                    if (!String.IsNullOrEmpty(question.Answer))
                    {
                        QuestionAnswer answer = new QuestionAnswer(question.QuestionGuid, ResponseGuid);
                        answer.Answer = question.Answer;
                        answer.Save();
                    }
                }
            }
        }

        private void PopulateAnswer(IQuestion question)
        {
            //QuestionAnswer answer = new QuestionAnswer(question.QuestionGuid, ResponseGuid);
            QuestionAnswer answer = new QuestionAnswer(question.QuestionGuid, ResponseGuid);

            if (!String.IsNullOrEmpty(answer.Answer))
            {
                question.Answer = answer.Answer;
            }
        }

        private void SubmitResponse()
        {
            SurveyResponse response = new SurveyResponse(GetCurrentResponseGuid());
            response.SubmissionDate = DateTime.UtcNow;
            response.Complete = true;

            SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

            if (siteUser != null)
            {
                response.UserGuid = siteUser.UserGuid;
            }

            response.Save();

            SetCookie(Guid.Empty);
        }

        #region Events

        void btnSurveyBack_Click(object sender, EventArgs e)
        {
            //Save answers
            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/CompleteSurvey.aspx?SurveyGuid=" + surveyGuid
                + "&PageId=" + pageId
                + "&mid=" + moduleId
                + "&SurveyPageGuid=" + previousSurveyPageGuid);
        }

        void btnSurveyForward_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                SaveAnswers();

                //Check if this is a move forward or a submit
                Button button = (Button)sender;

                if (button.CommandName == "Submit")
                {
                    SubmitResponse();
                    //We need to go back to the module main page
                    WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx?pageId=" + pageId
                        + "&SurveyEnd=" + surveyGuid); //TODO: Fix to redirect by pageId
                }
                else
                {
                    WebUtils.SetupRedirect(this, SiteRoot + "/Survey/CompleteSurvey.aspx?SurveyGuid="
                        + surveyGuid + "&PageId="
                        + pageId + "&mid=" + moduleId + "&SurveyPageGuid=" + nextSurveyPageGuid);
                }
            }
        }

        #endregion

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, SurveyResources.SurveyPageTitle);

            heading.Text = string.Format(CultureInfo.InvariantCulture,
                SurveyResources.CompleteSurveyHeaderFormatString,
                survey.SurveyName, page.PageTitle);

            btnSurveyBack.Text = SurveyResources.SurveyButtonBack;
            btnSurveyBack.ToolTip = SurveyResources.SurveyButtonBackToolTip;
            btnSurveyForward.Text = SurveyResources.SurveyButtonForward;
            btnSurveyForward.ToolTip = SurveyResources.SurveyButtonForwardToolTip;

        }

        private void LoadSettings()
        {
            surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
            pageId = WebUtils.ParseInt32FromQueryString("PageId", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);
            surveyPageGuid = WebUtils.ParseGuidFromQueryString("SurveyPageGuid", Guid.Empty);

            survey = new Survey(surveyGuid);
            page = new SurveyFeature.Business.Page(surveyPageGuid);
            questions = Question.GetAll(surveyPageGuid);

            AddClassToBody("surveypage");
            
        }

        

        private Guid ResponseGuid
        {
            get
            {
                Guid responseGuid = Guid.Empty;

                if (Request.Cookies["mojoSurvey"] != null)
                {
                    string stringGuid = Request.Cookies["mojoSurvey"].Values[survey.SurveyGuid.ToString()];

                    if (stringGuid != null)
                    {
                        try
                        {
                            responseGuid = new Guid(stringGuid);
                        }
                        catch (OverflowException)
                        {
                            responseGuid = Guid.Empty;
                        }
                        catch (FormatException)
                        {
                            responseGuid = Guid.Empty;
                        }
                    }
                }

                return responseGuid;
            }
        }

        private Guid GetCurrentResponseGuid()
        {
            Guid responseGuid = Guid.Empty;

            if (Request.Cookies["mojoSurvey"] != null)
            {
                string stringGuid = Request.Cookies["mojoSurvey"].Values[survey.SurveyGuid.ToString()];

                if (stringGuid != null)
                {
                    try
                    {
                        responseGuid = new Guid(stringGuid);
                    }
                    catch (OverflowException)
                    {
                        //problem parsing guid from cookie - cookie may be corrupt so delete
                        Response.Cookies.Remove("mojoSurvey");
                    }
                    catch (FormatException)
                    {
                        //problem parsing guid from cookie - cookie may be corrupt so delete
                        Response.Cookies.Remove("mojoSurvey");
                    }
                }
            }

            return responseGuid;
        }

        

        

        private void SetCookie(Guid responseGuid)
        {
            if (Request.Cookies["mojoSurvey"] == null)
            {
                HttpCookie cookie = new HttpCookie("mojoSurvey");
                Response.Cookies.Add(cookie);
            }

            if (responseGuid == Guid.Empty)
            {
                Response.Cookies["mojoSurvey"].Values.Remove(surveyGuid.ToString());
            }
            else
            {
                Response.Cookies["mojoSurvey"].Values.Set(surveyGuid.ToString(), responseGuid.ToString());
            }
        }

        private bool UserHasPermissionToComplete()
        {
            if (HttpContext.Current == null || HttpContext.Current.User == null) return false;

            if (WebUser.IsAdmin || WebUser.IsContentAdmin) return true;

            Module module = new Module(moduleId, pageId);
            PageSettings pageSettings = new PageSettings(siteSettings.SiteId, module.PageId);

            if (pageSettings == null) return false;
            if (pageSettings.PageId < 0) return false;
            
            //check the user has permission to view this page and that the survey is the current selected survey of
            //this module
            if (Survey.GetModulesCurrentSurvey(moduleId) == surveyGuid && WebUser.IsInRoles(pageSettings.AuthorizedRoles))
            {
                return true;
            }

            return false;
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
            this.btnSurveyForward.Click += new EventHandler(btnSurveyForward_Click);
            this.btnSurveyBack.Click += new EventHandler(btnSurveyBack_Click);
            SuppressPageMenu();
        }

        #endregion

        
    }
}
