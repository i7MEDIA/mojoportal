using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PageEventHandlers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
	public partial class PageProperties : NonCmsBasePage
	{
		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);

			AllowSkinOverride = (pageId > -1);
			base.OnPreInit(e);

			//http://www.4guysfromrolla.com/articles/071410-1.aspx
			//optimize viewstate for .NET 4
			this.ViewStateMode = ViewStateMode.Disabled;
			grdContentMeta.ViewStateMode = ViewStateMode.Enabled;
			grdMetaLinks.ViewStateMode = ViewStateMode.Enabled;
			ddChangeFrequency.ViewStateMode = ViewStateMode.Enabled;
			//ddIcons.ViewStateMode = ViewStateMode.Enabled;
			ddPages.ViewStateMode = ViewStateMode.Enabled;
			ddSiteMapPriority.ViewStateMode = ViewStateMode.Enabled;
			//ddSkins.ViewStateMode = ViewStateMode.Enabled;
			chkListAuthRoles.ViewStateMode = ViewStateMode.Enabled;
			chkListEditRoles.ViewStateMode = ViewStateMode.Enabled;
			chkDraftEditRoles.ViewStateMode = ViewStateMode.Enabled;
			chkDraftApprovalRoles.ViewStateMode = ViewStateMode.Enabled; //joe davis
			chkListCreateChildPageRoles.ViewStateMode = ViewStateMode.Enabled;
			chkIncludeInChildSiteMap.ViewStateMode = ViewStateMode.Enabled;
			SkinSetting.ViewStateMode = ViewStateMode.Enabled;
			publishType.ViewStateMode = ViewStateMode.Enabled;

			if (pageId > -1 &&
				siteSettings.AllowPageSkins &&
				CurrentPage != null &&
				CurrentPage.Skin.Length > 0)
			{
				if (Global.RegisteredVirtualThemes)
				{
					Theme = "pageskin-" + siteSettings.SiteId.ToInvariantString() + CurrentPage.Skin;
				}
				else
				{
					Theme = "pageskin";
				}
			}
		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			applyBtn.Click += new EventHandler(Apply_Click);
			btnDelete.Click += new EventHandler(btnDelete_Click);

			grdContentMeta.RowCommand += new GridViewCommandEventHandler(grdContentMeta_RowCommand);
			grdContentMeta.RowEditing += new GridViewEditEventHandler(grdContentMeta_RowEditing);
			grdContentMeta.RowUpdating += new GridViewUpdateEventHandler(grdContentMeta_RowUpdating);
			grdContentMeta.RowCancelingEdit += new GridViewCancelEditEventHandler(grdContentMeta_RowCancelingEdit);
			grdContentMeta.RowDeleting += new GridViewDeleteEventHandler(grdContentMeta_RowDeleting);
			grdContentMeta.RowDataBound += new GridViewRowEventHandler(grdContentMeta_RowDataBound);
			btnAddMeta.Click += new EventHandler(btnAddMeta_Click);

			grdMetaLinks.RowCommand += new GridViewCommandEventHandler(grdMetaLinks_RowCommand);
			grdMetaLinks.RowEditing += new GridViewEditEventHandler(grdMetaLinks_RowEditing);
			grdMetaLinks.RowUpdating += new GridViewUpdateEventHandler(grdMetaLinks_RowUpdating);
			grdMetaLinks.RowCancelingEdit += new GridViewCancelEditEventHandler(grdMetaLinks_RowCancelingEdit);
			grdMetaLinks.RowDeleting += new GridViewDeleteEventHandler(grdMetaLinks_RowDeleting);
			grdMetaLinks.RowDataBound += new GridViewRowEventHandler(grdMetaLinks_RowDataBound);
			btnAddMetaLink.Click += new EventHandler(btnAddMetaLink_Click);

			//ScriptConfig.IncludeYuiTabs = true;
			//IncludeYuiTabsCss = true;

			SuppressPageMenu();

			//JQueryUIThemeName = "base";
			ScriptConfig.IncludeJQTable = true;
		}

		#endregion

		private static readonly ILog log = LogManager.GetLogger(typeof(PageProperties));

		private bool sslIsAvailable = false;
		private bool isAdmin = false;
		private bool isAdminOrContentAdmin = false;
		private bool isSiteEditor = false;
		private bool canEdit = false;
		private bool canEditDraftOnly = false;
		private PageSettings pageSettings = null;
		private PageSettings parentPage = null;
		private int pageId = -1;
		private int startPageId = -1;
		private bool autosuggestFriendlyUrls = WebConfigSettings.AutoSuggestFriendlyUrls;
		private SiteMapDataSource siteMapDataSource;
		ContentMetaRespository metaRepository = new ContentMetaRespository();
		private SiteUser currentUser = null;
		private int pageCount = 0;
		private int childPageCount = 0;
		private bool useSeparatePagesForRoles = false;
		private TimeZoneInfo timeZone = null;
		private bool useWorkFlow = false;

		private void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadSettings();

			if (pageId > -1)
			{
				pageSettings = new PageSettings(siteSettings.SiteId, pageId);

				if (pageSettings.PageId == -1)
				{
					pageId = -1; // in case url was manipulated with an invlaid page id
				}

				if (pageSettings.EditRoles == "Admins;")
				{
					if (!WebUser.IsAdmin)
					{
						SiteUtils.RedirectToAccessDeniedPage(this);
						return;
					}
				}

				if (!IsPostBack)
				{
					if ((!ddPages.Visible) && (pageId > -1))
					{
						if (pageSettings.ParentId > -1)
						{
							parentPage = new PageSettings(siteSettings.SiteId, pageSettings.ParentId);
							hdnParentPageId.Value = parentPage.PageId.ToInvariantString();
							lblParentPageName.Text = parentPage.PageName;
						}
						else
						{
							hdnParentPageId.Value = "-1";
							lblParentPageName.Text = Resource.PageSettingsRootLabel;
						}
					}
				}
			}

			if (pageId == -1) //new page
			{
				pageSettings = new PageSettings();

				if (startPageId > -1)
				{
					// we'll inherit some defaults from parent
					parentPage = new PageSettings(siteSettings.SiteId, startPageId);
					if (parentPage.PageId == -1)
					{ parentPage = null; }

					if (parentPage != null)
					{
						// by default inherit setting from parent
						pageSettings.AuthorizedRoles = parentPage.AuthorizedRoles;
						pageSettings.EditRoles = parentPage.EditRoles;
						pageSettings.DraftEditOnlyRoles = parentPage.DraftEditOnlyRoles;
						pageSettings.DraftApprovalRoles = parentPage.DraftApprovalRoles; //joe davis
						pageSettings.CreateChildPageRoles = parentPage.CreateChildPageRoles;
						pageSettings.CreateChildDraftRoles = parentPage.CreateChildDraftRoles;

						if (!IsPostBack)
						{
							hdnParentPageId.Value = parentPage.PageId.ToInvariantString();
							lblParentPageName.Text = parentPage.PageName;
						}
					}
				}
				else
				{
					pageSettings.AuthorizedRoles = siteSettings.DefaultRootPageViewRoles;
					pageSettings.EditRoles = siteSettings.DefaultRootPageEditRoles;
					pageSettings.CreateChildPageRoles = siteSettings.DefaultRootPageCreateChildPageRoles;

					if (!isAdminOrContentAdmin && !isSiteEditor && WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
					{
						if (!IsPostBack)
						{
							lblParentPageName.Text = Resource.PageSettingsRootLabel;
							hdnParentPageId.Value = "-1";
						}
					}
				}

				btnDelete.Visible = false;
				heading.Text = Resource.PageSettingsCreateNewPageLabel;
			}

			pnlMeta.Visible = (pageSettings.PageGuid != Guid.Empty);

			canEdit = UserCanEdit();
			canEditDraftOnly = UserCanEditDraftOnly();

			if (!canEdit && !canEditDraftOnly)
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
			SetupScripts();

			if (!Page.IsPostBack)
			{
				PopulateControls();
				BindMeta();
				BindMetaLinks();
			}
		}

		private void PopulateControls()
		{
			PopulatePageList();
			PopulateChangeFrequencyDropdown();

			ListItem listItem;

			if (ddPages.Visible)
			{
				if (isAdminOrContentAdmin || isSiteEditor ||
					WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages) ||
					(pageSettings.PageId != -1 && pageSettings.ParentId == -1))
				{
					ddPages.Items.Insert(0, new ListItem(Resource.PageSettingsRootLabel, "-1"));
				}
				else
				{
					if ((ddPages.Items.Count == 0) && (pageId == -1))
					{
						//this user has no permission to edit child pages beneath any pages
						SiteUtils.RedirectToAccessDeniedPage();
					}
				}
			}

			// pre-select the parent for new pages
			if (startPageId > -1)
			{
				if (ddPages.Visible)
				{
					listItem = ddPages.Items.FindByValue(startPageId.ToString(CultureInfo.InvariantCulture));

					if (listItem != null)
					{
						ddPages.ClearSelection();
						listItem.Selected = true;
					}
				}

				//if user can only save in draft, and this is a new page, then mark as pending;
				chkIsPending.Checked = canEditDraftOnly;
				chkIsPending.Enabled = !canEditDraftOnly;
			}
			else
			{
				chkIsPending.Checked = pageSettings.IsPending;
				//if not new, then allow chkIsPending to be enabled if not actually checked:
				chkIsPending.Enabled = !canEditDraftOnly || !chkIsPending.Checked;
			}

			// default to monthly
			listItem = ddChangeFrequency.Items.FindByValue("Monthly");

			if (listItem != null)
			{
				ddChangeFrequency.ClearSelection();
				listItem.Selected = true;
			}

			if (pageId > -1)
			{
				if (pageSettings.BodyCssClass.Length > 0)
				{
					AddClassToBody(pageSettings.BodyCssClass);
				}

				listItem = ddChangeFrequency.Items.FindByValue(pageSettings.ChangeFrequency.ToString());

				if (listItem != null)
				{
					ddChangeFrequency.ClearSelection();
					listItem.Selected = true;
				}

				listItem = ddSiteMapPriority.Items.FindByValue(pageSettings.SiteMapPriority);

				if (listItem != null)
				{
					ddSiteMapPriority.ClearSelection();
					listItem.Selected = true;
				}

				lnkEditContent.NavigateUrl = SiteRoot + "/Admin/PageLayout.aspx?pageid=" + pageId.ToInvariantString();

				heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.SettingsForPageFormat, pageSettings.PageName);

				if (ddPages.Visible)
				{
					ddPages.ClearSelection();
					ddPages.SelectedIndex = ddPages.Items.IndexOf(ddPages.Items.FindByValue(pageSettings.ParentId.ToInvariantString()));
				}

				txtPageName.Text = pageSettings.PageName;
				txtPageTitle.Text = pageSettings.PageTitle;
				txtPageHeading.Text = pageSettings.PageHeading;
				chkShowPageHeading.Checked = pageSettings.ShowPageHeading;
				hdnPageName.Value = txtPageName.Text;
				publishType.SetValue(pageSettings.PublishMode.ToInvariantString());
				chkRequireSSL.Checked = pageSettings.RequireSsl;
				chkShowBreadcrumbs.Checked = pageSettings.ShowBreadcrumbs;
				chkShowChildPageBreadcrumbs.Checked = pageSettings.ShowChildPageBreadcrumbs;
				chkShowHomeCrumb.Checked = pageSettings.ShowHomeCrumb;
				txtPageKeywords.Text = pageSettings.PageMetaKeyWords;
				txtPageDescription.Text = pageSettings.PageMetaDescription;
				//txtPageEncoding.Text = pageSettings.PageMetaEncoding;
				//txtPageAdditionalMetaTags.Text = pageSettings.PageMetaAdditional;

				txtMenuDesc.Text = pageSettings.MenuDescription;

				chkUseUrl.Checked = pageSettings.UseUrl;
				txtUrl.Text = pageSettings.Url;
				chkNewWindow.Checked = pageSettings.OpenInNewWindow;
				chkIsClickable.Checked = pageSettings.IsClickable;
				chkShowChildMenu.Checked = pageSettings.ShowChildPageMenu;
				chkIncludeInMenu.Checked = pageSettings.IncludeInMenu;
				chkIncludeInSiteMap.Checked = pageSettings.IncludeInSiteMap;
				chkExpandOnSiteMap.Checked = pageSettings.ExpandOnSiteMap;
				chkIncludeInChildSiteMap.Checked = pageSettings.IncludeInChildSiteMap;
				chkAllowBrowserCache.Checked = pageSettings.AllowBrowserCache;
				chkHideAfterLogin.Checked = pageSettings.HideAfterLogin;
				chkEnableComments.Checked = pageSettings.EnableComments;

				lnkViewPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();
				chkIncludeInSearchEngineSiteMap.Checked = pageSettings.IncludeInSearchMap;
				txtCannonicalOverride.Text = pageSettings.CanonicalOverride;

				txtBodyCssClass.Text = pageSettings.BodyCssClass;
				txtMenuCssClass.Text = pageSettings.MenuCssClass;
				txtMenuLinkRelation.Text = pageSettings.LinkRel;

				pnlModified.Visible = true;
				lblCreatedDate.Text = TimeZoneInfo.ConvertTimeFromUtc(pageSettings.CreatedUtc, timeZone).ToString();
				lblLastModifiedDate.Text = TimeZoneInfo.ConvertTimeFromUtc(pageSettings.LastModUtc, timeZone).ToString();
				lblLastModifiedBy.Text = pageSettings.LastModByName;
				lblLastModifiedFromIp.Text = pageSettings.LastModFromIp;

				if (autosuggestFriendlyUrls && txtUrl.Text == string.Empty)
				{
					string friendlyUrl = SiteUtils.SuggestFriendlyUrl(txtPageName.Text, siteSettings);

					if (WebConfigSettings.AlwaysUrlEncode)
					{
						txtUrl.Text = "~/" + Server.UrlEncode(friendlyUrl);
					}
					else
					{
						txtUrl.Text = "~/" + friendlyUrl;
					}
				}
			}

			if (siteSettings.AllowPageSkins)
			{
				if (pageId > -1)
				{
					SkinSetting.PageUrl = SiteUtils.GetCurrentPageUrl();
				}

				if (pageSettings.Skin.Length > 0)
				{
					SkinSetting.SetValue(pageSettings.Skin);
				}
				else
				{
					if (pageId == -1 && WebConfigSettings.AssignNewPagesParentPageSkinByDefault)
					{
						if (parentPage != null)
						{
							SkinSetting.SetValue(parentPage.Skin);
						}
					}
				}
			}
			else
			{
				divSkin.Visible = false;
				SkinSetting.Visible = false;
			}

			if (siteSettings.AllowHideMenuOnPages)
			{
				chkHideMainMenu.Checked = pageSettings.HideMainMenu;
			}
			else
			{
				divHideMenu.Visible = false;
			}

			BindRoles(pageSettings);
		}

		private void BindRoles(PageSettings pageSettings)
		{
			if (useSeparatePagesForRoles)
			{
				return;
			}

			chkListAuthRoles.Items.Clear();

			ListItem allItem = new ListItem();

			// localize display
			allItem.Text = Resource.RolesAllUsersRole;
			allItem.Value = "All Users";

			if (pageSettings.AuthorizedRoles.LastIndexOf("All Users") > -1)
			{
				allItem.Selected = true;
			}

			chkListAuthRoles.Items.Add(allItem);

			chkListEditRoles.Items.Clear();
			chkListCreateChildPageRoles.Items.Clear();

			using (IDataReader reader = Role.GetSiteRoles(siteSettings.SiteId))
			{
				while (reader.Read())
				{
					string roleName = reader["RoleName"].ToString();

					// no need or benefit to checking content admins role
					// since they are not limited by roles except the special case of Admins only role
					if (roleName == Role.ContentAdministratorsRole)
					{
						continue;
					}

					// administrators role doensn't need permission, the only reason to show it is so that
					// an admin can lock the content down to only admins
					if (roleName == Role.AdministratorsRole)
					{
						continue;
					}

					ListItem listItem = new ListItem();

					listItem.Text = reader["DisplayName"].ToString();
					listItem.Value = reader["RoleName"].ToString();

					ListItem editItem = new ListItem();
					editItem.Text = reader["DisplayName"].ToString();
					editItem.Value = reader["RoleName"].ToString();

					ListItem draftItem = new ListItem();
					draftItem.Text = reader["DisplayName"].ToString();
					draftItem.Value = reader["RoleName"].ToString();

					ListItem draftApprovalItem = new ListItem();
					draftApprovalItem.Text = reader["DisplayName"].ToString();
					draftApprovalItem.Value = reader["RoleName"].ToString();

					ListItem childItem = new ListItem();
					childItem.Text = reader["DisplayName"].ToString();
					childItem.Value = reader["RoleName"].ToString();

					if (pageSettings.AuthorizedRoles == "Admins;")
					{
						rbViewAdminOnly.Checked = true;
						rbViewUseRoles.Checked = false;
					}
					else
					{
						rbViewAdminOnly.Checked = false;
						rbViewUseRoles.Checked = true;

						if (pageSettings.AuthorizedRoles.LastIndexOf(listItem.Value + ";") > -1)
						{
							listItem.Selected = true;
						}
					}

					if (pageSettings.EditRoles == "Admins;")
					{
						rbEditAdminOnly.Checked = true;
						rbEditUseRoles.Checked = false;
					}
					else
					{
						rbEditAdminOnly.Checked = false;
						rbEditUseRoles.Checked = true;

						if (pageSettings.EditRoles.LastIndexOf(editItem.Value + ";") > -1)
						{
							editItem.Selected = true;
						}
					}
					
					if (pageSettings.DraftEditOnlyRoles.LastIndexOf(draftItem.Value + ";") > -1)
					{
						draftItem.Selected = true;
					}


					if (pageSettings.DraftApprovalRoles.LastIndexOf(draftApprovalItem.Value + ";") > -1)
					{
						draftApprovalItem.Selected = true;
					}
					
					if (pageSettings.CreateChildPageRoles == "Admins;")
					{
						rbCreateChildAdminOnly.Checked = true;
						rbCreateChildUseRoles.Checked = false;
					}
					else
					{
						rbCreateChildAdminOnly.Checked = false;
						rbCreateChildUseRoles.Checked = true;

						if (pageSettings.CreateChildPageRoles.LastIndexOf(childItem.Value + ";") > -1)
						{
							childItem.Selected = true;
						}
					}

					chkListAuthRoles.Items.Add(listItem);
					chkListEditRoles.Items.Add(editItem);
					chkDraftEditRoles.Items.Add(draftItem);
					if (WebConfigSettings.Use3LevelContentWorkflow)
					{
						chkDraftApprovalRoles.Items.Add(draftApprovalItem); 
					}

					chkListCreateChildPageRoles.Items.Add(childItem);
				}
			}

			if (!isAdminOrContentAdmin && !isSiteEditor)
			{
				chkListAuthRoles.Enabled = false;
				chkListEditRoles.Enabled = false;
				chkListCreateChildPageRoles.Enabled = false;
				chkDraftEditRoles.Enabled = false;
				chkDraftApprovalRoles.Enabled = false; 
			}
		}

		private bool UserCanEdit()
		{
			if (isAdminOrContentAdmin)
			{
				return true;
			}

			if (WebUser.IsInRoles(pageSettings.EditRoles))
			{
				return true;
			}

			if (isSiteEditor)
			{
				return true;
			}

			if (WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
			{
				return true;
			}

			if (startPageId > -1)
			{
				PageSettings parentPage = new PageSettings(siteSettings.SiteId, startPageId);

				if (WebUser.IsInRoles(parentPage.CreateChildPageRoles))
				{
					return true;
				}
			}

			return false;
		}

		private bool UserCanEditDraftOnly()
		{
			if (isAdminOrContentAdmin)
			{
				return false;
			}

			if (isSiteEditor)
			{
				return false;
			}

			if (WebUser.IsInRoles(pageSettings.DraftEditOnlyRoles))
			{
				return true;
			}

			return false;
		}

		private void PopulateChangeFrequencyDropdown()
		{
			// TODO: localize display

			ListItem listItem = new ListItem(Resource.PageChangeFrequencyAlways, "Always");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyHourly, "Hourly");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyDaily, "Daily");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyWeekly, "Weekly");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyMonthly, "Monthly");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyYearly, "Yearly");
			ddChangeFrequency.Items.Add(listItem);

			listItem = new ListItem(Resource.PageChangeFrequencyNever, "Never");
			ddChangeFrequency.Items.Add(listItem);
		}

		private void PopulatePageList()
		{
			if (!ddPages.Visible)
			{
				return;
			}

			siteMapDataSource = (SiteMapDataSource)Page.Master.FindControl("SiteMapData");
			siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();
			SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
			PopulateListControl(ddPages, siteMapNode, string.Empty);
		}

		private void PopulateListControl(
			ListControl listBox,
			SiteMapNode siteMapNode,
			string pagePrefix
		)
		{
			mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

			if (!mojoNode.IsRootNode)
			{
				if (
					isAdminOrContentAdmin ||
					isSiteEditor ||
					WebUser.IsInRoles(mojoNode.CreateChildPageRoles) ||
					(pageSettings.ParentId == mojoNode.PageId)
				)
				{
					// don't let children of this page be a choice for this page parent, its circular and causes an error
					// dont let a page be it's own parent
					if (
						((mojoNode.ParentId != pageId) && (mojoNode.PageId != pageId)) ||
						(pageId == -1)
					)
					{
						if (mojoNode.ParentId > -1)
						{
							pagePrefix += "-";
						}

						ListItem listItem = new ListItem
						{
							Text = pagePrefix + Server.HtmlDecode(mojoNode.Title),
							Value = mojoNode.PageId.ToString()
						};

						listBox.Items.Add(listItem);
					}
				}
			}

			if ((mojoNode.PageId != pageId) || (pageId == -1)) // don't recurse through children of the current page
			{
				foreach (SiteMapNode childNode in mojoNode.ChildNodes)
				{
					//recurse to populate children
					PopulateListControl(listBox, childNode, pagePrefix);
				}
			}
		}

		private void Apply_Click(object sender, EventArgs e)
		{
			bool pageNewlyCreated = pageId == -1;

			if (SavePageData())
			{
				// for some users it may be clearer to redirect them back
				// to the new page where they will see the new content wizard
				if (SiteUtils.RedirectToPageAfterCreation(siteSettings))
				{
					WebUtils.SetupRedirect(this, SiteUtils.GetPageUrl(pageSettings));
				}
				else
				{
					WebUtils.SetupRedirect(
						this,
						string.Format(
							CultureInfo.InvariantCulture, "{0}/Admin/PageSettingsSaved.ashx?pageid={1}&pagenewlycreated={2}",
							SiteRoot,
							pageId,
							pageNewlyCreated
						)
					);
				}
			}
		}

		void btnDelete_Click(object sender, EventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (WebConfigSettings.LogIpAddressForContentDeletions)
			{
				string userName = string.Empty;

				if (currentUser != null)
				{
					userName = currentUser.Name;
				}

				log.Info($"user {userName} deleted page {pageSettings.PageName} from ip address {SiteUtils.GetIP4Address()}");
			}

			metaRepository.DeleteByContent(pageSettings.PageGuid);
			Module.DeletePageModules(pageSettings.PageId);
			PageSettings.DeletePage(pageSettings.PageId);
			FriendlyUrl.DeleteByPageGuid(pageSettings.PageGuid);

			mojoPortal.SearchIndex.IndexHelper.ClearPageIndexAsync(pageSettings);
			CacheHelper.ResetSiteMapCache(siteSettings.SiteId);

			WebUtils.SetupRedirect(this, SiteRoot + "/Default.aspx");
		}

		private bool SavePageData()
		{
			Page.Validate("pagesettings");

			if (!Page.IsValid)
			{
				return false;
			}

			bool result = true;
			bool reIndexPage = false;
			bool clearIndex = false;
			int newParentID;

			if (ddPages.Visible)
			{
				if (!int.TryParse(ddPages.SelectedValue, NumberStyles.Any, CultureInfo.InvariantCulture, out newParentID))
				{
					newParentID = -1;
				}
			}
			else
			{
				if (!int.TryParse(hdnParentPageId.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out newParentID))
				{
					newParentID = -1;
				}
			}

			PageSettings parentPage = new PageSettings(siteSettings.SiteId, newParentID);
			pageSettings.ParentGuid = parentPage.PageGuid;
			pageSettings.SiteId = siteSettings.SiteId;
			pageSettings.SiteGuid = siteSettings.SiteGuid;

			if ((pageSettings.PageId != newParentID && pageSettings.ParentId != newParentID) ||
				pageSettings.PageId == -1)
			{
				pageSettings.ParentId = newParentID;
				pageSettings.PageOrder = PageSettings.GetNextPageOrder(pageSettings.SiteId, newParentID);
			}

			string userName = string.Empty;

			if (currentUser != null)
			{
				userName = currentUser.Name;
			}

			if (isAdminOrContentAdmin && !useSeparatePagesForRoles)
			{
				if (rbViewAdminOnly.Checked)
				{
					if (pageSettings.AuthorizedRoles != "Admins;")
					{
						log.Info($"user {userName} changed page view roles for {pageSettings.PageName} to Admins from ip address {SiteUtils.GetIP4Address()}");
						pageSettings.AuthorizedRoles = "Admins;";
						reIndexPage = true;
					}
				}
				else
				{
					string authorizedRoles = chkListAuthRoles.Items.SelectedItemsToSemiColonSeparatedString();

					if (pageSettings.AuthorizedRoles != authorizedRoles)
					{
						log.Info($"user {userName} changed page view roles for {pageSettings.PageName} to {authorizedRoles} from ip address {SiteUtils.GetIP4Address()}");
						pageSettings.AuthorizedRoles = authorizedRoles;
						reIndexPage = true;
					}
				}

				if (rbEditAdminOnly.Checked)
				{
					pageSettings.EditRoles = "Admins;";
				}
				else
				{
					pageSettings.EditRoles = chkListEditRoles.Items.SelectedItemsToSemiColonSeparatedString();
				}

				if (rbCreateChildAdminOnly.Checked)
				{
					pageSettings.CreateChildPageRoles = "Admins;";
				}
				else
				{
					pageSettings.CreateChildPageRoles = chkListCreateChildPageRoles.Items.SelectedItemsToSemiColonSeparatedString();
				}

				pageSettings.DraftEditOnlyRoles = chkDraftEditRoles.Items.SelectedItemsToSemiColonSeparatedString();
				pageSettings.DraftApprovalRoles = chkDraftApprovalRoles.Items.SelectedItemsToSemiColonSeparatedString();
			}

			if (pageId == -1)
			{
				if ((!isAdminOrContentAdmin) || (useSeparatePagesForRoles))
				{
					if (newParentID > -1)
					{
						// by default inherit permissions from parent
						pageSettings.AuthorizedRoles = parentPage.AuthorizedRoles;
						pageSettings.EditRoles = parentPage.EditRoles;
						pageSettings.CreateChildPageRoles = parentPage.CreateChildPageRoles;
						pageSettings.DraftEditOnlyRoles = parentPage.DraftEditOnlyRoles;
						pageSettings.DraftApprovalRoles = parentPage.DraftApprovalRoles; 

						if (WebUser.IsInRoles(parentPage.EditRoles))
						{
							pageSettings.EditRoles = parentPage.EditRoles;
						}
						else
						{
							pageSettings.EditRoles = parentPage.CreateChildPageRoles;
						}
					}
					else
					{
						if (WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
						{
							pageSettings.AuthorizedRoles = siteSettings.DefaultRootPageViewRoles;
							pageSettings.EditRoles = siteSettings.RolesThatCanCreateRootPages;
							pageSettings.CreateChildPageRoles = siteSettings.RolesThatCanCreateRootPages;
							pageSettings.DraftEditOnlyRoles = siteSettings.SiteRootDraftEditRoles;
							pageSettings.DraftApprovalRoles = siteSettings.SiteRootDraftApprovalRoles;
						}
					}
				}

				if (currentUser != null)
				{
					pageSettings.CreatedBy = currentUser.UserGuid;
				}

				pageSettings.CreatedFromIp = SiteUtils.GetIP4Address();
			}

			pageSettings.PageName = SecurityHelper.RemoveMarkup(txtPageName.Text);
			pageSettings.PageTitle = txtPageTitle.Text;
			pageSettings.PublishMode = Convert.ToInt32(publishType.GetValue(), CultureInfo.InvariantCulture);

			if (divPageHeading.Visible)
			{
				pageSettings.PageHeading = txtPageHeading.Text;
			}

			if (divShowPageHeading.Visible)
			{
				pageSettings.ShowPageHeading = chkShowPageHeading.Checked;
			}

			if (sslIsAvailable)
			{
				pageSettings.RequireSsl = chkRequireSSL.Checked;
			}

			pageSettings.AllowBrowserCache = chkAllowBrowserCache.Checked;
			pageSettings.ShowBreadcrumbs = chkShowBreadcrumbs.Checked;
			pageSettings.ShowChildPageBreadcrumbs = chkShowChildPageBreadcrumbs.Checked;
			pageSettings.ShowHomeCrumb = chkShowHomeCrumb.Checked;

			if (WebConfigSettings.IndexPageMeta && pageSettings.PageMetaKeyWords != txtPageKeywords.Text)
			{
				reIndexPage = true;
			}

			pageSettings.PageMetaKeyWords = txtPageKeywords.Text;

			if (WebConfigSettings.IndexPageMeta && pageSettings.PageMetaDescription != txtPageDescription.Text)
			{
				reIndexPage = true;
			}

			pageSettings.PageMetaDescription = txtPageDescription.Text;

			if (divUseUrl.Visible)
			{
				pageSettings.UseUrl = chkUseUrl.Checked;
			}
			else
			{
				pageSettings.UseUrl = (txtUrl.Text.Length > 0);
			}

			if (divMenuDesc.Visible)
			{
				pageSettings.MenuDescription = txtMenuDesc.Text;
			}

			pageSettings.OpenInNewWindow = chkNewWindow.Checked;
			pageSettings.ShowChildPageMenu = chkShowChildMenu.Checked;
			pageSettings.IsClickable = chkIsClickable.Checked;
			pageSettings.IncludeInMenu = chkIncludeInMenu.Checked;
			pageSettings.IncludeInSiteMap = chkIncludeInSiteMap.Checked;
			pageSettings.ExpandOnSiteMap = chkExpandOnSiteMap.Checked;
			pageSettings.IncludeInChildSiteMap = chkIncludeInChildSiteMap.Checked;

			pageSettings.HideAfterLogin = chkHideAfterLogin.Checked;
			pageSettings.IncludeInSearchMap = chkIncludeInSearchEngineSiteMap.Checked;
			pageSettings.CanonicalOverride = txtCannonicalOverride.Text;
			pageSettings.EnableComments = chkEnableComments.Checked;
			pageSettings.BodyCssClass = txtBodyCssClass.Text;
			pageSettings.MenuCssClass = txtMenuCssClass.Text;
			pageSettings.LinkRel = txtMenuLinkRelation.Text;

			if (siteSettings.AllowPageSkins)
			{
				pageSettings.Skin = SkinSetting.GetValue();
			}

			if (siteSettings.AllowHideMenuOnPages)
			{
				pageSettings.HideMainMenu = chkHideMainMenu.Checked;
			}

			string friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", string.Empty));

			//when using extensionless urls lets not allow a trailing slash
			//if the user enters on in the browser we can resolve it to the page
			//but its easier if we store them consistently in the db without the /
			if (friendlyUrlString.EndsWith("/") && (!friendlyUrlString.StartsWith("http")))
			{
				friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
			}

			FriendlyUrl friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

			if (friendlyUrl.FoundFriendlyUrl && friendlyUrl.PageGuid != pageSettings.PageGuid
				&& pageSettings.Url != txtUrl.Text
				&& !txtUrl.Text.StartsWith("http"))
			{
				lblError.Text = Resource.PageUrlInUseErrorMessage;
				return false;
			}

			string oldUrl = pageSettings.Url.Replace("~/", string.Empty);
			string newUrl = friendlyUrlString;

			if (txtUrl.Text.StartsWith("http") || (txtUrl.Text == "~/"))
			{
				pageSettings.Url = txtUrl.Text;
			}
			else if (friendlyUrlString.Length > 0)
			{
				pageSettings.Url = "~/" + friendlyUrlString;
			}
			else if (friendlyUrlString.Length == 0)
			{
				pageSettings.Url = string.Empty;
			}

			pageSettings.ChangeFrequency = (PageChangeFrequency)Enum.Parse(typeof(PageChangeFrequency), ddChangeFrequency.SelectedValue);
			pageSettings.SiteMapPriority = ddSiteMapPriority.SelectedValue;

			if (pageSettings.PageId == -1)
			{
				pageSettings.PageCreated += new PageCreatedEventHandler(pageSettings_PageCreated);

				// no need to index new page until content is added
				reIndexPage = false;
			}

			if (divIsPending.Visible && chkIsPending.Enabled)
			{
				if (pageSettings.IsPending && !chkIsPending.Checked)
				{
					// page changed from draft to published so need to index
					reIndexPage = true;
				}

				if (!pageSettings.IsPending && chkIsPending.Checked)
				{
					//changed from published back to draft
					//need to clear the search index 
					reIndexPage = false;
					clearIndex = true;
				}

				pageSettings.IsPending = chkIsPending.Checked;
			}

			pageSettings.LastModifiedUtc = DateTime.UtcNow;

			if (currentUser != null)
			{
				pageSettings.LastModBy = currentUser.UserGuid;
			}

			pageSettings.LastModFromIp = SiteUtils.GetIP4Address();

			bool saved = pageSettings.Save();
			pageId = pageSettings.PageId;

			//if page was renamed url will change, if url changes we need to redirect from the old url to the new with 301
			// don't create a redirect for external urls, ie starting with "http"
			if (
				oldUrl.Length > 0 &&
				newUrl.Length > 0 &&
				!SiteUtils.UrlsMatch(oldUrl, newUrl) &&
				!oldUrl.StartsWith("http") &&
				!newUrl.StartsWith("http")
			)
			{
				//worry about the risk of a redirect loop if the page is restored to the old url again
				// don't create it if a redirect for the new url exists
				if (
					!RedirectInfo.Exists(siteSettings.SiteId, oldUrl) &&
					!RedirectInfo.Exists(siteSettings.SiteId, newUrl)
				)
				{
					RedirectInfo redirect = new RedirectInfo();
					redirect.SiteGuid = siteSettings.SiteGuid;
					redirect.SiteId = siteSettings.SiteId;
					redirect.OldUrl = oldUrl;
					redirect.NewUrl = newUrl;
					redirect.Save();
				}

				// since we have created a redirect we don't need the old friendly url
				FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);

				if (oldFriendlyUrl.FoundFriendlyUrl && (oldFriendlyUrl.PageGuid == pageSettings.PageGuid))
				{
					FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
				}

				// url changed so it needs ot be re-indexed
				reIndexPage = true;
				clearIndex = true;
			}

			if (
				(txtUrl.Text.EndsWith(".aspx") || siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName) &&
				txtUrl.Text.StartsWith("~/")
			)
			{
				if (!friendlyUrl.FoundFriendlyUrl)
				{
					if (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString))
					{
						FriendlyUrl newFriendlyUrl = new FriendlyUrl
						{
							SiteId = siteSettings.SiteId,
							SiteGuid = siteSettings.SiteGuid,
							PageGuid = pageSettings.PageGuid,
							Url = friendlyUrlString,
							RealUrl = "~/Default.aspx?pageid=" + pageId.ToInvariantString()
						};

						newFriendlyUrl.Save();
					}
				}
			}

			CacheHelper.ResetSiteMapCache();

			if (saved && reIndexPage)
			{
				pageSettings.PageIndex = CurrentPage.PageIndex;
				SearchIndex.IndexHelper.RebuildPageIndexAsync(pageSettings);
				SiteUtils.QueueIndexing();
			}
			else if (saved && clearIndex)
			{
				SearchIndex.IndexHelper.ClearPageIndexAsync(pageSettings);
			}

			return result;
		}


		void pageSettings_PageCreated(object sender, PageCreatedEventArgs e)
		{
			// this is a hook so that custom code can be fired when pages are created
			// implement a PageCreatedEventHandlerPovider and put a config file for it in
			// /Setup/ProviderConfig/pagecreatedeventhandlers
			try
			{
				foreach (PageCreatedEventHandlerPovider handler in PageCreatedEventHandlerPoviderManager.Providers)
				{
					handler.PageCreatedHandler(sender, e);
				}
			}
			catch (TypeInitializationException ex)
			{
				log.Error(ex);
			}
		}


		#region Meta Data
		private void BindMeta()
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			List<ContentMeta> meta = metaRepository.FetchByContent(pageSettings.PageGuid);
			grdContentMeta.DataSource = meta;
			grdContentMeta.DataBind();

			btnAddMeta.Visible = true;
		}

		void grdContentMeta_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;
			string sGuid = e.CommandArgument.ToString();
			
			if (sGuid.Length != 36)
			{
				return;
			}

			Guid guid = new Guid(sGuid);
			ContentMeta meta = metaRepository.Fetch(guid);

			if (meta == null)
			{
				return;
			}

			switch (e.CommandName)
			{
				case "MoveUp":
					meta.SortRank -= 3;
					break;

				case "MoveDown":
					meta.SortRank += 3;
					break;
			}

			metaRepository.Save(meta);

			List<ContentMeta> metaList = metaRepository.FetchByContent(pageSettings.PageGuid);
			metaRepository.ResortMeta(metaList);

			pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
			pageSettings.Save();

			BindMeta();
			upMeta.Update();
		}

		void grdContentMeta_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;
			Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
			metaRepository.Delete(guid);

			pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
			pageSettings.Save();
			grdContentMeta.Columns[2].Visible = true;

			BindMeta();
			upMeta.Update();
		}

		void grdContentMeta_RowEditing(object sender, GridViewEditEventArgs e)
		{
			GridView grid = (GridView)sender;
			grid.EditIndex = e.NewEditIndex;

			BindMeta();
			
			Button btnDeleteMeta = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMeta");

			if (btnDeleteMeta != null)
			{
				btnDelete.Attributes.Add("OnClick", "return confirm('" + Resource.ContentMetaDeleteWarning + "');");
			}

			upMeta.Update();
		}

		void grdContentMeta_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			GridView grid = (GridView)sender;

			if (grid.EditIndex > -1)
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					DropDownList ddDirection = (DropDownList)e.Row.Cells[1].FindControl("ddDirection");

					if (ddDirection != null)
					{
						if (e.Row.DataItem is ContentMeta)
						{
							ListItem item = ddDirection.Items.FindByValue(((ContentMeta)e.Row.DataItem).Dir);

							if (item != null)
							{
								ddDirection.ClearSelection();
								item.Selected = true;
							}
						}
					}

					if (!(e.Row.DataItem is ContentMeta))
					{
						//the add button was clicked so hide the delete button
						Button btnDeleteMeta = (Button)e.Row.Cells[1].FindControl("btnDeleteMeta");

						if (btnDeleteMeta != null)
						{
							btnDeleteMeta.Visible = false;
						}
					}
				}
			}
		}

		void grdContentMeta_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;

			Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
			TextBox txtName = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtName");
			TextBox txtNameProperty = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtNameProperty");
			TextBox txtScheme = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtScheme");
			TextBox txtLangCode = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtLangCode");
			DropDownList ddDirection = (DropDownList)grid.Rows[e.RowIndex].Cells[1].FindControl("ddDirection");
			TextBox txtMetaContent = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtMetaContent");
			TextBox txtContentProperty = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtContentProperty");

			ContentMeta meta = null;

			if (guid != Guid.Empty)
			{
				meta = metaRepository.Fetch(guid);
			}
			else
			{
				meta = new ContentMeta();

				if (currentUser != null)
				{
					meta.CreatedBy = currentUser.UserGuid;
				}

				meta.SortRank = metaRepository.GetNextSortRank(pageSettings.PageGuid);
			}

			if (meta != null)
			{
				meta.SiteGuid = siteSettings.SiteGuid;
				meta.ContentGuid = pageSettings.PageGuid;
				meta.Dir = ddDirection.SelectedValue;
				meta.LangCode = txtLangCode.Text;
				meta.MetaContent = txtMetaContent.Text;
				meta.ContentProperty = txtContentProperty.Text;
				meta.Name = txtName.Text;
				meta.NameProperty = txtNameProperty.Text;
				meta.Scheme = txtScheme.Text;

				if (currentUser != null)
				{
					meta.LastModBy = currentUser.UserGuid;
				}

				metaRepository.Save(meta);

				pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
				pageSettings.Save();
			}

			grid.EditIndex = -1;
			grdContentMeta.Columns[2].Visible = true;

			BindMeta();
			upMeta.Update();
		}

		void grdContentMeta_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{
			grdContentMeta.EditIndex = -1;
			grdContentMeta.Columns[2].Visible = true;

			BindMeta();
			upMeta.Update();
		}

		void btnAddMeta_Click(object sender, EventArgs e)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Guid", typeof(Guid));
			dataTable.Columns.Add("SiteGuid", typeof(Guid));
			dataTable.Columns.Add("ModuleGuid", typeof(Guid));
			dataTable.Columns.Add("ContentGuid", typeof(Guid));
			dataTable.Columns.Add("Name", typeof(string));
			dataTable.Columns.Add("NameProperty", typeof(string));
			dataTable.Columns.Add("Scheme", typeof(string));
			dataTable.Columns.Add("LangCode", typeof(string));
			dataTable.Columns.Add("Dir", typeof(string));
			dataTable.Columns.Add("MetaContent", typeof(string));
			dataTable.Columns.Add("ContentProperty", typeof(string));
			dataTable.Columns.Add("SortRank", typeof(int));

			DataRow row = dataTable.NewRow();
			row["Guid"] = Guid.Empty;
			row["SiteGuid"] = siteSettings.SiteGuid;
			row["ModuleGuid"] = Guid.Empty;
			row["ContentGuid"] = Guid.Empty;
			row["Name"] = string.Empty;
			row["NameProperty"] = string.Empty;
			row["Scheme"] = string.Empty;
			row["LangCode"] = string.Empty;
			row["Dir"] = string.Empty;
			row["MetaContent"] = string.Empty;
			row["ContentProperty"] = string.Empty;
			row["SortRank"] = 3;

			dataTable.Rows.Add(row);

			grdContentMeta.EditIndex = 0;
			grdContentMeta.DataSource = dataTable.DefaultView;
			grdContentMeta.DataBind();
			grdContentMeta.Columns[2].Visible = false;
			btnAddMeta.Visible = false;

			upMeta.Update();
		}

		private void BindMetaLinks()
		{
			if (pageSettings == null)
			{
				return;
			}
			
			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			List<ContentMetaLink> meta = metaRepository.FetchLinksByContent(pageSettings.PageGuid);

			grdMetaLinks.DataSource = meta;
			grdMetaLinks.DataBind();

			btnAddMetaLink.Visible = true;
		}

		void btnAddMetaLink_Click(object sender, EventArgs e)
		{
			DataTable dataTable = new DataTable();
			dataTable.Columns.Add("Guid", typeof(Guid));
			dataTable.Columns.Add("SiteGuid", typeof(Guid));
			dataTable.Columns.Add("ModuleGuid", typeof(Guid));
			dataTable.Columns.Add("ContentGuid", typeof(Guid));
			dataTable.Columns.Add("Rel", typeof(string));
			dataTable.Columns.Add("Href", typeof(string));
			dataTable.Columns.Add("HrefLang", typeof(string));
			dataTable.Columns.Add("SortRank", typeof(int));

			DataRow row = dataTable.NewRow();
			row["Guid"] = Guid.Empty;
			row["SiteGuid"] = siteSettings.SiteGuid;
			row["ModuleGuid"] = Guid.Empty;
			row["ContentGuid"] = Guid.Empty;
			row["Rel"] = string.Empty;
			row["Href"] = string.Empty;
			row["HrefLang"] = string.Empty;
			row["SortRank"] = 3;

			dataTable.Rows.Add(row);

			grdMetaLinks.Columns[2].Visible = false;
			grdMetaLinks.EditIndex = 0;
			grdMetaLinks.DataSource = dataTable.DefaultView;
			grdMetaLinks.DataBind();
			btnAddMetaLink.Visible = false;

			updMetaLinks.Update();
		}

		void grdMetaLinks_RowDataBound(object sender, GridViewRowEventArgs e)
		{
			GridView grid = (GridView)sender;

			if (grid.EditIndex > -1)
			{
				if (e.Row.RowType == DataControlRowType.DataRow)
				{
					if (!(e.Row.DataItem is ContentMetaLink))
					{
						//the add button was clicked so hide the delete button
						Button btnDeleteMetaLink = (Button)e.Row.Cells[1].FindControl("btnDeleteMetaLink");

						if (btnDeleteMetaLink != null)
						{
							btnDeleteMetaLink.Visible = false;
						}
					}
				}
			}
		}

		void grdMetaLinks_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;
			Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
			metaRepository.DeleteLink(guid);

			pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
			pageSettings.Save();

			grid.Columns[2].Visible = true;
			BindMetaLinks();

			updMetaLinks.Update();
		}

		void grdMetaLinks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
		{
			grdMetaLinks.EditIndex = -1;
			grdMetaLinks.Columns[2].Visible = true;
			BindMetaLinks();
			updMetaLinks.Update();
		}

		void grdMetaLinks_RowUpdating(object sender, GridViewUpdateEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;

			Guid guid = new Guid(grid.DataKeys[e.RowIndex].Value.ToString());
			TextBox txtRel = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtRel");
			TextBox txtHref = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHref");
			TextBox txtHrefLang = (TextBox)grid.Rows[e.RowIndex].Cells[1].FindControl("txtHrefLang");

			ContentMetaLink meta = null;

			if (guid != Guid.Empty)
			{
				meta = metaRepository.FetchLink(guid);
			}
			else
			{
				meta = new ContentMetaLink();

				if (currentUser != null)
				{
					meta.CreatedBy = currentUser.UserGuid;
				}

				meta.SortRank = metaRepository.GetNextLinkSortRank(pageSettings.PageGuid);
			}

			if (meta != null)
			{
				meta.SiteGuid = siteSettings.SiteGuid;
				meta.ContentGuid = pageSettings.PageGuid;
				meta.Rel = txtRel.Text;
				meta.Href = txtHref.Text;
				meta.HrefLang = txtHrefLang.Text;

				if (currentUser != null)
				{
					meta.LastModBy = currentUser.UserGuid;
				}

				metaRepository.Save(meta);

				pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
				pageSettings.Save();
			}

			grid.EditIndex = -1;
			grdMetaLinks.Columns[2].Visible = true;
			BindMetaLinks();
			updMetaLinks.Update();
		}

		void grdMetaLinks_RowEditing(object sender, GridViewEditEventArgs e)
		{
			GridView grid = (GridView)sender;
			grid.EditIndex = e.NewEditIndex;

			BindMetaLinks();

			Guid guid = new Guid(grid.DataKeys[grid.EditIndex].Value.ToString());

			Button btnDelete = (Button)grid.Rows[e.NewEditIndex].Cells[1].FindControl("btnDeleteMetaLink");

			if (btnDelete != null)
			{
				btnDelete.Attributes.Add("OnClick", "return confirm('" + Resource.ContentMetaLinkDeleteWarning + "');");

				if (guid == Guid.Empty)
				{
					btnDelete.Visible = false;
				}
			}

			updMetaLinks.Update();
		}

		void grdMetaLinks_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			if (pageSettings == null)
			{
				return;
			}

			if (pageSettings.PageGuid == Guid.Empty)
			{
				return;
			}

			GridView grid = (GridView)sender;
			string sGuid = e.CommandArgument.ToString();

			if (sGuid.Length != 36)
			{
				return;
			}

			Guid guid = new Guid(sGuid);
			ContentMetaLink meta = metaRepository.FetchLink(guid);

			if (meta == null)
			{
				return;
			}

			switch (e.CommandName)
			{
				case "MoveUp":
					meta.SortRank -= 3;
					break;

				case "MoveDown":
					meta.SortRank += 3;
					break;
			}

			metaRepository.Save(meta);
			List<ContentMetaLink> metaList = metaRepository.FetchLinksByContent(pageSettings.PageGuid);
			metaRepository.ResortMeta(metaList);

			pageSettings.CompiledMeta = metaRepository.GetMetaString(pageSettings.PageGuid);
			pageSettings.Save();

			BindMetaLinks();
			updMetaLinks.Update();
		}

		#endregion


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageSettingsPageTitle);

			heading.Text = Resource.PageSettingsLabel;

			litSettingsTab.Text = Resource.PageSettingsMainSettingsTab;
			litSecurityTab.Text = "<a href='#" + tabSecurity.ClientID + "'>" + Resource.PageSettingsSecurityTab + "</a>";

			litMetaSettingsTab.Text = Resource.PageSettingsMetaDataTab;
			litSEOTab.Text = Resource.PageSettingsSearchEngineOptimizationSettingsLabel;

			lnkPageViewRoles.Text = Resource.PageLayoutViewRolesLabel;
			lnkPageViewRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=v";

			lnkPageEditRoles.Text = Resource.PageLayoutEditRolesLabel;
			lnkPageEditRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=e";

			lnkPageDraftRoles.Text = Resource.PageLayoutDraftEditRolesLabel;
			lnkPageDraftRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=d";

			lnkPageDraftApprovalRoles.Text = Resource.DraftApprovalRoles;
			lnkPageDraftApprovalRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=a";

			lnkChildPageRoles.Text = Resource.PageLayoutCreateChildPageRolesLabel;
			lnkChildPageRoles.NavigateUrl = SiteRoot + "/Admin/PagePermission.aspx?pageid=" + pageId.ToInvariantString() + "&p=ce";

			reqPageName.ErrorMessage = Resource.PageNameRequiredWarning;
			regexUrl.ErrorMessage = Resource.FriendlyUrlRegexWarning;

			if (pageId > -1)
			{
				applyBtn.Text = Resource.PageSettingsSaveButton;
				SiteUtils.SetButtonAccessKey(applyBtn, AccessKeys.PageSettingsSaveButtonAccessKey);

				btnDelete.Text = Resource.PageSettingsDeleteButton;
				SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.PageSettingsDeleteButtonAccessKey);

				if (childPageCount > 0)
				{
					UIHelper.AddConfirmationDialog(btnDelete, Resource.PageWithChildrenDeleteWarning);
				}
				else
				{
					UIHelper.AddConfirmationDialog(btnDelete, Resource.PageSettingsDeleteWarning);
				}

				lnkEditContent.Text = Resource.AddFeaturesToPageLink;
				lnkViewPage.Text = Resource.PageViewPageLink;
			}
			else
			{
				Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageSettingsNewPageTitle);
				applyBtn.Text = Resource.PageSettingsCreateNewPageButton;
				SiteUtils.SetButtonAccessKey(applyBtn, AccessKeys.PageSettingsSaveButtonAccessKey);

				if (!IsPostBack)
				{
					txtPageName.Text = Resource.PageLayoutNewPageLabel;
					chkIncludeInMenu.Checked = true;

					if (autosuggestFriendlyUrls)
					{
						string friendlyUrl = SiteUtils.SuggestFriendlyUrl(txtPageName.Text, siteSettings);
						txtUrl.Text = "~/" + friendlyUrl;
						chkUseUrl.Checked = true;
					}
				}
			}

			tabSSL.Visible = false;

			if (!siteSettings.UseSslOnAllPages)
			{
				if (SiteUtils.SslIsAvailable())
				{
					sslIsAvailable = true;
					tabSSL.Visible = true;
				}
			}

			lnkPageTree.Visible = WebUser.IsAdminOrContentAdmin;
			lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
			lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
			lnkPageTree.NavigateUrl = SiteRoot + WebConfigSettings.PageTreeRelativeUrl;

			btnAddMeta.Text = Resource.AddMetaButton;
			grdContentMeta.Columns[0].HeaderText = string.Empty;
			grdContentMeta.Columns[1].HeaderText = Resource.ContentMetaNameLabel;
			grdContentMeta.Columns[2].HeaderText = Resource.ContentMetaMetaContentLabel;

			btnAddMetaLink.Text = Resource.AddMetaLinkButton;

			grdMetaLinks.Columns[0].HeaderText = string.Empty;
			grdMetaLinks.Columns[1].HeaderText = Resource.ContentMetaRelLabel;
			grdMetaLinks.Columns[2].HeaderText = Resource.ContentMetaMetaHrefLabel;

			regexBodyCss.ErrorMessage = Resource.InvalidBodyCssClass;
			regexMenuCss.ErrorMessage = Resource.InvalidMenuCSSClass;

			rbViewAdminOnly.Text = Resource.AdminsOnly;
			rbViewUseRoles.Text = Resource.RolesAllowed;

			rbEditAdminOnly.Text = Resource.AdminsOnly;
			rbEditUseRoles.Text = Resource.RolesAllowed;

			rbCreateChildAdminOnly.Text = Resource.AdminsOnly;
			rbCreateChildUseRoles.Text = Resource.RolesAllowed;
		}

		private void SetupRoleToggleScript()
		{
			StringBuilder script = new StringBuilder();

			script.Append("\n<script type='text/javascript'>");
			script.Append("function DeSelectRoles(chkBoxContainer) {");
			script.Append("$(chkBoxContainer).find('input[type=checkbox]').each(function(){this.checked = false; }); ");
			script.Append("} ");
			script.Append("$(document).ready(function() {");
			script.Append("$('#" + rbViewAdminOnly.ClientID + "').change(function(){");
			script.Append("var selectedVal = $('#" + rbViewAdminOnly.ClientID + "').attr('checked'); ");
			script.Append("if(selectedVal === 'checked'){");
			script.Append("DeSelectRoles('#" + chkListAuthRoles.ClientID + "');}");
			script.Append("});");

			script.Append("$('#" + rbEditAdminOnly.ClientID + "').change(function(){");
			script.Append("var selectedVal = $('#" + rbEditAdminOnly.ClientID + "').attr('checked'); ");
			script.Append("if(selectedVal === 'checked'){");
			script.Append("DeSelectRoles('#" + chkListEditRoles.ClientID + "');}");
			script.Append("});");

			script.Append("$('#" + rbCreateChildAdminOnly.ClientID + "').change(function(){");
			script.Append("var selectedVal = $('#" + rbCreateChildAdminOnly.ClientID + "').attr('checked'); ");
			script.Append("if(selectedVal === 'checked'){");
			script.Append("DeSelectRoles('#" + chkListCreateChildPageRoles.ClientID + "');}");
			script.Append("});");

			script.Append("}); ");

			script.Append("</script>");

			Page.ClientScript.RegisterStartupScript(typeof(Page), "roletoggle", script.ToString());
		}

		private void SetupParentPageSelectorScript()
		{
			StringBuilder script = new StringBuilder();

			script.Append("\n<script type='text/javascript'>");
			script.Append("function SetPage(pageId, pageName) {");

			script.Append("var hdnUI = document.getElementById('" + hdnParentPageId.ClientID + "'); ");
			script.Append("hdnUI.value = pageId; ");

			script.Append("var lbl = document.getElementById('" + lblParentPageName.ClientID + "');  ");
			script.Append("lbl.innerHTML = pageName; ");

			script.Append("$.colorbox.close(); ");

			script.Append("}");
			script.Append("</script>");

			Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectPrentPageHandler", script.ToString());
		}

		private void LoadSettings()
		{
			isAdmin = WebUser.IsAdmin;
			isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin;
			isSiteEditor = SiteUtils.UserIsSiteEditor();
			useWorkFlow = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
			pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
			startPageId = WebUtils.ParseInt32FromQueryString("start", -1);
			currentUser = SiteUtils.GetCurrentSiteUser();
			divIsClickable.Visible = StyleCombiner.EnableNonClickablePageLinks;
			ScriptConfig.IncludeColorBox = true;
			timeZone = SiteUtils.GetUserTimeZone();
			
			h3DraftRoles.Visible = useWorkFlow;
			divIsPending.Visible = useWorkFlow;
			divDraftRoles.Visible = useWorkFlow;

			if (!useWorkFlow || !WebConfigSettings.Use3LevelContentWorkflow)
			{
				h3DraftApprovalRoles.Visible = false;
				divDraftApprovalRoles.Visible = false;
				liDraftApprovalRoles.Visible = false;
			}

			pnlComments.Visible = (!WebConfigSettings.DisableExternalCommentSystems);
			divUseUrl.Visible = WebConfigSettings.ShowUseUrlSettingInPageSettings;

			divBodyCss.Visible = WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins);
			divMenuCss.Visible = WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins) && StyleCombiner.UseMenuTooltipForCustomCss;

			AddClassToBody("administration");
			AddClassToBody("pagesettings");

			divAdminLinks.Attributes.Add("class", displaySettings.AdminLinksContainerDivCssClass);
			lnkEditContent.CssClass = displaySettings.AdminLinkCssClass;
			lnkViewPage.CssClass = displaySettings.AdminLinkCssClass;
			lnkPageTree.CssClass = displaySettings.AdminLinkCssClass;
			litLinkSpacer1.Text = displaySettings.AdminLinkSeparator;
			litLinkSpacer2.Text = displaySettings.AdminLinkSeparator;

			divMenuDesc.Visible = displaySettings.ShowMenuDescription;

			SkinSetting.Enabled = WebUser.IsInRoles(siteSettings.RolesThatCanAssignSkinsToPages) || isSiteEditor || isAdminOrContentAdmin;

			Control pageTitle = Master.FindControl("PageTitle1");

			if (pageTitle == null)
			{
				pageTitle = Master.FindControl("PageHeading1");

				if (pageTitle == null)
				{
					divPageHeading.Visible = false;
					divShowPageHeading.Visible = false;
				}
			}

			pageCount = PageSettings.GetCount(siteSettings.SiteId, true);

			if (pageId > -1)
			{
				childPageCount = PageSettings.GetCountChildPages(pageId, true);
			}

			if (pageCount > WebConfigSettings.TooManyPagesForDropdownList)
			{
				ddPages.Visible = false;

				lblParentPageName.Visible = true;

				lnkParentPageEdit.Visible = true;
				lnkParentPageEdit.Text = Resource.Browse;
				lnkParentPageEdit.ToolTip = Resource.ChooseParentPage;
				lnkParentPageEdit.NavigateUrl = SiteRoot + "/Dialog/ParentPageDialog.aspx?pageid=" + pageId.ToInvariantString();

				SetupParentPageSelectorScript();
			}

			useSeparatePagesForRoles = (Role.CountOfRoles(siteSettings.SiteId) >= WebConfigSettings.TooManyRolesForPageSettings);

			if (!isAdmin)
			{
				// only admins can lock content down to only admins
				rbViewAdminOnly.Enabled = false;
				rbViewUseRoles.Enabled = false;
				rbEditAdminOnly.Enabled = false;
				rbEditUseRoles.Enabled = false;
				rbCreateChildAdminOnly.Enabled = false;
				rbCreateChildUseRoles.Enabled = false;
			}

			liSecurity.Visible = isAdminOrContentAdmin || isSiteEditor;
			tabSecurity.Visible = isAdminOrContentAdmin || isSiteEditor;

			if (useSeparatePagesForRoles)
			{
				divRoles.Visible = false;
				if (pageId > -1)
				{
					divRoleLinks.Visible = true;
				}
				else
				{
					lblSavePageBeforeSettingPermissions.Visible = true;
				}
			}
			else if (isAdmin)
			{
				SetupRoleToggleScript();
			}
		}


		private void SetupScripts()
		{
			if (
				autosuggestFriendlyUrls &&
				((pageId == -1) || WebConfigSettings.AutoSuggestFriendlyUrlsOnPageNameChanges)
			)
			{
				if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
				{
					Page.ClientScript.RegisterClientScriptBlock(
						GetType(),
						"friendlyurlsuggest", "<script type=\"text/javascript\" src=\"" + ResolveUrl(WebConfigSettings.FriendlyUrlSuggestScript) + "\"></script>"
					);
				}

				string hookupInputScript = "<script type=\"text/javascript\">"
					+ "new UrlHelper( "
					+ "document.getElementById('" + txtPageName.ClientID + "'),  "
					+ "document.getElementById('" + txtUrl.ClientID + "'), "
					+ "document.getElementById('" + hdnPageName.ClientID + "'), "
					+ "document.getElementById('" + spnUrlWarning.ClientID + "'), "
					+ "\"" + SiteRoot + "/Services/FriendlyUrlSuggestXml.aspx" + "\""
					+ ");</script>";

				if (!Page.ClientScript.IsStartupScriptRegistered(UniqueID + "urlscript"))
				{
					Page.ClientScript.RegisterStartupScript(
						GetType(),
						UniqueID + "urlscript", hookupInputScript
					);
				}
			}
		}
	}
}
