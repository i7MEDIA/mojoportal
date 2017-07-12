//  Author:                 
//	Created:			    2010-06-15
//	Last Modified:		    2011-08-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.DatePicker;
using Resources;

namespace mojoPortal.Web.UI
{
    public class jDatePickerAdapter : IDatePicker
    {
        private jDatePicker control;

        #region Constructors

        public jDatePickerAdapter()
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

                return control.ButtonImage;
            }
            set
            {

                control.ButtonImage = value;
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

        public bool ShowMonthList
        {
            get
            {
                return control.ChangeMonth;
            }
            set
            {
                control.ChangeMonth = value;
            }
        }

        public bool ShowYearList
        {
            get
            {
                return control.ChangeYear;
            }
            set
            {
                control.ChangeYear = value;
            }
        }

        public string YearRange
        {
            get
            {
                return control.YearRange;
            }
            set
            {
                control.YearRange = value;
            }
        }

        public bool ShowWeek
        {
            get { return control.ShowWeek; }
            set { control.ShowWeek = value; }
        }

        public string CalculateWeek
        {
            get { return control.CalculateWeek; }
            set { control.CalculateWeek = value; }
        }

        public int FirstDay
        {
            get { return control.FirstDay; }
            set { control.FirstDay = value; }
        }


        private void InitializeAdapter()
        {
            control = new jDatePicker();
            control.DoneLabel = Resource.DoneButton;
            control.HourLabel = Resource.Hour;
            control.MinuteLabel = Resource.Minute;

        }

        #region Public Methods

        public Control GetControl()
        {
            return control;
        }



        #endregion
    }
}