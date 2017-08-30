using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.ContactUI
{
	public class ContactFormDisplaySettings : WebControl
	{
		private string containerClass = "container-fluid";
		public string ContainerClass
		{
			get => containerClass;
			set => containerClass = value;
		}

		private string messageListClass = "col-sm-4";
		public string MessageListClass
		{
			get => messageListClass;
			set => messageListClass = value;
		}

		private string messageViewClass = "col-sm-8";
		public string MessageViewClass
		{
			get => messageViewClass;
			set => messageViewClass = value;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + ID + "]");
				return;
			}
		}
	}
}
