namespace mojoPortal.Web.ContactUI;

public class ContactFormDisplaySettings : BasePluginDisplaySettings
{
	public ContactFormDisplaySettings() : base() { }
	public string ContainerClass { get; set; } = "container-fluid";
	public string MessageListClass { get; set; } = "col-sm-4";
	public string MessageViewClass { get; set; } = "col-sm-8";
}
