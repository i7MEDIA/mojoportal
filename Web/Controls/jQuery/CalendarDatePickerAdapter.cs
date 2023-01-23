using mojoPortal.Web.Controls.DatePicker;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class CalendarDatePickerAdapter : IDatePicker
    {
        private CalendarsDatePicker control;

        #region Constructors

        public CalendarDatePickerAdapter()
        {
            InitializeAdapter();
        }

        #endregion


        public string ControlID
        {
            get
            {
                return control.ID;
            }
            set
            {
                control.ID = value;
            }
        }

        public string Text
        {
            get
            {
                return control.Text;
            }
            set
            {
                control.Text = value;
            }
        }

        string buttonImageUrl = string.Empty;
        /// <summary>
        /// implemented only to support the interface but not really used for this datepicker
        /// </summary>
        public string ButtonImageUrl
        {
            get
            {

                //return control.ButtonImage;
                return buttonImageUrl;
            }
            set
            {

                //control.ButtonImage = value;
                buttonImageUrl = value;
            }
        }

        public Unit Width
        {
            get
            {
                return control.Width;
            }
            set
            {
                control.Width = value;
            }
        }

        public bool ShowTime
        {
            get
            {
                return control.ShowTime;
            }
            set
            {
                control.ShowTime = value;
            }
        }

        private string clockHours = string.Empty;

        public string ClockHours
        {
            get { return clockHours; }
            set { clockHours = value; }
        }

        private bool showMonthList = false;
        public bool ShowMonthList
        {
            get
            {
                return showMonthList;
            }
            set
            {
                showMonthList = value;
            }
        }

        private bool showYearList = false;
        public bool ShowYearList
        {
            get
            {
                return showYearList;
            }
            set
            {
                showYearList = value;
            }
        }

        private string yearRange = string.Empty;
        public string YearRange
        {
            get
            {
                return yearRange;
            }
            set
            {
                yearRange = value;
            }
        }

        private bool showWeek = false;
        public bool ShowWeek
        {
            get { return showWeek; }
            set { showWeek = value; }
        }

        private string calculateWeek = string.Empty;
        public string CalculateWeek
        {
            get { return calculateWeek; }
            set { calculateWeek = value; }
        }

        private int firstDay = 1;
        public int FirstDay
        {
            get { return firstDay; }
            set { firstDay = value; }
        }

        public string RelatedPickerControl
		{
			get { return control.RelatedPickerControl; }
			set { control.RelatedPickerControl = value; }
		}

		public RelatedPickerRelation RelatedPickerRelation 
		{ 
			get { return control.RelatedPickerRelation; }
			set { control.RelatedPickerRelation = value; }
		}

		public string View 
		{ 
			get { return control.View; }
			set { control.View = value; }
		}

		public string MinView
		{
			get { return control.View; }
			set { control.View = value; }
		}

		public bool ShowTimeOnly
		{
			get { return control.ShowTimeOnly; }
			set { control.ShowTimeOnly = value; }
		}
		public string MinDate
		{
			get { return control.MinDate; }
			set { control.MinDate = value; }
		}
		public string MaxDate
		{
			get { return control.MaxDate; }
			set { control.MaxDate = value; }
		}

		public string OnSelectJS
		{
			get { return control.OnSelectJS; }
			set { control.OnSelectJS = value; }
		}

		public string ExtraSettingsJS
		{
			get { return control.ExtraSettingsJS; }
			set { control.ExtraSettingsJS = value; }
		}
		private void InitializeAdapter()
        {
            control = new CalendarsDatePicker();
            //control.DoneLabel = Resource.DoneButton;
            //control.HourLabel = Resource.Hour;
            //control.MinuteLabel = Resource.Minute;

        }

        #region Public Methods

        public Control GetControl()
        {
            return control;
        }



        #endregion
    }
}
