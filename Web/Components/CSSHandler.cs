//		Author:
//		Created:		2008-10-31
//		Last Modified:	2017-08-08
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

//using dotless.Core;
//using dotless.Core.configuration;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;


namespace mojoPortal.Web.UI
{
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
		private readonly static TimeSpan CACHE_DURATION = TimeSpan.FromDays(WebConfigSettings.CssCacheInDays);

		private string skinVersion = string.Empty;

		//private readonly static Regex URL_REGEX = new Regex(@"url\((\""|\')?(?<path>(.*))?(\""|\')?\)", RegexOptions.Compiled);

		// 2014-12-09 URL_REGEX changed from the above per Todd Hansen to fix multi url on a line problem
		// https://www.mojoportal.com/Forums/Thread.aspx?pageid=5&t=12924~1#post53334

		private readonly static Regex URL_REGEX = new Regex(@"url\((\""|\')?(?<path>(.*?))?(\""|\')?\)", RegexOptions.Compiled);

		// We don't want to append "/Data/Sites/[site number]/skins/[skin folder]" to external links
		// so we check all "url()" links if they start with "http://", "https://", "data:", and "//" to ignore them if they do
		private readonly static Regex urlPathsToIgnore = new Regex(@"^(https?://|data:|//)");

		// 1 string to keep things DRY
		private string urlReplacement = "url(\"{0}\")";

		private StringBuilder less = null;

		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/css";

			if (context.Request.RequestType == POST)
			{
				return;
			}

			bool isCompressed = DO_GZIP && CanGZip(context.Request);

			int siteId = SiteUtils.ParseSiteIdFromSkinRequestUrl();

			string skinName = "framework";

			if (context.Request["skin"] != null)
			{
				skinName = SiteUtils.SanitizeSkinParam(context.Request["skin"]);
			}

			string media = "screen";

			if (context.Request["media"] != null)
			{
				media = context.Request["media"];
			}

			skinVersion = WebUtils.ParseGuidFromQueryString("sv", Guid.Empty).ToString();

			UTF8Encoding encoding = new UTF8Encoding(false);

			if (!WriteFromCache(context, siteId, skinName, isCompressed))
			{
				byte[] cssBytes = GetCss(context, siteId, skinName, encoding);

				using (MemoryStream memoryStream = new MemoryStream(5000))
				{
					using (Stream writer = isCompressed ?
						(Stream)(new GZipStream(memoryStream, CompressionMode.Compress)) :
						memoryStream)
					{
						writer.Write(cssBytes, 0, cssBytes.Length);
					}

					byte[] responseBytes = memoryStream.ToArray();

					if (ShouldCacheOnServer())
					{
						// TODO: maybe we should cache it to disk instead of memory
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

			return WebConfigSettings.CacheCssOnServer;
		}

		private bool ShouldCacheInBrowser()
		{
			if (HasNoCacheCookie())
			{
				return false;
			}

			return WebConfigSettings.CacheCssInBrowser;
		}

		private void WriteBytes(byte[] bytes, HttpContext context, bool isCompressed)
		{
			if (bytes.Length == 0)
			{
				return;
			}

			HttpResponse response = context.Response;

			response.AppendHeader("Content-Length", bytes.Length.ToInvariantString());
			response.ContentType = "text/css";

			if (isCompressed)
			{
				response.AppendHeader("Content-Encoding", "gzip");
			}

			bool isIE6 = BrowserHelper.IsIE6();

			if (ShouldCacheInBrowser())
			{
				context.Response.Cache.SetCacheability(HttpCacheability.Public);
				context.Response.Cache.SetExpires(DateTime.Now.Add(CACHE_DURATION));
				context.Response.Cache.SetMaxAge(CACHE_DURATION);

				if (!isIE6)
				{
					context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
				}
			}
			else
			{
				if (!ShouldCacheOnServer() && !isIE6)
				{
					//if both cache settings are off it must be a designer so make it easy
					context.Response.Cache.SetExpires(new DateTime(1995, 5, 6, 12, 0, 0, DateTimeKind.Utc));
					context.Response.Cache.SetNoStore();
					context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
					context.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
					context.Response.Cache.AppendCacheExtension("post-check=0,pre-check=0");
				}
			}

			response.OutputStream.Write(bytes, 0, bytes.Length);
		}

		private byte[] GetCss(HttpContext context, int siteId, string skinName, Encoding encoding)
		{
			string basePath = HttpContext.Current.Server.MapPath(
				"~/Data/Sites/" +
				siteId.ToInvariantString() +
				"/skins/" +
				skinName +
				"/"
			);

			string siteRoot = string.Empty;

			if (WebConfigSettings.UseFullUrlsForSkins)
			{
				siteRoot = WebUtils.GetSiteRoot();
			}
			else
			{
				siteRoot = WebUtils.GetRelativeSiteRoot();
			}

			string skinImageBasePath = 
				siteRoot +
				"/Data/Sites/" +
				siteId.ToInvariantString() +
				"/skins/" +
				skinName +
				"/"
			;

			StringBuilder cssContent = new StringBuilder();
			bool hasLessFiles = false;

			if (File.Exists(basePath + "style.config"))
			{
				ProcessCssFileList(cssContent, basePath, siteRoot, skinImageBasePath, out hasLessFiles);
			}

			//2013-08-20 JA added this to support add on products on the demo site
			// so I can add css needed to demo the add on features without having to add/maintain it in every skin
			// whereas customers would typically add the css that ships with the feature to their skin and list it in style.config
			if (WebConfigSettings.GlobalAddOnStyleFolder.Length > 0)
			{
				basePath = HttpContext.Current.Server.MapPath(WebConfigSettings.GlobalAddOnStyleFolder);

				if (File.Exists(basePath + "style.config"))
				{
					skinImageBasePath = siteRoot + WebConfigSettings.GlobalAddOnStyleFolder.Replace("~/", "/");
					bool globalHasLess = false; // not supported/needed in global add on css
					ProcessCssFileList(cssContent, basePath, siteRoot, skinImageBasePath, out globalHasLess);
				}
			}


			if (hasLessFiles && (less != null))
			{
				log.Error($"LESS parser has been removed from mojoPortal. Compile your LESS files to CSS using another tool (i.e., prepros) and then reference the CSS files in your skin style.config");
			}

			if (ShouldCacheOnServer() && WebConfigSettings.MinifyCSS)
			{
				// this method is expensive (7.87 seconds as measured by ANTS Profiler
				// we do cache so its not called very often
				return encoding.GetBytes(CssMinify.Minify(cssContent.ToString()));
			}

			return encoding.GetBytes(cssContent.ToString());
		}

		private void ProcessCssFileList(StringBuilder cssContent, string basePath, string siteRoot, string skinImageBasePath, out bool hasLessFiles)
		{
			hasLessFiles = false;

			using (XmlReader reader = new XmlTextReader(new StreamReader(basePath + "style.config")))
			{
				reader.MoveToContent();

				while (reader.Read())
				{
					if (("file" == reader.Name) && (reader.NodeType != XmlNodeType.EndElement))
					{
						// config based css for things like YUI where the folder changes per version
						string csswebconfigkey = reader.GetAttribute("csswebconfigkey");
						string imagebasewebconfigkey = reader.GetAttribute("imagebasewebconfigkey");

						if (!string.IsNullOrEmpty(csswebconfigkey) && csswebconfigkey.Contains("YUI"))
						{
							csswebconfigkey = string.Empty;
						}

						if (WebConfigSettings.UseGoogleCDN && !string.IsNullOrEmpty(csswebconfigkey))
						{
							if (csswebconfigkey.Contains("YUI"))
							{
								csswebconfigkey = string.Empty;
							}
						}

						// full virtual path option for things that don't move 
						string cssVPath = reader.GetAttribute("cssvpath");
						string imageBaseVPath = reader.GetAttribute("imagebasevpath");

						if (
							!string.IsNullOrEmpty(csswebconfigkey) &&
							!string.IsNullOrEmpty(imagebasewebconfigkey)
						)
						{
							if (
								ConfigurationManager.AppSettings[csswebconfigkey] != null && 
								ConfigurationManager.AppSettings[imagebasewebconfigkey] != null
							)
							{

								string cssFullPath = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings[csswebconfigkey]);
								string imageBasePath = ConfigurationManager.AppSettings[imagebasewebconfigkey];

								if (File.Exists(cssFullPath))
								{
									FileInfo file = new FileInfo(cssFullPath);

									if (file.Extension == ".less")
									{
										hasLessFiles = true;
									}

									using (StreamReader sr = file.OpenText())
									{
										string fileContent = sr.ReadToEnd();

										string css = URL_REGEX.Replace(
											fileContent,
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
														return string.Format(urlReplacement, siteRoot + imageBasePath + imgPath);
													}
												}
											)
										);

										if (file.Extension == ".less")
										{
											AddToLess(css);
										}
										else
										{
											cssContent.Append(css);
										}
									}
								}
							}
						}
						else if ((!string.IsNullOrEmpty(cssVPath)) && (!string.IsNullOrEmpty(imageBaseVPath)))
						{
							string cssFilePath;

							if (cssVPath.StartsWith("/"))
							{
								cssFilePath = HttpContext.Current.Server.MapPath("~" + cssVPath);
							}
							else
							{
								cssFilePath = HttpContext.Current.Server.MapPath(cssVPath);
							}

							if (File.Exists(cssFilePath))
							{
								FileInfo file = new FileInfo(cssFilePath);
								if (file.Extension == ".less") { hasLessFiles = true; }
								using (StreamReader sr = file.OpenText())
								{
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
													return string.Format(urlReplacement, siteRoot + imageBaseVPath + imgPath);
												}
											}
										)
									);

									if (file.Extension == ".less")
									{
										AddToLess(css);
									}
									else
									{
										cssContent.Append(css);
									}
								}
							}
						}
						else
						{
							string cssFile = reader.ReadElementContentAsString();

							if (File.Exists(basePath + cssFile))
							{
								FileInfo file = new FileInfo(basePath + cssFile);

								if (file.Extension == ".less")
								{
									hasLessFiles = true;
								}

								using (StreamReader sr = file.OpenText())
								{
									string fileContent = sr.ReadToEnd();

									if (cssFile == "style.css")
									{
										fileContent = fileContent.Replace(")", ")\n");
										fileContent = fileContent.Replace("form.art-search", "div.art-search");
									}

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
													return string.Format(urlReplacement, skinImageBasePath + imgPath);
												}
											}
										)
									);

									if (file.Extension == ".less")
									{
										AddToLess(css);
									}
									else
									{
										cssContent.Append(css);
									}
								}
							}
						}
					}
				}
			}
		}

		// Added to keep things DRY
		private string trimQuotes(string url)
		{
			char[] quotes = {'"', '\''};

			return url.Trim(quotes);
		}

		private void EnsureLessBuilder()
		{
			if (less == null)
			{
				less = new StringBuilder();
			}
		}

		private void AddToLess(string lessContent)
		{
			EnsureLessBuilder();

			less.Append(lessContent);
		}


		private bool WriteFromCache(HttpContext context, int siteId, string skinName, bool isCompressed)
		{
			if (!ShouldCacheOnServer()) { return false; }

			byte[] responseBytes = context.Cache[GetCacheKey(siteId, skinName, isCompressed)] as byte[];

			if (null == responseBytes) { return false; }
			if (responseBytes.Length == 0) { return false; }

			WriteBytes(responseBytes, context, isCompressed);
			return true;
		}

		private bool CanGZip(HttpRequest request)
		{
			string acceptEncoding = request.Headers["Accept-Encoding"];
			if (
				!string.IsNullOrEmpty(acceptEncoding) &&
				(acceptEncoding.Contains("gzip") || acceptEncoding.Contains("deflate"))
			)
			{
				return true;
			}
			return false;
		}

		private string GetCacheKey(int siteId, string skinName, bool isCompressed)
		{
			return "CssHandler." + siteId.ToInvariantString() + skinName + "." + isCompressed + skinVersion;
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}