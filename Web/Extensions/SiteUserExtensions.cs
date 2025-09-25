using System;
using System.Web;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

namespace mojoPortal.Web.Extensions;

public static class SiteUserExtensions
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SiteUserExtensions));

	public static void TrackUserActivity(this SiteUser siteUser)
	{
		if (shouldTrackActivity())
		{
			if (siteUser?.UserId > -1)
			{
				siteUser.UpdateLastActivityTime();
				log.Debug($"Tracked user activity for request {HttpContext.Current.Request.RawUrl}");

				siteUser.TrackLocation();
			}
		}
	}

	private static bool shouldTrackActivity()
	{
		if (HttpContext.Current is not null 
			&& HttpContext.Current.Request is not null 
			&& HttpContext.Current.User.Identity.IsAuthenticated 
			&& WebConfigSettings.TrackAuthenticatedRequests)
		{
			return true;
		}
		return false;
	}

	private static void TrackLocation(this SiteUser siteUser)
	{
		if (shouldTrackActivity()
			 && WebConfigSettings.TrackIPForAuthenticatedRequests)
		{
			var ip4 = SiteUtils.GetIP4Address();
			// track user ip address
			try
			{
				var userLocation = new UserLocation(siteUser.UserGuid, ip4)
				{
					SiteGuid = CacheHelper.GetCurrentSiteSettings().SiteGuid,
					Hostname = HttpContext.Current.Request.UserHostName
				};

				userLocation.Save();

				log.Debug($"Set UserLocation : {HttpContext.Current.Request.UserHostName}:{ip4}");
			}
			catch (Exception ex)
			{
				log.Error(ip4, ex);
			}
		}
	}
}