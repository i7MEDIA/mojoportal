using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;

// Source: http://www.freshclickmedia.com/blog/2008/02/gravatar-aspnet-control/
// 2008-08-13  added to this project and changed namespace to be more convenient
// 2008-08-13  added support for SSL by returning the secure url if request is secure
namespace mojoPortal.Web.Controls;

[DefaultProperty("Email"), ToolboxData("<{0}:Gravatar runat=server></{0}:Gravatar>")]
public class Gravatar : System.Web.UI.WebControls.WebControl
{
	public const string SecureUrl = "https://secure.gravatar.com/avatar/";
	public const string StandardUrl = "http://www.gravatar.com/avatar/";

	public enum RatingType { G, PG, R, X }

	private string _email;
	private string _defaultImage;
	private string _linkUrl = "http://www.gravatar.com";

	/// <summary>
	/// The Email for the user
	/// </summary>
	[Bindable(true), Category("Appearance"), DefaultValue("")]
	public string Email
	{
		get { return _email; }
		set { _email = value.ToLower(); }
	}

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
	public string LinkUrl
	{
		get { return _linkUrl; }
		set { _linkUrl = value; }
	}

	/// <summary>
	/// An optional "default" parameter may follow that specifies the full, URL encoded URL, protocol included, of a GIF, JPEG, or PNG image that should be returned if either the requested email address has no associated gravatar, or that gravatar has a rating higher than is allowed by the "rating" parameter.
	/// </summary>
	[Bindable(true), Category("Appearance"), DefaultValue("")]
	public string DefaultImage
	{
		get { return _defaultImage; }
		set { _defaultImage = value; }
	}

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

	protected override void Render(HtmlTextWriter output)
	{
		if (HttpContext.Current == null)
		{
			output.Write("[" + this.ID + "]");
			return;
		}

		// output the default attributes:
		AddAttributesToRender(output);

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
		//string imageUrl = "http://www.gravatar.com/avatar.php?";
		// changes by  2008-08-13
		string imageUrl = GravatarBaseUrl;

		if (!string.IsNullOrEmpty(Email))
		{
			// build up image url, including MD5 hash for supplied email:
			var md5 = new MD5CryptoServiceProvider();

			var encoder = new UTF8Encoding();
			var md5Hasher = new MD5CryptoServiceProvider();

			byte[] hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(Email));

			var sb = new StringBuilder(hashedBytes.Length * 2);
			for (int i = 0; i < hashedBytes.Length; i++)
			{
				sb.Append(hashedBytes[i].ToString("X2"));
			}

			// changes by  2008-08-13
			imageUrl += sb.ToString().ToLower() + $".jpg?r={MaxAllowedRating}&s={Size}";
		}

		// output default parameter if specified
		if (!string.IsNullOrEmpty(DefaultImage))
		{
			imageUrl += $"&default={HttpUtility.UrlEncode(DefaultImage)}";
		}

		// if we need to output the site link:
		if (OutputGravatarSiteLink)
		{
			output.AddAttribute(HtmlTextWriterAttribute.Href, LinkUrl);
			output.AddAttribute(HtmlTextWriterAttribute.Title, LinkTitle);
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