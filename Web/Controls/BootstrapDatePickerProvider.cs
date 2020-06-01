using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using mojoPortal.Web.Controls.DatePicker;
namespace mojoPortal.Web.UI
{
	public class BootstrapDatePickerProvider : DatePickerProvider
	{
		public override IDatePicker GetDatePicker()
		{
			return new BootstrapDatePickerAdapter();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
		}
	}
}