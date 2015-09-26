using mojoPortal.Web.Controls.DatePicker;
using System.Collections.Specialized;

namespace mojoPortal.Web.UI
{
    public class jDatePickerProvider : DatePickerProvider
    {
        public override IDatePicker GetDatePicker()
        {
            return new jDatePickerAdapter();
        }

        public override void Initialize(
            string name,
            NameValueCollection config)
        {
            base.Initialize(name, config);
            // don't read anything from config
            // here as this would raise an error under Medium Trust

        }
    }
}
