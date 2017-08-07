using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mojoPortal.Web.Helpers
{
	/// <summary>
	/// Gravatar HtmlHelper extension methods.
	/// </summary>
	public static class AvatarExtensions
	{
		/// <summary>
		/// Gets or sets the Func used to retrieve the HttpContext.
		/// </summary>
		/// <value>
		/// The get HTTP context.
		/// </value>
		internal static Func<HttpContextBase> GetHttpContext { get; set; } = () => new HttpContextWrapper(HttpContext.Current);

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize)
		{
			return CreateGravatarImage(email, imageSize, null, null, null, null, null);
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, null, null, null, null);
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, object htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, null, null, null, null, new RouteValueDictionary(htmlAttributes));
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, IDictionary<string, object> htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, null, null, null, null, htmlAttributes);
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage, object htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, null, null, null, new RouteValueDictionary(htmlAttributes));
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage, GravatarRating? rating, object htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, rating, null, null, new RouteValueDictionary(htmlAttributes));
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage, GravatarRating? rating, IDictionary<string, object> htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, rating, null, null, htmlAttributes);
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="addExtension">Whether to add the .jpg extension to the provided Gravatar.</param>
		/// <param name="forceDefault">Forces Gravatar to always serve the default image.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage, GravatarRating? rating, bool addExtension, bool forceDefault, object htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, rating, addExtension, forceDefault, new RouteValueDictionary(htmlAttributes));
		}

		/// <summary>
		/// Returns a Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="addExtension">Whether to add the .jpg extension to the provided Gravatar.</param>
		/// <param name="forceDefault">Forces Gravatar to always serve the default image.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Gravatar(this HtmlHelper helper, string email, int imageSize, string defaultImage, GravatarRating? rating, bool addExtension, bool forceDefault, IDictionary<string, object> htmlAttributes)
		{
			return CreateGravatarImage(email, imageSize, defaultImage, rating, addExtension, forceDefault, htmlAttributes);
		}

		/// <summary>
		/// Returns a Gravatar or internal avatar img tag for the provided parameters.
		/// </summary>
		/// <param name="helper">The HtmlHelper to extend.</param>
		/// <param name="avatarUrl">Internal custom avatar.</param>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <param name="linkAttributes">Object containing the HTML attributes to set for the link element.</param>
		/// <param name="useLink">Wrap img in link if true.</param>
		/// <param name="linkGravatarToUserProfile">Gravatar links to User Profile instead of Gravatar's site.</param>
		/// <param name="useGravatar">Force use of either Gravatar or internal avatar.</param>
		/// <param name="defaultImage">Custom internal avatar.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="addExtension">Whether to add the .jpg extension to the provided Gravatar.</param>
		/// <param name="forceDefault">Forces Gravatar to always serve the default image.</param>
		/// <returns>A Gravatar img tag for the provided parameters.</returns>
		[ExcludeFromCodeCoverage]
		public static MvcHtmlString Avatar(
			this HtmlHelper helper,
			string avatarUrl,
			string email,
			int userID = -1,
			object htmlAttributes = null,
			object linkAttributes = null,
			bool useLink = false,
			bool linkGravatarToUserProfile = true,
			bool useGravatar = true,
			string defaultImage = "~/Data/SiteImages/anonymous.png",
			int imageSize = 80,
			GravatarRating? rating = null,
			bool addExtension = false,
			bool forceDefault = true
		)
		{
			var disable = false;
			var user = SiteUtils.GetCurrentSiteUser();
			var siteSettings = CacheHelper.GetCurrentSiteSettings();
			var siteID = siteSettings.SiteId;
			var linkUrl = string.Empty;

			disable = avatarUrl == "disable" ? true : false;
			rating = (GravatarRating)SiteUtils.GetMaxAllowedGravatarRating();

			if (disable || siteSettings == null || siteSettings.AvatarSystem == "none")
			{
				return MvcHtmlString.Create(string.Empty);
			}

			if (siteSettings.AvatarSystem == "internal")
			{
				useGravatar = false;
			}

			dynamic newHtmlAttributes = new ExpandoObject();

			if (htmlAttributes != null)
			{
				foreach (var htmlAttribute in new RouteValueDictionary(htmlAttributes))
				{
					if (htmlAttribute.Key.ToLower().Contains("src")) { continue; }

					AddAttribute(newHtmlAttributes, htmlAttribute.Key, htmlAttribute.Value.ToString());
				}
			}

			dynamic newLinkAttributes = new ExpandoObject();
			newLinkAttributes.Rel = "nofollow";

			if (linkAttributes != null)
			{
				foreach (var linkAttribute in new RouteValueDictionary(linkAttributes))
				{
					if (linkAttribute.Key.ToLower().Contains("href")) { continue; }

					AddAttribute(newLinkAttributes, linkAttribute.Key, linkAttribute.Value.ToString());
				}
			}

			if (userID == -2 && user != null)
			{
				userID = user.UserId;
			}

			if (userID > -1 && useLink)
			{
				linkUrl = VirtualPathUtility.ToAbsolute("/ProfileView.aspx?userid=" + userID.ToInvariantString());
			}
			else
			{
				if (useGravatar)
				{
					linkGravatarToUserProfile = false;
				}
				else
				{
					useLink = false;
				}
			}

			#region Use Gravatar

			if (useGravatar)
			{
				defaultImage = WebUtils.ResolveServerUrl(defaultImage);

				if (defaultImage.Contains("localhost"))
				{
					defaultImage = string.Empty;
					forceDefault = false;
				}

				if (!useLink)
				{
					return CreateGravatarImage(email, imageSize, defaultImage, rating, addExtension, forceDefault, new RouteValueDictionary(newHtmlAttributes));
				}

				if (!linkGravatarToUserProfile)
				{
					linkUrl = "https://www.gravatar.com";
					newLinkAttributes.Target = "_blank";
					AddAttribute(newHtmlAttributes, "Title", "Get your avatar");
					AddAttribute(newLinkAttributes, "Title", "Get your avatar");
				}

				var gravatarLinkTag = CreateAvatarLink(linkUrl, new RouteValueDictionary(newLinkAttributes));
				gravatarLinkTag.InnerHtml = CreateGravatarImage(email, imageSize, defaultImage, rating, addExtension, forceDefault, new RouteValueDictionary(newHtmlAttributes)).ToString();

				return MvcHtmlString.Create(gravatarLinkTag.ToString());
			}

			#endregion Use Gravatar

			#region Use Internal Avatar

			if (siteID == -1 || string.IsNullOrEmpty(avatarUrl) || avatarUrl == "blank.gif")
			{
				defaultImage = VirtualPathUtility.ToAbsolute(defaultImage);
			}
			else
			{
				defaultImage = VirtualPathUtility.ToAbsolute("~/Data/Sites/" + siteID.ToInvariantString() + "/useravatars/" + avatarUrl);
			}

			if (!useLink)
			{
				return CreateInternalAvatarImage(defaultImage, new RouteValueDictionary(newHtmlAttributes));
			}

			var linkTag = CreateAvatarLink(linkUrl, new RouteValueDictionary(newLinkAttributes));
			linkTag.InnerHtml = CreateInternalAvatarImage(defaultImage, new RouteValueDictionary(newHtmlAttributes)).ToString();

			return MvcHtmlString.Create(linkTag.ToString());

			#endregion Use Internal Avatar

			void AddAttribute(ExpandoObject expando, string attributeName, object attributeValue)
			{
				var expandoDict = expando as IDictionary<string, object>;
				if (expandoDict.ContainsKey(attributeName))
				{
					expandoDict[attributeName] = attributeValue;
				}
				else
				{
					expandoDict.Add(attributeName, attributeValue);
				}
			}
		}

		/// <summary>
		/// Returns the Gravatar img tag for the provided parameters.
		/// </summary>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		/// can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="addExtension">Whether to add the .jpg extension to the provided Gravatar.</param>
		/// <param name="forceDefault">Forces Gravatar to always serve the default image.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>The Gravatar img tag for the provided parameters.</returns>
		internal static MvcHtmlString CreateGravatarImage(string email, int imageSize, string defaultImage, GravatarRating? rating, bool? addExtension, bool? forceDefault, IDictionary<string, object> htmlAttributes)
		{
			var httpContext = GetHttpContext();
			var httpRequest = httpContext.Request;

			var imgTag = new TagBuilder("img");
			imgTag.MergeAttribute("src", GravatarCommon.CreateGravatarUrl(email, imageSize, defaultImage, rating, addExtension, forceDefault));

			if (htmlAttributes != null)
			{
				foreach (var htmlAttribute in htmlAttributes)
				{
					imgTag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value.ToString());
				}
			}

			return MvcHtmlString.Create(imgTag.ToString());
		}

		/// <summary>
		/// Returns a hyperlink tag for the provided parameters.
		/// </summary>
		/// <param name="href">Href for link element.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the link element.</param>
		/// <returns>A hyperlink tag for the provided parameters.</returns>
		internal static TagBuilder CreateAvatarLink(string href, IDictionary<string, object> htmlAttributes = null)
		{
			var linkTag = new TagBuilder("a");
			linkTag.MergeAttribute("href", href);

			if (htmlAttributes != null)
			{
				foreach (var htmlAttribute in htmlAttributes)
				{
					linkTag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value.ToString());
				}
			}

			return linkTag;
		}

		/// <summary>
		/// Returns the internal avatar img tag for the provided parameters.
		/// </summary>
		/// <param name="imageUrl">Image src for img element.</param>
		/// <param name="htmlAttributes">Object containing the HTML attributes to set for the img element.</param>
		/// <returns>The internal avatar img tag for the provided parameters.</returns>
		internal static MvcHtmlString CreateInternalAvatarImage(string imageUrl, IDictionary<string, object> htmlAttributes = null)
		{
			var imgTag = new TagBuilder("img");
			imgTag.MergeAttribute("src", imageUrl);

			if (htmlAttributes != null)
			{
				foreach (var htmlAttribute in htmlAttributes)
				{
					imgTag.MergeAttribute(htmlAttribute.Key, htmlAttribute.Value.ToString());
				}
			}

			return MvcHtmlString.Create(imgTag.ToString());
		}
	}
}