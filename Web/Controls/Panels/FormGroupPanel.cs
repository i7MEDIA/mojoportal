using mojoPortal.Web.Framework;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public class FormGroupPanel : Panel
	{
		private string groupElement = "div";
		public string GroupElement
		{
			get { return groupElement; }
			set { groupElement = value; }
		}

		private string groupClass = string.Empty;
		public string GroupClass
		{
			get { return groupClass; }
			set { groupClass = value; }
		}

		private bool renderGroupElement = true;
		public bool RenderGroupElement
		{
			get { return renderGroupElement; }
			set { renderGroupElement = value; }
		}

		private string literalTopMarkup = string.Empty;
		public string LiteralTopMarkup
		{
			get { return literalTopMarkup; }
			set { literalTopMarkup = value; }
		}

		private string literalBottomMarkup = string.Empty;
		public string LiteralBottomMarkup
		{
			get { return literalBottomMarkup; }
			set { literalBottomMarkup = value; }
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

			if (groupClass.Length > 0)
			{
				if (CssClass.Length > 0)
				{
					if (!CssClass.Contains(groupClass))
					{
						CssClass = groupClass + " " + CssClass;
					}
				}
				else
				{
					CssClass = groupClass;
				}
			}

		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + this.ID + "]");
				return;
			}

			if (renderGroupElement)
			{
				if (renderId)
				{
					writer.Write("<");
					writer.Write(groupElement);
					writer.Write(" id='" + this.ClientID + "' class='" + CssClass + "'>");
				}
				else
				{
					writer.Write("<");
					writer.Write(groupElement);
					writer.Write(" class='" + CssClass + "'>");
				}
			}

			if (literalTopMarkup.Length > 0)
			{
				writer.Write(literalTopMarkup);
			}

			base.RenderContents(writer);

			if (literalBottomMarkup.Length > 0)
			{
				writer.Write(literalBottomMarkup);
			}

			if (renderGroupElement)
			{
				writer.Write("</" + groupElement + ">");
			}
		}
	}
}