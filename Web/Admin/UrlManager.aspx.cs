using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;

namespace mojoPortal.Web.AdminUI
{
	public partial class UrlManagerPage : NonCmsBasePage
	{
		private int totalPages = 1;
		private int pageNumber = 1;
		private int pageSize = 15;
		protected string RootUrl = string.Empty;
		private SiteMapDataSource siteMapDataSource;
		private Collection<DictionaryEntry> pageList;
		private bool isAdminOrContentAdmin = false;
		private bool isSiteEditor = false;
		protected string EditPropertiesImage = "~/Data/SiteImages/" + WebConfigSettings.EditPropertiesImage;
		protected string DeleteLinkImage = "~/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
		private string searchTerm = string.Empty;

		protected Collection<DictionaryEntry> PageList
		{
			get { return pageList; }

		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

			if ((!isAdminOrContentAdmin) && (!isSiteEditor))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}

			if (SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			PopulateLabels();
			if (!Page.IsPostBack)
			{
				PopulateControls();
			}

		}

		private void PopulateControls()
		{
			if (pageList != null)
			{
				ddPages.DataSource = pageList;
				ddPages.DataBind();
			}

			BindGrid();

			btnAddFriendlyUrl.Enabled = ddPages.Items.Count > 0;
		}

		private void BindGrid()
		{
			if (searchTerm.Length > 0)
			{
				BindForSearch();
			}
			else
			{
				BindNormal();
			}
		}

		private void BindNormal()
		{
			using IDataReader reader = FriendlyUrl.GetPage(
				siteSettings.SiteId,
				pageNumber,
				pageSize,
				out totalPages);
			if (totalPages > 1)
			{
				string pageUrl = SiteRoot + "/Admin/UrlManager.aspx?pagenumber={0}";

				pgrFriendlyUrls.PageURLFormat = pageUrl;
				pgrFriendlyUrls.ShowFirstLast = true;
				pgrFriendlyUrls.CurrentIndex = pageNumber;
				pgrFriendlyUrls.PageSize = pageSize;
				pgrFriendlyUrls.PageCount = totalPages;
			}
			else
			{
				pgrFriendlyUrls.Visible = false;
			}

			dlUrlMap.DataSource = reader;
			dlUrlMap.DataBind();
		}

		private void BindForSearch()
		{
			using IDataReader reader = FriendlyUrl.GetPage(
				siteSettings.SiteId,
				searchTerm,
				pageNumber,
				pageSize,
				out totalPages);
			if (totalPages > 1)
			{
				string pageUrl = $"{SiteRoot}/Admin/UrlManager.aspx?pagenumber={{0}}&amp;s={Server.UrlEncode(searchTerm)}";

				pgrFriendlyUrls.PageURLFormat = pageUrl;
				pgrFriendlyUrls.ShowFirstLast = true;
				pgrFriendlyUrls.CurrentIndex = pageNumber;
				pgrFriendlyUrls.PageSize = pageSize;
				pgrFriendlyUrls.PageCount = totalPages;
			}
			else
			{
				pgrFriendlyUrls.Visible = false;
			}

			dlUrlMap.DataSource = reader;
			dlUrlMap.DataBind();
		}

		void btnSearchUrls_Click(object sender, EventArgs e)
		{
			string pageUrl = SiteRoot + "/Admin/UrlManager.aspx?s=" + Server.UrlEncode(txtSearch.Text);
			WebUtils.SetupRedirect(this, pageUrl);
		}

		private void btnAddFriendlyUrl_Click(object sender, EventArgs e)
		{
			//FriendlyUrl needs a value
			if (string.IsNullOrWhiteSpace(txtFriendlyUrl.Text))
			{
				lblError.Text = Resource.FriendlyUrlInvalidEntryMessage;
				return;
			}

			//FriendlyUrl can't be a "Physical" page
			if (WebPageInfo.IsPhysicalWebPage("~/" + txtFriendlyUrl.Text))
			{
				lblError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
				return;
			}

			//FriendlyUrl can't already exist
			if (FriendlyUrl.Exists(siteSettings.SiteId, txtFriendlyUrl.Text)
				|| FriendlyUrl.Exists(siteSettings.SiteId, txtFriendlyUrl.Text.ToLower()))
			{
				lblError.Text = Resource.FriendlyUrlDuplicateWarning;
				return;
			}

			var url = new FriendlyUrl();

			//determine which "real" url entry to use (dropdown or manual input)
			if (ddPages.SelectedValue == "-1" && !string.IsNullOrWhiteSpace(txtRealUrl.Text))
			{
				//use direct real url entry
				url = new FriendlyUrl()
				{
					SiteId = siteSettings.SiteId,
					SiteGuid = siteSettings.SiteGuid,
					Url = txtFriendlyUrl.Text,
					RealUrl = txtRealUrl.Text
				};
			}
			else if (ddPages.SelectedValue != "-1")
			{
				//use dropdown selection
				url = new FriendlyUrl()
				{
					SiteId = siteSettings.SiteId,
					SiteGuid = siteSettings.SiteGuid,
					Url = txtFriendlyUrl.Text,
					RealUrl = Invariant($"Default.aspx?pageid={ddPages.SelectedValue}")
				};

				//get pageGuid from PageSettings
				if (int.TryParse(ddPages.SelectedValue, out int pageId))
				{
					if (pageId > -1)
					{
						var page = new PageSettings(siteSettings.SiteId, pageId);
						url.PageGuid = page.PageGuid;
					}
				}
			}
			else
			{
				lblError.Text = Resource.FriendlyUrlInvalidEntryMessage;
				return;
			}
			url.Save();
			WebUtils.SetupRedirect(this, Request.RawUrl);
		}

