using System.Data;
using System.Web.UI;

namespace mojoPortal.Web.Controls;

/// <summary>
/// Based on an article by Mike Ellison
/// http://www.codeproject.com/aspnet/MellDataCalendar.asp
/// 
/// allowing for databinding syntax like the following
///	to be used in the .aspx page:
/// </summary>
public class DataCalendarItem(DataRow dr) : Control, INamingContainer
{
	// because the source data will be a DataTable
	// object, it makes sense for our DataItem
	// property to return a DataRow object
	// (i.e. a single item in the data source
	//  corresponds to a single row of data)
	public DataRow DataItem { get; set; } = dr;
}
