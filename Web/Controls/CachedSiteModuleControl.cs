/// Created:			2004-12-26
/// Last Modified:		2011-08-05

using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using mojoPortal.Web.Caching;

namespace mojoPortal.Web
{
	
	public class CachedSiteModuleControl : Control 
	{
		private Module  moduleConfiguration;
		private String  cachedOutput = null;
		private int     siteID = 0;
        private static readonly ILog log = LogManager.GetLogger(typeof(CachedSiteModuleControl));
        private bool debugLog = log.IsDebugEnabled;
        private string cacheKey = string.Empty;


        public Module ModuleConfiguration
        {
            get { return moduleConfiguration; }
            set { moduleConfiguration = value; }
        }

        public int ModuleId
        {
            get { return moduleConfiguration.ModuleId; }
        }

		public int PageId
		{
            get { return moduleConfiguration.PageId; }
		}

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

		public string GetCacheKey() 
		{
            //get 
            //{
            //    return "Module-"
            //        + CultureInfo.CurrentUICulture.Name
            //        + moduleConfiguration.ModuleId.ToInvariantString()
            //        + HttpContext.Current.Request.QueryString
            //        + WebUser.IsInRoles("Admins;Content Administrators;" + moduleConfiguration.AuthorizedEditRoles);
            //}

            bool canEdit = false;
            mojoBasePage basePage = Page as mojoBasePage;
            if (basePage != null)
            {
                canEdit = basePage.UserCanEditModule(moduleConfiguration.ModuleId, moduleConfiguration.FeatureGuid);
            }

            return "Module-" + moduleConfiguration.ModuleId.ToInvariantString()
                + canEdit.ToString(CultureInfo.InvariantCulture);

		}

        //public String CacheDependencyKey
        //{
        //    get
        //    {
        //        return "Module-" + moduleConfiguration.ModuleId.ToInvariantString();
        //    }
        //}

		

		protected override void CreateChildControls()
		{
            cacheKey = GetCacheKey();
			if (
                (!Page.IsPostBack)
                &&(moduleConfiguration.CacheTime > 0)
                )
			{
                //cachedOutput = (string)HttpRuntime.Cache[cacheKey];
                cachedOutput = CacheManager.Cache.GetObject(cacheKey) as string;
                if (debugLog)
                {
                    if (cachedOutput == null)
                    {
                        log.Debug("cached module content was null for cacheKey " + cacheKey);
                    }
                    else
                    {
                        log.Debug("cached module content was found for cacheKey " + cacheKey);
                    }
                }
			}

			if ((Page.IsPostBack)||(cachedOutput == null))
			{

				base.CreateChildControls();

				SiteModuleControl module = (SiteModuleControl) Page.LoadControl(moduleConfiguration.ControlSource);
                
				module.ModuleConfiguration = this.ModuleConfiguration;
				module.SiteId = this.SiteId;

                try
                {
                    this.Controls.Add(module);
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
                            log.Error("Exception caught and handled in CachedSiteModule for requested url:" 
                                + HttpContext.Current.Request.RawUrl, ex);
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
			if (
                (Page.IsPostBack)
                ||(moduleConfiguration.CacheTime == 0)
                )
			{
				base.Render(output);
				return;
			}

            if (cacheKey.Length == 0) { return; }

			if (cachedOutput == null)
			{

                TextWriter tempWriter = new StringWriter(CultureInfo.InvariantCulture);
				base.Render(new HtmlTextWriter(tempWriter));
				cachedOutput = tempWriter.ToString();

                DateTime absoluteExpiration = DateTime.Now.AddSeconds(moduleConfiguration.CacheTime);
                CacheManager.Cache.Add(cacheKey, absoluteExpiration, cachedOutput);
                if (debugLog) { log.Debug("added module content to cache for cachekey " + cacheKey); }

                //CacheItemPriority priority = CacheItemPriority.Default;
                //CacheItemRemovedCallback callback = null;

                //HttpRuntime.Cache.Insert(
                //    cacheKey,
                //    cachedOutput,
                //    null,
                //    absoluteExpiration,
                //    Cache.NoSlidingExpiration,
                //    priority,
                //    callback);

			}

			output.Write(cachedOutput);
		}
	}

	


}
