/// Author:				        
/// Created:			        2005-04-12
/// Last Modified:		        2011-05-23
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.EventCalendarUI
{
    public partial class EventCalendarDayView : mojoBasePage
	{
        
		private int moduleId = -1;
        private DateTime theDate = DateTime.Now;

        
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }

		private void Page_Load(object sender, System.EventArgs e)
		{
            LoadParams();

            if (!UserCanViewPage(moduleId, CalendarEvent.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateControls();

		}

        private void PopulateControls()
        {
            //Title = SiteUtils.FormatPageTitle(siteSettings, calendarEvent.Title);

            //if (!UserCanEditModule(moduleId, CalendarEvent.FeatureGuid))
            //{
            //    this.lnkNewEvent.Visible = false;

            //}

            

            if (moduleId > -1)
            {
                Module module = new Module(moduleId);
                //this.litDate.Text = module.ModuleTitle + " " + this.theDate.ToShortDateString();
                heading.Text = module.ModuleTitle + " " + this.theDate.ToShortDateString();

                Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

                if (UserCanEditModule(moduleId, CalendarEvent.FeatureGuid))
                {
                    heading.LiteralExtraMarkup = "<a href='" + SiteRoot + "/EventCalendar/EditEvent.aspx?"
                        + "mid=" + moduleId.ToInvariantString()
                        + "&date=" + Server.UrlEncode(this.theDate.ToString("s"))
                        + "&pageid=" + CurrentPage.PageId.ToInvariantString() + "' class='ModuleEditLink'>" + EventCalResources.EventCalendarAddEventLabel + "</a>";
                }

                //lnkNewEvent.HRef = SiteRoot + "/EventCalendar/EditEvent.aspx?"
                //    + "mid=" + moduleId.ToInvariantString()
                //    + "&date=" + Server.UrlEncode(this.theDate.ToString("s"))
                //    + "&pageid=" + CurrentPage.PageId.ToInvariantString();

                //lnkNewEvent.InnerHtml = Resources.EventCalResources.EventCalendarAddEventLabel;

                DataSet ds = CalendarEvent.GetEvents(this.moduleId, theDate, theDate);
                //				DataView dv = ds.Tables[0].DefaultView;
                //				dv.Sort = "StartTime ASC ";
                this.rptEvents.DataSource = ds;
                this.rptEvents.DataBind();

            }
        }

        private void LoadParams()
        {
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            theDate = WebUtils.ParseDateFromQueryString("date", DateTime.Now);

            AddClassToBody("eventcaldayview");
  
        }

	}
}
