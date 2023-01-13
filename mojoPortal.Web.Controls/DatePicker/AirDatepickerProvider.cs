using System.Collections.Specialized;

namespace mojoPortal.Web.Controls.DatePicker
{
	public class AirDatepickerProvider : DatePickerProvider
	{
		public override IDatePicker GetDatePicker()
		{
			return new AirDatepickerAdapter();
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			base.Initialize(name, config);
		}
	}
}
