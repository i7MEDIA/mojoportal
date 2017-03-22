using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class Link : HyperLink
	{
		private string linkClass = string.Empty;
		public string LinkClass
		{
			get { return linkClass; }
			set { linkClass = value; }
		}

		private string outsideTopMarkup = string.Empty;
		public string OutsideTopMarkup
		{
			get { return outsideTopMarkup; }
			set { outsideTopMarkup = value; }
		}

		private string insideTopMarkup = string.Empty;
		public string InsideTopMarkup
		{
			get { return insideTopMarkup; }
			set { insideTopMarkup = value; }
		}

		private string insideBottomMarkup = string.Empty;
		public string InsideBottomMarkup
		{
			get { return insideBottomMarkup; }
			set { insideBottomMarkup = value; }
		}

		private string outsideBottomMarkup = string.Empty;
		public string OutsideBottomMarkup
		{
			get { return outsideBottomMarkup; }
			set { outsideBottomMarkup = value; }
		}

		private bool renderId = false;
		public bool RenderId
		{
			get { return renderId; }
			set { renderId = value; }
		}


		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (linkClass.Length > 0)
			{
				if (CssClass.Length > 0)
				{
					if (!CssClass.Contains(linkClass))
					{
						CssClass = linkClass + " " + CssClass;
					}
				}
				else
				{
					CssClass = linkClass;
				}
			}
		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (outsideTopMarkup.Length > 0)
			{
				writer.Write(outsideTopMarkup);
			}

			if (renderId)
			{
				writer.Write("<a id='" + ClientID + "' class='" + CssClass + "' href='" + NavigateUrl + "'>");
			}
			else
			{
				writer.Write("<a class='" + CssClass + "' href='" + NavigateUrl + "'>");
			}

			if (insideTopMarkup.Length > 0)
			{
				writer.Write(insideTopMarkup);
			}

			base.RenderContents(writer);

			if (insideBottomMarkup.Length > 0)
			{
				writer.Write(insideBottomMarkup);
			}

			writer.Write("</a>");

			if (outsideBottomMarkup.Length > 0)
			{
				writer.Write(outsideBottomMarkup);
			}
		}
	}
}