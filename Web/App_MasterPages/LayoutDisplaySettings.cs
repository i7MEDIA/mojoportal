// Author:					
// Created:				    2012-04-25
// Last Modified:		    2013-01-06
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This control can be added to your layout.master file just inside the body element
    /// <portal:LayoutDisplaySettings ID="LayoutDisplaySettings1" runat="server" />
    /// it must have the exact id shown above 
    /// then you can override the css classes used for column layout from theme.skin 
    /// like this example that uses the grid system from foundation http://foundation.zurb.com/docs/grid.php:
    /// 
    /// <portal:LayoutDisplaySettings runat="server" 
	///    LeftSideNoRightSideCss="four columns"
	///    RightSideNoLeftSideCss="four columns"
	///    CenterNoLeftSideCss="eight columns"
	///    CenterNoRightSideCss="eight columns"
	///    CenterNoLeftOrRightSideCss="twelve columns"
	///    CenterWithLeftAndRightSideCss="four columns"
	///    />
    /// 
    /// See the bootstrap-foundation skin for an example skin using this
    /// </summary>
    public class LayoutDisplaySettings : WebControl
    {

        private string leftSideNoRightSideCss = "art-layout-cell art-sidebar1 leftside left2column";
        /// <summary>
        /// css class(es) assigned to divLeft when only left and center are used
        /// replaces default classes on divLeft when only left and center are used
        /// </summary>
        public string LeftSideNoRightSideCss
        {
            get { return leftSideNoRightSideCss; }
            set { leftSideNoRightSideCss = value; }
        }

        private string rightSideNoLeftSideCss = "art-layout-cell art-sidebar2 rightside right2column";
        /// <summary>
        /// css class(es) assigned to divRight when only right and center are used
        /// replaces default classes on divRight when only right and center are used
        /// </summary>
        public string RightSideNoLeftSideCss
        {
            get { return rightSideNoLeftSideCss; }
            set { rightSideNoLeftSideCss = value; }
        }

        private string leftAndRightNoCenterCss = string.Empty;
        /// <summary>
        /// css class(es) assigned to divRight and divLeft when both have content
        /// and center has no content if this property is populated from theme.skin
        /// in order to not change the behavior in older skins if this proiperty is empty it is not applied
        /// added 2013-01-06
        /// </summary>
        public string LeftAndRightNoCenterCss
        {
            get { return leftAndRightNoCenterCss; }
            set { leftAndRightNoCenterCss = value; }
        }

        private string leftOnlyCss = string.Empty;
        /// <summary>
        /// css class(es) assigned to  divLeft when it is the only panel that has content
        /// generally users should just use the center column, there is no good reason to choose a side column if its going
        /// to use the full width, but this property allows you to add css if you want to handle that
        /// in order to not change the behavior in older skins if this proiperty is empty it is not applied
        /// added 2013-01-06
        /// </summary>
        public string LeftOnlyCss
        {
            get { return leftOnlyCss; }
            set { leftOnlyCss = value; }
        }

        private string rightOnlyCss = string.Empty;
        /// <summary>
        /// css class(es) assigned to  divRight when it is the only panel that has content
        /// generally users should just use the center column, there is no good reason to choose a side column if its going
        /// to use the full width, but this property allows you to add css if you want to handle that
        /// in order to not change the behavior in older skins if this proiperty is empty it is not applied
        /// added 2013-01-06
        /// </summary>
        public string RightOnlyCss
        {
            get { return rightOnlyCss; }
            set { rightOnlyCss = value; }
        }

        private string centerNoLeftSideCss = "art-layout-cell art-content center-rightmargin cmszone";
        /// <summary>
        /// css class(es) assigned to divCenter when left side has no content and is not used
        /// </summary>
        public string CenterNoLeftSideCss
        {
            get { return centerNoLeftSideCss; }
            set { centerNoLeftSideCss = value; }
        }

        private string centerNoRightSideCss = "art-layout-cell art-content center-leftmargin cmszone";
        /// <summary>
        /// css class(es) assigned to divCenter when right side has no content and is not used
        /// </summary>
        public string CenterNoRightSideCss
        {
            get { return centerNoRightSideCss; }
            set { centerNoRightSideCss = value; }
        }

        private string centerNoLeftOrRightSideCss = "art-layout-cell art-content-wide center-nomargins cmszone";
        /// <summary>
        /// css class(es) assigned to divCenter when left side and right side has no content and is not used
        /// </summary>
        public string CenterNoLeftOrRightSideCss
        {
            get { return centerNoLeftOrRightSideCss; }
            set { centerNoLeftOrRightSideCss = value; }
        }

        private string centerWithLeftAndRightSideCss = "art-layout-cell art-content-narrow center-rightandleftmargins cmszone";
        /// <summary>
        /// css class(es) assigned to divCenter when both left side and right side are used
        /// </summary>
        public string CenterWithLeftAndRightSideCss
        {
            get { return centerWithLeftAndRightSideCss; }
            set { centerWithLeftAndRightSideCss = value; }
        }

        private string emptyCenterCss = string.Empty;

        /// <summary>
        /// css class(es) assigned to divCenter if the setting is not empty and the center pane has no content but one or both side panels have content
        /// </summary>
        public string EmptyCenterCss
        {
            get { return emptyCenterCss; }
            set { emptyCenterCss = value; }
        }

        private bool hideEmptyCenterIfOnlySidesHaveContent = false;
        /// <summary>
        /// if true and the side columns have content but the center does not this will prevent rendering the div for the center
        /// </summary>
        public bool HideEmptyCenterIfOnlySidesHaveContent
        {
            get { return hideEmptyCenterIfOnlySidesHaveContent; }
            set { hideEmptyCenterIfOnlySidesHaveContent = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // nothing to render this is just used as a property bag that can be configured from theme.skin
        }
    }
}