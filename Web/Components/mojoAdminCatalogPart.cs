using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace mojoPortal.Web
{
    public class MojoAdminCatalogPart : CatalogPart
    {

        public override WebPartDescriptionCollection GetAvailableWebPartDescriptions()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override WebPart GetWebPart(WebPartDescription description)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
