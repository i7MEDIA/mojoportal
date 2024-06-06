using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using Brettle.Web.NeatHtml;
using log4net;
using mojoPortal.Core.Configuration;

namespace mojoPortal.Web.Framework;

public static class SecurityHelper
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SecurityHelper));

	public const string RegexRelativeImageUrlPatern = @"^(?i)/.*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";
	public const string RegexAnyImageUrlPatern = @"^(?i).*[_a-zA-Z0-9]+\.(png|jpg|jpeg|gif|PNG|JPG|JPEG|GIF)$";

	public const string RegexAnyHttpOrHttpsUrl = @"^(http|https)://([^\s]+)/?";

	//** Email Validation
	/// <summary>
	/// a regular expression for validating email addresses, efficient but not completely RFC 822 compliant
	/// </summary>
	public const string RegexEmailValidationPattern = @"^(?i)([0-9A-Z](['-.\w]*[_0-9A-Z])*@(([0-9A-Z])+([-\w']*[0-9A-Z])*\.)+[A-Z]{2,9})$|^$";

	// 2014-03-18 this expression hangs with patriciarichards5395@yahoo.comgetgoing
	// but is still the best one I've found because the others fail to accept foo+test@foo.com
	// @"^([0-9a-zA-Z](['-.\w]*[_0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$";

	// 2014-03-18 tried this one http://stackoverflow.com/questions/16167983/best-regular-expression-for-email-validation-in-c-sharp
	// @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z");
	// but it doesn't accept + signs like foo+test@gmail.com

	// http://stackoverflow.com/questions/201323/using-a-regular-expression-to-validate-an-email-address
	// ^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$
	// this one also fails to accept foo+test@foo.com

	/// <summary>
	/// it is better to use this method than to directly use the constant above becuase this allows you to override the experession if needed
	/// to solve a problem
	/// </summary>
	/// <returns></returns>
	public static string GetEmailRegexExpression()
	{
		string overrideRegex = ConfigHelper.GetStringProperty("CustomEmailRegex", string.Empty);
		if (!string.IsNullOrWhiteSpace(overrideRegex))
		{
			return overrideRegex;
		}

		return RegexEmailValidationPattern;
	}


	public static bool IsValidEmailAddress(string email)
	{
		if (string.IsNullOrWhiteSpace(email) || email.Length < 3)
		{
			//email addresses must be at least 3 characters
			//a@b would be a valid email address for an account named 'a' on a local email server for the 'b' domain
			return false;
		}

		try
		{
			var m = new MailAddress(email);

			return true;
		}
		catch (FormatException) { }

		return false;
	}


	// this one I modified from the one I was using for Register.aspx. I modified it to accept apostrophe
	//^([0-9a-zA-Z](['-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w']*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$

	// this is the one I was using on Secure/Register.aspx 
	//^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@(([0-9a-zA-Z])+([-\w]*[0-9a-zA-Z])*\.)+[a-zA-Z]{2,9})$
	// this is the one I was using on ManageUsers.aspx                  
	//^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$

	// this one allows apostrophes which are valid according to the spec
	//^(['_a-z0-9-]+)(\.['_a-z0-9-]+)*@([a-z0-9-]+)(\.[a-z0-9-]+)*(\.[a-z]{2,5})$

	//from http://www.regular-expressions.info/email.html, this is supposedly compliant with the rfc
	//(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])

	//***

	// /(\.|\/)(zip|gif|jpe?g|png)$/i

	public static string GetRegexValidationForAllowedExtensions(string pipeSeparatedExtensions)
	{
		var regex = new StringBuilder();

		// (([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG); *)*(([^.;]*[.])+(jpg|gif|png|JPG|GIF|PNG))?$

		regex.Append("(?i)(([^.;]*[.])+(");

		List<string> allowedExtensions = pipeSeparatedExtensions.SplitOnPipes();
		string pipe = string.Empty;
		foreach (string ext in allowedExtensions)
		{
			regex.Append(pipe + ext.Replace(".", string.Empty));
			pipe = "|";
		}

		regex.Append("); *)*(([^.;]*[.])+(");

		pipe = string.Empty;
		foreach (string ext in allowedExtensions)
		{
			regex.Append(pipe + ext.Replace(".", string.Empty));
			pipe = "|";
		}

		regex.Append("))?$");

		return regex.ToString();
	}


	public static string GetRegexValidationForAllowedExtensionsJqueryFileUploader(string pipeSeparatedExtensions)
	{
		var regex = new StringBuilder();

		// @"/(\.|\/)(gif|jpe?g|png)$/i"

		regex.Append(@"(?i)/(\.|\/)(");

		List<string> allowedExtensions = pipeSeparatedExtensions.SplitOnPipes();
		string pipe = string.Empty;
		foreach (string ext in allowedExtensions)
		{
			regex.Append(pipe + ext.Replace(".", string.Empty));
			pipe = "|";
		}

		regex.Append(")$/i");

		return regex.ToString();
	}


	//http://www.codeproject.com/KB/aspnet/LengthValidation.aspx
	public static string GetMaxLengthRegexValidationExpression(int length)
	{
		return Invariant($"[\\s\\S]{{0,{length}}}");
	}


	public static string PreventCrossSiteScripting(string html)
	{
		string errorHeader = ResourceHelper.GetMessageTemplate("NeatHtmlValidationErrorHeader.config");
		return PreventCrossSiteScripting(html, errorHeader);
	}


	public static string PreventCrossSiteScripting(string html, string errorHeader)
	{
		return PreventCrossSiteScripting(html, errorHeader, false);
	}


	public static string PreventCrossSiteScripting(string html, string errorHeader, bool removeMarkupOnFailure)
	{
		try
		{
			Filter filter = GetXssFilter();

			if (filter == null)
			{
				log.Info("XssFilter was null");
				return html.Replace("script", "s cript");
			}

			return filter.FilterUntrusted(html);
		}
		catch (Exception ex)
		{
			if (removeMarkupOnFailure)
			{
				return $"<span style=\"color: #ff0000;\">{errorHeader}</span><br />{HttpUtility.HtmlEncode(html.RemoveMarkup())}";
			}
			else
			{
				return $"<span style=\"color: #ff0000;\">{errorHeader} {HttpUtility.HtmlEncode(ex.Message)}</span><br />{HttpUtility.HtmlEncode(html)}";
			}
		}
	}


	public static string SanitizeHtml(string html)
	{
		var neathHtmlScript = ConfigHelper.GetStringProperty("NeatHtmlScriptUrl", "~/NeatHtml/NeatHtml.js");

		try
		{
			if (HttpContext.Current?.Handler is Page page)
			{
				if (!page.ClientScript.IsClientScriptBlockRegistered("NeatHtmlJs"))
				{
					page.ClientScript.RegisterClientScriptBlock(typeof(Page), "NeatHtmlJs", $"<script data-loader=\"SecurityHelper\" src=\"{Helpers.ApplyAppPathModifier(neathHtmlScript)}?guid={Guid.NewGuid()}\"></script>");
				}
			}

			Filter filter = GetXssFilter();

			if (filter == null)
			{
				log.Info("Filter was null");
				return html.RemoveMarkup();
			}

			return filter.FilterUntrusted(html);
		}
		catch (Exception)
		{
			return html.RemoveMarkup();
		}
	}


	private static Filter GetXssFilter()
	{
		if (HttpContext.Current == null)
		{
			return null;
		}

		string key = "xssfilter";

		if (HttpContext.Current.Items[key] != null)
		{
			//return (XssFilter)HttpContext.Current.Items[key];
			return (Filter)HttpContext.Current.Items[key];
		}
		else
		{
			//string schemaFolder = HttpContext.Current.Server.MapPath(WebUtils.GetApplicationRoot() + "/NeatHtml/schema");
			//string schemaFile = Path.Combine(schemaFolder, "NeatHtml.xsd");

			//XssFilter filter = XssFilter.GetForSchema(schemaFile);
			Filter filter = Filter.DefaultFilter;
			HttpContext.Current.Items[key] = filter;

			return filter;
		}
	}


	/// <summary>
	/// Checks string for possible XSS content.
	/// </summary>
	/// <param name="text"></param>
	/// <returns>True if string is possibly XSS attemp. False if string is clean.</returns>
	public static bool IsPossibleXss(string text)
	{
		if (text.RemoveMarkup() != text)
		{
			return true;
		}

		if (text.RemoveAngleBrackets() != text)
		{
			return true;
		}

		return false;
	}


	public static string GetRandomKey(int bytelength)
	{
		byte[] buff = new byte[bytelength];
		var rng = new RNGCryptoServiceProvider();
		rng.GetBytes(buff);
		var sb = new StringBuilder(bytelength * 2);
		for (int i = 0; i < buff.Length; i++)
		{
			sb.Append(string.Format("{0:X2}", buff[i]));
		}
		return sb.ToString();
	}


	//todo: modernize, ensure methods work across chromium, firefox, webkit
	public static void DisableBrowserCache()
	{
		if (HttpContext.Current != null)
		{
			HttpContext.Current.Response.Cache.SetExpires(new DateTime(1942, 12, 30, 12, 0, 0, DateTimeKind.Utc));
			HttpContext.Current.Response.Cache.SetNoStore();
			HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
		}
	}


	//todo: modernize, ensure methods work across chromium, firefox, webkit
	public static void DisableDownloadCache()
	{
		HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddYears(-1));

		// no-store makes firefox reload page
		// no-cache makes firefox reload page only over SSL
		// IE will fail when downloading a file over SSL if no-store or no-cache is set
		HttpBrowserCapabilities oBrowser = HttpContext.Current.Request.Browser;

		if (!oBrowser.Browser.ToLower().Contains("ie"))
		{
			HttpContext.Current.Response.Cache.SetNoStore();
		}

		//HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);

		// private cache allows IE to download over SSL with no-store set. 
		HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.Private);
		HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
		HttpContext.Current.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
	}
}