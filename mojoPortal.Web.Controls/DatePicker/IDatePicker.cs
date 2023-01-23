using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.DatePicker
{

	public interface IDatePicker
	{
		Control GetControl();
		string ControlID { get; set; }
		string Text { get; set; }
		bool ShowTime { get; set; }
		bool ShowTimeOnly { get; set; }
		string ClockHours { get; set; }
		Unit Width { get; set; }
		bool ShowMonthList { get; set; }
		bool ShowYearList { get; set; }
		string YearRange { get; set; }
		string CalculateWeek { get; set; }
		bool ShowWeek { get; set; }
		int FirstDay { get; set; }
		string View { get; set; }
		string MinView { get; set; }
		string RelatedPickerControl { get; set; }
		RelatedPickerRelation RelatedPickerRelation { get; set; }
		string MinDate { get; set; }
		string MaxDate { get; set; }
		string OnSelectJS { get; set; }
		string ExtraSettingsJS { get; set; }

	}
	public enum RelatedPickerRelation { Start, End, None }
}
