using System;
using System.Collections;
using System.ComponentModel;
using System.Web.Util;
using System.Web.UI;
using System.Web.UI.WebControls.Adapters;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Drawing;

namespace mojoPortal.Web.Controls
{

    /// <summary>
    /// A Calendar control that can be databound to a data source to show events
    /// </summary>
    public class EventCalendarControl : System.Web.UI.WebControls.CompositeDataBoundControl
    {
        public EventCalendarControl()
        {
        }


        /*********************************************************************************
         * 
         * Private class variables
         * 
         *********************************************************************************/

        private ITemplate _dayEventTemplate;
        private ITemplate _dayEmptyTemplate;
        private ITemplate _headerTemplate;
        internal const string ViewStateDataKey = "_!DataBoundData";

        private Table m_table = null;

        /*********************************************************************************
        * 
        * Private constants 
        * 
        *********************************************************************************/

        private const int STYLEMASK_DAY = 16;
        private const int STYLEMASK_UNIQUE = 15;
        private const int STYLEMASK_SELECTED = 8;
        private const int STYLEMASK_TODAY = 4;
        private const int STYLEMASK_OTHERMONTH = 2;
        private const int STYLEMASK_WEEKEND = 1;

        private const string COMMAND_PREVMONTH = "PrevMonth";
        private const string COMMAND_NEXTMONTH = "NextMonth";

        private static DateTime baseDate = new DateTime(2000, 1, 1);

        private TableItemStyle titleStyle;
        private TableItemStyle nextPrevStyle;
        private TableItemStyle dayHeaderStyle;
        private TableItemStyle dayStyle;
        private Style dayNumberStyle;
        private TableItemStyle otherMonthDayStyle;
        private TableItemStyle todayDayStyle;
        private TableItemStyle weekendDayStyle;
        private string defaultButtonColorText;
        private string dayLinkFormat = string.Empty;

        /*********************************************************************************
        * 
        * Public Control Properties
        * 
        *********************************************************************************/

        public string DayLinkFormat
        {
            get { return dayLinkFormat; }
            set { dayLinkFormat = value; }
        }

        /// <summary>
        /// The name of the field in the dataset that will represent the datetime
        /// </summary>
        [DefaultValue("")]
        [Themeable(false)]
        public virtual string DayField
        {
            get
            {
                object o = ViewState["DayField"];
                if (o != null)
                {
                    return (string)o;
                }
                return String.Empty;
            }
            set
            {
                ViewState["DayField"] = value;
                if (Initialized)
                {
                    OnDataPropertyChanged();
                }
            }
        }

        [DefaultValue("")]
        [Themeable(false)]
        public virtual string EndDayField
        {
            get
            {
                object o = ViewState["EndDayField"];
                if (o != null)
                {
                    return (string)o;
                }
                return String.Empty;
            }
            set
            {
                ViewState["EndDayField"] = value;
                if (Initialized)
                {
                    OnDataPropertyChanged();
                }
            }
        }

