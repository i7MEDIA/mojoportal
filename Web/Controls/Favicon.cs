using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI;

public class Favicon : WebControl
{
    protected override void Render(HtmlTextWriter writer)
    {
        writer.Write($"\n<link rel=\"shortcut icon\" href=\"{SiteUtils.DetermineSkinBaseUrl(true, Page)}favicon.ico\" />");
    }
}

