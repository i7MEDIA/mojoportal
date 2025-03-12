using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web;

public sealed class mojoSetup
{
	private static readonly ILog log = LogManager.GetLogger(typeof(mojoSetup));

	public const string DefaultPageEncoding = "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />";
	private static readonly char pathSep = Path.DirectorySeparatorChar;
	private const string testFilename = "test.config";

	#region File System Tests

	public static bool DataFolderIsWritable()
	{
		try
		{
			if (!WebConfigSettings.FileSystemIsWritable)
			{
				return false;
			}

			if (HttpContext.Current is not null)
			{
				TouchTestFile(HttpContext.Current.Server.MapPath($"~/data/{testFilename}"), false);
				return true;
			}
			return false;

		}
		catch (Exception ex)
		{
			log.Fatal($"File System is not writable, error was {ex}");
			return false;
		}
	}

	/// <summary>
	/// Ensures path is writable
	/// </summary>
	/// <param name="pathToFile"></param>
	/// <param name="deleteFile">Deletes the file after ensuring we can write to it.</param>
	public static void TouchTestFile(string pathToFile, bool deleteFile)
	{
		if (!WebConfigSettings.FileSystemIsWritable)
		{
			return;
		}

		if (pathToFile is not null)
		{
			if (pathToFile.StartsWith("~/"))
			{
				pathToFile = HttpContext.Current.Server.MapPath(pathToFile);
			}

			var fileInfo = new FileInfo(pathToFile);
			if (File.Exists(pathToFile))
			{
				File.SetLastWriteTimeUtc(pathToFile, DateTime.UtcNow);
				if (deleteFile)
				{
					fileInfo.Delete();
				}
			}
			else
			{
				if (!Directory.Exists(fileInfo.DirectoryName))
				{
					Directory.CreateDirectory(fileInfo.DirectoryName);
				}

				StreamWriter streamWriter = File.CreateText(pathToFile);
				streamWriter.Close();

				if (deleteFile)
				{
					fileInfo.Delete();
				}
			}
		}
	}

	//public static void EnsureFolderGalleryFolder(SiteSettings siteSettings)
	//{
	//	if (HttpContext.Current is null)
	//	{
	//		return;
	//	}

	//	string path = Invariant($"{sitesPath}{siteSettings.SiteId}/FolderGalleries/");
	//	TouchTestFile(path, true);
	//}

	#endregion

	#region System Probing

	public static bool UpgradeIsNeeded()
	{
		bool result = false;

		Version dbCodeVersion = DatabaseHelper.AppCodeVersion();
		Version dbSchemaVersion = DatabaseHelper.SchemaVersion();
		if (dbCodeVersion > dbSchemaVersion) result = true;

		return result;
	}

	#endregion

	#region Schema Upgrade methods

