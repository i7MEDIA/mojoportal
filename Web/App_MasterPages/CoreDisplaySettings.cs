using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class CoreDisplaySettings : WebControl
	{
		/// <summary>
		/// Use h1 only if your skin doesn't use an h1 around the site name.
		/// </summary>
		public string DefaultPageHeaderMarkupTop { get; set; } = "<h2>";
		public string DefaultPageHeaderMarkupBottom { get; set; } = "</h2>";


		public string AlertNoticeMarkup { get; set; } = "<div class='alert alert-info'>{0}</div>";
		public string AlertErrorMarkup { get; set; } = "<div class='alert alert-danger'>{0}</div>";
		public string AlertWarningMarkup { get; set; } = "<div class='alert alert-warning'>{0}</div>";


	}
}