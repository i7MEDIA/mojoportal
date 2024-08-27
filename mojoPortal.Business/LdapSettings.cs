namespace mojoPortal.Business;

public class LdapSettings
{
	public LdapSettings() { }

	public string Server { get; set; } = string.Empty;

	public string Domain { get; set; } = string.Empty;

	public int Port { get; set; } = 389;

	public string RootDN { get; set; } = string.Empty;

	public string UserDNKey { get; set; } = "CN";

}
