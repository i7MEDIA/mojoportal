using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// the only purpose of this class is to hookup the correct menu adapter in CSSFriendly.browser
    /// </summary>
    public class mojoMenu : Menu
    {
        private bool useDataRole = false;

        public bool UseDataRole
        {
            get { return useDataRole; }
            set { useDataRole = value; }
        }

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

        private bool renderMenuText = true;

        public bool RenderMenuText
        {
            get { return renderMenuText; }
            set { renderMenuText = value; }
        }
        

        private bool renderImages = true;

        public bool RenderImages
        {
            get { return renderImages; }
            set { renderImages = value; }
        }

        private string containerElement = "div";

        public string ContainerElement
        {
            get { return containerElement; }
            set { containerElement = value; }
        }

        private bool renderContainerCssClass = true;

        public bool RenderContainerCssClass
        {
            get { return renderContainerCssClass; }
            set { renderContainerCssClass = value; }
        }

        private string containerCssClass = string.Empty;

        public string ContainerCssClass
        {
            get { return containerCssClass; }
            set { containerCssClass = value; }
        }

        private bool renderCustomClassOnLi = true;

        public bool RenderCustomClassOnLi
        {
            get { return renderCustomClassOnLi; }
            set { renderCustomClassOnLi = value; }
        }

        private bool renderCustomClassOnAnchor = false;

        public bool RenderCustomClassOnAnchor
        {
            get { return renderCustomClassOnAnchor; }
            set { renderCustomClassOnAnchor = value; }
        }

        private bool renderCssClasses = true;

        public bool RenderCssClasses
        {
            get { return renderCssClasses; }
            set { renderCssClasses = value; }
        }

        private bool renderLiSelectedCss = true;

        public bool RenderLiSelectedCss
        {
            get { return renderLiSelectedCss; }
            set { renderLiSelectedCss = value; }
        }

        private bool renderAnchorSelectedCss = true;

        public bool RenderAnchorSelectedCss
        {
            get { return renderAnchorSelectedCss; }
            set { renderAnchorSelectedCss = value; }
        }

        private string ulCssClass = "AspNet-Menu";

        public string UlCssClass
        {
            get { return ulCssClass; }
            set { ulCssClass = value; }
        }

        private string liCssClassWithChildren = "AspNet-Menu-WithChildren";

        public string LiCssClassWithChildren
        {
            get { return liCssClassWithChildren; }
            set { liCssClassWithChildren = value; }
        }

        private string liCssClassWithoutChildren = "AspNet-Menu-Leaf";

        public string LiCssClassWithoutChildren
        {
            get { return liCssClassWithoutChildren; }
            set { liCssClassWithoutChildren = value; }
        }

        private string liSelectedCssClassWithChildren = "AspNet-Menu-SelectedWithChildren";

        public string LiSelectedCssClassWithChildren
        {
            get { return liSelectedCssClassWithChildren; }
            set { liSelectedCssClassWithChildren = value; }
        }

        private string liSelectedCssClassWithoutChildren = "AspNet-Menu-SelectedLeaf";

        public string LiSelectedCssClassWithoutChildren
        {
            get { return liSelectedCssClassWithoutChildren; }
            set { liSelectedCssClassWithoutChildren = value; }
        }

        private string liChildSelectedCssClass = "AspNet-Menu-ChildSelected";

        public string LiChildSelectedCssClass
        {
            get { return liChildSelectedCssClass; }
            set { liChildSelectedCssClass = value; }
        }

        private string liParentSelectedCssClass = "AspNet-Menu-ParentSelected";

        public string LiParentSelectedCssClass
        {
            get { return liParentSelectedCssClass; }
            set { liParentSelectedCssClass = value; }
        }

        private string anchorCssClass = "AspNet-Menu";

        public string AnchorCssClass
        {
            get { return anchorCssClass; }
            set { anchorCssClass = value; }
        }

        private string anchorSelectedCssClassWithChildren = "AspNet-Menu-SelectedWithChildren";

        public string AnchorSelectedCssClassWithChildren
        {
            get { return anchorSelectedCssClassWithChildren; }
            set { anchorSelectedCssClassWithChildren = value; }
        }

        private string anchorSelectedCssClassWithoutChildren = "AspNet-Menu-SelectedLeaf";

        public string AnchorSelectedCssClassWithoutChildren
        {
            get { return anchorSelectedCssClassWithoutChildren; }
            set { anchorSelectedCssClassWithoutChildren = value; }
        }

        private string anchorChildSelectedCssClass = "AspNet-Menu-ChildSelected";

        public string AnchorChildSelectedCssClass
        {
            get { return anchorChildSelectedCssClass; }
            set { anchorChildSelectedCssClass = value; }
        }

        private string anchorParentSelectedCssClass = "AspNet-Menu-ParentSelected";

        public string AnchorParentSelectedCssClass
        {
            get { return anchorParentSelectedCssClass; }
            set { anchorParentSelectedCssClass = value; }
        }

        private string innerSpanMode = string.Empty;
        /// <summary>
        /// valid values are empty string, None, Artisteer, SingleSpan, ThreeSpans
        /// </summary>
        public string InnerSpanMode
        {
            get { return innerSpanMode; }
            set { innerSpanMode = value; }
        }

        private bool useNet35Mode = false;

        public bool UseNet35Mode
        {
            get { return useNet35Mode; }
            set { useNet35Mode = value; }
        }

        private bool includeAdapterScripts = false;

        public bool IncludeAdapterScripts
        {
            get { return includeAdapterScripts; }
            set { includeAdapterScripts = value; }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
#if !NET35
            // this leaves out a style block that was rendered for the menu under .NET 2/3.5
            IncludeStyleBlock = false;

            // this doesn't really make it render as a table since we use the css adapters
            // but it does turn off the ASP.NET 4 menu javascript that we don't want
            // because the javascript adds hard coded inlne styles.
            // the down side of setting this to table is that it also restores the things we didn't like about .NET 2/3.5 Menu
            // it causes it to ignore the IncludeStyleBlock setting above and it will render
            if (useNet35Mode)
            {
                RenderingMode = MenuRenderingMode.Table;
            }

            //this is needed under .NET 4 because the menu javscript otherwise adds events for mouseover that interfere with the superfish mouseover transitions
            //this.Enabled = false;


#endif

            if (includeAdapterScripts)
            {
                mojoBasePage basePage = Page as mojoBasePage;
                if (basePage != null) { basePage.ScriptConfig.IncludeAspMenu = true; }
            }

        }

        



    }
}