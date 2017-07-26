using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using System;
using System.Web.Mvc;
using static mojoPortal.Web.UI.Avatar;

// https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/views/creating-custom-html-helpers-cs

namespace mojoPortal.Web.Helpers
{
	public static class AvatarExtensions
	{
		public static string Avatar(
			this HtmlHelper helper,
			bool autoConfigure = false,
			string avatarFile = "",
			string defaultInternalAvatar = "~/Data/SiteImages/anonymous.png",
			bool disableUseLinkWithAutoConfigure = true,
			bool disable = false,
			string email = "",
			string extraCssClass = "",
			string gravatarFallbackEmailAddress = "",
			string imageUrl = "",
			string linkTitle = "Get your avatar",
			string linkUrl = "http://www.gravatar.com",
			bool outputGravatarSiteLink = true,
			string secureUrl = "https://secure.gravatar.com/avatar/",
			short size = 80,
			string standardUrl = "http://www.gravatar.com/avatar/",
			string target = "",
			bool useInternalDefaultForGravatar = true,
			bool useLink = false
		)
		{
			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			bool useGravatar = true;
			int siteId = siteSettings.SiteId;
			RatingType maxAllowedRating = SiteUtils.GetMaxAllowedGravatarRating();
			int userId = currentUser.UserId;

			if (avatarFile == string.Empty)
			{
				avatarFile = currentUser.AvatarUrl;
			}

			if (email == string.Empty)
			{
				email = currentUser.Email;
			}

			if (currentUser == null)
			{
				disable = true;
				return string.Empty;
			}

			if (disableUseLinkWithAutoConfigure)
			{
				useLink = false;
			}

			switch (siteSettings.AvatarSystem)
			{
				case "gravatar":
					useGravatar = true;
					disable = false;
					break;

				case "internal":
					useGravatar = false;
					disable = false;
					break;

				case "none":
				default:
					useGravatar = false;
					disable = true;
					break;
			}

			if (disable)
			{
				return string.Empty;
			}

			return "";
		}
	}
}