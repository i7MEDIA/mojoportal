namespace mojoPortal.Web.UI;


public class CommentSystemDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public CommentSystemDisplaySettings() : base() { }

	public string DeleteLinkImage { get; set; } = "~/Data/SiteImages/delete.png";

	public bool ShowNameInputWhenAuthenticated { get; set; } = false;

	public bool ShowEmailInputWhenAuthenticated { get; set; } = false;

	public bool ShowUrlInputWhenAuthenticated { get; set; } = false;

	public string PreferredEditor { get; set; } = "";
}