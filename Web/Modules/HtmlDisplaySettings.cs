namespace mojoPortal.Web.ContentUI;


public class HtmlDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => "HtmlModule";
	public HtmlDisplaySettings() : base() { }

	public string EditorTogglePositionMy { get; set; } = "left bottom";

	public string EditorTogglePositionAt { get; set; } = "left top";

	public string EditToggleCommonCssClass { get; set; } = "inlineedittoggle ui-icon";

	public string EditToggleLockCssClass { get; set; } = "ui-icon-lock";

	public string EditToggleUnLockCssClass { get; set; } = "ui-icon-unlock";

	public bool AddEditToggleToModuleTitle { get; set; } = true;

	/// <summary>
	/// example: Created {0} by {1}
	/// </summary>
	public string OverrideCreatedByUserDateFormat { get; set; } = string.Empty;

	/// <summary>
	/// example: Created {0}
	/// </summary>
	public string OverrideCreatedDateFormat { get; set; } = string.Empty;

	/// <summary>
	/// example: Created by {0}
	/// </summary>
	public string OverrideCreatedByUserFormat { get; set; } = string.Empty;

	/// <summary>
	/// example: Modified {0} by {1}
	/// </summary>
	public string OverrideModifiedByUserDateFormat { get; set; } = string.Empty;

	/// <summary>
	/// example: Modified {0}
	/// </summary>
	public string OverrideModifiedDateFormat { get; set; } = string.Empty;

	/// <summary>
	/// example: Modified by {0}
	/// </summary>
	public string OverrideModifiedByUserFormat { get; set; } = string.Empty;

	/// <summary>
	/// http://msdn.microsoft.com/en-us/library/az4se3k1.aspx
	/// </summary>
	public string DateFormat { get; set; } = "d";

	/// <summary>
	/// if true will try to use the first and last name of created by and last mod user
	/// will fallback to disoplayname if those are empty
	/// </summary>
	public bool UseAuthorFirstAndLastName { get; set; } = false;

	public bool DisableContentRating { get; set; } = false;

	public bool UseBottomContentRating { get; set; } = false;

	public bool UseHtml5Elements { get; set; } = false;

	public bool UseOuterBodyForHtml5Article { get; set; } = false;

	public bool LinkAuthorAvatarToProfile { get; set; } = false;

	public string AvatarUserNameTooltipFormat { get; set; } = "View User Profile for {0}";
}