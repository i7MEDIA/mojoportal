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
using System.Threading;

namespace mojoPortal.Features
{
    /// <summary>
    /// updates the search index when CalendarEvent data is changed.
    /// </summary>
    public class CalendarEventIndexBuilderProvider : IndexBuilderProvider
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CalendarEventIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

        public CalendarEventIndexBuilderProvider()
        { }

        public override void RebuildIndex(
            PageSettings pageSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (pageSettings == null) return;

            //don't index pending/unpublished pages
            if (pageSettings.IsPending) { return; }

            log.Info(Resources.EventCalResources.EventCalendarFeatureName + " indexing page - " + pageSettings.PageName);

            try
            {
                Guid calendarFeatureGuid = new Guid("c5e6a5df-ac2a-43d3-bb7f-9739bc47194e");
                ModuleDefinition calendarFeature = new ModuleDefinition(calendarFeatureGuid);

                List<PageModule> pageModules
                        = PageModule.GetPageModulesByPage(pageSettings.PageId);

                DataTable dataTable = CalendarEvent.GetEventsByPage(pageSettings.SiteId, pageSettings.PageId);

                foreach (DataRow row in dataTable.Rows)
                {
                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    indexItem.SiteId = pageSettings.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.PageIndex = pageSettings.PageIndex;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = row["ViewRoles"].ToString();
                    indexItem.FeatureId = calendarFeatureGuid.ToString();
                    indexItem.FeatureName = calendarFeature.FeatureName;
                    indexItem.FeatureResourceFile = calendarFeature.ResourceFile;

                    indexItem.ItemId = Convert.ToInt32(row["ItemID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleId = Convert.ToInt32(row["ModuleID"], CultureInfo.InvariantCulture);
                    indexItem.ModuleTitle = row["ModuleTitle"].ToString();
                    indexItem.Title = row["Title"].ToString();
                    indexItem.Content = row["Description"].ToString();
                    indexItem.ViewPage = "EventCalendar/EventDetails.aspx";

                    indexItem.CreatedUtc = Convert.ToDateTime(row["CreatedDate"]);
                    indexItem.LastModUtc = Convert.ToDateTime(row["LastModUtc"]);

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
            if (sender == null) return;
            if (!(sender is CalendarEvent)) return;

            CalendarEvent calendarEvent = (CalendarEvent)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            calendarEvent.SiteId = siteSettings.SiteId;
            calendarEvent.SearchIndexPath = mojoPortal.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(calendarEvent.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    mojoPortal.SearchIndex.IndexHelper.RemoveIndexItem(
                        pageModule.PageId,
                        calendarEvent.ModuleId,
                        calendarEvent.ItemId);
                }
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), calendarEvent))
                {
                    if (debugLog) log.Debug("CalendarEventIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for CalendarEventIndexBuilderProvider.IndexItem");
                }
                //IndexItem(calendarEvent);
            }

        }

        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is CalendarEvent)) return;

            CalendarEvent content = o as CalendarEvent;
            IndexItem(content);

        }

        private static void IndexItem(CalendarEvent calendarEvent)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (calendarEvent == null) return;

            try
            {
                //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                
                //if ((siteSettings == null) 
                //    || (calendarEvent == null))
                //{
                //    return;
                //}

                Module module = new Module(calendarEvent.ModuleId);
                Guid calendarFeatureGuid = new Guid("c5e6a5df-ac2a-43d3-bb7f-9739bc47194e");
                ModuleDefinition calendarFeature = new ModuleDefinition(calendarFeatureGuid);

                // get list of pages where this module is published
                List<PageModule> pageModules
                    = PageModule.GetPageModulesByModule(calendarEvent.ModuleId);

                foreach (PageModule pageModule in pageModules)
                {
                    PageSettings pageSettings
                    = new PageSettings(
                    calendarEvent.SiteId,
                    pageModule.PageId);

                    //don't index pending/unpublished pages
                    if (pageSettings.IsPending) { continue; }

                    mojoPortal.SearchIndex.IndexItem indexItem = new mojoPortal.SearchIndex.IndexItem();
                    if (calendarEvent.SearchIndexPath.Length > 0)
                    {
                        indexItem.IndexPath = calendarEvent.SearchIndexPath;
                    }
                    indexItem.SiteId = calendarEvent.SiteId;
                    indexItem.PageId = pageSettings.PageId;
                    indexItem.PageName = pageSettings.PageName;
                    indexItem.ViewRoles = pageSettings.AuthorizedRoles;
                    indexItem.ModuleViewRoles = module.ViewRoles;
                    indexItem.ItemId = calendarEvent.ItemId;
                    indexItem.ModuleId = calendarEvent.ModuleId;
                    indexItem.ViewPage = "EventCalendar/EventDetails.aspx";
                    indexItem.FeatureId = calendarFeatureGuid.ToString();
                    indexItem.FeatureName = calendarFeature.FeatureName;
                    indexItem.FeatureResourceFile = calendarFeature.ResourceFile;
                    indexItem.ModuleTitle = module.ModuleTitle;
                    indexItem.Title = calendarEvent.Title;
                    indexItem.Content = calendarEvent.Description;
                    indexItem.PublishBeginDate = pageModule.PublishBeginDate;
                    indexItem.PublishEndDate = pageModule.PublishEndDate;

                    indexItem.CreatedUtc = calendarEvent.CreatedDate;
                    indexItem.LastModUtc = calendarEvent.LastModUtc;

                    mojoPortal.SearchIndex.IndexHelper.RebuildIndex(indexItem);
                }

                if (debugLog) log.Debug("Indexed " + calendarEvent.Title);
            

            }
            catch (System.Data.Common.DbException ex)
            {
                log.Error("CalendarEventIndexBuilderProvider.IndexItem", ex);
            }
        }

    }
}
