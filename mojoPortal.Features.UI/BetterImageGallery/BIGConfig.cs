using mojoPortal.Web.Framework;
using System;
using System.Collections;

namespace mojoPortal.Features.UI.BetterImageGallery
{
	public class BIGConfig
	{
		public int PageSize { get; private set; } = 25;
		public string Layout { get; private set; } = "_BetterImageGallery";
		public string FolderPath { get; private set; } = string.Empty;


		public BIGConfig()
		{ }


		public BIGConfig(Hashtable settings)
		{
			LoadSettings(settings);
		}


		private void LoadSettings(Hashtable settings)
		{
			if (settings == null)
			{
				throw new ArgumentException("must pass in a hashtable of settings");
			}

			PageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", PageSize);

			if (settings.Contains("Layout"))
			{
				string layoutString = settings["Layout"].ToString();

				if (!String.IsNullOrWhiteSpace(layoutString))
				{
					Layout = settings["Layout"].ToString();
				}
			}

			//excerptLength = WebUtils.ParseInt32FromHashtable(settings, "BlogExcerptLengthSetting", excerptLength);

			if (settings.Contains("FolderGalleryPath"))
			{
				FolderPath = settings["FolderGalleryPath"].ToString();
			}
		}
	}
}

