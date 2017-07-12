using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// Based on an article by Mike Ellison
    /// http://www.codeproject.com/aspnet/MellDataCalendar.asp
    /// 
    /// allowing for databinding syntax like the following
    ///	to be used in the .aspx page:
    ///     
    ///	
    /// Last Modified:		4/10/2005 
    /// 
    /// </summary>
    public class DataCalendarItem : Control, INamingContainer
    {

        private DataRow _dataItem;

        public DataCalendarItem(DataRow dr)
        {
            _dataItem = dr;
        }

        // because the source data will be a DataTable
        // object, it makes sense for our DataItem
        // property to return a DataRow object
        // (i.e. a single item in the data source
        //  corresponds to a single row of data)
        public DataRow DataItem
        {
            get { return _dataItem; }
            set { _dataItem = value; }
        }
    }
}
