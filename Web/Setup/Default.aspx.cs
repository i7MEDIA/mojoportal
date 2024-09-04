using System;
using System.CodeDom;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using Google.GData.Extensions.Apps;
using Ionic.Zip;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

/// <summary>
/// This is the setup page for initial installation and upgrades.
/// It can create an initial site if none exists, run upgrade scripts for the core and features and configure 
/// default settings or add new settings to features.
/// </summary>
public partial class SetupHome : Page
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SetupHome));

	private bool setupIsDisabled = false;
	private bool dataFolderIsWritable = false;
	private bool canAccessDatabase = false;
	private bool schemaHasBeenCreated = false;
	private bool canAlterSchema = false;
	private bool showConnectionError = false;
	private int existingSiteCount = 0;
	private bool needSchemaUpgrade = false;
	private int scriptTimeout;
	private DateTime startTime;
	private string dbPlatform = string.Empty;
	private Version appCodeVersion;
	private Version dbSchemaVersion;
	private const string successFormat = "<div class=\"ms-3 text-success\"><i class=\"bi bi-check2-circle\"></i> {0}</div>";
	private const string infoFormat = "<div class=\"ms-3 text-info\"><i class=\"bi bi-info-circle\"></i> {0}</div>";
	private const string warnFormat = "<div class=\"ms-3 text-warning\"><i class=\"bi bi-exclamation-triangle\"></i> {0}</div>";
	private const string errorFormat = "<div class=\"ms-3 text-danger\"><i class=\"bi bi-exclamation-octagon\"></i> {0}</div>";
	private const string noFormat = "<div class=\"ms-3\">{0}</div>";
	private const string sqlFormat = "<div class=\"ms-3\"><i class=\"bi bi-filetype-sql\"></i> {0}</div>";
	private bool machineKeyGenerated = false;

	protected void Page_Load(object sender, EventArgs e)
	{
		scriptTimeout = Server.ScriptTimeout;
		Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);

		setupIsDisabled = WebConfigSettings.DisableSetup;

		Server.ScriptTimeout = int.MaxValue;
		startTime = DateTime.UtcNow;

		//we won't have an admin on initial install
		bool isAdmin = false;
		try
		{
			isAdmin = WebUser.IsAdmin;
		}
		catch { }

		WritePageHeader();

		if (setupIsDisabled && !isAdmin)
		{
			WritePageContentCard(SetupStatus.Info, SetupResource.SetupDisabledMessage);
		}
		else
		{
			if (setupIsDisabled && isAdmin)
			{
				WritePageContent(SetupStatus.Info, SetupResource.RunningSetupForAdminUser);
			}

			if (LockForSetup())
			{
				try
				{
					ProbeSystem();

					ShowStatusMessage(RunSetup());
				}
				finally
				{
					ClearSetupLock();
				}
			}
			else
			{
				WritePageContentCard(SetupStatus.Info, SetupResource.SetupInProgressMessage, SetupResource.SetupInProgressTitle);
			}

			WritePageContent(SetupStatus.Warning, SetupResource.SetupEnabledMessage);
		}

		WritePageFooter();

		//restore Script timeout
		Server.ScriptTimeout = scriptTimeout;
	}

	private (SetupStatus status, string msg) RunSetup()
	{
		#region setup mojoportal-core

		if (!schemaHasBeenCreated)
		{
			if (canAlterSchema)
			{
				WritePageContent(SetupStatus.none, "<h3>Installing Core Schema</h3>");
				CreateInitialSchema("mojoportal-core");
				schemaHasBeenCreated = DatabaseHelper.SchemaHasBeenCreated();
				if (schemaHasBeenCreated)
				{
					//recheck
					needSchemaUpgrade = mojoSetup.UpgradeIsNeeded();
				}
			}
		}

		if (schemaHasBeenCreated && needSchemaUpgrade && canAlterSchema)
		{
			needSchemaUpgrade = UpgradeSchema("mojoportal-core");
		}

		var (status, msg) = CoreSystemIsReady();

		if (status != SetupStatus.Success)
		{
			return (status, msg);
		}

		SiteSettings siteSettings;

		//existingSiteCount = DatabaseHelper.ExistingSiteCount();
		if (DatabaseHelper.ExistingSiteCount() == 0)
		{
			siteSettings = CreateSite();
			CreateAdminUser(siteSettings);
		}
		else
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
		}

		if (siteSettings is not null)
		{
			//check for site folder in data, restore if it's missing
			//this helps during development
			string siteFolderPath = Invariant($"~/Data/Sites/{siteSettings.SiteId}/");


			if (HttpContext.Current is not null && !Directory.Exists(HttpContext.Current.Server.MapPath(siteFolderPath)))
			{
				mojoSetup.CreateDefaultSiteFolders(siteSettings.SiteId, true);
				//mojoSetup.EnsureSkins(siteSettings.SiteId);
			}
		}


		// look for new features or settings to install

		var featureConfigMarkup = $"<h3>{SetupResource.ConfigureFeaturesMessage}</h3>";
