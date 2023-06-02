/// Author:        
///	Last Modified: 2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
					siteSettings.AllowPageSkins &&
					(CurrentPage != null) &&
					(CurrentPage.Skin.Length > 0)
					)
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
		}


		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			btnSave.Click += new EventHandler(btnSave_Click);
			btnDelete.Click += new EventHandler(btnDelete_Click);

			SuppressMenuSelection();
			SuppressPageMenu();

			LoadSettings();
			PopulateCustomSettings();
		}

		#endregion


		private void Page_Load(object sender, EventArgs e)
		{
			SecurityHelper.DisableBrowserCache();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			divEditUser.Visible = false;

			lblValidationSummary.Text = string.Empty;

			if (!canEdit || module == null || SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			PopulateLabels();
			//SetupIconScript();

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
				lblFeatureName.Text =
					ResourceHelper.GetResourceString(
					moduleDefinition.ResourceFile,
					moduleDefinition.FeatureName);

				litFeatureSpecificSettingsTab.Text = string.Format(CultureInfo.InvariantCulture, Resource.FeatureSettingsTabFormat, lblFeatureName.Text);

				divCacheTimeout.Visible = (!WebConfigSettings.DisableContentCache && moduleDefinition.IsCacheable);

				divIsGlobal.Visible = (moduleDefinition.SupportsPageReuse && (!WebConfigSettings.DisableGlobalContent));

				PopulatePageList();
				lblModuleId.Text = module.ModuleId.ToInvariantString();
				lblModuleGuid.Text = " {" + module.ModuleGuid.ToString() + "}";
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

				if (isAdmin || isContentAdmin || isSiteEditor)
				{
					divEditUser.Visible = true;

					if (module.EditUserId > 0)
					{
						SiteUser siteUser = new SiteUser(siteSettings, module.EditUserId);
						acUser.Text = siteUser.Name;
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

				//if (module.Icon.Length > 0)
				//{
				//	ddIcons.SelectedValue = module.Icon;
				//	imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/" + module.Icon;
				//}
				//else
				//{
				//	imgIcon.Src = ImageSiteRoot + "/Data/SiteImages/FeatureIcons/blank.gif";
				//}

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

					foreach (ListItem item in draftEditRoles.Items)
					{
						if ((module.DraftEditRoles.LastIndexOf(item.Value + ";")) > -1)
						{
							item.Selected = true;
						}
					}

					if (use3LevelWorkFlow)
					{
						foreach (ListItem item in draftApprovalRoles.Items)
						{
							if ((module.DraftApprovalRoles.LastIndexOf(item.Value + ";")) > -1)
							{
								item.Selected = true;
							}
						}
					}
				}

				cblViewRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
				authEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
			}
		}

		private void PopulateRoleList()
		{
			liSecurity.Visible = isAdmin || isContentAdmin || isSiteEditor;
			tabSecurity.Visible = isAdmin || isContentAdmin || isSiteEditor;

			if (useSeparatePagesForRoles)
			{
				return;
			}

			authEditRoles.Items.Clear();
			cblViewRoles.Items.Clear();

			ListItem vAllItem = new ListItem();
			vAllItem.Text = Resource.RolesAllUsersRole;
			vAllItem.Value = "All Users";

			ListItem allItem = new ListItem();
			allItem.Text = Resource.RolesAllUsersRole;
			allItem.Value = "All Users";

			cblViewRoles.Items.Add(vAllItem);

			foreach (var role in Role.GetBySite(siteSettings.SiteId))
			{
				// no need or benefit to checking content admins or admins roles
				// since they are not limited by roles except the special case of Admins only role
				// an admin can lock the content down to only admins

				if (role.RoleName == Role.ContentAdministratorsRole || role.RoleName == Role.AdministratorsRole)
				{
					continue;
				}

				ListItem vItem = new ListItem();
				vItem.Text = role.DisplayName;
				vItem.Value = role.RoleName;
				cblViewRoles.Items.Add(vItem);

				ListItem item = new ListItem();
				item.Text = role.DisplayName;
				item.Value = role.RoleName;
				authEditRoles.Items.Add(item);

				ListItem draftItem = new ListItem();
				draftItem.Text = role.DisplayName;
				draftItem.Value = role.RoleName;
				draftEditRoles.Items.Add(draftItem);

				if (use3LevelWorkFlow)
				{
					ListItem draftApprovalItem = new ListItem();
					draftApprovalItem.Text = role.DisplayName;
					draftApprovalItem.Value = role.RoleName;
					draftApprovalRoles.Items.Add(draftApprovalItem);
				}
			}

			//using (IDataReader roles = Role.GetSiteRoles(siteSettings.SiteId))
			//{
			//	while (roles.Read())
			//	{
			//		string roleName = roles["RoleName"].ToString();

			//		// no need or benefit to checking content admins role
			//		// since they are not limited by roles except the special case of Admins only role
			//		if (roleName == Role.ContentAdministratorsRole)
			//		{
			//			continue;
			//		}

			//		// administrators role doesn't need permission, the only reason to show it is so that
			//		// an admin can lock the content down to only admins
			//		if (roleName == Role.AdministratorsRole)
			//		{
			//			continue;
			//		}

			//		ListItem vItem = new ListItem();
			//		vItem.Text = roles["DisplayName"].ToString();
			//		vItem.Value = roles["RoleName"].ToString();
			//		cblViewRoles.Items.Add(vItem);

			//		ListItem item = new ListItem();
			//		item.Text = roles["DisplayName"].ToString();
			//		item.Value = roles["RoleName"].ToString();
			//		authEditRoles.Items.Add(item);

			//		ListItem draftItem = new ListItem();
			//		draftItem.Text = roles["DisplayName"].ToString();
			//		draftItem.Value = roles["RoleName"].ToString();
			//		draftEditRoles.Items.Add(draftItem);

			//		if (use3LevelWorkFlow)
			//		{
			//			ListItem draftApprovalItem = new ListItem();
			//			draftApprovalItem.Text = roles["DisplayName"].ToString();
			//			draftApprovalItem.Value = roles["RoleName"].ToString();
			//			draftApprovalRoles.Items.Add(draftApprovalItem);
			//		}
			//	}
			//}

			cblViewRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
			authEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
			draftEditRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;
			draftApprovalRoles.Enabled = isAdmin || isContentAdmin || isSiteEditor;

			divMyPage.Visible = (WebConfigSettings.MyPageIsInstalled && (isAdmin || isContentAdmin || isSiteEditor) && siteSettings.EnableMyPageFeature);
			divMyPageMulti.Visible = divMyPage.Visible;
		}


		private void PopulateCustomSettings()
		{
			if (module == null)
			{
				return;
			}

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

			if (groupsExist)
			{
				pnlcustomSettings.CssClass = "mojo-accordion-nh";
			}

			// these are the settings attached to the module instance
			ArrayList customSettingValues = ModuleSettings.GetCustomSettingValues(module.ModuleId);

			Panel groupWrapper = new Panel();
			string currentGroup = string.Empty;
			int loopCount = 0;

			foreach (CustomModuleSetting s in DefaultSettings)
			{
				if ((s.GroupName != currentGroup) || (loopCount == 0))
				{
					currentGroup = s.GroupName;

					if (groupsExist)
					{
						string localizedGroup = ResourceHelper.GetResourceString(s.ResourceFile, s.GroupName);
						Literal groupHeader = new()
						{
							Text = "<h3><a href=\"#\">" + localizedGroup + "</a></h3>"
						};
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

			if (s.SettingControlType == string.Empty)
			{
				return;
			}

			String resourceFile = "Resource";

			if (s.ResourceFile.Length > 0)
			{
				resourceFile = s.ResourceFile;
			}

			String settingLabel = GetResourceString(resourceFile, s.SettingName);


			BasePanel panel = new BasePanel();
			panel.Element = displaySettings.ModuleSettingsSettingPanelElement;

			var controlMin = string.Empty;
			var controlMax = string.Empty;
			var attributes = UIHelper.GetDictionaryFromString(s.Attributes);

			string panelClass = string.Empty;
			if (!attributes.TryGetValue("panelClass", out panelClass))
			{
				panelClass = $"$default$ {s.SettingName}";
			}
			panelClass = panelClass.Replace("$default$", displaySettings.ModuleSettingsSettingPanelClass);

			string labelClass = string.Empty;
			if (!attributes.TryGetValue("labelClass", out labelClass))
			{
				labelClass = "$default$";
			}
			labelClass = labelClass.Replace("$default$", displaySettings.ModuleSettingsSettingLabelClass);

			string controlClass = string.Empty;
			if (!attributes.TryGetValue("controlClass", out controlClass))
			{
				controlClass = "$default$";
			}
			controlClass = controlClass.Replace("$default$", displaySettings.ModuleSettingsSettingControlClass);

			panel.CssClass = panelClass;

			attributes.TryGetValue("min", out controlMin);
			attributes.TryGetValue("max", out controlMax);

			StringBuilder attribsMarkup = new();
			foreach (var attrib in attributes.Where(a => !a.Key.IsIn("panelClass", "labelClass", "controlClass")))
			{
				string attribValue = attrib.Value.StartsWith("Resource:") ? GetResourceString(resourceFile, attrib.Value.Substring(9)).ToString() : attrib.Value;
				
				attribsMarkup.Append($" {attrib.Key}=\"{attribValue}\"");
			}

			string controlID = s.SettingName + moduleId.ToInvariantString();

			Literal label = new()
			{
				Text = $"<label class=\"{labelClass}\" for=\"{controlID}\">{settingLabel}</label>"
			};
			panel.Controls.Add(label);

			//creating generic control here so we can cast it as whatever type we need to and still add it to the panel in a single location
			Control control = new();

			//string txtBoxMarkupFormat = "<input name=\"{0}\" id=\"{0}\" type=\"text\" class=\"{1}\" value=\"{2}\"{3} />";
			var settingType = s.SettingControlType.ToLower();
			switch (settingType)
			{
				case "textbox":
				case "number":
				case "color":
				case "password":
				case "range":
				case "email":
					Literal textBox = new();
					control = textBox;
					//textBox.Text = string.Format(txtBoxMarkupFormat, controlID, controlClass, s.SettingValue.HtmlEscapeQuotes(), attribsMarkup);
					string type = "text";
					if (settingType != "textbox")
					{
						type = settingType;
					}

					string min = string.Empty;
					string max = string.Empty;
					if (settingType == "number" || settingType == "range")
					{
						min = string.IsNullOrWhiteSpace(controlMin) ? string.Empty : $" min=\"{controlMin}\" ";
						max = string.IsNullOrWhiteSpace(controlMax) ? string.Empty : $" max=\"{controlMax}\" ";
					}

					textBox.Text = $"<input name=\"{controlID}\" id=\"{controlID}\" type=\"{type}\"{min}{max}class=\"{controlClass}\" value=\"{s.SettingValue.HtmlEscapeQuotes()}\"{attribsMarkup} />";
					break;
				case "checkbox":
					Literal checkBox = new Literal();
					control = checkBox;
					string check = string.Equals(s.SettingValue, "true", StringComparison.InvariantCultureIgnoreCase) ? " checked" : string.Empty;
					//checkBox.Text = string.Format(controlMarkupFormat, controlID, "checkbox", controlClass, s.SettingValue.HtmlEscapeQuotes(), attribsMarkup + (isChecked ? " checked" : ""));
					checkBox.Text = $"<input name=\"{controlID}\" id=\"{controlID}\" type=\"checkbox\" class=\"{controlClass}\"{attribsMarkup}{check}/>";
					break;
				case "dropdownlist":
					Literal ddl = new Literal();
					control = ddl;
					var options = UIHelper.GetDictionaryFromString(s.Options);

					StringBuilder optionsMarkup = new StringBuilder();
					foreach (var op in options)
					{
						string optionName = op.Key.StartsWith("Resource:") ? GetResourceString(resourceFile, op.Key.Substring(9)).ToString() : op.Key;
						string selected = s.SettingValue == op.Value ? " selected" : string.Empty;
						optionsMarkup.Append($"<option value=\"{op.Value}\"{selected}>{optionName}</option>");
					}
					ddl.Text = $"<select name=\"{controlID}\" id=\"{controlID}\" class=\"{controlClass}\"{attribsMarkup}>{optionsMarkup}</select>";
					break;
				case "isettingcontrol":
				case "customfield":
					if (s.ControlSrc.Length > 0)
					{
						if (s.ControlSrc.EndsWith(".ascx"))
						{
							control = Page.LoadControl(s.ControlSrc);

							if (control is ISettingControl)
							{
								ISettingControl sc = control as ISettingControl;

								if (!IsPostBack)
								{
									sc.SetValue(s.SettingValue);
								}

								control.ID = "uc" + moduleId.ToInvariantString() + s.SettingName;
							}
							else if (control is ICustomField)
							{
								ICustomField sc = control as ICustomField;
								if (!IsPostBack)
								{
									sc.SetValue(s.SettingValue);
								}

								var attribs = UIHelper.GetDictionaryFromString(s.Attributes);

								sc.Attributes(attribs);

								control.ID = "uc" + moduleId.ToInvariantString() + s.SettingName;
								panel.Controls.Add(control);
							}
						}
						else
						{
							try
							{
								control = Activator.CreateInstance(Type.GetType(s.ControlSrc)) as Control;

								if (control != null)
								{
									if (control is ISettingControl)
									{
										ISettingControl sc = control as ISettingControl;
										control.ID = "uc" + moduleId.ToInvariantString() + s.SettingName;
										panel.Controls.Add(control);

										if (!IsPostBack)
										{
											sc.SetValue(s.SettingValue);
										}
									}
									else
									{
										log.Error($"setting control {s.ControlSrc} does not implement ISettingControl");
									}
								}
							}
							catch (Exception ex)
							{
								log.Error(ex);
							}
						}
					}
					else
					{
						log.Error($"could not add setting control for {s.SettingControlType}, missing controlsrc for {s.SettingName}");
					}
					break;
			}
			
			panel.Controls.Add(control);
			if (s.HelpKey.Length > 0)
			{
				panel.Controls.Add(mojoHelpLink.GetHelpLinkControl(s.HelpKey));
			}
			groupPanel.Controls.Add(panel);
		}

		private string GetResourceString (string resourceFile, string resourceKey)
		{
			string resourceString = resourceKey;
			try
			{
				resourceString = GetGlobalResourceObject(resourceFile, resourceKey).ToString();
				resourceString ??= resourceKey;
			}
			catch (NullReferenceException ex)
			{
				log.Error($"ModuleSettings.aspx.cs handled error getting resource for {resourceKey} from {resourceFile}", ex);
			}
			return resourceString;
		}

		private void PopulatePageList()
		{
			if (!divParentPage.Visible)
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
					if (mojoNode.ParentId > -1)
					{
						pagePrefix += "-";
					}

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
			if (debugLog)
			{
				log.Debug("ModuleSettingsPage about to call Page.Validate()");
			}

			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			string userName = string.Empty;

			if (currentUser != null)
			{
				userName = currentUser.Name;
			}

			Page.Validate("ModuleSettings");

			if (Page.IsValid)
			{
				if (debugLog)
				{
					log.Debug("ModuleSettingsPage about to call Page IsValid = true");
				}

				bool ok = true;
				bool allSetingsAreValid = true;
				bool needToReIndex = false;
				int currentPageId = module.PageId;
				int newPageId = module.PageId;

				if (module.ModuleId > -1)
				{
					if (isAdmin || isContentAdmin || isSiteEditor)
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
									if (debugLog)
									{
										log.Debug("ModuleSettingsPage inside loop of Role ListItems");
									}

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

							if (debugLog)
							{
								log.Debug("ModuleSettingsPage about to loop through Role ListItems");
							}

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
									if (debugLog)
									{
										log.Debug("ModuleSettingsPage inside loop of Role ListItems");
									}

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

							foreach (ListItem item in draftEditRoles.Items)
							{
								if (item.Selected == true)
								{
									draftEdits = draftEdits + item.Value + ";";
								}
							}

							module.DraftEditRoles = draftEdits;

							log.Info("user " + userName + " changed Module Draft Edit roles for " + module.ModuleTitle
									+ " to " + draftEdits
									+ " from ip address " + SiteUtils.GetIP4Address());

							if (use3LevelWorkFlow)
							{
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
						module.PublishMode = Convert.ToInt32(publishType.GetValue(), CultureInfo.InvariantCulture);
						module.AvailableForMyPage = chkAvailableForMyPage.Checked;
						module.AllowMultipleInstancesOnMyPage = chkAllowMultipleInstancesOnMyPage.Checked;
						//module.Icon = ddIcons.SelectedValue;
						module.HideFromAuthenticated = chkHideFromAuth.Checked;
						module.HideFromUnauthenticated = chkHideFromUnauth.Checked;
						module.IncludeInSearch = chkIncludeInSearch.Checked;
						module.IsGlobal = chkIsGlobal.Checked;

						if (divParentPage.Visible)
						{
							if (debugLog)
							{
								log.Debug("ModuleSettingsPage about to check Page dropdown");
							}

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
							if (debugLog)
							{
								log.Debug("ModuleSettingsPage about to check user dropdown");
							}

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

					if (debugLog)
					{
						log.Debug("ModuleSettingsPage about to Save Module");
					}

					module.Save();

					if (needToReIndex)
					{
						// if content is moved from 1 page to another, need to reindex both pages
						// to keep view permissions in sync

						SearchIndex.IndexHelper.RebuildPageIndexAsync(CurrentPage);

						PageSettings newPage = new PageSettings(siteSettings.SiteId, newPageId);
						newPage.PageIndex = 0;
						SearchIndex.IndexHelper.RebuildPageIndexAsync(newPage);
					}

					ArrayList defaultSettings = ModuleSettings.GetDefaultSettings(module.ModuleDefId);

					foreach (CustomModuleSetting s in defaultSettings)
					{
						if (s.SettingControlType == string.Empty)
						{
							continue;
						}

						ok = true;

						//Object oSettingLabel = GetGlobalResourceObject("Resource", s.SettingName + "RegexWarning");

						Object oSettingLabel = null;

						try
						{
							oSettingLabel = GetGlobalResourceObject(s.ResourceFile, s.SettingName + "RegexWarning");
						}
						catch (NullReferenceException) { }
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
							if (s.SettingControlType == "ISettingControl" || s.SettingControlType == "CustomField")
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
									else if (c is ICustomField)
									{
										ICustomField icf = c as ICustomField;
										settingValue = icf.GetValue();
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
					SearchIndex.IndexHelper.RebuildPageIndexAsync(new PageSettings(siteSettings.SiteId, pageId));
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
				CacheHelper.ClearModuleCache(module.ModuleId);
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
		}


		private void PopulatePageArray(ArrayList sitePages)
		{
			SiteMapDataSource siteMapDataSource = (SiteMapDataSource)Page.Master.FindControl("SiteMapData");

			siteMapDataSource.SiteMapProvider
					= "mojosite" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);

			SiteMapNode siteMapNode = siteMapDataSource.Provider.RootNode;
			mojoSiteMapProvider.PopulateArrayList(sitePages, siteMapNode);


		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ModuleSettingsPageTitle);
			litFeatureSpecificSettingsTab.Text = Resource.ModuleSettingsSettingsTab;

			heading.Text = Resource.ModuleSettingsSettingsLabel;

			btnSave.Text = Resource.ModuleSettingsApplyButton;
			SiteUtils.SetButtonAccessKey(btnSave, AccessKeys.ModuleSettingsApplyButtonAccessKey);

			btnDelete.Text = Resource.ModuleSettingsDeleteButton;
			SiteUtils.SetButtonAccessKey(btnDelete, AccessKeys.ModuleSettingsDeleteButtonAccessKey);
			UIHelper.AddConfirmationDialog(btnDelete, Resource.ModuleSettingsDeleteConfirm);

			lnkCancel.Text = Resource.ModuleSettingsCancelButton;

			//if (!Page.IsPostBack)
			//{
			//	FileInfo[] fileInfo = SiteUtils.GetFeatureIconList();
			//	ddIcons.DataSource = fileInfo;
			//	ddIcons.DataBind();

			//	ddIcons.Items.Insert(0, new ListItem(Resource.ModuleSettingsNoIconLabel, "blank.gif"));
			//	ddIcons.Attributes.Add("onChange", "javascript:showIcon(this);");
			//	ddIcons.Attributes.Add("size", "6");
			//}

			acUser.DataUrl = SiteRoot + "/Services/UserLookup.asmx/AutoComplete";

			reqCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRequiredMessage;
			regexCacheTime.ErrorMessage = Resource.ModuleSettingsCacheRegexWarning;

			litGeneralSettingsTabLink.Text = "<a href='#" + tabGeneralSettings.ClientID + "'>" + Resource.ModuleSettingsGeneralTab + "</a>";

			litSecurityLink.Text = "<a href='#" + tabSecurity.ClientID + "'>" + Resource.ModuleSettingsSecurityTab + "</a>";

			rbViewAdminOnly.Text = Resource.AdminsOnly;
			rbViewUseRoles.Text = Resource.RolesAllowed;

			rbEditAdminsOnly.Text = Resource.AdminsOnly;
			rbEditUseRoles.Text = Resource.RolesAllowed;

			lnkPageViewRoles.Text = Resource.ModuleSettingsViewRolesLabel;
			lnkPageViewRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString() + "&p=v";

			lnkPageEditRoles.Text = Resource.ModuleSettingsEditRolesLabel;
			lnkPageEditRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString() + "&p=e";

			lnkPageDraftRoles.Text = Resource.ModuleSettingsDraftEditRolesLabel;
			lnkPageDraftRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString() + "&p=d";

			lnkPageApprovalRoles.Text = Resource.DraftApprovalRoles;
			lnkPageApprovalRoles.NavigateUrl = SiteRoot + "/Admin/ModulePermissions.aspx?pageid=" + pageId.ToInvariantString()
				+ "&mid=" + moduleId.ToInvariantString() + "&p=a";

			AddClassToBody("administration");
			AddClassToBody("featuresettings");

			acUser.TargetValueElementClientId = txtEditUserId.ClientID;
		}


		//private void SetupIconScript()
		//{
		//	string logoScript = "<script type=\"text/javascript\">"
		//		+ "function showIcon(listBox) { if(!document.images) return; "
		//		+ "var iconPath = '" + iconPath + "'; "
		//		+ "document.images." + imgIcon.ClientID + ".src = iconPath + listBox.value;"
		//		+ "}</script>";

		//	Page.ClientScript.RegisterClientScriptBlock(GetType(), "showIcon", logoScript);

		//}


		private void LoadSettings()
		{
			moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
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

			if (isAdmin || isContentAdmin || isSiteEditor)
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
				if (WebConfigSettings.HideModuleSettingsDeleteButtonFromNonAdmins)
				{
					btnDelete.Visible = false;
				}

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

			if (module.ModuleId == -1)
			{
				module = null;
				return; // module doesn't exist
			}

			if (module.SiteId != siteSettings.SiteId)
			{
				module = null;
				return;
			}

			if (!canEdit)
			{
				if (
					WebUser.IsInRoles(module.AuthorizedEditRoles) ||
					((WebUser.IsInRoles(module.DraftEditRoles)) && (!module.IsGlobal)) ||
					((WebUser.IsInRoles(CurrentPage.EditRoles)) && (!module.IsGlobal)) ||
					((WebUser.IsInRoles(CurrentPage.DraftEditOnlyRoles)) && (!module.IsGlobal))
					)
				{
					canEdit = true;
				}
			}

			if ((isContentAdmin || isSiteEditor) && (module.AuthorizedEditRoles == "Admins;"))
			{
				canEdit = false;
			}

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

			if (canEdit && (!isAdmin) && (WebUser.IsInRoles(siteSettings.RolesNotAllowedToEditModuleSettings)))
			{
				canEdit = false;
			}

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
			}
			else
			{
				SetupRoleToggleScript();
			}
		}


		private void SetupRoleToggleScript()
		{
			if (useSeparatePagesForRoles)
			{
				return;
			}

			StringBuilder script = new StringBuilder();

			script.Append("\n<script type='text/javascript'>");

			script.Append("function DeSelectRoles(chkBoxContainer) {");

			script.Append("$(chkBoxContainer).find('input[type=checkbox]').each(function(){this.checked = false; }); ");

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

			script.Append("}); ");

			script.Append("</script>");

			Page.ClientScript.RegisterStartupScript(typeof(Page), "roletoggle", script.ToString());
		}
	}
}
