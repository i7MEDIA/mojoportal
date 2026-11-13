#nullable enable
using log4net;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Security;
using System.Web;
using System.Web.Caching;

namespace mojoPortal.Web;

public class WebPageInfo
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(WebPageInfo));
	private readonly FileInfo _webPageFile;


	public WebPageInfo(FileInfo webPageFile)
	{
		_webPageFile = webPageFile;
	}


	public bool Equals(string url)
	{
		var result = false;

		if (HttpContext.Current is not null)
		{
			try
			{
				var resolvedUrl = Plugins.PluginFileResolver.ResolveVirtualPath(url);

				if (HttpContext.Current.Server.MapPath(resolvedUrl).IsCaseInsensitiveMatch(_webPageFile.FullName) ||
					HttpContext.Current.Server.MapPath(url).IsCaseInsensitiveMatch(_webPageFile.FullName))
				{
					result = true;
				}
			}
			catch (HttpException) { }
		}

		return result;
	}


	public static Collection<WebPageInfo>? GetPhysicalPages()
	{
		return GetPhysicalPages("*.aspx");
	}

	public static Collection<WebPageInfo>? GetPhysicalPages(string fileExtensionPattern)
	{
		string cachekey = "physicalwebpages" + fileExtensionPattern;

		Collection<WebPageInfo>? physicalPages;

		if (HttpContext.Current is not null && HttpRuntime.Cache[cachekey] is null)
		{
			_log.Debug("couldn't find cache item " + cachekey + " creating cache item now.");

			physicalPages = LoadPhysicalPages(fileExtensionPattern);

			var aggregateCacheDependency = new AggregateCacheDependency();

			aggregateCacheDependency.Add(new CacheDependency(HttpContext.Current.Server.MapPath("~/Web.config")));

			var absoluteExpiration = DateTime.Now.AddMinutes(WebConfigSettings.WebPageInfoCacheMinutes);
			var slidingExpiration = TimeSpan.Zero;
			var priority = CacheItemPriority.Default;
			CacheItemRemovedCallback? callback = null;

			HttpRuntime.Cache.Insert(
				cachekey,
				physicalPages,
				aggregateCacheDependency,
				absoluteExpiration,
				slidingExpiration,
				priority,
				callback);

		}

		physicalPages = HttpRuntime.Cache[cachekey] as Collection<WebPageInfo>;

		return physicalPages;
	}


	private static Collection<WebPageInfo> LoadPhysicalPages(string fileExtensionPattern)
	{
		Collection<WebPageInfo> physicalPages = [];

		if (HttpContext.Current is not null)
		{
			try
			{
				PopulatePagesCollection(
					HttpContext.Current,
					physicalPages,
					fileExtensionPattern);
			}
			catch (UnauthorizedAccessException) { }
			catch (SecurityException) { }
		}

		return physicalPages;
	}


	private static void PopulatePagesCollection(
		HttpContext httpContext,
		Collection<WebPageInfo> webPages,
		string fileExtensionPattern)
	{
		if (httpContext is null)
		{
			return;
		}

		var siteBasePath = httpContext.Server.MapPath("~/");
		var rootDirectory = new DirectoryInfo(siteBasePath);

		AddPhysicalPages(webPages, rootDirectory, fileExtensionPattern);

		try
		{
			//  recurse through sub folders
			// how deep is too deep?
			// I don't think it should be endless
			// start with 4 levels which should satify most use cases
			// while minimizing performance issues
			var firstLevelSubDirectories = rootDirectory.GetDirectories();

			foreach (var firstLevelSubDirectory in firstLevelSubDirectories)
			{
				try
				{
					AddPhysicalPages(webPages, firstLevelSubDirectory, fileExtensionPattern);
				}
				catch (UnauthorizedAccessException) { }
				catch (SecurityException) { }
				// catch these errors because there can be other folders in the root that are not part of mojoportal
				// don't fail to read the folders we can read just because we can't read others
				//http://www.mojoportal.com/ForumThreadView.aspx?thread=1875&forumid=2&ItemID=2&pageid=5&pagenumber=1#post7654
			}

			foreach (DirectoryInfo firstLevelSubDirectory in firstLevelSubDirectories)
			{
				try
				{
					var secondLevelSubDirectories = firstLevelSubDirectory.GetDirectories();

					foreach (var secondLevelSubDirectory in secondLevelSubDirectories)
					{
						try
						{
							AddPhysicalPages(webPages, secondLevelSubDirectory, fileExtensionPattern);
						}
						catch (UnauthorizedAccessException) { }
						catch (SecurityException) { }
					}

					foreach (var secondLevelSubDirectory in secondLevelSubDirectories)
					{
						try
						{
							var thirdLevelSubDirectories = secondLevelSubDirectory.GetDirectories();

							foreach (DirectoryInfo thirdLevelSubDirectory in thirdLevelSubDirectories)
							{
								try
								{
									AddPhysicalPages(webPages, thirdLevelSubDirectory, fileExtensionPattern);
								}
								catch (UnauthorizedAccessException) { }
								catch (SecurityException) { }
							}

							foreach (var thirdLevelSubDirectory in thirdLevelSubDirectories)
							{
								try
								{
									var fourthLevelSubDirectories = thirdLevelSubDirectory.GetDirectories();

									foreach (var fourthLevelSubDirectory in fourthLevelSubDirectories)
									{
										try
										{
											AddPhysicalPages(webPages, fourthLevelSubDirectory, fileExtensionPattern);
										}
										catch (UnauthorizedAccessException) { }
										catch (SecurityException) { }
									}
								}
								catch (UnauthorizedAccessException) { }
								catch (SecurityException) { }
							}
						}
						catch (UnauthorizedAccessException) { }
						catch (SecurityException) { }
					}
				}
				catch (UnauthorizedAccessException) { }
				catch (SecurityException) { }
			}

		}
		catch (UnauthorizedAccessException) { }
		catch (SecurityException) { }
		// catch these errors because there can be other folders in the root that are not part of mojoportal
		// don't fail to read the folders we can read just because we can't read others
	}


	private static void AddPhysicalPages(
		Collection<WebPageInfo> physicalPages,
		DirectoryInfo directoryInfo,
		string fileExtensionPattern)
	{
		var matchingFiles = directoryInfo.GetFiles(fileExtensionPattern);

		foreach (var matchingFile in matchingFiles)
		{
			var webPageInfo = new WebPageInfo(matchingFile);

			physicalPages.Add(webPageInfo);
		}
	}


	public static bool IsPhysicalWebPage(string url)
	{
		var result = false;
		var physicalPages = GetPhysicalPages();

		foreach (var webPage in physicalPages ?? [])
		{
			if (webPage.Equals(url))
			{
				result = true;
			}
		}

		return result;
	}
}
