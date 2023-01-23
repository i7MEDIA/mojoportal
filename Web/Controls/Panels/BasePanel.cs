using mojoPortal.Web.Framework;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// primarily a base class for other panel controls
	/// by sub classing we can configure properties on different sub classes differently from theme.skin file
	/// this technique allows a nice way to use additional markup or less markup as needed
	/// to support different versions of Artisteer skins, jqueryui skin, and makes it possible to use html 5 structural elements like article
	/// and is intended to help us with our mobile strategy by making the markup rendering more flexible
	/// </summary>
	public class BasePanel : Panel
	{
		public string Element { get; set; } = "div";
		public string ExtraCssClasses { get; set; } = string.Empty;
		public bool RenderContentsOnly { get; set; } = false;
		public string InsideTopMarkup { get; set; } = string.Empty;
		[Obsolete("Use InsideTopMarkup instead.")]
		public string LiteralExtraTopContent
		{
			get { return InsideTopMarkup; }
			set { InsideTopMarkup = value; }
		}
		public string InsideBottomMarkup { get; set; } = string.Empty;
		[Obsolete("Use InsideBottomMarkup instead.")]
		public string LiteralExtraBottomContent
		{
			get { return InsideBottomMarkup; }
			set { InsideBottomMarkup = value; }
		}
		public string OutsideTopMarkup { get; set; } = string.Empty;
		public string OutsideBottomMarkup { get; set; } = string.Empty;
		public bool DetectSideColumn { get; set; } = false;
		public string SideColumnxtraCssClasses { get; set; } = string.Empty;
		public string SideColumnLiteralExtraTopContent { get; set; } = string.Empty;
		public string SideColumnLiteralExtraBottomContent { get; set; } = string.Empty;
		public virtual bool RenderId { get; set; } = true;
		public bool DontRender { get; set; } = false;
		public bool Autohide { get; set; } = false;


		private int countOfVisibleWebControls = 0;
		private string columnId = UIHelper.CenterColumnId;


		protected override void OnPreRender(EventArgs e)
		{
			if (HttpContext.Current == null)
			{
				base.OnPreRender(e);

				return;
			}

			countOfVisibleWebControls = GetCountVisibleChildWebControls();
			Visible = !(Autohide && countOfVisibleWebControls == 0);

			if (DetectSideColumn)
			{
				columnId = this.GetColumnId();

				switch (columnId)
				{
					case UIHelper.LeftColumnId:
					case UIHelper.RightColumnId:
						ExtraCssClasses = SideColumnxtraCssClasses;
						InsideTopMarkup = SideColumnLiteralExtraTopContent;
						InsideBottomMarkup = SideColumnLiteralExtraBottomContent;

						break;

					case UIHelper.CenterColumnId:
					default:
						// nothing to do here

						break;
				}

				base.OnPreRender(e);
			}

			if (ExtraCssClasses.Length > 0)
			{
				if (CssClass.Length > 0)
				{
					if (!CssClass.Contains(ExtraCssClasses))
					{
						CssClass = CssClass + " " + ExtraCssClasses;
					}
				}
				else
				{
					CssClass = ExtraCssClasses;
				}
			}
		}


		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write($"[{ID}]");

				return;
			}

			if (DontRender)
			{
				return;
			}

			countOfVisibleWebControls = GetCountVisibleChildWebControls();

			if (Autohide && countOfVisibleWebControls == 0)
			{
				return;
			}

			if (OutsideTopMarkup.Length > 0)
			{
				writer.Write(OutsideTopMarkup);
			}

			if (!RenderContentsOnly)
			{
				if (RenderId)
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Id, ClientID.ToString());
				}

				if (!string.IsNullOrWhiteSpace(CssClass))
				{
					writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass);
				}

				writer.RenderBeginTag(Element);
			}

			if (InsideTopMarkup.Length > 0)
			{
				writer.Write(InsideTopMarkup);
			}

			base.RenderContents(writer);

			if (InsideBottomMarkup.Length > 0)
			{
				writer.Write(InsideBottomMarkup);
			}

			if (!RenderContentsOnly)
			{
				writer.RenderEndTag();
			}

			if (OutsideBottomMarkup.Length > 0)
			{
				writer.Write(OutsideBottomMarkup);
			}
		}


		private int GetCountVisibleChildWebControls()
		{
			foreach (Control c in Controls)
			{
				if (c is WebControl && c.Visible)
				{
					return 1;
				}

				if (c is ContentPlaceHolder)
				{
					foreach (Control child in c.Controls)
					{
						if (child is WebControl && child.Visible)
						{
							return 1;
						}
					}
				}
			}

			return 0;
		}
	}
}
