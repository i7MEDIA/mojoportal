/// Author:					    
/// Created:				    2007-08-30
/// Last Modified:			    2013-10-10
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.SearchIndex;

namespace mojoPortal.Features
{
    public class SharedFilesIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedFilesIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

        public SharedFilesIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if ((pageSettings == null) || (indexPath == null))
            {
                return;
            }

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info(Resources.SharedFileResources.SharedFilesFeatureName + " indexing page - " + pageSettings.PageName);

            try
            {
                Guid sharedFilesFeatureGuid = new Guid("dc873d76-5bf2-4ac5-bff7-434a87a3fc8e");
                ModuleDefinition sharedFilesFeature = new ModuleDefinition(sharedFilesFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                DataTable dataTable = SharedFile.GetSharedFilesByPage(pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.CreatedUtc = Convert.ToDateTime(row["UploadDate"]);
                    indexItem.LastModUtc = indexItem.CreatedUtc;

                    //if (pageSettings.UseUrl)
                    //{
                    //    if (pageSettings.UrlHasBeenAdjustedForFolderSites)
                    //    {
                    //        indexItem.ViewPage = pageSettings.UnmodifiedUrl.Replace("~/", string.Empty);
                    //    }
                    //    else
                    //    {
                    //        indexItem.ViewPage = pageSettings.Url.Replace("~/", string.Empty);
                    //    }

                    //    indexItem.UseQueryStringParams = false;
                    //}

                    indexItem.FeatureId = sharedFilesFeatureGuid.ToString();
                    indexItem.FeatureName = sharedFilesFeature.FeatureName;
                    indexItem.FeatureResourceFile = sharedFilesFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"]);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"]);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();

                    indexItem.Title = row["FriendlyName"].ToString().Replace("_", " ").Replace("-", " ").Replace(".", " ");
                    indexItem.Content = row["Description"].ToString();

                    // make the search results a download link
                    indexItem.ViewPage = "SharedFiles/Download.aspx?pageid=" + indexItem.PageId.ToInvariantString()
                        + "&fileid=" + indexItem.ItemId.ToInvariantString()
                        + "&mid=" + indexItem.ModuleId.ToInvariantString();

                    indexItem.UseQueryStringParams = false;

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

            SharedFile sharedFile = (SharedFile)sender;
            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(sharedFile.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        sharedFile.ModuleId,
                        sharedFile.ItemId);
                }
            }
            else
            {
                IndexItem(sharedFile);
            }

        }

        private static void IndexItem(SharedFile sharedFile)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (
                (sharedFile == null)
                || (siteSettings == null)
                )
            {
                return;
            }

            Guid sharedFilesFeatureGuid = new Guid("dc873d76-5bf2-4ac5-bff7-434a87a3fc8e");
            ModuleDefinition sharedFilesFeature = new ModuleDefinition(sharedFilesFeatureGuid);

            Module module = new Module(sharedFile.ModuleId);

            // get list of pages where this module is published
            List<PageModule> pageModules
                = PageModule.GetPageModulesByModule(sharedFile.ModuleId);

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
                indexItem.FeatureId = sharedFilesFeatureGuid.ToString();
                indexItem.FeatureName = sharedFilesFeature.FeatureName;
                indexItem.FeatureResourceFile = sharedFilesFeature.ResourceFile;
                indexItem.CreatedUtc = sharedFile.UploadDate;
                indexItem.LastModUtc = sharedFile.UploadDate;

                indexItem.ItemId = sharedFile.ItemId;
                indexItem.ModuleId = sharedFile.ModuleId;
                indexItem.ModuleTitle = module.ModuleTitle;
                indexItem.Title = sharedFile.FriendlyName.Replace("_", " ").Replace("-", " ").Replace(".", " ") ;
                indexItem.Content = sharedFile.Description;
                indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                indexItem.PublishEndDate = pageModule.PublishEndDate;
                // make the search results a download link
                indexItem.ViewPage = "SharedFiles/Download.aspx?pageid=" + indexItem.PageId.ToInvariantString()
                    + "&fileid=" + indexItem.ItemId.ToInvariantString()
                    + "&mid=" + indexItem.ModuleId.ToInvariantString();
                indexItem.UseQueryStringParams = false;

                mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed "  + sharedFile.FriendlyName);
            
        }

    }
}
