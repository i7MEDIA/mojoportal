// Author:					
// Created:				    2006-09-30
// Last Modified:		    2019-01-07
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{
    /// <summary>
    /// This is the setup page for initial installation and upgrades.
    /// It can create an initial site if none exists, run upgrade scripts for the core and features and configure 
    /// default settings or add new settings to features.
    /// </summary>
    public partial class SetupHome : Page
    {
        private static readonly ILog log 
            = LogManager.GetLogger(typeof(SetupHome));

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
        private Version dbCodeVersion;
        private Version dbSchemaVersion;

        
        protected void Page_Load(object sender, EventArgs e)
        {
            scriptTimeout = Server.ScriptTimeout;
            Response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            
            
            setupIsDisabled = WebConfigSettings.DisableSetup;

            Server.ScriptTimeout = int.MaxValue;
            startTime = DateTime.UtcNow;
            bool isAdmin = false;
            try
            {
                isAdmin = WebUser.IsAdmin;
            }
            catch { }

            WritePageHeader();

            if (setupIsDisabled && !isAdmin)
            {
                WritePageContent(SetupResource.SetupDisabledMessage);
            }
            else
            {
                if (setupIsDisabled && isAdmin)
                {
                    WritePageContent(SetupResource.RunningSetupForAdminUser);

                }
                
                if (LockForSetup())
                {
                    try
                    {
                        ProbeSystem();
                        RunSetup();

                        if (CoreSystemIsReady())
                        {
                            ShowSetupSuccess();
                        }
                    }
                    finally
                    {
                        ClearSetupLock();
                    }

                }
                else
                {
                    WritePageContent("Setup already in progress.");
                }

                WritePageContent(SetupResource.SetupEnabledMessage);


            }

            WritePageFooter();

            //restore Script timeout
            Server.ScriptTimeout = scriptTimeout;

        }

        private void RunSetup()
        {
            #region setup mojoportal-core

            if (!schemaHasBeenCreated)
            {
                if (canAlterSchema)
                {
                     
                    CreateInitialSchema("mojoportal-core");
                    schemaHasBeenCreated = DatabaseHelper.SchemaHasBeenCreated();
                    if (schemaHasBeenCreated)
                    {
                        //recheck
                        needSchemaUpgrade = mojoSetup.UpgradeIsNeeded();
                    }
                  
                } 
            }

            if (
                (schemaHasBeenCreated)
                && (needSchemaUpgrade)
                && (canAlterSchema)
                )
            {
                needSchemaUpgrade = UpgradeSchema("mojoportal-core");
           
            }

            if (!CoreSystemIsReady()) return;

            existingSiteCount = DatabaseHelper.ExistingSiteCount();
            if (existingSiteCount == 0)
            {
                CreateSiteAndAdminUser();
            }
            

            // look for new features or settings to install
            SetupFeatures("mojoportal-core");

            
            #endregion

            #region setup other applications

            // install other apps

            String pathToApplicationsFolder
                = HttpContext.Current.Server.MapPath(
                "~/Setup/applications/");

            if (!Directory.Exists(pathToApplicationsFolder))
            {
                WritePageContent(
                pathToApplicationsFolder 
                + " " + SetupResource.ScriptFolderNotFoundAddendum,
                false);

                return;
            }

            DirectoryInfo appRootFolder = new(pathToApplicationsFolder);

            DirectoryInfo[] appFolders = appRootFolder.GetDirectories();

            foreach (DirectoryInfo appFolder in appFolders)
            {
                if (!string.Equals(appFolder.Name,"mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateInitialSchema(appFolder.Name);
                    UpgradeSchema(appFolder.Name);
                    SetupFeatures(appFolder.Name);
                }
            }

            #endregion

            WritePageContent(SetupResource.EnsuringFeaturesInAdminSites, true);
            ModuleDefinition.EnsureInstallationInAdminSites();

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (siteSettings != null)
            {
                if (PageSettings.GetCountOfPages(siteSettings.SiteId) == 0)
                {
                    WritePageContent(SetupResource.CreatingDefaultContent);
                    //SetupContentPages(siteSettings);
                    mojoSetup.SetupDefaultContentPages(siteSettings);
                }

                try
                {
                    int userCount = SiteUser.UserCount(siteSettings.SiteId);
                    if (userCount == 0) { mojoSetup.EnsureRolesAndAdminUser(siteSettings); }
                    
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
            mojoSetup.EnsureAdditionalSiteFolders();

            // added 2013-10-18 
            if (WebConfigSettings.TryEnsureCustomMachineKeyOnSetup)
            {
                try
                {
                    WebConfigSettings.EnsureCustomMachineKey();
                }
                catch (Exception ex)
                {
                    log.Error("tried to ensure a custom machinekey in Web.config but an error occurred.", ex);
                }
            }

        }

        //private static void SyncDefinitions(object o)
        //{
        //    ModuleDefinition.SyncDefinitions();
        //}

        private bool CreateInitialSchema(string applicationName)
        {
            Guid appID = DatabaseHelper.GetApplicationId(applicationName);
            Version currentSchemaVersion = DatabaseHelper.GetSchemaVersion(appID);
            Version zeroVersion = new Version(0,0,0,0);

            if (currentSchemaVersion > zeroVersion) { return true; } //already installed only run upgrade scripts

            Version versionToStopAt = null;
            Guid mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            if (appID == mojoAppGuid)
            {
                versionToStopAt = DatabaseHelper.DBCodeVersion(); ;
            }

            
            var pathToScriptFolder = HttpContext.Current.Server.MapPath($"~/Setup/applications/{applicationName}/SchemaInstallScripts/{DatabaseHelper.DBPlatform().ToLowerInvariant()}/");

            if(!Directory.Exists(pathToScriptFolder))
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
                WritePageContent($"{pathToScriptFolder} {SetupResource.ScriptFolderNotFoundMessage}", false);

                return false;
            }

            DirectoryInfo directoryInfo = new(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");
            Array.Sort(scriptFiles, UIHelper.CompareFileNames);

            
            if (scriptFiles.Length == 0)
            {
                WritePageContent(
                SetupResource.NoScriptsFilesFoundMessage
                + " " + pathToScriptFolder,
                false);

                return false;

            }

            // We only want to run the highest version script from the /SchemaInstallationScripts/dbplatform folder
            // normally there is only 1 script in this folder, but if someone upgrades and then starts with a clean db
            // there can be more than one script because of the previous installs so we nned to make sure we only run the highest version found
            // since we sorted it the highest version is the last item in the array
            FileInfo scriptFile = scriptFiles[(scriptFiles.Length -1)];

            Version currentSchemaVersion
                = DatabaseHelper.GetSchemaVersion(applicationId);

            
            Version scriptVersion
                = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

            if (
                (scriptVersion != null)
                && (scriptVersion > currentSchemaVersion)
                && (versionToStopAt == null || (scriptVersion <= versionToStopAt))
                )
            {
                string message = string.Format(
                    SetupResource.RunningScriptMessage,
                    applicationName,
                    scriptFile.Name.Replace(".config", string.Empty));

                WritePageContent(
                    message,
                    true);

                string overrideConnectionString = GetOverrideConnectionString(applicationName);

                string errorMessage
                    = DatabaseHelper.RunScript(
                        applicationId,
                        scriptFile,
                        overrideConnectionString);

                if (errorMessage.Length > 0)
                {
                    WritePageContent(errorMessage, true);
                    return false;

                }

                if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                {
                    mojoSetup.DoPostScriptTasks(scriptVersion, null);
					if (scriptVersion >= Version.Parse("2.7.0.3"))
					{
						SiteSettings.UpdateSkinVersionGuidForAllSites();
						log.Info("Skin Version updated on all sites by Setup.");
					}
				}

                Version newVersion
                    = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

                if (
                    (applicationName != null)
                    && (newVersion != null)
                    )
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
            string overrideConnectionString = ConfigHelper.GetStringProperty(applicationName + "_ConnectionString", string.Empty);

            if (string.IsNullOrEmpty(overrideConnectionString)) { return null; }

            return overrideConnectionString;
        }

        private bool UpgradeSchema(string applicationName)
        {
            Guid appID = DatabaseHelper.GetApplicationId(applicationName);
            Version currentSchemaVersion = DatabaseHelper.GetSchemaVersion(appID);
            Version versionToStopAt = null;
            Guid mojoAppGuid = new Guid("077e4857-f583-488e-836e-34a4b04be855");
            if (appID == mojoAppGuid)
            {
                versionToStopAt = DatabaseHelper.DBCodeVersion(); ;
            }

            String pathToScriptFolder
                = HttpContext.Current.Server.MapPath(
                "~/Setup/applications/" + applicationName
                    + "/SchemaUpgradeScripts/"
                    + DatabaseHelper.DBPlatform().ToLowerInvariant()
                    + "/");

            if (!Directory.Exists(pathToScriptFolder))
            {
                //string warning = string.Format(
                //    SetupResource.SchemaUpgradeFolderNotFound,
                //    applicationName, pathToScriptFolder);

                //log.Warn(warning);

                //WritePageContent(warning);
                return false;

            }

            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");


            if (scriptFiles.Length == 0)
            {
                string warning = string.Format(
                    SetupResource.NoUpgradeScriptsFound,
                    applicationName);

                return false;

            }

            bool result = RunUpgradeScripts(
                appID,
                applicationName,
                pathToScriptFolder,
                versionToStopAt);

            return result;

        }

        private bool RunUpgradeScripts(
            Guid applicationId,
            string applicationName,
            string pathToScriptFolder,
            Version versionToStopAt)
        {
            bool result = true;

            if (!Directory.Exists(pathToScriptFolder))
            {
                WritePageContent(
                pathToScriptFolder + " " + SetupResource.ScriptFolderNotFoundMessage,
                false);

                return false;
            }
                
            DirectoryInfo directoryInfo
                = new DirectoryInfo(pathToScriptFolder);

            FileInfo[] scriptFiles = directoryInfo.GetFiles("*.config");
            Array.Sort(scriptFiles, UIHelper.CompareFileNames);

            if (scriptFiles.Length == 0)
            {
                //WritePageContent(
                //SetupResource.NoScriptsFilesFoundMessage 
                //+ " " + pathToScriptFolder,
                //false);

                return false;

            }

           
            Version currentSchemaVersion 
                = DatabaseHelper.GetSchemaVersion(applicationId);

            foreach (FileInfo scriptFile in scriptFiles)
            {
                Version scriptVersion
                    = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

                if (
                    (scriptVersion != null)
                    && (scriptVersion > currentSchemaVersion)
                    && (versionToStopAt == null ||(scriptVersion <= versionToStopAt))
                    // commented out 2007-08-26
                    // script is still logged if it fails but version
                    // isn't updated. This was blocking the script from
                    // running again unless user deleted row from mp_SchemaScriptHistory
                    //&& (!DatabaseHelper.SchemaScriptHasBeenRun(
                    //        applicationID, 
                    //        scriptFile.Name)
                    //    )
                    )
                {
                    string message = string.Format(
                        SetupResource.RunningScriptMessage,
                        applicationName,
                        scriptFile.Name.Replace(".config", string.Empty));

                    WritePageContent(
                        message,
                        true);

                    string overrideConnectionString = GetOverrideConnectionString(applicationName);

                    string errorMessage
                        = DatabaseHelper.RunScript(
                            applicationId, 
                            scriptFile,
                            overrideConnectionString);

                    if (errorMessage.Length > 0)
                    {
                        WritePageContent(errorMessage, true);
                        return false;

                    }
                    
                    if (string.Equals(applicationName, "mojoportal-core", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mojoSetup.DoPostScriptTasks(scriptVersion, null);
                    }

                    Version newVersion
                        = DatabaseHelper.ParseVersionFromFileName(scriptFile.Name);

                    if (
                        (applicationName != null)
                        && (newVersion != null)
                        )
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

        private void CreateSiteAndAdminUser()
        {
            WritePageContent(SetupResource.CreatingSiteMessage, true);
            SiteSettings newSite = mojoSetup.CreateNewSite();
            mojoSetup.CreateDefaultSiteFolders(newSite.SiteId);
            mojoSetup.CreateOrRestoreSiteSkins(newSite.SiteId);
            WritePageContent(SetupResource.CreatingRolesAndAdminUserMessage, true);
            mojoSetup.CreateRequiredRolesAndAdminUser(newSite);
            
        }

        

        private void SetupFeatures(string applicationName)
        {
            ContentFeatureConfiguration appFeatureConfig
                = ContentFeatureConfiguration.GetConfig(applicationName);

            //WritePageContent(
            //    string.Format(SetupResource.ConfigureFeaturesMessage, 
            //    applicationName));

            foreach (ContentFeature feature in appFeatureConfig.ContentFeatures)
            {
#if MONO
                Guid WebPartGuid = new Guid("437a19b0-ef57-4963-b311-a1e13d9d883c");
                if (feature.FeatureGuid != WebPartGuid)
#endif
                if (feature.SupportedDatabases.ToLower().Contains(dbPlatform.ToLower()))
                {
                    SetupFeature(feature);
                }

            }
        }

        private void SetupFeature(ContentFeature feature)
        {
            WritePageContent(
                    string.Format(SetupResource.ConfigureFeatureMessage,
                    ResourceHelper.GetResourceString(
                    feature.ResourceFile,
                    feature.FeatureNameReasourceKey))
                    , true);

            ModuleDefinition moduleDefinition = new ModuleDefinition(feature.FeatureGuid);
            moduleDefinition.ControlSrc = feature.ControlSource;
            moduleDefinition.DefaultCacheTime = feature.DefaultCacheTime;
            moduleDefinition.FeatureName = feature.FeatureNameReasourceKey;
            moduleDefinition.Icon = feature.Icon;
            moduleDefinition.IsAdmin = feature.ExcludeFromFeatureList;
            moduleDefinition.SortOrder = feature.SortOrder;
            moduleDefinition.ResourceFile = feature.ResourceFile;
            moduleDefinition.IsCacheable = feature.IsCacheable;
            moduleDefinition.IsSearchable = feature.IsSearchable;
            moduleDefinition.SearchListName = feature.SearchListNameResourceKey;
            moduleDefinition.SupportsPageReuse = feature.SupportsPageReuse;
            moduleDefinition.DeleteProvider = feature.DeleteProvider;
            moduleDefinition.PartialView = feature.PartialView;
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

        

        private void ShowSetupSuccess()
        {
            
            StringBuilder successMessage = new StringBuilder();
            successMessage.Append("<hr /><div>" + SetupResource.SetupSuccessMessage + "</div>");
            successMessage.Append("<a href='" + Page.ResolveUrl("~/")
                + "' title='" + SetupResource.HomeLink + "'>"
                + SetupResource.HomeLink + "</a>");

            successMessage.Append("<br /><br />");

            successMessage.Append("<div class='settingrow'>");
            successMessage.Append("<span class='settinglabel'>");
            successMessage.Append(SetupResource.DatabasePlatformLabel);
            successMessage.Append("</span>");
            successMessage.Append(DatabaseHelper.DBPlatform());
            successMessage.Append("</div>");

            if (schemaHasBeenCreated)
            {
                dbCodeVersion = DatabaseHelper.DBCodeVersion();
                dbSchemaVersion = DatabaseHelper.DBSchemaVersion();

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                successMessage.Append(SetupResource.VersionLabel);
                successMessage.Append("</span>");
                successMessage.Append(dbCodeVersion.ToString());
                successMessage.Append("</div>");

                successMessage.Append("<div class='settingrow'>");
                successMessage.Append("<span class='settinglabel'>");
                successMessage.Append(SetupResource.DatabaseStatusLabel);
                successMessage.Append("</span>");

                if (dbCodeVersion > dbSchemaVersion)
                {
                    successMessage.Append(SetupResource.SchemaUpgradeNeededMessage);
                }

                if (dbCodeVersion < dbSchemaVersion)
                {
                    successMessage.Append(SetupResource.CodeUpgradeNeededMessage);
                }

                if (dbCodeVersion == dbSchemaVersion)
                {
                    successMessage.Append(SetupResource.InstallationUpToDateMessage);

                }

                successMessage.Append("</div>");
            }

            WritePageContent(successMessage.ToString(), false);

        }


        private void WritePageContent(string message)
        {
            WritePageContent(message, false);
        }
        
        private void WritePageContent(string message, bool showTime)
        {
            
            if (showTime)
            {
                HttpContext.Current.Response.Write(
                    string.Format("{0} - {1}",
                    message,
                    DateTime.UtcNow.Subtract(startTime)));
            }
            else
            {
                HttpContext.Current.Response.Write(message);
            }
            HttpContext.Current.Response.Write("<br/>");
            HttpContext.Current.Response.Flush();
            
        }

        
        private void WritePageHeader()
        {
            if (HttpContext.Current == null) return;

            string setupTemplatePath = WebConfigSettings.SetupHeaderConfigPath;
            if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                setupTemplatePath = WebConfigSettings.SetupHeaderConfigPathRtl;
            }


            if (File.Exists(HttpContext.Current.Server.MapPath(setupTemplatePath)))
            {
                string sHtml = string.Empty;
                using (StreamReader oStreamReader = File.OpenText(System.Web.HttpContext.Current.Server.MapPath(setupTemplatePath)))
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
				using (StreamReader oStreamReader = File.OpenText(System.Web.HttpContext.Current.Server.MapPath(setupFooterTemplatePath)))
				{
					sHtml = oStreamReader.ReadToEnd();
				}
				Response.Write(sHtml);
			}
			else
			{
				Response.Write("</body>");
				Response.Write("</html>");
			}

            Response.Flush();
        }

        
        private void ProbeSystem()
        {
            WritePageContent(
                SetupResource.ProbingSystemMessage,
                false);

            dbPlatform = DatabaseHelper.DBPlatform();
            dataFolderIsWritable = mojoSetup.DataFolderIsWritable();

            //if (dbPlatform == "SqlCe")
            //{
            //    DatabaseHelper.EnsureDatabase();
            //}

            if (dataFolderIsWritable)
            {
                WritePageContent(
                    SetupResource.FileSystemPermissionsOKMesage,
                    false);
            }
            else
            {
                WritePageContent(
                    SetupResource.FileSystemPermissionProblemsMessage,
                    false);

                WritePageContent(
                    "<div>" + GetFolderDetailsHtml() + "</div>",
                    false);
            }

            canAccessDatabase = DatabaseHelper.CanAccessDatabase();

			/*  LUIS SILVA ForceDatabaseCreation for MS SQL 2012-06-13
				add this to user.config <add key="TryToCreateMsSqlDatabase" value="true"/> default is false
				and make sure the connection string is configured with a user that has permission to create the database			
            */
			if (!canAccessDatabase && dbPlatform == "MSSQL" && WebConfigSettings.TryToCreateMsSqlDatabase)
			{
				WritePageContent(dbPlatform + " " + SetupResource.TryingToCreateDatabase, false);
				DatabaseHelper.EnsureDatabase();
				canAccessDatabase = DatabaseHelper.CanAccessDatabase();
				if (canAccessDatabase)
				{
					WritePageContent(dbPlatform + " " + SetupResource.DatabaseCreationSucceeded, false);
				}
			}
            
            if (canAccessDatabase)
            {
                WritePageContent(
                    dbPlatform 
                    + " " + SetupResource.DatabaseConnectionOKMessage,
                    false);
            }
            else
            {
                string dbError = string.Format(
                    SetupResource.FailedToConnectToDatabase,
                    dbPlatform);

                WritePageContent("<div>" + dbError + "</div>", false);

                showConnectionError = ConfigHelper.GetBoolProperty("ShowConnectionErrorOnSetup", false);


                if (showConnectionError)
                {
                    WritePageContent(
                        "<div>" + DatabaseHelper.GetConnectionError(null).ToString()
                        + "</div>",
                        false);
                }
            }

            
            if (canAccessDatabase)
            {
                canAlterSchema = DatabaseHelper.CanAlterSchema(null);

                if (canAlterSchema)
                {
                    WritePageContent(
                        SetupResource.DatabaseCanAlterSchemaMessage,
                        false);
                }
                else
                {
                   
                    if (WebConfigSettings.SetupTryAnywayIfFailedAlterSchemaTest)
                    {
                        canAlterSchema = true;
                    }
                    else
                    {
                        WritePageContent(
                       "<div>" + SetupResource.CantAlterSchemaWarning
                       + "</div>",
                       false);
                    }
                }

                schemaHasBeenCreated = DatabaseHelper.SchemaHasBeenCreated();

                if (schemaHasBeenCreated)
                {
                    WritePageContent(
                        SetupResource.DatabaseSchemaAlreadyExistsMessage,
                        false);

                    
                    needSchemaUpgrade = mojoSetup.UpgradeIsNeeded();

                    if (needSchemaUpgrade)
                    {
                        WritePageContent(
                            SetupResource.DatabaseSchemaNeedsUpgradeMessage,
                            false);
                    }
                    else
                    {
                        WritePageContent(
                            SetupResource.DatabaseSchemaUpToDateMessage,
                            false);
                    }

                    existingSiteCount = DatabaseHelper.ExistingSiteCount();

                    WritePageContent(
                        string.Format(
                        SetupResource.ExistingSiteCountMessageMessage,
                        existingSiteCount.ToString()),
                        false);
                    
                }
                else
                {
                    WritePageContent(
                        SetupResource.DatabaseSchemaNotCreatedYetMessage,
                        false);
                }

            }
            
            if (!mojoSetup.RunningInFullTrust())
            {
                // inform of Medium trust configuration issues
                WritePageContent(
                    "<b>" + SetupResource.MediumTrustGeneralMessage + "</b><br />"
                    //+ GetDataAccessMediumTrustMessage() + "<br /><br />",
                    ,false);

            }
        }

        private bool CoreSystemIsReady()
        {
            bool result = true;

            if (!canAccessDatabase) return false;

            if (!DatabaseHelper.SchemaHasBeenCreated()) return false;

            if (mojoSetup.UpgradeIsNeeded()) return false;

            


            return result;
        }

        private bool LockForSetup()
        {
            if (Application["UpgradeInProgress"] != null)
            {
                bool upgradeInProgress = (bool)Application["UpgradeInProgress"];
                if (upgradeInProgress) return false;

            }

            Application["UpgradeInProgress"] = true;
            return true;
        }

        private void ClearSetupLock()
        {
            Application["UpgradeInProgress"] = false;
        }

        //private string GetDataAccessMediumTrustMessage()
        //{
        //    string message = string.Empty;
        //    string dbPlatform = DatabaseHelper.DBPlatform();
        //    switch (dbPlatform)
        //    {
        //        case "MySQL":
        //            message = SetupResource.MediumTrustMySQLMessage;
        //            break;

        //        case "pgsql":
        //            message = SetupResource.MediumTrustnpgsqlMessage;
        //            break;


        //    }

        //    return message;

        //}

        //private string GetLuceneMediumTrustMessage()
        //{
        //    string result = SetupResource.MediumTrustLuceneConfigPreambleMessage 
        //            + "<br /><br />"
        //            + Server.HtmlEncode(GetLuceneExampleMediumTrustConfig())
        //            + "<br /><br />";

        //    return result;

        //}

        //private string GetLuceneExampleMediumTrustConfig()
        //{
        //    string example = "<add key=\"Lucene.Net.lockdir\" value=\"" 
        //        + GetPathToIndexFolder() + "\" />";

        //    return example;

        //}

        private string GetPathToIndexFolder()
        {
            String result = Server.MapPath("~/Data/Sites/1/index");
            return result;

        }

        private string GetFolderDetailsHtml()
        {
            StringBuilder folderErrors = new StringBuilder();
            string crlf = "\r\n";
            folderErrors.Append(
                SetupResource.DataFolderNotWritableMessage.Replace(crlf, "<br />")
                + "<h3>" + SetupResource.FolderDetailsLabel + "</h3>");
            
            String pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataRootNotWritableMessage + "</li>");
            }

            pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataSiteFolderNotWritableMessage + "</li>");
            }

            pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/systemfiles/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataSystemFilesFolderNotWritableMessage + "</li>");
            }

            pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/index/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataSiteIndexFolderNotWritableMessage + "</li>");
            }

            //pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/GalleryImages/test.config");
            //try
            //{
            //    mojoSetup.TouchTestFile(pathToTestFile);
            //}
            //catch (UnauthorizedAccessException)
            //{
            //    folderErrors.Append("<li>" + SetupResource.DataGalleryFolderNotWritableMessage + "</li>");
            //}

            pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataSharedFilesFolderNotWritableMessage + "</li>");
            }

            pathToTestFile = HttpContext.Current.Server.MapPath("~/Data/Sites/1/SharedFiles/History/test.config");
            try
            {
                mojoSetup.TouchTestFile(pathToTestFile);
            }
            catch (UnauthorizedAccessException)
            {
                folderErrors.Append("<li>" + SetupResource.DataSharedFilesHistoryFolderNotWritableMessage + "</li>");

            }

            return folderErrors.ToString();

        }

        
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
}


