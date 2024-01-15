using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

/// <summary>
/// Code sample by Mike Ellison from article
/// http://www.codeproject.com/aspnet/MellDataCalendar.asp
/// 
/// subclass of the ASP.NET Calendar control for
///	displaying events from a DataTable with support
///	for templates
///	
///	Mike has given permission to use this as we will with no conditions
///	though he does appreciate any reference to him in the credits or code
/// 
/// </summary>
public class DataCalendar : System.Web.UI.WebControls.Calendar, INamingContainer
{
	private object _dataSource;
	private DataTable _dtSource;
	private DataView dv;

	// Support either a DataSet or DataTable object
	// for the DataSource property
	public object DataSource
	{
		get { return _dataSource; }
		set
		{
			if (value is DataTable || value is DataSet)
				_dataSource = value;

			else
				throw new Exception("The DataSource property of the DataCalendar control must be a DataTable or DataSet object");
		}
	}

	// If a DataSet is supplied for DataSource,
	// use this property to determine which
	// DataTable within the DataSet should
	// be used; if DataMember is not supplied,
	// the first table in the DataSet will
	// be used.
	public string DataMember { get; set; }


	// Specify the name of the field within
	// the source DataTable that contains
	// a DateTime value for displaying in the
	// calendar.
	public string DayField { get; set; }



	public TableItemStyle DayWithEventsStyle { get; set; } = new TableItemStyle();

	public TableItemStyle CurrentDayStyle { get; set; } = new TableItemStyle();

	[TemplateContainer(typeof(DataCalendarItem))]
	public ITemplate ItemTemplate { get; set; }


	[TemplateContainer(typeof(DataCalendarItem))]
	public ITemplate NoEventsTemplate { get; set; }


	// Constructor    
	public DataCalendar() : base()
	{
		// since this control will be used for displaying
		// events, set these properties as a default
		SelectionMode = CalendarSelectionMode.None;
		//this.ShowGridLines = true;
		//this.SkinID = "EventCalendar";
	}

	private void SetupCalendarItem(TableCell cell, DataRow r, ITemplate t)
	{
		// given a calendar cell and a datarow, set up the
		// templated item and resolve data binding syntax
		// in the template
		var dti = new DataCalendarItem(r);
		t.InstantiateIn(dti);
		if (r is not null)
		{
			if (cell.CssClass.Length > 0)
			{
				cell.CssClass += " daywithevents";
			}
			else
			{
				cell.CssClass = "daywithevents";
			}

			dti.DataBind();
		}
		cell.Controls.Add(dti);
	}

	private static string GetDayFilter(System.Web.UI.WebControls.CalendarDay day, string dayField)
	{
		// changed to use invariant culture 2008-09-02 to fix error that would happen if
		// browser language is Persian
		return $"{dayField} >= #{day.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}# and {dayField} < #{day.Date.AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}#";
	}


	protected override void OnDayRender(TableCell cell, System.Web.UI.WebControls.CalendarDay day)
	{
		if (dv is not null)
		{
			if (_dtSource.Rows.Count > 0)
			{
				// 0.0343
				dv.RowFilter = GetDayFilter(day, DayField);

				// are there events on this day?
				if (dv.Count > 0)
				{
					// there are events on this day; if indicated, 
					// apply the DayWithEventsStyle to the table cell
					if (day.Date == SelectedDate)
					{
						if (CurrentDayStyle is not null)
						{
							cell.ApplyStyle(CurrentDayStyle);
						}

					}
					else
					{
						if (DayWithEventsStyle is not null)
						{
							cell.ApplyStyle(DayWithEventsStyle);
						}
					}

					// for each event on this day apply the
					// ItemTemplate, with data bound to the item's row
					// from the data source
					if (ItemTemplate is not null)
					{
						for (int i = 0; i < dv.Count; i++)
						{
							SetupCalendarItem(cell, dv[i].Row, ItemTemplate);
						}
					}
				}
				else
				{
					// no events this day;
					if (day.Date == SelectedDate)
					{
						if (CurrentDayStyle is not null)
						{
							cell.ApplyStyle(CurrentDayStyle);
						}
					}
					else
					{
						if (NoEventsTemplate is not null)
						{
							SetupCalendarItem(cell, null, NoEventsTemplate);
						}
					}
				}
			}
			else
			{
				// no events 
				if (day.Date == SelectedDate)
				{
					if (CurrentDayStyle is not null)
					{

						cell.ApplyStyle(CurrentDayStyle);
					}
				}
				else
				{
					if (NoEventsTemplate is not null)
					{
						SetupCalendarItem(cell, null, NoEventsTemplate);
					}
				}
			}
		}

		// call the base render method too
		// under windows commenting this has no effect on result
		base.OnDayRender(cell, day);
	}

	protected override void Render(HtmlTextWriter html)
	{
		if (Site is not null && Site.DesignMode)
		{
			html.Write($"[{ID}]");
		}
		else
		{
			_dtSource = null;

			if (DataSource is not null && DayField is not null)
			{
				// determine if the datasource is a DataSet or DataTable
				if (DataSource is DataTable table)
				{
					_dtSource = table;
				}

				if (DataSource is DataSet ds)
				{
					if (DataMember is null || DataMember == string.Empty)
					{
						// if data member isn't supplied, default to the first table
						_dtSource = ds.Tables[0];
					}
					else
					{
						// if data member is supplied, use it
						_dtSource = ds.Tables[DataMember];
					}
				}
				// throw an exception if there is a problem with the data source
				if (_dtSource is null)
				{
					throw new Exception("Error finding the DataSource.  Please check the DataSource and DataMember properties.");
				}
				else
				{
					dv = new DataView(_dtSource);
				}
			}

			// call the base Calendar's Render method
			// allowing OnDayRender() to be executed
			base.Render(html);
		}
	}
}