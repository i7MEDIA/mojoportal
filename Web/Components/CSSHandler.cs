using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;

using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI;

/// <summary>
/// the handler will combine the css files in the order they are listed in
/// the css.config file located in the skin folder
/// It will also remove white space (and comments?)
/// This can improve performance as measured using the YSlow Firefox plugin
/// </summary>
public class CssHandler : IHttpHandler
{
	private static readonly ILog log = LogManager.GetLogger(typeof(CssHandler));
	private const string POST = "POST";
	private const bool DO_GZIP = true;
	private const string responseContentType = "text/css";
	private string urlReplacement = """url("{0}")""";

	private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(AppConfig.CssCacheInDays);

	private string skinVersion = string.Empty;

	//private readonly static Regex URL_REGEX = new Regex(@"url\((\""|\')?(?<path>(.*))?(\""|\')?\)", RegexOptions.Compiled);

	// 2014-12-09 URL_REGEX changed from the above per Todd Hansen to fix multi url on a line problem
	// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12924~1#post53334

	//private readonly static Regex URL_REGEX = new Regex(@"url\((\""|\')?(?<path>(.*?))?(\""|\')?\)", RegexOptions.Compiled);
	private readonly static Regex URL_REGEX = new(AppConfig.CssHandlerUrlRegEx, RegexOptions.Compiled);

	// We don't want to append "/Data/Sites/[site number]/skins/[skin folder]" to external links
	// so we check all "url()" links if they start with "http://", "https://", "data:", and "//" to ignore them if they do
	//private readonly static Regex urlPathsToIgnore = new Regex(@"^(https?://|data:|//)");
	private readonly static Regex urlPathsToIgnore = new(AppConfig.CssHandlerUrlIgnoreRegEx);


	public void ProcessRequest(HttpContext context)
	{
		context.Response.ContentType = responseContentType;

		if (context.Request.RequestType == POST)
		{
			return;
		}

		var isCompressed = DO_GZIP && CanGZip(context.Request);

		var siteId = SiteUtils.ParseSiteIdFromSkinRequestUrl();

		var skinName = "framework";

		if (context.Request["skin"] != null)
		{
			skinName = SiteUtils.SanitizeSkinParam(context.Request["skin"]);
		}

		//var media = "screen";

		//if (context.Request["media"] != null)
		//{
		//	media = context.Request["media"];
		//}

		skinVersion = WebUtils.ParseGuidFromQueryString("sv", Guid.Empty).ToString();

		var encoding = new UTF8Encoding(false);

		if (!WriteFromCache(context, siteId, skinName, isCompressed))
		{
			byte[] cssBytes = GetCss(context, siteId, skinName, encoding);

			using var memoryStream = new MemoryStream(5000);
			using (Stream writer = isCompressed ? new GZipStream(memoryStream, CompressionMode.Compress) : memoryStream)
			{
				writer.Write(cssBytes, 0, cssBytes.Length);
			}

			byte[] responseBytes = memoryStream.ToArray();

			if (ShouldCacheOnServer())
			{
				// TODO: Cache CSS to disk instead of memory or make it configurable
				lock (this)
				{
					string cahceKey = GetCacheKey(siteId, skinName, isCompressed);

					context.Cache.Insert(
						cahceKey,
						responseBytes,
						null,
						System.Web.Caching.Cache.NoAbsoluteExpiration,
						CACHE_DURATION
					);
				}
			}

			WriteBytes(responseBytes, context, isCompressed);
		}
	}


	private bool HasNoCacheCookie()
	{
		SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

		if (siteSettings != null)
		{
			string cookieName = SiteUtils.GetCssCacheCookieName(siteSettings);

			if (CookieHelper.CookieExists(cookieName))
			{
				return true;
			}
		}

		return false;
	}


	private bool ShouldCacheOnServer()
	{
		if (HasNoCacheCookie())
		{
			return false;
		}

		return AppConfig.CacheCssOnServer;
	}


	private bool ShouldCacheInBrowser()
	{
		if (HasNoCacheCookie())
		{
			return false;
		}

		return AppConfig.CacheCssInBrowser;
	}


