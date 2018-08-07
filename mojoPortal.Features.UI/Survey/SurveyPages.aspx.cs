// Author:        Rob Henry
// Created:       2007-09-25
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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public partial class SurveyPagesPage : mojoBasePage
	{
		private Survey survey = null;
		private mojoPortal.Business.Module currentModule = null;

		public Guid SurveyGuid { get; private set; } = Guid.Empty;

		#region protected properties

		protected string DeleteLinkImage = $"~/Data/SiteImages/{WebConfigSettings.DeleteLinkImage}";
		protected int PageId { get; private set; }
		protected int ModuleId { get; private set; }
		protected string EditContentImage { get; } = WebConfigSettings.EditContentImage;

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
			}

			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			if (survey == null) return;

			lnkPages.Text = string.Format(
				CultureInfo.InvariantCulture,
				SurveyResources.SurveyPagesLabelFormatString,
				survey.SurveyName
			);

			Title = SiteUtils.FormatPageTitle(siteSettings, lnkPages.Text);
			heading.Text = lnkPages.Text;

			if (currentModule != null)
			{
				lnkSurveys.Text = string.Format(
					CultureInfo.InvariantCulture,
					SurveyResources.ChooseActiveSurveyFormatString,
					currentModule.ModuleTitle
				);
			}

			if (Page.IsPostBack) return;

			BindGrid();
		}


		private void PopulateLabels()
		{
			lnkPageCrumb.Text = CurrentPage.PageName;
			lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			lnkSurveys.Text = SurveyResources.ChooseActiveSurveyLink;
			lnkSurveys.NavigateUrl = $"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			lnkPages.Text = SurveyResources.SurveyPagesBreadCrumbText;
			lnkPages.NavigateUrl = Request.RawUrl;

			lnkAddNew.Text = SurveyResources.PageAddButton;

			lnkAddNew.NavigateUrl = $"{SiteRoot}/Survey/SurveyPageEdit.aspx?SurveyGuid={SurveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			grdSurveyPages.Columns[0].HeaderText = SurveyResources.PagesGridEditDeleteHeader;
			grdSurveyPages.Columns[1].HeaderText = SurveyResources.PagesGridPageTitleHeader;
			grdSurveyPages.Columns[2].HeaderText = SurveyResources.PagesGridPageEnabledHeader;
			grdSurveyPages.Columns[3].HeaderText = SurveyResources.PagesGridQuestionCountHeader;
			grdSurveyPages.Columns[4].HeaderText = SurveyResources.PagesGridMoveUpOrDownHeader;
		}


		private void LoadSettings()
		{
			SurveyGuid = WebUtils.ParseGuidFromQueryString("SurveyGuid", Guid.Empty);
			PageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

			currentModule = GetModule(ModuleId, Survey.FeatureGuid);

			if (SurveyGuid != Guid.Empty)
			{
				survey = new Survey(SurveyGuid);

				if (survey.SiteGuid != siteSettings.SiteGuid)
				{
					SurveyGuid = Guid.Empty;
					survey = null;
				}
			}

			AddClassToBody("surveypages");
		}


		private void BindGrid()
		{
			List<Business.Page> pages = Business.Page.GetAll(SurveyGuid);
			grdSurveyPages.DataSource = pages;
			grdSurveyPages.DataBind();

			//hide the grid and display a message if no questions have been added.

			if (pages.Count == 0)
			{
				grdSurveyPages.Visible = false;
				lblMessages.Text = SurveyResources.SurveyHasNoPagesWarning;
			}
			else
			{
				grdSurveyPages.Visible = true;
			}
		}


		#region Events

		void GrdSurveyPages_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			if (e.Row.RowType != DataControlRowType.Header && e.Row.RowType != DataControlRowType.Footer)
			{
				GridViewRow row = e.Row;
				ImageButton deleteButton = (ImageButton)row.FindControl("btnDelete");
				UIHelper.AddConfirmationDialog(deleteButton, Resources.SurveyResources.PageDeleteDeletesAllQuestionsWarning);
			}
		}


		void GrdSurveyPages_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{}


		void GrdSurveyPages_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			List<Business.Page> pages = SurveyFeature.Business.Page.GetAll(SurveyGuid);
			Guid currentPageGuid = new Guid(e.CommandArgument.ToString());

			Business.Page currentPage = null;

			Business.Page swapPage;

			int currentItemIndex = -1;
			int i = 0;

			foreach (Business.Page p in pages)
			{
				if (p.SurveyPageGuid == currentPageGuid)
				{
					currentPage = p;
					currentItemIndex = i;
				}

				i += 1;
			}

			if (currentPage == null) return;

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
					Business.Page.Delete(currentPageGuid);

					break;
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);
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
			grdSurveyPages.RowCommand += new GridViewCommandEventHandler(GrdSurveyPages_RowCommand);
			grdSurveyPages.RowDeleting += new GridViewDeleteEventHandler(GrdSurveyPages_RowDeleting);
			grdSurveyPages.RowDataBound += new GridViewRowEventHandler(GrdSurveyPages_RowDataBound);

			SuppressPageMenu();
		}

		#endregion
	}
}
