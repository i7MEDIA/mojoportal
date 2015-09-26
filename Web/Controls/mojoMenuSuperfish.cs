using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// update 2011-03-17 this menu is now obsolete and only kept for backward comaptibility
    /// the functionality provided by this menu can be replicated using mojoMenu and setting menu configuration from theme.skin
    /// 
    /// the only purpose of this class is to hookup the correct menu adapter in CSSFriendly.browser
    /// </summary>
    public class mojoMenuSuperfish : Menu
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

        protected override void OnLoad(EventArgs e)
        {
            //this is needed under .NET 4 because the menu javscript otherwise adds events for mouseover that interfere with the superfish mouseover transitions
#if !NET35
            this.Enabled = false;
#endif
            base.OnLoad(e);
            
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

#if !NET35
            IncludeStyleBlock = false;

            // this doesn't really make it render as a table since we use the css adapters
            // but it does turn off the ASP.NET 4 menu javascript that we don't want
            // because the javascript adds hard coded inlne styles.
            RenderingMode = MenuRenderingMode.Table;


#endif
        }
    }
}
