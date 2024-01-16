using System;
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

namespace SuperFlexiUI;

public partial class SuperWrapper : SiteModuleControl
{
	public bool HideOnAdminPages { get; set; } = false;

	public bool HideOnNonCmsPages { get; set; } = false;

	public Guid FeatureGuid { get; set; } = Guid.Empty;

	public string DefaultGeneralSettings { get; set; } = string.Empty;

	public string DefaultModuleSettings { get; set; } = string.Empty;

	private int _moduleID = -1;
	public int ModuleID
	{
		get { return _moduleID; }
		set
		{
			// this can throw an error when set during page pre-init
			try
			{
				_moduleID = value;
				LoadModule();
			}
			catch (NullReferenceException) { }
		}
	}

	public Guid ModuleGuidToUse { get; set; } = Guid.Empty;

	public bool UniquePerPage { get; set; } = false;

	private bool moduleLoaded = false;

	private static readonly ILog log = LogManager.GetLogger(typeof(SuperWrapper));
	protected void Page_Load(object sender, EventArgs e)
	{
		// don't render on admin or edit pages
		if ((Page is not CmsPage && HideOnAdminPages)
			|| (Page is NonCmsBasePage && HideOnNonCmsPages))
		{
			Visible = false; return;
		}

		Description = "Module Wrapper";

		if (ModuleID > -1 || ModuleGuidToUse != Guid.Empty)
		{
			if (!moduleLoaded)
			{
				LoadModule();
			}
		}
	}

	protected void LoadModule()
	{

		siteSettings ??= CacheHelper.GetCurrentSiteSettings();

		if (_moduleID > -1 || ModuleGuidToUse != Guid.Empty)
		{
			Controls.Clear();

			Module module = null;

			if (_moduleID > -1) module = new Module(ModuleID);

			if (module == null && ModuleGuidToUse != Guid.Empty) module = new Module(ModuleGuidToUse);

			if (module.ModuleId > -1)
			{
				if (WebConfigSettings.EnforceSiteIdInModuleWrapper)
				{
					//SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
					if (siteSettings is not null)
					{
						//could add a web.config setting check here to determine if the module should still be loaded even if it is from a different site.
						if (module.SiteId != siteSettings.SiteId)
						{
							//this will adjust the ModuleGuidToUse set on the control to have the SiteID for the first two characters.
							//doing this gives us the ability to create/load module instances in multi-site installs using skins containing SuperWrapper
							//with the same ModuleGuidToUse.
							var sb = new StringBuilder(ModuleGuidToUse.ToString());

							int siteIdLength = siteSettings.SiteId.ToString().Length;

							// mojo can run more than 999 sites but the odds are not high that this would ever happen. 
							// to keep the overhead down we'll just use some simple math here
							sb.Remove(0, siteIdLength + 5);

							sb.Insert(0, "00000" + siteSettings.SiteId.ToString());

							ModuleGuidToUse = Guid.Parse(sb.ToString());

							LoadModule();

							return;
						}
					}
				}

				Control c = Page.LoadControl($"~/{module.ControlSource}") ?? throw new ArgumentException($"Unable to load control from ~/{module.ControlSource}");
				if (c is SiteModuleControl siteModule)
				{
					siteModule.SiteId = siteSettings.SiteId;
					siteModule.ModuleConfiguration = module;
					Title = module.ModuleTitle;
				}

				Controls.Add(c);
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

		if (FeatureGuid != Guid.Empty && ModuleGuidToUse != Guid.Empty)
		{
			var moduleDefinition = new ModuleDefinition(FeatureGuid);

			if (moduleDefinition == null)
			{
				log.Error($"Cannot create module because featureGuid \"{FeatureGuid}\" does not correspond to an installed feature.");
				return;
			}

			var module = new Module
			{
				ModuleGuid = ModuleGuidToUse,
				ModuleDefId = moduleDefinition.ModuleDefId,
				FeatureGuid = moduleDefinition.FeatureGuid,
				Icon = moduleDefinition.Icon,
				SiteId = siteSettings.SiteId,
				SiteGuid = siteSettings.SiteGuid
			};

			//need to account for user not being logged in the first time the site is visited with this SuperWrapper in the skin.
			SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
			if (siteUser is null)
			{
				Role adminsRole = Role.GetRoleByName(siteSettings.SiteId, "Admins");
				DataTable dtUsers = SiteUser.GetRoleMembers(adminsRole.RoleId);
				if (dtUsers.Rows.Count > 0)
				{
					siteUser = SiteUser.GetByEmail(siteSettings, dtUsers.Rows[0]["Email"].ToString());
				}

				if (siteUser is null)
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
				var newModule = new Module(ModuleGuidToUse);
				// set default module settings from json
				if (!string.IsNullOrWhiteSpace(DefaultModuleSettings))
				{
					DefaultModuleSettings = DefaultModuleSettings.Replace("$_SiteID_$", siteSettings.SiteId.ToString());

					var moduleSettings = ModuleSettings.GetModuleSettings(newModule.ModuleId);

					var oModuleSettings = new JObject();

					try
					{
						oModuleSettings = JObject.Parse(DefaultModuleSettings);
					}
					catch (Newtonsoft.Json.JsonReaderException)
					{
						log.Error($"{ID} -- could not load defaultModuleSettings because of invalid json");
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
				if (!string.IsNullOrWhiteSpace(DefaultGeneralSettings))
				{
					DefaultGeneralSettings = DefaultGeneralSettings.Replace("$_SiteID_$", siteSettings.SiteId.ToString());

					JObject oGeneralSettings = JObject.Parse(DefaultGeneralSettings);

					IList<System.Reflection.PropertyInfo> props = new List<System.Reflection.PropertyInfo>(newModule.GetType().GetProperties());

					foreach (System.Reflection.PropertyInfo prop in props)
					{
						JProperty jProp = oGeneralSettings.Property(prop.Name);

						if (jProp != null)
						{
							Type propType = prop.PropertyType.Name switch
							{
								"Boolean" => true.GetType(),
								"Int32" => 1.GetType(),
								"Guid" => Guid.Empty.GetType(),
								_ => "".GetType(),
							};
							prop.SetValue(newModule, Convert.ChangeType(jProp.Value, propType));
						}
					}

					newModule.Save();
				}
			}
		}
	}
}