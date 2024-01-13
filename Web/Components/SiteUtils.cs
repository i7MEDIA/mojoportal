using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.Core.Helpers;
using mojoPortal.FileSystem;
using mojoPortal.Net;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using static System.FormattableString;
using Config = mojoPortal.Core.Configuration;

namespace mojoPortal.Web
{
	public static class SiteUtils
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SiteUtils));
		private static bool debugLog = log.IsDebugEnabled;

		public static bool UsingIntegratedPipeline()
		{
			try
			{
				// new as of .NET 3.5 SP1
				return HttpRuntime.UsingIntegratedPipeline;
			}
			catch (MissingMethodException)
			{
				return false;
			}
			catch (MissingMemberException)
			{
				return false;
			}
		}

		/// <summary>
		/// compares 2 urls, if running on Mono does a case sensitive match
		/// else it does caseinsenitive match
		/// returns false if either string isnullorempty
		/// </summary>
		/// <param name="url1"></param>
		/// <param name="url2"></param>
		/// <returns></returns>
		public static bool UrlsMatch(string url1, string url2)
		{
			if (string.IsNullOrEmpty(url1)) { return false; }
			if (string.IsNullOrEmpty(url2)) { return false; }

			return string.Equals(url1, url2, StringComparison.InvariantCultureIgnoreCase);

		}

		//Updated 2011-10-04 based on suggestions by Warner
		//http://www.mojoportal.com/Forums/Thread.aspx?thread=9176&mid=34&pageid=5&ItemID=9&pagenumber=1#post38114

		public static string SuggestFriendlyUrl(
			string pageName,
			SiteSettings siteSettings,
			string prefix = "")
		{
			string friendlyUrl = CleanStringForUrl(prefix, false) + CleanStringForUrl(pageName);
			if (WebConfigSettings.AlwaysUrlEncode)
			{
				friendlyUrl = HttpUtility.UrlEncode(friendlyUrl);
			}

			string urlTail = string.Empty;
			switch (siteSettings.DefaultFriendlyUrlPattern)
			{
				case SiteSettings.FriendlyUrlPattern.PageNameWithDotASPX:
					urlTail = ".aspx";
					break;

			}

			string tempFriendlyUrl = friendlyUrl;
			int i = 1;

			while (FriendlyUrl.Exists(siteSettings.SiteId, tempFriendlyUrl + urlTail) || SiteFolder.Exists(tempFriendlyUrl))
			{
				tempFriendlyUrl = friendlyUrl + "-" + i.ToString();
				i++;
			}

			friendlyUrl = tempFriendlyUrl + urlTail;

			if (WebConfigSettings.ForceFriendlyUrlsToLowerCase) { return friendlyUrl.ToLower(); }

			return friendlyUrl;
		}

		/// <summary>
		/// to help mitigate cross site request forgery
		/// </summary>
		/// <param name="page"></param>
		/// <returns></returns>
		public static bool IsFishyPost(Page page)
		{
			return IsFishyPost(page.Request);

		}

		public static bool IsFishyPost(HttpRequest request)
		{
			if (!WebConfigSettings.CheckFishyReferrer)
			{
				return false; // a way to disable this if it causes problems
			}

			if (request.RequestType == "GET") { return false; } //not a postback

			if (!request.UrlReferrer.IsWellFormedOriginalString())
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.UrlReferrer.ToString()));
				}

				return true; //bogus referer
			}

			if (!request.UrlReferrer.Host.Equals(request.Url.Host))
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.UrlReferrer.ToString()));
				}

				return true; //referer is different host
			}

			if (Regex.IsMatch(request.UrlReferrer.ToString(), "%3C", RegexOptions.IgnoreCase))
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.UrlReferrer.ToString()));
				}

				return true; //referrer has angle brakcets
			}

			//ok
			return false;
		}

		public static bool IsFishyPost(HttpRequestMessage request)
		{
			if (!WebConfigSettings.CheckFishyReferrer)
			{
				return false; // a way to disable this if it causes problems
			}

			if (!request.RequestUri.IsWellFormedOriginalString())
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.RequestUri.ToString()));
				}

				return true; //bogus referer
			}

			if (!request.RequestUri.Host.Equals(request.RequestUri.Host))
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.RequestUri.ToString()));
				}
				return true; //referer is different host
			}

			if (Regex.IsMatch(request.RequestUri.ToString(), "%3C", RegexOptions.IgnoreCase))
			{
				if (WebConfigSettings.LogFishyReferrer)
				{
					log.Info(string.Format(Resource.FishyPostFoundFromReferrer, request.RequestUri.ToString()));
				}

				return true; //referrer has angle brakcets
			}

			return false; // OK
		}

		public static string RemoveInvalidUrlChars(string input)
		{
			return input.Replace(":", string.Empty).Replace("?", string.Empty).Replace("&", "-").Replace("#", string.Empty);
		}

		public static string RemoveQuotes(string input)
		{
			string outputString = input.Replace("\"", string.Empty).Replace("'", string.Empty);
			return outputString;
		}

		public static string CleanStringForUrl(string input, bool removeForwardSlash = true)
		{
			string outputString = RemovePunctuation(input.Replace("&", "-")).Replace("\\", "-").Replace(" - ", "-").Replace("--", "-").Replace(" ", "-").Replace("\"", string.Empty).Replace("'", string.Empty).Replace("#", string.Empty).Replace("~", string.Empty).Replace("`", string.Empty).Replace("@", string.Empty).Replace("$", string.Empty).Replace("*", string.Empty).Replace("^", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty).Replace("+", string.Empty).Replace("=", string.Empty).Replace("%", string.Empty).Replace(">", string.Empty).Replace("<", string.Empty);

			if (removeForwardSlash)
			{
				outputString = outputString.Replace("/", string.Empty);
			}

			if (WebConfigSettings.UseClosestAsciiCharsForUrls) { return outputString.ToAsciiIfPossible(); }

			return outputString;
		}

		private static string RemovePunctuation(string input)
		{
			string outputString = string.Empty;
			if (input is not null)
			{
				outputString = input.Replace(".", string.Empty).Replace(",", string.Empty).Replace(":", string.Empty).Replace("?", string.Empty).Replace("!", string.Empty).Replace(";", string.Empty).Replace("&", string.Empty).Replace("{", string.Empty).Replace("}", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
			}
			return outputString;
		}

		public static string GetReturnUrlParam(Page page, string siteRoot)
		{
			// only allow return urls that are relative or start with the site root
			//http://www.mojoportal.com/Forums/Thread.aspx?thread=5314&mid=34&pageid=5&ItemID=2&pagenumber=1#post22121

			if (page.Request.Params.Get("returnurl") is not null)
			{
				string returnUrlParam = page.Request.Params.Get("returnurl");
				if (returnUrlParam.Contains(","))
				{
					returnUrlParam = returnUrlParam.Substring(0, returnUrlParam.IndexOf(","));
				}
				if (!string.IsNullOrEmpty(returnUrlParam))
				{
					returnUrlParam = SecurityHelper.RemoveMarkup(returnUrlParam);
					string returnUrl = page.ResolveUrl(SecurityHelper.RemoveMarkup(page.Server.UrlDecode(returnUrlParam)));
					if ((returnUrl.StartsWith("/")) && (!(returnUrl.StartsWith("//"))))
					{
						return returnUrl;
					}

					if (returnUrl.StartsWith(siteRoot) || returnUrl.StartsWith(siteRoot.Replace("https://", "http://")))
					{
						return returnUrl;
					}
				}
			}

			return string.Empty;
		}

		public static string GetUrlWithQueryParams(string pageUrl, int siteId = -1, int pageId = -1, int moduleId = -1, int itemId = -1, bool includeSiteRoot = true)
		{
			var queryParams = new StringBuilder("?");
			if (siteId != -1)
			{
				queryParams.Append(Invariant($"siteId={siteId}&")); 
			}

			if (pageId != -1)
			{
				queryParams.Append(Invariant($"pageId={pageId}&"));
			}

			if (moduleId != -1)
			{
				queryParams.Append(Invariant($"mid={moduleId}&"));
			}

			if (itemId != -1)
			{
				queryParams.Append(Invariant($"itemId={itemId}&"));
			}

			string siteRoot = GetNavigationSiteRoot();
			if (!includeSiteRoot)
			{
				siteRoot = string.Empty;
			}

			return $"{siteRoot}/{pageUrl.TrimStart('/')}{queryParams.ToString().TrimEnd('&')}";
		}

		/// <summary>
		/// You should pass your editor to this method during pre-init or init
		/// </summary>
		/// <param name="editor"></param>
		public static void SetupNewsletterEditor(EditorControl editor)
		{
			if (HttpContext.Current is null) { return; }
			if (HttpContext.Current.Request is null) { return; }
			if (editor is null)
			{
				return;
			}

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return; }

			string providerName = siteSettings.NewsletterEditor;

			string loweredBrowser = string.Empty;

			if (HttpContext.Current.Request.UserAgent is not null)
			{
				loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
			}

			if (
				(loweredBrowser.Contains("iphone"))
				&& (WebConfigSettings.ForcePlainTextInIphone)
				)
			{
				providerName = "TextAreaProvider";
			}

			if (
				(loweredBrowser.Contains("android"))
				&& (WebConfigSettings.ForcePlainTextInAndroid)
				)
			{
				providerName = "TextAreaProvider";
			}

			//string siteRoot = null;
			//if (siteSettings.SiteFolderName.Length > 0)
			//{
			//    siteRoot = siteSettings.SiteRoot;
			//}
			//if (siteRoot is null) siteRoot = WebUtils.GetSiteRoot();

			string siteRoot = GetNavigationSiteRoot();

			editor.ProviderName = providerName;
			editor.WebEditor.SiteRoot = siteRoot;

			CultureInfo defaultCulture = GetDefaultCulture();
			if (defaultCulture.TextInfo.IsRightToLeft)
			{
				editor.WebEditor.TextDirection = Direction.RightToLeft;
			}



		}

		public static string GetIP4Address()
		{
			string ip4Address = string.Empty;
			if (HttpContext.Current is null) { return ip4Address; }
			if (HttpContext.Current.Request is null) { return ip4Address; }

			if (!string.IsNullOrWhiteSpace(WebConfigSettings.ClientIpServerVariable))
			{
				if (HttpContext.Current.Request.ServerVariables[WebConfigSettings.ClientIpServerVariable] is not null)
				{
					return HttpContext.Current.Request.ServerVariables[WebConfigSettings.ClientIpServerVariable];
				}
			}

			if (HttpContext.Current.Request.UserHostAddress is null) { return ip4Address; }

			try
			{
				IPAddress ip = IPAddress.Parse(HttpContext.Current.Request.UserHostAddress);
				if (ip.AddressFamily.ToString() == "InterNetwork") { return ip.ToString(); }
			}
			catch (FormatException)
			{ }
			catch (ArgumentNullException) { }

			try
			{
				foreach (IPAddress IPA in Dns.GetHostAddresses(HttpContext.Current.Request.UserHostAddress))
				{
					if (IPA.AddressFamily.ToString() == "InterNetwork")
					{
						ip4Address = IPA.ToString();
						break;
					}
				}
			}
			catch (ArgumentException)
			{ }
			catch (System.Net.Sockets.SocketException) { }

			if (ip4Address != string.Empty)
			{
				return ip4Address;
			}

			//this part makes no sense it would get the local server ip address
			try
			{
				foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
				{
					if (IPA.AddressFamily.ToString() == "InterNetwork")
					{
						ip4Address = IPA.ToString();
						break;
					}
				}
			}
			catch (ArgumentException)
			{ }
			catch (System.Net.Sockets.SocketException) { }

			return ip4Address;
		}

		//public static string BuildStylesListForTinyMce()
		//{
		//	StringBuilder styles = new StringBuilder();

		//	string comma = string.Empty;

		//	if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates)
		//	{
		//		styles.Append("Image on Right=image-right;Image on Left=image-left");
		//		comma = ";";
		//	}


		//	SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		//	if (siteSettings is not null)
		//	{
		//		using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
		//		{
		//			while (reader.Read())
		//			{
		//				styles.Append(comma + reader["Name"].ToString() + "=" + reader["CssClass"].ToString());
		//				comma = ";";
		//			}
		//		}

		//	}

		//	if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates)
		//	{
		//		styles.Append(comma + "Image on Right=image-right;Image on Left=image-left");
		//	}

		//	return styles.ToString();
		//}

		//public static string BuildStylesListForTinyMce4()
		//{
		//	//http://www.tinymce.com/wiki.php/Configuration:style_formats

		//	StringBuilder styles = new StringBuilder();

		//	styles.Append("[");

		//	//{title : 'Example 1', inline : 'span', classes : 'example1'}

		//	string comma = string.Empty;

		//	if (WebConfigSettings.AddSystemStyleTemplatesAboveSiteTemplates)
		//	{
		//		//styles.Append("{\"title\":\"FloatPanel\",\"inline\":\"span\",\"classes\":\"floatpanel\"}");
		//		styles.Append(",{\"title\":\"Image on Right\",\"selector\":\"img\",\"classes\":\"image-right\"}");
		//		styles.Append(",{\"title\":\"Image on Left\",\"selector\":\"img\",\"classes\":\"image-left\"}");
		//		//styles.Append("FloatPanel=floatpanel;Image on Right=floatrightimage;Image on Left=floatleftimage");
		//		comma = ",";
		//	}


		//	SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		//	if (siteSettings is not null)
		//	{
		//		using (IDataReader reader = ContentStyle.GetAllActive(siteSettings.SiteGuid))
		//		{
		//			while (reader.Read())
		//			{
		//				styles.Append(comma);
		//				styles.Append("{\"title\":\"" + reader["Name"].ToString().JsonEscape()
		//					+ "\",\"selector\":\"" + reader["Element"].ToString().JsonEscape()
		//					+ "\",\"classes\":\"" + reader["CssClass"].ToString().JsonEscape() + "\"}");

		//				//styles.Append(comma + reader["Name"].ToString() + "=" + reader["CssClass"].ToString());

		//				comma = ",";
		//			}
		//		}

		//	}

		//	if (WebConfigSettings.AddSystemStyleTemplatesBelowSiteTemplates)
		//	{
		//		styles.Append(comma);
		//		//styles.Append(comma + "FloatPanel=floatpanel;Image on Right=floatrightimage;Image on Left=floatleftimage");
		//		//styles.Append("{\"title\":\"FloatPanel\",\"inline\":\"span\",\"classes\":\"floatpanel\"}");
		//		styles.Append(",{\"title\":\"Image on Right\",\"selector\":\"img\",\"classes\":\"image-right\"}");
		//		styles.Append(",{\"title\":\"Image on Left\",\"selector\":\"img\",\"classes\":\"image-left\"}");
		//	}

		//	styles.Append("]");

		//	return styles.ToString();
		//}

		public static bool IsWebImageFile(this WebFile file)
		{
			return isExtensionAllowed(file, WebConfigSettings.ImageFileExtensions);
		}

		public static bool IsWebImageFile(this Dtos.FileServiceDto file)
		{
			return isExtensionAllowed(file, WebConfigSettings.ImageFileExtensions);
		}

		public static bool IsAllowedMediaFile(this WebFile file)
		{
			return isExtensionAllowed(file, WebConfigSettings.AllowedMediaFileExtensions);
		}

		public static bool IsAllowedMediaFile(this Dtos.FileServiceDto file)
		{
			return isExtensionAllowed(file, WebConfigSettings.AllowedMediaFileExtensions);
		}

		public static bool IsAllowedAudioFile(this WebFile file)
		{
			return isExtensionAllowed(file, WebConfigSettings.AudioFileExtensions);
		}

		public static bool IsAllowedVideoFile(this WebFile file)
		{
			return isExtensionAllowed(file, WebConfigSettings.VideoFileExtensions);

		}

		public static bool IsAllowedFileType(this WebFile file, string allowedExtensionsString)
		{
			string extension = System.IO.Path.GetExtension(file.Name);
			List<string> allowedExtensions = allowedExtensionsString.SplitOnPipes();
			return allowedExtensions.Contains(extension.ToLower());
		}

		public static bool IsAllowedFileType(this Dtos.FileServiceDto file, string allowedExtensionsString)
		{
			string extension = System.IO.Path.GetExtension(file.Name);
			List<string> allowedExtensions = allowedExtensionsString.SplitOnPipes();
			return allowedExtensions.Contains(extension.ToLower());
		}

		public static bool IsAllowedUploadBrowseFile(this WebFile file, string allowedExtensions)
		{
			return isExtensionAllowed(file, allowedExtensions);
		}

		public static bool IsImageFileExtension(string fileExtension)
		{
			List<string> allowedExtensions = WebConfigSettings.ImageFileExtensions.SplitOnPipes();
			return allowedExtensions.Contains(fileExtension.ToLower());
		}

		private static bool isExtensionAllowed(WebFile file, string configSetting)
		{
			string extension = System.IO.Path.GetExtension(file.Name);
			List<string> allowedExtensions = configSetting.SplitOnPipes();
			return allowedExtensions.Contains(extension.ToLower());
		}

		private static bool isExtensionAllowed(Dtos.FileServiceDto file, string configSetting)
		{
			string extension = System.IO.Path.GetExtension(file.Name);
			List<string> allowedExtensions = configSetting.SplitOnPipes();
			return allowedExtensions.Contains(extension.ToLower());
		}

		[Obsolete("Use IsAllowedFileType")]
		public static string ImageFileExtensions()
		{
			return WebConfigSettings.ImageFileExtensions;
		}

		public static List<string> ExtractUrls(string html)
		{
			string urlRegex = WebConfigSettings.UrlRegex;

			List<string> urls = new List<string>();

			MatchCollection matches = Regex.Matches(html, urlRegex);
			foreach (Match m in matches)
			{
				if (urls.Contains(m.Value))
				{
					continue; //this should prevent duplicates
				}

				urls.Add(SecurityHelper.RemoveAngleBrackets(RemoveQuotes(m.Value)));
			}

			return urls;
		}


		public static void DeleteAttachmentFiles(IFileSystem fileSystem, List<FileAttachment> attachments, string basePath)
		{
			if (attachments is null) { return; }
			if (string.IsNullOrEmpty(basePath)) { return; }

			foreach (FileAttachment f in attachments)
			{
				if (fileSystem.FileExists(basePath + f.ServerFileName))
				{
					fileSystem.DeleteFile(basePath + f.ServerFileName);
				}
			}

		}

		public static void DeleteAttachmentFile(IFileSystem fileSystem, FileAttachment attachment, string basePath)
		{
			if (attachment is null) { return; }
			if (string.IsNullOrEmpty(basePath)) { return; }


			if (fileSystem.FileExists(basePath + attachment.ServerFileName))
			{
				fileSystem.DeleteFile(basePath + attachment.ServerFileName);
			}
		}

		public static bool IsAllowedMediaFile(this FileInfo fileInfo)
		{
			List<string> allowedExtensions = WebConfigSettings.AllowedMediaFileExtensions.SplitOnPipes();
			foreach (string ext in allowedExtensions)
			{
				if (string.Equals(fileInfo.Extension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
			}

			return false;
		}

		public static bool IsAllowedUploadBrowseFile(this FileInfo fileInfo, string allowedExtensions)
		{
			List<string> allowed = allowedExtensions.SplitOnPipes();
			foreach (string ext in allowed)
			{
				if (string.Equals(fileInfo.Extension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
			}

			return false;
		}

		public static bool IsAllowedUploadBrowseFile(string fileExtension, string allowedExtensions)
		{
			List<string> allowed = allowedExtensions.SplitOnPipes();
			foreach (string ext in allowed)
			{
				if (string.Equals(fileExtension, ext, StringComparison.InvariantCultureIgnoreCase))
				{
					return true;
				}
			}

			return false;
		}

		public static void SetButtonAccessKey(Button button, string accessKey)
		{
			if (!WebConfigSettings.UseShortcutKeys)
			{
				return;
			}

			button.AccessKey = accessKey;
			button.Text += GetButtonAccessKeyPostfix(accessKey);
		}


		public static string GetButtonAccessKeyPostfix(string accessKey)
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			string browser = HttpContext.Current.Request.Browser.Browser;
			string browserAccessKey = browser.ToLower().Contains("opera")
										  ? AccessKeys.BrowserOperaAccessKey
										  : AccessKeys.BrowserAccessKey;

			return string.Format(CultureInfo.InvariantCulture, " [{0}+{1}]", browserAccessKey, accessKey);
		}

		/// <summary>
		/// this method is deprecated
		/// </summary>
		[Obsolete("This method is obsolete. You should use if(!Request.IsAuthenticated) SiteUtils.RedirectToLogin(PageorControl); return;")]
		public static void AllowOnlyAuthenticated()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				RedirectToLoginPage();
			}
		}

		[Obsolete("This method is obsolete. You should use if(!Request.IsAuthenticated) SiteUtils.RedirectToLogin(PageorControl); return;")]
		public static void AllowOnlyAuthenticated(Control pageOrControl)
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				RedirectToLoginPage(pageOrControl);
			}
		}

		[Obsolete("This method is obsolete. You should use if(!WebUser.IsAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
		public static void AllowOnlyAdmin()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			AllowOnlyAuthenticated();
			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return;
			}

			if (!WebUser.IsAdmin)
			{
				RedirectToAccessDeniedPage();
			}
		}

		[Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrRoleAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
		public static void AllowOnlyAdminAndRoleAdmin()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			AllowOnlyAuthenticated();
			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return;
			}

			if ((!WebUser.IsAdmin) && (!WebUser.IsRoleAdmin))
			{
				RedirectToAccessDeniedPage();
			}
		}

		[Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrContentAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
		public static void AllowOnlyAdminAndContentAdmin()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			AllowOnlyAuthenticated();
			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return;
			}

			if (!WebUser.IsAdminOrContentAdmin)
			{
				RedirectToAccessDeniedPage();
			}
		}

		[Obsolete("This method is obsolete. You should use if(!WebUser.IsAdminOrContentAdminOrRoleAdmin) SiteUtils.RedirectToAccessDenied(PageorControl); return;")]
		public static void AllowOnlyAdminAndContentAdminAndRoleAdmin()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			AllowOnlyAuthenticated();
			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return;
			}

			if (!WebUser.IsAdminOrContentAdminOrRoleAdmin)
			{
				RedirectToAccessDeniedPage();
			}
		}

		[Obsolete("This method is obsolete. You should use RedirectToLoginPage(pageOrControl); return;")]
		public static void RedirectToLoginPage()
		{
			HttpContext.Current.Response.Redirect
				(string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
							   GetNavigationSiteRoot(),
							   HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl)),
				 true);
		}

		public static void RedirectToLoginPage(Control pageOrControl)
		{
			string redirectUrl
				= string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
							   GetNavigationSiteRoot(),
							   HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));

			WebUtils.SetupRedirect(pageOrControl, redirectUrl);
		}

		public static void RedirectToLoginPage(Control pageOrControl, string returnUrl)
		{
			string redirectUrl
				= string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
							   GetNavigationSiteRoot(),
							   HttpUtility.UrlEncode(returnUrl));

			WebUtils.SetupRedirect(pageOrControl, redirectUrl);
		}

		public static void RedirectToLoginPage(Control pageOrControl, bool useHardRedirect)
		{
			if (!useHardRedirect)
			{
				RedirectToLoginPage(pageOrControl);
				return;
			}

			string redirectUrl
				= string.Format(CultureInfo.InvariantCulture, "{0}" + GetLoginRelativeUrl() + "?returnurl={1}",
							   GetNavigationSiteRoot(),
							   HttpUtility.UrlEncode(HttpContext.Current.Request.RawUrl));

			pageOrControl.Page.Response.Redirect(redirectUrl);
		}

		public static string GetLoginRelativeUrl()
		{
			if (ConfigurationManager.AppSettings["LoginPageRelativeUrl"] is not null)
			{
				return ConfigurationManager.AppSettings["LoginPageRelativeUrl"];
			}

			return "/Secure/Login.aspx";

		}

		public static void RedirectToUrl(string url)
		{
			if (HttpContext.Current is null) { return; }

			HttpContext.Current.Response.RedirectLocation = url;
			HttpContext.Current.Response.StatusCode = 302;
			HttpContext.Current.Response.StatusDescription = "Redirecting to " + url;
			HttpContext.Current.Response.Write("Redirecting to " + url);
			HttpContext.Current.ApplicationInstance.CompleteRequest();

		}
		[Obsolete("Please use SiteUtils.RedirectToAccessDeniedPage()")]
		public static void RedirectToEditAccessDeniedPage()
		{
			RedirectToAccessDeniedPage();
		}

		//public static void RedirectToAccessDeniedPage()
		//{
		//	RedirectToAccessDeniedPage("");
		//}
		public static void RedirectToAccessDeniedPage(string returnUrl = "")
		{
			if (HttpContext.Current is null) { return; }

			string url = GetNavigationSiteRoot() + "/AccessDenied.aspx" + (String.IsNullOrWhiteSpace(returnUrl) ? "" : "?ReturnUrl=" + returnUrl);

			HttpContext.Current.Response.TrySkipIisCustomErrors = true;
			HttpContext.Current.Response.StatusCode = 403;
			HttpContext.Current.Response.StatusDescription = "Redirecting to " + url;
			HttpContext.Current.Response.Write("Redirecting to " + url);
			HttpContext.Current.Response.Redirect(url);
			HttpContext.Current.ApplicationInstance.CompleteRequest();
		}

		public static void RedirectToAccessDeniedPage(Control pageOrControl)
		{
			string redirectUrl
				= GetNavigationSiteRoot() + "/AccessDenied.aspx";

			WebUtils.SetupRedirect(pageOrControl, redirectUrl);
		}

		public static void RedirectToAccessDeniedPage(Control pageOrControl, bool useHardRedirect)
		{
			if (!useHardRedirect)
			{
				RedirectToAccessDeniedPage(pageOrControl);
				return;
			}

			string redirectUrl
				= GetNavigationSiteRoot() + "/AccessDenied.aspx";

			pageOrControl.Page.Response.Redirect(redirectUrl);
		}

		public static void RedirectToAdminMenu(Control pageOrControl)
		{
			string redirectUrl = GetNavigationSiteRoot() + WebConfigSettings.AdminDirectoryLocation;

			WebUtils.SetupRedirect(pageOrControl, redirectUrl);
		}

		public static void RedirectToSiteRoot()
		{
			RedirectToUrl(GetNavigationSiteRoot() + "/Default.aspx");
		}


		[Obsolete("Will be removed in a future version. Please use RedirectToSiteRoot", false)]
		public static void RedirectToDefault()
		{
			RedirectToSiteRoot();
		}

		public static void SetFormAction(Page page, string action)
		{
			page.Form.Action = action;
		}

		public static void AddNoIndexFollowMeta(Page page)
		{
			if (page.Header is null) { return; }

			Literal meta = new Literal
			{
				ID = "metanoindexfollow",
				Text = "\n<meta name='robots' content='NOINDEX,FOLLOW' />"
			};

			page.Header.Controls.Add(meta);

		}

		public static void AddNoIndexMeta(Page page)
		{
			if (page.Header is null) { return; }

			Literal meta = new Literal
			{
				ID = "metanoindexfollow",
				Text = "\n<meta name='robots' content='NOINDEX' />"
			};

			page.Header.Controls.Add(meta);

		}

		//GetSkinName

		public static string GetMasterPage(
			Page page,
			SiteSettings siteSettings,
			bool allowOverride)
		{
			string skinName = GetSkinName(allowOverride, page);

			return GetMasterPage(page, skinName, siteSettings, allowOverride);
		}

		public static string GetMasterPage(
			Page page,
			string skinName,
			SiteSettings siteSettings,
			bool allowOverride,
			string masterPageName = "layout.Master")
		{
			string skinFolder = "~/App_MasterPages/";
			string masterPage = masterPageName;
			PageSettings currentPage = CacheHelper.GetCurrentPage();

			if (
				(HttpContext.Current is not null)
				&& (page is not null)
				&& (siteSettings is not null)
				)
			{
				skinFolder = $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/";
				masterPage = $"{skinName}/{masterPageName}";
			}

			if (page is mojoBasePage)
			{
				if (((mojoBasePage)page).UseMobileSkin)
				{
					if (siteSettings.MobileSkin.Length > 0)
					{
						masterPage = $"{siteSettings.MobileSkin}/{masterPageName}";
					}
					//web.config setting trumps site setting
					if (!string.IsNullOrWhiteSpace(WebConfigSettings.MobilePhoneSkin))
					{
						masterPage = $"{WebConfigSettings.MobilePhoneSkin}/{masterPageName}";
					}
				}
			}

			//log.Info("set master page to " + skinFolder + masterPage);

			return skinFolder + masterPage;
		}

		public static string GetSkinPreviewParam(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return string.Empty; }
			// implement skin preview using querystring param
			if (HttpContext.Current.Request.Params.Get("skin") is not null)
			{
				string skinFolder = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/"; ;
				string previewSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));

				try
				{
					if (File.Exists(HttpContext.Current.Server.MapPath(skinFolder + previewSkin + "/layout.Master")))
					{
						return previewSkin;
					}
				}
				catch (HttpException) { }
			}

			return string.Empty;
		}


		public static string SanitizeSkinParam(string skinName)
		{
			if (string.IsNullOrEmpty(skinName)) { return skinName; }

			// protected against this xss attack
			// ?skin=1%00'"><ScRiPt%20%0a%0d>alert(403326057258)%3B</ScRiPt>
			return skinName.Replace("%", string.Empty).Replace(" ", string.Empty).Replace(">", string.Empty).Replace("<", string.Empty).Replace("'", string.Empty).Replace("\"", string.Empty);


		}

		//public static string GetMyPageMasterPage(SiteSettings siteSettings)
		//{
		//	if (siteSettings is null)
		//	{
		//		return "~/App_MasterPages/layout.Master";
		//	}

		//	if (siteSettings.MyPageSkin.Length > 0)
		//	{
		//		return "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + siteSettings.MyPageSkin + "/layout.Master";
		//	}

		//	return "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/" + siteSettings.Skin + "/layout.Master";

		//}

		public static void SetSkinCookie(SiteUser siteUser)
		{
			if (siteUser is null)
			{
				return;
			}

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return; }
			string skinCookieName = "mojoUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
			HttpCookie cookie = HttpContext.Current.Request.Cookies[skinCookieName];
			bool setCookie = (cookie is null) || (cookie.Value != siteUser.Skin);
			if (setCookie)
			{
				HttpCookie userSkinCookie = new HttpCookie(skinCookieName, siteUser.Skin);
				userSkinCookie.Expires = DateTime.Now.AddYears(1);
				HttpContext.Current.Response.Cookies.Add(userSkinCookie);

			}
		}

		public static void SetDisplayNameCookie(string displayName)
		{
			if (string.IsNullOrEmpty(displayName))
			{
				return;
			}

			HttpCookie cookie = HttpContext.Current.Request.Cookies["DisplayName"];
			bool setCookie = (cookie is null) || (cookie.Value != displayName);
			if (setCookie)
			{
				HttpCookie userNameCookie = new HttpCookie("DisplayName", HttpUtility.HtmlEncode(displayName));
				userNameCookie.Expires = DateTime.Now.AddYears(1);
				HttpContext.Current.Response.Cookies.Add(userNameCookie);
			}
		}

		[Obsolete("These will be removed soon. 10/31/2018")]
		public static List<ContentTemplate> GetSystemContentTemplates()
		{
			List<ContentTemplate> templates = new List<ContentTemplate>();

			ContentTemplate t = ContentTemplate.GetEmpty();

			if (WebConfigSettings.IncludejQueryAccordionContentTemplate)
			{
				//jQuery accordion
				t.Guid = new Guid("e110400d-c92d-4d78-a830-236f584af115");
				t.Body = "<p>Paragraph before the accordion</p><div class=\"mojo-accordion\"><h3><a href=\"#\">Section 1</a></h3><div><p>Section 1 content.</p></div><h3><a href=\"#\">Section 2</a></h3><div><p>Section 2 content</p></div><h3><a href=\"#\">Section 3</a></h3><div><p>Section 3 content</p></div><h3><a href=\"#\">Section 4</a></h3><div><p>Section 4 content</p></div></div><p>Paragraph after the accordion</p>";
				t.Title = Resource.TemplatejQueryAccordionTitle;
				t.ImageFileName = "jquery-accordion.gif";
				templates.Add(t);
			}

			if (WebConfigSettings.IncludejQueryAccordionNoHeightContentTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//jQuery Accordion NoHeight
				t.Guid = new Guid("08e2a92f-d346-416b-b37b-bd82acf51514");
				t.Body = "<p>Paragraph before the accordion</p><div class=\"mojo-accordion-nh\"><h3><a href=\"#\">Section 1</a></h3><div><p>Section 1 content.</p></div><h3><a href=\"#\">Section 2</a></h3><div><p>Section 2 content</p></div><h3><a href=\"#\">Section 3</a></h3><div><p>Section 3 content</p></div><h3><a href=\"#\">Section 4</a></h3><div><p>Section 4 content</p></div></div><p>Paragraph after the accordion</p>";
				t.Title = Resource.TemplatejQueryAccordionNoAutoHeightTitle;
				t.ImageFileName = "jquery-accordion.gif";
				templates.Add(t);
			}

			if (WebConfigSettings.IncludejQueryTabsContentTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//jQuery Tabs
				t.Guid = new Guid("7efaeb03-a1f9-4b08-9ffd-46237ba993b0");
				t.Body = "<p>Paragraph before the tabs</p><div class=\"mojo-tabs\"><ul><li><a href=\"#tab1\">Tab 1</a></li><li><a href=\"#tab2\">Tab 2</a></li><li><a href=\"#tab3\">Tab 3</a></li></ul><div id=\"tab1\"><p>Tab 1 content</p></div><div id=\"tab2\"><p>Tab 2 content</p></div><div id=\"tab3\"><p>Tab 3 content</p></div></div><p>Paragraph after the tabs</p>";
				t.Title = Resource.TemplatejQueryTabsTitle;
				t.ImageFileName = "jquerytabs.gif";
				templates.Add(t);
			}

			if (WebConfigSettings.IncludeFaqContentTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//jQuery FAQ
				t.Guid = new Guid("ad5f5b63-d07a-4e6b-bbd5-2b6201743dab");
				t.Body = "<h3>Example FAQ</h3><dl class=\"faqs\"><dt>Is this the first question?</dt><dd>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.&nbsp;</dd><dt>This must be the second question.</dt><dd>Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium.</dd><dt>And what about the third question?</dt><dd>Nam eget dui. Etiam rhoncus. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo.</dd></dl>";
				t.Title = Resource.TemplatejQueryFAQTitle;
				t.ImageFileName = "faq.jpg";
				templates.Add(t);
			}

			if (WebConfigSettings.Include2ColumnsOver1ColumnTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//2 columns over 1 
				t.Guid = new Guid("cfb9e9c4-b740-42f5-8c16-1957b536b8e9");
				t.Body = "<div class=\"floatpanel\"><div class=\"floatpanel section\" style=\"width: 46%;\"><h3>Lorem Ipsum</h3><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p></div><div class=\"floatpanel section\" style=\"width: 46%;\"><h3>Duis a Mauris</h3><p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p></div></div><div class=\"clear section\"><h3>Aenean?</h3><p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p></div><p>new paragraph</p>";
				t.Title = Resource.Template2ColumnsOver1ColumnTitle;
				t.ImageFileName = "columns2over1.gif";
				templates.Add(t);
			}

			if (WebConfigSettings.Include3ColumnsOver1ColumnTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//3 columns over 1
				t.Guid = new Guid("9ac79a8d-7dfd-4485-af3c-b8fdf256bbb8");
				t.Body = "<div class=\"floatpanel\"><div class=\"floatpanel section\" style=\"width: 31%;\"><h3>Lorem Ipsum</h3><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p></div><div class=\"floatpanel section\" style=\"width: 31%;\"><h3>Duis a Mauris</h3><p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p></div><div class=\"floatpanel section\" style=\"width: 31%;\"><h3>Vivamus Tristique!</h3><p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p></div></div><div class=\"clear section\"><h3>Aenean?</h3><p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p></div><p>new paragraph</p>";
				t.Title = Resource.Template3ColumnsOver1ColumnTitle;
				t.ImageFileName = "columns3over1.gif";
				templates.Add(t);
			}

			if (WebConfigSettings.Include4ColumnsOver1ColumnTemplate)
			{
				t = ContentTemplate.GetEmpty();
				//4 columns over 1
				t.Guid = new Guid("28ae8c68-b619-4e23-8dde-17d0a34ee7c6");
				t.Body = "<div class=\"floatpanel\"><div class=\"floatpanel section\" style=\"width: 23%;\"><h3>Lorem Ipsum</h3><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent varius varius est, id dictum lectus aliquet non. Fusce laoreet auctor facilisis. Nullam eget tortor at leo pellentesque pellentesque. Nunc tortor neque, elementum varius pretium sit amet, vulputate at erat. Duis nec nisi mauris, in gravida sapien.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h3>Duis a Mauris</h3><p>Duis a mauris non felis dapibus cursus. Aliquam eu dignissim purus. Donec at orci vitae sem laoreet molestie sed eu urna. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus magna velit, fringilla egestas vehicula at, adipiscing eget augue. Suspendisse porta, tellus id consequat volutpat.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h3>Vivamus Tristique!</h3><p>Vivamus tristique purus eget nisl sollicitudin varius. Praesent turpis sapien, imperdiet ut vehicula pretium, tristique nec mauris. Quisque eget lacus mi. Quisque adipiscing velit euismod enim venenatis eleifend. Donec commodo purus non mauris ultricies ultricies. Nulla facilisi.</p></div><div class=\"floatpanel section\" style=\"width: 23%;\"><h3>Sed Varius</h3><p>Sed varius porta consequat. Proin ante neque, mattis sit amet condimentum in, vulputate ac ipsum. Proin eu consequat est. Integer at vehicula lacus. Nulla faucibus dolor ut augue euismod eget volutpat ligula venenatis. Curabitur bibendum consequat orci, sagittis elementum dolor commodo vel.</p></div></div><div class=\"clear section\"><h3>Aenean?</h3><p>Aenean at urna nibh. Aliquam euismod tortor ut mauris eleifend ut vehicula neque convallis. Aenean dui orci, luctus non aliquet eu, semper non arcu. Aliquam tincidunt metus at ligula fringilla ornare. Praesent euismod, lacus vel condimentum convallis, massa quam auctor nisl, ut egestas felis sapien eget augue. Etiam eleifend auctor nunc, id facilisis ante ultrices in. Integer sagittis augue a tortor luctus ut tristique metus sagittis.</p></div><p>new paragraph</p>";
				t.Title = Resource.Template4ColumnsOver1ColumnTitle;
				t.ImageFileName = "columns4over1.gif";
				templates.Add(t);
			}


			return templates;
		}

		public static FileInfo[] GetLogoList(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return null;
			}

			string logoPath;

			if (WebConfigSettings.SiteLogoUseMediaFolder)
			{
				logoPath = HttpContext.Current.Server.MapPath
				(WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/media/logos/");
			}
			else
			{
				logoPath = HttpContext.Current.Server.MapPath
				(WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/logos/");
			}

			DirectoryInfo dir = new DirectoryInfo(logoPath);

			if (!dir.Exists)
			{
				Directory.CreateDirectory(logoPath);
			}

			return dir.GetFiles();
		}

		public static FileInfo[] GetContentTemplateImageList(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return null;
			}

			string filePath = HttpContext.Current.Server.MapPath
				(WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/htmltemplateimages/");

			DirectoryInfo dir = new DirectoryInfo(filePath);
			return dir.Exists ? dir.GetFiles() : null;
		}

		public static mojoPortal.Web.UI.Avatar.RatingType GetMaxAllowedGravatarRating()
		{
			switch (WebConfigSettings.GravatarMaxAllowedRating)
			{
				case "PG":
					return Avatar.RatingType.PG;

				case "R":
					return Avatar.RatingType.R;

				case "X":
					return Avatar.RatingType.X;
			}

			return mojoPortal.Web.UI.Avatar.RatingType.G;
		}

		public static FileInfo[] GetAvatarList(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return null;
			}

			string p = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture) + "/avatars";
			string avatarPath = HttpContext.Current.Server.MapPath(p);

			DirectoryInfo dir = new DirectoryInfo(avatarPath);
			return dir.Exists ? dir.GetFiles("*.gif") : null;
		}

		public static List<string> GetFileIconNames()
		{
			List<string> fileNames = new List<string>();

			string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/SiteImages/Icons");
			if (Directory.Exists(filePath))
			{
				DirectoryInfo dir = new DirectoryInfo(filePath);
				FileInfo[] files = dir.GetFiles("*.png");
				foreach (FileInfo f in files)
				{
					fileNames.Add(f.Name);
				}
			}
			return fileNames;
		}

		public static FileInfo[] GetFileIconList()
		{
			string filePath = System.Web.Hosting.HostingEnvironment.MapPath("~/Data/SiteImages/Icons");

			DirectoryInfo dir = new DirectoryInfo(filePath);
			return dir.Exists ? dir.GetFiles("*.png") : null;
		}

		/// <summary>
		/// deprecated, better to just call IOHelper.GetMimeType
		/// </summary>
		/// <param name="fileExtension"></param>
		/// <returns></returns>
		public static string GetMimeType(string fileExtension)
		{
			return IOHelper.GetMimeType(fileExtension);
		}

		/// <summary>
		/// deprecated, better to just call IOHelper.IsNonAttacmentFileType
		/// </summary>
		/// <param name="fileExtension"></param>
		/// <returns></returns>
		public static bool IsNonAttacmentFileType(string fileExtension)
		{
			return IOHelper.IsNonAttacmentFileType(fileExtension);
		}

		public static string GetSiteSystemFolder()
		{
			if (HttpContext.Current is null) { return string.Empty; }
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return string.Empty; }

			return HttpContext.Current.Server.MapPath("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/systemfiles/");


		}

		public static string GetSiteSkinFolderPath()
		{
			if (HttpContext.Current is null) { return string.Empty; }
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return string.Empty; }

			return HttpContext.Current.Server.MapPath("~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/");


		}

		public static DirectoryInfo[] GetSkinList(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return null;
			}

			string skinPath = HttpContext.Current.Server.MapPath
				(WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/");

			DirectoryInfo dir = new DirectoryInfo(skinPath);
			return dir.Exists ? dir.GetDirectories() : null;
		}

		public static DirectoryInfo[] GetSkinCatalogList()
		{
			string skinPath = HttpContext.Current.Server.MapPath("~/Data/skins/");

			DirectoryInfo dir = new DirectoryInfo(skinPath);
			return dir.Exists ? dir.GetDirectories() : null;
		}

		public static FileInfo[] GetCodeTemplateList()
		{
			string filePath = HttpContext.Current.Server.MapPath("~/DevAdmin/CodeTemplates");

			DirectoryInfo dir = new DirectoryInfo(filePath);
			return dir.Exists ? dir.GetFiles("*.aspx") : null;
		}


		public static SmtpSettings GetSmtpSettings()
		{
			if (WebConfigSettings.EnableSiteSettingsSmtpSettings)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
				return GetSmtpSettings(siteSettings);
			}
			else
			{
				return GetSmtpSettingsFromConfig();
			}
		}

		public static SmtpSettings GetSmtpSettings(SiteSettings siteSettings)
		{
			SmtpSettings smtpSettings = new SmtpSettings();

			if (WebConfigSettings.EnableSiteSettingsSmtpSettings)
			{
				if (siteSettings is not null)
				{
					if (WebConfigSettings.UseLegacyCryptoHelper)
					{
						if (siteSettings.SMTPUser.Length > 0)
						{
							smtpSettings.User = CryptoHelper.Decrypt(siteSettings.SMTPUser);
						}

						if (siteSettings.SMTPPassword.Length > 0)
						{
							smtpSettings.Password = CryptoHelper.Decrypt(siteSettings.SMTPPassword);
						}
					}
					else
					{
						if (siteSettings.SMTPUser.Length > 0)
						{
							smtpSettings.User = SiteUtils.Decrypt(siteSettings.SMTPUser);
						}

						if (siteSettings.SMTPPassword.Length > 0)
						{
							smtpSettings.Password = SiteUtils.Decrypt(siteSettings.SMTPPassword);
						}
					}

					smtpSettings.Server = siteSettings.SMTPServer;
					smtpSettings.Port = siteSettings.SMTPPort;
					smtpSettings.RequiresAuthentication = siteSettings.SMTPRequiresAuthentication;
					smtpSettings.UseSsl = siteSettings.SMTPUseSsl;
					smtpSettings.PreferredEncoding = siteSettings.SMTPPreferredEncoding;

					foreach (var header in siteSettings.SMTPCustomHeaders.SplitOnNewLineAndTrim())
					{
						var keyFinalIndex = header.IndexOf(':');
						var key = header.Substring(0, keyFinalIndex).Trim();
						var val = header.Substring(keyFinalIndex + 1).Trim();
						smtpSettings.AdditionalHeaders.Add(new SmtpHeader { Name = key, Value = val });
					}
				}
			}
			else
			{
				return GetSmtpSettingsFromConfig();
			}

			return smtpSettings;
		}

		private static SmtpSettings GetSmtpSettingsFromConfig()
		{
			SmtpSettings smtpSettings = new SmtpSettings();

			if (ConfigurationManager.AppSettings["SMTPUser"] is not null)
			{
				smtpSettings.User = ConfigurationManager.AppSettings["SMTPUser"];
			}

			if (ConfigurationManager.AppSettings["SMTPPassword"] is not null)
			{
				smtpSettings.Password = ConfigurationManager.AppSettings["SMTPPassword"];
			}
			if (ConfigurationManager.AppSettings["SMTPServer"] is not null)
			{
				smtpSettings.Server = ConfigurationManager.AppSettings["SMTPServer"];
			}

			smtpSettings.Port = Config.ConfigHelper.GetIntProperty("SMTPPort", 25);

			bool byPassContext = true;
			smtpSettings.RequiresAuthentication = Config.ConfigHelper.GetBoolProperty("SMTPRequiresAuthentication", false, byPassContext); ;
			smtpSettings.UseSsl = Config.ConfigHelper.GetBoolProperty("SMTPUseSsl", false, byPassContext);

			if (
		   (ConfigurationManager.AppSettings["SmtpPreferredEncoding"] is not null)
		   && (ConfigurationManager.AppSettings["SmtpPreferredEncoding"].Length > 0)
		   )
			{
				smtpSettings.PreferredEncoding = ConfigurationManager.AppSettings["SmtpPreferredEncoding"];
			}

			///
			/// IF YOU WANT TO USE THE SMTPCUSTOMHEADERS, YOU MUST ENTER THEM IN THE SITE SETTINGS
			///


			return smtpSettings;
		}


		[Obsolete("Use GetSkinBaseUrl(Page page)")]
		/// <summary>
		/// deprecated, you should pass in the Page
		/// </summary>
		/// <returns></returns>
		public static string GetSkinBaseUrl()
		{
			return GetSkinBaseUrl(null);
		}

		public static string GetSkinBaseUrl(Page page)
		{
			bool allowPageOverride = true;
			return GetSkinBaseUrl(allowPageOverride, page);
		}


		public static string GetSkinBaseUrl(bool allowPageOverride, Page page)
		{
			if (allowPageOverride)
			{
				return GetSkinBaseUrlWithOverride(page);
			}

			return GetSkinBaseUrlNoOverride(page);
		}

		private static string GetSkinBaseUrlNoOverride(Page page)
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			string baseUrl = HttpContext.Current.Items["skinBaseUrlfalse"] as string;
			if (baseUrl is null)
			{
				baseUrl = DetermineSkinBaseUrl(false, page);
				if (baseUrl is not null)
				{
					HttpContext.Current.Items["skinBaseUrlfalse"] = baseUrl;
				}
			}
			return baseUrl;


		}

		private static string GetSkinBaseUrlWithOverride(Page page)
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			string baseUrl = HttpContext.Current.Items["skinBaseUrltrue"] as string;
			if (baseUrl is null)
			{
				baseUrl = DetermineSkinBaseUrl(true, page);
				if (baseUrl is not null)
				{
					HttpContext.Current.Items["skinBaseUrltrue"] = baseUrl;
				}
			}
			return baseUrl;


		}

		private static string DetermineSkinBaseUrl(bool allowPageOverride, Page page)
		{
			bool fullUrl = true;

			return DetermineSkinBaseUrl(allowPageOverride, fullUrl, page);


		}

		public static string DetermineSkinBaseUrl(string skinName)
		{
			if (string.IsNullOrEmpty(skinName)) { return $"/Data/Skins/{WebConfigSettings.DefaultInitialSkin}/"; }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return $"/Data/Skins/{WebConfigSettings.DefaultInitialSkin}/"; }


			string skinUrl = $"/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{skinName}/";

			return skinUrl;

		}

		public static string DetermineSkinBaseUrl(bool allowPageOverride, bool fullUrl, Page page)
		{
			string skinFolder;
			string siteRoot = string.Empty;

			if (fullUrl)
			{
				siteRoot = WebUtils.GetSiteRoot();
				skinFolder = siteRoot + "/Data/Sites/1/skins/";
			}
			else
			{
				siteRoot = WebUtils.GetRelativeSiteRoot();
				skinFolder = "/Data/Sites/1/skins/";
			}

			string currentSkin = WebConfigSettings.DefaultInitialSkin;

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			PageSettings currentPage = CacheHelper.GetCurrentPage();

			if (siteSettings is not null)
			{
				currentSkin = siteSettings.Skin + "/";

				if (siteSettings.AllowUserSkins)
				{
					string skinCookieName = "mojoUserSkin" + siteSettings.SiteId.ToInvariantString();
					if (CookieHelper.CookieExists(skinCookieName))
					{
						string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
						if (cookieValue.Length > 0)
						{
							currentSkin = cookieValue + "/";
						}
					}
				}

				if (
					allowPageOverride
					&& (currentPage is not null)
					&& siteSettings.AllowPageSkins
					&& (page is not null)
						&& (
						(page is mojoPortal.Web.AdminUI.PageLayout)
						|| (page is mojoPortal.Web.AdminUI.PageProperties)
						|| (page is mojoPortal.Web.AdminUI.ModuleSettingsPage)
						|| (!(page is NonCmsBasePage))
						)
					)
				{
					if (currentPage.Skin.Length > 0)
					{
						currentSkin = currentPage.Skin + "/";

					}
				}

				if (SiteUtils.UseMobileSkin())
				{
					if (siteSettings.MobileSkin.Length > 0)
					{
						currentSkin = siteSettings.MobileSkin + "/";
					}
					//web.config setting trumps site setting
					if (!string.IsNullOrWhiteSpace(WebConfigSettings.MobilePhoneSkin))
					{
						currentSkin = WebConfigSettings.MobilePhoneSkin + "/";
					}
				}


				skinFolder = siteRoot + "/Data/Sites/"
					+ siteSettings.SiteId.ToInvariantString() + "/skins/";


				if (HttpContext.Current.Request.Params.Get("skin") is not null)
				{
					currentSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin")) + "/";
				}
			}

			return skinFolder + currentSkin;
		}

		public static string GetEditorStyleSheetUrl(bool allowPageOverride, bool fullUrl, Page page)
		{
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.EditorCssUrlOverride)) { return WebConfigSettings.EditorCssUrlOverride; }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null)
			{
				return string.Empty;
			}

			if (page is not null)
			{
				if (fullUrl)
				{
					return GetNavigationSiteRoot() + "/csshandler.ashx?skin=" + SiteUtils.GetSkinName(allowPageOverride, page)
						+ "&amp;s=" + siteSettings.SiteId.ToInvariantString()
						 + "&amp;sv=" + siteSettings.SkinVersion + WebConfigSettings.EditorExtraCssUrlCsv;
				}
				else
				{
					return GetRelativeNavigationSiteRoot() + "/csshandler.ashx?skin=" + SiteUtils.GetSkinName(allowPageOverride, page)
						+ "&amp;s=" + siteSettings.SiteId.ToInvariantString()
						 + "&amp;sv=" + siteSettings.SkinVersion + WebConfigSettings.EditorExtraCssUrlCsv;
				}

			}

			string editorCss = "csshandler.ashx";
			if (WebConfigSettings.UsingOlderSkins)
			{
				editorCss = "style.css";
			}

			string skinName = WebConfigSettings.DefaultInitialSkin;



			string basePath = WebUtils.GetSiteRoot() + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/";

			PageSettings currentPage = CacheHelper.GetCurrentPage();
			if (siteSettings is not null)
			{
				// very old skins were .ascx
				skinName = siteSettings.Skin.Replace(".ascx", string.Empty);

				if (siteSettings.AllowUserSkins)
				{
					string skinCookieName = "mojoUserSkin" + siteSettings.SiteId.ToInvariantString();

					if (CookieHelper.CookieExists(skinCookieName))
					{
						string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
						if (cookieValue.Length > 0)
						{
							skinName = cookieValue.Replace(".ascx", string.Empty);
						}
					}
				}

				if ((currentPage is not null) && (siteSettings.AllowPageSkins))
				{
					if (currentPage.Skin.Length > 0)
					{
						skinName = currentPage.Skin.Replace(".ascx", string.Empty);

					}
				}



				if (HttpContext.Current.Request.Params.Get("skin") is not null)
				{
					skinName = HttpContext.Current.Request.Params.Get("skin");

				}

				if (editorCss == "csshandler.ashx")
				{

					//return basePath + skinName + "/" + editorCss + "?skin=" + skinName;

					if (fullUrl)
					{
						return GetNavigationSiteRoot() + "/csshandler.ashx?skin=" + skinName + WebConfigSettings.EditorExtraCssUrlCsv;
					}
					else
					{

						return GetRelativeNavigationSiteRoot() + "/csshandler.ashx?skin=" + skinName + WebConfigSettings.EditorExtraCssUrlCsv;
					}
				}

				return basePath + skinName + "/" + editorCss + WebConfigSettings.EditorExtraCssUrlCsv;

			}

			return "/Data/Sites/1/skins/" + WebConfigSettings.DefaultInitialSkin + "/style.css" + WebConfigSettings.EditorExtraCssUrlCsv;
		}

		public static void SetupEditor(EditorControl editor)
		{
			SetupEditor(editor, WebConfigSettings.UseSkinCssInEditor);
		}

		/// <summary>
		/// this is the preferred overload
		/// </summary>
		/// <param name="editor"></param>
		/// <param name="allowPageOverride"></param>
		/// <param name="fullUrl"></param>
		/// <param name="page"></param>
		public static void SetupEditor(EditorControl editor, bool allowPageOverride, Page page)
		{
			SetupEditor(editor, WebConfigSettings.UseSkinCssInEditor, string.Empty, allowPageOverride, false, page);
		}

		public static void SetupEditor(EditorControl editor, bool useSkinCss)
		{
			SetupEditor(editor, useSkinCss, string.Empty, false, true, null);
		}

		/// <summary>
		/// You should pass your editor to this method during pre-init or init
		/// </summary>
		/// <param name="editor"></param>
		public static void SetupEditor(EditorControl editor, bool useSkinCss, string preferredProvider, bool allowPageOverride, bool fullUrl, Page page)
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			if (HttpContext.Current.Request is null)
			{
				return;
			}

			if (editor is null)
			{
				return;
			}

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null)
			{
				return;
			}

			string providerName = siteSettings.EditorProviderName;
			if (siteSettings.AllowUserEditorPreference)
			{
				SiteUser siteUser = GetCurrentSiteUser();
				if ((siteUser is not null) && (siteUser.EditorPreference.Length > 0))
				{
					providerName = siteUser.EditorPreference;
				}

			}


			string loweredBrowser = string.Empty;

			if (HttpContext.Current.Request.UserAgent is not null)
			{
				loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();
			}


			if (
				(loweredBrowser.Contains("safari"))
				&& (WebConfigSettings.ForceTinyMCEInSafari)
				)
			{
				providerName = "TinyMCEProvider";
			}

			if (
				(loweredBrowser.Contains("opera"))
				&& (WebConfigSettings.ForceTinyMCEInOpera)
				)
			{
				providerName = "TinyMCEProvider";
			}


			//if (WebConfigSettings.AdaptEditorForMobile)
			//{
			//	if (
			//		(IsMobileDevice() || BrowserHelper.IsIpad())
			//		&& (
			//			(!BrowserHelper.MobileDeviceSupportsWYSIWYG()) || (WebConfigSettings.ForceTextAreaEditorInMobile)
			//			)
			//		)
			//	{
			//		providerName = "TextAreaProvider";
			//		if ((page is not null) && (page is mojoBasePage))
			//		{
			//			mojoBasePage basePage = page as mojoBasePage;
			//			basePage.ScriptConfig.IncludeMarkitUpHtml = true;
			//		}

			//	}
			//}

			string siteRoot = GetNavigationSiteRoot();

			if (!string.IsNullOrEmpty(preferredProvider)) { providerName = preferredProvider; }

			editor.ProviderName = providerName;
			if (editor.WebEditor is not null)
			{
				editor.WebEditor.SiteRoot = siteRoot;

				if (useSkinCss)
				{
					editor.WebEditor.EditorCSSUrl = GetEditorStyleSheetUrl(allowPageOverride, fullUrl, page);
				}

				CultureInfo defaultCulture = SiteUtils.GetDefaultCulture();
				if (defaultCulture.TextInfo.IsRightToLeft)
				{
					editor.WebEditor.TextDirection = Direction.RightToLeft;
				}
			}
		}

		public static int ParseSiteIdFromSkinRequestUrl()
		{
			int siteId = -1;

			if (HttpContext.Current is null) { return siteId; }
			if (HttpContext.Current.Request is null) { return siteId; }

			if (
				(HttpContext.Current.Request.RawUrl.IndexOf("Data/Sites/") == -1)
				|| (HttpContext.Current.Request.RawUrl.IndexOf("/skins/") == -1)
				)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
				if (siteSettings is not null) { return siteSettings.SiteId; }

			}

			string tagged = HttpContext.Current.Request.RawUrl.Replace("/Sites/", "|").Replace("/skins/", "|");
			try
			{
				string strId = tagged.Substring(tagged.IndexOf("|") + 1, tagged.LastIndexOf("|") - tagged.IndexOf("|") - 1);

				//log.Info("Parsing siteId to " + strId);

				int.TryParse(strId, NumberStyles.Integer, CultureInfo.InvariantCulture, out siteId);
			}
			catch (ArgumentOutOfRangeException)
			{
				log.Error("Could not parse siteid from skin url so using SiteSettings.");
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
				if (siteSettings is not null) { return siteSettings.SiteId; }

			}

			return siteId;
		}

		public static string GetSkinName(bool allowPageOverride, Page page = null)
		{

			string currentSkin = WebConfigSettings.DefaultInitialSkin;

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			PageSettings currentPage = CacheHelper.GetCurrentPage();

			if (siteSettings is not null)
			{
				currentSkin = siteSettings.Skin;

				if (
					(allowPageOverride)
					&& (currentPage is not null)
					&& (siteSettings.AllowPageSkins)

					)
				{
					if (currentPage.Skin.Length > 0)
					{
						currentSkin = currentPage.Skin;

					}
				}

				if (
					(siteSettings.AllowUserSkins)
					|| ((WebConfigSettings.AllowEditingSkins) && (WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
					)
				{
					string skinCookieName = "mojoUserSkin" + siteSettings.SiteId.ToString(CultureInfo.InvariantCulture);
					if (CookieHelper.CookieExists(skinCookieName))
					{
						string cookieValue = CookieHelper.GetCookieValue(skinCookieName);
						if (cookieValue.Length > 0)
						{
							currentSkin = cookieValue;
						}
					}
				}

				if (HttpContext.Current.Request.Params.Get("skin") is not null)
				{
					currentSkin = SanitizeSkinParam(HttpContext.Current.Request.Params.Get("skin"));
				}

			}

			return currentSkin;
		}

		public static string GetStyleSheetUrl(Page page)
		{
			bool allowPageOverride = false;

			//string cssUrl = SiteUtils.GetSkinBaseUrl(allowPageOverride, page)
			string cssUrl = SiteUtils.GetNavigationSiteRoot()
			+ "/csshandler.ashx?skin=" + SiteUtils.GetSkinName(allowPageOverride, page);

			return cssUrl;
		}

		public static string ChangeRelativeUrlsToFullyQualifiedUrls(string navigationSiteRoot, string imageSiteRoot, string htmlContent)
		{
			if (string.IsNullOrEmpty(htmlContent)) { return htmlContent; }
			if (string.IsNullOrEmpty(navigationSiteRoot)) { return htmlContent; }
			if (string.IsNullOrEmpty(imageSiteRoot)) { return htmlContent; }

			return htmlContent.Replace("href=\"/", "href=\"" + navigationSiteRoot + "/").Replace("href='/", "href='" + navigationSiteRoot + "/").Replace("src=\"/", "src=\"" + imageSiteRoot + "/").Replace("src='/", "src='" + imageSiteRoot + "/");

		}

		public static string ChangeRelativeLinksToFullyQualifiedLinks(string navigationSiteRoot, string htmlContent)
		{
			if (string.IsNullOrEmpty(htmlContent)) { return htmlContent; }
			if (string.IsNullOrEmpty(navigationSiteRoot)) { return htmlContent; }

			return htmlContent.Replace("href=\"/", "href=\"" + navigationSiteRoot + "/").Replace("href='/", "href='" + navigationSiteRoot + "/");

		}

		public static string ChangeFullyQualifiedLocalUrlsToRelative(string navigationSiteRoot, string imageSiteRoot, string htmlContent)
		{
			if (string.IsNullOrEmpty(htmlContent)) { return htmlContent; }
			if (string.IsNullOrEmpty(navigationSiteRoot)) { return htmlContent; }
			if (string.IsNullOrEmpty(imageSiteRoot)) { return htmlContent; }

			return htmlContent.Replace("href=\"" + navigationSiteRoot + "/", "href=\"/").Replace("href='" + navigationSiteRoot + "/", "href='/").Replace("src=\"" + imageSiteRoot + "/", "src=\"/").Replace("src='" + imageSiteRoot + "/", "src='/");

		}

		public static string GetImageSiteRoot(Page page)
		{
			string imageRoot = page.ResolveUrl("~/");
			if (imageRoot.EndsWith("/")) { imageRoot = imageRoot.Remove(imageRoot.Length - 1, 1); }

			return imageRoot;
		}


		public static string GetNavigationSiteRoot()
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			if (HttpContext.Current.Items["navigationRoot"] is not null)
			{
				return HttpContext.Current.Items["navigationRoot"].ToString();
			}


			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			if (useFolderForSiteDetection)
			{
				return GetRelativeNavigationSiteRoot();
			}

			string navigationRoot = WebUtils.GetSiteRoot();

			if (navigationRoot.StartsWith("http:"))
			{
				var useSSL = false;
				if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings && siteSettings.UseSslOnAllPages) 
				{ 
					useSSL = true;
				}
				if (WebHelper.IsSecureRequest() || useSSL)
				{
					navigationRoot = navigationRoot.Replace("http:", "https:");
				}
			}

			HttpContext.Current.Items["navigationRoot"] = navigationRoot;

			return navigationRoot;

		}

		public static string GetNavigationSiteRoot(SiteSettings siteSettings)
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			if (HttpContext.Current.Items["navigationRoot"] is not null)
			{
				return HttpContext.Current.Items["navigationRoot"].ToString();
			}

			string navigationRoot = WebUtils.GetSiteRoot();
			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			if (useFolderForSiteDetection)
			{

				if ((siteSettings is not null)
					&& (siteSettings.SiteFolderName.Length > 0))
				{
					navigationRoot = navigationRoot + "/" + siteSettings.SiteFolderName;
				}
			}

			if (navigationRoot.StartsWith("http:"))
			{
				var useSSL = false;
				if (siteSettings is not null && siteSettings.UseSslOnAllPages)
				{
					useSSL = true;
				}
				if (WebHelper.IsSecureRequest() || useSSL)
				{
					navigationRoot = navigationRoot.Replace("http:", "https:");
				}
			}

			HttpContext.Current.Items["navigationRoot"] = navigationRoot;

			return navigationRoot;

		}

		public static string GetRelativeNavigationSiteRoot()
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			if (HttpContext.Current.Items["relativenavigationRoot"] is not null)
			{
				return HttpContext.Current.Items["relativenavigationRoot"].ToString();
			}

			string navigationRoot = WebUtils.GetRelativeSiteRoot();
			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			if (useFolderForSiteDetection)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

				if ((siteSettings is not null)
					&& (siteSettings.SiteFolderName.Length > 0))
				{
					navigationRoot = "/" + siteSettings.SiteFolderName;
				}
			}

			HttpContext.Current.Items["relativenavigationRoot"] = navigationRoot;

			return navigationRoot;

		}

		public static string GetSecureNavigationSiteRoot()
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			if (HttpContext.Current.Items["securenavigationRoot"] is not null)
			{
				return HttpContext.Current.Items["securenavigationRoot"].ToString();
			}

			string navigationRoot = WebUtils.GetSecureSiteRoot();
			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			if (useFolderForSiteDetection)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

				if ((siteSettings is not null)
					&& (siteSettings.SiteFolderName.Length > 0))
				{
					navigationRoot = navigationRoot + "/" + siteSettings.SiteFolderName;
				}
			}

			HttpContext.Current.Items["securenavigationRoot"] = navigationRoot;

			return navigationRoot;

		}

		public static string GetInSecureNavigationSiteRoot()
		{
			if (HttpContext.Current is null)
			{
				return string.Empty;
			}

			if (HttpContext.Current.Items["securenavigationRoot"] is not null)
			{
				return HttpContext.Current.Items["securenavigationRoot"].ToString();
			}

			string navigationRoot = WebUtils.GetInSecureSiteRoot();
			bool useFolderForSiteDetection = WebConfigSettings.UseFolderBasedMultiTenants;

			if (useFolderForSiteDetection)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

				if ((siteSettings is not null)
					&& (siteSettings.SiteFolderName.Length > 0))
				{
					navigationRoot = navigationRoot + "/" + siteSettings.SiteFolderName;
				}
			}

			HttpContext.Current.Items["securenavigationRoot"] = navigationRoot;

			return navigationRoot;

		}

		public static string GetCurrentPageUrl()
		{
			PageSettings currentPage = CacheHelper.GetCurrentPage();
			return GetPageUrl(currentPage);

		}

		private static string PageTitleFormatName()
		{
			if (HttpContext.Current is null) { return string.Empty; }

			if (HttpContext.Current.Items["PageTitleFormatName"] is not null)
			{
				return HttpContext.Current.Items["PageTitleFormatName"].ToString();
			}

			HttpContext.Current.Items["PageTitleFormatName"] = WebConfigSettings.PageTitleFormatName;

			return HttpContext.Current.Items["PageTitleFormatName"].ToString();

		}

		private static string PageTitleSeparatorString()
		{
			if (HttpContext.Current is null) { return string.Empty; }

			if (HttpContext.Current.Items["PageTitleSeparatorString"] is not null)
			{
				return HttpContext.Current.Items["PageTitleSeparatorString"].ToString();
			}

			HttpContext.Current.Items["PageTitleSeparatorString"] = WebConfigSettings.PageTitleSeparatorString;

			return HttpContext.Current.Items["PageTitleSeparatorString"].ToString();

		}

		public const string TitleFormat_TitleOnly = "TitleOnly";
		public const string TitleFormat_SitePlusTitle = "SitePlusTitle";
		public const string TitleFormat_TitlePlusSite = "TitlePlusSite";

		public static string FormatPageTitle(SiteSettings siteSettings, string topicTitle)
		{
			if (siteSettings is null)
			{
				return topicTitle;
			}

			string pageTitle = topicTitle;

			if (pageTitle.Contains("$_"))
			{
				// allow tokens in pageTitle so the format can change per page if necessary
				pageTitle = pageTitle.Replace("$_SiteName_$", siteSettings.SiteName).Replace("$_PageTitle_$", topicTitle);
			}
			else
			{
				pageTitle = PageTitleFormatName() switch
				{
					TitleFormat_TitleOnly => topicTitle,
					TitleFormat_TitlePlusSite => $"{topicTitle}{PageTitleSeparatorString()}{siteSettings.SiteName}",
					TitleFormat_SitePlusTitle or _ => $"{siteSettings.SiteName}{PageTitleSeparatorString()}{topicTitle}",
				};
				if ((pageTitle.Length > 65) && WebConfigSettings.AutoTruncatePageTitles)
				{
					pageTitle = UIHelper.CreateExcerpt(pageTitle, 65);
				}
			}

			return pageTitle.Trim();
		}

		public static string GetPageUrl(PageSettings pageSettings)
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (
				siteSettings is null
				|| pageSettings is null
				|| pageSettings.PageId == -1
				|| pageSettings.SiteId != siteSettings.SiteId
				)
			{
				return WebUtils.GetSiteRoot();
			}

			if (pageSettings.Url.StartsWith("http"))
			{
				return pageSettings.Url;
			}

			string resolvedUrl;

			if (pageSettings.UseUrl && WebConfigSettings.UseUrlReWriting)
			{

				if (pageSettings.Url.StartsWith("~/"))
				{
					if (pageSettings.UrlHasBeenAdjustedForFolderSites)
					{
						resolvedUrl = pageSettings.Url.Replace("~/", "/");
					}
					else
					{
						resolvedUrl = $"{GetNavigationSiteRoot()}{pageSettings.Url.Replace("~/", "/")}";
					}
				}
				else
				{
					resolvedUrl = pageSettings.Url;
				}

			}
			else
			{
				resolvedUrl = $"{GetNavigationSiteRoot()}/Default.aspx?pageid={pageSettings.PageId.ToInvariantString()}";
			}

			return resolvedUrl;
		}

		public static string GetFileAttachmentUploadPath()
		{
			if (HttpContext.Current is null) { return string.Empty; }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

			if (siteSettings is null) { return string.Empty; }

			return $"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/Attachments/";
		}

		public static void EnsureFileAttachmentFolder(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return; }
			if (HttpContext.Current is null) { return; }

			string filePath = HttpContext.Current.Server.MapPath($"{WebUtils.GetApplicationRoot()}/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/Attachments/");

			if (!Directory.Exists(filePath))
			{
				try
				{
					Directory.CreateDirectory(filePath);
				}
				catch (IOException ex)
				{
					log.Error($"failed to create path for file attachments {filePath} ", ex);
				}
			}

		}

		/// <summary>
		/// encapsulates checks for a secure connection with configurable server variable checks
		/// </summary>
		/// <returns></returns>
		[Obsolete("Use mojoPortal.Core.Helpers.WebHelper.IsSecureRequest")]
		public static bool IsSecureRequest()
		{
			return WebHelper.IsSecureRequest();
		}

		public static bool SslIsAvailable()
		{
			if (WebConfigSettings.SslisAvailable)
			{
				return true;
			}

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				string key = $"Site{siteSettings.SiteId.ToInvariantString()}-SSLIsAvailable";
				if (ConfigurationManager.AppSettings[key] is not null)
				{
					return Config.ConfigHelper.GetBoolProperty(key, false);
				}
			}

			return false;
		}

		public static void ForceSsl()
		{
			if (WebHelper.IsSecureRequest() || !SslIsAvailable())
			{
				return;
			}

			if (!WebConfigSettings.ProxyPreventsSSLDetection)
			{
				string pageUrl = HttpContext.Current.Request.Url.ToString();
				if (pageUrl.StartsWith("http:"))
				{
					string secureUrl;

					if (WebConfigSettings.IsRunningInRootSite)
					{
						secureUrl = WebUtils.GetSecureSiteRoot() + HttpContext.Current.Request.RawUrl;
					}
					else
					{
						secureUrl = WebUtils.GetSecureHostRoot() + HttpContext.Current.Request.RawUrl;
					}

					if (WebConfigSettings.RedirectSslWith301Status)
					{
						HttpContext.Current.Response.RedirectPermanent(secureUrl, true);
					}
					else
					{
						HttpContext.Current.Response.Redirect(secureUrl, true);
					}
				}
			}
		}

		public static void ClearSsl()
		{
			if (HttpContext.Current is null) { return; }

			if (!WebConfigSettings.ClearSslOnNonSecurePages) { return; }

			string pageUrl = HttpContext.Current.Request.Url.ToString();
			if (pageUrl.StartsWith("https:"))
			{
				string insecureUrl;

				if (WebConfigSettings.IsRunningInRootSite)
				{
					insecureUrl = WebUtils.GetInSecureSiteRoot() + HttpContext.Current.Request.RawUrl;
				}
				else
				{
					insecureUrl = WebUtils.GetInSecureHostRoot() + HttpContext.Current.Request.RawUrl;
				}

				HttpContext.Current.Response.Redirect(insecureUrl, true);
			}
		}

		/// <summary>
		/// this uses the membership provider to encrypt strings the same way that passwords are encrypted
		/// </summary>
		/// <param name="unencrypted"></param>
		/// <returns></returns>
		public static string Encrypt(string unencrypted)
		{
			if (Membership.Provider is not mojoMembershipProvider m)
			{
				throw new InvalidOperationException("could not obtain membership provider to use for encryption");
			}

			return m.EncodePassword(unencrypted, string.Empty, MembershipPasswordFormat.Encrypted);
		}

		/// <summary>
		/// this uses th emembership provider to decrypt strings that were encrypted with the membership provider
		/// </summary>
		/// <param name="encrypted"></param>
		/// <returns></returns>
		public static string Decrypt(string encrypted)
		{
			if (Membership.Provider is not mojoMembershipProvider m)
			{
				throw new System.InvalidOperationException("could not obtain membership provider to use for decryption");
			}

			return m.UnencodePassword(encrypted, MembershipPasswordFormat.Encrypted);
		}


		//http://msdn.microsoft.com/en-us/library/ff649308.aspx
		//public static string GenerateKey(int length)
		//{
		//	byte[] buff = new byte[length / 2];
		//	RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
		//	rng.GetBytes(buff);
		//	StringBuilder key = new StringBuilder(length);
		//	for (int i = 0; i < buff.Length; i++)
		//		key.Append(string.Format("{0:X2}", buff[i]));

		//	return key.ToString();
		//}


		// Based off https://www.niteshluharuka.com/generate-machinekey-using-windows-powershell/
		public static (string, string, string, string) GenerateRandomMachineKey()
		{
			var decryptionAlgorithm = WebConfigSettings.MachineKeyDecryptionAlgorithm;
			var validationAlgorithm = WebConfigSettings.MachineKeyValidationAlgorithm;
			SymmetricAlgorithm decryptionObject;
			HMAC validationObject;

			static string BinaryToHex(byte[] bytes)
			{
				var result = new StringBuilder();

				foreach (var b in bytes)
				{
					result = result.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
				}

				return result.ToString().ToUpperInvariant();
			}

			switch (decryptionAlgorithm)
			{
				case "AES":
					decryptionObject = new AesCryptoServiceProvider();
					break;

				case "DES":
					decryptionObject = new DESCryptoServiceProvider();
					break;

				case "3DES":
					decryptionObject = new TripleDESCryptoServiceProvider();
					break;

				default:
					var e = new Exception("MachineKeyDecryptionAlgorithm's value must be AES, DES, or 3DES");
					log.Error(e.Message, e);
					throw e;
			}

			switch (validationAlgorithm)
			{
				case "MD5":
					validationObject = new HMACMD5();
					break;
				case "SHA1":
					validationObject = new HMACSHA1();
					break;
				case "HMACSHA256":
					validationObject = new HMACSHA256();
					break;
				case "HMACSHA385":
					validationObject = new HMACSHA384();
					break;
				case "HMACSHA512":
					validationObject = new HMACSHA512();
					break;
				default:
					var e = new Exception("MachineKeyValidationAlgorithm's value must be MD5, SHA1, HMACSHA256, HMACSHA385, or HMACSHA512 ");
					log.Error(e.Message, e);
					throw e;
			}

			decryptionObject.GenerateKey();

			var decryptionKey = BinaryToHex(decryptionObject.Key);
			var validationKey = BinaryToHex(validationObject.Key);

			decryptionObject.Dispose();
			validationObject.Dispose();

			return (validationKey, decryptionKey, validationAlgorithm, decryptionAlgorithm);
		}

		public static string GenerateRandomMachineKeyXml()
		{
			var (validationKey, decryptionKey, validationAlgorithm, decryptionAlgorithm) = GenerateRandomMachineKey();

			return $"<machineKey validationKey=\"{validationKey}\" decryptionKey=\"{decryptionKey}\" validation=\"{validationAlgorithm}\" decryption=\"{decryptionAlgorithm}\" />";
		}

		public static string GetEditorProviderName()
		{
			string providerName = "CKEditorProvider";

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				providerName = siteSettings.EditorProviderName;
			}

			if (HttpContext.Current is not null)
			{
				string loweredBrowser = HttpContext.Current.Request.Browser.Browser.ToLower();
				// FCKeditor doesn't work in safari or opera
				// so just force TinyMCE
				if (loweredBrowser.Contains("safari") && WebConfigSettings.ForceTinyMCEInSafari)
				{
					providerName = "TinyMCEProvider";
				}

				if (loweredBrowser.Contains("opera") && WebConfigSettings.ForceTinyMCEInOpera)
				{
					providerName = "TinyMCEProvider";
				}
			}

			return providerName;
		}

		public static bool IsMobileDevice()
		{
			if (HttpContext.Current is null) { return false; }
			if (HttpContext.Current.Request is null) { return false; }
			if (HttpContext.Current.Request.UserAgent is null) { return false; }

			if (!string.IsNullOrWhiteSpace(WebConfigSettings.MobileDetectionExcludeUrlsCsv))
			{
				List<string> excludeUrls = WebConfigSettings.MobileDetectionExcludeUrlsCsv.SplitOnCharAndTrim(',');
				foreach (string u in excludeUrls)
				{
					if (HttpContext.Current.Request.RawUrl.Contains(u)) { return false; }
				}
			}

			string loweredBrowser = HttpContext.Current.Request.UserAgent.ToLower();

			//http://googlewebmastercentral.blogspot.com/2012/11/giving-tablet-users-full-sized-web.html
			//https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=11092~1#post46239
			// Android phones can be differentiated by Android; Mobile;
			if (loweredBrowser.Contains("android") && loweredBrowser.Contains("mobile"))
			{
				return true;
			}

			string mobileAgentsConcat = string.Empty;
			string siteSpecificConfigKey;
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is not null)
			{
				siteSpecificConfigKey = $"Site{siteSettings.SiteId.ToInvariantString()}-MobilePhoneUserAgents";
				if (ConfigurationManager.AppSettings[siteSpecificConfigKey] is not null)
				{
					mobileAgentsConcat = ConfigurationManager.AppSettings[siteSpecificConfigKey];
				}
			}

			// if no site specific setting found use the general web.config setting default value = iphone,android,iemobile
			// if any of these fragments is found in the user agent it is considered a match
			if (mobileAgentsConcat.Length == 0)
			{
				mobileAgentsConcat = WebConfigSettings.MobilePhoneUserAgents;
			}

			List<string> mobileAgents = mobileAgentsConcat.SplitOnCharAndTrim(',');
			foreach (string agent in mobileAgents)
			{
				if (loweredBrowser.Contains(agent)) { return true; }
			}

			return false;
		}

		public const string MobileUseFullViewCookieName = "MobileUseFullViewCookieName";
		public const string NonMobileUseMobileViewCookieName = "UseMobileViewCookieName";

		public static bool UseMobileSkin()
		{
			if (HttpContext.Current is null) { return false; }
			if (HttpContext.Current.Request.UserAgent is null) { return false; }
			if (!WebConfigSettings.AllowMobileSkinForNonMobile && !IsMobileDevice())
			{
				return false;
			}

			//if (!WebConfigSettings.UseMobileSpecificSkin) { return false; }
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null)
			{
				return false;
			}
			if (siteSettings.MobileSkin.Length == 0 && WebConfigSettings.MobilePhoneSkin.Length == 0)
			{
				return false;
			}

			if (CookieHelper.CookieExists(MobileUseFullViewCookieName)) { return false; }

			// we established it is a mobile device and there is no cookie to make it use the full site skin instead of the mobile skin
			if (!IsMobileDevice())
			{
				if (CookieHelper.CookieExists(NonMobileUseMobileViewCookieName))
				{
					return true;
				}
				else
				{
					return false;
				}
			}

			return true;
		}

		#region Url Rewrite tracking

		public static void TrackUrlRewrite()
		{
			if (HttpContext.Current is null)
			{
				return;
			}

			HttpContext.Current.Items["urlwasrewritten"] = true;
		}

		public static bool UrlWasReWritten()
		{
			if (HttpContext.Current.Items["urlwasrewritten"] is not null)
			{
				return true;
			}

			return false;
		}

		#endregion

		public static string GetRoleCookieName(SiteSettings siteSettings)
		{
			string hostName = WebUtils.GetHostName();
			if (WebConfigSettings.UseRelatedSiteMode)
			{
				return $"{hostName}portalroles";
			}

			if (siteSettings is null)
			{
				return $"{hostName}portalroles1";
			}

			return $"{hostName}portalroles{siteSettings.SiteId.ToInvariantString()}";
		}

		public static string GetCssCacheCookieName(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return "csscache";
			}

			return $"csscache{siteSettings.SiteId.ToInvariantString()}";
		}

		public static void RedirectToSignOut()
		{
			if (HttpContext.Current is null) { return; }
			if (HttpContext.Current.Request is null) { return; }

			HttpContext.Current.Response.Clear();
			HttpContext.Current.Response.Redirect($"{SiteUtils.GetNavigationSiteRoot()}/Logoff.aspx", true);
		}

		#region Current User

		public static SiteUser GetCurrentSiteUser()
		{
			bool bypassAuthCheck = false;

			SiteUser currentUser = GetCurrentSiteUser(bypassAuthCheck);

			if ((currentUser is not null) && currentUser.IsLockedOut)
			{
				RedirectToSignOut();
				return null;
			}

			return currentUser;
		}

		public static SiteUser GetCurrentSiteUser(bool bypassAuthCheck)
		{
			if (HttpContext.Current is null)
			{
				return null;
			}

			if (bypassAuthCheck || HttpContext.Current.Request.IsAuthenticated)
			{
				if (HttpContext.Current.Items["CurrentUser"] is not null)
				{
					return (SiteUser)HttpContext.Current.Items["CurrentUser"];
				}

				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
				if (siteSettings is not null)
				{
					SiteUser siteUser = new SiteUser(siteSettings, HttpContext.Current.User.Identity.Name);
					if (siteUser.UserId > -1)
					{
						HttpContext.Current.Items["CurrentUser"] = siteUser;
						return siteUser;
					}
				}
			}

			return null;
		}

		public static string SuggestLoginNameFromEmail(int siteId, string email)
		{
			string login = email.Substring(0, email.IndexOf("@"));
			int offset = 1;
			while (SiteUser.LoginExistsInDB(siteId, login))
			{
				offset += 1;
				login = email.Substring(0, email.IndexOf("@")) + offset.ToInvariantString();
			}

			return login;
		}

		public static SiteUser CreateMinimalUser(SiteSettings siteSettings, string email, bool includeInMemberList, string adminComments)
		{
			if (siteSettings is null)
			{
				throw new ArgumentException("a valid siteSettings object is required for this method");
			}
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("a valid email address is required for this method");
			}

			if (!Email.IsValidEmailAddressSyntax(email))
			{
				throw new ArgumentException("a valid email address is required for this method");
			}

			//first make sure he doesn't exist
			SiteUser siteUser = SiteUser.GetByEmail(siteSettings, email);
			if (siteUser is not null && siteUser.UserGuid != Guid.Empty)
			{
				return siteUser;
			}

			string login = SuggestLoginNameFromEmail(siteSettings.SiteId, email);

			siteUser = new SiteUser(siteSettings)
			{
				Email = email,
				LoginName = login,
				Name = login,
				Password = SiteUser.CreateRandomPassword(siteSettings.MinRequiredPasswordLength + 2, WebConfigSettings.PasswordGeneratorChars),
				ProfileApproved = true,
				DisplayInMemberList = includeInMemberList,
				PasswordQuestion = Resource.ManageUsersDefaultSecurityQuestion,
				PasswordAnswer = Resource.ManageUsersDefaultSecurityAnswer,
				Comment = adminComments
			};

			if (Membership.Provider is mojoMembershipProvider m)
			{
				siteUser.Password = m.EncodePassword(siteSettings, siteUser, siteUser.Password);
			}

			siteUser.Save();

			Role.AddUserToDefaultRoles(siteUser);

			return siteUser;
		}

		public static bool UserIsSiteEditor()
		{
			if (WebUser.IsAdmin) { return true; }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is not null)
			{
				return (WebConfigSettings.UseRelatedSiteMode) && (WebUser.IsInRoles(siteSettings.SiteRootEditRoles));
			}

			return false;
		}

		public static bool UserCanEditModule(int moduleId)
		{
			if (HttpContext.Current is null)
			{
				return false;
			}

			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return false;
			}

			if (WebUser.IsAdminOrContentAdmin)
			{
				return true;
			}

			PageSettings currentPage = CacheHelper.GetCurrentPage();
			if (currentPage is null)
			{
				return false;
			}

			bool moduleFoundOnPage = false;
			foreach (Module m in currentPage.Modules)
			{
				if (m.ModuleId == moduleId)
				{
					moduleFoundOnPage = true;
				}
			}

			if (!moduleFoundOnPage)
			{
				return false;
			}

			if (WebUser.IsInRoles(currentPage.EditRoles))
			{
				return true;
			}

			SiteUser currentUser = GetCurrentSiteUser();
			if (currentUser is null)
			{
				return false;
			}

			foreach (Module m in currentPage.Modules)
			{
				if (m.ModuleId == moduleId)
				{
					if (m.EditUserId == currentUser.UserId)
					{
						return true;
					}

					if (WebUser.IsInRoles(m.AuthorizedEditRoles))
					{
						return true;
					}
				}
			}

			return false;
		}

		public static void TrackUserActivity()
		{
			if (HttpContext.Current is null) { return; }
			if (HttpContext.Current.Request is null) { return; }
			if (!HttpContext.Current.User.Identity.IsAuthenticated) { return; }
			if (!WebConfigSettings.TrackAuthenticatedRequests) { return; }

			bool bypassAuthCheck = false;
			if ((GetCurrentSiteUser(bypassAuthCheck) is SiteUser siteUser) && (siteUser.UserId > -1))
			{
				siteUser.UpdateLastActivityTime();
				if (debugLog) { log.Debug($"Tracked user activity for request {HttpContext.Current.Request.RawUrl}"); }

				if (WebConfigSettings.TrackIPForAuthenticatedRequests)
				{

					if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
					{
						// track user ip address
						var userLocation = new UserLocation(siteUser.UserGuid, GetIP4Address())
						{
							SiteGuid = siteSettings.SiteGuid,
							Hostname = HttpContext.Current.Request.UserHostName
						};
						userLocation.Save();
					}
				}
			}
		}

		#endregion

		public static void QueueIndexing()
		{
			if (WebConfigSettings.DisableSearchIndex) { return; }

			if (!WebConfigSettings.IsSearchIndexingNode) { return; }

			if (IndexWriterTask.IsRunning()) { return; }

			IndexWriterTask indexWriter = new IndexWriterTask();

			indexWriter.StoreContentForResultsHighlighting = WebConfigSettings.EnableSearchResultsHighlighting;

			// Commented out 2009-01-24
			// seems to cause errors for some languages if we localize this
			// perhaps because the background thread is not running on the same culture as the
			// web ui which is driven by browser language preferecne.
			// if we do localize it we should localize to the site default culture, not the user's
			//indexWriter.TaskName = Resource.IndexWriterTaskName;
			//indexWriter.StatusCompleteMessage = Resource.IndexWriterTaskCompleteMessage;
			//indexWriter.StatusQueuedMessage = Resource.IndexWriterTaskQueuedMessage;
			//indexWriter.StatusStartedMessage = Resource.IndexWriterTaskStartedMessage;
			//indexWriter.StatusRunningMessage = Resource.IndexWriterTaskRunningFormatString;

			indexWriter.QueueTask();

			WebTaskManager.StartOrResumeTasks();
		}

		public static string GetFullPathToThemeFile()
		{
			if (HttpContext.Current is not null)
			{
				SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
				return HttpContext.Current.Server.MapPath($"~/Data/Sites/{siteSettings.SiteId.ToInvariantString()}/skins/{siteSettings.Skin}/theme.skin");
			}

			return null;
		}

		public static int GetCurrentPageDepth(SiteMapNode rootNode)
		{
			if ((CacheHelper.GetCurrentPage() is PageSettings pageSettings) && (pageSettings.ParentId > -1))
			{
				//SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
				if (GetSiteMapNodeForPage(rootNode, pageSettings) is mojoSiteMapNode currentPageNode)
				{
					mojoSiteMapNode node = currentPageNode;
					return node.Depth;
				}
				return 0;
			}
			return 0;
		}

		public static string GetActivePageValuePath(SiteMapNode rootNode, int offSet)
		{
			return GetActivePageValuePath(rootNode, offSet, string.Empty);
		}

		/// <summary>
		/// this overload was added to support menu highlighting on physical .aspx pages added to the menu
		/// </summary>
		/// <param name="rootNode"></param>
		/// <param name="offSet"></param>
		/// <param name="currentUrl"></param>
		/// <returns></returns>
		public static string GetActivePageValuePath(SiteMapNode rootNode, int offSet, string currentUrl = "")
		{
			string valuePath = string.Empty;

			mojoSiteMapNode currentPageNode = null;

			if (string.IsNullOrWhiteSpace(currentUrl))
			{
				if (CacheHelper.GetCurrentPage() is PageSettings pageSettings)
				{
					currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
				}
			}
			else
			{
				currentPageNode = GetSiteMapNodeForPage(rootNode, currentUrl);
			}

			if (currentPageNode is not null)
			{
				valuePath = currentPageNode.PageGuid.ToString();

				while (currentPageNode.ParentId > -1)
				{
					if (currentPageNode.ParentNode is not null)
					{
						currentPageNode = currentPageNode.ParentNode as mojoSiteMapNode;

						valuePath = $"{currentPageNode.PageGuid}|{valuePath}";
					}
				}

				if (offSet > 0)
				{
					for (int i = 1; i <= offSet; i++)
					{
						if (valuePath.IndexOf("|") > -1)
						{
							valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
						}
					}
				}
			}

			return valuePath;
		}

		public static String GetPageMenuActivePageValuePath(SiteMapNode rootNode)
		{

			String valuePath = GetActivePageValuePath(rootNode, 0);

			// need to remove the topmost level from value path
			// which is from the beginning to the first separator
			if (valuePath.IndexOf("|") > -1)
			{
				valuePath = valuePath.Remove(0, valuePath.IndexOf("|") + 1);
			}

			return valuePath;
		}

		public static bool TopPageHasChildren(SiteMapNode rootNode)
		{
			return TopPageHasChildren(rootNode, 0);
		}

		public static mojoSiteMapNode GetCurrentPageSiteMapNode(SiteMapNode rootNode)
		{
			if (rootNode is null)
			{
				return null;
			}

			if (CacheHelper.GetCurrentPage() is PageSettings currentPage)
			{
				return GetSiteMapNodeForPage(rootNode, currentPage);
			}

			return null;
		}

		public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, PageSettings pageSettings)
		{
			if (rootNode is null) { return null; }
			if (pageSettings is null) { return null; }
			if (rootNode is not mojoSiteMapNode) { return null; }

			return GetSiteMapNodeForPage(rootNode, pageSettings.PageId);
		}

		public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, int pageId)
		{
			if (rootNode is null) { return null; }

			if (rootNode is not mojoSiteMapNode) { return null; }

			foreach (SiteMapNode childNode in rootNode.GetAllNodes())
			{
				if (childNode is not mojoSiteMapNode) { return null; }

				mojoSiteMapNode node = childNode as mojoSiteMapNode;
				if (node.PageId == pageId) { return node; }
			}

			return null;
		}

		/// <summary>
		/// this overload was added to implement support for menu highlighting in inline code .aspx pages
		/// added to the menu
		/// </summary>
		/// <param name="rootNode"></param>
		/// <param name="currentUrl"></param>
		/// <returns></returns>
		public static mojoSiteMapNode GetSiteMapNodeForPage(SiteMapNode rootNode, string currentUrl)
		{
			if (rootNode is null) { return null; }
			if (string.IsNullOrEmpty(currentUrl)) { return null; }
			if (rootNode is not mojoSiteMapNode) { return null; }

			foreach (SiteMapNode childNode in rootNode.ChildNodes)
			{
				if (childNode is not mojoSiteMapNode) { return null; }

				mojoSiteMapNode node = childNode as mojoSiteMapNode;
				if (string.Equals(node.Url.Replace("~", string.Empty), currentUrl, StringComparison.InvariantCultureIgnoreCase)) { return node; }

				mojoSiteMapNode foundNode = GetSiteMapNodeForPage(node, currentUrl);
				if (foundNode is not null) { return foundNode; }
			}

			return null;
		}

		public static mojoSiteMapNode GetTopLevelParentNode(SiteMapNode siteMapNode)
		{
			if (siteMapNode is null) { return null; }
			if (siteMapNode is not mojoSiteMapNode) { return null; }

			mojoSiteMapNode node = siteMapNode as mojoSiteMapNode;

			if (node.ParentId < 0) { return node; }

			while ((node is not null) && (node.ParentId > -1))
			{
				if (node.ParentNode is not null)
				{
					node = node.ParentNode as mojoSiteMapNode;
				}
				else
				{
					return node;
				}
			}

			return node;
		}

		public static mojoSiteMapNode GetOffsetNode(SiteMapNode siteMapNode, int startingNodeOffset)
		{
			if (siteMapNode is null) { return null; }
			if (siteMapNode is not mojoSiteMapNode) { return null; }

			mojoSiteMapNode node = siteMapNode as mojoSiteMapNode;

			if (node.ParentId < 0) { return node; }
			if (node.Depth == startingNodeOffset) { return node; }

			if (node.Depth < startingNodeOffset)
			{
				// requested starting node should be below the passed in node
				while ((node is not null) && (node.Depth < startingNodeOffset))
				{
					if (node.ChildNodes.Count > 0)
					{
						node = (mojoSiteMapNode)node.ChildNodes[0];
						if (node.Depth == startingNodeOffset) { return node; }
					}
				}
			}
			else
			{
				//node.Depth > startingNodeOffset so climb up through parnet nodes

				while ((node is not null) && (node.ParentId > -1))
				{
					if (node.ParentNode is not null)
					{
						node = node.ParentNode as mojoSiteMapNode;
						if (node.Depth == startingNodeOffset) { return node; }
					}
					else
					{
						return node;
					}
				}
			}

			return node;
		}

		public static bool NodeHasVisibleChildrenAtDepth(mojoSiteMapNode node, int depth)
		{
			bool recurse = true;
			return NodeHasVisibleChildrenAtDepth(node, depth, recurse);
		}

		public static bool NodeHasVisibleChildrenAtDepth(mojoSiteMapNode node, int depth, bool recurse)
		{
			foreach (SiteMapNode cNode in node.ChildNodes)
			{
				if (cNode is not mojoSiteMapNode) { return false; }

				mojoSiteMapNode childNode = cNode as mojoSiteMapNode;
				if (childNode.Depth >= depth)
				{
					if (childNode.IncludeInMenu
						&& !(childNode.PublishMode == (int)ContentPublishMode.MobileOnly)
						&& WebUser.IsInRoles(childNode.ViewRoles)
						)
					{
						return true;
					}
				}
				else
				{
					if (recurse)
					{
						if (NodeHasVisibleChildrenAtDepth(childNode, depth, recurse))
						{
							return true;
						}
					}
				}
			}

			return false;
		}

		/// <summary>
		/// A helper method for determining if the top level page has children at this offsetlevel
		/// used to determine whether to show or hide left or right div if it contains a menu.
		/// </summary>
		/// <param name="startingNodeOffset"></param>
		/// <returns></returns>
		public static bool TopPageHasChildren(SiteMapNode rootNode, int startingNodeOffset)
		{
			if (rootNode is null) { return false; }

			PageSettings pageSettings = CacheHelper.GetCurrentPage();

			if (pageSettings is null)
			{
				return false;
			}

			if (pageSettings.ParentId == -1 && startingNodeOffset > 0)
			{
				return false;
			}

			if (pageSettings.ParentId > -1
				&& pageSettings.IncludeInMenu //added 2009-05-06
				&& startingNodeOffset == 0
				)
			{
				return true;
			}

			mojoSiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
			if (currentPageNode is null) { return false; }

			if (startingNodeOffset >= 2)
			{
				if (currentPageNode.Depth >= startingNodeOffset && currentPageNode.IncludeInMenu) { return true; }

				bool recurse = false;
				mojoSiteMapNode parent;

				if (NodeHasVisibleChildrenAtDepth(currentPageNode, startingNodeOffset, recurse)) { return true; }

				if (currentPageNode.ParentNode is not null)
				{
					parent = currentPageNode.ParentNode as mojoSiteMapNode;
					if (parent.Depth >= startingNodeOffset)
					{
						return NodeHasVisibleChildrenAtDepth(parent, startingNodeOffset, recurse);
					}
				}

				return false;
			}

			mojoSiteMapNode topParent = GetTopLevelParentNode(currentPageNode);
			if (topParent is null) { return false; }

			if (NodeHasVisibleChildrenAtDepth(topParent, startingNodeOffset)) { return true; }

			return false;
		}

		public static String GetStartUrlForPageMenu(SiteMapNode rootNode, int startingNodeOffset)
		{

			PageSettings pageSettings = CacheHelper.GetCurrentPage();

			SiteMapNode currentPageNode = GetSiteMapNodeForPage(rootNode, pageSettings);
			if (currentPageNode is null) { return string.Empty; }

			if (startingNodeOffset == 0)
			{
				SiteMapNode startingNode = GetTopLevelParentNode(currentPageNode);
				if (startingNode is null) { return string.Empty; }

				return startingNode.Url;
			}

			//work our way up from the current page to the parent node at the offset depth
			mojoSiteMapNode n = currentPageNode as mojoSiteMapNode;

			while (n.ParentNode is not null && n.Depth > startingNodeOffset)
			{
				n = n.ParentNode as mojoSiteMapNode;
			}

			return n.Url;
		}

		public static void PropagateCurrentPagePermissionsToAllChildPages()
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return; }

			var siteMapDataSource = new SiteMapDataSource
			{
				SiteMapProvider = $"mojosite{siteSettings.SiteId.ToInvariantString()}"
			};

			mojoSiteMapNode currentPageNode = GetCurrentPageSiteMapNode(siteMapDataSource.Provider.RootNode);

			foreach (SiteMapNode childNode in currentPageNode.GetAllNodes())
			{
				if (childNode is not mojoSiteMapNode) { continue; }

				mojoSiteMapNode node = childNode as mojoSiteMapNode;
				var page = new PageSettings(node.PageGuid)
				{
					AuthorizedRoles = currentPageNode.ViewRoles,
					EditRoles = currentPageNode.EditRoles,
					CreateChildPageRoles = currentPageNode.CreateChildPageRoles,
					DraftEditOnlyRoles = currentPageNode.DraftEditRoles,
					CreateChildDraftRoles = currentPageNode.CreateChildDraftPageRoles
				};
				page.Save();
			}

			CacheHelper.ResetSiteMapCache(siteSettings.SiteId);
		}

		public static string GetPrivateProfileUrl()
		{
			return WebConfigSettings.PrivateProfileRelativeUrl;
		}

		public static string GetPublicProfileUrl(int userId)
		{
			var url = WebConfigSettings.PublicProfileRelativeUrl;
			if (url.Contains("?"))
			{
				return $"{url}&userid={userId.ToInvariantString()}";
			}
			else
			{
				return $"{url}?userid={userId.ToInvariantString()}";
			}
		}

		public static string GetProfileLink(object objUserId, object userName)
		{
			string result = string.Empty;
			if (objUserId is not null)
			{
				int userId = Convert.ToInt32(objUserId, CultureInfo.InvariantCulture);

				if (userName.ToString().Length > 0)
				{
					result = $"<a  href='{GetNavigationSiteRoot()}/ProfileView.aspx?userid={userId.ToInvariantString()}'>{userName}</a>";
				}
			}

			return result;
		}

		public static string GetProfileLink(Page page, object objUserId, object userName)
		{
			string result = string.Empty;
			if (objUserId is not null)
			{
				int userId = Convert.ToInt32(objUserId, CultureInfo.InvariantCulture);

				if (userName.ToString().Length > 0)
				{
					result = $"<a class='profileviewlink' href='{GetNavigationSiteRoot()}/ProfileView.aspx?userid={userId.ToInvariantString()}'>{userName}</a>";
				}
			}

			return result;
		}

		public static string GetProfileAvatarLink(
			Page page,
			object objUserId,
			int siteId,
			string avatar,
			string toolTip)
		{
			string result = string.Empty;
			if (objUserId is not null)
			{
				int userId = Convert.ToInt32(objUserId);


				if (string.IsNullOrWhiteSpace(avatar))
				{
					avatar = "blank.gif";
				}
				string avaterImageMarkup = $"<img title=\"{toolTip}\" alt=\"{toolTip}\" src=\"{page.ResolveUrl("~/Data/Sites/" + siteId.ToString(CultureInfo.InvariantCulture) + "/useravatars/" + avatar)}\" />";

				result = $"<a title=\"{toolTip}\" href=\"{GetNavigationSiteRoot()}/ProfileView.aspx?userid={userId.ToString(CultureInfo.InvariantCulture)}\">{avaterImageMarkup}</a>";
			}

			return result;
		}

		public static string GetGmapApiKey()
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (!string.IsNullOrWhiteSpace(siteSettings?.GmapApiKey))
			{
				return siteSettings.GmapApiKey;
			}

			return WebConfigSettings.GoogleMapsAPIKey;
		}

		public static string GetBingApiId()
		{
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.BingAPIId))
			{
				return WebConfigSettings.BingAPIId;

			}
			else
			{
				if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
				{
					return siteSettings.BingAPIId;
				}
			}

			return string.Empty;
		}

		public static string GetSearchDomain()
		{
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.CustomSearchDomain))
			{
				return WebConfigSettings.CustomSearchDomain.Trim();
			}
			else
			{
				return WebUtils.GetHostName();
			}
		}

		public static string GetGoogleCustomSearchId()
		{
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.GoogleCustomSearchId))
			{
				return WebConfigSettings.GoogleCustomSearchId;
			}

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				return siteSettings.GoogleCustomSearchId;
			}

			return string.Empty;
		}

		public static string GetPrimarySearchProvider()
		{
			if (!string.IsNullOrWhiteSpace(WebConfigSettings.PrimarySearchEngine))
			{
				return WebConfigSettings.PrimarySearchEngine;
			}

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				return siteSettings.PrimarySearchEngine switch
				{
					"bing" when !string.IsNullOrWhiteSpace(siteSettings.BingAPIId) => siteSettings.PrimarySearchEngine,
					"google" when !string.IsNullOrWhiteSpace(siteSettings.GoogleCustomSearchId) => siteSettings.PrimarySearchEngine,
					_ => "internal"
				};
			}

			return "internal";
		}

		public static bool ShowAlternateSearchIfConfigured()
		{
			if (WebConfigSettings.ShowAlternateSearchIfConfigured)
			{
				return true;
			}

			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings)
			{
				return siteSettings.ShowAlternateSearchIfConfigured;
			}

			return false;
		}

		public static bool DisableRecentContentFeed(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return WebConfigSettings.DisableRecentContentFeed;
			}

			string key = $"Site{siteSettings.SiteId.ToInvariantString()}-DisableRecentContentFeed";

			return Config.ConfigHelper.GetBoolProperty(key, WebConfigSettings.DisableRecentContentFeed);
		}

		public static string RecentContentChannelDescription(SiteSettings siteSettings)
		{
			if (siteSettings is null)
			{
				return WebConfigSettings.RecentContentChannelDescription;
			}

			string key = $"Site{siteSettings.SiteId.ToInvariantString()}-RecentContentChannelDescription";

			return Config.ConfigHelper.GetStringProperty(key, WebConfigSettings.RecentContentChannelDescription);
		}

		public static string RecentContentChannelCopyright(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentChannelCopyright; }

			string key = $"Site{siteSettings.SiteId.ToInvariantString()}-RecentContentChannelCopyright";

			return Config.ConfigHelper.GetStringProperty(key, WebConfigSettings.RecentContentChannelCopyright);
		}

		public static string RecentContentChannelNotifyEmail(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentChannelNotifyEmail; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentChannelNotifyEmail";

			return Config.ConfigHelper.GetStringProperty(key, WebConfigSettings.RecentContentChannelNotifyEmail);

		}

		public static int RecentContentFeedMaxDaysOld(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentFeedMaxDaysOld; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentFeedMaxDaysOld";

			return Config.ConfigHelper.GetIntProperty(key, WebConfigSettings.RecentContentFeedMaxDaysOld);

		}

		//RecentContentChannelDescription

		public static int RecentContentDefaultItemsToRetrieve(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentDefaultItemsToRetrieve; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentDefaultItemsToRetrieve";

			return Config.ConfigHelper.GetIntProperty(key, WebConfigSettings.RecentContentDefaultItemsToRetrieve);

		}

		public static int RecentContentMaxItemsToRetrieve(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentMaxItemsToRetrieve; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentMaxItemsToRetrieve";

			return Config.ConfigHelper.GetIntProperty(key, WebConfigSettings.RecentContentMaxItemsToRetrieve);

		}

		public static int RecentContentFeedTimeToLive(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentFeedTimeToLive; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentFeedTimeToLive";

			return Config.ConfigHelper.GetIntProperty(key, WebConfigSettings.RecentContentFeedTimeToLive);

		}

		public static int RecentContentFeedCacheTimeInMinutes(SiteSettings siteSettings)
		{
			if (siteSettings is null) { return WebConfigSettings.RecentContentFeedCacheTimeInMinutes; }

			string key = "Site" + siteSettings.SiteId.ToInvariantString() + "-RecentContentFeedCacheTimeInMinutes";

			return Config.ConfigHelper.GetIntProperty(key, WebConfigSettings.RecentContentFeedCacheTimeInMinutes);

		}

		public static bool RedirectToPageAfterCreation(SiteSettings siteSettings)
		{
			return Config.ConfigHelper.GetSiteConfigSetting(siteSettings.SiteId, "RedirectToPageAfterCreation", WebConfigSettings.RedirectToNewPageOnCreationGlobalDefault);
		}


		public static CultureInfo GetDefaultCulture()
		{
			if (CacheHelper.GetCurrentSiteSettings() is SiteSettings siteSettings && siteSettings.SiteId > -1)
			{
				string siteCultureKey = "site" + siteSettings.SiteId.ToInvariantString() + "culture";

				if (ConfigurationManager.AppSettings[siteCultureKey] is not null)
				{
					try
					{
						string cultureName = ConfigurationManager.AppSettings[siteCultureKey];

						// change these neutral cultures which cannot be used to reasonable specific cultures
						if (cultureName == "zh-CHS") { cultureName = "zh-CN"; }
						if (cultureName == "zh-CHT") { cultureName = "zh-HK"; }

						CultureInfo siteCulture = new CultureInfo(cultureName);
						return siteCulture;
					}
					catch { }
				}
			}

			return new CultureInfo("en-US");

		}

		public static CultureInfo GetDefaultUICulture()
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if ((siteSettings is not null) && (siteSettings.SiteId > -1))
			{
				return GetDefaultUICulture(siteSettings.SiteId);
			}

			// return ResourceHelper.GetDefaultUICulture();

			return new CultureInfo("en-US");

		}

		public static CultureInfo GetDefaultUICulture(int siteId)
		{
			string siteCultureKey = "site" + siteId.ToInvariantString() + "uiculture";

			if (ConfigurationManager.AppSettings[siteCultureKey] is not null)
			{
				try
				{
					string cultureName = ConfigurationManager.AppSettings[siteCultureKey];

					// change these neutral cultures which cannot be used to reasonable specific cultures
					if (cultureName == "zh-CHS") { cultureName = "zh-CN"; }
					if (cultureName == "zh-CHT") { cultureName = "zh-HK"; }

					CultureInfo siteCulture = new CultureInfo(cultureName);
					return siteCulture;
				}
				catch { }
			}
			else
			{
				if (WebConfigSettings.UseCultureForUICulture)
				{
					return GetDefaultCulture();
				}

			}

			return ResourceHelper.GetDefaultUICulture();

		}

		public static Guid GetDefaultCountry()
		{

			// US
			Guid defaultCountry = new Guid("a71d6727-61e7-4282-9fcb-526d1e7bc24f");


			SiteSettings site = CacheHelper.GetCurrentSiteSettings();
			if (site is not null)
			{
				if (site.DefaultCountryGuid != Guid.Empty)
				{
					return site.DefaultCountryGuid;
				}
			}

			return defaultCountry;

		}

		public static string GetTimeZoneLabel(double timeOffset)
		{
			string key = "TZ" + timeOffset.ToString(CultureInfo.InvariantCulture).Replace(".", string.Empty).Replace("-", "minus");
			return ResourceHelper.GetResourceString("TimeZoneResources", key);

		}

		public static List<TimeZoneInfo> GetTimeZoneList()
		{

			List<TimeZoneInfo> timeZones = null;

			if (HttpContext.Current is not null)
			{
				timeZones = HttpContext.Current.Items["tzlist"] as List<TimeZoneInfo>;
			}
			if (timeZones is null)
			{
				if (WebConfigSettings.CacheTimeZoneList)
				{
					timeZones = CacheHelper.GetTimeZones();
				}
				else
				{
					timeZones = DateTimeHelper.GetTimeZoneList();
				}

				if (timeZones is not null)
				{
					if (HttpContext.Current is not null)
					{
						HttpContext.Current.Items["tzlist"] = timeZones;
					}
				}
			}
			return timeZones;
		}



		public static TimeZoneInfo GetTimeZone(string id)
		{
			if (string.IsNullOrEmpty(id)) { return null; }
			List<TimeZoneInfo> timeZones = GetTimeZoneList();
			return DateTimeHelper.GetTimeZone(timeZones, id);
		}


		public static TimeZoneInfo GetSiteTimeZone()
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return null; }
			return GetTimeZone(siteSettings.TimeZoneId);

		}

		public static TimeZoneInfo GetUserTimeZone()
		{
			SiteUser siteUser = GetCurrentSiteUser();
			if ((siteUser is not null) && (siteUser.TimeZoneId.Length > 0))
			{
				return GetTimeZone(siteUser.TimeZoneId);
			}


			return GetSiteTimeZone();
		}

		private static Double GetSiteTimeZoneOffset()
		{
			Double timeOffset = 0;
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if (siteSettings is null) { return timeOffset; }

			string cacheKey = "sitetzoffset_" + siteSettings.SiteId.ToInvariantString();
			object o = HttpContext.Current.Items[cacheKey];

			try
			{
				if (o is not null)
				{
					return Convert.ToDouble(o);
				}
				else
				{
					TimeZoneInfo tz = GetTimeZone(siteSettings.TimeZoneId);
					if (tz is null) { return timeOffset; }
					Double siteTimeOffset = tz.GetUtcOffset(DateTime.UtcNow).TotalHours;
					HttpContext.Current.Items[cacheKey] = siteTimeOffset;
					return siteTimeOffset;
				}
			}
			catch (Exception ex)
			{
				log.Error(ex);
				return timeOffset;
			}


		}

		public static double GetUserTimeOffset()
		{
			Double timeOffset = 0;

			if (HttpContext.Current is null) { return timeOffset; }
			if (HttpContext.Current.Request is null) { return timeOffset; }


			if (!HttpContext.Current.Request.IsAuthenticated)
			{
				return GetSiteTimeZoneOffset();
			}

			SiteUser siteUser = GetCurrentSiteUser();
			if (siteUser is not null)
			{


				if (siteUser.TimeZoneId.Length == 0) { return siteUser.TimeOffsetHours; }
				string cacheKey = "tzoffset_" + siteUser.UserId.ToInvariantString();
				object o = HttpContext.Current.Items[cacheKey];

				try
				{
					if (o is not null)
					{
						return Convert.ToDouble(o);
					}
					else
					{
						TimeZoneInfo tz = GetTimeZone(siteUser.TimeZoneId);
						if (tz is null) { return siteUser.TimeOffsetHours; }
						Double userTimeOffset = tz.GetUtcOffset(DateTime.UtcNow).TotalHours;
						HttpContext.Current.Items[cacheKey] = userTimeOffset;
						return userTimeOffset;
					}
				}
				catch (Exception ex)
				{
					log.Error(ex);
					return siteUser.TimeOffsetHours;
				}
			}

			return timeOffset;

		}


		public static CommerceConfiguration GetCommerceConfig()
		{
			if (HttpContext.Current is null)
			{
				return null;
			}

			if (HttpContext.Current.Items["commerceConfig"] is not null)
			{
				return (CommerceConfiguration)HttpContext.Current.Items["commerceConfig"];
			}

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

			if ((siteSettings is null) || (siteSettings.SiteGuid == Guid.Empty))
			{
				return null;
			}

			CommerceConfiguration commerceConfig = new CommerceConfiguration(siteSettings);

			HttpContext.Current.Items.Add("commerceConfig", commerceConfig);

			return commerceConfig;


		}

		public static bool CheckForBadWords(string toBeChecked)
		{
			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
			if ((siteSettings is null) || (siteSettings.SiteGuid == Guid.Empty))
			{
				return false;
			}

			foreach (string badword in siteSettings.BadWordList.SplitOnNewLineAndTrim())
			{
				var testValue = $"\\b{badword}\\b";
				var regex = new Regex(testValue, RegexOptions.IgnoreCase);
				var match = regex.Match(toBeChecked);
				if (match.Captures.Count > 0)
				{
					return true;
				}
				//if (toBeChecked.ContainsCaseInsensitive(badword)) return true;
			}

			return false;
		}

		public static bool ContainsBadWords(this string s)
		{
			return CheckForBadWords(s);
		}

	}
}
