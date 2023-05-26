using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.AdminUI
{
	public partial class PageLayout : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(PageLayout));

		private bool canEdit = false;
		private bool isSiteEditor = false;
		private int pageID = -1;
		private bool pageHasAltContent1 = false;
		private bool pageHasAltContent2 = false;
		protected string EditSettingsImage = WebConfigSettings.EditPropertiesImage;
		protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;
		private int globalContentCount = 0;


		#region OnInit

		protected override void OnPreInit(EventArgs e)
		{
			// this page needs to use the same skin as the page in case there are extra content place holders
			//SetMasterInBasePage = false;
			AllowSkinOverride = true;

			base.OnPreInit(e);

			if (siteSettings.AllowPageSkins && (CurrentPage != null) && (CurrentPage.Skin.Length > 0))
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

			//SiteUtils.SetMasterPage(this, siteSettings, true);

			//StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
			//if (styleCombiner != null) { styleCombiner.AllowPageOverride = true; }
		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);

			btnCreateNewContent.Click += new EventHandler(btnCreateNewContent_Click);


			//
			// Altcontent 1
			// ==================================
			btnAlt1MoveUp.ServerClick += new EventHandler(btnAlt1MoveUp_Click);
			btnAlt1MoveDown.ServerClick += new EventHandler(btnAlt1MoveDown_Click);
			btnMoveAlt1ToCenter.ServerClick += new EventHandler(btnMoveAlt1ToCenter_Click);
			btnEditAlt1.ServerClick += new EventHandler(EditBtn_Click);
			btnDeleteAlt1.ServerClick += new EventHandler(DeleteBtn_Click);

			//
			// Left
			// ==================================
			LeftUpBtn.ServerClick += new EventHandler(LeftUpBtn_Click);
			LeftDownBtn.ServerClick += new EventHandler(LeftDownBtn_Click);
			LeftRightBtn.ServerClick += new EventHandler(LeftRightBtn_Click);
			LeftEditBtn.ServerClick += new EventHandler(EditBtn_Click);
			LeftDeleteBtn.ServerClick += new EventHandler(DeleteBtn_Click);

			// Center
			ContentUpBtn.ServerClick += new EventHandler(ContentUpBtn_Click);
			ContentDownBtn.ServerClick += new EventHandler(ContentDownBtn_Click);
			ContentLeftBtn.ServerClick += new EventHandler(ContentLeftBtn_Click);
			ContentRightBtn.ServerClick += new EventHandler(ContentRightBtn_Click);
			ContentDownToNextButton.ServerClick += new EventHandler(ContentDownToNextButton_Click);
			ContentUpToNextButton.ServerClick += new EventHandler(ContentUpToNextButton_Click);
			ContentEditBtn.ServerClick += new EventHandler(EditBtn_Click);
			ContentDeleteBtn.ServerClick += new EventHandler(DeleteBtn_Click);

			// Right
			RightUpBtn.ServerClick += new EventHandler(RightUpBtn_Click);
			RightDownBtn.ServerClick += new EventHandler(RightDownBtn_Click);
			RightLeftBtn.ServerClick += new EventHandler(RightLeftBtn_Click);
			RightEditBtn.ServerClick += new EventHandler(EditBtn_Click);
			RightDeleteBtn.ServerClick += new EventHandler(DeleteBtn_Click);

			// Altcontent 2
			btnAlt2MoveUp.ServerClick += new EventHandler(btnAlt2MoveUp_Click);
			btnAlt2MoveDown.ServerClick += new EventHandler(btnAlt2MoveDown_Click);
			btnMoveAlt2ToCenter.ServerClick += new EventHandler(btnMoveAlt2ToCenter_Click);
			btnEditAlt2.ServerClick += new EventHandler(EditBtn_Click);
			btnDeleteAlt2.ServerClick += new EventHandler(DeleteBtn_Click);


			btnAddExisting.Click += new ImageClickEventHandler(btnAddExisting_Click);

			SuppressPageMenu();
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
			//siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (WebUser.IsAdminOrContentAdmin || isSiteEditor || WebUser.IsInRoles(CurrentPage.EditRoles))
			{
				canEdit = true;
			}

			if (CurrentPage.EditRoles == "Admins;")
			{
				if (!WebUser.IsAdmin)
				{
					canEdit = false;
				}
			}

			if ((!canEdit) || (pageID != CurrentPage.PageId))
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
			SetupExistingContentScript();

			if (!Page.IsPostBack)
			{
				PopulateControls();
			}
		}


		private void PopulateControls()
		{
			//lblPageName.Text = CurrentPage.PageName;
			heading.Text = string.Format(CultureInfo.InvariantCulture, Resource.ContentForPageFormat, CurrentPage.PageName);

			if (pageID > -1)
			{
				BindFeatureList();
				BindPanes();

				UIHelper.AddConfirmationDialog(LeftDeleteBtn, Resource.PageLayoutRemoveContentWarning);
				UIHelper.AddConfirmationDialog(ContentDeleteBtn, Resource.PageLayoutRemoveContentWarning);
				UIHelper.AddConfirmationDialog(RightDeleteBtn, Resource.PageLayoutRemoveContentWarning);
				if (pageHasAltContent1) UIHelper.AddConfirmationDialog(btnDeleteAlt1, Resource.PageLayoutRemoveContentWarning);
				if (pageHasAltContent2) UIHelper.AddConfirmationDialog(btnDeleteAlt2, Resource.PageLayoutRemoveContentWarning);
			}

		}

		private void BindPanes()
		{
			leftPane.Items.Clear();
			contentPane.Items.Clear();
			rightPane.Items.Clear();
			lbAltContent1.Items.Clear();
			lbAltContent2.Items.Clear();

			BindPaneModules(leftPane, "leftPane");
			BindPaneModules(contentPane, "contentPane");
			BindPaneModules(rightPane, "rightPane");

			if (pageHasAltContent1)
			{
				BindPaneModules(lbAltContent1, "altcontent1");
			}
			else
			{
				BindPaneModules(contentPane, "altcontent1");
			}

			if (pageHasAltContent2)
			{
				BindPaneModules(lbAltContent2, "altcontent2");
			}
			else
			{
				BindPaneModules(contentPane, "altcontent2");
			}
		}

		private void BindFeatureList()
		{
			using (IDataReader reader = ModuleDefinition.GetUserModules(siteSettings.SiteId))
			{
				ListItem listItem;
				while (reader.Read())
				{
					string allowedRoles = reader["AuthorizedRoles"].ToString();
					if (WebUser.IsInRoles(allowedRoles))
					{
						listItem = new ListItem(
							ResourceHelper.GetResourceString(
							reader["ResourceFile"].ToString(),
							reader["FeatureName"].ToString()),
							reader["ModuleDefID"].ToString());

						moduleType.Items.Add(listItem);
					}

				}

			}

		}

		private ArrayList GetPaneModules(string pane)
		{
			ArrayList paneModules = new ArrayList();

			foreach (Module module in CurrentPage.Modules)
			{
				if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, pane))
				{
					paneModules.Add(module);
				}

				if (!pageHasAltContent1)
				{
					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
					{
						paneModules.Add(module);
					}

				}

				if (!pageHasAltContent2)
				{
					if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
					{
						paneModules.Add(module);
					}

				}
			}
			
			return paneModules;
		}

		private void BindPaneModules(ListControl listControl, string pane)
		{

			foreach (Module module in CurrentPage.Modules)
			{
				if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, pane))
				{
					var moduleTitle = module.ModuleTitle.Coalesce(Resource.ContentNoTitle);
					var useElementTitle = false;
					if (module.ModuleTitle.Length > 50)
					{
						moduleTitle = moduleTitle.Substring(0, 48) + "...";
						useElementTitle = true;
					}
					ListItem listItem = new(moduleTitle, module.ModuleId.ToInvariantString());
					if (useElementTitle)
					{
						listItem.Attributes.Add("title", module.ModuleTitle);
					}
					listControl.Items.Add(listItem);
				}
			}
		}


		private void OrderModules(ArrayList list)
		{
			int i = 1;
			list.Sort();

			foreach (Module m in list)
			{
				// number the items 1, 3, 5, etc. to provide an empty order
				// number when moving items up and down in the list.
				m.ModuleOrder = i;
				i += 2;
			}
		}

		void btnAddExisting_Click(object sender, ImageClickEventArgs e)
		{
			int moduleId = -1;
			if (int.TryParse(hdnModuleID.Value, out moduleId))
			{
				Module m = new Module(moduleId);
				if (m.SiteId == siteSettings.SiteId)
				{
					Module.Publish(
						CurrentPage.PageGuid,
						m.ModuleGuid,
						m.ModuleId,
						CurrentPage.PageId,
						ddPaneNames.SelectedValue,
						999,
						DateTime.UtcNow,
						DateTime.MinValue);

					globalContentCount = Module.GetGlobalCount(siteSettings.SiteId, -1, pageID);
					// lnkContentLookup.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
					lnkGlobalContent.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);

					CurrentPage.RefreshModules();

					ArrayList modules = GetPaneModules(m.PaneName);
					OrderModules(modules);

					foreach (Module item in modules)
					{
						Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, m.PaneName);
					}

					CurrentPage.RefreshModules();

					if (m.IncludeInSearch)
					{
						mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(CurrentPage);
					}

					BindPanes();
					upLayout.Update();
				}


			}

		}

		private void btnCreateNewContent_Click(Object sender, EventArgs e)
		{
			Page.Validate("pagelayout");
			if (!Page.IsValid) { return; }

			int moduleDefID = int.Parse(moduleType.SelectedItem.Value);
			ModuleDefinition moduleDefinition = new ModuleDefinition(moduleDefID);

			Module m = new Module();
			m.SiteId = siteSettings.SiteId;
			m.SiteGuid = siteSettings.SiteGuid;
			m.ModuleDefId = moduleDefID;
			m.FeatureGuid = moduleDefinition.FeatureGuid;
			m.Icon = moduleDefinition.Icon;
			m.CacheTime = moduleDefinition.DefaultCacheTime;
			m.PageId = pageID;
			m.ModuleTitle = moduleTitle.Text;
			m.PaneName = ddPaneNames.SelectedValue;
			//m.AuthorizedEditRoles = "Admins";
			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			if (currentUser != null)
			{
				m.CreatedByUserId = currentUser.UserId;
			}
			m.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
			m.HeadElement = WebConfigSettings.ModuleTitleTag;
			m.Save();

			CurrentPage.RefreshModules();

			ArrayList modules = GetPaneModules(m.PaneName);
			OrderModules(modules);

			foreach (Module item in modules)
			{
				Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, m.PaneName);
			}

			//WebUtils.SetupRedirect(this, Request.RawUrl);
			//return;

			CurrentPage.RefreshModules();
			BindPanes();
			upLayout.Update();
		}

		#region Move Up or Down


		//
		// Alt Content 1
		// =============================================================

		void btnAlt1MoveUp_Click(object sender, EventArgs e)
		{
			MoveUpDown(lbAltContent1, "altcontent1", "up");
		}

		void btnAlt1MoveDown_Click(object sender, EventArgs e)
		{
			MoveUpDown(lbAltContent1, "altcontent1", "down");
		}


		//
		// Left Content
		// =============================================================

		void LeftUpBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(leftPane, "leftPane", "up");
		}

		void LeftDownBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(leftPane, "leftPane", "down");
		}


		//
		// Center Content
		// =============================================================

		void ContentUpBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(contentPane, "contentPane", "up");
		}

		void ContentDownBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(contentPane, "contentPane", "down");
		}


		//
		// Right Content
		// =============================================================

		void RightUpBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(rightPane, "rightPane", "up");
		}

		void RightDownBtn_Click(object sender, EventArgs e)
		{
			MoveUpDown(rightPane, "rightPane", "down");
		}


		//
		// Alt Content 2
		// =============================================================

		void btnAlt2MoveUp_Click(object sender, EventArgs e)
		{
			MoveUpDown(lbAltContent2, "altcontent2", "up");
		}

		void btnAlt2MoveDown_Click(object sender, EventArgs e)
		{
			MoveUpDown(lbAltContent2, "altcontent2", "down");
		}


		private void MoveUpDown(ListBox listbox, string pane, string direction)
		{

			ArrayList modules = GetPaneModules(pane);
			Module m = null;
			if (listbox.SelectedIndex != -1)
			{
				int delta;
				int selection = -1;

				// Determine the delta to apply in the order number for the module
				// within the list.  +3 moves down one item; -3 moves up one item

				if (direction == "down")
				{
					delta = 3;
					if (listbox.SelectedIndex < listbox.Items.Count - 1)
						selection = listbox.SelectedIndex + 1;
				}
				else
				{
					delta = -3;
					if (listbox.SelectedIndex > 0)
						selection = listbox.SelectedIndex - 1;
				}


				m = (Module)modules[listbox.SelectedIndex];
				m.ModuleOrder += delta;

				OrderModules(modules);

				foreach (Module item in modules)
				{
					Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, pane);
				}
			}

			//WebUtils.SetupRedirect(this, Request.RawUrl);
			CurrentPage.RefreshModules();
			BindPanes();
			if (m != null)
			{
				SelectModule(m, pane);
			}
			upLayout.Update();
		}

		#endregion


		#region Move To Pane

		void LeftRightBtn_Click(object sender, EventArgs e)
		{
			MoveContent(leftPane, "leftPane", "contentPane");
		}

		void ContentLeftBtn_Click(object sender, EventArgs e)
		{
			MoveContent(contentPane, "contentPane", "leftPane");
		}

		void ContentRightBtn_Click(object sender, EventArgs e)
		{
			MoveContent(contentPane, "contentPane", "rightPane");
		}

		void RightLeftBtn_Click(object sender, EventArgs e)
		{
			MoveContent(rightPane, "rightPane", "contentPane");
		}

		void ContentUpToNextButton_Click(object sender, EventArgs e)
		{
			MoveContent(contentPane, "contentPane", "altcontent1");
		}

		void ContentDownToNextButton_Click(object sender, EventArgs e)
		{
			MoveContent(contentPane, "contentPane", "altcontent2");
		}

		void btnMoveAlt1ToCenter_Click(object sender, EventArgs e)
		{
			MoveContent(lbAltContent1, "altcontent1", "contentPane");
		}

		void btnMoveAlt2ToCenter_Click(object sender, EventArgs e)
		{
			MoveContent(lbAltContent2, "altcontent2", "contentPane");
		}

		private void MoveContent(ListBox listBox, string sourcePane, string targetPane)
		{
			if (listBox.SelectedIndex != -1)
			{
				ArrayList sourceList = GetPaneModules(sourcePane);

				Module m = (Module)sourceList[listBox.SelectedIndex];
				Module.UpdateModuleOrder(pageID, m.ModuleId, 998, targetPane);

				CurrentPage.RefreshModules();

				ArrayList modulesSource = GetPaneModules(sourcePane);
				OrderModules(modulesSource);

				foreach (Module item in modulesSource)
				{
					Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, sourcePane);
				}

				ArrayList modulesTarget = GetPaneModules(targetPane);
				OrderModules(modulesTarget);

				foreach (Module item in modulesTarget)
				{
					Module.UpdateModuleOrder(pageID, item.ModuleId, item.ModuleOrder, targetPane);
				}

				BindPanes();
				SelectModule(m, targetPane);
				upLayout.Update();
			}
		}

		#endregion


		private void SelectModule(Module m, string paneName)
		{
			ListBox listbox = null;
			switch (paneName.ToLower())
			{
				case "leftpane":
					listbox = leftPane;
					break;

				case "rightpane":
					listbox = rightPane;
					break;

				case "contentpane":
					listbox = contentPane;
					break;

				case "altcontent1":
					listbox = lbAltContent1;
					break;

				case "altcontent2":
					listbox = lbAltContent2;
					break;

			}

			if (listbox != null)
			{
				ListItem item = listbox.Items.FindByValue(m.ModuleId.ToInvariantString());
				if (item != null)
				{
					listbox.ClearSelection();
					item.Selected = true;
				}

			}

		}


		private void EditBtn_Click(Object sender, EventArgs e)
		{
			string pane = ((HtmlButton)sender).Attributes["data-panel"];
			ListBox _listbox = (ListBox)MPContent.FindControl(pane);

			if (_listbox.SelectedIndex != -1)
			{
				int mid = Int32.Parse(_listbox.SelectedItem.Value, CultureInfo.InvariantCulture);

				WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ModuleSettings.aspx?mid=" + mid + "&pageid=" + pageID);
			}
		}


		private void DeleteBtn_Click(Object sender, EventArgs e)
		{
			if (sender == null) return;

			string pane = ((HtmlButton)sender).Attributes["data-panel"];
			ListBox listbox = (ListBox)this.MPContent.FindControl(pane);

			if (listbox.SelectedIndex != -1)
			{

				int mid = int.Parse(listbox.SelectedItem.Value);

				//Module m = new Module(mid);

				if (WebConfigSettings.LogIpAddressForContentDeletions)
				{
					string userName = string.Empty;
					SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
					if (currentUser != null)
					{
						userName = currentUser.Name;
					}

					log.Info($"user {userName} with ip address {SiteUtils.GetIP4Address()}, removed module {listbox.SelectedItem.Text} from page {CurrentPage.PageName}");
				}

				Module.DeleteModuleInstance(mid, pageID);
				SearchIndex.IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageID));

			}

			globalContentCount = Module.GetGlobalCount(siteSettings.SiteId, -1, pageID);
			//lnkContentLookup.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
			lnkGlobalContent.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
			CurrentPage.RefreshModules();
			BindPanes();
			upLayout.Update();
		}

		protected Collection<DictionaryEntry> PaneList()
		{
			Collection<DictionaryEntry> paneList = new Collection<DictionaryEntry>();
			paneList.Add(new DictionaryEntry(Resource.ContentManagerCenterColumnLabel, "contentpane"));
			paneList.Add(new DictionaryEntry(Resource.ContentManagerLeftColumnLabel, "leftpane"));
			paneList.Add(new DictionaryEntry(Resource.ContentManagerRightColumnLabel, "rightpane"));

			if (pageHasAltContent1)
			{
				paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel1Label, "altcontent1"));

			}

			if (pageHasAltContent2)
			{
				paneList.Add(new DictionaryEntry(Resource.PageLayoutAltPanel2Label, "altcontent2"));

			}

			return paneList;
		}



		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.PageLayoutPageTitle);

			btnCreateNewContent.Text = Resource.ContentManagerCreateNewContentButton;
			btnCreateNewContent.ToolTip = Resource.ContentManagerCreateNewContentButton;

			SiteUtils.SetButtonAccessKey(btnCreateNewContent, AccessKeys.ContentManagerCreateNewContentButtonAccessKey);

			lnkEditSettings.Text = Resource.PageLayoutEditSettingsLink;
			lnkEditSettings.ToolTip = Resource.PageLayoutEditSettingsLink;
			lnkViewPage.Text = Resource.PageViewPageLink;
			lnkViewPage.ToolTip = Resource.PageViewPageLink;


			//
			// Layout Panes
			//

			// Altcontent Notice
			divAltLayoutNotice.Visible = true;

			if (pageHasAltContent1 || pageHasAltContent2)
			{
				divAltLayoutNotice.Visible = true;
			}
			else
			{
				divAltLayoutNotice.Visible = false;
			}

			litEditNotes.Text = string.Format(
				CultureInfo.InvariantCulture,
				Resource.LayoutEditNotesFormat,
				$"<a href=\"{SiteUtils.GetCurrentPageUrl()}\" title=\"{Resource.LayoutViewThePageLink}\">{Resource.LayoutViewThePageLink}</a>"
			);

			litAltLayoutNotice.Text = Resource.PageLayoutAltPanelInfo;

			// Altcontent Panes
			pnlAlt1LayoutPane.Visible = pageHasAltContent1;
			pnlAlt1LayoutPane.CssClass = displaySettings.Alt1PaneCssClass;

			pnlAlt2LayoutPane.Visible = pageHasAltContent2;
			pnlAlt2LayoutPane.CssClass = displaySettings.Alt2PaneCssClass;

			// Regular Panes
			pnlRegularLayoutPanesWrap.CssClass = displaySettings.RegularLayoutPanesWrapCssClass;
			pnlRegularLayoutPaneLeft.CssClass = displaySettings.RegularLayoutPaneLeftCssClass;
			pnlRegularLayoutPaneCenter.CssClass = displaySettings.RegularLayoutPaneCenterCssClass;
			pnlRegularLayoutPaneRight.CssClass = displaySettings.RegularLayoutPaneRightCssClass;

			// Pane List Box
			pnlPaneListBox1.CssClass = displaySettings.PaneListBoxCssClass;
			pnlPaneListBox2.CssClass = displaySettings.PaneListBoxCssClass;
			pnlPaneListBox3.CssClass = displaySettings.PaneListBoxCssClass;
			pnlPaneListBox4.CssClass = displaySettings.PaneListBoxCssClass;
			pnlPaneListBox5.CssClass = displaySettings.PaneListBoxCssClass;


			//
			// Altcontent 1
			//

			// Altcontent 1 | Item Up
			btnAlt1MoveUp.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutUpButtonInnerHtml, Resource.PageLayoutAlt1MoveUpButton)));
			btnAlt1MoveUp.Attributes.Add("class", displaySettings.PageLayoutUpButtonCssClass);
			btnAlt1MoveUp.Attributes.Add("title", Resource.PageLayoutAlt1MoveUpButton);

			// Altcontent 1 | Item Down
			btnAlt1MoveDown.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDownButtonInnerHtml, Resource.PageLayoutAlt1MoveDownButton)));
			btnAlt1MoveDown.Attributes.Add("class", displaySettings.PageLayoutDownButtonCssClass);
			btnAlt1MoveDown.Attributes.Add("title", Resource.PageLayoutAlt1MoveDownButton);

			// Altcontent 1 | Top to Center
			btnMoveAlt1ToCenter.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutAlt1ToCenterButtonInnerHtml, Resource.PageLayoutMoveAltToCenterButton)));
			btnMoveAlt1ToCenter.Attributes.Add("class", displaySettings.PageLayoutAlt1ToCenterButtonCssClass);
			btnMoveAlt1ToCenter.Attributes.Add("title", Resource.PageLayoutMoveAltToCenterButton);

			// Altcontent 1 | Edit
			btnEditAlt1.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutEditButtonInnerHtml, Resource.PageLayoutAlt1EditButton)));
			btnEditAlt1.Attributes.Add("class", displaySettings.PageLayoutEditButtonCssClass);
			btnEditAlt1.Attributes.Add("title", Resource.PageLayoutAlt1EditButton);

			// Altcontent 1 | Delete
			btnDeleteAlt1.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDeleteButtonInnerHtml, Resource.PageLayoutAlt1DeleteButton)));
			btnDeleteAlt1.Attributes.Add("class", displaySettings.PageLayoutDeleteButtonCssClass);
			btnDeleteAlt1.Attributes.Add("title", Resource.PageLayoutAlt1EditButton);

			//
			// Left
			//

			// Left | Item Up
			LeftUpBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutUpButtonInnerHtml, Resource.PageLayoutLeftUpAlternateText)));
			LeftUpBtn.Attributes.Add("class", displaySettings.PageLayoutUpButtonCssClass);
			LeftUpBtn.Attributes.Add("title", Resource.PageLayoutLeftUpAlternateText);

			// Left | Item Down
			LeftDownBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDownButtonInnerHtml, Resource.PageLayoutLeftDownAlternateText)));
			LeftDownBtn.Attributes.Add("class", displaySettings.PageLayoutDownButtonCssClass);
			LeftDownBtn.Attributes.Add("title", Resource.PageLayoutLeftDownAlternateText);

			// Left | Item To Right
			LeftRightBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutLeftToRightButtonInnerHtml, Resource.PageLayoutLeftRightAlternateText)));
			LeftRightBtn.Attributes.Add("class", displaySettings.PageLayoutLeftToRightButtonCssClass);
			LeftRightBtn.Attributes.Add("title", Resource.PageLayoutLeftRightAlternateText);

			// Left | Edit
			LeftEditBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutEditButtonInnerHtml, Resource.PageLayoutLeftEditAlternateText)));
			LeftEditBtn.Attributes.Add("class", displaySettings.PageLayoutEditButtonCssClass);
			LeftEditBtn.Attributes.Add("title", Resource.PageLayoutLeftEditAlternateText);

			// Left | Delete
			LeftDeleteBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDeleteButtonInnerHtml, Resource.PageLayoutLeftDeleteAlternateText)));
			LeftDeleteBtn.Attributes.Add("class", displaySettings.PageLayoutDeleteButtonCssClass);
			LeftDeleteBtn.Attributes.Add("title", Resource.PageLayoutLeftDeleteAlternateText);

			//
			// Center
			//

			// Center | Item Up
			ContentUpBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutUpButtonInnerHtml, Resource.PageLayoutContentUpAlternateText)));
			ContentUpBtn.Attributes.Add("class", displaySettings.PageLayoutUpButtonCssClass);
			ContentUpBtn.Attributes.Add("title", Resource.PageLayoutContentUpAlternateText);

			// Center | Item Down
			ContentDownBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDownButtonInnerHtml, Resource.PageLayoutContentDownAlternateText)));
			ContentDownBtn.Attributes.Add("class", displaySettings.PageLayoutDownButtonCssClass);
			ContentDownBtn.Attributes.Add("title", Resource.PageLayoutContentDownAlternateText);

			// Center | Item Right to Left
			ContentLeftBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutRightToLeftButtonInnerHtml, Resource.PageLayoutContentLeftAlternateText)));
			ContentLeftBtn.Attributes.Add("class", displaySettings.PageLayoutRightToLeftButtonCssClass);
			ContentLeftBtn.Attributes.Add("title", Resource.PageLayoutContentLeftAlternateText);

			// Center | Item Left to Right
			ContentRightBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutLeftToRightButtonInnerHtml, Resource.PageLayoutContentRightAlternateText)));
			ContentRightBtn.Attributes.Add("class", displaySettings.PageLayoutLeftToRightButtonCssClass);
			ContentRightBtn.Attributes.Add("title", Resource.PageLayoutContentRightAlternateText);

			// Center | Item Center to Alt1
			ContentUpToNextButton.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutCenterToAlt1ButtonInnerHtml, Resource.PageLayoutMoveCenterToAlt1Button)));
			ContentUpToNextButton.Attributes.Add("class", displaySettings.PageLayoutCenterToAlt1ButtonCssClass);
			ContentUpToNextButton.Attributes.Add("title", Resource.PageLayoutMoveCenterToAlt1Button);
			ContentUpToNextButton.Visible = pageHasAltContent1;

			// Center | Item Center to Alt2
			ContentDownToNextButton.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutCenterToAlt2ButtonInnerHtml, Resource.PageLayoutMoveCenterToAlt2Button)));
			ContentDownToNextButton.Attributes.Add("class", displaySettings.PageLayoutCenterToAlt2ButtonCssClass);
			ContentDownToNextButton.Attributes.Add("title", Resource.PageLayoutMoveCenterToAlt2Button);
			ContentDownToNextButton.Visible = pageHasAltContent2;

			// Center | Edit
			ContentEditBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutEditButtonInnerHtml, Resource.PageLayoutContentEditAlternateText)));
			ContentEditBtn.Attributes.Add("class", displaySettings.PageLayoutEditButtonCssClass);
			ContentEditBtn.Attributes.Add("title", Resource.PageLayoutContentEditAlternateText);

			// Center | Delete
			ContentDeleteBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDeleteButtonInnerHtml, Resource.PageLayoutContentDeleteAlternateText)));
			ContentDeleteBtn.Attributes.Add("class", displaySettings.PageLayoutDeleteButtonCssClass);
			ContentDeleteBtn.Attributes.Add("title", Resource.PageLayoutContentDeleteAlternateText);

			//
			// Right
			//

			RightUpBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutUpButtonInnerHtml, Resource.PageLayoutRightUpAlternateText)));
			RightUpBtn.Attributes.Add("class", displaySettings.PageLayoutUpButtonCssClass);
			RightUpBtn.Attributes.Add("title", Resource.PageLayoutRightUpAlternateText);

			RightDownBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDownButtonInnerHtml, Resource.PageLayoutRightDownAlternateText)));
			RightDownBtn.Attributes.Add("class", displaySettings.PageLayoutDownButtonCssClass);
			RightDownBtn.Attributes.Add("title", Resource.PageLayoutRightDownAlternateText);

			RightLeftBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutRightToLeftButtonInnerHtml, Resource.PageLayoutRightLeftAlternateText)));
			RightLeftBtn.Attributes.Add("class", displaySettings.PageLayoutRightToLeftButtonCssClass);
			RightLeftBtn.Attributes.Add("title", Resource.PageLayoutRightLeftAlternateText);

			RightEditBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutEditButtonInnerHtml, Resource.PageLayoutRightEditAlternateText)));
			RightEditBtn.Attributes.Add("class", displaySettings.PageLayoutEditButtonCssClass);
			RightEditBtn.Attributes.Add("title", Resource.PageLayoutRightEditAlternateText);

			RightDeleteBtn.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDeleteButtonInnerHtml, Resource.PageLayoutRightDeleteAlternateText)));
			RightDeleteBtn.Attributes.Add("class", displaySettings.PageLayoutDeleteButtonCssClass);
			RightDeleteBtn.Attributes.Add("title", Resource.PageLayoutRightDeleteAlternateText);

			//
			// Altcontent 2
			//

			// Altcontent 2 | Item Up
			btnAlt2MoveUp.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutUpButtonInnerHtml, Resource.PageLayoutAlt2MoveUpButton)));
			btnAlt2MoveUp.Attributes.Add("class", displaySettings.PageLayoutUpButtonCssClass);
			btnAlt2MoveUp.Attributes.Add("title", Resource.PageLayoutAlt2MoveUpButton);

			// Altcontent 2 | Item Down
			btnAlt2MoveDown.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDownButtonInnerHtml, Resource.PageLayoutAlt2MoveDownButton)));
			btnAlt2MoveDown.Attributes.Add("class", displaySettings.PageLayoutDownButtonCssClass);
			btnAlt2MoveDown.Attributes.Add("title", Resource.PageLayoutAlt2MoveDownButton);

			// Altcontent 2 | Top to Center
			btnMoveAlt2ToCenter.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutAlt2ToCenterButtonInnerHtml, Resource.PageLayoutMoveAltToCenterButton)));
			btnMoveAlt2ToCenter.Attributes.Add("class", displaySettings.PageLayoutAlt2ToCenterButtonCssClass);
			btnMoveAlt2ToCenter.Attributes.Add("title", Resource.PageLayoutMoveAltToCenterButton);

			// Altcontent 2 | Edit
			btnEditAlt2.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutEditButtonInnerHtml, Resource.PageLayoutAlt2EditButton)));
			btnEditAlt2.Attributes.Add("class", displaySettings.PageLayoutEditButtonCssClass);
			btnEditAlt2.Attributes.Add("title", Resource.PageLayoutAlt2EditButton);

			// Altcontent 2 | Delete
			btnDeleteAlt2.Controls.Add(new LiteralControl(String.Format(displaySettings.PageLayoutDeleteButtonInnerHtml, Resource.PageLayoutAlt2DeleteButton)));
			btnDeleteAlt2.Attributes.Add("class", displaySettings.PageLayoutDeleteButtonCssClass);
			btnDeleteAlt2.Attributes.Add("title", Resource.PageLayoutAlt2EditButton);


			// Button Groups
			pnlAlt1ItemButtons.CssClass = displaySettings.PageLayoutButtonGroupCssClass;
			pnlLeftItemButtons.CssClass = displaySettings.PageLayoutButtonGroupCssClass;
			pnlCenterItemButtons.CssClass = displaySettings.PageLayoutButtonGroupCssClass;
			pnlRightItemButtons.CssClass = displaySettings.PageLayoutButtonGroupCssClass;
			pnlAlt2ItemButtons.CssClass = displaySettings.PageLayoutButtonGroupCssClass;

			if (!string.IsNullOrEmpty(displaySettings.PageLayoutButtonGroupSeparatorMarkup)) {
				litButtonSeparator1.Text = displaySettings.PageLayoutButtonGroupSeparatorMarkup;
				litButtonSeparator2.Text = displaySettings.PageLayoutButtonGroupSeparatorMarkup;
				litButtonSeparator3.Text = displaySettings.PageLayoutButtonGroupSeparatorMarkup;
				litButtonSeparator4.Text = displaySettings.PageLayoutButtonGroupSeparatorMarkup;
				litButtonSeparator5.Text = displaySettings.PageLayoutButtonGroupSeparatorMarkup;
			}


			if (!Page.IsPostBack)
			{
				if (WebConfigSettings.PrePopulateDefaultContentTitle)
				{
					moduleTitle.Text = Resource.PageLayoutDefaultNewModuleName;
				}
			}

			if (!Page.IsPostBack)
			{
				ddPaneNames.DataSource = PaneList();
				ddPaneNames.DataBind();
			}

			lnkPageTree.Visible = WebUser.IsAdminOrContentAdmin;
			lnkPageTree.Text = Resource.AdminMenuPageTreeLink;
			lnkPageTree.ToolTip = Resource.AdminMenuPageTreeLink;
			lnkPageTree.NavigateUrl = SiteRoot + WebConfigSettings.PageTreeRelativeUrl;

			btnAddExisting.ImageUrl = "~/Data/SiteImages/1x1.gif";
			btnAddExisting.Attributes.Add("tabIndex", "-1");

			lnkGlobalContent.Text = Resource.AddExistingContent;
			lnkGlobalContent.ToolTip = Resource.AddExistingContent;
			lnkGlobalContent.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
			lnkGlobalContent.NavigateUrl = SiteRoot + "/Dialog/GlobalContentDialog.aspx?pageid=" + pageID.ToInvariantString();
			lnkGlobalContent.Attributes.Add("data-modal", "");
			lnkGlobalContent.Attributes.Add("data-size", "fluid-large");
			lnkGlobalContent.Attributes.Add("data-close-text", Resource.CloseDialogButton);
			lnkGlobalContent.Attributes.Add("data-modal-type", "iframe");


			reqModuleTitle.ErrorMessage = Resource.TitleRequiredWarning;
			reqModuleTitle.Enabled = WebConfigSettings.RequireContentTitle;

			cvModuleTitle.ValueToCompare = Resource.PageLayoutDefaultNewModuleName;
			cvModuleTitle.ErrorMessage = Resource.DefaultContentTitleWarning;
			cvModuleTitle.Enabled = WebConfigSettings.RequireChangeDefaultContentTitle;
		}

		private void SetupExistingContentScript()
		{


			StringBuilder script = new();

			script.Append("\n<script data-loader=\"PageLayout\">");

			//script.Append("$('#" + lnkGlobalContent.ClientID + "').colorbox({width:\"80%\", height:\"80%\", iframe:true});");

			script.Append("function AddModule(moduleId) {");
			script.Append("var hdnUI = document.getElementById('" + this.hdnModuleID.ClientID + "'); ");
			script.Append("hdnUI.value = moduleId; ");


			script.Append("var btn = document.getElementById('" + this.btnAddExisting.ClientID + "');  ");
			script.Append("btn.click(); ");

			//script.Append("$.colorbox.close(); ");

			script.Append("}");
			script.Append("</script>");


			ScriptManager.RegisterStartupScript(this, typeof(Page), "SelectContentHandler", script.ToString(), false);

		}

		private void LoadSettings()
		{
			pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
			isSiteEditor = SiteUtils.UserIsSiteEditor();
			pageHasAltContent1 = this.ContainsPlaceHolder("altContent1");
			pageHasAltContent2 = this.ContainsPlaceHolder("altContent2");



			if (pageID > -1)
			{
				pnlContent.Visible = true;

				lnkEditSettings.NavigateUrl = SiteRoot + "/Admin/PageSettings.aspx?pageid=" + pageID.ToString();

				if (CurrentPage != null)
				{
					lnkViewPage.NavigateUrl = SiteUtils.GetCurrentPageUrl();
					if (CurrentPage.BodyCssClass.Length > 0) { AddClassToBody(CurrentPage.BodyCssClass); }
				}
				else
				{
					lnkViewPage.Visible = false;
				}

			}

			AddClassToBody("administration");
			AddClassToBody("pagelayout");

			divAdminLinks.Attributes.Add("class", displaySettings.AdminLinksContainerDivCssClass);
			lnkEditSettings.CssClass = displaySettings.AdminLinkCssClass;
			lnkViewPage.CssClass = displaySettings.AdminLinkCssClass;
			lnkPageTree.CssClass = displaySettings.AdminLinkCssClass;
			litLinkSpacer1.Text = displaySettings.AdminLinkSeparator;
			litLinkSpacer2.Text = displaySettings.AdminLinkSeparator;

			globalContentCount = Module.GetGlobalCount(siteSettings.SiteId, -1, pageID);

			try
			{
				// this keeps the action from changing during ajax postback in folder based sites
				SiteUtils.SetFormAction(Page, Request.RawUrl);
			}
			catch (MissingMethodException)
			{
				//this method was introduced in .NET 3.5 SP1
			}

			if (ScriptController != null)
			{
				ScriptController.RegisterAsyncPostBackControl(btnCreateNewContent);

				ScriptController.RegisterAsyncPostBackControl(LeftUpBtn);
				ScriptController.RegisterAsyncPostBackControl(LeftDownBtn);
				ScriptController.RegisterAsyncPostBackControl(ContentUpBtn);
				ScriptController.RegisterAsyncPostBackControl(ContentDownBtn);
				ScriptController.RegisterAsyncPostBackControl(RightUpBtn);
				ScriptController.RegisterAsyncPostBackControl(RightDownBtn);
				ScriptController.RegisterAsyncPostBackControl(btnAlt1MoveUp);
				ScriptController.RegisterAsyncPostBackControl(btnAlt1MoveDown);
				ScriptController.RegisterAsyncPostBackControl(btnAlt2MoveUp);
				ScriptController.RegisterAsyncPostBackControl(btnAlt2MoveDown);
				ScriptController.RegisterAsyncPostBackControl(LeftEditBtn);

				ScriptController.RegisterAsyncPostBackControl(ContentUpToNextButton);
				ScriptController.RegisterAsyncPostBackControl(ContentDownToNextButton);
				
				ScriptController.RegisterAsyncPostBackControl(LeftDeleteBtn);
				ScriptController.RegisterAsyncPostBackControl(ContentDeleteBtn);
				ScriptController.RegisterAsyncPostBackControl(RightDeleteBtn);
				ScriptController.RegisterAsyncPostBackControl(btnDeleteAlt1);
				ScriptController.RegisterAsyncPostBackControl(btnDeleteAlt2);
				ScriptController.RegisterAsyncPostBackControl(LeftRightBtn);
				ScriptController.RegisterAsyncPostBackControl(ContentLeftBtn);
				ScriptController.RegisterAsyncPostBackControl(ContentRightBtn);
				ScriptController.RegisterAsyncPostBackControl(RightLeftBtn);
				ScriptController.RegisterAsyncPostBackControl(btnMoveAlt1ToCenter);
				ScriptController.RegisterAsyncPostBackControl(btnMoveAlt2ToCenter);
				//ScriptController.RegisterAsyncPostBackControl(btnMoveAlt2ToAlt1);
			}
		}
	}
}

