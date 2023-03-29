/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using mojoPortal.Features.UI.EventCalendar;
using System.Collections;

namespace mojoPortal.Web.EventCalendarUI
{
	
	public partial class EventCalendar : SiteModuleControl 
	{
		
        private string editContentImage = ConfigurationManager.AppSettings["EditContentImage"]; 

        protected string EditContentImage
        {
            get
            {
                return editContentImage;
            }
            
        }
	
        private string beginDate = String.Empty;
        private string endDate = String.Empty;
        private DateTime visibleDate;
        private DateTime currentDate;
        private string visibleDateParam;
  //      private string instanceCssClass = string.Empty;
  //      private bool useFillerOnEmptyDays = true;
		//private bool showTimeInCalendar = false;
		protected CalendarConfiguration config;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.cal1.VisibleMonthChanged += new MonthChangedEventHandler(Cal1VisibleMonthChanged);
            this.cal1.SelectionChanged += new EventHandler(Cal1SelectionChanged);
        }

		protected void Page_Load(object sender, EventArgs e)
		{
			Title1.EditUrl = SiteRoot + "/EventCalendar/EditEvent.aspx";
            Title1.EditText = EventCalResources.EventCalendarAddEventLabel;
            
            Title1.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            //pnlContainer.ModuleId = ModuleId;

            LoadParams();

            SetupCss();

            if (
                (!Page.IsPostBack)
                ||(this.RenderInWebPartMode)
                )
			{
				PopulateControls();
			}
		}


		private void PopulateControls()
		{
			
			this.cal1.VisibleDate = visibleDate;
			if(currentDate == visibleDate)
			{
				this.cal1.SelectedDate = visibleDate;
			}
            // add 7 days to begin and end date 
            // this allows showing events from previous or future month
            // when those days happen to be visible on the calendar
			DateTime beginMonth = new DateTime(visibleDate.Year, visibleDate.Month,1).AddDays(-7);
			DateTime endMonth = beginMonth.AddDays(49);

            DataTable dt = CalendarEvent.GetEventsTable(this.ModuleId, beginMonth, endMonth);
            this.cal1.DataSource = dt;
			
		}

		private void Cal1VisibleMonthChanged(object sender, MonthChangedEventArgs e)
		{
			
            if (!this.RenderInWebPartMode)
            {
               
                //string baseUrl = currentPage.ResolveUrl(siteSettings);
                string baseUrl = SiteUtils.GetCurrentPageUrl();
                //if (baseUrl.EndsWith(".aspx"))
                //{
                    baseUrl += "?";
                //}
                //else
                //{
                //    baseUrl += "&";
                //}

                string redirectUrl =
                    baseUrl
                    + visibleDateParam + "="
                    + Page.Server.UrlEncode(e.NewDate.ToString("s"));

                WebUtils.SetupRedirect(this, redirectUrl);
            }
			

		}

		private void Cal1SelectionChanged(object sender, EventArgs e)
		{
			
            string redirectUrl =
                   SiteRoot 
                   + "/EventCalendar/DayView.aspx?mid=" + this.ModuleId.ToString()
                   + "&date=" 
                   + Page.Server.UrlEncode(cal1.SelectedDate.ToString("s"))
                   + WebUtils.BuildQueryString("date");

            WebUtils.SetupRedirect(this, redirectUrl);
           

		}

        private void SetupCss()
        {
            // older skins have this
            StyleSheet stylesheet = (StyleSheet)Page.Master.FindControl("StyleSheet");
            if (stylesheet != null)
            {
                if (stylesheet.FindControl("mpdatacalendarcss") == null)
                {
                    Literal cssLink = new Literal();
                    cssLink.ID = "mpdatacalendarcss";
                    cssLink.Text = "\n<link href='"
                    + SiteUtils.GetSkinBaseUrl(this.Page)
                    + "mpdatacalendar.css' type='text/css' rel='stylesheet' media='screen' />";

                    stylesheet.Controls.Add(cssLink);
                }
            }
           
        }


        private void LoadParams()
        {
            visibleDateParam = "visdate" + this.ModuleId.ToString();
            visibleDate = WebUtils.ParseDateFromQueryString(visibleDateParam, DateTime.Now);
            currentDate = DateTime.Now;

			config = new CalendarConfiguration(Settings);

			if (config.InstanceCssClass.Length > 0) { pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass); }



		}
	}
}
