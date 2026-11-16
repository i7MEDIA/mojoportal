using System;
using System.Globalization;
using Helpers;
using mojoPortal.Business;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ContentUI;

public partial class HtmlCompare : mojoDialogBasePage
{
	private int pageId = -1;
	private int moduleId = -1;
	private Guid historyGuid = Guid.Empty;
	private Guid workflowGuid = Guid.Empty;
	protected Double timeOffset = 0;
	protected TimeZoneInfo timeZone = null;
	protected string currentFloat = "left";
	protected string historyFloat = "right";
	private bool userCanEdit = false;
	private bool userCanEditAsDraft = false;
	private bool highlightDiff = true;
	private HtmlRepository repository = new HtmlRepository();

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadParams();
	
		userCanEdit = UserCanEditModule(moduleId);
		userCanEditAsDraft = UserCanOnlyEditModuleAsDraft(moduleId);

		if ((!userCanEdit) && (!userCanEditAsDraft))
		{
			SiteUtils.RedirectToAccessDeniedPage();
			return;
		}

		LoadSettings();

		btnRestore.Text = Resource.RestoreToEditorButton;

		PopulateControls();

	}

	private void PopulateControls()
	{
		if (moduleId == -1) { return; }

		if (historyGuid != Guid.Empty)
		{
			ShowVsHistory();
			return;
		}

		if (workflowGuid != Guid.Empty)
		{
			ShowVsDraft();
		}
	}

	private void ShowVsHistory()
	{
		var html = repository.Fetch(moduleId);
		var history = new ContentHistory(historyGuid);
		if (history.ContentGuid != html.ModuleGuid) { return; }

		litCurrentHeading.Text = string.Format(Resource.CurrentVersionHeadingFormat,
			DateTimeHelper.Format(html.LastModUtc, timeZone, "g", timeOffset));

		if ((HtmlConfiguration.UseHtmlDiff) && (highlightDiff))
		{
			HtmlDiff diffHelper = new HtmlDiff(history.ContentText, html.Body);
			litCurrentVersion.Text = diffHelper.Build();
		}
		else
		{
			litCurrentVersion.Text = html.Body;
		}

		litHistoryHead.Text = string.Format(Resource.VersionAsOfHeadingFormat,
			DateTimeHelper.Format(history.CreatedUtc, timeZone, "g", timeOffset));

		litHistoryVersion.Text = history.ContentText;

		string onClick = "top.window.LoadHistoryInEditor('" + historyGuid.ToString() + "');  return false;";
		btnRestore.Attributes.Add("onclick", onClick);
	}

	private void ShowVsDraft()
	{
		var html = repository.Fetch(moduleId);

		if (html == null) { return; }

		var draftContent = ContentWorkflow.GetWorkInProgress(html.ModuleGuid);

		if (draftContent.Guid != workflowGuid) { return; }

		if (HtmlConfiguration.UseHtmlDiff && highlightDiff)
		{
			var diffHelper = new HtmlDiff(html.Body, draftContent.ContentText);
			litCurrentVersion.Text = diffHelper.Build();
		}
		else
		{
			litCurrentVersion.Text = draftContent.ContentText;
		}

		litCurrentHeading.Text = string.Format(Resource.CurrentDraftHeadingFormat,
			DateTimeHelper.Format(draftContent.RecentActionOn, timeZone, "g", timeOffset));

		litHistoryHead.Text = string.Format(Resource.CurrentVersionHeadingFormat,
			DateTimeHelper.Format(html.LastModUtc, timeZone, "g", timeOffset));

		litHistoryVersion.Text = html.Body;

		btnRestore.Visible = false;
	}

	void btnRestore_Click(object sender, EventArgs e)
	{
		// this should only fire if javascript is disabled because we put a client side on click
		string redirectUrl = $"{SiteUtils.GetNavigationSiteRoot()}/HtmlEdit.aspx?mid={moduleId.ToInvariantString()}&pageid={pageId.ToInvariantString()}&r={historyGuid}";

		Response.Redirect(redirectUrl);
	}

	private void LoadSettings()
	{
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();

		if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
		{
			currentFloat = "right";
			historyFloat = "left";

		}

		if (HtmlConfiguration.UseHtmlDiff)
		{
			if (highlightDiff)
			{
				lnkToggleHighlight.Text = Resource.DontHighlightDifferences;
				lnkToggleHighlight.NavigateUrl = $"{SiteRoot}/HtmlCompare.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}&h={historyGuid}&d={workflowGuid}&hd=false";
			}
			else
			{
				lnkToggleHighlight.Text = Resource.HighlightDifferences;
				lnkToggleHighlight.NavigateUrl = $"{SiteRoot}/HtmlCompare.aspx?pageid={pageId.ToInvariantString()}&mid={moduleId.ToInvariantString()}&h={historyGuid}&d={workflowGuid}&hd=true";
			}
		}
		else
		{
			lnkToggleHighlight.Visible = false;
		}
	}

	private void LoadParams()
	{
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
		historyGuid = WebUtils.ParseGuidFromQueryString("h", historyGuid);
		workflowGuid = WebUtils.ParseGuidFromQueryString("d", workflowGuid);
		highlightDiff = WebUtils.ParseBoolFromQueryString("hd", highlightDiff);
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(Page_Load);
		btnRestore.Click += new EventHandler(btnRestore_Click);
	}
}