	public static void DoSchemaUpgrade(string overrideConnectionInfo)
	{
		log.Info("mojoSetup entered DoSchemaUpgrade");
		bool canAlterSchema = DatabaseHelper.CanAlterSchema(overrideConnectionInfo);

		if (HttpContext.Current is not null && canAlterSchema)
		{
			Version currentSchemaVersion = DatabaseHelper.SchemaVersion();

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
			UseSslOnAllPages = WebConfigSettings.SslisAvailable,
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
			Email = string.Format(AppConfig.DefaultAdminUserEmailFormat, siteDifferentiator),
			Name = "Admin",
			LoginName = string.Format(AppConfig.DefaultAdminUsernameFormat, siteDifferentiator)
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
			adminUser.Password = AppConfig.DefaultAdminPassword;

			if (Membership.Provider is mojoMembershipProvider membership)
			{
				adminUser.Password = membership.EncodePassword(site, adminUser, AppConfig.DefaultAdminPassword);
			}

			adminUser.PasswordQuestion = AppConfig.DefaultAdminSecurityQuestion;
			adminUser.PasswordAnswer = AppConfig.DefaultAdminSecurityAnswer;
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
		if (role.RoleId != -1)
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

	#region Create New Site Data

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
					// This must stay as it is
					RealUrl = Invariant($"~/Default.aspx?pageid={pageSettings.PageId}"),
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

	public static bool CreateDefaultSiteFolders(int siteId, bool copyBaseFiles)
	{
		if (HttpContext.Current is null)
		{
			return false;
		}

		string siteFolderPath = Invariant($"~/Data/Sites/{siteId}/");

		var directories = new List<SystemRequiredDirectory>
		{
			new() {Path = "~/Data/"},
			new() {Path = siteFolderPath},
			new() {Path = $"{siteFolderPath}SharedFiles/"},
			new() {Path = $"{siteFolderPath}SharedFiles/"},
			new() {Path = $"{siteFolderPath}SharedFiles/History/" },
			new() {Path = $"{siteFolderPath}userfiles/"},
			new() {Path = $"{siteFolderPath}skins/", DeleteTestFile = true },
			new() {Path = $"{siteFolderPath}media/", DeleteTestFile = true},
			new() {Path = $"{siteFolderPath}htmltemplateimages/", BaseFilesPath = "~/Data/htmltemplateimages", DeleteTestFile = true},

			new() {Path = $"{siteFolderPath}systemfiles/", Condition = WebConfigSettings.UseCacheDependencyFiles},
			new() {Path = $"{siteFolderPath}index/", Condition = !WebConfigSettings.DisableSearchIndex, DeleteTestFile = true},

			new() {Path = $"{siteFolderPath}media/logos/", BaseFilesPath = "~/Data/logos", Condition = WebConfigSettings.SiteLogoUseMediaFolder, DeleteTestFile = true},
			new() {Path = $"{siteFolderPath}logos/", BaseFilesPath = "~/Data/logos", Condition = !WebConfigSettings.SiteLogoUseMediaFolder, DeleteTestFile = true},

			new() {Path = $"{siteFolderPath}media/GalleryImages/", Condition = WebConfigSettings.ImageGalleryUseMediaFolder},
			new() {Path = $"{siteFolderPath}GalleryImages/", Condition = !WebConfigSettings.ImageGalleryUseMediaFolder},

			new() {Path = $"{siteFolderPath}media/FolderGalleries/", Condition = WebConfigSettings.ImageGalleryUseMediaFolder},
			new() {Path = $"{siteFolderPath}FolderGalleries/", Condition = !WebConfigSettings.ImageGalleryUseMediaFolder},

			new() {Path = $"{siteFolderPath}media/htmlfragments/", BaseFilesPath = "~/Data/htmlfragments", Condition = WebConfigSettings.HtmlFragmentUseMediaFolder},
			new() {Path = $"{siteFolderPath}htmlfragments/", BaseFilesPath = "~/Data/htmlfragments", Condition = !WebConfigSettings.HtmlFragmentUseMediaFolder},

			new() {Path = $"{siteFolderPath}media/xml/", BaseFilesPath = "~/Data/xml", Condition = WebConfigSettings.XmlUseMediaFolder},
			new() {Path = $"{siteFolderPath}xml/", BaseFilesPath = "~/Data/xml", Condition = !WebConfigSettings.XmlUseMediaFolder},

			new() {Path = $"{siteFolderPath}media/xsl/", BaseFilesPath = "~/Data/xsl", Condition = WebConfigSettings.XmlUseMediaFolder},
			new() {Path = $"{siteFolderPath}xsl/", BaseFilesPath = "~/Data/xsl", Condition = !WebConfigSettings.XmlUseMediaFolder},
		};

		foreach (var dir in directories)
		{
			if (dir.Condition)
			{
				TouchTestFile(dir.Path + dir.TestFile, dir.DeleteTestFile);
				if (copyBaseFiles)
				{
					if (Directory.Exists(dir.BaseFilesPath))
					{
						var source = new DirectoryInfo(dir.BaseFilesPath);
						var dest = new DirectoryInfo(dir.Path);
						recursiveCopy(source, dest, false);
					}
				}
			}
		}

		return true;
	}

	public static bool EnsureSkins(int siteId, bool restore = false, bool copyNew = false)
	{
		if (HttpContext.Current is null)
		{
			return false;
		}

		string siteSkinsPath = HttpContext.Current.Server.MapPath(Invariant($"{GetApplicationRoot()}{pathSep}Data{pathSep}Sites{pathSep}{siteId}{pathSep}skins"));
		string sourceFilesPath = HttpContext.Current.Server.MapPath($"{GetApplicationRoot()}{pathSep}Data{pathSep}skins");
		DirectoryInfo srcDir;
		DirectoryInfo destDir;

		if (Directory.Exists(sourceFilesPath))
		{
			if (restore
				|| copyNew
				|| !Directory.Exists(siteSkinsPath)
				|| Directory.EnumerateDirectories(siteSkinsPath).Count() == 0)
			{
				srcDir = new DirectoryInfo(sourceFilesPath);
				destDir = new DirectoryInfo(siteSkinsPath);

				recursiveCopy(srcDir, destDir, copyNew);
			}
		}

		return true;
	}

	private static void recursiveCopy(DirectoryInfo source, DirectoryInfo target, bool copyNewOnly = false)
	{
		if (!source.Name.StartsWith(".")) // Make sure to not copy .git, .svn, .vs, etc, folders from the Data/Sites/skins folder
		{
			foreach (DirectoryInfo dir in source.GetDirectories())
			{
				if (!dir.Name.StartsWith(".")) // Make sure to not copy .git, .svn, .vs, etc, folders from the current skin folder
				{
					// if we're copying new folders only (usually skins) we want to check if the dir exists in the target. If it does, we want to 
					// skip this dir because we only want to copy new folders.
					if (copyNewOnly)
					{
						var targetDirChild = new DirectoryInfo(target.FullName + pathSep + dir.Name);
						if (targetDirChild.Exists)
						{
							continue;
						}
					}

					recursiveCopy(dir, target.CreateSubdirectory(dir.Name), copyNewOnly);
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

	#endregion
}

public class SystemRequiredDirectory
{
	public string Path { get; set; }
	public string TestFile { get; set; } = "test.config";
	public bool DeleteTestFile { get; set; } = false;
	public bool Condition { get; set; } = true;
	/// <summary>
	/// Path of initial or default files to be copied when creating new sites.
	/// </summary>
	public string BaseFilesPath { get; set; }
}