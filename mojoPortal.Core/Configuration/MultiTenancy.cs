namespace mojoPortal.Core.Configuration;

public class MultiTenancy
{
	public bool Enabled
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:Enabled", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("AllowMultipleSites", true);
			}
			return configValue;
		}
	}

	public bool UseFolders => Mode == TenantModes.Folder;
	public bool UseHostNames => Mode == TenantModes.HostName;

	public RelatedSitesMode RelatedSites => new();


	public string DisallowedFolderNames => ConfigHelper.GetStringProperty("MultiTenancy:DisallowedFolderNames", "Admin;ClientScript;Controls;Data;Modules;NeatHtml;NeatUpload;Secure;Services;Setup;SuperFlexi;WebStore");

	public bool AllowDeletingSites
	{

		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:AllowDeletingSites", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("AllowDeletingChildSites", false);
			}
			return configValue;
		}
	}

	public bool DeleteSiteFolder
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:DeleteSiteFolder", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("DeleteSiteFolderWhenDeletingSites", false);
			}
			return configValue;
		}
	}

	//todo: implement this in the file manager, not just the link
	public bool AllowFileManager
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:AllowFileManager", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("AllowFileManagerInChildSites", false);
			}
			return configValue;
		}
	}

	public bool AllowPasswordFormatChange
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:AllowPasswordFormatChange", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("AllowPasswordFormatChangeInChildSites", false);
			}
			return configValue;
		}
	}

	public bool ShowSystemInfo
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:ShowSystemInfo", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("ShowSystemInformationInChildSiteAdminMenu", true);
			}
			return configValue;
		}
	}

	public bool AllowEditingSkins
	{
		get
		{
			var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:AllowEditingSkins", false);
			if (!configValue)
			{
				//legacy support
				configValue = ConfigHelper.GetBoolProperty("AllowEditingSkinsInChildSites", false);
			}
			return configValue;
		}
	}

	private TenantModes Mode
	{
		get
		{
			var configValue = ConfigHelper.GetStringProperty("MultiTenancy:Mode", null);
			if (string.IsNullOrWhiteSpace(configValue))
			{
				if (ConfigHelper.GetBoolProperty("UseFoldersInsteadOfHostnamesForMultipleSites", false)
					|| ConfigHelper.GetBoolProperty("UseFolderBasedMultiTenants", false))
				{
					return TenantModes.Folder;
				}
			}
			else
			{
				return configValue.ToLower() switch
				{
					var mode when mode == nameof(TenantModes.Folder).ToLower() => TenantModes.Folder,
					_ => TenantModes.HostName,
				};
			}
			return TenantModes.HostName;
		}
	}

	private enum TenantModes
	{
		HostName = 0,
		Folder = 1
	}

	public class RelatedSitesMode
	{
		public bool Enabled
		{
			get
			{
				var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:EnableRelatedSiteMode", false);
				if (!configValue)
				{
					//legacy support
					configValue = ConfigHelper.GetBoolProperty("UseRelatedSiteMode", false);
				}
				return configValue;
			}
		}

		public int ParentSiteId
		{
			get
			{
				//legacy support
				var configValue = ConfigHelper.GetIntProperty("RelatedSiteID", -1);
				if (configValue == -1)
				{
					//current key
					configValue = ConfigHelper.GetIntProperty("MultiTenancy:ParentSiteId", 1);
				}
				return configValue;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the content folder is shared across multiple tenants.
		/// </summary>
		/// <remarks>This property first checks if RelatedSites is <c>Enabled</c> and then checks
		/// configuration settings "MultiTenancy:ShareContentFolder" and, for
		/// legacy support,  "UseSameContentFolderForRelatedSiteMode".</remarks>
		public bool ShareContentFolder
		{
			get
			{
				if (!Enabled)
				{
					return false;
				}

				var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:ShareContentFOlder", false);
				if (!configValue)
				{
					//legacy support
					configValue = ConfigHelper.GetBoolProperty("UseSameContentFolderForRelatedSiteMode", false);
				}
				return configValue;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the Role Manager should be hidden in Child Sites.
		/// </summary>
		/// <remarks>This property first checks if RelatedSites is <c>Enabled</c> and then checks
		/// configuration settings "MultiTenancy:AllowRoleManager" and, for
		/// legacy support,  "RelatedSiteModeHideRoleManagerInChildSites".</remarks>
		public bool AllowRoleManager
		{
			get
			{
				if (!Enabled)
				{
					return false;
				}

				var configValue = ConfigHelper.GetBoolProperty("MultiTenancy:AllowRoleManager", true);
				if (!configValue)
				{
					//legacy support
					configValue = ConfigHelper.GetBoolProperty("RelatedSiteModeHideRoleManagerInChildSites", true);
				}
				return configValue;
			}
		}
	}
}