		void btnClearSearch_Click(object sender, EventArgs e)
		{
			string pageUrl = SiteRoot + "/Admin/UrlManager.aspx";
			WebUtils.SetupRedirect(this, pageUrl);
		}

		private void dlUrlMap_ItemCommand(object sender, DataListCommandEventArgs e)
		{
			int urlID = Convert.ToInt32(dlUrlMap.DataKeys[e.Item.ItemIndex]);
			var friendlyUrl = new FriendlyUrl(urlID);

			switch (e.CommandName)
			{
				case "edit":
					dlUrlMap.EditItemIndex = e.Item.ItemIndex;
					PopulateControls();
					break;

				case "apply":

					var txtItemFriendlyUrl = (TextBox)e.Item.FindControl("txtItemFriendlyUrl");
					var ddPagesEdit = (DropDownList)e.Item.FindControl("ddPagesEdit");
					var txtItemRealUrl = (TextBox)e.Item.FindControl("txtItemRealUrl");
					var lblEditError = (mojoLabel)e.Item.FindControl("lblEditError");
					//FriendlyUrl needs a value
					if (string.IsNullOrWhiteSpace(txtItemFriendlyUrl.Text))
					{
						lblEditError.Text = Resource.FriendlyUrlInvalidEntryMessage;
						return;
					}

					//FriendlyUrl can't be a "Physical" page
					if (WebPageInfo.IsPhysicalWebPage("~/" + txtItemFriendlyUrl.Text))
					{
						lblEditError.Text = Resource.FriendlyUrlWouldMaskPhysicalPageWarning;
						return;
					}

					//FriendlyUrl can't already exist
					if (friendlyUrl.Url.ToLower() != txtItemFriendlyUrl.Text.ToLower() && 
						(FriendlyUrl.Exists(siteSettings.SiteId, txtItemFriendlyUrl.Text)
						|| FriendlyUrl.Exists(siteSettings.SiteId, txtItemFriendlyUrl.Text.ToLower())))
					{
						lblEditError.Text = Resource.FriendlyUrlDuplicateWarning;
						return;
					}
					
					friendlyUrl.Url = txtItemFriendlyUrl.Text;

					//determine which "real" url entry to use (dropdown or manual input)
					if (ddPagesEdit is not null && ddPagesEdit.SelectedValue == "-1" && !string.IsNullOrWhiteSpace(txtItemRealUrl.Text))
					{
						//use direct real url entry
						friendlyUrl.RealUrl = txtItemRealUrl.Text;
						friendlyUrl.PageGuid = Guid.Empty;
					}
					else if (ddPagesEdit.SelectedValue != "-1")
					{
						//use dropdown selection
						if (int.TryParse(ddPagesEdit.SelectedValue, out int pageId))
						{
							//get pageGuid from PageSettings
							if (pageId > -1)
							{
								var page = new PageSettings(siteSettings.SiteId, pageId);
								friendlyUrl.PageGuid = page.PageGuid;
							}
						}

						friendlyUrl.RealUrl = Invariant($"Default.aspx?pageid={ddPagesEdit.SelectedValue}");
					}
					else
					{
						lblEditError.Text = Resource.FriendlyUrlInvalidEntryMessage;
						return;
					}

					friendlyUrl.Save();
					WebUtils.SetupRedirect(this, Request.RawUrl);
					
					break;

				case "delete":

					FriendlyUrl.DeleteUrl(urlID);
					WebUtils.SetupRedirect(this, Request.RawUrl);
					break;

				case "cancel":
					WebUtils.SetupRedirect(this, Request.RawUrl);
					break;
			}
		}

