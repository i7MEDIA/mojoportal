/// Author:                     Joe Audette
///	Last Modified:              2013-10-011
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
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.UI;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;
using Resources;

namespace mojoPortal.Web.AdminUI 
{
    public partial class ModuleSettingsPage : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(ModuleSettingsPage));
        private static bool debugLog = log.IsDebugEnabled;

		private Module module;
        private ArrayList DefaultSettings;
        private bool canEdit = false;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private string iconPath;
        //private String cacheDependencyKey;
        private int pageId = -1;
        private int moduleId = -1;
        private SiteMapDataSource siteMapDataSource;
        private string skinBaseUrl = string.Empty;
        private bool useSeparatePagesForRoles = false;
        private bool use3LevelWorkFlow = false;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            //when not accessing settings from content manager use the page specific skin
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            if (pageId > -1)
            {
                AllowSkinOverride = true;
            }

            base.OnPreInit(e);

            if (pageId > -1)
            {
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

            }
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            btnSave.Click += new EventHandler(btnSave_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            
            SuppressMenuSelection();
            SuppressPageMenu();

            //ScriptConfig.IncludeYuiTabs = true;
            //IncludeYuiTabsCss = true;

            LoadSettings();
            PopulateCustomSettings();
        }

        

       
        #endregion

        private void Page_Load(object sender, EventArgs e) 
		{
            SecurityHelper.DisableBrowserCache();

			divEditUser.Visible = false;

			lblValidationSummary.Text = string.Empty;


            if (!canEdit)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (module == null)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

			PopulateLabels();
            SetupIconScript();
            

			if (!Page.IsPostBack)
			{
                PopulateRoleList();
				PopulateControls();
			}
		}

		


		private void PopulateControls() 
		{
			if (module.ModuleId > -1)
			{
                ModuleDefinition moduleDefinition = new ModuleDefinition(module.ModuleDefId);
                lblFeatureName.Text 
                    = ResourceHelper.GetResourceString(
                    moduleDefinition.ResourceFile, 
                    moduleDefinition.FeatureName);

                litFeatureSpecificSettingsTab.Text = string.Format(CultureInfo.InvariantCulture, Resource.FeatureSettingsTabFormat, lblFeatureName.Text);

                divCacheTimeout.Visible = (!WebConfigSettings.DisableContentCache && moduleDefinition.IsCacheable);

                divIsGlobal.Visible = (moduleDefinition.SupportsPageReuse && (!WebConfigSettings.DisableGlobalContent));
                
                PopulatePageList();
                lblModuleId.Text = module.ModuleId.ToInvariantString();
				moduleTitle.Text = module.ModuleTitle;
				cacheTime.Text = module.CacheTime.ToString();
				chkShowTitle.Checked = module.ShowTitle;
                txtTitleElement.Text = module.HeadElement;
                publishType.SetValue(module.PublishMode.ToInvariantString());
                chkIncludeInSearch.Checked = module.IncludeInSearch;
                chkHideFromAuth.Checked = module.HideFromAuthenticated;
                chkIsGlobal.Checked = module.IsGlobal;
                chkHideFromUnauth.Checked = module.HideFromUnauthenticated;
                chkAvailableForMyPage.Checked = module.AvailableForMyPage;
                chkAllowMultipleInstancesOnMyPage.Checked = module.AllowMultipleInstancesOnMyPage;
				if(isAdmin || isContentAdmin || isSiteEditor)
				{
					divEditUser.Visible = true;

					if(module.EditUserId > 0)
					{
						SiteUser siteUser = new SiteUser(this.siteSettings ,module.EditUserId);
						acUser.Text = siteUser.Name;
						//scUser.Value = siteUser.UserId.ToString();
                        txtEditUserId.Text = siteUser.UserId.ToInvariantString();

					}
				}

                if (divParentPage.Visible)
                {
                    ListItem listItem = ddPages.Items.FindByValue(module.PageId.ToString());
                    if (listItem != null)
                    {
                        ddPages.ClearSelection();
                        listItem.Selected = true;
                    }

                   
                }

               
                if (module.Icon.Length > 0)
                {
                    ddIcons.SelectedValue = module.Icon;
                    imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/" + module.Icon;
                        
                }
                else
                {
                    imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/blank.gif";
                }

                if (!useSeparatePagesForRoles)
                {

                    if (module.ViewRoles == "Admins;")
                    {
                        rbViewAdminOnly.Checked = true;
                        rbViewUseRoles.Checked = false;
                    }
                    else
                    {
                        rbViewAdminOnly.Checked = false;
                        rbViewUseRoles.Checked = true;
                        foreach (ListItem item in cblViewRoles.Items)
                        {
                            if ((module.ViewRoles.LastIndexOf(item.Value + ";")) > -1)
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    if (module.AuthorizedEditRoles == "Admins;")
                    {
                        rbEditAdminsOnly.Checked = true;
                        rbEditUseRoles.Checked = false;
                    }
                    else
                    {
                        rbEditAdminsOnly.Checked = false;
                        rbEditUseRoles.Checked = true;

                        foreach (ListItem item in authEditRoles.Items)
                        {
                            if ((module.AuthorizedEditRoles.LastIndexOf(item.Value + ";")) > -1)
                            {
                                item.Selected = true;
                            }
                        }
                    }

                    //if (module.DraftEditRoles == "Admins;")
                    //{
                    //    rbDraftAdminsOnly.Checked = true;
                    //    rbDraftUseRoles.Checked = false;
                    //}
                    //else
                    //{
                    //    rbDraftAdminsOnly.Checked = false;
                    //    rbDraftUseRoles.Checked = true;

                    foreach (ListItem item in draftEditRoles.Items)
                    {
                        if ((module.DraftEditRoles.LastIndexOf(item.Value + ";")) > -1)
                        {
                            item.Selected = true;
                        }
                    }

                    if (use3LevelWorkFlow)
                    {
                        //joe davis
                        foreach (ListItem item in draftApprovalRoles.Items)
                        {
                            if ((module.DraftApprovalRoles.LastIndexOf(item.Value + ";")) > -1)
                            {
                                item.Selected = true;
                            }
                        }
                    }
                    //}
                }

                cblViewRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
                authEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
				

				
			}
		}

        private void PopulateRoleList()
        {
            liSecurity.Visible = isAdmin || isContentAdmin || isSiteEditor;
            tabSecurity.Visible = isAdmin || isContentAdmin || isSiteEditor;

            if (useSeparatePagesForRoles) { return; }

            authEditRoles.Items.Clear();
            cblViewRoles.Items.Clear();

            ListItem vAllItem = new ListItem();
            vAllItem.Text = Resource.RolesAllUsersRole;
            vAllItem.Value = "All Users";

            ListItem allItem = new ListItem();
            allItem.Text = Resource.RolesAllUsersRole;
            allItem.Value = "All Users";

            //if (this.module.AuthorizedEditRoles.LastIndexOf("All Users") > -1)
            //{
            //    allItem.Selected = true;
            //}

            cblViewRoles.Items.Add(vAllItem);
            //authEditRoles.Items.Add(allItem);

            using (IDataReader roles = Role.GetSiteRoles(siteSettings.SiteId))
            {
                while (roles.Read())
                {
                    string roleName = roles["RoleName"].ToString();

                    // no need or benefit to checking content admins role
                    // since they are not limited by roles except the special case of Admins only role
                    if (roleName == Role.ContentAdministratorsRole) { continue; }
                    // administrators role doesn't need permission, the only reason to show it is so that
                    // an admin can lock the content down to only admins
                    if (roleName == Role.AdministratorsRole)  { continue; }

                    ListItem vItem = new ListItem();
                    vItem.Text = roles["DisplayName"].ToString();
                    vItem.Value = roles["RoleName"].ToString();
                    cblViewRoles.Items.Add(vItem);

                    ListItem item = new ListItem();
                    item.Text = roles["DisplayName"].ToString();
                    item.Value = roles["RoleName"].ToString();
                    authEditRoles.Items.Add(item);

                    ListItem draftItem = new ListItem();
                    draftItem.Text = roles["DisplayName"].ToString();
                    draftItem.Value = roles["RoleName"].ToString();
                    draftEditRoles.Items.Add(draftItem);

                    if (use3LevelWorkFlow)
                    {
                        //joe davis
                        ListItem draftApprovalItem = new ListItem();
                        draftApprovalItem.Text = roles["DisplayName"].ToString();
                        draftApprovalItem.Value = roles["RoleName"].ToString();
                        draftApprovalRoles.Items.Add(draftApprovalItem);
                    }

                }
            }

            
            cblViewRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
            authEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
            draftEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
            draftApprovalRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
           
            divMyPage.Visible = (WebConfigSettings.MyPageIsInstalled && (isAdmin || isContentAdmin || isSiteEditor) && siteSettings.EnableMyPageFeature);
            divMyPageMulti.Visible = divMyPage.Visible;
            

        }

        

        private void PopulateCustomSettings()
        {
            if (module == null) { return; }
            
            // these are the Settings attached to the Module Definition
            DefaultSettings = ModuleSettings.GetDefaultSettings(this.module.ModuleDefId);
            bool groupsExist = false;
            foreach (CustomModuleSetting s in DefaultSettings)
            {
                if (s.GroupName.Length > 0)
                {
                    groupsExist = true;
                    break;
                }
            }

            if (groupsExist) { pnlcustomSettings.CssClass = "mojo-accordion-nh"; }

            // these are the settings attached to the module instance
            ArrayList customSettingValues = ModuleSettings.GetCustomSettingValues(this.module.ModuleId);

            Panel groupWrapper = new Panel();
            string currentGroup = string.Empty;
            int loopCount = 0;

            foreach (CustomModuleSetting s in DefaultSettings)
            {
                if ((s.GroupName != currentGroup)||(loopCount == 0))
                {
                    currentGroup = s.GroupName;
                    if (groupsExist)
                    {
                        string localizedGroup = ResourceHelper.GetResourceString(s.ResourceFile, s.GroupName);
                        Literal groupHeader = new Literal();
                        groupHeader.Text = "<h3><a href=\"#\">" + localizedGroup + "</a></h3>";
                        pnlcustomSettings.Controls.Add(groupHeader);
                    }
                    groupWrapper = new Panel();
                    pnlcustomSettings.Controls.Add(groupWrapper);

                }

                loopCount += 1;

                bool found = false;
                foreach (CustomModuleSetting v in customSettingValues)
                {
                    if (v.SettingName == s.SettingName)
                    {
                        found = true;
                        AddSettingControl(v, groupWrapper);

                    }
                }

                if (!found)
                {
                    // if a new module setting has been added since the
                    // last version upgrade, the code might reach this
                    // it means a Module Definition Setting was found for which there is not
                    // a Module Setting on this instance of the module, so we need to add the setting

                    ModuleSettings.CreateModuleSetting(
                        module.ModuleGuid,
                        moduleId,
                        s.SettingName,
                        s.SettingValue,
                        s.SettingControlType,
                        s.SettingValidationRegex,
                        s.ControlSrc,
                        s.HelpKey,
                        s.SortOrder);

                    // add control with default settings
                    AddSettingControl(s, groupWrapper);
                }

            }
        }

        private void AddSettingControl(CustomModuleSetting s, Panel groupPanel)
        {
            if (s.SettingName == "WebPartModuleWebPartSetting")
            {
                // Special handling for this one
                divWebParts.Visible = true;
                using (IDataReader reader = WebPartContent.SelectBySite(siteSettings.SiteId))
                {
                    this.ddWebParts.DataSource = reader;
                    this.ddWebParts.DataBind();
                }
                if (s.SettingValue.Length == 36)
                {
                    ListItem listItem = ddWebParts.Items.FindByValue(s.SettingValue);
                    if (listItem != null)
                    {
                        ddWebParts.ClearSelection();
                        listItem.Selected = true;

                    }

                }

            }
            else
            {
                if (s.SettingControlType == string.Empty) { return; }

                String settingLabel = s.SettingName;
                String resourceFile = "Resource";
                if (s.ResourceFile.Length > 0)
                {
                    resourceFile = s.ResourceFile;
                }

                try
                {
                    settingLabel = GetGlobalResourceObject(resourceFile, s.SettingName).ToString();
                    if (settingLabel == null) { settingLabel = s.SettingName; }
                }
                catch (NullReferenceException ex)
                {
                    log.Error("ModuleSettings.aspx.cs handled error getting resource for s.SettingName " + s.SettingName, ex);   
                }

                Panel panel = new Panel();
                panel.CssClass = "settingrow " + s.SettingName;
                Literal label = new Literal();
                label.Text = "<label class='settinglabel' >" + settingLabel + "</label>";
                panel.Controls.Add(label);

                if ((s.SettingControlType == "TextBox") || (s.SettingControlType == string.Empty))
                {
                    Literal textBox = new Literal();
                    textBox.Text = "<input name=\""
                        + s.SettingName + moduleId.ToInvariantString()
                        + "\" type='text' class=\"forminput\" value=\"" + s.SettingValue.HtmlEscapeQuotes()
                        + "\" size=\"45\" id=\"" + s.SettingName + moduleId.ToInvariantString() + "\" />";

                    panel.Controls.Add(textBox);

                }

                if (s.SettingControlType == "CheckBox")
                {
                    Literal checkBox = new Literal();
                    String isChecked = String.Empty;

                    if (string.Equals(s.SettingValue, "true", StringComparison.InvariantCultureIgnoreCase))
                    {
                        isChecked = "checked";
                    }

                    checkBox.Text = "<input id='"
                        + s.SettingName + this.moduleId.ToInvariantString()
                        + "' type='checkbox' class='forminput' " + isChecked
                        + " name='" + s.SettingName + moduleId.ToInvariantString() + "' />";

                    panel.Controls.Add(checkBox);

                }

                if (s.SettingControlType == "ISettingControl")
                {
                    if (s.ControlSrc.Length > 0)
                    {
                        if(s.ControlSrc.EndsWith(".ascx"))
                        {
                            Control uc = Page.LoadControl(s.ControlSrc);
                            if (uc is ISettingControl)
                            {

                                ISettingControl sc = uc as ISettingControl;
                                if (!IsPostBack)
                                    sc.SetValue(s.SettingValue);

                                uc.ID = "uc" + moduleId.ToInvariantString() + s.SettingName;
                                panel.Controls.Add(uc);
                            }

                        }
                        else
                        {
                            try
                            {
                                
                                Control c = Activator.CreateInstance(Type.GetType(s.ControlSrc)) as Control;
                                if (c != null)
                                {
                                    if (c is ISettingControl)
                                    {
                                        ISettingControl sc = c as ISettingControl;
                                        
                                        c.ID = "uc" + moduleId.ToInvariantString() + s.SettingName;
                                        panel.Controls.Add(c);

                                        if (!IsPostBack)
                                        {
                                            sc.SetValue(s.SettingValue);
                                        }
                                            
                                    }
                                    else
                                    {
                                        log.Error("setting control " + s.ControlSrc + " does not implement ISettingControl");
                                    }
                                }

                               
                            }
                            catch(Exception ex)
                            {
                                log.Error(ex);
                            }
                            


                        }
                        

                    }
                    else
                    {
                        log.Error("could not add setting control for ISettingControl, missing controlsrc for " + s.SettingName);
                    }
                }

                if (s.HelpKey.Length > 0)
                {
                    mojoHelpLink.AddHelpLink(panel, s.HelpKey);
                }

                //this.PlaceHolderAdvancedSettings.Controls.Add(panel);
                //pnlcustomSettings.Controls.Add(panel);
                groupPanel.Controls.Add(panel);
            }

        }


        private void PopulatePageList()
        {
            if (!divParentPage.Visible) { return; }

            siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "mojosite" + siteSettings.SiteId.ToInvariantString();

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;

            PopulateListControl(ddPages, siteMapNode, string.Empty);

        }

        private void PopulateListControl(
            ListControl listBox,
            SiteMapNode siteMapNode,
            string pagePrefix)
        {
            mojoSiteMapNode mojoNode = (mojoSiteMapNode)siteMapNode;

            if (!mojoNode.IsRootNode)
            {
                if (
                    (isAdmin || isContentAdmin || isSiteEditor)
                    || (WebUser.IsInRoles(mojoNode.EditRoles))
                    || (mojoNode.PageId == module.PageId)
                    )
                {
                    if (mojoNode.ParentId > -1) pagePrefix += "-";
                    ListItem listItem = new ListItem();
                    listItem.Text = pagePrefix + Server.HtmlDecode(mojoNode.Title);
                    listItem.Value = mojoNode.PageId.ToInvariantString();

                    listBox.Items.Add(listItem);
                }
            }


            foreach (SiteMapNode childNode in mojoNode.ChildNodes)
            {
                //recurse to populate children
                PopulateListControl(listBox, childNode, pagePrefix);

            }


        }


		

        private void btnSave_Click(Object sender, EventArgs e) 
		{
            if (debugLog) log.Debug("ModuleSettingsPage about to call Page.Validate()");

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            string userName = string.Empty;
            if (currentUser != null)
            {
                userName = currentUser.Name;
            }

			Page.Validate("ModuleSettings");
			if(Page.IsValid)
			{
                if (debugLog) log.Debug("ModuleSettingsPage about to call Page IsValid = true");

				bool ok = true;
                bool allSetingsAreValid = true;
				bool needToReIndex = false;
				int currentPageId = module.PageId;
				int newPageId = module.PageId;

				if (module.ModuleId > -1)
				{
					if(isAdmin || isContentAdmin || isSiteEditor)
					{
                        if (!useSeparatePagesForRoles)
                        {
                            string viewRoles = string.Empty;
                            if (rbViewAdminOnly.Checked)
                            {
                                viewRoles = "Admins;";

                                log.Info("user " + userName + " changed Module view roles for " + module.ModuleTitle
                                   + " to Admins "
                                   + " from ip address " + SiteUtils.GetIP4Address());
                            }
                            else
                            {
                                foreach (ListItem item in cblViewRoles.Items)
                                {
                                    if (debugLog) log.Debug("ModuleSettingsPage inside loop of Role ListItems");
                                    if (item.Selected == true)
                                    {
                                        viewRoles = viewRoles + item.Value + ";";
                                    }
                                }
                            }
                            string previousViewRoles = module.ViewRoles;
                            module.ViewRoles = viewRoles;
                            if (previousViewRoles != viewRoles) 
                            { 
                                needToReIndex = true;

                                log.Info("user " + userName + " changed Module view roles for " + module.ModuleTitle
                                    + " to " + viewRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());
                            }

                            string editRoles = string.Empty;
                            if (debugLog) log.Debug("ModuleSettingsPage about to loop through Role ListItems");
                            if (rbEditAdminsOnly.Checked)
                            {
                                editRoles = "Admins;";
                                log.Info("user " + userName + " changed Module Edit roles for " + module.ModuleTitle
                                   + " to Admins "
                                   + " from ip address " + SiteUtils.GetIP4Address());
                            }
                            else
                            {
                                foreach (ListItem item in authEditRoles.Items)
                                {
                                    if (debugLog) log.Debug("ModuleSettingsPage inside loop of Role ListItems");
                                    if (item.Selected == true)
                                    {
                                        editRoles = editRoles + item.Value + ";";
                                    }
                                }
                            }

                            module.AuthorizedEditRoles = editRoles;
                            log.Info("user " + userName + " changed Module Edit roles for " + module.ModuleTitle
                                    + " to " + editRoles
                                    + " from ip address " + SiteUtils.GetIP4Address());

                            string draftEdits = string.Empty;
                            //if (rbDraftAdminsOnly.Checked)
                            //{
                            //    draftEdits = "Admins;";
                            //}
                            //else
                            //{
                            foreach (ListItem item in draftEditRoles.Items)
                            {

                                if (item.Selected == true)
                                {
                                    draftEdits = draftEdits + item.Value + ";";
                                }
                            }
                            //}

                            module.DraftEditRoles = draftEdits;

                            log.Info("user " + userName + " changed Module Draft Edit roles for " + module.ModuleTitle
                                    + " to " + draftEdits
                                    + " from ip address " + SiteUtils.GetIP4Address());

                            if (use3LevelWorkFlow)
                            {
                                //joe davis
                                string draftApprovers = string.Empty;
                                foreach (ListItem item in draftApprovalRoles.Items)
                                {
                                    if (item.Selected == true)
                                    {
                                        draftApprovers = draftApprovers + item.Value + ";";
                                    }
                                }

                                module.DraftApprovalRoles = draftApprovers;


                                log.Info("user " + userName + " changed Module Draft Approval roles for " + module.ModuleTitle
                                        + " to " + draftApprovers
                                        + " from ip address " + SiteUtils.GetIP4Address());
                            }
                        }
					}

                    if (tabGeneralSettings.Visible)
                    {

                        module.ModuleTitle = moduleTitle.Text;
                        module.CacheTime = int.Parse(cacheTime.Text);
                        if (divTitleElement.Visible)
                        {
                            module.HeadElement = txtTitleElement.Text;
                        }
                        module.ShowTitle = chkShowTitle.Checked;
                        module.PublishMode = Convert.ToInt32(publishType.GetValue(),CultureInfo.InvariantCulture);
                        module.AvailableForMyPage = chkAvailableForMyPage.Checked;
                        module.AllowMultipleInstancesOnMyPage = chkAllowMultipleInstancesOnMyPage.Checked;
                        module.Icon = ddIcons.SelectedValue;
                        module.HideFromAuthenticated = chkHideFromAuth.Checked;
                        module.HideFromUnauthenticated = chkHideFromUnauth.Checked;
                        module.IncludeInSearch = chkIncludeInSearch.Checked;
                        module.IsGlobal = chkIsGlobal.Checked;

                        if (divParentPage.Visible)
                        {
                            if (debugLog) log.Debug("ModuleSettingsPage about to check Page dropdown");
                            newPageId = int.Parse(ddPages.SelectedValue);
                            if (newPageId != currentPageId)
                            {
                                needToReIndex = true;
                                Module.UpdatePage(currentPageId, newPageId, module.ModuleId);
                            }
                            module.PageId = newPageId;
                        }

                        if (isAdmin)
                        {
                            if (debugLog) log.Debug("ModuleSettingsPage about to check user dropdown");
                            if (txtEditUserId.Text.Length > 0)
                            {
                                try
                                {
                                    module.EditUserId = int.Parse(txtEditUserId.Text);
                                }
                                catch (ArgumentException) { }
                                catch (FormatException) { }
                                catch (OverflowException) { }
                            }
                            else
                            {
                                module.EditUserId = 0;
                            }

                        }

                    }

                    if (debugLog) log.Debug("ModuleSettingsPage about to Save Module");
					module.Save();
                    

                    if (needToReIndex)
                    {
                        // if content is moved from 1 page to another, need to reindex both pages
                        // to keep view permissions in sync

                        mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(CurrentPage);

                        PageSettings newPage = new PageSettings(siteSettings.SiteId, newPageId);
                        newPage.PageIndex = 0;
                        mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(newPage);
                    }

				    ArrayList defaultSettings = ModuleSettings.GetDefaultSettings(module.ModuleDefId);
                    
					foreach(CustomModuleSetting s in defaultSettings)
					{
                        if (s.SettingControlType == string.Empty) { continue; }
                        ok = true;

                        //Object oSettingLabel = GetGlobalResourceObject("Resource", s.SettingName + "RegexWarning");

                        Object oSettingLabel = null;

                        try
                        {
                            oSettingLabel = GetGlobalResourceObject(s.ResourceFile, s.SettingName + "RegexWarning");
                        }
                        catch (NullReferenceException){ }
                        catch (System.Resources.MissingManifestResourceException) { }
                        
                        string settingLabel = String.Empty;
                        if (oSettingLabel == null)
                        {
                            settingLabel = "Regex Warning";
                        }
                        else
                        {
                            settingLabel = oSettingLabel.ToString();
                        }

                        string settingValue = string.Empty;

                        if (s.SettingName == "WebPartModuleWebPartSetting")
                        {
                            ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, s.SettingName, ddWebParts.SelectedValue);
                        }
                        else
                        {
                            if (s.SettingControlType == "ISettingControl")
                            {
                                string controlID = "uc" + moduleId.ToInvariantString() + s.SettingName;
                                //Control c = PlaceHolderAdvancedSettings.FindControl(controlID);
                                Control c = pnlcustomSettings.FindControl(controlID);
                                if (c != null)
                                {
                                    if (c is ISettingControl)
                                    {
                                        ISettingControl isc = c as ISettingControl;
                                        settingValue = isc.GetValue();
                                    }
                                    else
                                    {
                                        ok = false;
                                    }

                                }
                                else
                                {
                                    log.Error("could not find control for " + s.SettingName);
                                    ok = false;
                                }

                            }
                            else
                            {

                                settingValue = Request.Params.Get(s.SettingName + moduleId.ToInvariantString());

                                if (s.SettingControlType == "CheckBox")
                                {
                                    if (settingValue == "on")
                                    {
                                        settingValue = "true";
                                    }
                                    else
                                    {
                                        settingValue = "false";
                                    }
                                }
                                else
                                {
                                    if (s.SettingValidationRegex.Length > 0)
                                    {
                                        if (!Regex.IsMatch(settingValue, s.SettingValidationRegex))
                                        {
                                            ok = false;
                                            allSetingsAreValid = false;
                                            lblValidationSummary.Text += "<br />"
                                                + settingLabel;

                                        }
                                    }
                                }
                            }

                            if (ok)
                            {
                                ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, s.SettingName, settingValue);
                            }
                        }
					}
				}

                if (allSetingsAreValid)
				{
                    //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    CacheHelper.ClearModuleCache(module.ModuleId);
                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
					return;

				}
				
			}
		}

        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (module.ModuleId > -1)
            {
                DataTable pageIds = Module.GetPageModulesTable(module.ModuleId);

                foreach (DataRow row in pageIds.Rows)
                {
                    int pageId = Convert.ToInt32(row["PageID"]);
                    Module.DeleteModuleInstance(module.ModuleId, pageId);
                    mojoPortal.SearchIndex.IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageId));
                }

                ModuleDefinition feature = new ModuleDefinition(module.ModuleDefId);

                if (feature.DeleteProvider.Length > 0)
                {
                    try
                    {
                        ContentDeleteHandlerProvider contentDeleter = ContentDeleteHandlerProviderManager.Providers[feature.DeleteProvider];
                        if (contentDeleter != null)
                        {
                            contentDeleter.DeleteContent(module.ModuleId, module.ModuleGuid);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Failed to invoke content delete handler " + feature.DeleteProvider, ex);
                    }
                }

                if (WebConfigSettings.LogIpAddressForContentDeletions)
                {
                    string userName = string.Empty;
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        userName = currentUser.Name;
                    }

                    log.Info("user " + userName + " deleted module " + module.ModuleTitle + " from ip address " + SiteUtils.GetIP4Address());

                }

                Module.DeleteModule(module.ModuleId);
                //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                CacheHelper.ClearModuleCache(module.ModuleId);

            }

            
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        

        private void PopulatePageArray(ArrayList sitePages)
        {
            SiteMapDataSource siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");

            siteMapDataSource.SiteMapProvider
                    = "mojosite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

            SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
            mojoSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);

           
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ModuleSettingsPageTitle);
            litFeatureSpecificSettingsTab.Text = Resource.ModuleSettingsSettingsTab;
            //litGeneralSettingsTab.Text = Resource.ModuleSettingsGeneralTab;

            heading.Text = Resource.ModuleSettingsSettingsLabel;

            btnSave.Text = Resource.ModuleSettingsApplyButton;
            SiteUtils.SetButtonAccessKey(btnSave, AccessKeys.ModuleSettingsApplyButtonAccessKey);

            btnDelete.Text = Resource.ModuleSettingsDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.ModuleSettingsDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialog(btnDelete, Resource.ModuleSettingsDeleteConfirm);

            lnkCancel.Text = Resource.ModuleSettingsCancelButton;

            if (!Page.IsPostBack)
            {
                FileInfo[] fileInfo = SiteUtils.GetFeatureIconList();
                this.ddIcons.DataSource = fileInfo;
                this.ddIcons.DataBind();

                ddIcons.Items.Insert(0, new ListItem(Resource.ModuleSettingsNoIconLabel, "blank.gif"));
                ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
                ddIcons.Attributes.Add("size", "6");
            }

            //scUser.ValueLabelText = Resource.ModuleSettingsEditUserIDLabel;
            //acUser.DataUrl = SiteRoot + "/Services/UserDropDownXml.aspx";
            acUser.DataUrl = SiteRoot + "/Services/UserLookup.asmx/AutoComplete";
            //scUser.ButtonImageUrl = ImageSiteRoot + "/Data/SiteImages/DownArrow.gif";

            reqCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRequiredMessage;
            regexCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRegexWarning;

            //lnkGeneralSettingsTab.HRef = "#" + tabGeneralSettings.ClientID;
            //lnkSecurityTab.HRef = "#" + tabSecurity.ClientID;
           // lnkSecurity.NavigateUrl = "#" + tabSecurity.ClientID;
            //lnkSecurity.Text = Resource.ModuleSettingsSecurityTab;

            litGeneralSettingsTabLink.Text = "<a href='#" + tabGeneralSettings.ClientID + "'>" + Resource.ModuleSettingsGeneralTab + "</a>";

            litSecurityLink.Text = "<a href='#" + tabSecurity.ClientID + "'>" + Resource.ModuleSettingsSecurityTab + "</a>";

            //litRoleInstructions.Text = Resource.ModuleSettingsRoleHelp;
            rbViewAdminOnly.Text = Resource.AdminsOnly;
            rbViewUseRoles.Text = Resource.RolesAllowed;

            rbEditAdminsOnly.Text = Resource.AdminsOnly;
            rbEditUseRoles.Text = Resource.RolesAllowed;

            //rbDraftAdminsOnly.Text = Resource.AdminsOnly;
            //rbDraftUseRoles.Text = Resource.RolesAllowed;

            lnkPageViewRoles.Text = Resource.ModuleSettingsViewRolesLabel;
            lnkPageViewRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString() + "&p=v";

            lnkPageEditRoles.Text = Resource.ModuleSettingsEditRolesLabel;
            lnkPageEditRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString() + "&p=e";

            lnkPageDraftRoles.Text = Resource.ModuleSettingsDraftEditRolesLabel;
            lnkPageDraftRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString() + "&p=d";
			//joe davis
            lnkPageApprovalRoles.Text = Resource.DraftApprovalRoles;
            lnkPageApprovalRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString() + "&p=a";

            AddClassToBody("administration");
            AddClassToBody("featuresettings");

            acUser.TargetValueElementClientId = txtEditUserId.ClientID;
            

            