namespace mojoPortal.Web.UI
{
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;

	/// <summary>
	/// this control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
	/// </summary>
	public class PageLayoutDisplaySettings : WebControl
	{
		private string adminLinksContainerDivCssClass = "breadcrumbs pageditlinks";

		public string AdminLinksContainerDivCssClass
		{
			get { return adminLinksContainerDivCssClass; }
			set { adminLinksContainerDivCssClass = value; }
		}

		private string adminLinkSeparator = "&nbsp;";

		public string AdminLinkSeparator
		{
			get { return adminLinkSeparator; }
			set { adminLinkSeparator = value; }
		}

		private string adminLinkCssClass = string.Empty;

		public string AdminLinkCssClass
		{
			get { return adminLinkCssClass; }
			set { adminLinkCssClass = value; }
		}

		private bool showMenuDescription = false;
		/// <summary>
		/// most skins don't use menu description
		/// only skins that use MegaMenu actually use it so by default it is not shown in pagesettings.aspx
		/// set this to true if you want to show this field and populate it for use in the menu
		/// </summary>
		public bool ShowMenuDescription
		{
			get { return showMenuDescription; }
			set { showMenuDescription = value; }
		}

		private string pageLayoutUpButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--up btn btn-sm btn-default";
		public string PageLayoutUpButtonCssClass
		{
			get { return pageLayoutUpButtonCssClass; }
			set { pageLayoutUpButtonCssClass = value; }
		}

