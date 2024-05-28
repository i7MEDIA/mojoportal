namespace mojoPortal.Web.UI;


public class RegistrationPageDisplaySettings : BaseDisplaySettings
{
	public override string FeatureName => "Core";
	public override string SubFeatureName => GetType().Name.Replace("DisplaySettings", string.Empty);
	public RegistrationPageDisplaySettings() : base() { }


	public string NewsletterListHeading { get; set; } = string.Empty;

	public bool IncludeNewsletterDescriptionInList { get; set; } = false;

	public bool ShowNewsLetters { get; set; } = true;
}