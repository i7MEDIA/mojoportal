// Author:        Rob Henry
// Created:       2007-03-10
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
using mojoPortal.Web.Framework;
using Resources;
using SurveyFeature.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public partial class SurveysPage : NonCmsBasePage
	{
		private Guid surveyGuid = Guid.Empty;
		private mojoPortal.Business.Module currentModule = null;
		protected string DeleteLinkImage = $"~/Data/SiteImages/{WebConfigSettings.DeleteLinkImage}";

		#region public properties

		public int PageId { get; private set; }
		public int ModuleId { get; private set; }
		public string EditContentImage { get; } = WebConfigSettings.EditContentImage;

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

			if (!UserCanEditModule(ModuleId, Survey.FeatureGuid))
			{
				SiteUtils.RedirectToAccessDeniedPage();

				return;
			}

			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			if (currentModule == null) return;

			lnkPageCrumb.Text = currentModule.ModuleTitle;

			lnkSurveys.Text = string.Format(
				CultureInfo.InvariantCulture,
				SurveyResources.ChooseActiveSurveyFormatString,
				currentModule.ModuleTitle
			);

			heading.Text = lnkSurveys.Text;

			if (Page.IsPostBack) return;

			BindGrid();
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, CurrentPage.PageName);

			lnkPageCrumb.Text = CurrentPage.PageName;
			lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			lnkSurveys.NavigateUrl = Request.RawUrl;
			lnkSurveys.Text = SurveyResources.ChooseActiveSurveyLink;

			heading.Text = SurveyResources.SurveysLabel;
			lnkAddNew.Text = SurveyResources.SurveyAddButton;

			lnkAddNew.NavigateUrl = $"{SiteRoot}/Survey/SurveyEdit.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			grdSurveys.Columns[0].HeaderText = SurveyResources.SurveysGridEditDeleteHeader;
			grdSurveys.Columns[1].HeaderText = SurveyResources.SurveysGridSurveyNameHeader;
			grdSurveys.Columns[2].HeaderText = SurveyResources.SurveysGridCreationDateHeader;
			grdSurveys.Columns[3].HeaderText = SurveyResources.SurveysGridPageCountHeader;
			grdSurveys.Columns[4].HeaderText = SurveyResources.SurveysGridSelectedSurveyHeader;
			grdSurveys.Columns[5].HeaderText = SurveyResources.SurveysGridResponsesHeader;
		}


		private void LoadSettings()
		{
			PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

			currentModule = GetModule(ModuleId, Survey.FeatureGuid);
			surveyGuid = Survey.GetModulesCurrentSurvey(ModuleId);

			AddClassToBody("surveys");
		}


		private void BindGrid()
		{
			List<Survey> surveys = Survey.GetAll(siteSettings.SiteGuid);

			grdSurveys.DataSource = surveys;
			grdSurveys.DataBind();

			//hide the grid and display a message if no surveys have been added.

			if (surveys.Count == 0)
			{
				grdSurveys.Visible = false;
				lblMessages.Text = SurveyResources.SurveyNoSurveysWarning;
			}
			else
			{
				grdSurveys.Visible = true;
			}
		}


		#region Events

		void GrdSurveys_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			Survey survey;

			switch (e.CommandName)
			{
				case "modAdd":
					survey = new Survey(new Guid(e.CommandArgument.ToString()));
					survey.AddToModule(ModuleId);
					surveyGuid = new Guid(e.CommandArgument.ToString());

					break;

				case "modRemove":
					survey = new Survey(new Guid(e.CommandArgument.ToString()));
					survey.RemoveFromModule(ModuleId);
					surveyGuid = Guid.Empty;

					break;

				case "delete":
					Survey.Delete(new Guid(e.CommandArgument.ToString()));

					break;

				case "export":
					DataTable dataTable = Survey.GetResultsTable(surveyGuid);
					string fileName = $"csv{DateTimeHelper.GetDateTimeStringForFileName()}.csv";

					ExportHelper.ExportDataTableToCsv(HttpContext.Current, dataTable, fileName);

					break;
			}

			BindGrid();
		}


		void GrdSurveys_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{

		}


		void GrdSurveys_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
			{
				GridViewRow row = e.Row;
				ImageButton deleteButton = (ImageButton)row.FindControl("btnDelete");
				UIHelper.AddConfirmationDialog(deleteButton, SurveyResources.SurveyDeleteDeletesAllPagesQuestionsWarning);

				Button linkAddRemove = (Button)row.FindControl("lnkAddRemoveFromModule");

				if (linkAddRemove.CommandArgument == surveyGuid.ToString())
				{
					linkAddRemove.Text = SurveyResources.SurveysGridSelectedSurveyLink;
					linkAddRemove.CommandName = "modRemove";
				}
				else
				{
					linkAddRemove.Text = SurveyResources.SurveysGridNotSelectedSurveyLink;
					linkAddRemove.CommandName = "modAdd";
				}
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
			Load += new EventHandler(Page_Load);

			grdSurveys.RowDeleting += new GridViewDeleteEventHandler(GrdSurveys_RowDeleting);
			grdSurveys.RowCommand += new GridViewCommandEventHandler(GrdSurveys_RowCommand);
			grdSurveys.RowDataBound += new GridViewRowEventHandler(GrdSurveys_RowDataBound);

			SuppressPageMenu();
		}

		#endregion
	}
}