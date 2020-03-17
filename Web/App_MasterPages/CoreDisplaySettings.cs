using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class CoreDisplaySettings : WebControl
	{
		public string DefaultPageHeaderMarkupTop { get; set; } = "<h1>";
		public string DefaultPageHeaderMarkupBottom { get; set; } = "</h1>";
	}
}