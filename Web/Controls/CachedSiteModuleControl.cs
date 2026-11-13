#nullable enable
using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Caching;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;

namespace mojoPortal.Web;

public class CachedSiteModuleControl(Module module) : Control
{
	private static readonly ILog _log = LogManager.GetLogger(typeof(CachedSiteModuleControl));
	private readonly bool _debugLog = _log.IsDebugEnabled;

	private string? _cachedOutput = null;
	private string _cacheKey = string.Empty;


	public Module Module { get; } = module;
	public int ModuleId => Module.ModuleId;
	public int PageId => Module.PageId;
	public int SiteId { get; set; } = 0;


	public string GetCacheKey()
	{
		var canEdit = false;

		if (Page is mojoBasePage basePage)
		{
			canEdit = basePage.UserCanEditModule(Module.ModuleId, Module.FeatureGuid);
		}

		return $"Module-{Module.ModuleId.ToInvariantString()}{canEdit.ToString(CultureInfo.InvariantCulture)}";
	}


	protected override void CreateChildControls()
	{
		_cacheKey = GetCacheKey();

		if (!Page.IsPostBack && Module.CacheTime > 0)
		{
			_cachedOutput = CacheManager.Cache.GetObject(_cacheKey) as string;

			if (_debugLog)
			{
				if (_cachedOutput is null)
				{
					_log.Debug($"cached module content was null for cacheKey {_cacheKey}");
				}
				else
				{
					_log.Debug($"cached module content was found for cacheKey {_cacheKey}");
				}
			}
		}

		if (Page.IsPostBack || _cachedOutput is null)
		{
			base.CreateChildControls();

			var controlVirtualPath = "~/" + Module.ControlSource;

			if (!Plugins.PluginFileResolver.FileExists(controlVirtualPath))
			{
				_log.Warn($"Skipping cached module '{Module.ModuleTitle}' because control '{Module.ControlSource}' was not found (plugin may be unloaded or disabled).");
				return;
			}

			try
			{
				var module = Page.LoadControl(Plugins.PluginFileResolver.ResolveVirtualPath(controlVirtualPath));

				if (module is not SiteModuleControl siteModule)
				{
					return;
				}

				siteModule.ModuleConfiguration = Module;
				siteModule.SiteId = SiteId;

				Controls.Add(siteModule);
			}
			catch (HttpException ex)
			{
				// when searching using the search input from a page
				// with viewstate enabled, this exception can be thrown
				// because when the  SearchResults page accesses the Page.PreviousPage
				// the viewstate is not the same
				// the user never sees this and the search works since we catch
				// the exception
				if (_log.IsErrorEnabled)
				{
					if (HttpContext.Current != null)
					{
						_log.Error($"Exception caught and handled in CachedSiteModule for requested url:{HttpContext.Current.Request.RawUrl}", ex);
					}
					else
					{
						_log.Error("Exception caught and handled in CachedSiteModule", ex);
					}
				}
			}
		}
	}


	protected override void Render(HtmlTextWriter output)
	{
		if (Page.IsPostBack || Module.CacheTime == 0)
		{
			base.Render(output);

			return;
		}

		if (string.IsNullOrWhiteSpace(_cacheKey))
		{
			return;
		}

		if (_cachedOutput is null)
		{
			TextWriter tempWriter = new StringWriter(CultureInfo.InvariantCulture);

			base.Render(new HtmlTextWriter(tempWriter));

			_cachedOutput = tempWriter.ToString();

			var absoluteExpiration = DateTime.Now.AddSeconds(Module.CacheTime);

			CacheManager.Cache.Add(_cacheKey, absoluteExpiration, _cachedOutput);

			if (_debugLog)
			{
				_log.Debug($"added module content to cache for cachekey {_cacheKey}");
			}
		}

		output.Write(_cachedOutput);
	}
}
