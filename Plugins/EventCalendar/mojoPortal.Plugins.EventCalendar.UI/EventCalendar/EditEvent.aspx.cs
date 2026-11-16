using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.EventCalendarUI;


public partial class EventCalendarEdit : NonCmsBasePage
{
	private int moduleId = -1;
	private int itemId = -1;
	private DateTime thisDay = DateTime.Today;


	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;

		base.OnPreInit(e);
	}


	protected override void OnInit(EventArgs e)
	{
		Load += new EventHandler(Page_Load);
		btnUpdate.Click += new EventHandler(btnUpdate_Click);
		btnDelete.Click += new EventHandler(btnDelete_Click);

		base.OnInit(e);

		SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);

		edContent.WebEditor.ToolBar = ToolBar.Full;
	}

	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);

			return;
		}

		SecurityHelper.DisableBrowserCache();

		LoadSettings();

		if (!UserCanEditModule(moduleId, CalendarEvent.FeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);

			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);

			return;
		}

		PopulateLabels();

		if (!IsPostBack)
		{
			PopulateControls();

			if (Request.UrlReferrer != null && hdnReturnUrl.Value.Length == 0)
			{
				hdnReturnUrl.Value = Request.UrlReferrer.ToString();
				lnkCancel.NavigateUrl = hdnReturnUrl.Value;
			}
		}
	}


	private void PopulateControls()
	{
		if (itemId > -1)
		{
			var calEvent = new CalendarEvent(itemId);

			if (calEvent.ModuleId != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			txtTitle.Text = calEvent.Title;
			edContent.Text = calEvent.Description;
			dpEventDate.Text = calEvent.EventDate.ToShortDateString();

			var item = ddStartTime.Items.FindByValue(calEvent.StartTime.ToShortTimeString());

			if (item != null)
			{
				ddStartTime.ClearSelection();

				item.Selected = true;
			}

			item = ddEndTime.Items.FindByValue(calEvent.EndTime.ToShortTimeString());

			if (item != null)
			{
				ddEndTime.ClearSelection();

				item.Selected = true;
			}

			txtLocation.Text = calEvent.Location;
			chkShowMap.Checked = calEvent.ShowMap;
		}
		else
		{
			btnDelete.Visible = false;
		}

		rfvTitle.ErrorMessage = EventCalResources.TitleRequiredValidationMessage;
	}


	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, EventCalResources.EditEventPageTitle);
		heading.Text = EventCalResources.EventCalendarEditEntryLabel;
		btnUpdate.Text = EventCalResources.EventCalendarEditUpdateButton;

		SiteUtils.SetButtonAccessKey(btnUpdate, EventCalResources.EventCalendarEditUpdateButtonAccessKey);

		ScriptConfig.EnableExitPromptForUnsavedContent = true;

		UIHelper.DisableButtonAfterClickAndClearExitCode(
			btnUpdate,
			EventCalResources.ButtonDisabledPleaseWait,
			Page.ClientScript.GetPostBackEventReference(btnUpdate, string.Empty)
		);

		lnkCancel.Text = EventCalResources.EventCalendarEditCancelButton;
		btnDelete.Text = EventCalResources.EventCalendarEditDeleteButton;

		SiteUtils.SetButtonAccessKey(btnDelete, EventCalResources.EventCalendarEditDeleteButtonAccessKey);
		UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, EventCalResources.EventCalendarDeleteEventWarning);
	}


	private void btnCancel_Click(object sender, EventArgs e)
	{
		if (hdnReturnUrl.Value.Length > 0)
		{
			WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

			return;
		}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
	}


	private void btnDelete_Click(object sender, EventArgs e)
	{
		if (itemId > -1)
		{
			var calendarEvent = new CalendarEvent(itemId);

			if (calendarEvent.ModuleId != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			calendarEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);


			calendarEvent.Delete();
			CurrentPage.UpdateLastModifiedTime();
			SiteUtils.QueueIndexing();
		}

		if (hdnReturnUrl.Value.Length > 0)
		{
			WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

			return;
		}

		WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
	}


	private void calendarEvent_ContentChanged(object sender, ContentChangedEventArgs e)
	{
		IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["CalendarEventIndexBuilderProvider"];

		indexBuilder?.ContentChangedHandler(sender, e);
	}


	private void btnUpdate_Click(object sender, EventArgs e)
	{
		Page.Validate("eventcalendaredit");

		if (Page.IsValid)
		{
			var userId = -1;
			var userGuid = Guid.Empty;

			if (Request.IsAuthenticated)
			{
				var siteUser = SiteUtils.GetCurrentSiteUser();

				userId = siteUser.UserId;
				userGuid = siteUser.UserGuid;
			}

			var calEvent = new CalendarEvent(itemId);

			if (itemId > -1 && calEvent.ModuleId != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			calEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);

			var module = new Module(moduleId);

			calEvent.ModuleId = module.ModuleId;
			calEvent.ModuleGuid = module.ModuleGuid;
			calEvent.UserId = userId;
			calEvent.UserGuid = userGuid;
			calEvent.LastModUserGuid = userGuid;
			calEvent.Title = txtTitle.Text;
			calEvent.Description = edContent.Text;
			calEvent.EventDate = DateTime.Parse(dpEventDate.Text);
			calEvent.StartTime = DateTime.Parse(ddStartTime.SelectedValue);
			calEvent.EndTime = DateTime.Parse(ddEndTime.SelectedValue);
			calEvent.Location = txtLocation.Text;
			calEvent.ShowMap = chkShowMap.Checked;

			if (calEvent.Save())
			{
				CurrentPage.UpdateLastModifiedTime();
				CacheHelper.ClearModuleCache(moduleId);

				SiteUtils.QueueIndexing();

				if (hdnReturnUrl.Value.Length > 0)
				{
					WebUtils.SetupRedirect(this, hdnReturnUrl.Value);

					return;
				}

				WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
			}
		}
	}


	private void LoadSettings()
	{
		moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
		itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
		thisDay = WebUtils.ParseDateFromQueryString("date", DateTime.Today);

		lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

		if (!Page.IsPostBack)
		{
			dpEventDate.Text = thisDay.ToShortDateString();
		}

		var tomorrow = thisDay.AddDays(1).AddMinutes(-15);

		ddStartTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
		ddEndTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));

		var i = 0;

		while (thisDay < tomorrow)
		{
			i += 1;

			thisDay = thisDay.AddMinutes(15);

			ddStartTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
			ddEndTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));

		}

		AddClassToBody("eventcaledit");
	}
}