        /// <summary>
        /// The template for the databound items
        /// </summary>
        [Browsable(false)]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DataBoundCalendarItem))]
        public ITemplate DayEventTemplate
        {
            get
            {
                return _dayEventTemplate;
            }
            set
            {
                _dayEventTemplate = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DataBoundCalendarItem))]
        public ITemplate DayEmptyTemplate
        {
            get
            {
                return _dayEmptyTemplate;
            }
            set
            {
                _dayEmptyTemplate = value;
            }
        }

        [Browsable(false)]
        [DefaultValue(null)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(DataBoundCalendarHeader))]
        public ITemplate HeaderTemplate
        {
            get
            {
                return _headerTemplate;
            }
            set
            {
                _headerTemplate = value;
            }
        }

        /*********************************************************************************
        * 
        * Date/Time Functionality
        * 
        *********************************************************************************/

        void checkVisibleDate()
        {
            if (VisibleDate == DateTime.MinValue)
            {
                VisibleDate = DateTime.Now;
            }
        }

        private DateTime FirstCalendarDay(DateTime visibleDate)
        {
            DateTime firstDayOfMonth = new DateTime(visibleDate.Year, visibleDate.Month, 1);
            System.Globalization.Calendar cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            int daysFromLastMonth = ((int)cal.GetDayOfWeek(firstDayOfMonth)) - NumericFirstDayOfWeek();
            // Always display at least one day from the previous month
            if (daysFromLastMonth <= 0)
            {
                daysFromLastMonth += 7;
            }
            return cal.AddDays(firstDayOfMonth, -daysFromLastMonth);
        }

        private int NumericFirstDayOfWeek()
        {
            // Used globalized value by default
            return (FirstDayOfWeek == System.Web.UI.WebControls.FirstDayOfWeek.Default)
            ? (int)DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek
            : (int)FirstDayOfWeek;
        }

        private DateTime EndDate(DateTime firstCalendarDay)
        {
            return firstCalendarDay.AddDays(6 * 7);
        }


        void onVisibleMonthChanged(DateTime oldDate, DateTime newDate)
        {
            MonthChangedEventArgs args = new MonthChangedEventArgs(oldDate, newDate);
            if (VisibleMonthChanged != null) VisibleMonthChanged(this, args);
            DataBind();
        }

        /*********************************************************************************
        * 
        * Data binding control methods
        * 
        *********************************************************************************/

        protected override int CreateChildControls(IEnumerable dataSource, bool dataBinding)
        {
            //set the count to zero
            int controlsCreated = 0;

            //Create the table for the rows

            DateTime visibleDate = this.VisibleDate;
            DateTime firstDay = FirstCalendarDay(visibleDate);
            DateTime todaysDate = TodaysDate;
            System.Globalization.Calendar threadCalendar = DateTimeFormatInfo.CurrentInfo.Calendar;

            m_table = CreateTable(visibleDate, firstDay, threadCalendar);
            this.Controls.Add(m_table);

            if (dataBinding)
            {
                //We have real data.
                System.Data.DataView dv = SetupRealData(dataSource);
                controlsCreated += CreateDataBoundChildren(dv, m_table, todaysDate, visibleDate, threadCalendar);
            }
            else
            {
                //Get the number of templates from viewstate and instantiate the templates
                controlsCreated += createChildrenFromViewstate(m_table, todaysDate, visibleDate, threadCalendar);
            }
            return controlsCreated;
        }


        // Creates the children based on the data
        //private int CreateDataBoundChildren(System.Data.DataView dv, Table table, DateTime todaysDate, DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        //{
        //    DateTime firstDay = FirstCalendarDay(this.VisibleDate);

        //    int dayoffset = 0;

        //    for (int iRow = 0; iRow < 6; iRow++)
        //    {
        //        TableRow row = new TableRow();
        //        table.Rows.Add(row);

        //        for (int iDay = 0; iDay < 7; iDay++)
        //        {
        //            DateTime d = firstDay.AddDays(dayoffset);

        //            //Initialize the cell
        //            CalendarDay day = getDay(d, TodaysDate, visibleDate, threadCalendar);
        //            TableCell cell = CreateDayCell(day);
        //            row.Cells.Add(cell);

        //            //Process real data for this day
        //            dv.RowFilter = string.Format("{0}>= #{1}# AND {0} < #{2}#", this.DayField, d.ToString("MM/dd/yyyy"), d.AddDays(1).ToString("MM/dd/yyyy"));
        //            if (dv.Count > 0 && this.DayEventTemplate != null)
        //            {
        //                foreach (System.Data.DataRowView drv in dv)
        //                {
        //                    DataBoundCalendarItem dataitem = new DataBoundCalendarItem(drv, day);
        //                    DayEventTemplate.InstantiateIn(dataitem);
        //                    //add the controls to both collections
        //                    cell.Controls.Add(dataitem);
        //                    //databind the data item
        //                    dataitem.DataBind();
        //                }
        //            }
        //            else if (this.DayEmptyTemplate != null)
        //            {
        //                DataBoundCalendarItem dataitem = new DataBoundCalendarItem(null, day);
        //               DayEmptyTemplate.InstantiateIn(dataitem);
        //                //add the controls to both collections
        //                cell.Controls.Add(dataitem);

        //            }
        //            dayoffset++;

        //        }

        //    }
        //    return 1;
        //}

        private int CreateDataBoundChildren(System.Data.DataView dv, Table table, DateTime todaysDate, DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            DateTime firstDay = FirstCalendarDay(this.VisibleDate);
            dv.Table.Locale = new CultureInfo("en-US");

            int dayoffset = 0;
            string dayField = this.DayField;
            string endDayField = this.EndDayField;

            for (int iRow = 0; iRow < 6; iRow++)
            {
                TableRow row = new TableRow();
                table.Rows.Add(row);

                for (int iDay = 0; iDay < 7; iDay++)
                {
                    DateTime d = firstDay.AddDays(dayoffset);

                    //Initialize the cell
                    CalendarDay day = getDay(d, TodaysDate, visibleDate, threadCalendar);
                    TableCell cell = CreateDayCell(day);
                    row.Cells.Add(cell);

                    //Process real data for this day
                    dv.RowFilter = string.Format("IsNull({1},{0}) >= #{2}# AND {0}<#{3}#", dayField, endDayField, d.ToString("MM/dd/yyyy"), d.AddDays(1).ToString("MM/dd/yyyy"));

                    if (dv.Count > 0 && this.DayEventTemplate != null)
                    {
                        foreach (System.Data.DataRowView drv in dv)
                        {
                            DataBoundCalendarItem dataitem = new DataBoundCalendarItem(drv, day);
                            DayEventTemplate.InstantiateIn(dataitem);
                            //add the controls to both collections
                            cell.Controls.Add(dataitem);
                            //databind the data item
                            dataitem.DataBind();
                        }
                    }
                    else if (this.DayEmptyTemplate != null)
                    {
                        DataBoundCalendarItem dataitem = new DataBoundCalendarItem(null, day);
                        DayEmptyTemplate.InstantiateIn(dataitem);
                        //add the controls to both collections
                        cell.Controls.Add(dataitem);

                    }
                    dayoffset++;
                }
            }
            return 1;
        }

        //Gets the data ready and caches the count of records in viewstate.

        System.Data.DataView SetupRealData(IEnumerable data)
        {
            System.Data.DataView dv = null;

            if (data is System.Data.DataSet)
            {
                System.Data.DataTable dt;
                if (DataMember != null && DataMember != "")
                {
                    dt = ((System.Data.DataSet)data).Tables[this.DataMember];
                }
                else
                {
                    dt = ((System.Data.DataSet)data).Tables[0];
                }

                dv = new System.Data.DataView(dt);
            }
            else if (data is System.Data.DataTable)
            {
                System.Data.DataTable dt = (System.Data.DataTable)data;
                dv = new System.Data.DataView(dt);
            }
            else if (data is System.Data.DataView)
            {
                dv = (System.Data.DataView)data;
            }

            cacheDataInViewstate(dv);

            return dv;
        }

        //void cacheDataInViewstate(System.Data.DataView dv)
        //{

        //    DateTime firstDay = FirstCalendarDay(this.VisibleDate);
        //    DateTime lastDay = EndDate(this.VisibleDate);

        //    System.Collections.Generic.Dictionary<DateTime, int> ctrlcount = new System.Collections.Generic.Dictionary<DateTime, int>();

        //    foreach (System.Data.DataRowView drv in dv)
        //    {
        //        DateTime rowdate = ((DateTime)drv[DayField]).Date;
        //        if (rowdate > firstDay && rowdate <= lastDay)
        //        {
        //            if (ctrlcount.ContainsKey(rowdate))
        //            {
        //                ctrlcount[rowdate] += 1;
        //            }
        //            else
        //            {
        //                ctrlcount[rowdate] = 1;
        //            }
        //        }
        //    }
        //    ViewState[ViewStateDataKey] = ctrlcount;
        //}

        void cacheDataInViewstate(System.Data.DataView dv)
        {
            DateTime firstDay = FirstCalendarDay(this.VisibleDate);
            DateTime lastDay = EndDate(this.VisibleDate);

            System.Collections.Generic.Dictionary<DateTime, int> ctrlcount = new System.Collections.Generic.Dictionary<DateTime, int>();

            foreach (System.Data.DataRowView drv in dv)
            {
                DateTime startdate = ((DateTime)drv[DayField]).Date;
                DateTime enddate = (drv[EndDayField] != DBNull.Value) ? ((DateTime)drv[EndDayField]).Date : startdate;
                DateTime rowdate = startdate;
                while (rowdate <= enddate)
                {
                    if (rowdate >= firstDay && rowdate <= lastDay)
                    {
                        if (ctrlcount.ContainsKey(rowdate))
                        {
                            ctrlcount[rowdate] += 1;
                        }
                        else
                        {
                            ctrlcount[rowdate] = 1;
                        }
                    }
                    rowdate = rowdate.AddDays(1);
                }
            }
            ViewState[ViewStateDataKey] = ctrlcount;
        }

        //Creates the control hierarchy based on the counts of data in viewstate
        //To be smart, we don't databind on postback. We rely on creating the same number of controls as the
        //initial population, the values for the controls then come from viewstate.
        private int createChildrenFromViewstate(Table table, DateTime todaysDate, DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            System.Collections.Generic.Dictionary<DateTime, int> ctrlcount;
            ctrlcount = (System.Collections.Generic.Dictionary<DateTime, int>)ViewState[ViewStateDataKey];

            DateTime firstDay = FirstCalendarDay(this.VisibleDate);
            int dayoffset = 0;

            for (int iRow = 0; iRow < 6; iRow++)
            {
                TableRow row = new TableRow();
                table.Rows.Add(row);

                for (int iDay = 0; iDay < 7; iDay++)
                {
                    DateTime d = firstDay.AddDays(dayoffset);

                    //Initialize the cell
                    CalendarDay day = getDay(d, TodaysDate, visibleDate, threadCalendar);
                    TableCell cell = CreateDayCell(day);
                    row.Cells.Add(cell);

                    if (ctrlcount != null)
                    {
                        if (this.DayEventTemplate != null && ctrlcount.ContainsKey(d))
                        {
                            int nControlsForDay = ctrlcount[d];

                            for (int count = 0; count < nControlsForDay; count++)
                            {
                                DataBoundCalendarItem dataitem = new DataBoundCalendarItem(null, day);
                                DayEventTemplate.InstantiateIn(dataitem);
                                cell.Controls.Add(dataitem);
                                //Note we don't databind these new controls.
                            }

                        }
                        else if (this.DayEmptyTemplate != null)
                        {
                            DataBoundCalendarItem dataitem = new DataBoundCalendarItem(null, day);
                            DayEmptyTemplate.InstantiateIn(dataitem);
                            cell.Controls.Add(dataitem);
                        }
                    }
                    dayoffset++;
                }
            }
            return 1;
        }

        //Validate that the datasource is something we can handle. If not then throw an exception    
        protected override void ValidateDataSource(object dataSource)
        {
            if ((dataSource == null) ||
                (dataSource is IListSource) ||
                (dataSource is IEnumerable) ||
                (dataSource is IDataSource))
            {
                return;
            }
            throw new InvalidOperationException("Data source is not a type that the control can handle. It should implement the IListSource, IEnumerable or IDataSource interface.");
        }


        /*********************************************************************************
        * 
        * Calendar rendering methods
        * 
        *********************************************************************************/

        //Creates the table for the calendar
        private Table CreateTable(DateTime visibleDate, DateTime firstDay, System.Globalization.Calendar threadCalendar)
        {
            Color defaultColor = ForeColor;
            if (defaultColor == Color.Empty)
            {
                defaultColor = Color.Black;
            }
            defaultButtonColorText = ColorTranslator.ToHtml(defaultColor);

            Table table = new Table();

            if (ID != null)
            {
                table.ID = ClientID;
            }
            table.CopyBaseAttributes(this);
            if (ControlStyleCreated)
            {
                table.ApplyStyle(ControlStyle);
            }
            table.Width = Width;
            table.Height = Height;
            table.CellPadding = CellPadding;
            table.CellSpacing = CellSpacing;

            // default look
            if ((ControlStyleCreated == false) ||
                BorderWidth.Equals(Unit.Empty))
            {
                table.BorderWidth = Unit.Pixel(1);
            }

            if (ShowGridLines)
            {
                table.GridLines = GridLines.Both;
            }
            else
            {
                table.GridLines = GridLines.None;
            }

            bool useAccessibleHeader = UseAccessibleHeader;
            if (useAccessibleHeader)
            {
                if (table.Attributes["title"] == null)
                {
                    table.Attributes["title"] = string.Empty;
                }
            }

            string caption = Caption;
            if (caption.Length > 0)
            {
                table.Caption = caption;
                table.CaptionAlign = CaptionAlign;
            }

            if (ShowTitle)
            {
                table.Rows.Add(CreateTitleRow(visibleDate, threadCalendar));
            }

            if (ShowDayHeader)
            {
                table.Rows.Add(CreateDayHeader(firstDay, visibleDate, threadCalendar));
            }

            return table;
        }

        //Creates a title row, using the template if specified
        private TableRow CreateTitleRow(DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            //inside the row for the title, we create a cell and an inner table
            TableRow row = new TableRow();

            TableCell titleCell = new TableCell();
            titleCell.ColumnSpan = 7;
            row.Cells.Add(titleCell);

            if (this.HeaderTemplate != null)
            {
                DataBoundCalendarHeader dataitem = new DataBoundCalendarHeader(visibleDate);
                HeaderTemplate.InstantiateIn(dataitem);
                dataitem.DataBind();
                titleCell.Controls.Add(dataitem);
            }
            else
                titleCell.Controls.Add(CreateStaticTitle(visibleDate, threadCalendar));
            return row;
        }

        //Creates a title block. No customization is offered, if that's needed then specify the header template.
        Control CreateStaticTitle(DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            //Create a new table for the header controls

            Table titleTable = new Table();
            titleTable.GridLines = GridLines.None;
            titleTable.Width = Unit.Percentage(100);
            titleTable.CellSpacing = 0;

            TableRow titleTableRow = new TableRow();
            titleTable.Rows.Add(titleTableRow);

            TableCell PrevCell = new TableCell();
            titleTableRow.Cells.Add(PrevCell);
            PrevCell.ApplyStyle(nextPrevStyle);
            Button PrevBtn = new Button();
            PrevBtn.CssClass = "buttonlink";
            PrevBtn.Text = "&lt; " + threadCalendar.AddMonths(visibleDate, -1).ToString("MMMM");
            PrevBtn.CommandName = COMMAND_PREVMONTH;
            PrevCell.Controls.Add(PrevBtn);

            TableCell MonthCell = new TableCell();
            titleTableRow.Cells.Add(MonthCell);
            MonthCell.ApplyStyle(titleStyle);
            MonthCell.Text = visibleDate.ToString("MMMM yyyy");

            TableCell NextCell = new TableCell();
            titleTableRow.Cells.Add(NextCell);
            NextCell.ApplyStyle(nextPrevStyle);
            Button NextBtn = new Button();
            NextBtn.CssClass = "buttonlink";
            NextBtn.Text = threadCalendar.AddMonths(visibleDate, +1).ToString("MMMM") + " &gt;";
            NextBtn.CommandName = COMMAND_NEXTMONTH;
            NextCell.Controls.Add(NextBtn);

            return titleTable;
        }


        CalendarDay getDay(DateTime d, DateTime todaysDate, DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            int dayOfWeek = (int)threadCalendar.GetDayOfWeek(d);
            int dayOfMonth = threadCalendar.GetDayOfMonth(d);
            string dayNumberText = d.ToString("dd", CultureInfo.CurrentCulture);
            int visibleDateMonth = threadCalendar.GetMonth(visibleDate);

            return new CalendarDay(d,
                (dayOfWeek == 0 || dayOfWeek == 6), // IsWeekend
                d.Equals(todaysDate), // IsToday
                threadCalendar.GetMonth(d) != visibleDateMonth, // IsOtherMonth
                dayNumberText // Number Text
                );
        }

        int _definedStyleMask = 0;
        private TableItemStyle[] _cellStyles;
        private TableCell CreateDayCell(CalendarDay day)
        {
            //initialize the cellstyles
            if (_cellStyles == null)
            {
                _cellStyles = new TableItemStyle[16];
            }

            //initialize style mask
            if (_definedStyleMask == 0)
            {
                _definedStyleMask = GetDefinedStyleMask();
            }

            int styleMask = STYLEMASK_DAY;
            if (day.IsOtherMonth)
                styleMask |= STYLEMASK_OTHERMONTH;
            if (day.IsToday)
                styleMask |= STYLEMASK_TODAY;
            if (day.IsWeekend)
                styleMask |= STYLEMASK_WEEKEND;
            int dayStyleMask = _definedStyleMask & styleMask;
            // determine the unique portion of the mask for the current calendar,
            // which will strip out the day style bit
            int dayStyleID = dayStyleMask & STYLEMASK_UNIQUE;

            TableItemStyle cellStyle = _cellStyles[dayStyleID];
            if (cellStyle == null)
            {
                cellStyle = new TableItemStyle();
                SetDayStyles(cellStyle, dayStyleMask, Unit.Percentage(14));
                _cellStyles[dayStyleID] = cellStyle;
            }

            TableCell cell = new TableCell();
            cell.ApplyStyle(cellStyle);


            DayNumberDiv div;
            if (dayLinkFormat.Length > 0)
            {
                div = new DayNumberDiv(day, dayLinkFormat);
            }
            else
            {
                div = new DayNumberDiv(day.DayNumberText);
            }
            div.ApplyStyle(DayNumberStyle);
            cell.Controls.Add(div);

            return cell;
        }

        private TableRow CreateDayHeader(DateTime firstDay, DateTime visibleDate, System.Globalization.Calendar threadCalendar)
        {
            TableRow row = new TableRow();

            DateTimeFormatInfo dtf = DateTimeFormatInfo.CurrentInfo;

            TableItemStyle dayNameStyle = new TableItemStyle();
            dayNameStyle.HorizontalAlign = HorizontalAlign.Center;
            dayNameStyle.CopyFrom(DayHeaderStyle);
            DayNameFormat dayNameFormat = DayNameFormat;

            int numericFirstDay = (int)threadCalendar.GetDayOfWeek(firstDay);
            for (int i = numericFirstDay; i < numericFirstDay + 7; i++)
            {
                string dayName;
                int dayOfWeek = i % 7;
                switch (dayNameFormat)
                {
                    case DayNameFormat.FirstLetter:
                        dayName = dtf.GetDayName((DayOfWeek)dayOfWeek).Substring(0, 1);
                        break;
                    case DayNameFormat.FirstTwoLetters:
                        dayName = dtf.GetDayName((DayOfWeek)dayOfWeek).Substring(0, 2);
                        break;
                    case DayNameFormat.Full:
                        dayName = dtf.GetDayName((DayOfWeek)dayOfWeek);
                        break;
                    case DayNameFormat.Short:
                        dayName = dtf.GetAbbreviatedDayName((DayOfWeek)dayOfWeek);
                        break;
                    case DayNameFormat.Shortest:
                        dayName = dtf.GetShortestDayName((DayOfWeek)dayOfWeek);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "Unknown DayNameFormat value!");
                        goto
                    case DayNameFormat.Short;
                }

                TableCell cell = new TableCell();
                cell.ApplyStyle(dayNameStyle);
                cell.Text = dayName;
                row.Cells.Add(cell);

            }
            return row;
        }

        private string GetMonthName(int m, bool bFull)
        {
            if (bFull)
            {
                return DateTimeFormatInfo.CurrentInfo.GetMonthName(m);
            }
            else
            {
                return DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(m);
            }
        }

        private int GetDefinedStyleMask()
        {

            // Selected is always defined because it has default effects
            int styleMask = STYLEMASK_SELECTED;
            //TODO: All of these were checking if the style was empty
            if (dayStyle != null)
                styleMask |= STYLEMASK_DAY;
            if (todayDayStyle != null)
                styleMask |= STYLEMASK_TODAY;
            if (otherMonthDayStyle != null)
                styleMask |= STYLEMASK_OTHERMONTH;
            if (weekendDayStyle != null)
                styleMask |= STYLEMASK_WEEKEND;
            return styleMask;
        }

        private void SetDayStyles(TableItemStyle style, int styleMask, Unit defaultWidth)
        {

            // default day styles
            style.Width = defaultWidth;
            style.HorizontalAlign = HorizontalAlign.Center;

            if ((styleMask & STYLEMASK_DAY) != 0)
            {
                style.CopyFrom(DayStyle);
            }
            if ((styleMask & STYLEMASK_WEEKEND) != 0)
            {
                style.CopyFrom(WeekendDayStyle);
            }
            if ((styleMask & STYLEMASK_OTHERMONTH) != 0)
            {
                style.CopyFrom(OtherMonthDayStyle);
            }
            if ((styleMask & STYLEMASK_TODAY) != 0)
            {
                style.CopyFrom(TodayDayStyle);
            }
        }


        /*********************************************************************************
        * 
        * PostBack Handling
        * 
        *********************************************************************************/

        protected override bool OnBubbleEvent(object source, EventArgs args)
        {
            IButtonControl btn = source as IButtonControl;
            if (btn != null)
            {
                DateTime oldDate = VisibleDate;
                if (btn.CommandName.Equals(COMMAND_PREVMONTH, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.VisibleDate = VisibleDate.AddMonths(-1);
                    this.OnDataPropertyChanged();
                    onVisibleMonthChanged(VisibleDate, oldDate);
                    return true;
                }
                if (btn.CommandName.Equals(COMMAND_NEXTMONTH, StringComparison.CurrentCultureIgnoreCase))
                {
                    this.VisibleDate = VisibleDate.AddMonths(1);
                    this.OnDataPropertyChanged();
                    onVisibleMonthChanged(VisibleDate, oldDate);
                    return true;
                }
            }
            return false;
        }

        /*********************************************************************************
        * 
        * Proxy properties for the calendar control
        * 
        *********************************************************************************/

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public DateTime TodaysDate
        {
            get
            {
                object o = ViewState["TodaysDate"];
                return ((o == null) ? DateTime.Today.Date : (DateTime)o);
            }
            set
            {
                ViewState["TodaysDate"] = value.Date;
            }
        }


        [DefaultValueAttribute("")]
        [LocalizableAttribute(true)]
        // [System.Web.WebCategoryAttribute("Accessibility")]
        public virtual string Caption
        {
            get
            {
                string s = (string)ViewState["Caption"];
                return (s != null) ? s : String.Empty;
            }
            set
            {
                ViewState["Caption"] = value;
            }
        }


        // [System.Web.WebCategoryAttribute("Accessibility")]
        [DefaultValue(TableCaptionAlign.NotSet)]
        public virtual TableCaptionAlign CaptionAlign
        {
            get
            {
                object o = ViewState["CaptionAlign"];
                return (o != null) ? (TableCaptionAlign)o : TableCaptionAlign.NotSet;
            }
            set
            {
                if ((value < TableCaptionAlign.NotSet) ||
                    (value > TableCaptionAlign.Right))
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["CaptionAlign"] = value;
            }
        }

        [DefaultValueAttribute(2)]
        //[System.Web.WebCategoryAttribute("Layout")]
        public int CellPadding
        {
            get
            {
                object o = ViewState["CellPadding"];
                return ((o == null) ? 2 : (int)o);
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["CellPadding"] = value;
            }
        }

        [DefaultValueAttribute(0)]
        //[System.Web.WebCategoryAttribute("Layout")]
        public int CellSpacing
        {
            get
            {
                object o = ViewState["CellSpacing"];
                return ((o == null) ? 0 : (int)o);
            }
            set
            {
                if (value < -1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["CellSpacing"] = (int)value;
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [NotifyParentPropertyAttribute(true)]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        //[System.Web.WebCategoryAttribute("Styles")]
        public TableItemStyle DayHeaderStyle
        {
            get
            {
                if (dayHeaderStyle == null)
                {
                    dayHeaderStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)dayHeaderStyle).TrackViewState();
                }
                return dayHeaderStyle;
            }
        }

        //[System.Web.WebCategoryAttribute("Appearance")]
        public DayNameFormat DayNameFormat
        {
            get
            {
                object dnf = ViewState["DayNameFormat"];
                return ((dnf == null) ? DayNameFormat.Short : (DayNameFormat)dnf);
            }
            set
            {
                if (value < DayNameFormat.Full || value > DayNameFormat.Shortest)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                ViewState["DayNameFormat"] = value;
            }
        }

        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        [NotifyParentPropertyAttribute(true)]
        //[System.Web.WebCategoryAttribute("Styles")]
        [DefaultValueAttribute("")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        public TableItemStyle DayStyle
        {
            get
            {
                if (dayStyle == null)
                {
                    dayStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)dayStyle).TrackViewState();
                }
                return dayStyle;
            }
        }

        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        [NotifyParentPropertyAttribute(true)]
        //[System.Web.WebCategoryAttribute("Styles")]
        [DefaultValueAttribute("")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        public Style DayNumberStyle
        {
            get
            {
                if (dayNumberStyle == null)
                {
                    dayNumberStyle = new Style();
                    if (IsTrackingViewState)
                        ((IStateManager)dayNumberStyle).TrackViewState();
                }
                return dayNumberStyle;
            }
        }

        //[System.Web.WebCategoryAttribute("Appearance")]
        public FirstDayOfWeek FirstDayOfWeek
        {
            get
            {
                object o = ViewState["FirstDayOfWeek"];
                return ((o == null) ? FirstDayOfWeek.Default : (FirstDayOfWeek)o);
            }
            set
            {
                if (value < FirstDayOfWeek.Sunday || value > FirstDayOfWeek.Default)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                ViewState["FirstDayOfWeek"] = value;
            }
        }

        [NotifyParentPropertyAttribute(true)]
        //[System.Web.WebCategoryAttribute("Styles")]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        public TableItemStyle NextPrevStyle
        {
            get
            {
                if (nextPrevStyle == null)
                {
                    nextPrevStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)nextPrevStyle).TrackViewState();
                }
                return nextPrevStyle;
            }
        }

        [DefaultValueAttribute("")]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [NotifyParentPropertyAttribute(true)]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        //[System.Web.WebCategoryAttribute("Styles")]
        public TableItemStyle OtherMonthDayStyle
        {
            get
            {
                if (otherMonthDayStyle == null)
                {
                    otherMonthDayStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)otherMonthDayStyle).TrackViewState();

                }
                return otherMonthDayStyle;
            }
        }

        //[System.Web.WebCategoryAttribute("Appearance")]
        [DefaultValueAttribute(true)]
        public bool ShowDayHeader
        {
            get
            {
                object b = ViewState["ShowDayHeader"];
                return ((b == null) ? true : (bool)b);
            }
            set
            {
                ViewState["ShowDayHeader"] = value;
            }
        }

        //[System.Web.WebCategoryAttribute("Appearance")]
        [DefaultValueAttribute(false)]
        public bool ShowGridLines
        {
            get
            {
                object b = ViewState["ShowGridLines"];
                return ((b == null) ? false : (bool)b);
            }
            set
            {
                ViewState["ShowGridLines"] = value;
            }
        }

        //[System.Web.WebCategoryAttribute("Appearance")]
        [DefaultValueAttribute(true)]
        public bool ShowTitle
        {
            get
            {
                object b = ViewState["ShowTitle"];
                return ((b == null) ? true : (bool)b);
            }
            set
            {
                ViewState["ShowTitle"] = value;
            }
        }

        //[System.Web.WebCategoryAttribute("Styles")]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [NotifyParentPropertyAttribute(true)]
        public TableItemStyle TitleStyle
        {
            get
            {
                if (titleStyle == null)
                {
                    titleStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)titleStyle).TrackViewState();
                }
                return titleStyle;
            }
        }

        [NotifyParentPropertyAttribute(true)]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        //[System.Web.WebCategoryAttribute("Styles")]
        [DefaultValue(null)]
        public TableItemStyle TodayDayStyle
        {
            get
            {
                if (todayDayStyle == null)
                {
                    todayDayStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)todayDayStyle).TrackViewState();
                }
                return todayDayStyle;
            }
        }

        [
     DefaultValue(true)
     ]
        public virtual bool UseAccessibleHeader
        {
            get
            {
                object o = ViewState["UseAccessibleHeader"];
                return (o != null) ? (bool)o : true;
            }
            set
            {
                ViewState["UseAccessibleHeader"] = value;
            }
        }


        [BindableAttribute(true)]
        [DefaultValueAttribute(typeof(DateTime), "1/1/0001")]
        public DateTime VisibleDate
        {
            get
            {
                object o = ViewState["VisibleDate"];
                return ((o == null) ? DateTime.Now : (DateTime)o);
            }
            set
            {
                ViewState["VisibleDate"] = value.Date;
                if (Initialized)
                {
                    OnDataPropertyChanged();
                }
            }
        }

        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Content)]
        [NotifyParentPropertyAttribute(true)]
        [PersistenceModeAttribute(PersistenceMode.InnerProperty)]
        //[System.Web.WebCategoryAttribute("Styles")]
        public TableItemStyle WeekendDayStyle
        {
            get
            {
                if (weekendDayStyle == null)
                {
                    weekendDayStyle = new TableItemStyle();
                    if (IsTrackingViewState)
                        ((IStateManager)weekendDayStyle).TrackViewState();
                }
                return weekendDayStyle;
            }
        }



        /*********************************************************************************
        * 
        * ViewState control
        * 
        *********************************************************************************/

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] myState = (object[])savedState;

                if (myState[0] != null)
                    base.LoadViewState(myState[0]);
                if (myState[1] != null)
                    ((IStateManager)TitleStyle).LoadViewState(myState[1]);
                if (myState[2] != null)
                    ((IStateManager)NextPrevStyle).LoadViewState(myState[2]);
                if (myState[3] != null)
                    ((IStateManager)DayStyle).LoadViewState(myState[3]);
                if (myState[4] != null)
                    ((IStateManager)DayHeaderStyle).LoadViewState(myState[4]);
                if (myState[5] != null)
                    ((IStateManager)TodayDayStyle).LoadViewState(myState[5]);
                if (myState[6] != null)
                    ((IStateManager)WeekendDayStyle).LoadViewState(myState[6]);
                if (myState[7] != null)
                    ((IStateManager)OtherMonthDayStyle).LoadViewState(myState[7]);
                if (myState[8] != null)
                    ((IStateManager)DayNumberStyle).LoadViewState(myState[8]);
            }
        }

        protected override object SaveViewState()
        {
            object[] myState = new object[9];

            myState[0] = base.SaveViewState();
            myState[1] = (titleStyle != null) ? ((IStateManager)titleStyle).SaveViewState() : null;
            myState[2] = (nextPrevStyle != null) ? ((IStateManager)nextPrevStyle).SaveViewState() : null;
            myState[3] = (dayStyle != null) ? ((IStateManager)dayStyle).SaveViewState() : null;
            myState[4] = (dayHeaderStyle != null) ? ((IStateManager)dayHeaderStyle).SaveViewState() : null;
            myState[5] = (todayDayStyle != null) ? ((IStateManager)todayDayStyle).SaveViewState() : null;
            myState[6] = (weekendDayStyle != null) ? ((IStateManager)weekendDayStyle).SaveViewState() : null;
            myState[7] = (otherMonthDayStyle != null) ? ((IStateManager)otherMonthDayStyle).SaveViewState() : null;
            myState[8] = (dayNumberStyle != null) ? ((IStateManager)dayNumberStyle).SaveViewState() : null;

            for (int i = 0; i < myState.Length; i++)
            {
                if (myState[i] != null)
                    return myState;
            }
            return null;
        }

        //Tell the styles that they need to track changes
        protected override void TrackViewState()
        {
            base.TrackViewState();

            if (titleStyle != null)
                ((IStateManager)titleStyle).TrackViewState();
            if (nextPrevStyle != null)
                ((IStateManager)nextPrevStyle).TrackViewState();
            if (dayStyle != null)
                ((IStateManager)dayStyle).TrackViewState();
            if (dayHeaderStyle != null)
                ((IStateManager)dayHeaderStyle).TrackViewState();
            if (todayDayStyle != null)
                ((IStateManager)todayDayStyle).TrackViewState();
            if (weekendDayStyle != null)
                ((IStateManager)weekendDayStyle).TrackViewState();
            if (otherMonthDayStyle != null)
                ((IStateManager)otherMonthDayStyle).TrackViewState();
            if (DayNumberStyle != null)
                ((IStateManager)dayNumberStyle).TrackViewState();
        }


        /*********************************************************************************
        * 
        * Proxy events for the calendar control
        * 
        *********************************************************************************/

        //[System.Web.WebCategoryAttribute("Action")]
        public event MonthChangedEventHandler VisibleMonthChanged;

    }
}
