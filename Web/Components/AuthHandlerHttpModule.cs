#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.UserRegisteredHandlers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Security;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace mojoPortal.Web;

public class AuthHandlerHttpModule : IHttpModule
{
	private static readonly ILog log = LogManager.GetLogger(typeof(AuthHandlerHttpModule));
	private static bool debugLog = log.IsDebugEnabled;


	public void Init(HttpApplication application)
	{
		if (AppConfig.OAuth.Configured)
		{
			// https://stackoverflow.com/questions/11098575/asp-net-is-it-possible-to-call-async-task-in-global-asax
			application.AddOnAuthenticateRequestAsync(BeginAuthenticateRequest, EndAuthenticateRequest);
		}
		else
		{
			application.AuthenticateRequest += new EventHandler(Application_AuthenticateRequest);
		}
	}


	/// <summary>
	/// This authenticates the user against OAuth/OpenID Connect tokens that are saved as cookies.
	/// We do this here rather than in the "IsAuthenticated" method of the mojoIdentity class because
	/// the code is asynchronous and the method is not.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="eventArgs"></param>
	/// <returns></returns>
	private async Task AuthenticateRequestAsync(object sender, EventArgs eventArgs)
	{
		if (!CanRun(sender, out HttpApplication app, out SiteSettings siteSettings, false))
		{
			return;
		}

		try
		{
			var authenticated = await Global.OidcService.IsAuthenticated();

			if (!authenticated)
			{
				log.Debug("User is not authenticated");
				return;
			}

			var identityToken = Global.OidcService.IdentityToken;

			// This should never happen, but we check anyway.
			if (identityToken is null)
			{
				log.Error("User tried to authenticate in but no identity token was found.");
				return;
			}

			var identityEmail = identityToken.Claims.FirstOrDefault(x => x.Type.Equals("email"))?.Value?.ToLower();

			// This should never happen, but we check anyway.
			if (identityEmail is null)
			{
				log.Error("User tried to authenticate but no email claim was found in the identity token.");
				return;
			}

			var existsInDB = SiteUser.EmailExistsInDB(siteSettings.SiteId, identityEmail);

			if (!existsInDB)
			{
				var identityName = identityToken.Claims.FirstOrDefault(x => x.Type.Equals("name"))?.Value;
				var identityGivenName = identityToken.Claims.FirstOrDefault(x => x.Type.Equals("given_name"))?.Value;
				var identityFamilyName = identityToken.Claims.FirstOrDefault(x => x.Type.Equals("family_name"))?.Value;

				// This should never happen, but we check anyway.
				if (identityName is null)
				{
					log.Error("User tried to authenticate but no name claim was found in the identity token.");
					return;
				}

				var newUser = new SiteUser(siteSettings)
				{
					Name = identityName,
					LoginName = identityEmail,
					Email = identityEmail,
					Password = SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars),
					FirstName = identityGivenName ?? string.Empty,
					LastName = identityFamilyName ?? string.Empty,
				};

				if (Membership.Provider is mojoMembershipProvider membershipProvider)
				{
					newUser.Password = membershipProvider.EncodePassword(siteSettings, newUser, newUser.Password);
				}

				newUser.Save();

				NewsletterHelper.ClaimExistingSubscriptions(newUser);

				var userRegisteredEventArgs = new UserRegisteredEventArgs(newUser);

				OnUserRegistered(userRegisteredEventArgs);
			}

			var siteUser = new SiteUser(siteSettings, identityEmail);

			//Copied logic from SiteLogin.cs  Since we will skip them if we use NTLM
			if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
			{
				SiteUtils.SetSkinCookie(siteUser);
			}

			// track user ip address
			//try
			//{
			//	var userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address())
			//	{
			//		SiteGuid = siteSettings.SiteGuid,
			//		Hostname = app.Request.UserHostName
			//	};

			//	userLocation.Save();

			//	log.Debug($"Set UserLocation : {app.Request.UserHostName}:{SiteUtils.GetIP4Address()}");
			//}
			//catch (Exception ex)
			//{
			//	log.Error(SiteUtils.GetIP4Address(), ex);
			//}

			app.Context.User = new mojoPrincipal(identityEmail);
		}
		catch (Exception e)
		{
			log.Error("Issue in the AuthHandlerHttpModule:", e);
			throw;
		}
	}


	private void Application_AuthenticateRequest(object sender, EventArgs e)
	{
		if (!CanRun(sender, out HttpApplication app, out SiteSettings siteSettings))
		{
			return;
		}

		// Added by Haluk Eryuksel - 2006-01-23
		// support for Windows authentication
		if (
			app.User.Identity.AuthenticationType == "NTLM" ||
			app.User.Identity.AuthenticationType == "Negotiate"
		)
		{
			//Added by Benedict Chan - 2008-08-05
			//Added Cookie here so that we don't have to check the users in every page, also to authenticate under NTLM with "useFolderForSiteDetection == true"
			var cookieName = "siteguid" + siteSettings.SiteGuid;

			if (!CookieHelper.CookieExists(cookieName))
			{
				var existsInDB = SiteUser.LoginExistsInDB(siteSettings.SiteId, app.Context.User.Identity.Name);

				if (!existsInDB)
				{
					var u = new SiteUser(siteSettings)
					{
						Name = app.Context.User.Identity.Name,
						LoginName = app.Context.User.Identity.Name,
						Email = GuessEmailAddress(app.Context.User.Identity.Name),
						Password = SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars),
					};

					if (Membership.Provider is mojoMembershipProvider m)
					{
						u.Password = m.EncodePassword(siteSettings, u, u.Password);
					}

					u.Save();

					NewsletterHelper.ClaimExistingSubscriptions(u);

					var args = new UserRegisteredEventArgs(u);

					OnUserRegistered(args);
				}

				var siteUser = new SiteUser(siteSettings, app.Context.User.Identity.Name);

				CookieHelper.SetCookie(cookieName, siteUser.UserGuid.ToString(), true);

				//Copied logic from SiteLogin.cs  Since we will skip them if we use NTLM
				if (siteUser.UserId > -1 && siteSettings.AllowUserSkins && siteUser.Skin.Length > 0)
				{
					SiteUtils.SetSkinCookie(siteUser);
				}

				// track user ip address
				try
				{
					var userLocation = new UserLocation(siteUser.UserGuid, SiteUtils.GetIP4Address())
					{
						SiteGuid = siteSettings.SiteGuid,
						Hostname = app.Request.UserHostName
					};

					userLocation.Save();

					log.Info($"Set UserLocation : {app.Request.UserHostName}:{SiteUtils.GetIP4Address()}");
				}
				catch (Exception ex)
				{
					log.Error(SiteUtils.GetIP4Address(), ex);
				}
			}
			//End-Added by Benedict Chan
		}
		// End-Added by Haluk Eryuksel


		if (WebConfigSettings.UseFolderBasedMultiTenants && !WebConfigSettings.UseRelatedSiteMode)
		{
			// replace GenericPrincipal with custom one
			//string roles = string.Empty;
			if (!(app.Context.User is mojoPrincipal))
			{
				app.Context.User = new mojoPrincipal(app.Context.User);
			}
		}
	}


	private bool CanRun(object sender, out HttpApplication app, out SiteSettings siteSettings, bool checkAuthentication = true)
	{
		app = null;
		siteSettings = null;

		if (sender == null)
		{
			return false;
		}

		app = (HttpApplication)sender;

		if (
			app.Request == null ||
			checkAuthentication == true && app.Request.IsAuthenticated == false ||
			// TODO: Escape requests that are files from API endpoints.
			!app.Request.Path.EndsWith("svg", StringComparison.OrdinalIgnoreCase) &&
			!app.Request.Path.EndsWith("png", StringComparison.OrdinalIgnoreCase) &&
			!app.Request.Path.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) &&
			!app.Request.Path.EndsWith("jpeg", StringComparison.OrdinalIgnoreCase) &&
			WebUtils.IsRequestForStaticFile(app.Request.Path) ||
			app.Request.Path.Contains(".ashx", StringComparison.OrdinalIgnoreCase) ||
			app.Request.Path.Contains(".axd", StringComparison.OrdinalIgnoreCase) ||
			app.Request.Path.Contains("setup/default.aspx", StringComparison.OrdinalIgnoreCase)
		)
		{
			return false;
		}

		try
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
		}
		catch (System.Data.Common.DbException ex)
		{
			// can happen during upgrades
			log.Error(ex);

			return false;
		}
		catch (InvalidOperationException ex)
		{
			log.Error(ex);

			return false;
		}
		catch (Exception ex)
		{
			// hate to trap System.Exception but SqlCeException does not inherit from DbException as it should
			if (DatabaseHelper.DBPlatform() != "SqlCe")
			{
				throw;
			}

			log.Error(ex);

			return false;
		}

		return true;
	}


	private string GuessEmailAddress(string userName)
	{
		if (WebConfigSettings.GuessEmailForWindowsAuth)
		{
			if (userName.Contains("\\"))
			{
				string domain = userName.Substring(0, userName.IndexOf("\\"));
				string user = userName.Replace(domain + "\\", string.Empty);
				return user + "@" + domain + WebConfigSettings.WindowsAuthDomainExtension;
			}
		}

		return string.Empty;
	}


	private void OnUserRegistered(UserRegisteredEventArgs e)
	{
		foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
		{
			handler.UserRegisteredHandler(null, e);
		}
	}


	private IAsyncResult BeginAuthenticateRequest(object sender, EventArgs args, AsyncCallback callback, object state)
	{
		var task = AuthenticateRequestAsync(sender, args);
		var tcs = new TaskCompletionSource<bool>(state);

		task.ContinueWith(_ =>
			{
				if (task.IsFaulted && task.Exception != null)
				{
					tcs.TrySetException(task.Exception.InnerExceptions);
				}
				else if (task.IsCanceled)
				{
					tcs.TrySetCanceled();
				}
				else
				{
					tcs.TrySetResult(true);
				}

				callback?.Invoke(tcs.Task);
			},
			CancellationToken.None,
			TaskContinuationOptions.None,
			TaskScheduler.Default
		);

		return tcs.Task;
	}


	private void EndAuthenticateRequest(IAsyncResult result)
	{
		// Nothing to do here.
	}


	public void Dispose() { }
}
