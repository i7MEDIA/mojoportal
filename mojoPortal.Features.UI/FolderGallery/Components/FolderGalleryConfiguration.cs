using System;
using System.Collections;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.GalleryUI;

public class FolderGalleryConfiguration
{
	public static Guid FeatureGuid => new("9e58fcda-90de-4ed7-abc7-12f096f5c58f");

	public FolderGalleryConfiguration() { }

	public FolderGalleryConfiguration(Hashtable settings) => LoadSettings(settings);

	private void LoadSettings(Hashtable settings)
	{
		if (settings is null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		GalleryRootFolder = WebUtils.ParseStringFromHashtable(settings, "FolderGalleryRootFolder", GalleryRootFolder);
		CustomCssClass = WebUtils.ParseStringFromHashtable(settings, "CustomCssClassSetting", CustomCssClass);
		ShowPermaLinks = WebUtils.ParseBoolFromHashtable(settings, "ShowPermaLinksSetting", ShowPermaLinks);
		ShowMetaDetails = WebUtils.ParseBoolFromHashtable(settings, "ShowMetaDetailsSetting", ShowMetaDetails);
		AllowEditUsersToChangeFolderPath = WebUtils.ParseBoolFromHashtable(settings, "AllowEditUsersToChangeFolderPath", AllowEditUsersToChangeFolderPath);
		AllowEditUsersToUpload = WebUtils.ParseBoolFromHashtable(settings, "AllowEditUsersToUpload", AllowEditUsersToUpload);
	}

	public string CustomCssClass { get; private set; } = string.Empty;

	public string GalleryRootFolder { get; private set; } = string.Empty;

	public bool AllowEditUsersToChangeFolderPath { get; private set; } = true;

	public bool AllowEditUsersToUpload { get; private set; } = true;

	public bool ShowPermaLinks { get; private set; } = false;

	public bool ShowMetaDetails { get; private set; } = false;

	public static int MaxFilesToUploadAtOnce => ConfigHelper.GetIntProperty("FolderGallery:MaxFilesToUploadAtOnce", 20);

	public static string BasePathFormat => ConfigHelper.GetStringProperty("FolderGallery:BasePathFormat", "~/Data/Sites/{0}/media/FolderGalleries/");
}