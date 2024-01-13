using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Security;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web;

public sealed class mojoSetup
{
	private static readonly ILog log = LogManager.GetLogger(typeof(mojoSetup));

	public const string DefaultPageEncoding = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
	private static readonly char pathSep = Path.DirectorySeparatorChar;
	private const string testFilename = "test.config";
	private const string sitesPath = "~/Data/Sites/";
	private const string site1path = $"{sitesPath}1/";

	#region File System Tests

	public static bool DataFolderIsWritable()
	{
		bool result = false;

		try
		{
			TouchTestFile();
			result = true;
		}
		catch (UnauthorizedAccessException ex) 
		{
			var exception = ex;
		}

		return result;
	}

	/// <summary>
	/// Ensures path is writable
	/// </summary>
	/// <param name="pathToFile"></param>
	public static void TouchTestFile(string pathToFile)
	{
		if (!WebConfigSettings.FileSystemIsWritable)
		{
			return;
		}

		if (pathToFile is not null)
		{
			if (File.Exists(pathToFile))
			{
				File.SetLastWriteTimeUtc(pathToFile, DateTime.UtcNow);
			}
			else
			{
				StreamWriter streamWriter = File.CreateText(pathToFile);
				streamWriter.Close();
			}
		}
	}

	public static void TouchTestFile()
	{
		if (!WebConfigSettings.FileSystemIsWritable)
		{
			return;
		}

		if (HttpContext.Current is not null)
		{
			string pathToTestFile = HttpContext.Current.Server.MapPath($"~/Data/{testFilename}");
			TouchTestFile(pathToTestFile);

			Global.FileSystem.CreateFolder("~/Data/systemfiles/");

			Global.FileSystem.CreateFolder(site1path);

			pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}{testFilename}");
			TouchTestFile(pathToTestFile);

			if (WebConfigSettings.UseCacheDependencyFiles)
			{
				Global.FileSystem.CreateFolder($"{site1path}systemfiles/");

				pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}systemfiles/{testFilename}");
				TouchTestFile(pathToTestFile);
			}

