using System;
using System.Globalization;
using System.Web.UI.Design;

namespace mojoPortal.Web.Editor
{
    public class XStandardDesigner : ControlDesigner
    {
        public XStandardDesigner()
        {
        }

        public override string GetDesignTimeHtml()
        {
            //XStandardControl control = (XStandardControl)Component;
            //return String.Format(CultureInfo.InvariantCulture,
            //    "<div><table width=\"{0}\" height=\"{1}\" bgcolor=\"#f5f5f5\" bordercolor=\"#c7c7c7\" cellpadding=\"0\" cellspacing=\"0\" border=\"1\"><tr><td valign=\"middle\" align=\"center\">XStandard<b>{2}</b></td></tr></table></div>",
            //        control.Width,
            //        control.Height,
            //        control.ID);
            return "<div>XStandard</div>";

        }
    }
}
