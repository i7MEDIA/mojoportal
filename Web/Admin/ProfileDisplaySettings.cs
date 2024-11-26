namespace mojoPortal.Web.UI;

public class ProfileDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => "UserProfile";
	public ProfileDisplaySettings() : base() { }

	public string OverrideAvatarLabel { get; set; } = string.Empty;

	public bool HidePostCount { get; set; } = false;

	public string IsDeletedUserNoteFormat { get; set; } = "<span class=\"txterror\">{0}</span>";
}
