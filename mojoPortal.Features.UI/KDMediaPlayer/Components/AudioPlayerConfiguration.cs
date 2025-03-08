using System;
using System.Collections;

namespace mojoPortal.MediaPlayerUI;

public class AudioPlayerConfiguration
{
	public AudioPlayerConfiguration() { }

	public AudioPlayerConfiguration(Hashtable settings) => LoadSettings(settings);

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
	}

	public string HeaderContent { get; private set; } = string.Empty;

	public string FooterContent { get; private set; } = string.Empty;

	public string InstanceCssClass { get; private set; } = "bluemonday";

	public bool AutoStart { get; private set; } = false;

	public bool ContinuousPlay { get; private set; } = false;

	public bool DisableShuffle { get; private set; } = false;
}