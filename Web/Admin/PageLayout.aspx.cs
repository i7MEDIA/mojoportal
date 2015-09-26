/// Author:                     Joe Audette
/// Created:                    2004-08-22
///	Last Modified:              2013-02-18
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.SearchIndex;
using Resources;

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

            if (
                    (siteSettings.AllowPageSkins)
                    && (CurrentPage != null)
                    && (CurrentPage.Skin.Length > 0)
                    )
            {

                if (Global.RegisteredVirtualThemes)
                {
                    this.Theme = "pageskin-" + siteSettings.SiteId.ToInvariantString() + CurrentPage.Skin; 
                }
                else
                {
                    this.Theme = "pageskin";
                }
            }

            //SiteUtils.SetMasterPage(this, siteSettings, true);

            //StyleSheetCombiner styleCombiner = (StyleSheetCombiner)Master.FindControl("StyleSheetCombiner");
            //if (styleCombiner != null) { styleCombiner.AllowPageOverride = true; }

            
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            this.btnCreateNewContent.Click += new EventHandler(this.btnCreateNewContent_Click);

            this.LeftUpBtn.Click += new ImageClickEventHandler(LeftUpBtn_Click);
            this.LeftDownBtn.Click += new ImageClickEventHandler(LeftDownBtn_Click);
            this.ContentUpBtn.Click += new ImageClickEventHandler(ContentUpBtn_Click);
            this.ContentDownBtn.Click += new ImageClickEventHandler(ContentDownBtn_Click);
            this.RightUpBtn.Click += new ImageClickEventHandler(RightUpBtn_Click);
            this.RightDownBtn.Click += new ImageClickEventHandler(RightDownBtn_Click);

            this.btnAlt1MoveUp.Click += new ImageClickEventHandler(btnAlt1MoveUp_Click);
            this.btnAlt1MoveDown.Click += new ImageClickEventHandler(btnAlt1MoveDown_Click);
            this.btnAlt2MoveUp.Click += new ImageClickEventHandler(btnAlt2MoveUp_Click);
            this.btnAlt2MoveDown.Click += new ImageClickEventHandler(btnAlt2MoveDown_Click);
            
            
            this.LeftEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.ContentEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.RightEditBtn.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.btnEditAlt1.Click += new ImageClickEventHandler(this.EditBtn_Click);
            this.btnEditAlt2.Click += new ImageClickEventHandler(this.EditBtn_Click);
            
            this.LeftDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.ContentDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.RightDeleteBtn.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.btnDeleteAlt1.Click += new ImageClickEventHandler(this.DeleteBtn_Click);
            this.btnDeleteAlt2.Click += new ImageClickEventHandler(this.DeleteBtn_Click);

           
            this.LeftRightBtn.Click += new ImageClickEventHandler(LeftRightBtn_Click);
            this.ContentLeftBtn.Click += new ImageClickEventHandler(ContentLeftBtn_Click);
            this.ContentRightBtn.Click += new ImageClickEventHandler(ContentRightBtn_Click);
            this.RightLeftBtn.Click += new ImageClickEventHandler(RightLeftBtn_Click);

            this.btnMoveAlt1ToCenter.Click += new ImageClickEventHandler(btnMoveAlt1ToCenter_Click);
            this.btnMoveAlt2ToCenter.Click += new ImageClickEventHandler(btnMoveAlt2ToCenter_Click);
            //this.btnMoveAlt1ToAlt2.Click += new ImageClickEventHandler(btnMoveAlt1ToAlt2_Click);
            //this.btnMoveAlt2ToAlt1.Click += new ImageClickEventHandler(btnMoveAlt2ToAlt1_Click);
            
            
            this.ContentDownToNextButton.Click += new ImageClickEventHandler(ContentDownToNextButton_Click);
            this.ContentUpToNextButton.Click += new ImageClickEventHandler(ContentUpToNextButton_Click);

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

			if(pageID > -1)
			{
                BindFeatureList();
                BindPanes();
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
                    ListItem listItem = new ListItem(module.ModuleTitle.Coalesce(Resource.ContentNoTitle), module.ModuleId.ToInvariantString());
                    listControl.Items.Add(listItem);
                    
                }
            }

        }


		private void OrderModules (ArrayList list) 
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


        void LeftUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(leftPane, pane, direction);
            
        }

        void LeftDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(leftPane, pane, direction);

        }

        void ContentUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(contentPane, pane, direction);

        }

        void ContentDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(contentPane, pane, direction);

        }

        void RightUpBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(rightPane, pane, direction);

        }

        void RightDownBtn_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(rightPane, pane, direction);

        }

        void btnAlt1MoveUp_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent1, pane, direction);
        }

        void btnAlt1MoveDown_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent1, pane, direction);

        }

        void btnAlt2MoveUp_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent2, pane, direction);
        }

        void btnAlt2MoveDown_Click(object sender, ImageClickEventArgs e)
        {
            string direction = ((ImageButton)sender).CommandName;
            string pane = ((ImageButton)sender).CommandArgument;
            MoveUpDown(lbAltContent2, pane, direction);

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

        
        void LeftRightBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "leftPane";
            string targetPane = "contentPane";
            MoveContent(leftPane, sourcePane, targetPane);

        }

        void ContentLeftBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "leftPane";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void ContentRightBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "rightPane";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void RightLeftBtn_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "rightPane";
            string targetPane = "contentPane";
            MoveContent(rightPane, sourcePane, targetPane);

        }

        void ContentDownToNextButton_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "altcontent2";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void ContentUpToNextButton_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "contentPane";
            string targetPane = "altcontent1";
            MoveContent(contentPane, sourcePane, targetPane);

        }

        void btnMoveAlt1ToCenter_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "altcontent1";
            string targetPane = "contentPane";
            MoveContent(lbAltContent1, sourcePane, targetPane);

        }

        void btnMoveAlt2ToCenter_Click(object sender, ImageClickEventArgs e)
        {
            string sourcePane = "altcontent2";
            string targetPane = "contentPane";
            MoveContent(lbAltContent2, sourcePane, targetPane);

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


		private void EditBtn_Click(Object sender, ImageClickEventArgs e)
		{
			string pane = ((ImageButton)sender).CommandArgument;
            ListBox _listbox = (ListBox)this.MPContent.FindControl(pane);

			if (_listbox.SelectedIndex != -1) 
			{
				int mid = Int32.Parse(_listbox.SelectedItem.Value,CultureInfo.InvariantCulture);
            
				WebUtils.SetupRedirect(this, SiteRoot + "/Admin/ModuleSettings.aspx?mid=" + mid + "&pageid=" + pageID);
			}
		}

        
		private void DeleteBtn_Click(Object sender, ImageClickEventArgs e) 
		{
            if (sender == null) return;

			string pane = ((ImageButton)sender).CommandArgument;
			ListBox listbox = (ListBox) this.MPContent.FindControl(pane);
			
			if (listbox.SelectedIndex != -1) 
			{
                
                int mid = Int32.Parse(listbox.SelectedItem.Value);

                Module m = new Module(mid);

                if (WebConfigSettings.LogIpAddressForContentDeletions)
                {
                    string userName = string.Empty;
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        userName = currentUser.Name;
                    }

                    log.Info("user " + userName + " removed module " + m.ModuleTitle + " from page " + CurrentPage.PageName + " from ip address " + SiteUtils.GetIP4Address());

                }

                Module.DeleteModuleInstance(mid, pageID);
                mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageID));

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
            
            SiteUtils.SetButtonAccessKey
                (btnCreateNewContent, AccessKeys.ContentManagerCreateNewContentButtonAccessKey);

            lnkEditSettings.Text = Resource.PageLayoutEditSettingsLink;
            lnkEditSettings.ToolTip = Resource.PageLayoutEditSettingsLink;
            lnkViewPage.Text = Resource.PageViewPageLink;
            lnkViewPage.ToolTip = Resource.PageViewPageLink;

            LeftUpBtn.AlternateText = Resource.PageLayoutLeftUpAlternateText;
            LeftUpBtn.ToolTip = Resource.PageLayoutLeftUpAlternateText;

            LeftRightBtn.AlternateText = Resource.PageLayoutLeftRightAlternateText;
            LeftRightBtn.ToolTip = Resource.PageLayoutLeftRightAlternateText;

            LeftDownBtn.AlternateText = Resource.PageLayoutLeftDownAlternateText;
            LeftDownBtn.ToolTip = Resource.PageLayoutLeftDownAlternateText;

            LeftEditBtn.AlternateText = Resource.PageLayoutLeftEditAlternateText;
            LeftEditBtn.ToolTip = Resource.PageLayoutLeftEditAlternateText;
            LeftEditBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditSettingsImage;

            LeftDeleteBtn.AlternateText = Resource.PageLayoutLeftDeleteAlternateText;
            LeftDeleteBtn.ToolTip = Resource.PageLayoutLeftDeleteAlternateText;
            LeftDeleteBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(LeftDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            ContentUpBtn.AlternateText = Resource.PageLayoutContentUpAlternateText;
            ContentUpBtn.ToolTip = Resource.PageLayoutContentUpAlternateText;

            ContentLeftBtn.AlternateText = Resource.PageLayoutContentLeftAlternateText;
            ContentLeftBtn.ToolTip = Resource.PageLayoutContentLeftAlternateText;

            ContentRightBtn.AlternateText = Resource.PageLayoutContentRightAlternateText;
            ContentRightBtn.ToolTip = Resource.PageLayoutContentRightAlternateText;

            ContentDownBtn.AlternateText = Resource.PageLayoutContentDownAlternateText;
            ContentDownBtn.ToolTip = Resource.PageLayoutContentDownAlternateText;

            ContentEditBtn.AlternateText = Resource.PageLayoutContentEditAlternateText;
            ContentEditBtn.ToolTip = Resource.PageLayoutContentEditAlternateText;
            ContentEditBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditSettingsImage;

            ContentDeleteBtn.AlternateText = Resource.PageLayoutContentDeleteAlternateText;
            ContentDeleteBtn.ToolTip = Resource.PageLayoutContentDeleteAlternateText;
            ContentDeleteBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(ContentDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            RightUpBtn.AlternateText = Resource.PageLayoutRightUpAlternateText;
            RightUpBtn.ToolTip = Resource.PageLayoutRightUpAlternateText;

            RightLeftBtn.AlternateText = Resource.PageLayoutRightLeftAlternateText;
            RightLeftBtn.ToolTip = Resource.PageLayoutRightLeftAlternateText;

            RightDownBtn.AlternateText = Resource.PageLayoutRightDownAlternateText;
            RightDownBtn.ToolTip = Resource.PageLayoutRightDownAlternateText;

            RightEditBtn.AlternateText = Resource.PageLayoutRightEditAlternateText;
            RightEditBtn.ToolTip = Resource.PageLayoutRightEditAlternateText;
            RightEditBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditSettingsImage;

            RightDeleteBtn.AlternateText = Resource.PageLayoutRightDeleteAlternateText;
            RightDeleteBtn.ToolTip = Resource.PageLayoutRightDeleteAlternateText;
            RightDeleteBtn.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(RightDeleteBtn, Resource.PageLayoutRemoveContentWarning);

            litEditNotes.Text = string.Format(CultureInfo.InvariantCulture,
                Resource.LayoutEditNotesFormat, "<a href='" + SiteUtils.GetCurrentPageUrl() + "' title='" + Resource.LayoutViewThePageLink + "'>"
                + Resource.LayoutViewThePageLink + "</a>");

            litAltLayoutNotice.Text = Resource.PageLayoutAltPanelInfo;

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

            ContentDownToNextButton.AlternateText = Resource.PageLayoutMoveCenterToAlt2Button;
            ContentDownToNextButton.ToolTip = Resource.PageLayoutMoveCenterToAlt2Button;
            ContentUpToNextButton.AlternateText = Resource.PageLayoutMoveCenterToAlt1Button;
            ContentUpToNextButton.ToolTip = Resource.PageLayoutMoveCenterToAlt1Button;

            btnMoveAlt1ToCenter.AlternateText = Resource.PageLayoutMoveAltToCenterButton;
            btnMoveAlt1ToCenter.ToolTip = Resource.PageLayoutMoveAltToCenterButton;

            btnAlt2MoveUp.AlternateText = Resource.PageLayoutAlt2MoveUpButton;
            btnAlt2MoveUp.ToolTip = Resource.PageLayoutAlt2MoveUpButton;

            btnAlt1MoveUp.AlternateText = Resource.PageLayoutAlt1MoveUpButton;
            btnAlt1MoveUp.ToolTip = Resource.PageLayoutAlt1MoveUpButton;

            btnAlt1MoveDown.AlternateText = Resource.PageLayoutAlt1MoveDownButton;
            btnAlt1MoveDown.ToolTip = Resource.PageLayoutAlt1MoveDownButton;

            //btnMoveAlt1ToAlt2.AlternateText = Resource.PageLayoutMoveAlt1ToAlt2Button;
            //btnMoveAlt1ToAlt2.ToolTip = Resource.PageLayoutMoveAlt1ToAlt2Button;

            btnEditAlt1.AlternateText = Resource.PageLayoutAlt1EditButton;
            btnEditAlt1.ToolTip = Resource.PageLayoutAlt1EditButton;
            btnEditAlt1.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditSettingsImage;

            btnDeleteAlt1.AlternateText = Resource.PageLayoutAlt1DeleteButton;
            btnDeleteAlt1.ToolTip = Resource.PageLayoutAlt1DeleteButton;
            btnDeleteAlt1.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;

            //btnMoveAlt2ToAlt1.AlternateText = Resource.PageLayoutMoveAlt2ToAlt1Button;
            //btnMoveAlt2ToAlt1.ToolTip = Resource.PageLayoutMoveAlt2ToAlt1Button;

            btnMoveAlt2ToCenter.AlternateText = Resource.PageLayoutMoveAltToCenterButton;
            btnMoveAlt2ToCenter.ToolTip = Resource.PageLayoutMoveAltToCenterButton;

            btnAlt2MoveUp.AlternateText = Resource.PageLayoutAlt2MoveUpButton;
            btnAlt2MoveUp.ToolTip = Resource.PageLayoutAlt2MoveUpButton;

            btnAlt2MoveDown.AlternateText = Resource.PageLayoutAlt2MoveDownButton;
            btnAlt2MoveDown.ToolTip = Resource.PageLayoutAlt2MoveDownButton;

            btnEditAlt2.AlternateText = Resource.PageLayoutAlt2EditButton;
            btnEditAlt2.ToolTip = Resource.PageLayoutAlt2EditButton;
            btnEditAlt2.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditSettingsImage;

            btnDeleteAlt2.AlternateText = Resource.PageLayoutAlt2DeleteButton;
            btnDeleteAlt2.ToolTip = Resource.PageLayoutAlt2DeleteButton;
            btnDeleteAlt2.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;



            this.divAltLayoutNotice.Visible = true;
            this.divAltPanel1.Visible = pageHasAltContent1;
            this.divAltPanel2.Visible = pageHasAltContent2;
            ContentUpToNextButton.Visible = pageHasAltContent1;
            ContentDownToNextButton.Visible = pageHasAltContent2;

            if (pageHasAltContent1 || pageHasAltContent2)
            {
                divAltLayoutNotice.Visible = true;
            }
            else
            {
                divAltLayoutNotice.Visible = false;
            }

            
                
            //lnkContentLookup.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
            //lnkContentLookup.Text = Resource.AddExistingContent;
            //lnkContentLookup.ToolTip = Resource.AddExistingContent;
            //lnkContentLookup.DialogCloseText = Resource.CloseDialogButton;
            //lnkContentLookup.NavigateUrl = SiteRoot + "/Dialog/GlobalContentDialog.aspx?pageid=" + pageID.ToInvariantString();
            btnAddExisting.ImageUrl = "~/Data/SiteImages/1x1.gif";
            btnAddExisting.Attributes.Add("tabIndex", "-1");

            lnkGlobalContent.Text = Resource.AddExistingContent;
            lnkGlobalContent.ToolTip = Resource.AddExistingContent;
            lnkGlobalContent.Visible = ((globalContentCount > 0) && !WebConfigSettings.DisableGlobalContent);
            lnkGlobalContent.NavigateUrl = SiteRoot + "/Dialog/GlobalContentDialog.aspx?pageid=" + pageID.ToInvariantString();

            reqModuleTitle.ErrorMessage = Resource.TitleRequiredWarning;
            reqModuleTitle.Enabled = WebConfigSettings.RequireContentTitle;

            cvModuleTitle.ValueToCompare = Resource.PageLayoutDefaultNewModuleName;
            cvModuleTitle.ErrorMessage = Resource.DefaultContentTitleWarning;
            cvModuleTitle.Enabled = WebConfigSettings.RequireChangeDefaultContentTitle;

        }

        private void SetupExistingContentScript()
        {
           

            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("$('#" + lnkGlobalContent.ClientID + "').colorbox({width:\"80%\", height:\"80%\", iframe:true});");

            script.Append("function AddModule(moduleId) {");

            //script.Append("GB_hide();");
            //script.Append("alert(moduleId);");

            script.Append("var hdnUI = document.getElementById('" + this.hdnModuleID.ClientID + "'); ");
            script.Append("hdnUI.value = moduleId; ");


            script.Append("var btn = document.getElementById('" + this.btnAddExisting.ClientID + "');  ");
            script.Append("btn.click(); ");

            script.Append("$.colorbox.close(); ");

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
    using System;
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
