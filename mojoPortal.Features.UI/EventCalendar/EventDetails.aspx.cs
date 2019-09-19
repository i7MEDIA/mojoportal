/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using mojoPortal.Business;
using mojoPortal.Features.UI.EventCalendar;
using mojoPortal.Web.Framework;
using Resources;
namespace mojoPortal.Web.EventCalendarUI
{

	public partial class EventCalendarViewEvent : mojoBasePage
	{
		private int moduleId = -1;
        private int itemId = -1;
        private Hashtable moduleSettings;
        protected string GmapApiKey = string.Empty;
		protected CalendarConfiguration config;
        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

        #endregion

        private void Page_Load(object sender, System.EventArgs e)
		{
            LoadSettings();

            if (!UserCanViewPage(moduleId, CalendarEvent.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateControls();
		}

        private void PopulateControls()
        {
            if (itemId > -1)
            {
                CalendarEvent calendarEvent = new CalendarEvent(itemId);
                if (calendarEvent.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                heading.Text = calendarEvent.Title + " - " + calendarEvent.EventDate.ToShortDateString();

                Title = SiteUtils.FormatPageTitle(siteSettings, calendarEvent.Title);

                this.litDescription.Text = calendarEvent.Description;
                this.lblStartTime.Text = calendarEvent.StartTime.ToShortTimeString();
                this.lblEndTime.Text = calendarEvent.EndTime.ToShortTimeString();

                if (config.EnableMap && calendarEvent.ShowMap && calendarEvent.Location.Length > 0)
                {
                    gmap.GMapApiKey = GmapApiKey;
                    gmap.Location = calendarEvent.Location;
                    gmap.EnableMapType = config.MapEnableMapType;
                    gmap.EnableZoom = config.MapEnableZoom;
                    gmap.ShowInfoWindow = config.MapShowInfoWindow;
                    gmap.EnableLocalSearch = config.MapEnableLocalSearch;
                    gmap.MapHeight = config.MapHeight;
                    gmap.MapWidth = config.MapWidth;
                    gmap.EnableDrivingDirections = config.MapEnableDirections;
                    gmap.GmapType = config.MapType;
                    gmap.ZoomLevel = config.MapInitialZoom;
                    gmap.DirectionsButtonText = EventCalResources.DrivingDirectionsButton;
                    gmap.DirectionsButtonToolTip = EventCalResources.DrivingDirectionsButton;
                }
                else
                {
                    gmap.Visible = false;
                }
				lblLocation.Text = calendarEvent.Location;
				pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
            }
        }

        private void LoadSettings()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
            GmapApiKey = SiteUtils.GetGmapApiKey();
			config = new CalendarConfiguration(ModuleSettings.GetModuleSettings(moduleId));
            
            AddClassToBody("eventcaldetail");
        }
	}
}
