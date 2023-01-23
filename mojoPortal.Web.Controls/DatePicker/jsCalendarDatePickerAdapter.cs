// Author:		       
// Created:            2007-11-07
// Last Modified:      2011-08-15
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.
// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls;


namespace mojoPortal.Web.Controls.DatePicker
{
   
    public class jsCalendarDatePickerAdapter : IDatePicker
    {
        private jsCalendarDatePicker control;

        #region Constructors

        public jsCalendarDatePickerAdapter() 
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

        public string ButtonImageUrl
        {
            get
            {

                return control.ButtonImageUrl;
            }
            set
            {

                control.ButtonImageUrl = value;
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

        public string ClockHours
        {
            get
            {
                return control.ClockHours;
            }
            set
            {
                control.ClockHours = value;
            }
        }

        // *** added by ghalib ghniem Aug-14-2011 ChangeMonth: bool ,ChangeYear: bool, YearRange: string
        // notes by 
        // these are added only becuase we must support all properties of IDataPicker
        // but these properties are not used in this datepicker, they are used in the jQueryUI datepicker
        private bool showMonthList = false;
        public bool ShowMonthList
        {
            get { return showMonthList; }
            set { showMonthList = value; }
        }

        private bool showYearList = false;
        public bool ShowYearList
        {
            get { return showYearList; }
            set { showYearList = value; }
        }

        private string yearRange = string.Empty;
        public string YearRange
        {
            get { return yearRange; }
            set { yearRange = value; }
        }

        // not really implemented in this datepicker but needed to support chanfges in IDatePicker so we can use more of jQuery Datepicker features
        private string calculateWeek = string.Empty;
        public string CalculateWeek
        {
            get { return calculateWeek; }
            set { calculateWeek = value; }
        }

        // not really implemented in this datepicker but needed to support chanfges in IDatePicker so we can use more of jQuery Datepicker features
        private int firstDay = -1; //-1 mean use default don't set it by script
        public int FirstDay
        {
            get { return firstDay; }
            set { firstDay = value; }
        }

        // not really implemented in this datepicker but needed to support chanfges in IDatePicker so we can use more of jQuery Datepicker features
        private bool showWeek = false;
        public bool ShowWeek
        {
            get { return showWeek; }
            set { showWeek = value; }
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
            control = new jsCalendarDatePicker();

        }

        #region Public Methods

        public Control GetControl()
        {
            return control;
        }



        #endregion
    }
}
