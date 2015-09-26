
namespace mojoPortal.Web.Controls
{
    using System.ComponentModel;
    using System;

    /// <devdoc>
    ///    <para> Represents a calendar day.</para>
    /// </devdoc>

    public class CalendarDay
    {
        private DateTime date;
        private bool isToday;
        private bool isWeekend;
        private bool isOtherMonth;
        private string dayNumberText;


        /// <devdoc>
        ///    <para>[To be supplied.]</para>
        /// </devdoc>
        public CalendarDay(DateTime date, bool isWeekend, bool isToday, bool isOtherMonth, string dayNumberText)
        {
            this.date = date;
            this.isWeekend = isWeekend;
            this.isToday = isToday;
            this.isOtherMonth = isOtherMonth;
            this.dayNumberText = dayNumberText;
        }


        /// <devdoc>
        ///    <para> Gets the date represented by an instance of this class. This
        ///       property is read-only.</para>
        /// </devdoc>
        public DateTime Date
        {
            get
            {
                return date;
            }
        }


        /// <devdoc>
        ///    <para>Gets the string equivilent of the date represented by an instance of this class. This property is read-only.</para>
        /// </devdoc>
        public string DayNumberText
        {
            get
            {
                return dayNumberText;
            }
        }


        /// <devdoc>
        ///    <para>Gets a value indicating whether the date represented by an instance of
        ///       this class is in a different month from the month currently being displayed. This
        ///       property is read-only.</para>
        /// </devdoc>
        public bool IsOtherMonth
        {
            get
            {
                return isOtherMonth;
            }
        }

        /// <devdoc>
        ///    <para>Gets a value indicating whether the date represented by an instance of this class is today's date. This property is read-only.</para>
        /// </devdoc>
        public bool IsToday
        {
            get
            {
                return isToday;
            }
        }


        /// <devdoc>
        ///    <para>Gets a value indicating whether the date represented by an instance of
        ///       this class is on a weekend day. This property is read-only.</para>
        /// </devdoc>
        public bool IsWeekend
        {
            get
            {
                return isWeekend;
            }
        }

    }
}

