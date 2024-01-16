using System;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.ELetterUI;

public partial class ArchivePage : NonCmsBasePage
{
	private int totalPages = 1;
	private int pageNumber = 1;
	private int pageSize = 15;
	protected Double timeOffset = 0;
	protected TimeZoneInfo timeZone = null;
	private LetterInfo letterInfo = null;
	private Guid letterInfoGuid = Guid.Empty;
	private bool canView = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (!canView)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			SiteUtils.RedirectToAccessDeniedPage(this);
			return;

		}

		PopulateLabels();
		PopulateControls();
	}

	private void PopulateControls()
	{
		if (letterInfo.AllowArchiveView)
		{
			heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.NewsleterArchiveHeadFormat, letterInfo.Title);

			BindGrid();
		}
		else
		{
			grdLetter.Visible = false;
			lblMessage.Text = Resource.NewsletterArchivesNotAllowed;
		}
	}

	private void BindGrid()
	{
		var LetterList = Letter.GetPage(letterInfoGuid, pageNumber, pageSize, out totalPages);

		string pageUrl = $"{SiteRoot}/eletter/Archive.aspx?l={letterInfoGuid}&amp;pagenumber={{0}}";

		pgrLetter.PageURLFormat = pageUrl;
		pgrLetter.ShowFirstLast = true;
		pgrLetter.CurrentIndex = pageNumber;
		pgrLetter.PageSize = pageSize;
		pgrLetter.PageCount = totalPages;
		pgrLetter.Visible = (totalPages > 1);

		grdLetter.PageIndex = pageNumber;
		grdLetter.PageSize = pageSize;
		grdLetter.DataSource = LetterList;
		grdLetter.DataBind();
	}

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, Resource.NewslettersLink);
		lnkNewsletters.Text = Resource.NewslettersLink;
		grdLetter.Columns[0].HeaderText = Resource.NewsletterArchiveSubjectHeader;
		grdLetter.Columns[1].HeaderText = Resource.NewsletterArchiveSentHeader;
	}

	private void LoadSettings()
	{
		letterInfoGuid = WebUtils.ParseGuidFromQueryString("l", letterInfoGuid);
		pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();
		pageSize = WebConfigSettings.NewsletterArchivePageSize;
		ScriptConfig.IncludeColorBox = true;

		letterInfo = new LetterInfo(letterInfoGuid);
		if ((letterInfo.LetterInfoGuid == Guid.Empty) || (letterInfo.SiteGuid != siteSettings.SiteGuid)) { letterInfo = null; }

		if (letterInfo != null)
		{
			canView = WebUser.IsInRoles(letterInfo.AvailableToRoles);
		}

		lnkNewsletters.NavigateUrl = $"{SiteRoot}/eletter/";

		AddClassToBody("administration");
		AddClassToBody("eletterarchive");
	}

	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		ScriptConfig.IncludeJQTable = true;
	}
	#endregion
}