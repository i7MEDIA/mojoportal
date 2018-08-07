// Created:       2007-02-10
// Last Modified: 2018-07-31
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Web;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using SurveyFeature.Business;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public partial class SurveyEditPage : NonCmsBasePage
	{
		#region private properties

		private Guid surveyGuid = Guid.Empty;

		#endregion


		#region public properties

		public int PageId { get; private set; }
		public int ModuleId { get; private set; }

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

			if (!UserCanEditModule(ModuleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
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

				if (survey.SubmissionLimit <= 0)
				{
					cbLimitSubmissions.Checked = false;
				}
				else
				{
					cbLimitSubmissions.Checked = true;
				}

				txtSubmissionLimit.Attributes.Add("data-initial-value", survey.SubmissionLimit.ToString());

				txtSubmissionLimit.Text = survey.SubmissionLimit.ToString();
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
			lnkSurveys.NavigateUrl = $"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

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
			PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", -1);

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

			Load += new EventHandler(Page_Load);
			btnSave.Click += new EventHandler(BtnSave_Click);
			btnCancel.Click += new EventHandler(BtnCancel_Click);

			SuppressPageMenu();
			SiteUtils.SetupEditor(edWelcomeMessage, AllowSkinOverride, this);
			SiteUtils.SetupEditor(edThankyouMessage, AllowSkinOverride, this);
		}

		#endregion


		#region Events

		private void BtnSave_Click(object sender, EventArgs e)
		{
			Page.Validate("survey");

			if (Page.IsValid)
			{
				Survey survey = new Survey(surveyGuid)
				{
					SurveyName = txtSurveyName.Text
				};

				if (cbLimitSubmissions.Checked)
				{
					survey.SubmissionLimit = Convert.ToInt32(txtSubmissionLimit.Text);
				}
				else
				{
					survey.SubmissionLimit = 0;
				}

				survey.SiteGuid = siteSettings.SiteGuid;
				survey.StartPageText = edWelcomeMessage.Text;
				survey.EndPageText = edThankyouMessage.Text;

				survey.Save();

				WebUtils.SetupRedirect(
					this,
					$"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
				);
			}
		}


		private void BtnCancel_Click(object sender, EventArgs e)
		{
			WebUtils.SetupRedirect(
				this,
				$"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
			);
		}

		#endregion
	}
}