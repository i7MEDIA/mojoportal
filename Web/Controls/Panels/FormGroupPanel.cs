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

		private string groupClass = "settingrow";
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
				writer.Write("[" + ID + "]");
				return;
			}

			if (renderGroupElement)
			{
				if (renderId)
				{
					writer.Write("<");
					writer.Write(groupElement);
					writer.Write(" id='" + ClientID + "' class='" + CssClass + "'");
					writer.Write(">");
				}
				else
				{
					writer.Write("<");
					writer.Write(groupElement);
					if (CssClass.Length > 0)
						writer.Write(" class='" + CssClass + "'");
					writer.Write(">");
				}
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

			if (renderGroupElement)
			{
				writer.Write("</" + groupElement + ">");
			}
		}
	}
}