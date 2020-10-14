/// Author:					    
/// Created:				    2007-08-30
/// Last Modified:			    2013-01-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Resources;
namespace mojoPortal.Features
{
    public class LinksIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LinksIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

        public LinksIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (pageSettings == null)
            {
                log.Error("LinksIndexBuilderProvider.RebuildIndex error: pageSettings was null ");
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info(LinkResources.FeatureName + " indexing page - " + pageSettings.PageName);

            try
            {
                Guid linksFeatureGuid = new Guid("74bdbcc2-0e79-47ff-bcd4-a159270bf36e");
                ModuleDefinition linksFeature = new ModuleDefinition(linksFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                DataTable dataTable = Link.GetLinksByPage(
                    pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.CreatedUtc = Convert.ToDateTime(row["CreatedDate"]);
                    indexItem.LastModUtc = indexItem.CreatedUtc;

                    if (pageSettings.UseUrl)
                    {
                        if (pageSettings.UrlHasBeenAdjustedForFolderSites)
                        {
                            indexItem.ViewPage = pageSettings.UnmodifiedUrl.Replace("~/", string.Empty);
                        }
                        else
                        {
                            indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                        }
                        indexItem.UseQueryStringParams = false;
                    }

                    indexItem.FeatureId = linksFeatureGuid.ToString();
                    indexItem.FeatureName = linksFeature.FeatureName;
                    indexItem.FeatureResourceFile = linksFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Title"].ToString();
                    indexItem.Content = row["Description"].ToString();
                    // lookup publish dates
                    foreach (PageModule pageModule in pageModules)
                    {
                        if (indexItem.ModuleId == pageModule.ModuleId)
                        {
                            indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                            indexItem.PublishEndDate = pageModule.PublishEndDate;
                        }
                    }

                    mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem, indexPath);

                    if (debugLog) log.Debug("Indexed " + indexItem.Title);

                }
            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error(ex);
            }


        }

        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            Link link = (Link)sender;
            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(link.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        link.ModuleId,
                        link.ItemId);
                }
            }
            else
            {
                IndexItem(link);
            }

        }

        private static void IndexItem(Link link)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("Link object passed to Links.IndexItem was null");
                }

                return;
            }

            if (link == null) return;

            Guid linksFeatureGuid = new Guid("74bdbcc2-0e79-47ff-bcd4-a159270bf36e");
            ModuleDefinition linksFeature = new ModuleDefinition(linksFeatureGuid);
            Module module = new Module(link.ModuleId);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(link.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    siteSettings.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                indexItem.SiteId = siteSettings.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                indexItem.FeatureId = linksFeatureGuid.ToString();
                indexItem.FeatureName = linksFeature.FeatureName;
                indexItem.FeatureResourceFile = linksFeature.ResourceFile;

                indexItem.ItemId = link.ItemId;
                indexItem.ModuleId = link.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = link.Title;
                indexItem.Content = link.Description;
                indexItem.CreatedUtc = link.CreatedDate;
                indexItem.LastModUtc = link.CreatedDate;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed " + link.Title);


        }

    }
}
