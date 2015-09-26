// Author:					    Joe Audette
// Created:				        2010-05-31
// Last Modified:			    2013-01-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// 

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using WebStore.Business;

namespace WebStore.UI.Helpers
{
    /// <summary>
    ///Indexes the products into the search index.
    /// </summary>
    public class OfferSearchIndexBuilder : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OfferSearchIndexBuilder));
        private static bool debugLog = log.IsDebugEnabled;

        private const string tabScript = "<script type=\"text/javascript\" > var myTabs = new YAHOO.widget.TabView(\"productdetails\"); </script>";

        /// <summary>
        /// This method is called when the site index is rebuilt
        /// </summary>
        /// <param name="pageSettings"></param>
        /// <param name="indexPath"></param>
        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (pageSettings == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("pageSettings object passed to OfferSearchIndexBuilder.RebuildIndex was null");
                }
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info("ProductSearchIndexBuilder indexing page - "
                + pageSettings.PageName);


            Guid webStoreFeatureGuid = new Guid("0cefbf18-56de-11dc-8f36-bac755d89593");
            ModuleDefinition webStoreFeature = new ModuleDefinition(webStoreFeatureGuid);

            List<PageModule> pageModules
                    = PageModule.GetPageModulesByPage(pageSettings.PageId);

            // adding a try catch here because this is invoked even for non-implemented db platforms and it causes an error
            try
            {
                DataTable dataTable
                    = Offer.GetBySitePage(
                    pageSettings.SiteId,
                    pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);

                    Hashtable moduleSettings = ModuleSettings.GetModuleSettings(indexItem.ModuleId);
                    WebStoreConfiguration config = new WebStoreConfiguration();
                    if (moduleSettings != null)
                    {
                        config = new WebStoreConfiguration(moduleSettings);
                    }

                    if (!config.IndexOffersInSearch) { continue; }

                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.FeatureId = webStoreFeatureGuid.ToString();
                    indexItem.FeatureName = webStoreFeature.FeatureName;
                    indexItem.FeatureResourceFile = webStoreFeature.ResourceFile;

                    indexItem.ItemKey = row["Guid"].ToString();
                    
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Name"].ToString();
                    indexItem.ViewPage = row["Url"].ToString().Replace("/", string.Empty);

                    if (indexItem.ViewPage.Length > 0)
                    {
                        indexItem.UseQueryStringParams = false;
                    }
                    else
                    {
                        indexItem.ViewPage = "WebStore/OfferDetail.aspx";
                    }

                    indexItem.PageMetaDescription = row["MetaDescription"].ToString();
                    indexItem.PageMetaKeywords = row["MetaKeywords"].ToString();

                    indexItem.CreatedUtc = Convert.ToDateTime(row["Created"]);
                    indexItem.LastModUtc = Convert.ToDateTime(row["LastModified"]);

                    indexItem.Content = row["Abstract"].ToString()
                        + " " + row["Description"].ToString()
                        + " " + row["MetaDescription"].ToString()
                        + " " + row["MetaKeywords"].ToString();


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
            catch { }



        }


        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is Offer)) return;

            Offer offer = sender as Offer;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            offer.SiteId = siteSettings.SiteId;
            offer.SearchIndexPath = mojoPortal.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                Store store = new Store(offer.StoreGuid);
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(store.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        store.ModuleId,
                        offer.Guid.ToString());
                }
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), offer))
                {
                    if (debugLog) log.Debug("OfferSearchIndexBuilder.IndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for OfferSearchIndexBuilder.IndexItem");
                }

            }

        }

        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is Offer)) return;

            Offer content = o as Offer;
            IndexItem(content);

        }


        private static void IndexItem(Offer offer)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (offer == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("product object passed to OfferSearchIndexBuilder.IndexItem was null");
                }
                return;
            }

            Store store = new Store(offer.StoreGuid);

            Module module = new Module(store.ModuleId);
            Guid webStoreFeatureGuid = new Guid("0cefbf18-56de-11dc-8f36-bac755d89593");
            ModuleDefinition webStoreFeature = new ModuleDefinition(webStoreFeatureGuid);

            //// get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(store.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                PageSettings pageSettings
                    = new PageSettings(
                    offer.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                if (offer.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = offer.SearchIndexPath;
                }
                indexItem.SiteId = offer.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                if (offer.Url.Length > 0)
                {
                    indexItem.ViewPage = offer.Url.Replace("~/", string.Empty);
                    indexItem.UseQueryStringParams = false;
                }
                else
                {
                    indexItem.ViewPage = "/WebStore/OfferDetail.aspx";
                }
                indexItem.ItemKey = offer.Guid.ToString();
                indexItem.ModuleId = store.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = offer.Name;
                indexItem.PageMetaDescription = offer.MetaDescription;
                indexItem.PageMetaKeywords = offer.MetaKeywords;

                indexItem.CreatedUtc = offer.Created;
                indexItem.LastModUtc = offer.LastModified;

                indexItem.Content
                    = offer.Teaser
                    + " " + offer.Description.Replace(tabScript, string.Empty)
                    + " " + offer.MetaDescription
                    + " " + offer.MetaKeywords;

                indexItem.FeatureId = webStoreFeatureGuid.ToString();
                indexItem.FeatureName = webStoreFeature.FeatureName;
                indexItem.FeatureResourceFile = webStoreFeature.ResourceFile;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed " + offer.Name);

        }


    }
}
