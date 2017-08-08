using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Net;


namespace mojoPortal.Web.Helpers
{
	/// <summary>
	/// A simple ASP.NET MVC helper for Gravatar providing extension methods to HtmlHelper and UrlHelper.
	/// </summary>
	public static class GravatarCommon
	{
		#region Default Images

		/// <summary>
		/// 404: do not load any image if none is associated with the email hash, instead return an HTTP 404 (File Not Found) response.
		/// </summary>
		public const string DefaultImage404 = "404";

		/// <summary>
		/// mm: (mystery-man) a simple, cartoon-style silhouetted outline of a person (does not vary by email hash).
		/// </summary>
		public const string DefaultImageMysteryMan = "mm";

		/// <summary>
		/// identicon: a geometric pattern based on an email hash.
		/// </summary>
		public const string DefaultImageIdenticon = "identicon";

		/// <summary>
		/// monsterid: a generated 'monster' with different colors, faces, etc.
		/// </summary>
		public const string DefaultImageMonsterId = "monsterid";

		/// <summary>
		/// wavatar: generated faces with differing features and backgrounds.
		/// </summary>
		public const string DefaultImageWavatar = "wavatar";

		/// <summary>
		/// retro: awesome generated, 8-bit arcade-style pixelated faces.
		/// </summary>
		public const string DefaultImageRetro = "retro";

		/// <summary>
		/// blank: a transparent PNG image
		/// </summary>
		public const string DefaultImageBlank = "blank";

		#endregion Default Images

		/// <summary>
		/// Minimum image size supported by gravatar.
		/// </summary>
		public const int MinImageSize = 1;

		/// <summary>
		/// Maximum image size supported by gravatar.
		/// </summary>
		public const int MaxImageSize = 2048;

		/// <summary>
		/// Gravatar HTTP url.
		/// </summary>
		private const string GravatarUrl = "//www.gravatar.com";

		/// <summary>
		/// Gravatar image path.
		/// </summary>
		private const string GravatarImagePath = "/avatar/";

		/// <summary>
		/// Gravatar profile path.
		/// </summary>
		private const string GravatarProfilePath = "/";

		/// <summary>
		/// Returns the Gravatar URL for the provided parameters.
		/// </summary>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="imageSize">Gravatar size in pixels.</param>
		/// <param name="defaultImage">The default image to use if the user does not have a Gravatar setup,
		///  can either be a url to an image or one of the DefaultImage* constants</param>
		/// <param name="rating">The content rating of the images to display.</param>
		/// <param name="addExtension">Whether to add the .jpg extension to the provided Gravatar.</param>
		/// <param name="forceDefault">Forces Gravatar to always serve the default image.</param>
		/// <param name="useSecureUrl">Whether to request the Gravatar over https.</param>
		/// <returns>The Gravatar URL for the provided parameters.</returns>
		public static string CreateGravatarUrl(string email, int imageSize, string defaultImage, GravatarRating? rating, bool? addExtension, bool? forceDefault)
		{
			// Limit our Gravatar size to be between the minimum and maximum sizes supported by Gravatar.
			imageSize = Math.Max(imageSize, MinImageSize);
			imageSize = Math.Min(imageSize, MaxImageSize);

			if (!string.IsNullOrWhiteSpace(defaultImage))
			{
				defaultImage = string.Concat("&d=", UrlEncode(defaultImage));
			}

			return $"{CreateGravatarBaseUrl(email, GravatarImagePath, addExtension.GetValueOrDefault(false) ? "jpg" : string.Empty)}?s={imageSize}{defaultImage}{(rating.HasValue ? string.Concat("&r=", rating) : string.Empty)}{(forceDefault.GetValueOrDefault(false) ? "&f=y" : string.Empty)}";
		}

		/// <summary>
		/// Returns the Gravatar profile URL for the provided parameters.
		/// </summary>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="extension">Extension to add to the url.</param>
		/// <param name="optionalParameters">Optional parameters to add to the url.</param>
		/// <param name="useSecureUrl">Whether to request the Gravatar over https.</param>
		/// <returns>The Gravatar profile URL for the provided parameters.</returns>
		public static string CreateGravatarProfileUrl(string email, string extension, IDictionary<string, string> optionalParameters)
		{
			var queryStringParameters = optionalParameters != null ?
					"?" + string.Join(
						"&",
						optionalParameters.Select(parameter => string.Concat(parameter.Key, "=", UrlEncode(parameter.Value.ToString()))))
					: string.Empty;

			return string.Concat(CreateGravatarBaseUrl(email, GravatarProfilePath, extension), queryStringParameters);
		}

		/// <summary>
		/// Creates a Gravatar hash for the provided email.
		/// </summary>
		/// <param name="email">The email to create the hash for</param>
		/// <returns>A Gravatar hash for the provided email.</returns>
		public static string CreateGravatarHash(string email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return string.Empty;
			}

			email = email.Trim();
			email = email.ToLower();

			// MD5 returns 128 bits which we represent in hexadecimal using 32 characters.
			var stringBuilder = new StringBuilder(32);

			using (var md5 = MD5.Create())
			{
				var hashedBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(email));

				foreach (var hashedByte in hashedBytes)
				{
					stringBuilder.Append(hashedByte.ToString("x2"));
				}
			}

			return stringBuilder.ToString();
		}

		/// <summary>
		/// Creates the base Gravatar URL shared by both Gravatar avatars and profiles.
		/// </summary>
		/// <param name="email">Email address to generate the Gravatar for.</param>
		/// <param name="basePath">The base path to use.</param>
		/// <param name="extension">Extension to add to the url.</param>
		/// <param name="useSecureUrl">Whether to request the Gravatar over https.</param>
		/// <returns>The Gravatar base URL for the provided parameters.</returns>
		private static string CreateGravatarBaseUrl(string email, string basePath, string extension)
		{
			return string.Concat(GravatarUrl, basePath, CreateGravatarHash(email), !string.IsNullOrWhiteSpace(extension) ? string.Concat(".", extension) : string.Empty);
		}


		private static string UrlEncode(string value)
		{
			return WebUtility.UrlEncode(value);
		}
	}
}