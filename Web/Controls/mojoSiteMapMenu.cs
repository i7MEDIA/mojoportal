using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class mojoSiteMapMenu : Menu
    {
        private bool useMenuTooltipForCustomCss = false;
        /// <summary>
        /// there are no good ways to expand MenuItem with additional properties so we are using a property for something other than its intended purposes
        /// the MenuAdapterArtisteer is used to override the rendering and there we can use the tooltip property as a way to add a custom css class to soecific menu items.
        /// Admittedly an ugly solution but no other solutions seem feasible
        /// </summary>
        public bool UseMenuTooltipForCustomCss
        {
            get { return useMenuTooltipForCustomCss; }
            set { useMenuTooltipForCustomCss = value; }
        }

    }
}