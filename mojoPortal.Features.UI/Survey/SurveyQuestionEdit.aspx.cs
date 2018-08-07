// Author:        Rob Henry
// Created:       2007-09-09
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
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SurveyFeature.UI
{
	public partial class QuestionEditPage : NonCmsBasePage
	{
		private Guid surveyGuid;
		private Guid surveyPageGuid;
		private Guid questionGuid;
		private QuestionType questionType;
		private Survey survey = null;
		private Business.Page surveyPage = null;
		private Question question = null;
		private mojoPortal.Business.Module currentModule = null;
		protected string EditContentImage = WebConfigSettings.EditContentImage;
		protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;

		const string Deleted_Options_Key = "DeletedOptions";


		#region protected properties

		protected int PageId { get; private set; }
		protected int ModuleId { get; private set; }

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
			if (survey != null)
			{
				lnkPages.Text = string.Format(
					CultureInfo.InvariantCulture,
					SurveyResources.SurveyPagesLabelFormatString,
					survey.SurveyName
				);

				heading.Text = string.Format(
					CultureInfo.InvariantCulture,
					SurveyResources.QuestionEditFormatString,
					survey.SurveyName
				);
			}

			if (currentModule != null)
			{
				lnkSurveys.Text = string.Format(
					CultureInfo.InvariantCulture,
					SurveyResources.ChooseActiveSurveyFormatString,
					currentModule.ModuleTitle
				);
			}

			if (surveyPage != null)
			{
				lnkQuestions.Text = string.Format(
					CultureInfo.InvariantCulture,
					SurveyResources.QuestionsPageFormatString,
					surveyPage.PageTitle
				);
			}

			if (Page.IsPostBack) return;

			if (questionGuid == Guid.Empty)
			{
				lblQuestionType.Text = questionType.ToString();

				if (questionType == QuestionType.TextBox || questionType == QuestionType.Date)
				{
					fgpAddOptionRow.Visible = false;
					fgpItemsRow.Visible = false;
				}

				return;
			}

			txtQuestionName.Text = question.QuestionName;
			lblQuestionType.Text = ((QuestionType)question.QuestionTypeId).ToString();
			edMessage.Text = question.QuestionText;
			chkAnswerRequired.Checked = question.AnswerIsRequired;
			txtValidationMessage.Text = question.ValidationMessage;

			if (
				(QuestionType)question.QuestionTypeId == QuestionType.TextBox ||
				(QuestionType)question.QuestionTypeId == QuestionType.Date
			)
			{
				fgpAddOptionRow.Visible = false;
				fgpItemsRow.Visible = false;
			}

			foreach (QuestionOption option in QuestionOption.GetAll(questionGuid))
			{
				lbOptions.Items.Add(new ListItem(option.Answer, option.QuestionOptionGuid.ToString()));
			}
		}


		private void PopulateLabels()
		{
			lnkPageCrumb.Text = CurrentPage.PageName;
			lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

			lnkSurveys.Text = SurveyResources.ChooseActiveSurveyLink;
			lnkSurveys.NavigateUrl = $"{SiteRoot}/Survey/Surveys.aspx?pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			lnkPages.Text = SurveyResources.SurveyPagesBreadCrumbText;
			lnkPages.NavigateUrl = $"{SiteRoot}/Survey/SurveyPages.aspx?SurveyGuid={surveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			lnkQuestions.NavigateUrl = $"{SiteRoot}/Survey/SurveyQuestions.aspx?SurveyGuid={surveyGuid.ToString()}&SurveyPageGuid={surveyPageGuid.ToString()}&PageId={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}";

			if (questionGuid != Guid.Empty)
			{
				heading.Text = SurveyResources.QuestionEditHeader;
			}
			else
			{
				heading.Text = SurveyResources.QuestionAddHeader;
			}

			btnSave.Text = SurveyResources.QuestionEditOptionsSaveButton;
			btnSave.ToolTip = SurveyResources.QuestionEditOptionsSaveButtonToolTip;

			btnAddOption.Text = SurveyResources.QuestionEditOptionsAddButton;
			btnAddOption.ToolTip = SurveyResources.QuestionEditOptionsAddToolTip;

			btnUp.AlternateText = SurveyResources.QuestionEditOptionsUpAlternateText;
			btnUp.ToolTip = SurveyResources.QuestionEditOptionsUpAlternateText;
			btnUp.ImageUrl = $"{ImageSiteRoot}/Data/SiteImages/up.png";

			btnDown.AlternateText = SurveyResources.QuestionEditOptionsDownAlternateText;
			btnDown.ToolTip = SurveyResources.QuestionEditOptionsDownAlternateText;
			btnDown.ImageUrl = $"{ImageSiteRoot}/Data/SiteImages/down.png";

			btnEdit.AlternateText = SurveyResources.QuestionEditOptionsEditAlternateText;
			btnEdit.ToolTip = SurveyResources.QuestionEditOptionsEditAlternateText;
			btnEdit.ImageUrl = $"{ImageSiteRoot}/Data/SiteImages/{EditContentImage}";

			btnDeleteOption.AlternateText = SurveyResources.QuestionEditOptionsDeleteAlternateText;
			btnDeleteOption.ToolTip = SurveyResources.QuestionEditOptionsDeleteAlternateText;
			btnDeleteOption.ImageUrl = $"{ImageSiteRoot}/Data/SiteImages/{DeleteLinkImage}";

			btnCancel.Text = SurveyResources.QuestionEditCancelButton;
			btnCancel.ToolTip = SurveyResources.QuestionEditCancelButtonToolTip;

			edMessage.WebEditor.ToolBar = ToolBar.Full;
		}


		private void LoadSettings()
		{
			PageId = WebUtils.ParseInt32FromQueryString("PageId", PageId);
			questionGuid = WebUtils.ParseGuidFromQueryString("QuestionGuid", Guid.Empty);
			ModuleId = WebUtils.ParseInt32FromQueryString("mid", ModuleId);
			currentModule = GetModule(ModuleId, Survey.FeatureGuid);
			question = new Question(questionGuid);

			if (questionGuid != Guid.Empty)
			{
				surveyPageGuid = question.SurveyPageGuid;
			}
			else
			{
				//we have no question guid so must be a new question
				string questionTypeParam = Request.QueryString["NewQuestionType"];
				questionType = EnumHelper<QuestionType>.Parse(questionTypeParam);
				surveyPageGuid = WebUtils.ParseGuidFromQueryString("SurveyPageGuid", Guid.Empty);
				question.SurveyPageGuid = surveyPageGuid;
			}

			if (surveyPageGuid != Guid.Empty)
			{
				surveyPage = new Business.Page(surveyPageGuid);
				survey = new Survey(surveyPage.SurveyGuid);
				surveyGuid = survey.SurveyGuid;

				if (survey.SiteGuid != siteSettings.SiteGuid)
				{
					surveyGuid = Guid.Empty;
					survey = null;

					surveyPageGuid = Guid.Empty;
					surveyPage = null;

					questionGuid = Guid.Empty;
					question = null;
				}
			}

			AddClassToBody("surveyquestionedit");
		}


		private void SaveQuestion(Question question)
		{
			question.QuestionName = txtQuestionName.Text;
			question.QuestionText = edMessage.Text;
			question.QuestionTypeId = (int)EnumHelper<QuestionType>.Parse(lblQuestionType.Text);
			question.AnswerIsRequired = chkAnswerRequired.Checked;
			question.ValidationMessage = txtValidationMessage.Text;

			if (questionGuid == Guid.Empty)
			{
				question.SurveyPageGuid = surveyPageGuid;
			}

			question.Save();
		}


		private void DeleteOldQuestionOptions()
		{
			//delete the options that have been saved in viewstate for full deletion on save
			QuestionOption option;
			List<Guid> deletedOptions;

			deletedOptions = (List<Guid>)ViewState[Deleted_Options_Key] ?? new List<Guid>();

			foreach (Guid deletedOptionGuid in deletedOptions)
			{
				option = new QuestionOption(deletedOptionGuid);
				option.Delete();
			}
		}


		private void SaveQuestionOptions(Question question)
		{
			QuestionOption option;
			int order = 0;

			// Save options

			foreach (ListItem item in lbOptions.Items)
			{
				Guid optionGuid = Guid.Empty;

				if (item.Value.Length == 36)
				{
					try
					{
						optionGuid = new Guid(item.Value);
					}
					catch (FormatException) { }
					catch (OverflowException) { }
				}

				if (optionGuid == Guid.Empty)
				{
					option = new QuestionOption();
				}
				else
				{
					option = new QuestionOption(optionGuid);
				}

				option.Answer = item.Text;
				option.QuestionGuid = question.QuestionGuid;
				option.Order = order++;
				option.Save();
			}
		}


		#region Events

		private void BtnAddOption_Click(object sender, EventArgs e)
		{
			txtNewOption.Text = txtNewOption.Text.Trim();

			if (txtNewOption.Text.Length == 0) return;

			if (String.IsNullOrEmpty(btnAddOption.CommandArgument))
			{
				if (lbOptions.Items.FindByText(txtNewOption.Text) != null) return;

				ListItem li = new ListItem(txtNewOption.Text);

				lbOptions.Items.Add(li);
			}
			else
			{
				ListItem itemToEdit = lbOptions.Items.FindByValue(btnAddOption.CommandArgument);

				if (itemToEdit != null)
				{
					itemToEdit.Text = txtNewOption.Text;
				}

				btnAddOption.CommandArgument = null;
				btnAddOption.Text = Resources.SurveyResources.QuestionEditOptionsAddButton;
			}

			txtNewOption.Text = String.Empty;
		}


		private void BtnDown_Click(Object sender, ImageClickEventArgs e)
		{
			if (lbOptions.SelectedItem == null) return;
			if (lbOptions.SelectedIndex == lbOptions.Items.Count - 1) return;

			ListItem selectedItem = lbOptions.SelectedItem;
			ListItem swapItem = lbOptions.Items[lbOptions.SelectedIndex + 1];

			String tmpText = selectedItem.Text;
			String tmpValue = selectedItem.Value;

			selectedItem.Text = swapItem.Text;
			selectedItem.Value = swapItem.Value;

			swapItem.Text = tmpText;
			swapItem.Value = tmpValue;

			lbOptions.SelectedIndex++;
		}


		private void BtnUp_Click(Object sender, ImageClickEventArgs e)
		{
			if (lbOptions.SelectedItem == null) return;
			if (lbOptions.SelectedIndex == 0) return;

			ListItem selectedItem = lbOptions.SelectedItem;
			ListItem swapItem = lbOptions.Items[lbOptions.SelectedIndex - 1];

			String tmpText = selectedItem.Text;
			String tmpValue = selectedItem.Value;

			selectedItem.Text = swapItem.Text;
			selectedItem.Value = swapItem.Value;

			swapItem.Text = tmpText;
			swapItem.Value = tmpValue;

			lbOptions.SelectedIndex--;
		}


		private void BtnDeleteOption_Click(Object sender, ImageClickEventArgs e)
		{
			if (lbOptions.SelectedItem == null) { return; }

			Guid optionGuid = Guid.Empty;

			if (lbOptions.SelectedItem.Value.Length == 36)
			{
				try
				{
					optionGuid = new Guid(lbOptions.SelectedItem.Value);
				}
				catch (FormatException) { }
				catch (OverflowException) { }
			}

			//if (lbOptions.SelectedItem.Value != lbOptions.SelectedItem.Text)
			if (optionGuid != Guid.Empty)
			{
				List<Guid> deletedOptions;
				deletedOptions = (List<Guid>)ViewState[Deleted_Options_Key] ?? new List<Guid>();

				//store the removed item in viewstate to remove from storage when saved
				//deletedOptions.Add(new Guid(lbOptions.SelectedItem.Value));
				deletedOptions.Add(optionGuid);
				ViewState.Add(Deleted_Options_Key, deletedOptions);
			}

			lbOptions.Items.Remove(lbOptions.SelectedItem);
		}


		private void BtnEdit_Click(object sender, ImageClickEventArgs e)
		{
			if (lbOptions.SelectedItem == null) return;

			String itemText = lbOptions.SelectedItem.Text;
			String itemValue = lbOptions.SelectedItem.Value;

			txtNewOption.Text = itemText;
			btnAddOption.CommandArgument = itemValue;
			btnAddOption.Text = SurveyResources.QuestionEditOptionsSaveButton;
		}


		protected void BtnSave_Click(object sender, EventArgs e)
		{
			Page.Validate("survey");

			if (Page.IsValid)
			{
				Question question = new Question(questionGuid);
				SaveQuestion(question);

				if (
					question.QuestionTypeId != (int)QuestionType.TextBox ||
					question.QuestionTypeId != (int)QuestionType.Date
				)
				{
					DeleteOldQuestionOptions();
					SaveQuestionOptions(question);
				}

				Business.Page page = new Business.Page(question.SurveyPageGuid);

				WebUtils.SetupRedirect(
					this,
					$"{SiteRoot}/Survey/SurveyQuestions.aspx?SurveyGuid={page.SurveyGuid.ToString()}&SurveyPageGuid={page.SurveyPageGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
				);
			}
		}


		void BtnCancel_Click(object sender, EventArgs e)
		{
			Business.Page page;

			if (surveyPageGuid == Guid.Empty)
			{
				Question question = new Question(questionGuid);
				page = new Business.Page(question.SurveyPageGuid);
			}
			else
			{
				page = new Business.Page(surveyPageGuid);
			}


			WebUtils.SetupRedirect(
				this,
				$"{SiteRoot}/Survey/SurveyQuestions.aspx?SurveyPageGuid={page.SurveyPageGuid}&SurveyGuid={page.SurveyGuid.ToString()}&pageid={PageId.ToInvariantString()}&mid={ModuleId.ToInvariantString()}"
			);
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
			btnAddOption.Click += new EventHandler(BtnAddOption_Click);
			btnDeleteOption.Click += new ImageClickEventHandler(BtnDeleteOption_Click);
			btnDown.Click += new ImageClickEventHandler(BtnDown_Click);
			btnUp.Click += new ImageClickEventHandler(BtnUp_Click);
			btnSave.Click += new EventHandler(BtnSave_Click);
			btnCancel.Click += new EventHandler(BtnCancel_Click);
			btnEdit.Click += new ImageClickEventHandler(BtnEdit_Click);

			SuppressPageMenu();

			SiteUtils.SetupEditor(edMessage, AllowSkinOverride, this);
		}

		#endregion
	}
}