	private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed)
	{
		if (bytes.Length == 0)
		{
			return;
		}

		HttpResponse response = context.Response;

		response.AppendHeader("Content-Length", bytes.Length.ToInvariantString());
		response.ContentType = responseContentType;

		if (isCompressed)
		{
			response.AppendHeader("Content-Encoding", "gzip");
		}

		if (ShouldCacheInBrowser())
		{
			context.Response.Cache.SetCacheability(HttpCacheability.Public);
			context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
			context.Response.Cache.SetMaxAge(CACHE_DURATION);
		}
		else
		{
			//if both cache settings are off it must be a designer so make it easy
			context.Response.Cache.SetExpires(new DateTime(1942, 12, 30, 0, 0, 0, DateTimeKind.Utc));
			context.Response.Cache.SetNoStore();
			context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
			context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
			context.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
		}

		response.OutputStream.Write(bytes, 0, bytes.Length);
	}

	private byte[] GetCss(HttpContext context, int siteId, string skinName, Encoding encoding)
	{

		var skinPath = Invariant($"~/Data/Sites/{siteId}/skins/{skinName}");
		string physicalSkinPath = HttpContext.Current.Server.MapPath(skinPath);
		string skinImageBasePath = skinPath.ToLinkBuilder().ToString();

		var cssContent = new StringBuilder();

		if (File.Exists($"{physicalSkinPath}/style.config"))
		{
			ProcessCssFileList(cssContent, physicalSkinPath, skinImageBasePath);
		}

		//2013-08-20 JA added this to support add on products on the demo site
		// so I can add css needed to demo the add on features without having to add/maintain it in every skin
		// whereas customers would typically add the css that ships with the feature to their skin and list it in style.config
		if (WebConfigSettings.GlobalAddOnStyleFolder.Length > 0)
		{
			physicalSkinPath = HttpContext.Current.Server.MapPath(WebConfigSettings.GlobalAddOnStyleFolder);

			if (File.Exists($"{physicalSkinPath}/style.config"))
			{
				skinImageBasePath = WebConfigSettings.GlobalAddOnStyleFolder.Replace("~/", "/").ToLinkBuilder().ToString();
				// not supported/needed in global add on css
				ProcessCssFileList(cssContent, physicalSkinPath, skinImageBasePath);
			}
		}

		return encoding.GetBytes(cssContent.ToString());
	}

	private void ProcessCssFileList(StringBuilder cssContent, string physicalSkinPath, string skinImageBasePath)
	{
		using XmlReader reader = new XmlTextReader(new StreamReader($"{physicalSkinPath}/style.config"));
		reader.MoveToContent();

		while (reader.Read())
		{
			if (("file" == reader.Name) && (reader.NodeType != XmlNodeType.EndElement))
			{
				// full virtual path option for things that don't move 
				string cssVPath = reader.GetAttribute("cssvpath");
				string imageBaseVPath = reader.GetAttribute("imagebasevpath");

				if ((!string.IsNullOrEmpty(cssVPath)) && (!string.IsNullOrEmpty(imageBaseVPath)))
				{
					string cssFilePath;

					if (cssVPath.StartsWith("/"))
					{
						cssFilePath = HttpContext.Current.Server.MapPath($"~{cssVPath}");
					}
					else
					{
						cssFilePath = HttpContext.Current.Server.MapPath(cssVPath);
					}

					if (File.Exists(cssFilePath))
					{
						var file = new FileInfo(cssFilePath);
						using StreamReader sr = file.OpenText();
						string fileContent = sr.ReadToEnd();

						string css = URL_REGEX.Replace(fileContent,
							new MatchEvaluator(
								delegate (Match m)
								{
									string imgPath = trimQuotes(m.Groups["path"].Value);

									// If paths are external or are a Data URL
									if (urlPathsToIgnore.Match(imgPath).Success)
									{
										return string.Format(urlReplacement, imgPath);
									}
									else
									{
										// Prefix the image path
										return string.Format(urlReplacement, $"{imageBaseVPath}/{imgPath}");
									}
								}
							)
						);

						cssContent.Append(css);
					}
				}
				else
				{
					string cssFile = reader.ReadElementContentAsString();

					if (File.Exists($"{physicalSkinPath}/{cssFile}"))
					{
						var file = new FileInfo($"{physicalSkinPath}/{cssFile}");

						using StreamReader sr = file.OpenText();
						string fileContent = sr.ReadToEnd();

						string css = URL_REGEX.Replace(fileContent,
							new MatchEvaluator(
								delegate (Match m)
								{
									string imgPath = trimQuotes(m.Groups["path"].Value);

									// If paths are external or are a Data URL
									if (urlPathsToIgnore.Match(imgPath).Success)
									{
										return string.Format(urlReplacement, imgPath);
									}
									else
									{
										// Prefix the image path
										return string.Format(urlReplacement, $"{skinImageBasePath}/{imgPath}");
									}
								}
							)
						);

						cssContent.Append(css);
					}
				}
			}
		}
	}


	private string trimQuotes(string url) => url.Trim(['"', '\'']);


	private bool WriteFromCache(HttpContext context, int siteId, string skinName, bool isCompressed)
	{
		if (!ShouldCacheOnServer() || context.Cache[GetCacheKey(siteId, skinName, isCompressed)] is not byte[] responseBytes || responseBytes.Length == 0)
		{
			return false;
		}

		WriteBytes(responseBytes, context, isCompressed);

		return true;
	}


	private bool CanGZip(HttpRequest request)
	{
		var acceptEncoding = request.Headers["Accept-Encoding"];
		if (!string.IsNullOrWhiteSpace(acceptEncoding) &&
			(acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"))
		)
		{
			return true;
		}

		return false;
	}

	private string GetCacheKey(int siteId, string skinName, bool isCompressed) => Invariant($"CssHandler.{siteId}{skinName}.{isCompressed}{skinVersion}");

	public bool IsReusable => false;
}