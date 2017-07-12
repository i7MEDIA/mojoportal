/// Author:             Joe Davis (i7MEDIA)
/// Created:			2015-11-01
///	Last Modified:		2017-03-07

///
/// You must not remove this notice, or any other, from this software.
/// Original code from mojoPortal.Web.ModuleWrapper
/// 

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.UI;
using Newtonsoft.Json.Linq;
namespace SuperFlexiUI
{
    public partial class SuperWrapper : SiteModuleControl
    {
        private bool hideOnAdminPages = false;
        public bool HideOnAdminPages
        {
            get { return hideOnAdminPages; }
            set { hideOnAdminPages = value; }
        }

        private bool hideOnNonCmsPages = false;
        public bool HideOnNonCmsPages
        {
            get { return hideOnNonCmsPages; }
            set { hideOnNonCmsPages = value; }
        }

        private Guid featureGuid = Guid.Empty;
        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        private string defaultGeneralSettings = string.Empty;
        public string DefaultGeneralSettings
        {
            get { return defaultGeneralSettings; }
            set { defaultGeneralSettings = value; }
        }

        private string defaultModuleSettings = string.Empty;
        public string DefaultModuleSettings
        {
            get { return defaultModuleSettings; }
            set { defaultModuleSettings = value; }
        }

        private int moduleID = -1;
        public int ModuleID
        {
            get { return this.moduleID;}
            set 
            {
                // this can throw an error when set during page pre-init
                try
                {
                    this.moduleID = value;
                    LoadModule();
                }
                catch (NullReferenceException) { }
            }
        }

        private Guid moduleGuidToUse = Guid.Empty;
        public Guid ModuleGuidToUse
        {
            get { return moduleGuidToUse; }
            set { moduleGuidToUse = value; }
        }

        // not used yet, need page level custom settings
        private bool uniquePerPage = false;
        public bool UniquePerPage { get => uniquePerPage; set => uniquePerPage = value; }
        

        private bool moduleLoaded = false;

        private static readonly ILog log = LogManager.GetLogger(typeof(SuperWrapper));
        protected void Page_Load(object sender, EventArgs e)
        {
            // don't render on admin or edit pages
            if ((!(this.Page is CmsPage)) && (hideOnAdminPages)) { this.Visible = false; return; }

            if ((hideOnNonCmsPages) && (Page is NonCmsBasePage)) { this.Visible = false; return; }

            Description = "Module Wrapper";

            if (ModuleID > -1 || moduleGuidToUse != Guid.Empty)
            {
                if (!moduleLoaded)
                {
                    LoadModule();
                }

            }
        } 

        protected void LoadModule()
        {

            if (siteSettings == null)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }

            if (moduleID > -1 || moduleGuidToUse != Guid.Empty)
            {
                this.Controls.Clear();

                Module module = null;

                if (moduleID > -1) module = new Module(ModuleID);

                if (module == null && moduleGuidToUse != Guid.Empty) module = new Module(moduleGuidToUse);

                if (module.ModuleId > -1)
                {
                    if (WebConfigSettings.EnforceSiteIdInModuleWrapper)
                    {
                        //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                        if (siteSettings != null)
                        {

                            //could add a web.config setting check here to determine if the module should still be loaded even if it is from a different site.
                            if (module.SiteId != siteSettings.SiteId)
                            {

                                //this will adjust the ModuleGuidToUse set on the control to have the SiteID for the first two characters.
                                //doing this gives us the ability to create/load module instances in multi-site installs using skins containing SuperWrapper
                                //with the same ModuleGuidToUse.
                                StringBuilder sb = new StringBuilder(moduleGuidToUse.ToString());

                                int siteIdLength = siteSettings.SiteId.ToString().Length;

                                // mojo can run more than 999 sites but the odds are not high that this would ever happen. 
                                // to keep the overhead down we'll just use some simple math here
                                sb.Remove(0, siteIdLength + 5);

                                sb.Insert(0, "00000" + siteSettings.SiteId.ToString());

                                moduleGuidToUse = Guid.Parse(sb.ToString());

                                LoadModule();

                                return;
                            }
                        }
                    }

                    Control c = Page.LoadControl("~/" + module.ControlSource);
                    if (c == null)
                    {
                        throw new ArgumentException("Unable to load control from ~/" + module.ControlSource);
                    }

                    if (c is SiteModuleControl)
                    {
                        SiteModuleControl siteModule = (SiteModuleControl)c;
                        siteModule.SiteId = siteSettings.SiteId;
                        siteModule.ModuleConfiguration = module;
                        this.Title = module.ModuleTitle;
                    }

                    this.Controls.Add(c);
                    moduleLoaded = true;
                }
                else
                {
                    CreateModule();
                    LoadModule();
                }
            }
        }

