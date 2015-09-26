using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public partial class NeatUploadStylesheet : System.Web.UI.UserControl
    {
        protected Literal literal = new Literal();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            literal.Text = "<style type=\"text/css\">@import url("
                    + SiteUtils.GetSkinBaseUrl(Page)
                    + "neatupload.css" + ");</style>\n";

            this.Controls.Add(literal);

            

        }
    }
}