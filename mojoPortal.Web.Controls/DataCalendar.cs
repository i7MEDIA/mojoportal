using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace mojoPortal.Web.Controls
{
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
    /// Last Modified:		2008-01-28 
    /// 
    /// </summary>
    public class DataCalendar : System.Web.UI.WebControls.Calendar, INamingContainer
    {

        private object _dataSource;
        private string _dataMember;
        private string _dayField;
        private ITemplate _itemTemplate;
        private ITemplate _noEventsTemplate;
        private TableItemStyle _dayWithEventsStyle = new TableItemStyle();
        private TableItemStyle _currentDayStyle = new TableItemStyle();
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
                    throw new Exception("The DataSource property of the DataCalendar control" +
                        " must be a DataTable or DataSet object");
            }
        }

        // If a DataSet is supplied for DataSource,
        // use this property to determine which
        // DataTable within the DataSet should
        // be used; if DataMember is not supplied,
        // the first table in the DataSet will
        // be used.
        public string DataMember
        {
            get { return _dataMember; }
            set { _dataMember = value; }
        }


        // Specify the name of the field within
        // the source DataTable that contains
        // a DateTime value for displaying in the
        // calendar.
        public string DayField
        {
            get { return _dayField; }
            set { _dayField = value; }
        }



        public TableItemStyle DayWithEventsStyle
        {
            get { return _dayWithEventsStyle; }
            set { _dayWithEventsStyle = value; }
        }

        public TableItemStyle CurrentDayStyle
        {
            get { return _currentDayStyle; }
            set { _currentDayStyle = value; }
        }

        [TemplateContainer(typeof(DataCalendarItem))]
        public ITemplate ItemTemplate
        {
            get { return _itemTemplate; }
            set { _itemTemplate = value; }
        }


        [TemplateContainer(typeof(DataCalendarItem))]
        public ITemplate NoEventsTemplate
        {
            get { return _noEventsTemplate; }
            set { _noEventsTemplate = value; }
        }

        //		[TemplateContainer(typeof(DataCalendarItem))]    
        //		public ITemplate SelectedDayTemplate
        //		{
        //			get {return _selectedDayTemplate; }
        //			set {_selectedDayTemplate = value;}
        //		}


        // Constructor    
        public DataCalendar()
            : base()
        {
            // since this control will be used for displaying
            // events, set these properties as a default
            this.SelectionMode = CalendarSelectionMode.None;
            //this.ShowGridLines = true;
            //this.SkinID = "EventCalendar";
        }

        

        

        private void SetupCalendarItem(TableCell cell, DataRow r, ITemplate t)
        {
            // given a calendar cell and a datarow, set up the
            // templated item and resolve data binding syntax
            // in the template
            DataCalendarItem dti = new DataCalendarItem(r);
            t.InstantiateIn(dti);
            if (r != null)
            {
                if (cell.CssClass.Length > 0)
                {
                    cell.CssClass = cell.CssClass + " daywithevents";
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
            //0.0041
            //return dayField
            //    + " >= #" + day.Date.ToString("MM/dd/yyyy") + "# and "
            //    + dayField + " < #" + day.Date.AddDays(1).ToString("MM/dd/yyyy") + "#";

            // changed to use invariant culture 2008-09-02 to fix error that would happen if
            // browser language is Persian
            return dayField
                + " >= #" + day.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "# and "
                + dayField + " < #" + day.Date.AddDays(1).ToString("MM/dd/yyyy", CultureInfo.InvariantCulture) + "#";


            //0.0057
            //return string.Format(
            //        "{0} >= #{1}# and {0} < #{2}#",
            //        dayField,
            //        day.Date.ToString("MM/dd/yyyy"),
            //        day.Date.AddDays(1).ToString("MM/dd/yyyy")
            //        );
                    
        }


        protected override void OnDayRender(TableCell cell, System.Web.UI.WebControls.CalendarDay day)
        {
            // _dtSource was already set by the Render method            
            //if (_dtSource != null)
            if (dv != null)
            {

                // We have the data source as a DataTable now;                
                // filter the records in the DataTable for the given day;
                // force the date format to be MM/dd/yyyy
                // to ensure compatibility with RowFilter
                // date expression syntax (#date#).
                // Also, take the possibility of time
                // values into account by specifying
                // a date range, to include the full day
                //DataView dv = new DataView(_dtSource);

                // ANTS profiler says this takes a long time and is called 42 times in rendering
                // 0.148
                //dv.RowFilter = string.Format(
                //    "{0} >= #{1}# and {0} < #{2}#",
                //    this.DayField,
                //    day.Date.ToString("MM/dd/yyyy"),
                //    day.Date.AddDays(1).ToString("MM/dd/yyyy")
                //    );

                
                if (_dtSource.Rows.Count > 0)
                {
                    // 0.0343
                    dv.RowFilter = GetDayFilter(day, DayField);

                    // are there events on this day?
                    if (dv.Count > 0)
                    {
                        // there are events on this day; if indicated, 
                        // apply the DayWithEventsStyle to the table cell
                        if (day.Date == this.SelectedDate)
                        {
                            if (this._currentDayStyle != null)
                            {

                                cell.ApplyStyle(this._currentDayStyle);
                            }

                        }
                        else
                        {
                            if (this.DayWithEventsStyle != null)
                                cell.ApplyStyle(this.DayWithEventsStyle);
                        }

                        // for each event on this day apply the
                        // ItemTemplate, with data bound to the item's row
                        // from the data source
                        if (this.ItemTemplate != null)
                            for (int i = 0; i < dv.Count; i++)
                            {
                                SetupCalendarItem(cell, dv[i].Row, this.ItemTemplate);
                            }

                    }
                    else
                    {
                        // no events this day;
                        if (day.Date == this.SelectedDate)
                        {
                            if (this._currentDayStyle != null)
                            {

                                cell.ApplyStyle(this._currentDayStyle);
                            }
                        }
                        else
                        {
                            if (this.NoEventsTemplate != null)
                                SetupCalendarItem(cell, null, this.NoEventsTemplate);
                        }

                    }
                }
                else
                {
                    // no events 
                    if (day.Date == this.SelectedDate)
                    {
                        if (this._currentDayStyle != null)
                        {

                            cell.ApplyStyle(this._currentDayStyle);
                        }
                    }
                    else
                    {
                        if (this.NoEventsTemplate != null)
                            SetupCalendarItem(cell, null, this.NoEventsTemplate);
                    }

                }

            }

            // call the base render method too
            // under windows commenting this has no effect on result
            base.OnDayRender(cell, day);

        }

        protected override void Render(HtmlTextWriter html)
        {
            if (this.Site != null && this.Site.DesignMode)
            {
                // TODO: show a bmp or some other design time thing?
                html.Write("[" + this.ID + "]");
            }
            else
            {

                _dtSource = null;

                if (this.DataSource != null && this.DayField != null)
                {
                    // determine if the datasource is a DataSet or DataTable
                    if (this.DataSource is DataTable)
                        _dtSource = (DataTable)this.DataSource;
                    if (this.DataSource is DataSet)
                    {
                        DataSet ds = (DataSet)this.DataSource;
                        if (this.DataMember == null || this.DataMember == "")
                            // if data member isn't supplied, default to the first table
                            _dtSource = ds.Tables[0];
                        else
                            // if data member is supplied, use it
                            _dtSource = ds.Tables[this.DataMember];
                    }
                    // throw an exception if there is a problem with the data source
                    if (_dtSource == null)
                    {
                        throw new Exception("Error finding the DataSource.  Please check " +
                            " the DataSource and DataMember properties.");
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
}
