using System;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class mojoTextBox : TextBox
	{
		public string LiteralTopMarkup { get; set; } = string.Empty;

		public string LiteralBottomMarkup { get; set; } = string.Empty;


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (HttpContext.Current == null) { return; }
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			if (LiteralTopMarkup.Length > 0)
			{
				writer.Write(LiteralTopMarkup);
			}

			base.Render(writer);

			if (LiteralBottomMarkup.Length > 0)
			{
				writer.Write(LiteralBottomMarkup);
			}
		}
	}
}