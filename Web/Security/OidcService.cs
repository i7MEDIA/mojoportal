#nullable enable
using IdentityModel.Client;
using log4net;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace mojoPortal.Web.Security;

public class OidcService
{
	#region Private Fields

	private static readonly ILog _log = LogManager.GetLogger(typeof(OidcService));
	private readonly DiscoveryCache _discoveryCache;
	private readonly JsonWebKeyCache _jsonWebKeyCache;
	private readonly string _issuer;
	private readonly string _clientId;
	private readonly string _clientSecret;
	private readonly string _redirectUri = "";
	private readonly string _postLogutUri = "";

	private const string TOKEN_KEY_ACCESS = "ExternalLogin:AccessToken";
	private const string TOKEN_KEY_IDENTITY = "ExternalLogin:IdentityToken";
	private const string TOKEN_KEY_REFRESH = "ExternalLogin:RefreshToken";

	#endregion


	#region Public Properties

	public JsonWebToken? AccessToken => ReadAccessToken();
	public JsonWebToken? IdentityToken => ReadIdentityToken();

	#endregion


	public OidcService(AppConfig.OAuthConfiguration model)
	{
		_discoveryCache = new(model.Authority);
		_jsonWebKeyCache = new(_discoveryCache);
		_issuer = model.Authority;
		_clientId = model.ClientId;
		_clientSecret = model.ClientSecret;

		PageUrlService.Overrides.LoginLink = "placeholder";
		PageUrlService.Overrides.LoginLinkProcessor = (url) => GetLoginUrl();
		//PageUrlService.Overrides.LoginLinkProcessor = (url) => GetLoginUrlAsync().Result;

		PageUrlService.Overrides.RegisterLink = "placeholder";
		PageUrlService.Overrides.RegisterLinkProcessor = (url) => GetLoginUrl().Replace("/auth", "/registrations");

		PageUrlService.Overrides.ExternalLoginCallbackLink = "~/ExternalLogin/Callback";
		PageUrlService.Overrides.ExternalLoginCallbackLinkProcessor = (url) => url.ToLinkBuilder().ToString();

		PageUrlService.Overrides.LogoutLink = "~/ExternalLogin/Logout";
		PageUrlService.Overrides.LogoutLinkProcessor = (url) => url.ToLinkBuilder().ReturnUrl(PageUrlService.GetLoginRedirectLink()).ToString();
	}


	#region Public Methods

	public string GetLoginUrl(Guid? nonce = null)
	//public string GetLoginUrlAsync(Guid? nonce = null)
	{
		// TODO: Fix this so the code doesn't deadlock
		//var disco = await _DiscoveryCache.GetAsync();
		//var requestUrl = new RequestUrl(disco.AuthorizeEndpoint!);
		var requestUrl = new RequestUrl(_issuer + "/protocol/openid-connect/auth");

		return requestUrl.CreateAuthorizeUrl(
			clientId: _clientId,
			responseType: "code",
			redirectUri: PageUrlService.GetExternalLoginCallbackLink(),
			nonce: nonce?.ToString(),
			scope: "openid"
		);
	}


	public async Task<(bool, string?, string?)> GetTokensByCodeAsync(string iss, string code)
	{
		var tokens = await GetTokensByCodeAsync(code);

		try
		{
			_ = await ValidateTokenAsync(tokens.AccessToken!);
		}
		catch (Exception e)
		{
			return (false, "There was an issue validating your access token.", e.Message);
		}

		SetTokenCookies(
			tokens?.AccessToken ?? string.Empty,
			tokens?.IdentityToken ?? string.Empty,
			tokens?.RefreshToken ?? string.Empty
		);

		return (true, null, null);
	}


	/// <summary>
	/// Authenticates user and returns the user's unique identitfier from the identity provider.
	/// </summary>
	/// <returns>A Guid is the user is authenticated successfully or null if unsuccessful.</returns>
	public async Task<bool> IsAuthenticated()
	{
		// If there is no authentication token, user is not logged in.
		var accessTokenString = CookieHelper.GetCookieValue(TOKEN_KEY_ACCESS);

		if (string.IsNullOrWhiteSpace(accessTokenString))
		{
			return false;
		}

		// If there is a token, validate it. Any validation issues will be handled via the catch statements.
		try
		{
			await ValidateTokenAsync(accessTokenString);
		}
		// If access token is expired try to use refresh token.
		catch (SecurityTokenExpiredException)
		{
			try
			{
				var refreshToken = CookieHelper.GetCookieValue(TOKEN_KEY_REFRESH);

				if (string.IsNullOrWhiteSpace(refreshToken))
				{
					return false;
				}

				var tokens = await GetTokensByRefreshToken(refreshToken);

				SetTokenCookies(
					tokens?.AccessToken ?? string.Empty,
					tokens?.IdentityToken ?? string.Empty,
					tokens?.RefreshToken ?? string.Empty
				);
			}
			catch (Exception e)
			{
				if (e.Message == "Token is not active")
				{
					// This should only happen in the case that there was a refresh token but it was expired, so clear those cookies
					// TODO: Redirect to the login page?
					//ClearTokenCookies();
				}
				else
				{
					throw e;
				}
			}
		}
		// All other exceptions get thrown, that way we can handle it in the request pipeline.
		catch (Exception e)
		{
			throw e;
		}

		return true;
	}