/*<ul class=""list-group"" style=""max-width: 75%;"">"; */
//<ul class=""list-group"" style=""max-width: 75%; width: 600px; white-space:nowrap; max-height: 400px; overflow: auto; border: 1px solid;"">";

		WritePageContent(SetupStatus.none, featureConfigMarkup);

		SetupFeatures("mojoportal-core");

		#endregion

		#region setup other applications

		// install other apps

		var pathToApplicationsFolder = HttpContext.Current.Server.MapPath("~/Setup/applications/");

		if (!Directory.Exists(pathToApplicationsFolder))
		{
			WritePageContent(SetupStatus.Error, $"{pathToApplicationsFolder} {SetupResource.ScriptFolderNotFoundAddendum}", false);
			return (SetupStatus.Error, "Setup/applications folder not found");
		}

		DirectoryInfo appRootFolder = new(pathToApplicationsFolder);
		DirectoryInfo[] appFolders = appRootFolder.GetDirectories();

		foreach (DirectoryInfo appFolder in appFolders)
		{
			if (!string.Equals(appFolder.Name, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
			{
				CreateInitialSchema(appFolder.Name);
				UpgradeSchema(appFolder.Name);
				SetupFeatures(appFolder.Name);
			}
		}

		#endregion
		var featureConfigMarkupEnd = "</ul>";
		WritePageContent(SetupStatus.none, featureConfigMarkupEnd);

		WritePageContent(SetupStatus.Info, SetupResource.EnsuringFeaturesInAdminSites);
		ModuleDefinition.EnsureInstallationInAdminSites();

		if (siteSettings is not null)
		{
			if (PageSettings.GetCountOfPages(siteSettings.SiteId) == 0)
			{
				WritePageContent(SetupStatus.Info, SetupResource.CreatingDefaultContent);
				mojoSetup.SetupDefaultContentPages(siteSettings);
			}

			try
			{
				if (SiteUser.UserCount(siteSettings.SiteId) == 0)
				{
					mojoSetup.EnsureRolesAndAdminUser(siteSettings);
				}
			}
			catch (Exception ex)
			{
				log.Error("EnsureAdminUserAndRoles", ex);
			}

			mojoSetup.EnsureSkins(siteSettings.SiteId);
		}

		// in case control type controlsrc, regex or sort changed on the definition
		// update instance properties to match
		//ThreadPool.QueueUserWorkItem(new WaitCallback(SyncDefinitions), null);
		//ModuleDefinition.SyncDefinitions();
		SiteSettings.EnsureExpandoSettings();

		if (WebConfigSettings.TryEnsureCustomMachineKeyOnSetup)
		{
			try
			{
				WebConfigSettings.EnsureCustomMachineKey();
				WritePageContent(SetupStatus.Success, SetupResource.CustomMachineKeyCreated);
				machineKeyGenerated = true;
			}
			catch (Exception ex)
			{
				log.Error("tried to ensure a custom machinekey in Web.config but an error occurred.", ex);
				WritePageContent(SetupStatus.Warning, SetupResource.CustomMachineKeyNotCreated);
			}
		}

		return (SetupStatus.Success, "setup complete");
	}


	private bool CreateInitialSchema(string applicationName)
	{
		Guid appID = DatabaseHelper.GetApplicationId(applicationName);
		var currentSchemaVersion = DatabaseHelper.GetSchemaVersion(appID);
		var zeroVersion = new Version(0, 0, 0, 0);

		if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

		Version versionToStopAt = null;
		var mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
		if (appID == mojoAppGuid)
		{
			versionToStopAt = DatabaseHelper.AppCodeVersion(); ;
		}

		var pathToScriptFolder = HttpContext.Current.Server.MapPath($"~/Setup/applications/{applicationName}/SchemaInstallScripts/{DatabaseHelper.DBPlatform().ToLowerInvariant()}/");

		if (!Directory.Exists(pathToScriptFolder))
		{
			//string warning = string.Format(
			//    SetupResource.CouldNotInstallFeatureSchemaMessage,
			//    applicationName, pathToScriptFolder);

			//WritePageContent(warning);
			return false;
		}

		return RunSetupScript(appID, applicationName, pathToScriptFolder, versionToStopAt);
	}

	private bool RunSetupScript(Guid applicationId, string applicationName, string pathToScriptFolder, Version versionToStopAt)
	{
		bool result = true;

		if (!Directory.Exists(pathToScriptFolder))
		{
			WritePageContent(SetupStatus.Error, $"{pathToScriptFolder} {SetupResource.ScriptFolderNotFoundMessage}");

			return false;
		}

		var directoryInfo = new DirectoryInfo(pathToScriptFolder);

		var scriptFiles = directoryInfo.GetFiles("*.config");

		Array.Sort(scriptFiles, UIHelper.CompareFileNames);

		if (scriptFiles.Length == 0)
		{
			WritePageContent(SetupStatus.Error, $"{SetupResource.NoScriptsFilesFoundMessage} {pathToScriptFolder}");

			return false;

		}

		// We only want to run the highest version script from the /SchemaInstallationScripts/dbplatform folder
		// normally there is only 1 script in this folder, but if someone upgrades and then starts with a clean db
		// there can be more than one script because of the previous installs so we nned to make sure we only run the highest version found
		// since we sorted it the highest version is the last item in the array
		FileInfo scriptFile = scriptFiles[(scriptFiles.Length - 1)];

		var currentSchemaVersion = DatabaseHelper.GetSchemaVersion(applicationId);
		var scriptVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

		if (
			scriptVersion is not null
			&& scriptVersion > currentSchemaVersion
			&& (versionToStopAt is null || scriptVersion <= versionToStopAt)
			)
		{
			var message = string.Format(SetupResource.InstallingFeature, applicationName, scriptFile.Name.Replace(".config", string.Empty));

			WritePageContent(SetupStatus.Info, message);

			string overrideConnectionString = GetOverrideConnectionString(applicationName);

			string errorMessage = DatabaseHelper.RunScript(applicationId, scriptFile, overrideConnectionString);

			if (!string.IsNullOrWhiteSpace(errorMessage))
			{
				WritePageContent(SetupStatus.Error, errorMessage, true);
				WritePageFooter();
				return false;
			}

			if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
			{
				mojoSetup.DoPostScriptTasks(scriptVersion, null);
				if (scriptVersion >= Version.Parse("2.7.0.3"))
				{
					SiteSettings.UpdateSkinVersionGuidForAllSites();
					WritePageContent(SetupStatus.Success, SetupResource.SkinVersionGuidUpdated);
					log.Debug("Skin Version updated on all sites by Setup.");
				}
			}

			var newVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

			if (applicationName is not null && newVersion is not null)
			{
				DatabaseHelper.UpdateSchemaVersion(
					applicationId,
					applicationName,
					newVersion.Major,
					newVersion.Minor,
					newVersion.Build,
					newVersion.Revision);

				DatabaseHelper.AddSchemaScriptHistory(
					applicationId,
					scriptFile.Name,
					DateTime.UtcNow,
					false,
					string.Empty,
					string.Empty);

				if (errorMessage.Length == 0)
				{
					currentSchemaVersion = newVersion;
				}
			}
		}

		return result;
	}

	private string GetOverrideConnectionString(string applicationName)
	{
		string overrideConnectionString = ConfigHelper.GetStringProperty($"{applicationName}_ConnectionString", string.Empty);

		if (string.IsNullOrWhiteSpace(overrideConnectionString))
		{
			return null;
		}

		return overrideConnectionString;
	}

	private bool UpgradeSchema(string applicationName)
	{
		var appID = DatabaseHelper.GetApplicationId(applicationName);
		//Version currentSchemaVersion = DatabaseHelper.GetSchemaVersion(appID);
		Version versionToStopAt = null;

		var mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");

		if (appID == mojoAppGuid)
		{
			versionToStopAt = DatabaseHelper.AppCodeVersion(); ;
		}

		var pathToScriptFolder = HttpContext.Current.Server.MapPath($"~/Setup/applications/{applicationName}/SchemaUpgradeScripts/{DatabaseHelper.DBPlatform().ToLowerInvariant()}/");

		if (!Directory.Exists(pathToScriptFolder))
		{
			//string warning = string.Format(
			//    SetupResource.SchemaUpgradeFolderNotFound,
			//    applicationName, pathToScriptFolder);

			//log.Warn(warning);

			//WritePageContent(warning);
			return false;
		}

		var directoryInfo = new DirectoryInfo(pathToScriptFolder);

		var scriptFiles = directoryInfo.GetFiles("*.config");

		if (scriptFiles.Length == 0)
		{
			log.WarnFormat(SetupResource.NoUpgradeScriptsFound, applicationName);
			return false;
		}

		bool result = RunUpgradeScripts(
			appID,
			applicationName,
			pathToScriptFolder,
			versionToStopAt);

		return result;
	}

	private bool RunUpgradeScripts(Guid applicationId, string applicationName, string pathToScriptFolder, Version versionToStopAt)
	{
		bool result = true;

		if (!Directory.Exists(pathToScriptFolder))
		{
			WritePageContent(SetupStatus.Error, $"{pathToScriptFolder} {SetupResource.ScriptFolderNotFoundMessage}");

			return false;
		}

		var directoryInfo = new DirectoryInfo(pathToScriptFolder);

		var scriptFiles = directoryInfo.GetFiles("*.config");
		Array.Sort(scriptFiles, UIHelper.CompareFileNames);

		if (scriptFiles.Length == 0)
		{
			//WritePageContent(
			//SetupResource.NoScriptsFilesFoundMessage 
			//+ " " + pathToScriptFolder,
			//false);

			return false;
		}

		var currentSchemaVersion = DatabaseHelper.GetSchemaVersion(applicationId);

		bool upgradeHeaderWritten = false;


		foreach (FileInfo scriptFile in scriptFiles)
		{
			var scriptVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

			if (scriptVersion != null
				&& scriptVersion > currentSchemaVersion
				&& (versionToStopAt is null || scriptVersion <= versionToStopAt)
				)
			{
				if (!upgradeHeaderWritten)
				{
					WritePageContent(SetupStatus.Info, string.Format(SetupResource.UpgradingFeature, applicationName));
				}
				upgradeHeaderWritten = true;

				//string message = string.Format(
				//	SetupResource.RunningScriptMessage,
				//	applicationName,
				//	scriptFile.Name.Replace(".config", string.Empty));

				string message = scriptFile.Name.Replace(".config", string.Empty);

				WritePageContent(SetupStatus.SqlInfo, message);

				string overrideConnectionString = GetOverrideConnectionString(applicationName);

				string errorMessage = DatabaseHelper.RunScript(applicationId, scriptFile, overrideConnectionString);

				if (errorMessage.Length > 0)
				{
					WritePageContent(SetupStatus.Error, errorMessage);
					return false;
				}

				if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
				{
					mojoSetup.DoPostScriptTasks(scriptVersion, null);
				}

				var newVersion = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

				if (applicationName is not null && newVersion is not null)
				{
					DatabaseHelper.UpdateSchemaVersion(
						applicationId,
						applicationName,
						newVersion.Major,
						newVersion.Minor,
						newVersion.Build,
						newVersion.Revision);

					DatabaseHelper.AddSchemaScriptHistory(
						applicationId,
						scriptFile.Name,
						DateTime.UtcNow,
						false,
						string.Empty,
						string.Empty);

					if (errorMessage.Length == 0)
					{
						currentSchemaVersion = newVersion;

						// here is where we need to update the new forum guid field, in non ms databases
						// this should execute right after the 2.2.0.2 forum script has runn
						if (applicationName == "forums")
						{
							if (currentSchemaVersion == new Version(2, 2, 0, 2))
							{
								try
								{
									DatabaseHelper.DoForumVersion2202PostUpgradeTasks(string.Empty);
								}
								catch (Exception ex)
								{
									log.Error(ex);
								}
							}

						}

						if (applicationName == "forums")
						{
							if (currentSchemaVersion == new Version(2, 2, 0, 3))
							{
								try
								{
									DatabaseHelper.DoForumVersion2203PostUpgradeTasks(string.Empty);
								}
								catch (Exception ex)
								{
									log.Error(ex);
								}
							}
						}
					}
				}
			}
		}

		return result;
	}


	private SiteSettings CreateSite()
	{
		WritePageContent(SetupStatus.Info, SetupResource.CreatingSiteMessage);
		SiteSettings newSite = mojoSetup.CreateNewSite();
		mojoSetup.CreateDefaultSiteFolders(newSite.SiteId);
		mojoSetup.EnsureSkins(newSite.SiteId);
		return newSite;
	}


	private void CreateAdminUser(SiteSettings newSite)
	{
		WritePageContent(SetupStatus.Info, SetupResource.CreatingRolesAndAdminUserMessage);
		mojoSetup.EnsureRolesAndAdminUser(newSite);
	}


	private void SetupFeatures(string applicationName)
	{
		var appFeatureConfig = ContentFeatureConfiguration.GetConfig(applicationName);

		foreach (ContentFeature feature in appFeatureConfig.ContentFeatures)
		{
			if (feature.SupportedDatabases.ToLower().Contains(dbPlatform.ToLower()))
			{
				SetupFeature(feature);
			}
		}
	}


	private void SetupFeature(ContentFeature feature)
	{
//		var html = @$"
//<li class=""list-group-item d-flex justify-content-between align-items-top"">
//{string.Format(successFormat, ResourceHelper.GetResourceString(feature.ResourceFile, feature.FeatureNameReasourceKey))} 
//</li>
//";
		WritePageContent(SetupStatus.Success, string.Format(SetupResource.ConfigureFeatureMessage, ResourceHelper.GetResourceString(feature.ResourceFile, feature.FeatureNameReasourceKey)));

		var moduleDefinition = new ModuleDefinition(feature.FeatureGuid)
		{
			ControlSrc = feature.ControlSource,
			DefaultCacheTime = feature.DefaultCacheTime,
			FeatureName = feature.FeatureNameReasourceKey,
			Icon = feature.Icon,
			IsAdmin = feature.ExcludeFromFeatureList,
			SortOrder = feature.SortOrder,
			ResourceFile = feature.ResourceFile,
			IsCacheable = feature.IsCacheable,
			IsSearchable = feature.IsSearchable,
			SearchListName = feature.SearchListNameResourceKey,
			SupportsPageReuse = feature.SupportsPageReuse,
			DeleteProvider = feature.DeleteProvider,
			PartialView = feature.PartialView
		};
		moduleDefinition.Save();

		foreach (ContentFeatureSetting featureSetting in feature.Settings)
		{
			ModuleDefinition.UpdateModuleDefinitionSetting(
				moduleDefinition.FeatureGuid,
				moduleDefinition.ModuleDefId,
				featureSetting.ResourceFile,
				featureSetting.GroupNameKey,
				featureSetting.ResourceKey,
				featureSetting.DefaultValue,
				featureSetting.ControlType,
				featureSetting.RegexValidationExpression,
				featureSetting.ControlSrc,
				featureSetting.HelpKey,
				featureSetting.SortOrder,
				featureSetting.Attributes,
				featureSetting.Options);
		}
	}

	private void ShowStatusMessage((SetupStatus status, string msg) setup)
	{
		if (setup.status != SetupStatus.Success)
		{
			WritePageContentCard(setup.status, setup.msg);
			return;
		}

		var schemaInfo = string.Empty;
		if (schemaHasBeenCreated)
		{
			appCodeVersion = DatabaseHelper.AppCodeVersion();
			dbSchemaVersion = DatabaseHelper.SchemaVersion();
			var statusMessage = string.Empty;
			var status = SetupStatus.Success;

			if (appCodeVersion > dbSchemaVersion)
			{
				statusMessage = SetupResource.SchemaUpgradeNeededMessage;
				status = SetupStatus.Warning;
			}

			if (appCodeVersion < dbSchemaVersion)
			{
				statusMessage = SetupResource.CodeUpgradeNeededMessage;
				status = SetupStatus.Warning;
			}

			if (appCodeVersion == dbSchemaVersion)
			{
				statusMessage = SetupResource.InstallationUpToDateMessage;
			}

			WritePageContentCard(status, statusMessage);
		}
	}


	private void WritePageContentCard(SetupStatus setupStatus, string message, string title = "")
	{
		var cssClass = string.Empty;
		//if (string.IsNullOrWhiteSpace(title))
		//{
		//	title = string.Empty;
		//}
		var msg = $"\r\n<p>{message}</p>";
		switch (setupStatus)
		{
			case SetupStatus.Success:
				cssClass = "success";
				title.Coalesce(SetupResource.SetupSuccessTitle);
				msg = $"\r\n<p>{string.Format(SetupResource.SetupSuccessMessage, Page.ResolveUrl("~/"))}</p>";

				if (machineKeyGenerated)
				{
					var key = ConfigHelper.GetMachineKeySection();
					var keyXml = @$"<machineKey 
	validationKey=""{key.ValidationKey}"" 
	decryptionKey=""{key.DecryptionKey}"" 
	validation=""{key.Validation}"" 
	decryption=""{key.Decryption}"" 
/>";
					msg += @$"
<div class=""alert alert-info""><p class=""lead""><strong>{SetupResource.CustomMachineKeyCreated}</strong> {SetupResource.CustomMachineKeyCreatedDetails}</p>
<pre><code>{Server.HtmlEncode(keyXml)}</code></pre></div>
";
				}
				else
				{
					var securityAdvisor = new SecurityAdvisor();
					if (!securityAdvisor.UsingCustomMachineKey())
					{
						msg += @$"
<div class=""alert alert-danger""><p class=""lead""><strong>{SetupResource.CustomMachineKeyNotCreated}</strong> {Resource.SecurityAdvisorMachineKeyWrong}</p>
<pre><code>{Server.HtmlEncode(SiteUtils.GenerateRandomMachineKeyXml())}</code></pre></div>";
					}
				}

				break;

			case SetupStatus.Info:
			default:
				cssClass = "info";
				title.Coalesce(SetupResource.InfoTitle);
				break;

			case SetupStatus.Warning:
				cssClass = "warning";
				title.Coalesce(SetupResource.WarningTitle);
				break;

			case SetupStatus.Error:
				cssClass = "danger";
				title.Coalesce(SetupResource.ErrorLabel);
				break;
		}

		var html = @$"
<hr />
<div class=""card border-{cssClass}"">
	<h3 class=""card-header text-bg-{cssClass}"">{title}</h3>
	<div class=""card-body"">{msg}
		<dl class=""row mb-0"">
			<dt class=""col-sm-3"">{SetupResource.DatabasePlatformLabel}</dt><dd class=""col-sm-9"">{DatabaseHelper.DBPlatform()}</dd>
			<dt class=""col-sm-3"">{SetupResource.SchemaVersionLabel}</dt><dd class=""col-sm-9"">{DatabaseHelper.SchemaVersion()}</dd>
			<dt class=""col-sm-3"">{SetupResource.AppCodeVersionLabel}</dt><dd class=""col-sm-9"">{DatabaseHelper.AppCodeVersion()}</dd>
			<dt class=""col-sm-3"">{SetupResource.MessageLabel}</dt><dd class=""col-sm-9"">{message}</dd>
		</dl>
	</div>
</div>";

		WritePageContent(SetupStatus.none, html);
	}


	private void WritePageContent(SetupStatus setupStatus, string message, bool showTime = false)
	{

		string format = setupStatus switch
		{
			SetupStatus.Success => successFormat,
			SetupStatus.Warning => warnFormat,
			SetupStatus.Error => errorFormat,
			SetupStatus.SqlInfo => sqlFormat,
			SetupStatus.none => "{0}",
			_ => infoFormat,
		};

		string time = string.Empty;

		//we don't really need this, never had a need for it, maybe someone will complain so we'll leave it here 
		//if (showTime)
		//{
		//	time = $" - {DateTime.UtcNow.Subtract(startTime)}";
		//}

		HttpContext.Current.Response.Write(string.Format(format, message + time));

		HttpContext.Current.Response.Flush();
	}


	private void WritePageHeader()
	{
		if (HttpContext.Current == null)
		{
			return;
		}

		string setupTemplatePath = WebConfigSettings.SetupHeaderConfigPath;
		if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
		{
			setupTemplatePath = WebConfigSettings.SetupHeaderConfigPathRtl;
		}

		if (File.Exists(HttpContext.Current.Server.MapPath(setupTemplatePath)))
		{
			string sHtml = string.Empty;
			using (StreamReader oStreamReader = File.OpenText(HttpContext.Current.Server.MapPath(setupTemplatePath)))
			{
				sHtml = oStreamReader.ReadToEnd();
			}
			Response.Write(sHtml);
		}

		Response.Flush();
	}

	private void WritePageFooter()
	{
		string setupFooterTemplatePath = WebConfigSettings.SetupFooterConfigPath;

		if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
		{
			setupFooterTemplatePath = WebConfigSettings.SetupFooterConfigPathRtl;
		}

		if (File.Exists(HttpContext.Current.Server.MapPath(setupFooterTemplatePath)))
		{
			string sHtml = string.Empty;
			using (StreamReader oStreamReader = File.OpenText(HttpContext.Current.Server.MapPath(setupFooterTemplatePath)))
			{
				sHtml = oStreamReader.ReadToEnd();
			}
			Response.Write(sHtml);
		}
		else
		{
			Response.Write(@"
	</body>
</html>");
		}

		Response.Flush();
	}


	private void ProbeSystem()
	{
		WritePageContent(SetupStatus.none, $"<h3>{SetupResource.ProbingSystemMessage}</h3>");

		dbPlatform = DatabaseHelper.DBPlatform();
		dataFolderIsWritable = mojoSetup.DataFolderIsWritable();

		if (dataFolderIsWritable)
		{
			WritePageContent(SetupStatus.Success, SetupResource.FileSystemPermissionsOKMesage);
		}
		else
		{
			WritePageContentCard(SetupStatus.Error, SetupResource.DataFolderNotWritableMessage);
			//WritePageContent(SetupResource.FileSystemPermissionProblemsMessage, false);

			//WritePageContent(@$"<div>{SetupResource.DataFolderNotWritableMessage.Replace("\r\n", " <br />")}</div>", false);
		}

		canAccessDatabase = DatabaseHelper.CanAccessDatabase();

		/*  LUIS SILVA ForceDatabaseCreation for MS SQL 2012-06-13
			add this to user.config <add key="TryToCreateMsSqlDatabase" value="true"/> default is false
			and make sure the connection string is configured with a user that has permission to create the database			
	*/
		if (!canAccessDatabase && dbPlatform == "MSSQL" && WebConfigSettings.TryToCreateMsSqlDatabase)
		{
			WritePageContent(SetupStatus.Info, $"{dbPlatform} {SetupResource.TryingToCreateDatabase}");
			DatabaseHelper.EnsureDatabase();
			canAccessDatabase = DatabaseHelper.CanAccessDatabase();
			if (canAccessDatabase)
			{
				WritePageContent(SetupStatus.Success, $"{dbPlatform} {SetupResource.DatabaseCreationSucceeded}");
			}
		}

		if (canAccessDatabase)
		{
			WritePageContent(SetupStatus.Success, $"{dbPlatform} {SetupResource.DatabaseConnectionOKMessage}");

			canAlterSchema = DatabaseHelper.CanAlterSchema(null);

			if (canAlterSchema)
			{
				WritePageContent(SetupStatus.Success, SetupResource.DatabaseCanAlterSchemaMessage);
			}
			else
			{

				if (WebConfigSettings.SetupTryAnywayIfFailedAlterSchemaTest)
				{
					canAlterSchema = true;
				}
				else
				{
					WritePageContentCard(SetupStatus.Error, SetupResource.CantAlterSchemaWarning);
				}
			}

			schemaHasBeenCreated = DatabaseHelper.SchemaHasBeenCreated();

			if (schemaHasBeenCreated)
			{
				WritePageContent(SetupStatus.Success, SetupResource.DatabaseSchemaAlreadyExistsMessage);

				needSchemaUpgrade = mojoSetup.UpgradeIsNeeded();

				if (needSchemaUpgrade)
				{
					WritePageContent(SetupStatus.Warning, SetupResource.DatabaseSchemaNeedsUpgradeMessage);
				}
				else
				{
					WritePageContent(SetupStatus.Success, SetupResource.DatabaseSchemaUpToDateMessage);
				}

				existingSiteCount = DatabaseHelper.ExistingSiteCount();

				WritePageContent(SetupStatus.Info, string.Format(SetupResource.ExistingSiteCountMessage, existingSiteCount.ToString()));
			}
			else
			{
				WritePageContent(SetupStatus.Warning, SetupResource.DatabaseSchemaNotCreatedYetMessage);
			}
		}
		else
		{
			string dbError = string.Format(SetupResource.FailedToConnectToDatabase, dbPlatform);

			if (WebConfigSettings.ShowConnectionErrorOnSetup)
			{
				dbError += $"<div>{DatabaseHelper.GetConnectionError(null)}</div>";
			}

			WritePageContentCard(SetupStatus.Error, dbError);
		}

	}

	private (SetupStatus status, string msg) CoreSystemIsReady()
	{
		bool result = true;

		if (!canAccessDatabase)
		{
			return (SetupStatus.Error, "cannotAccessDatabase");
		}

		if (!DatabaseHelper.SchemaHasBeenCreated())
		{
			return (SetupStatus.Error, "schemaNotCreated");
		}

		if (mojoSetup.UpgradeIsNeeded())
		{
			return (SetupStatus.Info, "upgradeNeeded");
		}

		return (SetupStatus.Success, "ready");
	}

	private bool LockForSetup()
	{
		if (Application["UpgradeInProgress"] != null)
		{
			bool upgradeInProgress = (bool)Application["UpgradeInProgress"];
			if (upgradeInProgress)
			{
				return false;
			}
		}

		Application["UpgradeInProgress"] = true;
		return true;
	}

	private void ClearSetupLock()
	{
		Application["UpgradeInProgress"] = false;
	}

	private string GetPathToIndexFolder()
	{
		String result = Server.MapPath("~/Data/Sites/1/index");
		return result;
	}

	//JMD: I don't like how this is done, I want this configurable and this is not entirely necessary anyway because we always recommend the entire /Data directory is writable
	//private string GetFolderDetailsHtml()
	//{
	//	var folderErrors = new StringBuilder();
	//	string crlf = "\r\n";
	//	folderErrors.Append($"{SetupResource.DataFolderNotWritableMessage.Replace(crlf, "<br />")}<h3>{SetupResource.FolderDetailsLabel}</h3>");

	//	var pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataRootNotWritableMessage}</li>");
	//	}

	//	pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataSiteFolderNotWritableMessage}</li>");
	//	}

	//	pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataSystemFilesFolderNotWritableMessage}</li>");
	//	}

	//	pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataSiteIndexFolderNotWritableMessage}</li>");
	//	}

	//	pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataSharedFilesFolderNotWritableMessage}</li>");
	//	}

	//	pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/test.config");
	//	try
	//	{
	//		mojoSetup.TouchTestFile(pathToTestFile);
	//	}
	//	catch (UnauthorizedAccessException)
	//	{
	//		folderErrors.Append($"<li>{SetupResource.DataSharedFilesHistoryFolderNotWritableMessage}</li>");

	//	}

	//	return folderErrors.ToString();
	//}


	void SetupHome_Error(object sender, EventArgs e)
	{
		Exception rawException = Server.GetLastError();
		Server.ClearError();
		Response.Clear();
		Response.Write(UIHelper.BuildHtmlErrorPage(rawException));
		Response.End();
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(Page_Load);
		this.Error += new EventHandler(SetupHome_Error);
	}
}

public enum SetupStatus
{
	Success = 0,
	Info = 1,
	Warning = 2,
	Error = 3,
	SqlInfo = 4,
	none = 99
}