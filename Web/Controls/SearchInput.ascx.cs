/// Author:
/// Created:       2005-06-26
/// Last Modified: 2017-07-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using Resources;
using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public partial class SearchInput : System.Web.UI.UserControl
	{
		protected Literal searchLink = new Literal();

		// these separator properties are deprecated
		// it is recommended not to use these properties
		// but instead to use mojoPortal.Web.Controls.SeparatorControl
		private bool useLeftSeparator = false;
		/// <summary>
		/// deprecated
		/// </summary>
		public bool UseLeftSeparator
		{
			get { return useLeftSeparator; }
			set { useLeftSeparator = value; }
		}

		private bool linkOnly = true;
		public bool LinkOnly
		{
			get { return linkOnly; }
			set { linkOnly = value; }
		}

		private bool useHeading = true;
		public bool UseHeading
		{
			get { return useHeading; }
			set { useHeading = value; }
		}

		private bool useWatermark = true;
		public bool UseWatermark
		{
			get { return useWatermark; }
			set { useWatermark = value; }
		}

		private string imageUrl = string.Empty;
		public string ImageUrl
		{
			get { return imageUrl; }
			set { imageUrl = value; }
		}

		private string buttonCssClass = string.Empty;
		public string ButtonCssClass
		{
			get { return buttonCssClass; }
			set { buttonCssClass = value; }
		}

		private string textBoxCssClass = string.Empty;
		public string TextBoxCssClass
		{
			get { return textBoxCssClass; }
			set { textBoxCssClass = value; }
		}

		private bool renderAsListItem = false;
		public bool RenderAsListItem
		{
			get { return renderAsListItem; }
			set { renderAsListItem = value; }
		}

		private string listItemCSS = "topnavitem";
		public string ListItemCss
		{
			get { return listItemCSS; }
			set { listItemCSS = value; }
		}

		private string linkCSS = "sitelink";
		public string LinkCSS
		{
			get { return linkCSS; }
			set { linkCSS = value; }
		}

		private string overrideWatermark = string.Empty;
		public string OverrideWatermark
		{
			get { return overrideWatermark; }
			set { overrideWatermark = value; }
		}

		private string overrideButtonText = string.Empty;
		public string OverrideButtonText
		{
			get { return overrideButtonText; }
			set { overrideButtonText = value; }
		}

		private bool hideOnSearchResultsPage = true;
		public bool HideOnSearchResultsPage
		{
			get { return hideOnSearchResultsPage; }
			set { hideOnSearchResultsPage = value; }
		}

		private bool hideOnSiteSettingsPage = true;
		public bool HideOnSiteSettingsPage
		{
			get { return hideOnSiteSettingsPage; }
			set { hideOnSiteSettingsPage = value; }
		}

		private bool hideOnLoginPage = true;
		public bool HideOnLoginPage
		{
			get { return hideOnLoginPage; }
			set { hideOnLoginPage = value; }
		}

		private bool hideOnRegistrationPage = true;
		public bool HideOnRegistrationPage
		{
			get { return hideOnRegistrationPage; }
			set { hideOnRegistrationPage = value; }
		}

		private bool hideOnPasswordRecoveryPage = true;
		public bool HideOnPasswordRecoveryPage
		{
			get { return hideOnPasswordRecoveryPage; }
			set { hideOnPasswordRecoveryPage = value; }
		}

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

			if (overrideButtonText.Length > 0)
			{
				searchButtonText = overrideButtonText;
			}

			string urlToUse = SiteUtils.GetRelativeNavigationSiteRoot() + "/SearchResults.aspx";

			if (linkOnly)
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
						+ urlToUse + "' class='" + linkCSS + "'>"
						+ searchButtonText + "<" + "/a>";
				}
				else
				{
					if (renderAsListItem)
					{
						searchLink.Text = "<li class='" + listItemCSS + "'><" + "a href='"
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
				heading.Visible = useHeading;

				if (
					(
					(!hideOnSearchResultsPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("SearchResults.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					|| (WebConfigSettings.ShowSkinSearchInputOnSearchResults)
					)
					&&
					(
					(!hideOnSiteSettingsPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("SiteSettings.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					|| (WebConfigSettings.ShowSearchInputOnSiteSettings)
					)
					&&
					(
					(!hideOnLoginPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("Login.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!hideOnRegistrationPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("Register.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!hideOnPasswordRecoveryPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("RecoverPassword.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					&&
					(
					(!hideOnPasswordRecoveryPage)
					|| (Request.CurrentExecutionFilePath.IndexOf("PasswordReset.aspx", StringComparison.InvariantCultureIgnoreCase) == -1)
					)
					)
				{
					btnSearch.Text = searchButtonText;

					if (useWatermark)
					{
						if (overrideWatermark.Length > 0)
						{
							txtSearch.Watermark = overrideWatermark;
						}
						else
						{
							txtSearch.Watermark = Resource.SearchInputWatermark;
						}
					}
					else
					{
						txtSearch.Watermark = string.Empty;
					}

					if (buttonCssClass.Length > 0)
					{
						btnSearch2.CssClass = buttonCssClass;
						btnSearch.CssClass = buttonCssClass;
					}

					if (textBoxCssClass.Length > 0)
					{
						txtSearch.CssClass = textBoxCssClass;
					}

					pnlSearch.Style.Add("display", "inline");

					if (imageUrl.Length > 0)
					{
						if (imageUrl.Contains("skinbase_"))
						{
							imageUrl = imageUrl.Replace("skinbase_", SiteUtils.GetSkinBaseUrl(Page));
						}

						pnlSearch.DefaultButton = btnSearch2.ID;
						btnSearch.Visible = false;
						btnSearch2.Visible = true;
						btnSearch2.ImageUrl = imageUrl;
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