#if MONO
            divMyPage.Visible = false;
            divMyPageMulti.Visible = false;
#endif


        }


	    private void SetupIconScript()
        {
            string logoScript = "<script type=\"text/javascript\">"
                + "function showIcon(listBox) { if(!document.images) return; "
                + "var iconPath = '" + iconPath + "'; "
                + "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
                + "}</script>";

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "showIcon", logoScript);

        }

        private void LoadSettings()
        {
            
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            //cacheDependencyKey = "Module-" + moduleId.ToString();
            iconPath = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/";
            skinBaseUrl = SiteUtils.GetSkinBaseUrl(this);
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            acUser.BlockTargetFocus = !WebConfigSettings.AllowDirectEntryOfUserIdForEditPermission;

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            //if ((WebUser.IsAdmin) || (isSiteEditor)) { isAdmin = true; }

            if (isAdmin || isContentAdmin || (isSiteEditor))
            {
                
                canEdit = true;
                
                lnkEditContent.Visible = true;
                lnkEditContent.Text = Resource.ContentManagerViewEditContentLabel;
                lnkEditContent.NavigateUrl = SiteRoot
                    + "/Admin/ContentManagerPreview.aspx?mid=" + moduleId.ToInvariantString();

                lnkPublishing.Visible = true;
                lnkPublishing.Text = Resource.ContentManagerPublishingContentLink;
                lnkPublishing.NavigateUrl = SiteRoot
                    + WebConfigSettings.ContentPublishPageRelativeUrl + "?mid=" + moduleId.ToInvariantString();

            }
            else
            {
                if (WebConfigSettings.HideModuleSettingsDeleteButtonFromNonAdmins) { btnDelete.Visible = false; }

                bool hideOtherTabs = WebConfigSettings.HideModuleSettingsGeneralAndSecurityTabsFromNonAdmins;
                if (hideOtherTabs)
                {
                    liGeneralSettings.Visible = false;
                    liSecurity.Visible = false;
                    tabGeneralSettings.Visible = false;
                    tabSecurity.Visible = false;
                }

            }

            use3LevelWorkFlow = WebConfigSettings.EnableContentWorkflow && WebConfigSettings.Use3LevelContentWorkflow && siteSettings.EnableContentWorkflow;

            divCacheTimeout.Visible = !WebConfigSettings.DisableContentCache;
            h2DraftEditRoles.Visible = (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow);
            divDraftEditRoles.Visible = h2DraftEditRoles.Visible;
			//joe davis
            h2DraftApprovalRoles.Visible = use3LevelWorkFlow;
            divDraftApprovalRoles.Visible = use3LevelWorkFlow;
            liApproverRoles.Visible = use3LevelWorkFlow;

            if (pageId > -1)
            {
                int pageCount = PageSettings.GetCount(siteSettings.SiteId, true);
                if (pageCount < WebConfigSettings.TooManyPagesForDropdownList)
                {
                    divParentPage.Visible = true;
                }
                module = new Module(moduleId, pageId);
            }
            else
            {
                module = new Module(moduleId);
            }

            if (module.ModuleId == -1) { module = null; return; } //module doesn't exist

            if (module.SiteId != siteSettings.SiteId) { module = null; return; }

            if (!canEdit)
            {
                if (
                    (WebUser.IsInRoles(module.AuthorizedEditRoles))
                    || ((WebUser.IsInRoles(module.DraftEditRoles))&&(!module.IsGlobal))
                    || ((WebUser.IsInRoles(CurrentPage.EditRoles)) && (!module.IsGlobal))
                    || ((WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) && (!module.IsGlobal))
                    )
                {
                    canEdit = true;
                }
            }

            if ((isContentAdmin || isSiteEditor) && (module.AuthorizedEditRoles == "Admins;")) 
            { 
                canEdit = false;
               
            }
            //if (WebUser.IsAdmin) { canEdit = true; isAdmin = true; } //needed bcause IsContentAdmin resolves to true when user is Admin

            if (!canEdit)
            {
                if (module.EditUserId > 0)
                {
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

                    if ((siteUser != null) && (module.EditUserId == siteUser.UserId))
                    {
                        canEdit = true;
                    }
                }
            }

            if (module.SiteGuid != siteSettings.SiteGuid)
            {
                canEdit = false;
            }

            if (canEdit &&(!isAdmin) &&(WebUser.IsInRoles(siteSettings.RolesNotAllowedToEditModuleSettings)))
            { canEdit = false; }

            divIncludeInSearch.Visible = (module.FeatureGuid == HtmlContent.FeatureGuid);

            divTitleElement.Visible = WebConfigSettings.EnableEditingModuleTitleElement && WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins);

            useSeparatePagesForRoles = (Role.CountOfRoles(siteSettings.SiteId) >= WebConfigSettings.TooManyRolesForModuleSettings);
            divRoles.Visible = !useSeparatePagesForRoles;
            divRoleLinks.Visible = useSeparatePagesForRoles;

            if (!isAdmin)
            {
                // only admins can lock content down to only admins
                rbViewAdminOnly.Enabled = false;
                rbViewUseRoles.Enabled = false;
                rbEditAdminsOnly.Enabled = false;
                rbEditUseRoles.Enabled = false;
                //rbDraftAdminsOnly.Enabled = false;
                //rbDraftUseRoles.Enabled = false;

            }
            else
            {
                SetupRoleToggleScript();
            }
        }

        private void SetupRoleToggleScript()
        {
            if (useSeparatePagesForRoles) { return; }

            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("function DeSelectRoles(chkBoxContainer) {");

            //script.Append("alert('function called'); ");

            script.Append("$(chkBoxContainer).find('input[type=checkbox]').each(function(){this.checked = false; }); ");

            //script.Append("}");

            script.Append("} ");

            script.Append("$(document).ready(function() {");

            script.Append("$('#" + rbViewAdminOnly.ClientID + "').change(function(){");
            script.Append("var selectedVal = $('#" + rbViewAdminOnly.ClientID + "').attr('checked'); ");
            script.Append("if(selectedVal === 'checked'){");
            script.Append("DeSelectRoles('#" + cblViewRoles.ClientID + "');}");
            script.Append("});");


            script.Append("$('#" + rbEditAdminsOnly.ClientID + "').change(function(){");
            script.Append("var selectedVal = $('#" + rbEditAdminsOnly.ClientID + "').attr('checked'); ");
            script.Append("if(selectedVal === 'checked'){");
            script.Append("DeSelectRoles('#" + authEditRoles.ClientID + "');}");
            script.Append("});");


            //script.Append("$('#" + rbDraftAdminsOnly.ClientID + "').change(function(){");
            //script.Append("var selectedVal = $('#" + rbDraftAdminsOnly.ClientID + "').attr('checked'); ");
            //script.Append("if(selectedVal === 'checked'){");
            //script.Append("DeSelectRoles('#" + draftEditRoles.ClientID + "');}");
            //script.Append("});");


           

            script.Append("}); ");

            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "roletoggle", script.ToString());

        }
        
        
		
	}
}
