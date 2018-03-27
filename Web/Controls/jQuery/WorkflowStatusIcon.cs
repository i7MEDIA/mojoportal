using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class WorkflowStatusIcon : Image
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ImageUrl = Page.ResolveUrl("~/Data/SiteImages/information.png");
            
            if (WebConfigSettings.DisablejQueryUI) { return; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (ToolTip.Length == 0) { return; }

            string script = "$(function() { $('#" + this.ClientID + "').tooltip(); });";
            ScriptManager.RegisterStartupScript(this, typeof(WorkflowStatusIcon), this.ClientID + "tt", script, true);
            

        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (WebConfigSettings.DisablejQueryUI) { return; }

            base.Render(writer);
        }
    }
}