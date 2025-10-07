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
            if (pageSettings.IsPending) 
            { 
                return; 
            }

			var pageModules = PageModule.GetPageModules(pageSettings.PageId, SharedFile.FeatureGuid);
			//only index pages with this feature
			if (pageModules.Count == 0)
			{
				return;
			}


			log.Info($"{Resources.SharedFileResources.SharedFilesFeatureName} indexing page - {pageSettings.PageName}");

            try
            {
                ModuleDefinition sharedFilesFeature = new ModuleDefinition(SharedFile.FeatureGuid);


                DataTable dataTable = SharedFile.GetSharedFilesByPage(pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
					IndexItem indexItem = new IndexItem();
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

                    indexItem.FeatureId = SharedFile.FeatureGuid.ToString();
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

					IndexHelper.RebuildIndex(indexItem, indexPath);

                    if (debugLog) log.Debug("Indexed " + indexItem.Title);

                }
            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error(ex);
            }
        }

        public override void ContentChangedHandler(object sender, ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            SharedFile sharedFile = (SharedFile)sender;
            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                var pageModules = PageModule.GetPageModulesByModule(sharedFile.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
					IndexHelper.RemoveIndexItem(
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
            if (WebConfigSettings.DisableSearchIndex) 
            { 
                return; 
            }

            if ((sharedFile is null) || (CacheHelper.GetCurrentSiteSettings() is not SiteSettings siteSettings))
            {
                return;
            }

            var sharedFilesFeature = new ModuleDefinition(SharedFile.FeatureGuid);

            var module = new Module(sharedFile.ModuleId);

            // get list of pages where this module is published
            var pageModules = PageModule.GetPageModulesByModule(sharedFile.ModuleId);

            foreach (PageModule pageModule in pageModules)
            {
                var pageSettings = new PageSettings(siteSettings.SiteId, pageModule.PageId);

                //don't index pending/unpublished pages
                if (pageSettings.IsPending) { continue; }

				var indexItem = new IndexItem
				{
					SiteId = siteSettings.SiteId,
					PageId = pageSettings.PageId,
					PageName = pageSettings.PageName,
					ViewRoles = pageSettings.AuthorizedRoles,
					ModuleViewRoles = module.ViewRoles,
					FeatureId = SharedFile.FeatureGuid.ToString(),
					FeatureName = sharedFilesFeature.FeatureName,
					FeatureResourceFile = sharedFilesFeature.ResourceFile,
					CreatedUtc = sharedFile.UploadDate,
					LastModUtc = sharedFile.UploadDate,
					ItemId = sharedFile.ItemId,
					ModuleId = sharedFile.ModuleId,
					ModuleTitle = module.ModuleTitle,
					Title = sharedFile.FriendlyName.Replace("_", " ").Replace("-", " ").Replace(".", " "),
					Content = sharedFile.Description,
					PublishBeginDate = pageModule.PublishBeginDate,
					PublishEndDate = pageModule.PublishEndDate
				};
				indexItem.ViewPage = "SharedFiles/Download.aspx?pageid=" + indexItem.PageId.ToInvariantString()
                    + "&fileid=" + indexItem.ItemId.ToInvariantString()
                    + "&mid=" + indexItem.ModuleId.ToInvariantString();
                indexItem.UseQueryStringParams = false;

				IndexHelper.RebuildIndex(indexItem);
            }

			if (debugLog)
			{
				log.Debug($"Indexed {sharedFile.FriendlyName}");
			}
		}

    }
}
