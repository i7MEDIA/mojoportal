using System;
using System.Collections;

namespace mojoPortal.MediaPlayerUI;

public class VideoPlayerConfiguration
{
	public VideoPlayerConfiguration() { }

	public VideoPlayerConfiguration(Hashtable settings) => LoadSettings(settings);

	private void LoadSettings(Hashtable settings)
	{
		if (settings == null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		AutoStart = settings.ParseBool("AutoStartSetting", AutoStart);
		ContinuousPlay = settings.ParseBool("ContinuousPlaySetting", ContinuousPlay);
		DisableShuffle = settings.ParseBool("DisableShuffleSetting", DisableShuffle);
		InstanceCssClass = settings.ParseString("CustomCssClassSetting", InstanceCssClass);
		HeaderContent = settings.ParseString("HeaderContent", HeaderContent);
		FooterContent = settings.ParseString("FooterContent", FooterContent);
		AllowFullScreen = settings.ParseBool("AllowFullScreen", AllowFullScreen);
	}

	public string HeaderContent { get; private set; } = string.Empty;

	public string FooterContent { get; private set; } = string.Empty;

	public string InstanceCssClass { get; private set; } = "bluemonday";

	public bool AllowFullScreen { get; private set; } = true;

	public bool AutoStart { get; private set; } = false;

	public bool ContinuousPlay { get; private set; } = false;

	public bool DisableShuffle { get; private set; } = false;

	public static bool EditPageSuppressPageMenu => ConfigHelper.GetBoolProperty("KDMediaPlayer:EditSuppressPageMenu", true);

	public static bool EnableWarnings => ConfigHelper.GetBoolProperty("KDMediaPlayer:EnableWarnings", false);

	public static bool EnableErrors => ConfigHelper.GetBoolProperty("KDMediaPlayer:EnableErrors", true);

	/// <summary>
	/// options are window, transparent, opaque, direct, gpu
	/// </summary>
	public static string VideoWindowMode => ConfigHelper.GetStringProperty("KDMediaPlayer:VideoWindowMode", "opaque");

	/// <summary>
	/// valid options are metadata, none, and auto, default is metadata
	/// </summary>
	public static string VideoPreload => ConfigHelper.GetStringProperty("KDMediaPlayer:VideoPreload", "metadata");
}