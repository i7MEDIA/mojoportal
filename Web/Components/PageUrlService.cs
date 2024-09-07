using DotNetOpenAuth.OpenId.Provider;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Components;

#nullable enable

public class PageUrlService
{
	#region Constants
	
	private const string _loginLink = "~/Secure/Login.aspx";
	private const string _loginRedirectLink = "~/";
	private const string _registerLink = "~/Secure/Register.aspx";
	private const string _recoverPasswordLink = "~/Secure/RecoverPassword.aspx";
	private const string _confirmRegistration = "~/";
	private const string _accessDenied = "~/";

	#endregion


	public static string GetLoginLink(string? returnUrl = null)
	{
		return GetLink(
			url: AppConfig.LoginLink,
			fallbackUrl: _loginLink,
			returnUrl
		);
	}


	public static string GetLoginRedirectLink(string? link = null)
	{
		var siteSettings = CacheHelper.GetCurrentSiteSettings();
		var fallbackUrl = _loginRedirectLink;

		if (
			IsRedirectableFromLogin(link) &&
			(
				siteSettings.IsServerAdminSite ||
				!WebConfigSettings.UseFolderBasedMultiTenants ||
				!WebConfigSettings.AppendDefaultPageToFolderRootUrl
			)
		)
		{
			fallbackUrl = link;
		}

		return GetLink(
			url: AppConfig.LoginRedirectLink,
			fallbackUrl!
		);
	}


	public static string GetRegisterLink(string? returnUrl = null)
	{
		return GetLink(
			url: AppConfig.RegistrationLink,
			fallbackUrl: _loginLink,
			returnUrl
		);
	}


	public static string GetRecoverPasswordLink(string? returnUrl = null)
	{
		return GetLink(
			url: null,
			fallbackUrl: _recoverPasswordLink,
			returnUrl
		);
	}


	#region Private Methods

	private static string GetLink(string? url, string fallbackUrl, string? returnUrl = null)
	{
		url = string.IsNullOrWhiteSpace(url) ? fallbackUrl.Trim() : url!.Trim();

		var linkBuilder = url.ToLinkBuilder(url.StartsWith("~/") || url.StartsWith("/"));

		if (returnUrl is not null)
		{
			linkBuilder.ReturnUrl(returnUrl);
		}

		return linkBuilder.ToString();
	}

	private static bool IsRedirectableFromLogin(string? link)
	{
		if (
			!string.IsNullOrWhiteSpace(link) &&
			!(
				link.Contains("AccessDenied") ||
				link.Contains("Login") ||
				link.Contains("SignIn") ||
				link.Contains("ConfirmRegistration.aspx") ||
				link.Contains("OpenIdRpxHandler.aspx") ||
				link.Contains("RecoverPassword.aspx") ||
				link.Contains("Register")
			)
		)
		{
			return true;
		}

		return false;
	}

	#endregion
}