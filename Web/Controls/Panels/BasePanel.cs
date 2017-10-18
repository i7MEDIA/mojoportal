// Author:					
// Created:				    2011-05-20
// Last Modified:			2017-09-07
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

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
		private string element = "div";

		public string Element
		{
			get { return element; }
			set { element = value; }
		}

		private string extraCssClasses = string.Empty;

		public string ExtraCssClasses
		{
			get { return extraCssClasses; }
			set { extraCssClasses = value; }
		}

		private bool renderContentsOnly = false;

		public bool RenderContentsOnly
		{
			get { return renderContentsOnly; }
			set { renderContentsOnly = value; }
		}

		private string insideTopMarkup = string.Empty;

		public string InsideTopMarkup
		{
			get { return insideTopMarkup; }
			set { insideTopMarkup = value; }
		}

		[Obsolete("Use InsideTopMarkup instead.")]
		public string LiteralExtraTopContent
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

		[Obsolete("Use InsideBottomMarkup instead.")]
		public string LiteralExtraBottomContent
		{
			get { return insideBottomMarkup; }
			set { insideBottomMarkup = value; }
		}


		private string outsideTopMarkup = string.Empty;
		public string OutsideTopMarkup
		{
			get => outsideTopMarkup;
			set => outsideTopMarkup = value;
		}

		private string outsideBottomMarkup = string.Empty;
		public string OutsideBottomMarkup
		{
			get => outsideBottomMarkup;
			set => outsideBottomMarkup = value;
		}

		private bool detectSideColumn = false;

		public bool DetectSideColumn
		{
			get { return detectSideColumn; }
			set { detectSideColumn = value; }
		}

		private string columnId = UIHelper.CenterColumnId;

		private string sideColumnxtraCssClasses = string.Empty;

		public string SideColumnxtraCssClasses
		{
			get { return sideColumnxtraCssClasses; }
			set { sideColumnxtraCssClasses = value; }
		}

		private string sideColumnLiteralExtraTopContent = string.Empty;

		public string SideColumnLiteralExtraTopContent
		{
			get { return sideColumnLiteralExtraTopContent; }
			set { sideColumnLiteralExtraTopContent = value; }
		}

		private string sideColumnLiteralExtraBottomContent = string.Empty;

		public string SideColumnLiteralExtraBottomContent
		{
			get { return sideColumnLiteralExtraBottomContent; }
			set { sideColumnLiteralExtraBottomContent = value; }
		}

		private bool renderId = true;

		public virtual bool RenderId
		{
			get { return renderId; }
			set { renderId = value; }
		}

		private bool dontRender = false;

		public bool DontRender
		{
			get { return dontRender; }
			set { dontRender = value; }
		}

		private bool autohide = false;

		public bool Autohide
		{
			get => autohide;
			set => autohide = value;
		}

		private int countOfVisibleWebControls = 0;

		protected override void OnPreRender(EventArgs e)
		{
			if (HttpContext.Current == null)
			{
				base.OnPreRender(e);
				return;
			}

			countOfVisibleWebControls = GetCountVisibleChildWebControls();
			Visible = !(autohide && countOfVisibleWebControls == 0);

			if (detectSideColumn)
			{
				columnId = this.GetColumnId();

				switch (columnId)
				{
					case UIHelper.LeftColumnId:
					case UIHelper.RightColumnId:
						extraCssClasses = sideColumnxtraCssClasses;
						insideTopMarkup = sideColumnLiteralExtraTopContent;
						insideBottomMarkup = sideColumnLiteralExtraBottomContent;

						break;

					case UIHelper.CenterColumnId:
					default:
						// nothing to do here

						break;
				}

				base.OnPreRender(e);
			}

			
			if (extraCssClasses.Length > 0)
			{
				if (CssClass.Length > 0)
				{
					if (!CssClass.Contains(extraCssClasses))
					{
						CssClass = CssClass + " " + extraCssClasses;
					}
				}
				else
				{
					CssClass = extraCssClasses;
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

			if (DontRender)
			{
				return;
			}

			countOfVisibleWebControls = GetCountVisibleChildWebControls();

			if (autohide && countOfVisibleWebControls == 0)
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
					writer.Write("<" + Element + " id='" + ClientID + "'");

					if (!string.IsNullOrWhiteSpace(CssClass))
					{
						writer.Write(" class='" + CssClass + "'");
					}

					writer.Write(">\n");
				}
				else
				{
					writer.Write("<" + Element);

					if (!string.IsNullOrWhiteSpace(CssClass))
					{
						writer.Write(" class='" + CssClass + "'");
					}

					writer.Write(">\n");
				}
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
				writer.Write("\n</" + element + ">");
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
				if ((c is WebControl) && c.Visible)
				{
					return 1;
				}

				if (c is ContentPlaceHolder)
				{
					foreach (Control child in c.Controls)
					{
						if ((child is WebControl) && child.Visible)
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