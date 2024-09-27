#nullable enable
using mojoPortal.Business.WebHelpers;
using System;

namespace mojoPortal.Web.Components;

public static class PageUrlService
{
	#region Private Fields

	#region Constants

	private const string _loginLink = "~/Secure/Login.aspx";
	private const string _externalLoginCallbackLink = "~/ExternalCallbackLink/Callback";
	private const string _loginRedirectLink = "~/";
	private const string _registerLink = "~/Secure/Register.aspx";
	private const string _passwordRecoveryLink = "~/Secure/RecoverPassword.aspx";
	private const string _logoutLink = "~/Logoff.aspx";
	private const string _confirmRegistrationLink = "~/";
	private const string _accessDeniedLink = "~/";

	#endregion

	/// <summary>
	/// Overrides are for programmatic on demand overrides, in the case of something like a different login process that doesn't utilize static strings.
	/// Override processors work in tandem with the override links as string templates to manipulate the end result.
	/// </summary>
	public static OverrideTypes Overrides = new();

	#endregion


	public static string GetLoginLink(string? returnUrl = null)
	{
		if (Overrides.LoginLink is not null)
		{
			var overrideLink = Overrides.LoginLink.Trim();

			if (Overrides.LoginLinkProcessor is not null)
			{
				overrideLink = Overrides.LoginLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

		return GetLink(
			url: AppConfig.LoginLink,
			fallbackUrl: _loginLink,
			returnUrl
		);
	}


	public static string GetExternalLoginCallbackLink(string? returnUrl = null)
	{
		if (Overrides.ExternalLoginCallbackLink is not null)
		{
			var overrideLink = Overrides.ExternalLoginCallbackLink.Trim();

			if (Overrides.ExternalLoginCallbackLinkProcessor is not null)
			{
				overrideLink = Overrides.ExternalLoginCallbackLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

		return GetLink(
			url: AppConfig.ExternalLoginCallbackLink,
			fallbackUrl: _externalLoginCallbackLink,
			returnUrl
		);
	}


	public static string GetLoginRedirectLink(string? link = null)
	{
		if (Overrides.LoginRedirectLink is not null)
		{
			var overrideLink = Overrides.LoginRedirectLink.Trim();

			if (Overrides.LoginRedirectLinkProcessor is not null)
			{
				overrideLink = Overrides.LoginRedirectLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

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
			fallbackUrl!,
			relativeUrl: true
		);
	}


	public static string GetRegisterLink(string? returnUrl = null)
	{
		string? overrideLink = null;

		if (Overrides.RegisterLink is not null)
		{
			overrideLink = Overrides.RegisterLink.Trim();

			if (Overrides.RegisterLinkProcessor is not null)
			{
				overrideLink = Overrides.RegisterLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

		return GetLink(
			url: AppConfig.RegistrationLink,
			fallbackUrl: _registerLink,
			returnUrl
		);
	}


	public static string GetRecoverPasswordLink(string? returnUrl = null)
	{
		string? overrideLink = null;

		if (Overrides.PasswordRecoveryLink is not null)
		{
			overrideLink = Overrides.PasswordRecoveryLink.Trim();

			if (Overrides.PasswordRecoveryLinkProcessor is not null)
			{
				overrideLink = Overrides.PasswordRecoveryLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

		return GetLink(
			url: AppConfig.PasswordRecoveryLink,
			fallbackUrl: _passwordRecoveryLink,
			returnUrl
		);
	}


	public static string GetLogoutLink(string? returnUrl = null)
	{
		string? overrideLink = null;

		if (Overrides.LogoutLink is not null)
		{
			overrideLink = Overrides.LogoutLink.Trim();

			if (Overrides.LogoutLinkProcessor is not null)
			{
				overrideLink = Overrides.LogoutLinkProcessor(overrideLink).Trim();
			}

			return overrideLink;
		}

		return GetLink(
			url: AppConfig.LogoutLink,
			fallbackUrl: _logoutLink,
			returnUrl
		);
	}


	#region Private Methods

	private static string GetLink(
		string? url,
		string fallbackUrl,
		string? returnUrl = null,
		bool relativeUrl = false
	)
	{
		url = string.IsNullOrWhiteSpace(url) ? fallbackUrl.Trim() : url!.Trim();

		var linkBuilder = url.ToLinkBuilder(relativeUrl || url.StartsWith("~/") || url.StartsWith("/"));

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
				link!.Contains("AccessDenied") ||
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


	#region Types

	public class OverrideTypes
	{
		public string? LoginLink { get; set; } = null;
		public Func<string, string>? LoginLinkProcessor { get; set; } = null;
		public string? ExternalLoginCallbackLink { get; set; } = null;
		public Func<string, string>? ExternalLoginCallbackLinkProcessor { get; set; } = null;
		public string? LoginRedirectLink { get; set; } = null;
		public Func<string, string>? LoginRedirectLinkProcessor { get; set; } = null;
		public string? RegisterLink { get; set; } = null;
		public Func<string, string>? RegisterLinkProcessor { get; set; } = null;
		public string? RegisterRedirectLink { get; set; } = null;
		public Func<string, string>? RegisterRedirectLinkProcessor { get; set; } = null;
		public string? PasswordRecoveryLink { get; set; } = null;
		public Func<string, string>? PasswordRecoveryLinkProcessor { get; set; } = null;
		public string? RecoverRedirectLink { get; set; } = null;
		public Func<string, string>? RecoverRedirectLinkProcessor { get; set; } = null;
		public string? ConfirmRegistrationLink { get; set; } = null;
		public Func<string, string>? ConfirmRegistrationLinkProcessor { get; set; } = null;
		public string? AccessDeniedLink { get; set; } = null;
		public Func<string, string>? AccessDeniedLinkProcessor { get; set; } = null;
		public string? LogoutLink { get; set; } = null;
		public Func<string, string>? LogoutLinkProcessor { get; set; } = null;
	}

	#endregion
}