		private string pageLayoutDownButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--down btn btn-sm btn-default";
		public string PageLayoutDownButtonCssClass
		{
			get { return pageLayoutDownButtonCssClass; }
			set { pageLayoutDownButtonCssClass = value; }
		}

		private string pageLayoutAlt1ToCenterButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--alt1-center btn btn-sm btn-default";
		public string PageLayoutAlt1ToCenterButtonCssClass
		{
			get { return pageLayoutAlt1ToCenterButtonCssClass; }
			set { pageLayoutAlt1ToCenterButtonCssClass = value; }
		}

		private string pageLayoutAlt2ToCenterButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--alt2-center btn btn-sm btn-default";
		public string PageLayoutAlt2ToCenterButtonCssClass
		{
			get { return pageLayoutAlt2ToCenterButtonCssClass; }
			set { pageLayoutAlt2ToCenterButtonCssClass = value; }
		}

		private string pageLayoutCenterToAlt1ButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--center-alt1 btn btn-sm btn-default";
		public string PageLayoutCenterToAlt1ButtonCssClass
		{
			get { return pageLayoutCenterToAlt1ButtonCssClass; }
			set { pageLayoutCenterToAlt1ButtonCssClass = value; }
		}

		private string pageLayoutCenterToAlt2ButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--center-alt2 btn btn-sm btn-default";
		public string PageLayoutCenterToAlt2ButtonCssClass
		{
			get { return pageLayoutCenterToAlt2ButtonCssClass; }
			set { pageLayoutCenterToAlt2ButtonCssClass = value; }
		}

		private string pageLayoutLeftToRightButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--left-right btn btn-sm btn-default";
		public string PageLayoutLeftToRightButtonCssClass
		{
			get { return pageLayoutLeftToRightButtonCssClass; }
			set { pageLayoutLeftToRightButtonCssClass = value; }
		}

		private string pageLayoutRightToLeftButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--right-left btn btn-sm btn-default";
		public string PageLayoutRightToLeftButtonCssClass
		{
			get { return pageLayoutRightToLeftButtonCssClass; }
			set { pageLayoutRightToLeftButtonCssClass = value; }
		}

		private string pageLayoutEditButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--edit btn btn-sm btn-default";
		public string PageLayoutEditButtonCssClass
		{
			get { return pageLayoutEditButtonCssClass; }
			set { pageLayoutEditButtonCssClass = value; }
		}

		private string pageLayoutDeleteButtonCssClass = "pagelayout__item-btn pagelayout__item-btn--delete btn btn-sm btn-default";
		public string PageLayoutDeleteButtonCssClass
		{
			get { return pageLayoutDeleteButtonCssClass; }
			set { pageLayoutDeleteButtonCssClass = value; }
		}

		private string pageLayoutUpButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-up'></use></svg>";
		public string PageLayoutUpButtonInnerHtml
		{
			get { return pageLayoutUpButtonInnerHtml; }
			set { pageLayoutUpButtonInnerHtml = value; }
		}