		void dlUrlMap_ItemDataBound(object sender, DataListItemEventArgs e)
		{
			ImageButton btnDeleteUrl = e.Item.FindControl("btnDeleteUrl") as ImageButton;
			UIHelper.AddConfirmationDialog(btnDeleteUrl, Resource.FriendlyUrlDeleteConfirmWarning);
		}

		protected string GetSelectedPage(string pageGuid)
		{
			var selectedPage = new PageSettings(Guid.Parse(pageGuid));
			return selectedPage.PageId.ToString();
		}

		protected string GetRealUrl(string realUrl, string pageGuid)
		{
			var selectedPage = new PageSettings(Guid.Parse(pageGuid));
			if (selectedPage.PageId != -1)
			{
				return string.Empty;
			}
			return realUrl.Replace("~/", string.Empty);
		}

		protected string ParsePageId(string stringToParse)
		{
			string result = string.Empty;

			if ((stringToParse.Length > 0) && (stringToParse.IndexOf("pageid=") > -1))
			{
				result = stringToParse.Remove(0, stringToParse.IndexOf("pageid=") + 7);

			}

			return result;
		}

		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuUrlManagerLink);

			heading.Text = Resource.AdminMenuUrlManagerLink;

			lnkAdminMenu.Text = Resource.AdminMenuLink;
			lnkAdminMenu.NavigateUrl = $"{SiteRoot}/Admin/AdminMenu.aspx";

			lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
			lnkAdvancedTools.NavigateUrl = $"{SiteRoot}/Admin/AdvancedTools.aspx";

			lnkUrlManager.Text = Resource.AdminMenuUrlManagerLink;
			lnkUrlManager.NavigateUrl = $"{SiteRoot}/Admin/UrlManager.aspx";

			btnAddFriendlyUrl.Text = Resource.FriendlyUrlAddNewLabel;
			SiteUtils.SetButtonAccessKey(btnAddFriendlyUrl, AccessKeys.FriendlyUrlAddNewLabelAccessKey);

			lblFriendlyUrlRoot.Text = RootUrl;

			btnSearchUrls.Text = Resource.SearchButtonText;
			btnClearSearch.Text = Resource.ClearSearch;
			pageList = [];
			PopulatePageList(pageList);
		}

		private void PopulatePageList(Collection<DictionaryEntry> deCollection)
		{
			pageList.Add(new DictionaryEntry(string.Empty, "-1"));
			siteMapDataSource = (SiteMapDataSource)Page.Master.FindControl("SiteMapData");

			siteMapDataSource.SiteMapProvider = $"mojosite{siteSettings.SiteId.ToString(CultureInfo.InvariantCulture)}";

			SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

			PopulatePageDictionary(deCollection, siteMapNode, string.Empty);
		}

		private void PopulatePageDictionary(
			Collection<DictionaryEntry> deCollection,
			SiteMapNode siteMapNode,
			string pagePrefix)
		{
			mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

			if (!mojoNode.IsRootNode)
			{
				if (mojoNode.ParentId > -1) pagePrefix += "-";

				deCollection.Add(new DictionaryEntry(
					mojoNode.Title,
					mojoNode.PageId.ToString())
					);
			}

			foreach (SiteMapNode childNode in mojoNode.ChildNodes)
			{
				//recurse to populate children
				PopulatePageDictionary(deCollection, childNode, pagePrefix);
			}
		}

		private void LoadSettings()
		{
			isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();
			pageSize = WebConfigSettings.UrlManagerPageSize;

			RootUrl = SiteRoot + "/";
			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

			if (Request.QueryString["s"] != null)
			{
				searchTerm = Request.QueryString["s"].RemoveMarkup();
			}

			if (!IsPostBack && searchTerm.Length > 0)
			{
				txtSearch.Text = searchTerm;
			}

			AddClassToBody("administration urlmanager");
		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			Load += new EventHandler(Page_Load);
			dlUrlMap.ItemDataBound += new DataListItemEventHandler(dlUrlMap_ItemDataBound);
			dlUrlMap.ItemCommand += new DataListCommandEventHandler(dlUrlMap_ItemCommand);
			btnAddFriendlyUrl.Click += new EventHandler(btnAddFriendlyUrl_Click);
			btnClearSearch.Click += new EventHandler(btnClearSearch_Click);
			btnSearchUrls.Click += new EventHandler(btnSearchUrls_Click);

			SuppressMenuSelection();
			SuppressPageMenu();

		}



		#endregion
	}
}
