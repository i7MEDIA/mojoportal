// Author:					    Joe Audette
// Created:				        2008-12-11
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
    public class ProductSearchIndexBuilder : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSearchIndexBuilder));
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
                log.Error("pageSettings object passed to ProductSearchIndexBuilder.RebuildIndex was null");
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
                    = Product.GetBySitePage(
                    pageSettings.SiteId,
                    pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.FeatureId = webStoreFeatureGuid.ToString();
                    indexItem.FeatureName = webStoreFeature.FeatureName;
                    indexItem.FeatureResourceFile = webStoreFeature.ResourceFile;

                    indexItem.ItemKey = row["Guid"].ToString();
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Name"].ToString();
                    indexItem.ViewPage = row["Url"].ToString().Replace("/", string.Empty);
                    if (indexItem.ViewPage.Length > 0)
                    {
                        indexItem.UseQueryStringParams = false;
                    }
                    else
                    {
                        indexItem.ViewPage = "WebStore/ProductDetail.aspx";
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
            if (!(sender is Product)) return;

            Product product = sender as Product;
            
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            product.SiteId = siteSettings.SiteId;
            product.SearchIndexPath = mojoPortal.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                Store store = new Store(product.StoreGuid);
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(store.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        store.ModuleId,
                        product.Guid.ToString());
                }
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), product))
                {
                    if (debugLog) log.Debug("ProductSearchIndexBuilder.IndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for ProductSearchIndexBuilder.IndexItem");
                }

            }

        }

        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is Product)) return;

            Product content = o as Product;
            IndexItem(content);

        }


        private static void IndexItem(Product product)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (product == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("product object passed to ProductSearchIndexBuilder.IndexItem was null");
                }
                return;
            }

            Store store = new Store(product.StoreGuid);

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
                    product.SiteId,
                    pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

                mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                if (product.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = product.SearchIndexPath;
                }
                indexItem.SiteId = product.SiteId;
                indexItem.PageId = pageSettings.PageId;
                indexItem.PageName = pageSettings.PageName;
                indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                indexItem.ModuleViewRoles = module.ViewRoles;
                if (product.Url.Length > 0)
                {
                    indexItem.ViewPage = product.Url.Replace("~/", string.Empty);
                    indexItem.UseQueryStringParams = false;
                }
                else
                {
                    indexItem.ViewPage = "/WebStore/ProductDetail.aspx";
                }
                indexItem.ItemKey = product.Guid.ToString();
                indexItem.ModuleId = store.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = product.Name;
                indexItem.PageMetaDescription = product.MetaDescription;
                indexItem.PageMetaKeywords = product.MetaKeywords;

                indexItem.CreatedUtc = product.Created;
                indexItem.LastModUtc = product.LastModified;

                indexItem.Content 
                    = product.Teaser 
                    + " " + product.Description.Replace(tabScript, string.Empty) 
                    + " " + product.MetaDescription 
                    + " " + product.MetaKeywords;

                indexItem.FeatureId = webStoreFeatureGuid.ToString();
                indexItem.FeatureName = webStoreFeature.FeatureName;
                indexItem.FeatureResourceFile = webStoreFeature.ResourceFile;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;

                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed " + product.Name);

        }


    }
}
