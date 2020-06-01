using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.DatePicker;
using Resources;

namespace mojoPortal.Web.UI
{
	public class BootstrapDatePickerAdapter : IDatePicker
	{
		private BootstrapDatePicker control;

		public BootstrapDatePickerAdapter()
		{
			InitializeAdapter();
		}

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

		/// <summary>
		/// implemented only to support the interface but not really used for this datepicker
		/// </summary>
		public string ButtonImageUrl { get; set; }

		private void InitializeAdapter()
		{
			control = new BootstrapDatePicker();
			//control.DoneLabel = Resource.DoneButton;
			//control.HourLabel = Resource.Hour;
			//control.MinuteLabel = Resource.Minute;

		}

		public Control GetControl()
		{
			return control;
		}
	}
}