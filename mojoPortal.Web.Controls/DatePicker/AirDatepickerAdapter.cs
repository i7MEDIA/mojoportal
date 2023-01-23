using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.DatePicker
{
	public class AirDatepickerAdapter : IDatePicker
	{
		private AirDatepicker control;

		public AirDatepickerAdapter()
		{
			InitializeAdapter();
		}

		public string ControlID
		{
			get { return control.ID; }
			set { control.ID = value; }
		}

		public string Text
		{
			get { return control.Text; }
			set { control.Text = value; }
		}

		public Unit Width
		{
			get { return control.Width; }
			set { control.Width = value; }
		}

		public bool ShowTime
		{
			get { return control.ShowTime; }
			set { control.ShowTime = value; }
		}
		public bool ShowTimeOnly
		{
			get { return control.ShowTimeOnly; }
			set { control.ShowTimeOnly = value; }
		}
		public string ClockHours
		{
			get { return control.ClockHours; }
			set { control.ClockHours = value; }
		}

		public bool ShowMonthList
		{
			get { return control.ChangeMonth; }
			set { control.ChangeMonth = value; }
		}

		public bool ShowYearList
		{
			get { return control.ChangeYear; }
			set { control.ChangeYear = value; }
		}

		public string YearRange
		{
			get { return control.YearRange; }
			set { control.YearRange = value; }
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

		public bool KeyboardNav
		{
			get { return control.KeyboardNav; }
			set { control.KeyboardNav = value; }
		}

		public string View 
		{ 
			get { return control.View; }
			set { control.View = value; }
		}

		public string MinView
		{
			get { return control.MinView; }
			set { control.MinView = value; }
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
			control = new AirDatepicker();
		}

		public Control GetControl()
		{
			return control;
		}
	}
}
