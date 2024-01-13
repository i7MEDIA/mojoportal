using System;
using System.Collections;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI.BetterImageGallery;

public class BIGConfig
{
	public int PageSize { get; private set; } = 25;
	public string Layout { get; private set; } = "_BetterImageGallery";
	public string FolderPath { get; private set; } = string.Empty;

	public BIGConfig()
	{ }

	public BIGConfig(Hashtable settings)
	{
		if (settings == null)
		{
			throw new ArgumentException("must pass in a hashtable of settings");
		}

		PageSize = WebUtils.ParseInt32FromHashtable(settings, "PageSize", PageSize);
		FolderPath = WebUtils.ParseStringFromHashtable(settings, "FolderGalleryPath", FolderPath);

		string layoutString = WebUtils.ParseStringFromHashtable(settings, "Layout", Layout);

		if (!string.IsNullOrWhiteSpace(layoutString))
		{
			Layout = layoutString;
		}
	}
}

