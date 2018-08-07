// Author:        Rob Henry
// Created:       2007-08-24
// Last Modified: 2018-07-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using SurveyFeature.Business;
using System;
using System.Linq;
using System.Web;

namespace SurveyFeature.UI
{
	public partial class SurveyModule : SiteModuleControl
	{
		#region OnInit

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
			btnStartSurvey.Click += new EventHandler(BtnStartSurvey_Click);
		}

		#endregion


		private SiteUser siteUser = null;
		private Guid surveyPageGuid;
		private Guid surveyResponse = Guid.Empty;
		private Survey survey;
		private SurveyManager manager;


		#region public properties

		public Guid SurveyGuid { get; private set; } = Guid.Empty;

		#endregion


		protected void Page_Load(object sender, EventArgs e)
		{
			LoadParameters();
			LoadSettings();
			manager = new SurveyManager(SurveyGuid);
			surveyPageGuid = manager.GetSurveyFirstSurveyPageGuid(SurveyGuid);
			survey = new Survey(SurveyGuid);
			PopulateLabels();
		}

		private void LoadParameters()
		{
			SurveyGuid = Survey.GetModulesCurrentSurvey(ModuleId);
			surveyResponse = WebUtils.ParseGuidFromQueryString("SurveyEnd", Guid.Empty);
		}


		private void LoadSettings()
		{
			siteUser = SiteUtils.GetCurrentSiteUser();
		}


		private void PopulateLabels()
		{
			TitleControl.EditUrl = SiteRoot + "/Survey/Surveys.aspx";
			TitleControl.EditText = SurveyResources.SurveyModuleEditText;
			TitleControl.Visible = !RenderInWebPartMode;

			if (ModuleConfiguration != null)
			{
				Title = ModuleConfiguration.ModuleTitle;
				Description = ModuleConfiguration.FeatureName;
			}

			if (SurveyGuid == Guid.Empty)
			{
				litSurveyMessage.Text = SurveyResources.SurveyNoneSelected;
				btnStartSurvey.Visible = false;
			}
			else if (surveyResponse != Guid.Empty)
			{
				litSurveyMessage.Text = survey.EndPageText;
				btnStartSurvey.Visible = false;
			}
			else if (SurveyHasNoPages())
			{
				litSurveyMessage.Text = SurveyResources.SurveyHasNoPagesWarning;
				btnStartSurvey.Visible = false;
			}
			else if (SubmissionLimit())
			{
				litSurveyMessage.Text = SurveyResources.SurveyLimitReached;
				btnStartSurvey.Visible = false;
			}
			else
			{
				litSurveyMessage.Text = survey.StartPageText;
				btnStartSurvey.Text = SurveyResources.SurveyStartText;

				if (PartialSurveyExists())
				{
					litOldResponses.Text = SurveyResources.SurveyPartialAnswersExist;
					fgpOldResponses.Visible = true;
				}
			}
		}


		private bool SubmissionLimit()
		{
			if (survey.SubmissionLimit <= 0) return false;
			if (GetUserSubmissionCount() >= survey.SubmissionLimit) return true;

			return false;
		}


		private int GetUserSubmissionCount()
		{
			var responses = SurveyResponse.GetAll(SurveyGuid);
			var userGuid = Guid.Empty;

			if (siteUser != null) userGuid = siteUser.UserGuid;

			if (userGuid != Guid.Empty)
			{
				return responses.Where(response => response.UserGuid == userGuid).Count();
			}

			return 0;
		}


		private bool PartialSurveyExists()
		{
			return (Request.Cookies["mojoSurvey"] != null && Request.Cookies["mojoSurvey"].Values[SurveyGuid.ToString()] != null);
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
				Response.Cookies["mojoSurvey"].Values.Remove(SurveyGuid.ToString());
			}
			else
			{
				Response.Cookies["mojoSurvey"].Values.Set(SurveyGuid.ToString(), responseGuid.ToString());
			}
		}


		#region Events

		private void BtnStartSurvey_Click(object sender, EventArgs e)
		{
			Guid currentResponseGuid = GetCurrentResponseGuid();

			if (!chkUseOldResponses.Checked)
			{
				if (currentResponseGuid != Guid.Empty)
				{
					SurveyResponse.Delete(currentResponseGuid);
				}

				SurveyResponse response = new SurveyResponse();

				response.Complete = false;
				response.SurveyGuid = survey.SurveyGuid;

				if (siteUser != null)
				{
					response.UserGuid = siteUser.UserGuid;
				}

				response.Save();

				currentResponseGuid = response.ResponseGuid;
			}

			SetCookie(currentResponseGuid);

			WebUtils.SetupRedirect(
				this,
				$"{SiteRoot}/Survey/CompleteSurvey.aspx?SurveyGuid={SurveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}&SurveyPageGuid={surveyPageGuid.ToString()}");
		}

		#endregion
	}
}