			Global.FileSystem.CreateFolder($"{site1path}GalleryImages/");
			pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}GalleryImages/{testFilename}");
			TouchTestFile(pathToTestFile);

			Global.FileSystem.CreateFolder($"{site1path}FolderGalleries/");
			pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}FolderGalleries/{testFilename}");
			TouchTestFile(pathToTestFile);

			Global.FileSystem.CreateFolder($"{site1path}SharedFiles/");
			pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}SharedFiles/{testFilename}");
			TouchTestFile(pathToTestFile);

			Global.FileSystem.CreateFolder($"{site1path}SharedFiles/History/");
			pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}SharedFiles/History/{testFilename}");
			TouchTestFile(pathToTestFile);

			if (!WebConfigSettings.DisableSearchIndex)
			{
				Global.FileSystem.CreateFolder($"{site1path}index/");
				pathToTestFile = HttpContext.Current.Server.MapPath($"{site1path}index/{testFilename}");
				TouchTestFile(pathToTestFile);

				if (File.Exists(pathToTestFile))
				{
					File.Delete(pathToTestFile);
				}
			}
		}
	}

	public static void EnsureFolderGalleryFolder(SiteSettings siteSettings)
	{
		if (HttpContext.Current is null) return;

		string path = Invariant($"{sitesPath}{siteSettings.SiteId}/FolderGalleries/");
		EnsureDirectory(path);
	}

	#endregion

	#region System Probing

	public static bool UpgradeIsNeeded()
	{
		bool result = false;

		Version dbCodeVersion = DatabaseHelper.DBCodeVersion();
		Version dbSchemaVersion = DatabaseHelper.DBSchemaVersion();
		if (dbCodeVersion > dbSchemaVersion) result = true;

		return result;
	}

	public static bool RunningInFullTrust()
	{
		bool result = false;

		AspNetHostingPermissionLevel currentTrustLevel = SecurityHelper.GetCurrentTrustLevel();

		if (currentTrustLevel == AspNetHostingPermissionLevel.Unrestricted)
		{
			result = true;
		}

		return result;
	}

	#endregion

	#region Schema Upgrade methods

	public static void DoSchemaUpgrade(string overrideConnectionInfo)
	{
		log.Debug("mojoSetup entered DoSchemaUpgrade");
		bool canAlterSchema = DatabaseHelper.CanAlterSchema(overrideConnectionInfo);

		if (HttpContext.Current is not null && canAlterSchema)
		{
			Version currentSchemaVersion = DatabaseHelper.DBSchemaVersion();

			Guid appID = DatabaseHelper.GetApplicationId();
			var pathToScriptFolder = HttpContext.Current.Server.MapPath(Invariant($"~/Setup/SchemaUpgradeScripts/{DatabaseHelper.DBPlatform()}/{DatabaseHelper.GetApplicationName()}/"));

			if (Directory.Exists(pathToScriptFolder))
			{
				var directoryInfo = new DirectoryInfo(pathToScriptFolder);
				var scriptFiles = directoryInfo.GetFiles("*.config");

				foreach (var fileInfo in scriptFiles)
				{
					Version scriptVersion = DatabaseHelper.ParseVersionFromFileName(fileInfo.Name);

					if ((scriptVersion is not null) && (scriptVersion > currentSchemaVersion) && !DatabaseHelper.SchemaScriptHasBeenRun(appID, fileInfo.Name))
					{
						DatabaseHelper.RunScript(appID, fileInfo, overrideConnectionInfo);
						DoPostScriptTasks(scriptVersion, overrideConnectionInfo);
					}
				}
			}
			else
			{
				log.Error($"mojoSetup.DoSchemaUpgrade Error could not find path: {pathToScriptFolder}");
			}
		}
		else
		{
			log.Error("mojoSetup.DoSchemaUpgrade Error: no httpcontext or no permission to alter schema. ");
		}
	}

	public static void DoPostScriptTasks(Version scriptVersion, string overrideConnectionInfo)
	{
		DatabaseHelper.RunPostUpgradeTasks(scriptVersion, overrideConnectionInfo);

		//if (scriptVersion == new Version(2, 2, 3, 0))
		//{
		//	DatabaseHelper.DoVersion2230PostUpgradeTasks(overrideConnectionInfo);
		//}

		//if (scriptVersion == new Version(2, 2, 3, 4))
		//{
		//	DatabaseHelper.DoVersion2234PostUpgradeTasks(overrideConnectionInfo);
		//}

		//if (scriptVersion == new Version(2, 2, 4, 7))
		//{
		//	DatabaseHelper.DoVersion2247PostUpgradeTasks(overrideConnectionInfo);
		//}

		//if (scriptVersion == new Version(2, 2, 5, 3))
		//{
		//	DatabaseHelper.DoVersion2253PostUpgradeTasks(overrideConnectionInfo);
		//}

		//if (scriptVersion == new Version(2, 3, 2, 0))
		//{
		//	DatabaseHelper.DoVersion2320PostUpgradeTasks(overrideConnectionInfo);
		//}
	}
	#endregion

	#region Create Initial Data

	public static SiteSettings CreateNewSite()
	{
		string templateFolderPath = GetMessageTemplateFolder();
		string templateFolder = templateFolderPath;

		var newSite = new SiteSettings
		{
			SiteName = GetSetupTemplate(templateFolder, "InitialSiteNameContent.config"),
			Skin = WebConfigSettings.DefaultInitialSkin,
			Logo = GetSetupTemplate(templateFolder, "InitialSiteLogoContent.config"),
			AllowHideMenuOnPages = false,
			AllowNewRegistration = true,
			AllowPageSkins = false,
			AllowUserFullNameChange = false,
			AllowUserSkins = false,
			AutoCreateLdapUserOnFirstLogin = true,
			EditorSkin = SiteSettings.ContentEditorSkin.normal,
			Icon = string.Empty,
			IsServerAdminSite = true,
			ReallyDeleteUsers = true,
			UseEmailForLogin = true,
			UseLdapAuth = false,
			SiteLdapSettings = new LdapSettings()
			{
				Port = 389,
				RootDN = string.Empty,
				Server = string.Empty
			},
			UseSecureRegistration = false,
			UseSslOnAllPages = WebConfigSettings.SslIsRequiredByWebServer,
			AllowPasswordReset = true,
			AllowPasswordRetrieval = true,
			PasswordFormat = WebConfigSettings.InitialSitePasswordFormat, //0 = clear, 1= hashed, 2= encrypted
			RequiresQuestionAndAnswer = true,
			MaxInvalidPasswordAttempts = 10,
			PasswordAttemptWindowMinutes = 5,
			RequiresUniqueEmail = true,
			MinRequiredNonAlphanumericCharacters = 0,
			MinRequiredPasswordLength = 7,
			PasswordStrengthRegularExpression = string.Empty,
			DefaultEmailFromAddress = GetSetupTemplate(templateFolder, "InitialEmailFromContent.config")
		};

		newSite.Save();

		//newSite.DefaultFriendlyUrlPattern = SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX;
		//newSite.EncryptPasswords = false;
		//newSite.CreateInitialDataOnCreate = false;

		return newSite;
	}

	private static SiteUser EnsureAdminUser(SiteSettings site)
	{
		// if using related sites mode there is a problem if we already have user admin@admin.com
		// and we create another one in the child site with the same email and login so we need to make it different
		// we could just skip creating this user since in related sites mode all users come from the first site
		// but then if the config were changed to not related sites mode there would be no admin user
		// so in related sites mode we create one only as a backup in case settings are changed later
		int countOfSites = SiteSettings.SiteCount();
		string siteDifferentiator = string.Empty;

		if (countOfSites >= 1 && WebConfigSettings.UseRelatedSiteMode)
		{
			siteDifferentiator = site.SiteId.ToInvariantString();
		}

		bool overridRelatedSiteMode = true;
		var adminUser = new SiteUser(site, overridRelatedSiteMode)
		{
			Email = string.Format(Core.Configuration.AppConfig.DefaultAdminUserEmailFormat, siteDifferentiator),
			Name = "Admin",
			LoginName = string.Format(Core.Configuration.AppConfig.DefaultAdminUsernameFormat, siteDifferentiator)
		};

		bool userExists;

		if (site.UseEmailForLogin)
		{
			userExists = SiteUser.EmailExistsInDB(site.SiteId, adminUser.Email);
		}
		else
		{
			userExists = SiteUser.LoginExistsInDB(site.SiteId, adminUser.LoginName);
		}

		if (!userExists)
		{
			adminUser.Password = Core.Configuration.AppConfig.DefaultAdminPassword;

			if (Membership.Provider is mojoMembershipProvider membership)
			{
				adminUser.Password = membership.EncodePassword(site, adminUser, Core.Configuration.AppConfig.DefaultAdminPassword);
			}

			adminUser.PasswordQuestion = Core.Configuration.AppConfig.DefaultAdminSecurityQuestion;
			adminUser.PasswordAnswer = Core.Configuration.AppConfig.DefaultAdminSecurityAnswer;
			adminUser.Save();
		}
		else
		{
			if (site.UseEmailForLogin)
			{
				adminUser = new SiteUser(site, adminUser.Email);
			}
			else
			{
				adminUser = new SiteUser(site, adminUser.LoginName);
			}
		}

		return adminUser;
	}

	public static Role EnsureRole(SiteSettings site, string roleName, string displayName)
	{
		var role = new Role(site.SiteId, roleName);
		if (role is not null)
		{
			return role;
		}
		else
		{
			role = new Role
			{
				RoleName = roleName,
				DisplayName = displayName,
				SiteId = site.SiteId,
				SiteGuid = site.SiteGuid
			};
			role.Save();
			return role;
		}
	}

	public static void EnsureRolesAndAdminUser(SiteSettings site)
	{
		SiteUser adminUser = EnsureAdminUser(site);

		var adminRole = EnsureRole(site, "Admins", "Administrators");
		if (adminRole is not null)
		{
			Role.AddUser(adminRole.RoleId, adminUser.UserId, adminRole.RoleGuid, adminUser.UserGuid);
		}

		EnsureRole(site, "Role Admins", SetupResource.RoleNameRoleAdministrators);
		EnsureRole(site, "Content Administrators", SetupResource.RoleNameContentAdministrators);
		EnsureRole(site, "Authenticated Users", SetupResource.RoleNameAuthenticated);
		EnsureRole(site, "Content Publishers", SetupResource.RoleNameContentPublishers);
		EnsureRole(site, "Content Authors", SetupResource.RoleNameContentAuthors);
		EnsureRole(site, "Newsletter Administrators", SetupResource.RoleNameNewsletterAdministrators);
	}

	#endregion

	#region CreateNewSiteData(SiteSettings siteSettings)

	public static void CreateNewSiteData(SiteSettings siteSettings)
	{
		EnsureRolesAndAdminUser(siteSettings);
		CreateDefaultSiteFolders(siteSettings.SiteId);
		EnsureSkins(siteSettings.SiteId);

		if (PageSettings.GetCountOfPages(siteSettings.SiteId) == 0)
		{
			SetupDefaultContentPages(siteSettings);
		}
	}

	public static void SetupDefaultContentPages(SiteSettings siteSettings)
	{
		ContentPageConfiguration appPageConfig = ContentPageConfiguration.GetConfig();

		foreach (ContentPage contentPage in appPageConfig.ContentPages)
		{
			CreatePage(siteSettings, contentPage, null);
		}

		CacheHelper.ResetSiteMapCache();
	}

	public static void CreatePage(SiteSettings siteSettings, ContentPage contentPage, PageSettings parentPage)
	{
		var pageSettings = new PageSettings
		{
			PageGuid = Guid.NewGuid(),
			SiteId = siteSettings.SiteId,
			SiteGuid = siteSettings.SiteGuid,
			AuthorizedRoles = contentPage.VisibleToRoles,
			EditRoles = contentPage.EditRoles,
			DraftEditOnlyRoles = contentPage.DraftEditRoles,
			CreateChildPageRoles = contentPage.CreateChildPageRoles,
			MenuImage = contentPage.MenuImage,
			PageMetaKeyWords = contentPage.PageMetaKeyWords,
			PageMetaDescription = contentPage.PageMetaDescription,
			PageOrder = contentPage.PageOrder,
			Url = contentPage.Url,
			RequireSsl = contentPage.RequireSsl,
			ShowBreadcrumbs = contentPage.ShowBreadcrumbs,
			BodyCssClass = contentPage.BodyCssClass,
			MenuCssClass = contentPage.MenuCssClass,
			IncludeInMenu = contentPage.IncludeInMenu,
			IsClickable = contentPage.IsClickable,
			IncludeInSiteMap = contentPage.IncludeInSiteMap,
			IncludeInChildSiteMap = contentPage.IncludeInChildPagesSiteMap,
			AllowBrowserCache = contentPage.AllowBrowserCaching,
			ShowChildPageBreadcrumbs = contentPage.ShowChildPageBreadcrumbs,
			ShowHomeCrumb = contentPage.ShowHomeCrumb,
			ShowChildPageMenu = contentPage.ShowChildPagesSiteMap,
			HideAfterLogin = contentPage.HideFromAuthenticated,
			EnableComments = contentPage.EnableComments
		};

		if (parentPage is not null)
		{
			pageSettings.ParentGuid = parentPage.PageGuid;
			pageSettings.ParentId = parentPage.PageId;
		}

		var uiCulture = Thread.CurrentThread.CurrentUICulture;

		if (WebConfigSettings.UseCultureOverride)
		{
			uiCulture = SiteUtils.GetDefaultUICulture(siteSettings.SiteId);
		}

		if (contentPage.ResourceFile.Length > 0)
		{
			pageSettings.PageName = ResourceHelper.GetResourceString(contentPage.ResourceFile, contentPage.Name, uiCulture, false);

			if (contentPage.Title.Length > 0)
			{
				pageSettings.PageTitle = ResourceHelper.GetResourceString(contentPage.ResourceFile, contentPage.Title, uiCulture, false);
			}
		}
		else
		{
			pageSettings.PageName = contentPage.Name;
			pageSettings.PageTitle = contentPage.Title;
		}

		pageSettings.Save();

		if (!FriendlyUrl.Exists(siteSettings.SiteId, pageSettings.Url))
		{
			if (!WebPageInfo.IsPhysicalWebPage(pageSettings.Url))
			{
				FriendlyUrl friendlyUrl = new FriendlyUrl
				{
					SiteId = siteSettings.SiteId,
					SiteGuid = siteSettings.SiteGuid,
					PageGuid = pageSettings.PageGuid,
					Url = pageSettings.Url.Replace("~/", string.Empty),
					RealUrl = $"~/Default.aspx?pageid={pageSettings.PageId.ToInvariantString()}"
				};
				friendlyUrl.Save();
			}
		}

		foreach (ContentPageItem pageItem in contentPage.PageItems)
		{
			Guid moduleGuid2Use = Guid.Empty;
			bool updateModule = false;

			Module findModule = null;

			if (pageItem.ModuleGuidToPublish != Guid.Empty)
			{
				var existingModule = new Module(pageItem.ModuleGuidToPublish);

				if (existingModule.ModuleGuid == pageItem.ModuleGuidToPublish && existingModule.SiteId == siteSettings.SiteId)
				{
					Module.Publish(
						pageSettings.PageGuid,
						existingModule.ModuleGuid,
						existingModule.ModuleId,
						pageSettings.PageId,
						pageItem.Location,
						pageItem.SortOrder,
						DateTime.UtcNow,
						DateTime.MinValue
					);

					continue;
				}
			}
			else if (pageItem.ModuleGuid != Guid.Empty)
			{
				findModule = new Module(pageItem.ModuleGuid);
				if (findModule.ModuleGuid == Guid.Empty)
				{
					// Module does not exist, we can create new one with the specified Guid 
					moduleGuid2Use = pageItem.ModuleGuid;
				}

				if (findModule.ModuleGuid == pageItem.ModuleGuid && findModule.SiteId == siteSettings.SiteId)
				{
					// The module already exist, we'll update existing one
					updateModule = true;
					moduleGuid2Use = findModule.ModuleGuid;
				}
			}

			var moduleDef = new ModuleDefinition(pageItem.FeatureGuid);

			// this only adds if its not already there
			try
			{
				SiteSettings.AddFeature(siteSettings.SiteGuid, pageItem.FeatureGuid);
			}
			catch (Exception ex)
			{
				log.Error(ex);
			}

			if (moduleDef.ModuleDefId > -1)
			{
				Module module = null;

				if (updateModule && (findModule is not null))
				{
					module = findModule;
				}
				else
				{
					module = new Module();
					module.ModuleGuid = moduleGuid2Use;
				}

				module.SiteId = siteSettings.SiteId;
				module.SiteGuid = siteSettings.SiteGuid;
				module.PageId = pageSettings.PageId;
				module.ModuleDefId = moduleDef.ModuleDefId;
				module.FeatureGuid = moduleDef.FeatureGuid;
				module.PaneName = pageItem.Location;

				if (contentPage.ResourceFile.Length > 0)
				{
					module.ModuleTitle = ResourceHelper.GetResourceString(contentPage.ResourceFile, pageItem.ContentTitle, uiCulture, false);
				}
				else
				{
					module.ModuleTitle = pageItem.ContentTitle;
				}

				module.ModuleOrder = pageItem.SortOrder;
				module.CacheTime = pageItem.CacheTimeInSeconds;
				module.Icon = moduleDef.Icon;
				module.ShowTitle = pageItem.ShowTitle;
				module.AuthorizedEditRoles = pageItem.EditRoles;
				module.DraftEditRoles = pageItem.DraftEditRoles;
				module.ViewRoles = pageItem.ViewRoles;
				module.IsGlobal = pageItem.IsGlobal;
				module.HeadElement = pageItem.HeadElement;
				module.HideFromAuthenticated = pageItem.HideFromAuthenticated;
				module.HideFromUnauthenticated = pageItem.HideFromAnonymous;

				module.Save();

				if ((pageItem.Installer is not null) && (pageItem.ConfigInfo.Length > 0))
				{
					//this is the newer implementation for populating feature content
					pageItem.Installer.InstallContent(module, pageItem.ConfigInfo);
				}
				else
				{
					// legacy implementation for backward compatibility
					if ((pageItem.FeatureGuid == HtmlContent.FeatureGuid) && pageItem.ContentTemplate.EndsWith(".config"))
					{
						var htmlContent = new HtmlContent
						{
							ModuleId = module.ModuleId,
							Body = ResourceHelper.GetMessageTemplate(uiCulture, pageItem.ContentTemplate),
							ModuleGuid = module.ModuleGuid
						};
						var repository = new HtmlRepository();
						repository.Save(htmlContent);
					}
				}

				// handling module settings
				foreach (KeyValuePair<string, string> item in pageItem.ModuleSettings)
				{
					ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, item.Key, item.Value);
				}
			}
		}

		foreach (ContentPage childPage in contentPage.ChildPages)
		{
			CreatePage(siteSettings, childPage, pageSettings);
		}
	}

	#endregion

	#region Site Folder Creation

	public static bool CreateDefaultSiteFolders(int siteId)
	{
		return CreateDefaultSiteFolders(siteId, true);
	}

	public static bool CreateDefaultSiteFolders(int siteId, bool includeStandardFiles)
	{
		if (HttpContext.Current is null)
		{
			return false;
		}

		//var pathSep = Path.DirectorySeparatorChar;
		string siteFolderPath = Invariant($"{HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}Sites{pathSep}")}{siteId}{pathSep}");
		string sourceFilesPath = HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}");

		//we only want to run the get for the FileSystem once so we create our own variable
		var fileSystem = Global.FileSystem;

		fileSystem.CreateFolder(siteFolderPath);

		fileSystem.CreateFolder($"{siteFolderPath}systemfiles");
		fileSystem.CreateFolder($"{siteFolderPath}media");
		fileSystem.CreateFolder($"{siteFolderPath}index");
		fileSystem.CreateFolder($"{siteFolderPath}SharedFiles");
		fileSystem.CreateFolder($"{siteFolderPath}SharedFiles{pathSep}History");
		fileSystem.CreateFolder($"{siteFolderPath}skins");

		if (WebConfigSettings.SiteLogoUseMediaFolder)
		{
			fileSystem.CreateFolder($"{siteFolderPath}media{pathSep}logos");
		}
		else
		{
			fileSystem.CreateFolder($"{siteFolderPath}logos");
		}

		if (WebConfigSettings.HtmlFragmentUseMediaFolder)
		{
			fileSystem.CreateFolder($"{siteFolderPath}media{pathSep}htmlfragments");
		}
		else
		{
			fileSystem.CreateFolder($"{siteFolderPath}htmlfragments");
		}

		if (WebConfigSettings.XmlUseMediaFolder)
		{
			fileSystem.CreateFolder($"{siteFolderPath}media{pathSep}xml");

			fileSystem.CreateFolder($"{siteFolderPath}media{pathSep}xsl");
		}
		else
		{
			fileSystem.CreateFolder($"{siteFolderPath}xml");

			fileSystem.CreateFolder($"{siteFolderPath}xsl");
		}

		if (includeStandardFiles)
		{
			if (WebConfigSettings.SiteLogoUseMediaFolder)
			{
				
				copyFiles($"{sourceFilesPath}logos", $"{siteFolderPath}media{pathSep}logos{pathSep}");
			}
			else
			{
				copyFiles($"{sourceFilesPath}logos", $"{siteFolderPath}logos{pathSep}");
			}

			if (WebConfigSettings.XmlUseMediaFolder)
			{
				copyFiles($"{sourceFilesPath}xml", $"{siteFolderPath}media{pathSep}xml{pathSep}");
				copyFiles($"{sourceFilesPath}xsl", $"{siteFolderPath}media{pathSep}xsl{pathSep}");
			}
			else
			{
				copyFiles($"{sourceFilesPath}xml", $"{siteFolderPath}xml{pathSep}");
				copyFiles($"{sourceFilesPath}xsl", $"{siteFolderPath}xsl{pathSep}");
			}
			if (WebConfigSettings.HtmlFragmentUseMediaFolder)
			{
				copyFiles($"{sourceFilesPath}htmlfragments", $"{siteFolderPath}media{pathSep}htmlfragments{pathSep}");
			}
			else
			{
				copyFiles($"{sourceFilesPath}htmlfragments", $"{siteFolderPath}htmlfragments{pathSep}");
			}
		}

		EnsureAdditionalSiteFolders(siteId);

		return true;
	}

	public static void EnsureAdditionalSiteFolders()
	{
		DataTable dataTable = SiteSettings.GetSiteIdList();
		foreach (DataRow row in dataTable.Rows)
		{
			int siteId = Convert.ToInt32(row["SiteID"]);
			EnsureAdditionalSiteFolders(siteId);
		}
	}

	public static void EnsureAdditionalSiteFolders(int siteId)
	{
		string siteFolderPath = HttpContext.Current.Server.MapPath(Invariant($"{GetApplicationRoot()}{pathSep}Data{pathSep}Sites{pathSep}{siteId}{pathSep}"));
		string sourceFilesPath = HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}");

		//EnsureTemplateImageFolder(siteFolderPath);
		EnsureDirectory($"{siteFolderPath}htmltemplateimages");
		copyFiles(sourceFilesPath, $"{siteFolderPath}htmltemplateimages{pathSep}");
		//EnsureUserFilesFolder(siteFolderPath);
		EnsureDirectory($"{siteFolderPath}userfiles");
	}

	public static bool EnsureSkins(int siteId, bool restore = false, bool copyNew = false)
	{
		if (HttpContext.Current is null)
		{
			return false;
		}

		string siteFolderPath = Invariant($"{GetApplicationRoot()}{pathSep}Data{pathSep}Sites{pathSep}{siteId}{pathSep}skins");
		string sourceFilesPath = HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}skins");
		DirectoryInfo dir;
		DirectoryInfo dirDest;

		var opResult = Global.FileSystem.CreateFolder(siteFolderPath);

		if (opResult == OpResult.AlreadyExist && (restore || copyNew))
		{
			if (Directory.Exists(sourceFilesPath))
			{
				dirDest = new DirectoryInfo(siteFolderPath);
				dir = new DirectoryInfo(sourceFilesPath);

				CopySkinFilesRecursively(dir, dirDest, copyNew);
			}
		}

		return true;
	}

	public static void CopySkinFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool copyNew = false)
	{
		if (!source.Name.StartsWith(".")) // Make sure to not copy .git, .svn, .vs, etc, folders from the Data/Sites/skins folder
		{
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!dir.Name.StartsWith(".")) // Make sure to not copy .git, .svn, .vs, etc, folders from the current skin folder
				{
					// if we're copying new skins we want to check if the dir exists in the target. If it does, we want to 
					// skip this dir because we only want to copy new skins.
					if (copyNew)
					{
						var targetDirChild = new DirectoryInfo(target.FullName + pathSep + dir.Name);
						if (targetDirChild.Exists)
						{
							continue;
						}
					}

					CopySkinFilesRecursively(dir, target.CreateSubdirectory(dir.Name), copyNew);
				}
			}

			foreach (FileInfo file in source.GetFiles())
			{
				file.CopyTo(Path.Combine(target.FullName, file.Name), true);
			}
		}
	}

	#endregion

	#region Newsletter Setup

	public static void CreateDefaultLetterTemplates(Guid siteGuid)
	{
		if (HttpContext.Current is null)
		{
			return;
		}

		string pathToTemplates = HttpContext.Current.Server.MapPath("~/Data/emailtemplates");

		if (!Directory.Exists(pathToTemplates))
		{
			return;
		}

		var dir = new DirectoryInfo(pathToTemplates);
		FileInfo[] templates = dir.GetFiles("*.config");
		foreach (FileInfo template in templates)
		{
			var emailTemplate = new LetterHtmlTemplate
			{
				SiteGuid = siteGuid,
				Title = template.Name.Replace(".config", string.Empty),
				Html = template.OpenText().ReadToEnd()
			};
			emailTemplate.Save();
		}
	}

	#endregion

	#region Helper Methods

	public static string GetSetupTemplate(string templateFolder, string templateFile)
	{
		if (templateFile is not null)
		{
			string culture = ConfigurationManager.AppSettings["Culture"] ?? "en-US-";
			string messageFile;

			if (File.Exists($"{templateFolder}{culture}-{templateFile}"))
			{
				messageFile = $"{templateFolder}{culture}-{templateFile}";
			}
			else
			{
				messageFile = $"{templateFolder}en-US-{templateFile}";
			}

			//var file = new FileInfo(messageFile);

			using var reader = new StreamReader(new FileStream(messageFile, FileMode.Open));
			return reader.ReadToEnd();

			//StreamReader sr = file.OpenText();
			//string message = file.OpenText().ReadToEnd();
			//sr.Close();
			//return message;
		}
		else
		{
			return string.Empty;
		}
	}

	public static string GetMessageTemplateFolder()
	{
		string result = string.Empty;

		if (HttpContext.Current is not null)
		{
			result = HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}MessageTemplates{pathSep}");
		}
		return result;
	}

	public static string GetApplicationRoot()
	{
		if (HttpContext.Current.Request.ApplicationPath.Length == 1)
		{
			return string.Empty;
		}
		else
		{
			return HttpContext.Current.Request.ApplicationPath;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="protocol"></param>
	/// <returns>hostname[:port] to use when constructing the site root URL</returns>
	private static string GetHost(string protocol)
	{
		string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		string serverPort = HttpContext.Current.Request.ServerVariables["SERVER_PORT"];

		// Most proxies add an X-Forwarded-Host header which contains the original Host header
		// including any non-default port.
		string forwardedHosts = HttpContext.Current.Request.Headers["X-Forwarded-Host"];
		if (forwardedHosts is not null)
		{
			// If the request passed thru multiple proxies, they will be separated by commas.
			// We only care about the first one.
			string forwardedHost = forwardedHosts.Split(',')[0];
			string[] serverAndPort = forwardedHost.Split(':');
			serverName = serverAndPort[0];
			serverPort = null;
			if (serverAndPort.Length > 1)
			{
				serverPort = serverAndPort[1];
			}
		}

		// Only include a port if it is not the default for the protocol and MapAlternatePort = true
		// in the config file.
		if ((protocol == "http" && serverPort == "80")
			|| (protocol == "https" && serverPort == "443"))
		{
			serverPort = null;
		}

		// added to fix issue reported by user running normal on port 80 but ssl on port 472
		if (protocol == "https" && serverPort == "80")
		{
			if (WebConfigSettings.MapAlternateSSLPort)
			{
				string alternatSSLPort = ConfigurationManager.AppSettings["AlternateSSLPort"];
				if (alternatSSLPort is not null)
				{
					serverPort = alternatSSLPort;
				}
			}
		}

		string host = serverName;

		if (serverPort is not null)
		{
			if (WebConfigSettings.MapAlternatePort)
			{
				host += $":{serverPort}";
			}
		}
		return host;
	}

	public static string GetSiteRoot()
	{
		string protocol = "http";
		if (HttpContext.Current.Request.ServerVariables["HTTPS"] == "on")
		{
			protocol += "s";
		}

		string host = GetHost(protocol);
		return $"{protocol}://{host}{GetApplicationRoot()}";
	}

	public static string GetHostName()
	{
		string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower();
		return serverName;
	}

	public static string GetSecureSiteRoot()
	{
		string protocol = "https";
		string host = GetHost(protocol);
		return $"{protocol}://{host}{GetApplicationRoot()}";
	}

	public static string GetVirtualRoot()
	{
		string serverName = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
		return $"/{serverName}{GetApplicationRoot()}";
	}

	public static void EnsureDirectory(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	private static void copyFiles(string source, string dest)
	{
		if (Directory.Exists(source))
		{
			var dirInfo = new DirectoryInfo(source);
			var files = dirInfo.GetFiles();

			foreach (FileInfo file in files)
			{
				if (!File.Exists(dest + file.Name))
				{
					File.Copy(file.FullName, dest + file.Name);
				}
			}
		}
	}
	#endregion
}