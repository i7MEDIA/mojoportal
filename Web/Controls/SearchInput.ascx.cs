using Resources;
using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public partial class SearchInput : System.Web.UI.UserControl
	{
		protected Literal searchLink = new();

		public bool UseLeftSeparator { get; set; } = false;

		public bool LinkOnly { get; set; } = true;

		public bool UseHeading { get; set; } = true;

		public bool UseWatermark { get; set; } = true;

		public string ImageUrl { get; set; } = string.Empty;

		public string ButtonCssClass { get; set; } = string.Empty;

		public string TextBoxCssClass { get; set; } = string.Empty;

		public bool RenderAsListItem { get; set; } = false;

		public string ListItemCss { get; set; } = "topnavitem";

		public string LinkCSS { get; set; } = "sitelink";

		/// <summary>
		/// Adds 'Placeholder' attribute to element
		/// </summary>
		public string OverrideWatermark { get; set; } = Resource.SearchInputWatermark;

		public string OverrideButtonText { get; set; } = string.Empty;

		public bool HideOnSearchResultsPage { get; set; } = true;

		public bool HideOnSiteSettingsPage { get; set; } = true;

		public bool HideOnLoginPage { get; set; } = true;

		public bool HideOnRegistrationPage { get; set; } = true;

		public bool HideOnPasswordRecoveryPage { get; set; } = true;

		private Guid featureGuid = Guid.Empty;
		/// <summary>
		/// used when you want to make a search input that only searches a single feature
		/// </summary>
		public Guid FeatureGuid
		{
			get { return featureGuid; }
			set { featureGuid = value; }
		}


		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (WebConfigSettings.DisableSearchIndex)
			{
				Visible = false;
				return;
			}

			lblSearchHeading.Text = Resource.SiteSearchHeading;
			txtSearch.ToolTip = Resource.SiteSearchHeading;
			string searchButtonText = Resources.Resource.SearchButtonText;

			if (OverrideButtonText.Length > 0)
			{
				searchButtonText = OverrideButtonText;
			}

			string urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + "/SearchResults.aspx";

			if (LinkOnly)
			{
				pnlSearch.Visible = false;
				heading.Visible = false;
				txtSearch.Visible = false;
				btnSearch.Visible = false;
				pnlS.Visible = false;

				EnableViewState = false;

				if (UseLeftSeparator)
				{
					searchLink.Text = "<" + "span class='accent'>|</span>"
						+ " <" + "a href='"
						+ urlToUse + "' class='" + LinkCSS + "'>"
						+ searchButtonText + "<" + "/a>";
				}
				else
				{
					if (RenderAsListItem)
					{
						searchLink.Text = "<li class='" + ListItemCss + "'><" + "a href='"
							+ urlToUse + "' class='sitelink'>"
							+ searchButtonText + "<" + "/a></li>";
					}
					else
					{
						searchLink.Text = " <" + "a href='"
							+ urlToUse + "' class='sitelink'>"
							+ searchButtonText + "<" + "/a>";
					}
				}

				Controls.Add(searchLink);
			}
			else
			{
				heading.Visible = UseHeading;

				if (
					(
					(!HideOnSearchResultsPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("SearchResults.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					|| (WebConfigSettings.ShowSkinSearchInputOnSearchResults)
					)
					&&
					(
					(!HideOnSiteSettingsPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("SiteSettings.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					|| (WebConfigSettings.ShowSearchInputOnSiteSettings)
					)
					&&
					(
					(!HideOnLoginPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("Login.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!HideOnRegistrationPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("Register.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!HideOnPasswordRecoveryPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("RecoverPassword.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!HideOnPasswordRecoveryPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("PasswordReset.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					)
				{
					btnSearch.Text = searchButtonText;

					if (UseWatermark)
					{
						if (OverrideWatermark.Length > 0)
						{
							txtSearch.Attributes.Add("placeholder", OverrideWatermark);
						}
						else
						{
							txtSearch.Attributes.Add("placeholder", Resource.SearchInputWatermark);
						}
					}

					if (ButtonCssClass.Length > 0)
					{
						btnSearch2.CssClass = ButtonCssClass;
						btnSearch.CssClass = ButtonCssClass;
					}

					if (TextBoxCssClass.Length > 0)
					{
						txtSearch.CssClass = TextBoxCssClass;
					}

					pnlSearch.Style.Add("display", "inline");

					if (ImageUrl.Length > 0)
					{
						if (ImageUrl.Contains("skinbase_"))
						{
							ImageUrl = ImageUrl.Replace("skinbase_", SiteUtils.DetermineSkinBaseUrl(page: Page));
						}

						pnlSearch.DefaultButton = btnSearch2.ID;
						btnSearch.Visible = false;
						btnSearch2.Visible = true;
						btnSearch2.ImageUrl = ImageUrl;
						btnSearch2.AlternateText = searchButtonText;
					}
					else
					{
						pnlSearch.DefaultButton = btnSearch.ID;
					}
				}
				else
				{
					pnlSearch.Visible = false;
					txtSearch.Visible = false;
					btnSearch.Visible = false;
				}
			}
		}

		protected void btnSearch2_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			DoRedirectToSearchResults();
		}

		protected void btnSearch_Click(object sender, EventArgs e)
		{
			DoRedirectToSearchResults();
		}

		private void DoRedirectToSearchResults()
		{
			string featureFilter = string.Empty;

			if (featureGuid != Guid.Empty)
			{
				featureFilter = "&f=" + featureGuid.ToString(); ;
			}

			if (
				(txtSearch.Text.Length > 0)
				&& (txtSearch.Text != Resource.SearchInputWatermark)
				)
			{
				string redirectUrl = SiteUtils.GetNavigationSiteRoot() + "/SearchResults.aspx?q=" + Server.UrlEncode(txtSearch.Text) + featureFilter;

				// need a hard redirect here to prevent the current page from continuing execution, not a soft redirect
				Response.Redirect(redirectUrl, true);
			}
		}
	}
}
