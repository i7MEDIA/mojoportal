using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	[ParseChildren(false)]
	public class TabsListItem : WebControl
	{
		protected override HtmlTextWriterTag TagKey
		{
			get
			{
				return HtmlTextWriterTag.Li;
			}
		}

		private string dataAttributes = string.Empty;
		public string DataAttributes
		{
			get { return dataAttributes; }
			set { dataAttributes = value; }
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

		private string customClass = string.Empty;
		public string CustomClass
		{
			get { return customClass; }
			set { customClass = value; }
		}



		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			if (customClass.Length > 0)
			{
				if (CssClass.Length > 0)
				{
					if (!CssClass.Contains(customClass))
					{
						CssClass = CssClass + " " + customClass;
					}
				}
				else
				{
					CssClass = customClass;
				}
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (dataAttributes.Length > 0)
			{
				Dictionary<string, string> attributes = dataAttributes.Split(',')
					.Select(v => v.Split(':'))
					.ToDictionary(pair => pair[0], pair => pair[1]);

				foreach (KeyValuePair<string, string> attribute in attributes)
				{
					Attributes.Add(attribute.Key, attribute.Value);
				}
			}

			Literal literalInsideTop = new Literal();
			literalInsideTop.Text = insideTopMarkup;
			Controls.Add(literalInsideTop);

			Literal literalInsideBottom = new Literal();
			literalInsideBottom.Text = insideBottomMarkup;
			Controls.Add(literalInsideBottom);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (outsideTopMarkup.Length > 0)
				writer.Write(outsideTopMarkup);

			base.Render(writer);

			if (outsideBottomMarkup.Length > 0)
				writer.Write(outsideBottomMarkup);
		}
	}
}