/// Author:					Rob Henry
/// Created:				2007-08-24
/// Last Modified:			2009-01-19
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using SurveyFeature.Business;

namespace SurveyFeature.UI
{
    public partial class SurveyModule : SiteModuleControl
    {
        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.btnStartSurvey.Click += new EventHandler(btnStartSurvey_Click);
        }
        #endregion

        private Guid surveyGuid = Guid.Empty;
        private Guid surveyPageGuid;
        private Guid surveyResponse = Guid.Empty;
        Survey survey;
        SurveyManager manager;

        #region public properties

        public Guid SurveyGuid
        {
            get
            {
                return surveyGuid;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParameters();
            manager = new SurveyManager(surveyGuid);
            surveyPageGuid = manager.GetSurveyFirstSurveyPageGuid(surveyGuid);
            survey = new Survey(surveyGuid);      
            PopulateLabels();
        }

        private void PopulateLabels()
        {
            TitleControl.EditUrl = SiteRoot + "/Survey/Surveys.aspx";
            TitleControl.EditText = Resources.SurveyResources.SurveyModuleEditText;
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (surveyGuid == Guid.Empty)
            {
                litSurveyMessage.Text = Resources.SurveyResources.SurveyNoneSelected;
                btnStartSurvey.Visible = false;
            }
            else if(surveyResponse != Guid.Empty)
            {
                litSurveyMessage.Text = survey.EndPageText;
                btnStartSurvey.Visible = false;
                litOldResponses.Visible = false;
            }
            else if(SurveyHasNoPages())
            {
                litSurveyMessage.Text = Resources.SurveyResources.SurveyHasNoPagesWarning;
                btnStartSurvey.Visible = false;
            }
            else
            {
                litSurveyMessage.Text = survey.StartPageText;
                btnStartSurvey.Text = Resources.SurveyResources.SurveyStartText;

                if (PartialSurveyExists())
                {
                    litOldResponses.Text = Resources.SurveyResources.SurveyPartialAnswersExist;
                    chkUseOldResponses.Visible = true;
                }
            }
        }

        private void LoadParameters()
        {
            surveyGuid = Survey.GetModulesCurrentSurvey(ModuleId);
            surveyResponse = WebUtils.ParseGuidFromQueryString("SurveyEnd", Guid.Empty);
        }

        private bool PartialSurveyExists()
        {
            return (Request.Cookies["mojoSurvey"] != null && Request.Cookies["mojoSurvey"].Values[surveyGuid.ToString()] != null);
        }

        private bool SurveyHasNoPages()
        {
            return surveyPageGuid == Guid.Empty;
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

        #region Events
        
        void btnStartSurvey_Click(object sender, EventArgs e)
        {
            Guid currentResponseGuid = GetCurrentResponseGuid();

            if (!chkUseOldResponses.Checked)
            {
                if (currentResponseGuid != Guid.Empty)
                {
                    SurveyResponse.Delete(currentResponseGuid);
                }

                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

                SurveyResponse response = new SurveyResponse();
                response.Complete = false;
                response.SurveyGuid = survey.SurveyGuid;

                if (siteUser != null)
                    response.UserGuid = siteUser.UserGuid;
                response.Save();

                currentResponseGuid = response.ResponseGuid;
            }

            SetCookie(currentResponseGuid);

            WebUtils.SetupRedirect(this, SiteRoot + "/Survey/CompleteSurvey.aspx?SurveyGuid=" + surveyGuid.ToString() + "&pageid="
                + PageId.ToInvariantString() + "&mid=" + ModuleId.ToInvariantString() + "&SurveyPageGuid=" + surveyPageGuid.ToString());
        }
        
        #endregion

    }
}