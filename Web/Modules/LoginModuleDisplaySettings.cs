namespace mojoPortal.Web.UI;


public class LoginModuleDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public LoginModuleDisplaySettings() : base() { }

	public bool HideWhenAuthenticated { get; set; } = false;

	public bool IncludeSocialLogin { get; set; } = false;

	public bool OnlyIncludeSocialLogin { get; set; } = false;

	public string LinkSeparator { get; set; } = "<br />";

	public bool UseOverlayForSocialLogin { get; set; } = false;

	public string SocialLoginLinkText { get; set; } = string.Empty;

	public bool ShowAvatar { get; set; } = true;

	public bool UseProfileLink { get; set; } = true;

	public bool DisableModuleChrome { get; set; } = false;
}