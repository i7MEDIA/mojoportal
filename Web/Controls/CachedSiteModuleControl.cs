using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Core.Extensions;
using mojoPortal.Web.Caching;

namespace mojoPortal.Web;

public class CachedSiteModuleControl : Control
{
	private string cachedOutput = null;
	private static readonly ILog log = LogManager.GetLogger(typeof(CachedSiteModuleControl));
	private bool debugLog = log.IsDebugEnabled;
	private string cacheKey = string.Empty;


	public Module ModuleConfiguration { get; set; }

	public int ModuleId
	{
		get { return ModuleConfiguration.ModuleId; }
	}

	public int PageId
	{
		get { return ModuleConfiguration.PageId; }
	}

	public int SiteId { get; set; } = 0;

	public string GetCacheKey()
	{
		bool canEdit = false;
		if (Page is mojoBasePage basePage)
		{
			canEdit = basePage.UserCanEditModule(ModuleConfiguration.ModuleId, ModuleConfiguration.FeatureGuid);
		}

		return $"Module-{ModuleConfiguration.ModuleId.ToInvariantString()}{canEdit.ToString(CultureInfo.InvariantCulture)}";
	}

	protected override void CreateChildControls()
	{
		cacheKey = GetCacheKey();
		if (!Page.IsPostBack && ModuleConfiguration.CacheTime > 0)
		{
			cachedOutput = CacheManager.Cache.GetObject(cacheKey) as string;
			if (debugLog)
			{
				if (cachedOutput == null)
				{
					log.Debug($"cached module content was null for cacheKey {cacheKey}");
				}
				else
				{
					log.Debug($"cached module content was found for cacheKey {cacheKey}");
				}
			}
		}

		if (Page.IsPostBack || cachedOutput is null)
		{
			base.CreateChildControls();

			SiteModuleControl module = (SiteModuleControl)Page.LoadControl(ModuleConfiguration.ControlSource);

			module.ModuleConfiguration = ModuleConfiguration;
			module.SiteId = SiteId;

			try
			{
				Controls.Add(module);
			}
			catch (HttpException ex)
			{
				// when searching using the search input from a page
				// with viewstate enabled, this exception can be thrown
				// because when the  SearchResults page accesses the Page.PreviousPage
				// the viewstate is not the same
				// the user never sees this and the search works since we catch
				// the exception
				if (log.IsErrorEnabled)
				{
					if (HttpContext.Current != null)
					{
						log.Error($"Exception caught and handled in CachedSiteModule for requested url:{HttpContext.Current.Request.RawUrl}", ex);
					}
					else
					{
						log.Error("Exception caught and handled in CachedSiteModule", ex);
					}
				}
			}
		}
	}

	protected override void Render(HtmlTextWriter output)
	{
		if (Page.IsPostBack || ModuleConfiguration.CacheTime == 0)
		{
			base.Render(output);
			return;
		}

		if (cacheKey.Length == 0) { return; }

		if (cachedOutput is null)
		{

			TextWriter tempWriter = new StringWriter(CultureInfo.InvariantCulture);
			base.Render(new HtmlTextWriter(tempWriter));
			cachedOutput = tempWriter.ToString();

			DateTime absoluteExpiration = DateTime.Now.AddSeconds(ModuleConfiguration.CacheTime);
			CacheManager.Cache.Add(cacheKey, absoluteExpiration, cachedOutput);
			if (debugLog)
			{
				log.Debug($"added module content to cache for cachekey {cacheKey}");
			}
		}
		output.Write(cachedOutput);
	}
}