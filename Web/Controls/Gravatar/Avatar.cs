// Author:
// Created:       2012-08-08
// Last Modified: 2019-01-09
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.UI
{
	/// <summary>
	/// encapsulates the logic for showing (or not) an avatar either using gravatar or the internal user upload avatar system
	/// based on bindable properties
	/// </summary>
	public class Avatar : WebControl
	{
		public const string SecureUrl = "https://secure.gravatar.com/avatar/";
		public const string StandardUrl = "http://www.gravatar.com/avatar/";

		public enum RatingType { G, PG, R, X }

		private string _email;

		public int SiteId { get; set; } = -1;

		public int UserId { get; set; } = -1;

		public string SiteRoot { get; set; } = string.Empty;

		/// <summary>
		/// if true uses gravatar, if false uses the internal site avatar system, default is true
		/// </summary>
		public bool UseGravatar { get; set; } = true;

		/// <summary>
		/// if true doesn't render at all
		/// </summary>
		public bool Disable { get; set; } = false;

		public string AvatarFile { get; set; } = string.Empty;

		/// <summary>
		/// The Email for the user
		/// </summary>
		[Bindable(true), Category("Appearance"), DefaultValue("")]
		public string Email
		{
			get { return _email; }
			set { _email = value.ToLower(); }
		}

		public string UserName { get; set; } = string.Empty;

		/// <summary>
		/// if specified the alt text for the image will use this format string and pass in the username
		/// </summary>
		public string UserNameTooltipFormat { get; set; } = string.Empty;

		/// <summary>
		/// Size of Gravatar image.  Must be between 1 and 512.
		/// </summary>
		[Bindable(true), Category("Appearance"), DefaultValue("80")]
		public short Size { get; set; } = 80;

		/// <summary>
		/// An optional "rating" parameter may follow with a value of [ G | PG | R | X ] that determines the highest rating (inclusive) that will be returned.
		/// </summary>
		public RatingType MaxAllowedRating { get; set; }

		/// <summary>
		/// Determines whether the image is wrapped in an anchor tag linking to the Gravatar sit
		/// </summary>
		public bool OutputGravatarSiteLink { get; set; } = true;

		/// <summary>
		/// Optional property for link title for gravatar website link
		/// </summary>
		public string LinkTitle { get; set; } = "Get your avatar";

		/// <summary>
		/// By default links to Gravatar so users can get a gravatar, but you can override it and link to a user profile for example
		/// </summary>
		public string LinkUrl { get; set; } = "http://www.gravatar.com";

		public string DefaultInternalAvatar { get; set; } = "~/Data/SiteImages/anonymous.png";

		/// <summary>
		/// if true uses the DefaultInternalAvatar for users who have no gravatar
		/// note that this doesn't work from localhost because gravatar cannot load the image
		/// </summary>
		public bool UseInternalDefaultForGravatar { get; set; } = true;

		public string Target { get; set; } = string.Empty;

		public string ImageUrl { get; set; } = string.Empty;

		/// <summary>
		/// Gets the base Url based on whether the request is secure or not.
		/// Added by  2008-08-13
		/// </summary>
		public string GravatarBaseUrl
		{
			get
			{
				if (Page.Request.IsSecureConnection) { return SecureUrl; }
				return StandardUrl;
			}
		}

		public bool UseLink { get; set; } = false;

		public bool AutoConfigure { get; set; } = false;

		public bool DisableUseLinkWithAutoConfigure { get; set; } = true;

		public string GravatarFallbackEmailAddress { get; set; } = string.Empty;

		public string ExtraCssClass { get; set; } = string.Empty;

		public bool LinkToPublicProfile { get; set; } = false;

		public bool LinkToPrivateProfile { get; set; } = false;

		public bool LinkToGravatar { get; set; } = true;

		public bool LinkToCustom { get; set; } = false;
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (!AutoConfigure) { return; }

			DoAutoConfiguration();
		}


		private void DoAutoConfiguration()
		{
			if (Disable)
			{
				return;
			}

			SiteUser currentUser = SiteUtils.GetCurrentSiteUser();

			if (currentUser == null)
			{
				Disable = true; return;
			}

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings == null)
			{
				Disable = true; return;
			}

			switch (siteSettings.AvatarSystem)
			{
				case "gravatar":
					UseGravatar = true;
					Disable = false;
					break;

				case "internal":
					UseGravatar = false;
					Disable = false;
					break;

				case "none":
				default:
					UseGravatar = false;
					Disable = true;
					break;
			}

			SiteId = siteSettings.SiteId;
			MaxAllowedRating = SiteUtils.GetMaxAllowedGravatarRating();
			UserId = currentUser.UserId;
			AvatarFile = currentUser.AvatarUrl;
			Email = currentUser.Email;

			if (DisableUseLinkWithAutoConfigure)
			{
				UseLink = false;
			}
		}


		protected override void Render(HtmlTextWriter output)
		{
			if (HttpContext.Current == null)
			{
				output.Write("[" + ID + "]");

				return;
			}

			if (Disable)
			{
				return;
			}

			if (CssClass.Length == 0)
			{
				CssClass = "avatar";
			}

			if (!string.IsNullOrWhiteSpace(ExtraCssClass))
			{
				CssClass += " " + ExtraCssClass;
			}

			if (UseGravatar)
			{
				if (string.IsNullOrEmpty(Email))
				{
					if (GravatarFallbackEmailAddress.Length > 0)
					{
						Email = GravatarFallbackEmailAddress;
					}
					else
					{
						return;
					}
				}

				RenderGravater(output);
			}
			else
			{
				RenderInternalAvatar(output);
			}
		}


		protected void RenderInternalAvatar(HtmlTextWriter output)
		{
			output.Write(GetAvatarMarkup());
		}

		private string GetAvatarMarkup()
		{
			if (!UseLink || (LinkUrl.Length == 0))
			{
				return "<img  alt='" + LinkTitle + "' src='" + GetInternalAvatarUrl() + "' class='" + CssClass + "' />";
			}

			return "<a rel='nofollow' href='" + GetLinkUrl() + "' class='" + CssClass + "'><img  alt='" + GetAltText() + "' src='" + GetInternalAvatarUrl() + "' /></a>";
		}

		/*
		 * 1/9/2019 - Added LinkTo... options in hopes to make this control a bit friendlier to skin developers
		 * while maintaing backwards compatibility. 
		 * The overall goal is to be able to link to a user's public or private profile from the skin, even if gravatar is used
		 * and have those links when mojo is installed in a virtual directory or the site is a folder-based tenant.
		 * 
		 */
		private string GetLinkUrl()
		{
			if (!LinkToGravatar && !LinkToCustom && UserId > -1)
			{
				if (LinkToPrivateProfile)
				{
					return SiteUtils.GetPrivateProfileUrl();
				}
				else if (LinkToPublicProfile)
				{
					return SiteUtils.GetPublicProfileUrl(UserId);
				}
			}

			return LinkUrl;
		}


		private string GetInternalAvatarUrl()
		{
			// if we get here we are using our own avatars
			if ((SiteId == -1) || (string.IsNullOrEmpty(AvatarFile)) || (AvatarFile == "blank.gif"))
			{
				return Page.ResolveUrl(DefaultInternalAvatar);
			}

			string userSiteId = SiteId.ToInvariantString();

			if (WebConfigSettings.UseRelatedSiteMode)
			{
				userSiteId = WebConfigSettings.RelatedSiteID.ToInvariantString();
			}

			return Page.ResolveUrl("~/Data/Sites/" + userSiteId + "/useravatars/" + AvatarFile);
		}


		private string GetAltText()
		{
			if (UserNameTooltipFormat.Length > 0)
			{
				return string.Format(CultureInfo.InvariantCulture, UserNameTooltipFormat, Page.Server.HtmlEncode(UserName));
			}

			return LinkTitle;
		}

		protected void RenderGravater(HtmlTextWriter output)
		{
			// output the default attributes:
			AddAttributesToRender(output);

			//if (UseLink)
			//{
			//	LinkUrl = GetLinkUrl();
			//}

			// if the size property has been specified, ensure it is a short, and in the range 
			// 1..512:
			try
			{
				// if it's not in the allowed range, throw an exception:
				if (Size < 1 || Size > 512)
				{
					throw new ArgumentOutOfRangeException();
				}
			}
			catch
			{
				Size = 80;
			}

			// default the image url:
			string imageUrl = GravatarBaseUrl;

			if (!string.IsNullOrEmpty(Email))
			{
				// build up image url, including MD5 hash for supplied email:
				MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

				UTF8Encoding encoder = new UTF8Encoding();
				MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();

				byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(Email));

				StringBuilder sb = new StringBuilder(hashedBytes.Length * 2);

				for (int i = 0; i < hashedBytes.Length; i++)
				{
					sb.Append(hashedBytes[i].ToString("X2"));
				}

				imageUrl += sb.ToString().ToLower();
				imageUrl += ".jpg?r=" + MaxAllowedRating.ToString();
				imageUrl += "&s=" + Size.ToString();
			}

			// output default parameter if specified
			if ((UseInternalDefaultForGravatar) && (DefaultInternalAvatar.Length > 0))
			{
				string defaultImageUrl = WebUtils.ResolveServerUrl(DefaultInternalAvatar);

				if (!defaultImageUrl.Contains("localhost"))
				{
					imageUrl += "&default=" + HttpUtility.UrlEncode(defaultImageUrl);
				}
			}

			// if we need to output the site link:
			if (OutputGravatarSiteLink)
			{
				output.AddAttribute(HtmlTextWriterAttribute.Href, GetLinkUrl());
				output.AddAttribute(HtmlTextWriterAttribute.Title, LinkTitle);

				if (CssClass.Length > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
				}

				if (Target.Length > 0)
				{
					output.AddAttribute(HtmlTextWriterAttribute.Target, Target);
				}

				output.RenderBeginTag(HtmlTextWriterTag.A);
			}

			// output required attributes/img tag:
			output.AddAttribute(HtmlTextWriterAttribute.Width, Size.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Height, Size.ToString());
			output.AddAttribute(HtmlTextWriterAttribute.Src, imageUrl);
			output.AddAttribute(HtmlTextWriterAttribute.Alt, "Gravatar");
			output.RenderBeginTag("img");
			output.RenderEndTag();

			// if we need to output the site link:
			if (OutputGravatarSiteLink)
			{
				output.RenderEndTag();
			}
		}
	}
}