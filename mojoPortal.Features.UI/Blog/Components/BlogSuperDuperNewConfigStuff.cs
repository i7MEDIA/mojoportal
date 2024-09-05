using System;
using Resources;
namespace mojoPortal.Features.UI;

public class BlogSuperDuperNewConfigStuff
{
	public bool ShowBlogSearchBox { get; set; }
	public string BlogAuthor { get; set; } = string.Empty;
	public string BlogAuthorEmail { get; set; } = string.Empty;
	public string BlogCopyright { get; set; } = string.Empty;
	public string BlogDescription { get; set; } = string.Empty;
	public string DefaultUrlPrefix { get; set; } = string.Empty;

	[Obsolete]
	public int BlogEditorHeightSetting { get; set; } = 350;

	public string BlogDateTimeFormat { get; set; } = "F";
	public int RelatedItemsToShow { get; set; } = 0;
	public bool DefaultIncludeImageInExcerptChecked { get; set; } = true;
	public bool DefaultIncludeImageInPostChecked { get; set; } = true;
	public bool HideDetailsFromUnauthenticated { get; set; }
	public bool BlogUseExcerptSetting { get; set; }
	public bool UseExcerptInFeedSetting { get; set; }
	public int BlogExcerptLengthSetting { get; set; } = 250;
	public string BlogExcerptSuffixSetting { get; set; } = BlogResources.ExcerptSuffix;
	public string BlogMoreLinkText { get; set; } = BlogResources.ReadMore;
	public bool BlogShowTitleOnlySetting { get; set; }
	public int BlogEntriesToShowSetting { get; set; } = 10;
	public bool BlogShowPagerInListSetting { get; set; } = true;
	public string ManagingEditorName { get; set; } = string.Empty;
}