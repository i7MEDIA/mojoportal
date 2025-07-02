namespace mojoPortal.Web.ContactUI;

/// <summary>
/// Display Settings for Contact Form feature.
/// Configuration is per skin via the config/plugins/ContactForm/display.json file.
/// </summary>
public class ContactFormDisplaySettings : BasePluginDisplaySettings
{
	public ContactFormDisplaySettings() : base() { }
	public string ContainerClass { get; set; } = "container-fluid";
	public string MessageListClass { get; set; } = "col-sm-4";
	public string MessageViewClass { get; set; } = "col-sm-8";
}
