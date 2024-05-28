namespace mojoPortal.Web.UI;

public class MemberListDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public MemberListDisplaySettings() : base() { }

	public bool HideWebSiteColumn { get; set; } = false;
	/// <summary>
	/// if true wraps a ul with li elements around the alpha pager links
	/// </summary>
	public bool UseListForAlphaPager { get; set; } = false;
	public bool ShowForumPosts { get; set; } = true;
	public bool ShowEmail { get; set; } = false;
	public bool ShowLoginName { get; set; } = false;
	public bool ShowFirstAndLastName { get; set; } = false;
	public bool ShowJoinDate { get; set; } = true;
	public bool ShowUserId { get; set; } = false;
	public string AlphaPagerContainerCssClass { get; set; } = string.Empty;
	public string AllUsersLinkCssClass { get; set; } = "ModulePager";
	public string AlphaPagerCurrentPageCssClass { get; set; } = "SelectedPage";
	public string AlphaPagerOtherPageCssClass { get; set; } = "ModulePager";
	public string TableCssClass { get; set; } = string.Empty;
	public string TableAttributes { get; set; } = "cellspacing='0' width='100%'";
}