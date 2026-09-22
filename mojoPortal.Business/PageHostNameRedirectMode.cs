namespace mojoPortal.Business;

/// <summary>
/// Specifies the redirection behavior when a page with a host name override is accessed via an alternate host name.
/// </summary>
public enum PageHostNameRedirectMode
{
	None,
	Permanent301,
	Temporary302
}