	public async Task<bool> ValidateTokenAsync(string token)
	{
		var jwt = ReadToken(token);

		await _jsonWebKeyCache.VerifyCache(jwt.Kid);

		var validationParameters = new TokenValidationParameters
		{
			// https://stackoverflow.com/questions/53550321/keycloak-gatekeeper-aud-claim-and-client-id-do-not-match/53627747#53627747
			ValidateAudience = false,
			ValidAudience = _clientId,
			ValidateIssuer = true,
			ValidIssuer = _issuer,
			ValidateIssuerSigningKey = true,
			ValidateLifetime = true,
			IssuerSigningKeys = _jsonWebKeyCache.Keys,
		};
		var handler = new JsonWebTokenHandler();
		var result = await handler.ValidateTokenAsync(jwt, validationParameters);

		if (!result.IsValid)
		{
			throw result.Exception;
		}

		return true;
	}


	public async Task<string?> Logout()
	{
		// I don't know if we should revoke the token or not.
		//var client = new HttpClient();
		//var result = await client.RevokeTokenAsync(new TokenRevocationRequest
		//{
		//	Address = disco.RevocationEndpoint,
		//	ClientId = "client",
		//	ClientSecret = "secret",
		//	Token = CookieHelper.GetCookieValue(TOKEN_KEY_ACCESS)
		//});

		//if (result.IsError)
		//{
		//	_log.Error(result.Error);
		//}

		var disco = await _discoveryCache.GetAsync();
		var requestUrl = new RequestUrl(disco.EndSessionEndpoint!);
		var endSessionUrl = requestUrl.CreateEndSessionUrl(
			idTokenHint: CookieHelper.GetCookieValue(TOKEN_KEY_IDENTITY),
			postLogoutRedirectUri: SiteUtils.GetNavigationSiteRoot()
		);

		ClearTokenCookies();

		return endSessionUrl;
	}

	#endregion


	#region Private Methods

	private async Task<TokenResponse?> GetTokensByCodeAsync(string code)
	{
		var disco = await _discoveryCache.GetAsync();
		var client = new HttpClient();

		var response = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
		{
			Address = disco.TokenEndpoint,
			ClientId = _clientId,
			ClientSecret = _clientSecret,
			Code = code,
			RedirectUri = PageUrlService.GetExternalLoginCallbackLink(),
		});

		if (response.IsError)
		{
			throw new Exception(response.ErrorDescription);
		}

		return response;
	}


	private async Task<TokenResponse?> GetTokensByRefreshToken(string refreshToken)
	{
		var disco = await _discoveryCache.GetAsync();
		var client = new HttpClient();

		var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
		{
			Address = disco.TokenEndpoint,
			ClientId = _clientId,
			ClientSecret = _clientSecret,
			RefreshToken = refreshToken
		});

		if (response.IsError)
		{
			throw new Exception(response.ErrorDescription);
		}

		return response;
	}


	private JsonWebToken ReadToken(string token)
	{
		var handler = new JsonWebTokenHandler();

		return handler.ReadJsonWebToken(token);
	}


	private JsonWebToken? ReadAccessToken(string? accessToken = null)
	{
		accessToken ??= CookieHelper.GetCookieValue(TOKEN_KEY_ACCESS);

		if (string.IsNullOrWhiteSpace(accessToken))
		{
			return null;
		}

		return ReadToken(accessToken);
	}


	private JsonWebToken? ReadIdentityToken(string? identityToken = null)
	{
		identityToken ??= CookieHelper.GetCookieValue(TOKEN_KEY_IDENTITY);

		if (string.IsNullOrWhiteSpace(identityToken))
		{
			return null;
		}

		return ReadToken(identityToken);
	}


	private void SetTokenCookies(
		string accessToken,
		string identityToken,
		string refreshToken
	)
	{
		CookieHelper.SetCookie(TOKEN_KEY_ACCESS, accessToken, true);
		CookieHelper.SetCookie(TOKEN_KEY_IDENTITY, identityToken, true);
		CookieHelper.SetCookie(TOKEN_KEY_REFRESH, refreshToken, true);
	}


	private void ClearTokenCookies()
	{
		if (CookieHelper.CookieExists(TOKEN_KEY_ACCESS))
		{
			CookieHelper.ExpireCookie(TOKEN_KEY_ACCESS);
		}

		if (CookieHelper.CookieExists(TOKEN_KEY_IDENTITY))
		{
			CookieHelper.ExpireCookie(TOKEN_KEY_IDENTITY);
		}

		if (CookieHelper.CookieExists(TOKEN_KEY_REFRESH))
		{
			CookieHelper.ExpireCookie(TOKEN_KEY_REFRESH);
		}
	}

	#endregion
}
