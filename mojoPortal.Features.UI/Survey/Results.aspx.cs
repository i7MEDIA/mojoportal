// Author:        Rob Henry
// Created:       2007-10-18
// Last Modified: 2018-07-31
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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;

namespace SurveyFeature.UI
{
	public partial class ResultsPage : NonCmsBasePage
	{
		private Guid surveyGuid = Guid.Empty;
		private Guid responseGuid = Guid.Empty;
		private Survey survey = null;
		private SurveyResponse currentResponse;
		private SurveyResponse previousResponse;
		private SurveyResponse nextResponse;
		protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;


		#region protected properties

		protected int PageId { get; private set; } = -1;
		protected int ModuleId { get; private set; } = -1;

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

			if (!UserCanEditModule(ModuleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
			}

			PopulateLabels();
			PopulateControls();
		}

		private void PopulateControls()
		{
			if (survey == null) return;

			lnkResults.Text = string.Format(
				CultureInfo.InvariantCulture,
				SurveyResources.SurveyResultsFormatString,
				survey.SurveyName
			);

			heading.Text = lnkResults.Text;

			if (previousResponse != null)
			{
				lnkPreviousResponse.NavigateUrl = $"{SiteRoot}/Survey/Results.aspx?SurveyGuid={surveyGuid}&ResponseGuid={previousResponse.ResponseGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";
			}
			else
			{
				liPrevResponse.Attributes["class"] += " disabled";
			}

			if (nextResponse != null)
			{
				lnkNextResponse.NavigateUrl = $"{SiteRoot}/Survey/Results.aspx?SurveyGuid={surveyGuid}&ResponseGuid={nextResponse.ResponseGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";
			}
			else
			{
				liNextResponse.Attributes["class"] += " disabled";
			}

			if (currentResponse == null) return;

			lblCompletionDate.Text = currentResponse.SubmissionDate.ToString();

			if (currentResponse.UserGuid != Guid.Empty)
			{
				SiteUser respondent = new SiteUser(siteSettings, currentResponse.UserGuid);

				if (respondent != null)
				{
					lblRespondent.Text = respondent.LoginName;
				}
			}

			BindGrid();
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);

			lblRespondent.Text = Resources.SurveyResources.SurveyRespondentAnonymousText;

			lnkPageCrumb.Text = CurrentPage.PageName;
			lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			lnkSurveys.Text = SurveyResources.SurveyBreadCrumbText;

			lnkSurveys.NavigateUrl = $"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			lnkResults.Text = SurveyResources.SurveyResultsBreadCrumbText;
			lnkResults.NavigateUrl = $"{SiteRoot}/Survey/Results.aspx?SurveyGuid={surveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			lnkNextResponse.Text = SurveyResources.ResponseGetNext;
			lnkNextResponse.ToolTip = SurveyResources.ResponseGetNextToolTip;
			lnkPreviousResponse.Text = SurveyResources.ResponseGetPrevious;
			lnkPreviousResponse.ToolTip = SurveyResources.ResponseGetPreviousToolTip;

			grdResults.Columns[0].HeaderText = SurveyResources.ResultsGridQuestionHeader;
			grdResults.Columns[1].HeaderText = SurveyResources.ResultsGridAnswerHeader;
			grdResults.Columns[2].HeaderText = SurveyResources.ResultsGridPageTitleHeader;

			btnDelete.ImageUrl = DeleteLinkImage;
		}


		private void LoadSettings()
		{
			surveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
			PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			responseGuid = WebUtils.ParseGuidFromQueryString("ResponseGuid", Guid.Empty);

			if (surveyGuid == Guid.Empty) return;

			if (responseGuid == Guid.Empty)
			{
				SurveyResponse response = SurveyResponse.GetFirst(surveyGuid);

				if (response == null)
				{
					WebUtils.SetupRedirect(
						this,
						$"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
					);
				}
				else
				{
					WebUtils.SetupRedirect(
						this,
						$"{SiteRoot}/Survey/Results.aspx?SurveyGuid={surveyGuid.ToString()}&ResponseGuid={response.ResponseGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
					);
				}
			}
			else
			{
				currentResponse = new SurveyResponse(responseGuid);
				previousResponse = SurveyResponse.GetPrevious(responseGuid);
				nextResponse = SurveyResponse.GetNext(responseGuid);
			}

			if (surveyGuid != Guid.Empty)
			{
				survey = new Survey(surveyGuid);

				if (survey.SiteGuid != siteSettings.SiteGuid)
				{
					surveyGuid = Guid.Empty;
					survey = null;

					responseGuid = Guid.Empty;
					currentResponse = null;
					previousResponse = null;
					nextResponse = null;
				}
			}

			AddClassToBody("surveyresults");
		}


		private void BindGrid()
		{
			if (currentResponse == null) return;

			List<Result> results;

			results = Survey.GetResults(currentResponse.ResponseGuid);

			grdResults.DataSource = results;
			grdResults.DataBind();
		}


		#region Events

		void BtnDelete_Click(object sender, ImageClickEventArgs e)
		{
			SurveyResponse.Delete(responseGuid);

			WebUtils.SetupRedirect(
				this,
				$"{SiteRoot}/Survey/Results.aspx?SurveyGuid={surveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
			);
		}

		#endregion


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
			btnDelete.Click += new ImageClickEventHandler(BtnDelete_Click);

			SuppressPageMenu();
		}

		#endregion
	}
}