		private string pageLayoutDownButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-down'></use></svg>";
		public string PageLayoutDownButtonInnerHtml
		{
			get { return pageLayoutDownButtonInnerHtml; }
			set { pageLayoutDownButtonInnerHtml = value; }
		}

		private string pageLayoutAlt1ToCenterButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-down'></use></svg>";
		public string PageLayoutAlt1ToCenterButtonInnerHtml
		{
			get { return pageLayoutAlt1ToCenterButtonInnerHtml; }
			set { pageLayoutAlt1ToCenterButtonInnerHtml = value; }
		}

		private string pageLayoutAlt2ToCenterButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-up'></use></svg>";
		public string PageLayoutAlt2ToCenterButtonInnerHtml
		{
			get { return pageLayoutAlt2ToCenterButtonInnerHtml; }
			set { pageLayoutAlt2ToCenterButtonInnerHtml = value; }
		}

		private string pageLayoutCenterToAlt1ButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-up'></use></svg>";
		public string PageLayoutCenterToAlt1ButtonInnerHtml
		{
			get { return pageLayoutCenterToAlt1ButtonInnerHtml; }
			set { pageLayoutCenterToAlt1ButtonInnerHtml = value; }
		}

		private string pageLayoutCenterToAlt2ButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-down'></use></svg>";
		public string PageLayoutCenterToAlt2ButtonInnerHtml
		{
			get { return pageLayoutCenterToAlt2ButtonInnerHtml; }
			set { pageLayoutCenterToAlt2ButtonInnerHtml = value; }
		}

		private string pageLayoutLeftToRightButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-right'></use></svg>";
		public string PageLayoutLeftToRightButtonInnerHtml
		{
			get { return pageLayoutLeftToRightButtonInnerHtml; }
			set { pageLayoutLeftToRightButtonInnerHtml = value; }
		}

		private string pageLayoutRightToLeftButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-angle-double-left'></use></svg>";
		public string PageLayoutRightToLeftButtonInnerHtml
		{
			get { return pageLayoutRightToLeftButtonInnerHtml; }
			set { pageLayoutRightToLeftButtonInnerHtml = value; }
		}

		private string pageLayoutEditButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-cog'></use></svg>";
		public string PageLayoutEditButtonInnerHtml
		{
			get { return pageLayoutEditButtonInnerHtml; }
			set { pageLayoutEditButtonInnerHtml = value; }
		}

		private string pageLayoutDeleteButtonInnerHtml = "<svg class='mp-svg-icon'><use href='#mp-trash-alt'></use></svg>";
		public string PageLayoutDeleteButtonInnerHtml
		{
			get { return pageLayoutDeleteButtonInnerHtml; }
			set { pageLayoutDeleteButtonInnerHtml = value; }
		}

		private string alt1PaneCssClass = "pane layoutalt1";

		public string Alt1PaneCssClass
		{
			get { return alt1PaneCssClass; }
			set { alt1PaneCssClass = value; }
		}

