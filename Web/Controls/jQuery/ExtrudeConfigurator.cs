//	Created:			    2010-01-03
//	Last Modified:		    2010-01-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a control that makes it easy to use the jQuery extruder plugin in an ASP.NET page
    /// http://plugins.jquery.com/project/mbextruder
    /// http://code.google.com/p/mbideasproject/wiki/mbExtruder
    /// you configure all the properties and it sets up the javascript.
    /// most important property is selector which tells it which html element(s) to wire up 
    /// or alternatively you can set a ControlID with the server side id of an asp.net panel for example
    /// </summary>
    public class ExtrudeConfigurator : WebControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            //base.RenderContents(writer);
            //no need to render anything just setup scripts
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (WebConfigSettings.DisablejQueryUI) { return; }

            if (controlId.Length > 0)
            {
                controlToWire = Page.FindControl(controlId);
                if (controlToWire == null) { controlToWire = Page.Master.FindControl(controlId); }
            }

            if ((controlToWire == null) && (selector.Length == 0)) { return; }

            SetupMainScripts();
            SetupInstanceScript();
        }

        private Control controlToWire = null;

        private string controlId = string.Empty;
        public string ControlID
        {
            get { return controlId; }
            set { controlId = value; }
        }

        private string selector = string.Empty;
        public string Selector
        {
            get { return selector; }
            set { selector = value; }
        }

        private bool positionFixed = true;
        public bool PositionFixed
        {
            get { return positionFixed; }
            set { positionFixed = value; }
        }

        private int panelWidth = 350;
        /// <summary>
        /// the width of the sliding panel
        /// </summary>
        public int PanelWidth
        {
            get { return panelWidth; }
            set { panelWidth = value; }
        }

        private int sensibility = 800;
        /// <summary>
        /// the time in milliseconds to wait befor the slider will open without a click event
        /// </summary>
        public int Sensibility
        {
            get { return sensibility; }
            set { sensibility = value; }
        }

        private string position = "top";
        /// <summary>
        /// top or left is all that is supported
        /// </summary>
        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private int flapDim = 100;
        /// <summary>
        /// the width or height (depends on the extruder position) of the flap containing the title
        /// </summary>
        public int FlapDim
        {
            get { return flapDim; }
            set { flapDim = value; }
        }

        private int flapLeftTop = 0;
        /// <summary>
        /// when using position = left, this determines the position of the flap vs the top of the page. 
        /// This setting si not part of the original extruder but was added by 
        /// </summary>
        public int FlapLeftTop
        {
            get { return flapLeftTop; }
            set { flapLeftTop = value; }
        }

        private string opacity = ".8";
        /// <summary>
        /// a number between 0 and 1 expressed as a string like .8
        /// </summary>
        public string Opacity
        {
            get { return opacity; }
            set { opacity = value; }
        }

        private int slideTimer = 300;
        /// <summary>
        /// the time in milliseconds for the slide transition
        /// </summary>
        public int SlideTimer
        {
            get { return slideTimer; }
            set { slideTimer = value; }
        }

        private string textOrientation = "bt";
        /// <summary>
        /// bt or tb bottom to top or top to bottom
        /// </summary>
        public string TextOrientation
        {
            get { return textOrientation; }
            set { textOrientation = value; }
        }

        private string onOpen = string.Empty;
        public string OnOpen
        {
            get { return onOpen; }
            set { onOpen = value; }
        }

        private string onClose = string.Empty;
        public string OnClose
        {
            get { return onClose; }
            set { onClose = value; }
        }

        private string onContentLoad = string.Empty;
        public string OnContentLoad
        {
            get { return onContentLoad; }
            set { onContentLoad = value; }
        }

        private void SetupInstanceScript()
        {

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append("$(function(){");

            if (controlToWire != null)
            {
                script.Append("$(\"#" + controlToWire.ClientID + "\").buildMbExtruder({");
            }
            else
            {
                script.Append("$('" + selector + "').buildMbExtruder({");
            }

            if (positionFixed)
            {
                script.Append("positionFixed:true");
            }
            else
            {
                script.Append("positionFixed:false");
            }

            script.Append(",position:\"" + position + "\"");
            script.Append(",sensibility:" + sensibility.ToInvariantString());
            script.Append(",width:" + panelWidth.ToInvariantString());

            script.Append(",flapDim:" + flapDim.ToInvariantString());
            script.Append(",flapLeftTop:" + flapLeftTop.ToInvariantString());

            //

            if ((textOrientation != "bt") && (textOrientation != "tb")) { textOrientation = "bt"; }

            script.Append(",textOrientation:\"" + textOrientation + "\"");
            script.Append(",extruderOpacity:" + opacity);

            if (onOpen.Length > 0)
            {
                script.Append(",onExtOpen:" + onOpen);
            }
            else
            {
                script.Append(",onExtOpen:function(){}");
            }

            if (onClose.Length > 0)
            {
                script.Append(",onExtClose:" + onClose);
            }
            else
            {
                script.Append(",onExtClose:function(){}");
            }

            if (onContentLoad.Length > 0)
            {
                script.Append(",onExtContentLoad:" + onContentLoad);
            }
            else
            {
                script.Append(",onExtContentLoad:function(){}");
            }
            
            script.Append(",slideTimer:" + slideTimer.ToInvariantString());

            script.Append("});");
            script.Append("});");

            script.Append("\n//--> ");
            script.Append(" </script>");

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "setup" + this.ClientID,
                script.ToString());
        }


        private void SetupMainScripts()
        {
            
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqhoverintent", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.hoverIntent.min.js") + "\" type=\"text/javascript\"></script>");
           
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqmetadata", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.metadata.js") + "\" type=\"text/javascript\"></script>");
           
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqfliptext", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/jquery.mb.flipText.min.js") + "\" type=\"text/javascript\"></script>");
           
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                     "jqextruder", "\n<script src=\""
                     + Page.ResolveUrl("~/ClientScript/jqmojo/mojo-mbExtruder.js") + "\" type=\"text/javascript\"></script>");

         }
            

    }
}
