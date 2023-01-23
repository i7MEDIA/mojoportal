using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.ProfileUpdatedHandlers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Configuration;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI.Pages
{
	public partial class UserProfile : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(UserProfile));
		private string userEmail = string.Empty;
		private SiteUser siteUser;
		private double timeOffset = 0;
		private TimeZoneInfo timeZone = null;
		private Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
		private bool allowGravatars = false;
		private bool disableAvatars = true;
		private CommerceConfiguration commerceConfig = null;
		private string rpxApiKey = string.Empty;
		private string rpxApplicationName = string.Empty;
		private bool allowUserSkin = false;


		protected override void OnPreInit(EventArgs e)
		{
			base.OnPreInit(e);

		}


		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Load += new EventHandler(Page_Load);
			btnUpdate.Click += new EventHandler(btnUpdate_Click);
			btnUpdateAvartar.Click += new System.Web.UI.ImageClickEventHandler(btnUpdateAvartar_Click);

			if (WebConfigSettings.HideAllMenusOnProfilePage)
			{
				SuppressAllMenus();
			}
			else if (WebConfigSettings.HidePageMenuOnProfilePage)
			{
				SuppressPageMenu();
			}

			SuppressMenuSelection();
			SetupAvatarScript();
		}

		#endregion


		private void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);

				return;
			}

			if (SiteUtils.SslIsAvailable())
			{
				SiteUtils.ForceSsl();
			}

			SecurityHelper.DisableBrowserCache();

			if (!WebConfigSettings.AllowUserProfilePage)
			{
				SiteUtils.RedirectToAccessDeniedPage();

				return;
			}

			siteUser = SiteUtils.GetCurrentSiteUser();
			LoadSettings();

			PopulateProfileControls();

			if (siteUser != null)
			{
				purchaseHx.UserGuid = siteUser.UserGuid;
			}

			PopulateLabels();

			if (!IsPostBack)
			{
				PopulateControls();
			}
		}


		private void PopulateControls()
		{
			lnkChangePassword.NavigateUrl = SiteRoot + "/Secure/ChangePassword.aspx";
			lnkChangePassword.Text = Resource.UserChangePasswordLabel;

			ListItem listItem;

			if (allowUserSkin && siteUser != null)
			{
				SkinSetting.SetValue(siteUser.Skin);
			}

			if (
				siteSettings.AllowUserEditorPreference &&
				siteUser != null &&
				siteUser.EditorPreference.Length > 0
			)
			{
				listItem = ddEditorProviders.Items.FindByValue(siteUser.EditorPreference);

				if (listItem != null)
				{
					ddEditorProviders.ClearSelection();
					listItem.Selected = true;
				}
			}

			if (siteUser != null)
			{
				if (timeZoneSetting is ISettingControl setting)
				{
					setting.SetValue(siteUser.TimeZoneId);
				}

				txtName.Text = SecurityHelper.RemoveMarkup(siteUser.Name);
				txtName.Enabled = siteSettings.AllowUserFullNameChange;
				lblLoginName.Text = SecurityHelper.RemoveMarkup(siteUser.LoginName);
				txtEmail.Text = siteUser.Email;
				lblOpenID.Text = siteUser.OpenIdUri;
				txtPasswordQuestion.Text = siteUser.PasswordQuestion;
				txtPasswordAnswer.Text = siteUser.PasswordAnswer;
				lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
				lblTotalPosts.Text = siteUser.TotalPosts.ToString();
				lnkUserPosts.UserId = siteUser.UserId;
				lnkUserPosts.TotalPosts = siteUser.TotalPosts;
				lnkPubProfile.NavigateUrl = SiteRoot + "/ProfileView.aspx?userid=" + siteUser.UserId.ToInvariantString();

				if (divLiveMessenger.Visible)
				{
					WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
					WindowsLiveMessenger m = new WindowsLiveMessenger(wl);

					if (WebConfigSettings.TestLiveMessengerDelegation)
					{
						lnkAllowLiveMessenger.NavigateUrl = m.ConsentOptInUrl;
					}
					else
					{
						lnkAllowLiveMessenger.NavigateUrl = m.NonDelegatedSignUpUrl;
					}

					if (siteUser.LiveMessengerId.Length > 0)
					{
						chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
						chkEnableLiveMessengerOnProfile.Enabled = true;
					}
					else
					{
						chkEnableLiveMessengerOnProfile.Checked = false;
						chkEnableLiveMessengerOnProfile.Enabled = false;
					}
				}

				userAvatar.UseGravatar = allowGravatars;
				userAvatar.Email = siteUser.Email;
				userAvatar.UserName = siteUser.Name;
				userAvatar.UserId = siteUser.UserId;
				userAvatar.AvatarFile = siteUser.AvatarUrl;
				userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
				userAvatar.Disable = disableAvatars;
				userAvatar.SiteId = siteSettings.SiteId;
				userAvatar.UseLink = false;
			}
		}


		private void PopulateProfileControls()
		{
			if (siteUser == null)
			{
				return;
			}

			var profileConfig = mojoProfileConfiguration.GetConfig();

			if (profileConfig != null)
			{
				foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
				{
					// we allow this to be configured as a profile property so it can be required for registration
					// but we don't need to load it here because we have a dedicated control for the property already
					if (propertyDefinition.Name == mojoProfilePropertyDefinition.TimeZoneIdKey)
					{
						continue;
					}

					if (
						 propertyDefinition.VisibleToUser &&
						 (
							propertyDefinition.OnlyAvailableForRoles.Length == 0 ||
							siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles)
						)
					)
					{
						object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);

						if (propValue != null)
						{
							if (propertyDefinition.EditableByUser)
							{
								mojoProfilePropertyDefinition.SetupPropertyControl(
									this,
									pnlProfileProperties,
									propertyDefinition,
									propValue.ToString(),
									timeOffset,
									timeZone,
									SiteRoot
								);
							}
							else
							{
								mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
									pnlProfileProperties,
									propertyDefinition,
									propValue.ToString(),
									timeOffset,
									timeZone
								);
							}
						}
						else
						{
							if (propertyDefinition.EditableByUser)
							{
								mojoProfilePropertyDefinition.SetupPropertyControl(
									this,
									pnlProfileProperties,
									propertyDefinition,
									propertyDefinition.DefaultValue,
									timeOffset,
									timeZone,
									SiteRoot
								);
							}
							else
							{
								mojoProfilePropertyDefinition.SetupReadOnlyPropertyControl(
									pnlProfileProperties,
									propertyDefinition,
									propertyDefinition.DefaultValue,
									timeOffset,
									timeZone
								);
							}
						}
					}
				}
			}
		}


		private void btnUpdate_Click(object sender, EventArgs e)
		{
			if (siteUser != null)
			{
				Page.Validate("profile");

				if (Page.IsValid)
				{
					UpdateUser();
				}
			}
		}


		private void UpdateUser()
		{
			userEmail = siteUser.Email;

			if (
				siteUser.Email != txtEmail.Text &&
				SiteUser.EmailExistsInDB(siteSettings.SiteId, siteUser.UserId, txtEmail.Text)
			)
			{
				lblErrorMessage.Text = Resource.DuplicateEmailMessage;

				return;
			}

			if (
				siteSettings.AllowUserEditorPreference &&
				divEditorPreference.Visible
			)
			{
				siteUser.EditorPreference = ddEditorProviders.SelectedValue;
			}

			if (siteSettings.AllowUserFullNameChange)
			{
				siteUser.Name = txtName.Text;
			}

			siteUser.Email = txtEmail.Text;

			if (WebConfigSettings.LogIpAddressForEmailChanges)
			{
				if (siteUser.UserId != -1 && userEmail != siteUser.Email)
				{
					log.Info("email for user changed from " + userEmail + " to " + siteUser.Email + " from ip address " + SiteUtils.GetIP4Address());
				}
			}

			if (pnlSecurityQuestion.Visible)
			{
				siteUser.PasswordQuestion = txtPasswordQuestion.Text;
				siteUser.PasswordAnswer = txtPasswordAnswer.Text;
			}
			else
			{
				// in case it is ever changed later to require password question and answer after making it not required
				// we need to ensure there is some question and answer.
				if (siteUser.PasswordQuestion.Length == 0)
				{
					siteUser.PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion;
					siteUser.PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer;
				}
			}

			if (siteUser.LiveMessengerId.Length > 0)
			{
				siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;
			}
			else
			{
				siteUser.EnableLiveMessengerOnProfile = false;
			}

			if (allowUserSkin)
			{
				siteUser.Skin = SkinSetting.GetValue();
			}

			if (timeZoneSetting is ISettingControl setting)
			{
				siteUser.TimeZoneId = setting.GetValue();
			}

			siteUser.PasswordFormat = siteSettings.PasswordFormat;

			if (siteUser.Save())
			{
				var profileConfig = mojoProfileConfiguration.GetConfig();

				foreach (mojoProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
				{
					if (
						propertyDefinition.EditableByUser &&
						(
							propertyDefinition.OnlyAvailableForRoles.Length == 0 ||
							WebUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles)
						)
					)
					{
						mojoProfilePropertyDefinition.SaveProperty(
							siteUser,
							pnlProfileProperties,
							propertyDefinition,
							timeOffset,
							timeZone
						);
					}
				}

				siteUser.UpdateLastActivityTime();

				if (
					userEmail != siteUser.Email &&
					siteSettings.UseEmailForLogin &&
					!siteSettings.UseLdapAuth
				)
				{
					FormsAuthentication.SetAuthCookie(siteUser.Email, false);
				}

				var updateEvent = new ProfileUpdatedEventArgs(siteUser, false);

				OnUserUpdated(updateEvent);

				SiteUtils.SetSkinCookie(siteUser);
				WebUtils.SetupRedirect(this, Request.RawUrl);

				return;
			}
		}


		protected void OnUserUpdated(ProfileUpdatedEventArgs e)
		{
			foreach (ProfileUpdatedHandlerProvider handler in ProfileUpdatedHandlerProviderManager.Providers)
			{
				handler.ProfileUpdatedHandler(null, e);
			}
		}


		private void LoadSettings()
		{
			divTimeZone.Visible = true;
			QuestionRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;
			AnswerRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;


			switch (siteSettings.AvatarSystem)
			{
				case "gravatar":
					allowGravatars = true;
					disableAvatars = false;

					break;

				case "internal":
					allowGravatars = false;
					disableAvatars = false;

					if (siteUser != null)
					{
						lnkAvatarUpld.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + siteUser.UserId.ToInvariantString();
					}

					if (WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
					{
						lnkAvatarUpld.Visible = false;
						avatarHelp.Visible = false;
					}

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

			if (displaySettings.HidePostCount)
			{
				divForumPosts.Visible = false;
			}

			allowUserSkin = siteSettings.AllowUserSkins || WebConfigSettings.AllowEditingSkins && WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins);

			if (allowUserSkin)
			{
				divSkin.Visible = true;
				ScriptConfig.IncludeColorBox = true;
			}
			else
			{
				divSkin.Visible = false;
				SkinSetting.Visible = false;
			}

			if (siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap)
			{
				lnkChangePassword.Visible = false;
			}

			divEditorPreference.Visible = siteSettings.AllowUserEditorPreference;

			timeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();

			divOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

			rpxApiKey = siteSettings.RpxNowApiKey;
			rpxApplicationName = siteSettings.RpxNowApplicationName;

			if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
			{
				if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
				{
					rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
				}

				if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
				{
					rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
				}
			}

			if (rpxApiKey.Length > 0)
			{
				lnkOpenIDUpdate.Visible = false;
				rpxLink.Visible = divOpenID.Visible;
			}

			if (!mojoSetup.RunningInFullTrust())
			{
				divOpenID.Visible = false;
			}

			if (
				WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger ||
				!siteSettings.AllowWindowsLiveMessengerForMembers ||
				siteSettings.WindowsLiveAppId.Length == 0 &&
				ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"] == null
			)
			{
				divLiveMessenger.Visible = false;
			}

			var countOfNewsLetters = LetterInfo.GetCount(siteSettings.SiteGuid);

			liNewsletters.Visible = WebConfigSettings.EnableNewsletter && countOfNewsLetters > 0;
			tabNewsletters.Visible = liNewsletters.Visible;
			newsLetterPrefs.Visible = liNewsletters.Visible;

			commerceConfig = SiteUtils.GetCommerceConfig();

			if (!commerceConfig.IsConfigured)
			{
				liOrderHistory.Visible = false;
				tabOrderHistory.Visible = false;
			}

			if (
				WebConfigSettings.UserNameValidationExpression.Length > 0 &&
				siteSettings.AllowUserFullNameChange
			)
			{
				regexUserName.ValidationExpression = WebConfigSettings.UserNameValidationExpression;
				regexUserName.ErrorMessage = WebConfigSettings.UserNameValidationWarning;
				regexUserName.Enabled = true;
			}

			FailSafeUserNameValidator.ErrorMessage = Resource.UserNameHasInvalidCharsWarning;
			FailSafeUserNameValidator.ServerValidate += new ServerValidateEventHandler(FailSafeUserNameValidator_ServerValidate);

			AddClassToBody("userprofile");
		}


		private void FailSafeUserNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
		{
			if (args.Value.Contains("<"))
			{
				args.IsValid = false;
			}

			if (args.Value.Contains(">"))
			{
				args.IsValid = false;
			}

			if (args.Value.Contains("/"))
			{
				args.IsValid = false;
			}

			if (args.Value.IndexOf("script", StringComparison.InvariantCultureIgnoreCase) > -1)
			{
				args.IsValid = false;
			}
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.ProfileLink);

			litSecurityTab.Text = $"<a href='#tabSecurity'>{Resource.UserProfileSecurityTab}</a>";
			litProfileTab.Text = $"<a href='#{tabProfile.ClientID}'>{Resource.UserProfileProfileTab}</a>";
			litOrderHistoryTab.Text = $"<a href='#{tabOrderHistory.ClientID}'>{Resource.CommerceOrderHistoryTab}</a>";
			litNewsletterTab.Text = $"<a href='#{tabNewsletters.ClientID}'>{Resource.UserProfileNewslettersTab}</a>";

			lnkAllowLiveMessenger.Text = Resource.EnableLiveMessengerLink;

			btnUpdate.Text = Resource.UserProfileUpdateButton;
			SiteUtils.SetButtonAccessKey(btnUpdate, AccessKeys.UserProfileSaveButtonAccessKey);

			if (siteSettings.AllowUserEditorPreference && ddEditorProviders.Items.Count == 0)
			{
				ddEditorProviders.DataSource = EditorManager.Providers;
				ddEditorProviders.DataBind();

				foreach (ListItem providerItem in ddEditorProviders.Items)
				{
					providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
				}

				var listItem = new ListItem
				{
					Value = string.Empty,
					Text = Resource.SiteDefaultEditor
				};

				ddEditorProviders.Items.Insert(0, listItem);
			}

			lnkPubProfile.Text = Resource.PublicProfileLink;
			lnkPubProfile.ToolTip = Resource.PublicProfileLink;

			rfvName.ErrorMessage = Resource.UserProfileNameRequired;
			regexEmail.ErrorMessage = Resource.UserProfileEmailValidation;
			rfvEmail.ErrorMessage = Resource.UserProfileEmailRequired;

			QuestionRequired.ErrorMessage = Resource.RegisterSecurityQuestionRequiredMessage;
			AnswerRequired.ErrorMessage = Resource.RegisterSecurityAnswerRequiredMessage;

			lnkAvatarUpld.Text = Resource.UploadAvatarLink;
			lnkAvatarUpld.ToolTip = Resource.UploadAvatarLink;

			btnUpdateAvartar.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
			btnUpdateAvartar.Attributes.Add("tabIndex", "-1");

			if (allowGravatars)
			{
				avatarHelp.Visible = false;
				lnkAvatarUpld.Visible = false;
			}
			else
			{
				if (disableAvatars)
				{
					divAvatar.Visible = false;
					avatarHelp.Visible = false;
					lnkAvatarUpld.Visible = false;
				}
				else
				{
					if (!WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
					{
						avatarHelp.Visible = true;
					}
				}
			}

			lnkOpenIDUpdate.Text = Resource.OpenIDUpdateButton;
			lnkOpenIDUpdate.ToolTip = Resource.OpenIDUpdateButton;
			lnkOpenIDUpdate.NavigateUrl = SiteRoot + "/Secure/UpdateOpenID.aspx";

			rpxLink.OverrideText = Resource.OpenIDUpdateButton;

			pnlSecurityQuestion.Visible = siteSettings.RequiresQuestionAndAnswer;
		}


		private void btnUpdateAvartar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
		{
			// this is fired when the avatar upload dialog is closed
			// we don't really know for sure if the image was updated
			// but if it was we should rename it since the previous version may be cached by web browsers
			// so we'll check if the files was modified recently, and if so rename it
			if (siteUser != null && siteUser.AvatarUrl.Length > 0)
			{
				var p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];

				if (p != null)
				{
					IFileSystem fileSystem = p.GetFileSystem();
					string avatarBasePath = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/useravatars/";
					WebFile avatarFile = fileSystem.RetrieveFile(avatarBasePath + siteUser.AvatarUrl);

					if (avatarFile != null)
					{
						if (avatarFile.Modified > DateTime.Today)
						{
							// it was updated today so we'll assume it was just now since the avatar dialog just closed
							string newfileName = "user"
								+ siteUser.UserId.ToInvariantString()
								+ "-" + siteUser.Name.ToCleanFileName()
								+ "-" + DateTime.UtcNow.Millisecond.ToInvariantString()
								+ System.IO.Path.GetExtension(siteUser.AvatarUrl);

							fileSystem.MoveFile(avatarBasePath + siteUser.AvatarUrl, avatarBasePath + newfileName, true);

							siteUser.AvatarUrl = newfileName;

							siteUser.Save();
						}
					}
				}
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		private void SetupAvatarScript()
		{
			var script = $@"
<script data-loader=""UserProfile.aspx.cs"">
	$('#{lnkAvatarUpld.ClientID}').colorbox({{
		width: '80%',
		height: '80%',
		iframe: true,
		onClosed: () => {{
			const btn = document.querySelector('#{btnUpdateAvartar.ClientID}');

			if (btn !== null) {{
				btn.click();
			}}
		}}
	}});
</script>";

			Page.ClientScript.RegisterStartupScript(GetType(), "cbupinit", script, false);
		}
	}
}