		private string regularLayoutPanesWrapCssClass = "regularpanes";

		public string RegularLayoutPanesWrapCssClass
		{
			get { return regularLayoutPanesWrapCssClass; }
			set { regularLayoutPanesWrapCssClass = value; }
		}

		private string regularLayoutPaneLeftCssClass = "pane layoutleft";

		public string RegularLayoutPaneLeftCssClass
		{
			get { return regularLayoutPaneLeftCssClass; }
			set { regularLayoutPaneLeftCssClass = value; }
		}

		private string regularLayoutPaneCenterCssClass = "pane layoutcenter";

		public string RegularLayoutPaneCenterCssClass
		{
			get { return regularLayoutPaneCenterCssClass; }
			set { regularLayoutPaneCenterCssClass = value; }
		}

		private string regularLayoutPaneRightCssClass = "pane layoutright";

		public string RegularLayoutPaneRightCssClass
		{
			get { return regularLayoutPaneRightCssClass; }
			set { regularLayoutPaneRightCssClass = value; }
		}

		private string alt2PaneCssClass = "pane layoutalt2";

		public string Alt2PaneCssClass
		{
			get { return alt2PaneCssClass; }
			set { alt2PaneCssClass = value; }
		}

		private string paneListBoxCssClass = "panelistbox";

		public string PaneListBoxCssClass
		{
			get { return paneListBoxCssClass; }
			set { paneListBoxCssClass = value; }
		}

		private string pageLayoutButtonGroupCssClass = "pagelayout-item-btns btn-group-vertical";

		public string PageLayoutButtonGroupCssClass
		{
			get => pageLayoutButtonGroupCssClass;
			set => pageLayoutButtonGroupCssClass = value;
		}

		private string pageLayoutButtonGroupSeparatorMarkup = "";

		public string PageLayoutButtonGroupSeparatorMarkup
		{
			get => pageLayoutButtonGroupSeparatorMarkup;
			set => pageLayoutButtonGroupSeparatorMarkup = value;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			// nothing to render
		}
	}
}
