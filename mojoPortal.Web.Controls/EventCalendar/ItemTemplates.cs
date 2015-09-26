using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web.Controls
{
    /*********************************************************************************
    * 
    * Class for the item template
    * 
    *********************************************************************************/

    public class DataBoundCalendarItem : Control, IDataItemContainer, INamingContainer
    {
        private object _dataItem;
        private CalendarDay _day;

        public DataBoundCalendarItem(object di, CalendarDay day)
        {
            _dataItem = di;
            _day = day;
        }


        object IDataItemContainer.DataItem
        {
            get
            {
                return _dataItem;
            }
        }

        public System.Data.DataRowView DataItem
        {
            get
            {
                return (System.Data.DataRowView)_dataItem;
            }
        }

        //This is required for paging support, but does not apply to the event calendar
        int IDataItemContainer.DataItemIndex
        {
            get { return 0; }
        }

        //This is required for paging support, but does not apply to the event calendar
        int IDataItemContainer.DisplayIndex
        {
            get { return 0; }
        }

        public CalendarDay Day
        {
            get { return _day; }
        }

    }


    /*********************************************************************************
    * 
    * Class for the calendar header
    * 
    *********************************************************************************/

    public class DataBoundCalendarHeader : Control, INamingContainer
    {
        private System.Collections.Generic.Dictionary<string, DateTime> _dataItem;

        public DataBoundCalendarHeader(DateTime VisibleDate)
        {
            _dataItem = new System.Collections.Generic.Dictionary<string, DateTime>();
            _prevMonth = VisibleDate.AddMonths(-1);
            _currMonth = VisibleDate;
            _nextMonth = VisibleDate.AddMonths(1);
        }

        private DateTime _prevMonth;
        private DateTime _currMonth;
        private DateTime _nextMonth;

        public DateTime PrevMonth
        {
            get
            {
                return _prevMonth;
            }
        }

        public DateTime NextMonth
        {
            get
            {
                return _nextMonth;
            }
        }

        public DateTime CurrentMonth
        {
            get
            {
                return _currMonth;
            }
        }
    }
}
