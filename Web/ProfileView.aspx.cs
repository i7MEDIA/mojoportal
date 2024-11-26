using System;
using System.Globalization;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages;

public partial class ProfileView : NonCmsBasePage
{
	private Guid userGuid = Guid.Empty;
	private int userID = -1;
	private Double timeOffset = 0;
	private TimeZoneInfo timeZone = null;
	private Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
	private bool allowGravatars = false;
	private bool disableAvatars = true;
	private string avatarPath = string.Empty;
	private SiteUser siteUser = null;
	private bool allowView = false;
	protected string allowedImageUrlRegexPattern = SecurityHelper.RegexRelativeImageUrlPatern;


	#region OnInit
	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);

		SuppressMenuSelection();
		SuppressPageMenu();
	}
	#endregion


	private void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();

		if (!allowView)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			else
			{
				WebUtils.SetupRedirect(this, SiteRoot);
				return;
			}
		}
		SiteUtils.ForceSsl();

		PopulateControls();
	}

	private void PopulateControls()
	{
		if (siteUser != null)
		{
			lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
			lblTotalPosts.Text = siteUser.TotalPosts.ToString(CultureInfo.InvariantCulture);

			lblUserName.Text = Server.HtmlEncode(siteUser.Name);

			Title = SiteUtils.FormatPageTitle(siteSettings, string.Format(CultureInfo.InvariantCulture,
				Resource.PageTitleFormatProfilePage, Server.HtmlEncode(siteUser.Name)));

			MetaDescription = string.Format(CultureInfo.InvariantCulture,
				Resource.ProfileViewMetaFormat, Server.HtmlEncode(siteUser.Name));

			userAvatar.UseGravatar = allowGravatars;
			userAvatar.Email = siteUser.Email;
			userAvatar.UserName = siteUser.Name;
			userAvatar.UserId = siteUser.UserId;
			userAvatar.AvatarFile = siteUser.AvatarUrl;
			userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
			userAvatar.Disable = disableAvatars;
			userAvatar.SiteId = siteSettings.SiteId;
			userAvatar.UseLink = false;

			if (disableAvatars) { divAvatar.Visible = false; }

			lnkUserPosts.UserId = siteUser.UserId;
			lnkUserPosts.TotalPosts = siteUser.TotalPosts;

			if (siteUser.TimeZoneId.Length > 0)
			{
				TimeZoneInfo userTz = SiteUtils.GetTimeZone(siteUser.TimeZoneId);
				if (userTz != null)
				{
					pnlTimeZone.Visible = true;

					if (userTz.IsDaylightSavingTime(DateTime.UtcNow))
					{
						lblTimeZone.Text = userTz.DaylightNameWithOffset();
					}
					else
					{
						lblTimeZone.Text = userTz.DisplayName;
					}
				}
			}

			if (WebConfigSettings.UseRelatedSiteMode)
			{
				// this can't be used in related site mode
				// because we can't assume forum posts were in this site.
				divForumPosts.Visible = false;
			}

			if (Request.IsAuthenticated)
			{
				ShowAuthenticatedProperties(siteUser);
			}
			else
			{
				ShowAnonymousProperties(siteUser);
			}
		}
		else
		{
			lblUserName.Text = Resource.UserNotFound;
			divAvatar.Visible = false;
		}
	}

	private void LoadSettings()
	{
		UntrustedContent2.TrustedImageUrlPattern = allowedImageUrlRegexPattern;

		allowView = WebUser.IsInRoles(siteSettings.RolesThatCanViewMemberList);
		userID = WebUtils.ParseInt32FromQueryString("userid", true, userID);
		timeOffset = SiteUtils.GetUserTimeOffset();
		timeZone = SiteUtils.GetUserTimeZone();
		userGuid = WebUtils.ParseGuidFromQueryString("u", Guid.Empty);

		if (userID > -1)
		{
			siteUser = new SiteUser(siteSettings, userID);
			if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
		}
		else if (userGuid != Guid.Empty)
		{
			siteUser = new SiteUser(siteSettings, userGuid);
			if (siteUser.UserGuid == Guid.Empty) { siteUser = null; }
		}

		switch (siteSettings.AvatarSystem)
		{
			case "gravatar":
				allowGravatars = true;
				disableAvatars = false;
				break;

			case "internal":
				allowGravatars = false;
				disableAvatars = false;
				break;

			case "none":
			default:
				allowGravatars = false;
				disableAvatars = true;
				break;
		}

		if (displaySettings.OverrideAvatarLabel.Length > 0)
		{
			lblAvatar.ConfigKey = displaySettings.OverrideAvatarLabel;
		}

		if (displaySettings.HidePostCount) { divForumPosts.Visible = false; }

		AddClassToBody("profileview");
	}

	private void ShowAuthenticatedProperties(SiteUser siteUser)
	{
		mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		if (profileConfig != null)
		{
			foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
			{
				// we are using the new TimeZoneInfo list but it doesn't work under Mono
				// this makes us skip the TimeOffsetHours setting from mojoProfile.config which is not used under windows
				if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }

				// we allow this to be configured as a profile property so it can be required for registration
				// but we don't need to load it here because we have a dedicated control for the property already
				if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey) { continue; }

				if (propertyDefinition.VisibleToAuthenticated
						&& (
								(propertyDefinition.OnlyAvailableForRoles.Length == 0)
								|| (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
							)
							&& (
								(propertyDefinition.OnlyVisibleForRoles.Length == 0)
									|| (WebUser.IsInRoles(propertyDefinition.OnlyVisibleForRoles))
							  )

					)
				{
					object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
					if (propValue != null)
					{
						mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
							pnlProfileProperties,
							propertyDefinition,
							propValue.ToString(),
							timeOffset,
							timeZone);
					}
					else
					{
						mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
							pnlProfileProperties,
							propertyDefinition,
						   propertyDefinition.DefaultValue,
							timeOffset,
							timeZone);
					}
				}
			}
		}
	}

	private void ShowAnonymousProperties(SiteUser siteUser)
	{
		bool wouldSeeMoreIfAuthenticated = false;

		mojoProfileConfiguration profileConfig = mojoProfileConfiguration.GetConfig();
		if (profileConfig != null)
		{
			foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
			{
				if (propertyDefinition.VisibleToAnonymous
					&& (propertyDefinition.OnlyVisibleForRoles.Length == 0)
					&& (
					(propertyDefinition.OnlyAvailableForRoles.Length == 0)
						|| (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
						)
					)
				{
					object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
					if (propValue != null)
					{
						mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
							pnlProfileProperties,
							propertyDefinition,
							propValue.ToString(),
							timeOffset,
							timeZone);
					}
					else
					{
						mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
							pnlProfileProperties,
							propertyDefinition,
							propertyDefinition.DefaultValue,
							timeOffset,
							timeZone);
					}
				}
				else
				{
					if (
						propertyDefinition.VisibleToAuthenticated
						&& (propertyDefinition.OnlyVisibleForRoles.Length == 0)
						&& (
						(propertyDefinition.OnlyAvailableForRoles.Length == 0)
							|| (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
							)
						)
					{
						wouldSeeMoreIfAuthenticated = true;
					}
				}
			}
		}

		if (wouldSeeMoreIfAuthenticated)
		{
			lblMessage.Text = ProfileResource.WouldSeeMoreIfAuthenticatedMessage;
		}
	}
}