        private void CreateModule()
        {

            //first we'll create the module and then we'll set the default settings, if any exist.

            if (featureGuid != Guid.Empty && moduleGuidToUse != Guid.Empty)
            {
                ModuleDefinition moduleDefinition = new ModuleDefinition(featureGuid);

                if (moduleDefinition == null)
                {
                    log.Error("Cannot create module because featureGuid \"" + featureGuid.ToString() + "\" does not correspond to an installed feature.");
                    return;
                }

                Module module = new Module();
                module.ModuleGuid = moduleGuidToUse;
                module.ModuleDefId = moduleDefinition.ModuleDefId;
                module.FeatureGuid = moduleDefinition.FeatureGuid;
                module.Icon = moduleDefinition.Icon;
                module.SiteId = siteSettings.SiteId;
                module.SiteGuid = siteSettings.SiteGuid;

                //need to account for user not being logged in the first time the site is visited with this SuperWrapper in the skin.
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser == null)
                {
                    Role adminsRole = Role.GetRoleByName(siteSettings.SiteId, "Admins");
                    DataTable dtUsers = SiteUser.GetRoleMembers(adminsRole.RoleId);
                    if (dtUsers.Rows.Count > 0)
                    {
                        siteUser = SiteUser.GetByEmail(siteSettings, dtUsers.Rows[0]["Email"].ToString());
                    }

                    if (siteUser == null)
                    {
                        //Can't get a user to create the module.
                        return;
                    }
                }

                module.CreatedByUserId = siteUser.UserId;
                module.CacheTime = moduleDefinition.DefaultCacheTime;
                module.ShowTitle = WebConfigSettings.ShowModuleTitlesByDefault;
                module.HeadElement = WebConfigSettings.ModuleTitleTag;
                
                if (module.Save())
                {
                    Module newModule = new Module(moduleGuidToUse);
                    // set default module settings from json
                    if (!String.IsNullOrWhiteSpace(defaultModuleSettings))
                    {

                        defaultModuleSettings = defaultModuleSettings.Replace("$_SiteID_$", siteSettings.SiteId.ToString());

                        Hashtable moduleSettings = ModuleSettings.GetModuleSettings(newModule.ModuleId);


                        JObject oModuleSettings = new JObject();

                        try
                        {
                            oModuleSettings = JObject.Parse(defaultModuleSettings);
                        }
                        catch (Newtonsoft.Json.JsonReaderException)
                        {
                            log.Error(this.ID + " -- could not load defaultModuleSettings because of invalid json");
                        }

                        foreach (var prop in oModuleSettings)
                        {
                            if (moduleSettings.ContainsKey(prop.Key))
                            {
                                ModuleSettings.UpdateModuleSetting(newModule.ModuleGuid, newModule.ModuleId, prop.Key, prop.Value.ToString());
                            }
                        }
                    }

                    //set default general settings from json
                    if (!String.IsNullOrWhiteSpace(defaultGeneralSettings))
                    {

                        defaultGeneralSettings = defaultGeneralSettings.Replace("$_SiteID_$", siteSettings.SiteId.ToString());
                     
                        JObject oGeneralSettings = JObject.Parse(defaultGeneralSettings);

                        IList<System.Reflection.PropertyInfo> props = new List<System.Reflection.PropertyInfo>(newModule.GetType().GetProperties());

                        foreach(System.Reflection.PropertyInfo prop in props)
                        {
                            JProperty jProp = oGeneralSettings.Property(prop.Name);
                            
                            if (jProp != null)
                            {
                                Type propType;
                                switch (prop.PropertyType.Name)
                                {
                                    case "String":
                                    default:
                                        propType = "".GetType();
                                        break;
                                    case "Boolean":
                                        propType = true.GetType();
                                        break;
                                    case "Int32":
                                        propType = 1.GetType();
                                        break;
                                    case "Guid":
                                        propType = Guid.Empty.GetType();
                                        break;
                                }
                                prop.SetValue(newModule, Convert.ChangeType(jProp.Value, propType));
                            }
                        }

                        newModule.Save();

                    }
                }
            }
        }
    }
}
