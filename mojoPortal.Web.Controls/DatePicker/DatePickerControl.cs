// Author:		        
// Created:            2007-11-07
// Last Modified:      2011-08-14
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.DatePicker;

namespace mojoPortal.Web.Controls
{
    
    [DefaultProperty("Text"), ToolboxData("<{0}:DatePickerControl runat=server></{0}:DatePickerControl>")]
    [ValidationProperty("Text")]
    public class DatePickerControl : WebControl
    {
        private DatePickerProvider provider;
        private IDatePicker picker;
        private string providerName = "jsCalendarDatePickerProvider";
        private Control datePickerControl;


        public IDatePicker DatePicker
        {
            get 
            {
                if (picker == null) InitPicker();
                return picker; 
            }
        }

       

        public string ProviderName
        {
            get { return providerName; }
            set
            {
                
                providerName = value;
                if (HttpContext.Current == null) { return; }
                DatePickerProvider newProvider = DatePickerManager.Providers[providerName];
                if ((newProvider != null)&&(newProvider != provider))
                {
                    provider = newProvider;
                    picker = provider.GetDatePicker();
                    datePickerControl = picker.GetControl();
                    datePickerControl.ID = "dp" + this.ID;
                    this.Controls.Clear();
                    this.Controls.Add(datePickerControl);

                   
                }

                

            }
        }

        public string Text
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.Text;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.Text = value;
            }
        }

        public string ButtonImageUrl
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.ButtonImageUrl;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ButtonImageUrl = value;
            }
        }

        public override Unit Width
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.Width;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.Width = value;
            }
        }

        public bool ShowTime
        {
            get 
            {
                if (picker == null) InitPicker();
                return picker.ShowTime; 
            }
            set 
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ShowTime = value; 
            }
        }

        public string ClockHours
        {
            get 
            {
                if (picker == null) InitPicker();
                return picker.ClockHours; 
            }
            set 
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ClockHours = value; 
            }
        }

        // *** added by ghalib ghniem Aug-14-2011 ChangeMonth: bool ,ChangeYear: bool, YearRange: string
        // these are only supported on the jQuery UI datepicker
        public bool ShowMonthList
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.ShowMonthList;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ShowMonthList = value;
            }
        }

        public bool ShowYearList
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.ShowYearList;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ShowYearList = value;
            }
        }

        public string YearRange
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.YearRange;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.YearRange = value;
            }
        }

        public bool ShowWeek
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.ShowWeek;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.ShowWeek = value;
            }
        }

        public string CalculateWeek
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.CalculateWeek;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.CalculateWeek = value;
            }
        }

        public int FirstDay
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.FirstDay;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                picker.FirstDay = value;
            }
        }

        public DatePickerProvider Provider
        {
            get { return provider; }
        }

        protected override void OnInit(EventArgs e)
        {

            
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            // an exception always happens here in design mode
            // this try is just to fix the display in design view in VS
            //try
            //{
            //    provider = DatePickerManager.Providers[providerName];
            //    picker = provider.GetDatePicker();
            //    this.Controls.Add(picker.GetControl());
            //}
            //catch { }
            InitPicker();

            

        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            base.RenderContents(writer);
        }

        private void InitPicker()
        {
            if (HttpContext.Current == null) { return; }
            try
            {
                if (datePickerControl == null)
                {

                    if(provider == null)provider = DatePickerManager.Provider;

                    if(picker == null)picker = provider.GetDatePicker();
                
                    datePickerControl = picker.GetControl();
                    datePickerControl.ID = "dp" + this.ID;
                    this.Controls.Clear();
                    this.Controls.Add(datePickerControl);
                }
            }
            catch { }

        }

    }
}
