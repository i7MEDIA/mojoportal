using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web.Controls
{
    public class DayNumberDiv : WebControl
    {
        private string m_innerText;
        private HyperLink link;

        public DayNumberDiv(CalendarDay d, string linkFormat)
            : base(HtmlTextWriterTag.Div)
        {
            if ((linkFormat != null) && (linkFormat.Length > 0))
            {
                link = new HyperLink();
                link.Text = d.DayNumberText;
                link.NavigateUrl = string.Format(CultureInfo.InvariantCulture,
                    linkFormat, d.Date.ToString("s"));
                link.ToolTip = string.Empty;

            }
            else
            {
                m_innerText = d.DayNumberText;
            }
        }

        public DayNumberDiv(string innerText)
            : base(HtmlTextWriterTag.Div)
        {
            //
            // TODO: Add constructor logic here
            //
            m_innerText = innerText;
        }

        public DayNumberDiv()
            : base(HtmlTextWriterTag.Div)
        {
            //
            // TODO: Add constructor logic here
            //
            m_innerText = null;
        }

        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (m_innerText != null) writer.WriteEncodedText(m_innerText);
            if (link != null) link.RenderControl(writer);
            base.RenderContents(writer);
        }
    }
}
