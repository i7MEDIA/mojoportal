// Author:        Rob Henry
// Created:       2007-08-24

using System;
using System.Linq;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using Resources;
using SurveyFeature.Business;

namespace SurveyFeature.UI;

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

	public string customCssClassSetting { get; private set; }

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
		customCssClassSetting = Settings.ParseString("CustomCssClassSetting");
	}


	private void PopulateLabels()
	{
		TitleControl.EditUrl = "Survey/Surveys.aspx".ToLinkBuilder().ToString();
		TitleControl.EditText = SurveyResources.SurveyModuleEditText;
		pnlOuterWrap.SetOrAppendCss(customCssClassSetting);

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