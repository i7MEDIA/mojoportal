#nullable enable
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Security.Principal;

namespace mojoPortal.Web.Security;

[Serializable()]
public class mojoIdentity : MarshalByRefObject, IIdentity
{
	private readonly IIdentity? _innerIdentity;
	private bool _isAuthenticated;
	private bool _alreadyChecked = false;
	private string? _username = null;

	public string Name => _innerIdentity?.Name ?? _username ?? string.Empty;
	public string AuthenticationType => _innerIdentity?.AuthenticationType ?? "Forms";


	public mojoIdentity(IIdentity innerIdentity)
	{
		_innerIdentity = innerIdentity;
		_isAuthenticated = innerIdentity.IsAuthenticated;
	}


	/// <summary>
	/// This constructor should only be used in conjunction with OAuth/OpenID Connect configuration
	/// </summary>
	/// <param name="username"></param>
	public mojoIdentity(string username)
	{
		// Since this only gets called once a request is authenticated, set these items to true.
		_alreadyChecked = true;
		_isAuthenticated = true;
		_username = username;
	}


	public bool IsAuthenticated
	{
		get
		{
			if (!_alreadyChecked)
			{
				var useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

				if (
					_isAuthenticated &&
					!WebConfigSettings.UseRelatedSiteMode &&
					useFolderForSiteDetection
				)
				{
					var siteSettings = CacheHelper.GetCurrentSiteSettings();

					if (siteSettings == null)
					{
						return false;
					}

					var cookieName = "siteguid" + siteSettings.SiteGuid.ToString();

					if (!CookieHelper.CookieExists(cookieName))
					{
						return false;
					}

					var cookieValue = CookieHelper.GetCookieValue(cookieName);

					SiteUser siteUser;

					try
					{
						// errors can happen here during upgrades if a new column was added for mp_Users
						siteUser = SiteUtils.GetCurrentSiteUser(true);
					}
					catch
					{
						return false;
					}

					if (siteUser is null)
					{
						return false;
					}

					if (siteUser.UserGuid.ToString() == cookieValue)
					{
						_isAuthenticated = true;
					}

					_alreadyChecked = true;
				}
			}

			return _isAuthenticated;
		}
	}
}
