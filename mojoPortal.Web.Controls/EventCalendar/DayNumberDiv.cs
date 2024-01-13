using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls;

public class DayNumberDiv : WebControl
{
	private readonly string m_innerText;
	private readonly HyperLink link;

	public DayNumberDiv(CalendarDay d, string linkFormat) : base(HtmlTextWriterTag.Div)
	{
		if ((linkFormat != null) && (linkFormat.Length > 0))
		{
			link = new HyperLink
			{
				Text = d.DayNumberText,
				NavigateUrl = string.Format(CultureInfo.InvariantCulture,
				linkFormat, d.Date.ToString("s")),
				ToolTip = string.Empty
			};
		}
		else
		{
			m_innerText = d.DayNumberText;
		}
	}

	public DayNumberDiv(string innerText) : base(HtmlTextWriterTag.Div)
	{
		m_innerText = innerText;
	}

	public DayNumberDiv() : base(HtmlTextWriterTag.Div)
	{
		m_innerText = null;
	}

	protected override void RenderContents(HtmlTextWriter writer)
	{
		if (m_innerText != null) writer.WriteEncodedText(m_innerText);
		link?.RenderControl(writer);
		base.RenderContents(writer);
	}
}
