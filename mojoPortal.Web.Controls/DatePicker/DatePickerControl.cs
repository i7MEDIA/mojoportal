using mojoPortal.Web.Controls.DatePicker;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls
{

    [DefaultProperty("Text"), ToolboxData("<{0}:DatePickerControl runat=server></{0}:DatePickerControl>")]
    [ValidationProperty("Text")]
    public class DatePickerControl : WebControl
    {
        private DatePickerProvider provider;
        private IDatePicker picker;
        private Control datePickerControl;


        public IDatePicker DatePicker
        {
            get 
            {
                if (picker == null) InitPicker();
                return picker; 
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

		public bool ShowTimeOnly
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.ShowTimeOnly;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.ShowTimeOnly = value;
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

        public string RelatedPickerControl
        {
            get
            {
                if (picker == null) InitPicker();
                return picker.RelatedPickerControl;
            }
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Control c = this.Controls[0].Parent.FindControl(value + "dp");
                    if (c != null)
                    {
                        picker.RelatedPickerControl = c.ClientID;
                    }
                    //picker.RelatedPickerControl = value + "dp";
                }
            }
        }

        public RelatedPickerRelation RelatedPickerRelation
        {
            get
            {
                if (picker == null) InitPicker();
				return picker.RelatedPickerRelation;
			}
            set
            {
                if (picker == null) InitPicker();
                if (HttpContext.Current == null) { return; }
				picker.RelatedPickerRelation = value;
			}
        }

		public string View
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.View;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.View = value;
			}
		}

		public string MinView
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.MinView;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.MinView = value;
			}
		}

		public string MinDate
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.MinDate;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.MinDate = value;
			}
		}

		public string MaxDate
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.MaxDate;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.MaxDate = value;
			}
		}
        public string OnSelectJS
        {
			get
			{
				if (picker == null) InitPicker();
				return picker.OnSelectJS;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.OnSelectJS = value;
			}
		}

		public string ExtraSettingsJS
		{
			get
			{
				if (picker == null) InitPicker();
				return picker.ExtraSettingsJS;
			}
			set
			{
				if (picker == null) InitPicker();
				if (HttpContext.Current == null) { return; }
				picker.ExtraSettingsJS = value;
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

                    if (provider == null)
                    {
                        provider = DatePickerManager.Provider;
                    }

                    if (picker == null)
                    {
                        picker = provider.GetDatePicker();
                    }

                    datePickerControl = picker.GetControl();
                    datePickerControl.ID = this.ID + "dp";
                    this.Controls.Clear();
                    this.Controls.Add(datePickerControl);
                }
            }
            catch { }

        }

    }
}
