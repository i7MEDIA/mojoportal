#if !MONO

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Web.Preview.UI.Controls.WebParts;

namespace mojoPortal.Features.UI
{
   
    public class mojoWebPartManager : Microsoft.Web.Preview.UI.Controls.WebParts.WebPartManager
    {
        public void AddDynamicWebPart(
            WebPart webPart,
            Microsoft.Web.Preview.UI.Controls.WebParts.WebPartZone webPartZone,
            int zoneIndex, bool isShared)
        {
            if (!this.WebParts.Contains(webPart))
            {
                Internals.SetZoneID(webPart, webPartZone.ID);
                Internals.SetIsShared(webPart, isShared);
                Internals.SetZoneIndex(webPart, zoneIndex);
                Internals.AddWebPart(webPart);
                this.SetPersonalizationDirty();

            }

        }

        public void SetDirty()
        {
            SetPersonalizationDirty();
        }

    }
}

#endif
