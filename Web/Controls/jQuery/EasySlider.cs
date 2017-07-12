//	Author:                 
//  Created:			    2010-08-10
//	Last Modified:		    2010-08-10
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// http://cssglobe.com/post/5780/easy-slider-17-numeric-navigation-jquery-slider
    /// expects internally to contain a ul
    /// <ul>				
    ///			<li><a href="http://templatica.com/preview/30"><img src="http://cssglobe.com/lab/easyslider1.7/images/01.jpg" alt="Css Template Preview" /></a></li> 
    ///			<li><a href="http://templatica.com/preview/7"><img src="http://cssglobe.com/lab/easyslider1.7/images/02.jpg" alt="Css Template Preview" /></a></li> 
    ///			<li><a href="http://templatica.com/preview/25"><img src="http://cssglobe.com/lab/easyslider1.7/images/03.jpg" alt="Css Template Preview" /></a></li> 
    ///			<li><a href="http://templatica.com/preview/26"><img src="http://cssglobe.com/lab/easyslider1.7/images/04.jpg" alt="Css Template Preview" /></a></li> 
    ///			<li><a href="http://templatica.com/preview/27"><img src="http://cssglobe.com/lab/easyslider1.7/images/05.jpg" alt="Css Template Preview" /></a></li>			
	///		</ul> 
    /// </summary>
    public class EasySlider : Panel
    {
        private string scriptFile = "easySlider1.7.js";

        public string ScriptFile
        {
            get { return scriptFile; }
            set { scriptFile = value; }
        }

        private bool auto = false;

        /// <summary>
        /// This option enables automatic sliding. If you set it to true the sliding will automatically start and continue to perform untill user clicks one of the buttons.
        /// </summary>
        public bool Auto
        {
            get { return auto; }
            set { auto = value; }
        }


        private bool autoLocalize = true;
        public bool AutoLocalize
        {
            get { return autoLocalize; }
            set { autoLocalize = value; }
        }

        private string firstId = "firstBtn";
        /// <summary>
        /// Id attribute for "First" button. Default value is "firstBtn".
        /// </summary>
        public string FirstId
        {
            get { return firstId; }
            set { firstId = value; }
        }

        private string firstText = "First";
        /// <summary>
        /// Inner text for "First" button. Default value is "First"
        /// </summary>
        public string FirstText
        {
            get { return firstText; }
            set { firstText = value; }
        }

        private string lastId = "lastBtn";
        /// <summary>
        /// Id attribute for "Last" button. Default value is "lastBtn".
        /// </summary>
        public string LastId
        {
            get { return lastId; }
            set { lastId = value; }
        }

        private string lastText = "Last";
        /// <summary>
        /// Inner text for "Last" button. Default value is "Last"
        /// </summary>
        public string LastText
        {
            get { return lastText; }
            set { lastText = value; }
        }



        private string prevId = "prevBtn";
        /// <summary>
        /// Id attribute for "previous" button. Default value is "prevBtn".
        /// </summary>
        public string PrevId
        {
            get { return prevId; }
            set { prevId = value; }
        }

        private string prevText = "Previous";
        /// <summary>
        /// Inner text for "previous" button. Default value is "Previous"
        /// </summary>
        public string PrevText
        {
            get { return prevText; }
            set { prevText = value; }
        }

        private string nextId = "nextBtn";
        /// <summary>
        /// Id attribute for "next" button. Default value is "nextBtn".
        /// </summary>
        public string NextId
        {
            get { return nextId; }
            set { nextId = value; }
        }

        private string nextText = "Next";
        /// <summary>
        /// Inner text for "next" button. Default value is "Next".
        /// </summary>
        public string NextText
        {
            get { return nextText; }
            set { nextText = value; }
        }

        private string orientation = "horizontal";
        /// <summary>
        /// Sliding can be horizontal or vertical. Horizontal is default and if you want vertical set this to 'vertical'.
        /// </summary>
        public string Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }

        private int speed = 800;
        /// <summary>
        /// Animation speed in milliseconds, default value is 800.
        /// </summary>
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        private bool controlsShow = true;
        /// <summary>
        /// By default set to true, but if set to false it will not add any controls. It can be used with auto scroll when you want to disable user interaction.
        /// </summary>
        public bool ControlsShow
        {
            get { return controlsShow; }
            set { controlsShow = value; }
        }

        private bool controlsFade = true;
        /// <summary>
        /// By default set to true. If set to false it will disable button hiding when slider reaches the end.
        /// </summary>
        public bool ControlsFade
        {
            get { return controlsFade; }
            set { controlsFade = value; }
        }

        private bool firstShow = false;
        /// <summary>
        /// These parameters hide (or show) "go to first" and "go to last" buttons.
        /// </summary>
        public bool FirstShow
        {
            get { return firstShow; }
            set { firstShow = value; }
        }

        private bool lastShow = false;
        /// <summary>
        /// These parameters hide (or show) "go to first" and "go to last" buttons.
        /// </summary>
        public bool LastShow
        {
            get { return lastShow; }
            set { lastShow = value; }
        }

        private int pause = 2000;
        /// <summary>
        /// This option is set in milliseconds and it represent the duration of each slide when plugin is set to auto sliding.
        /// </summary>
        public int Pause
        {
            get { return pause; }
            set { pause = value; }
        }

        private bool continuous = false;
        /// <summary>
        /// If set to true clicking the next button when the slider reached the end will simply continue showing all 
        /// of the slides again with continuous motion. Combining this option with auto (both set to true) 
        /// you'll get endless animation.
        /// </summary>
        public bool Continuous
        {
            get { return continuous; }
            set { continuous = value; }
        }

        private bool numeric = false;
        /// <summary>
        /// If you set this option to true you will get numeric navigation instead of next/prev buttons. 
        /// Plugin will create an ordered list just after the slider div. Ordered list will have the exact 
        /// number of elements as the slider. Clicking on each of the number will make the animation jump 
        /// to a certain slide.
        /// </summary>
        public bool Numeric
        {
            get { return numeric; }
            set { numeric = value; }
        }

        private string numericId = "controls";
        /// <summary>
        /// This option defines id attribute for the ordered list. You should use this id to select and style the list with CSS.
        /// </summary>
        public string NumericId
        {
            get { return numericId; }
            set { numericId = value; }
        }
        

        private void SetupInstanceScript()
        {
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">");

            script.Append("$(document).ready(function(){");

            script.Append("$('#" + this.ClientID + "').easySlider({");

            if (auto)
            {
                script.Append("auto: true");
            }
            else
            {
                script.Append("auto: false");
            }

            if (continuous)
            {
                script.Append(",continuous: true");
            }

            if (pause != 2000) { script.Append(",pause:" + pause.ToInvariantString()); }
            if (speed != 800) { script.Append(",speed:" + speed.ToInvariantString()); }

            if (!controlsShow) { script.Append(",controlsShow:false"); }
            if (!controlsFade) { script.Append(",controlsFade:false"); }
            if (firstShow) 
            { 
                script.Append(",firstShow:true");
                if (firstId != "firstBtn") { script.Append(",firstId:'" + firstId + "'"); }
                if (firstText != "First") { script.Append(",firstText:'" + firstText + "'"); }
            }

            if (lastShow) 
            { 
                script.Append(",lastShow:true");
                if (lastId != "lastBtn") { script.Append(",lastId:'" + lastId + "'"); }
                if (lastText != "Last") { script.Append(",lastText:'" + lastText + "'"); }
            }

            if (prevId != "prevBtn") { script.Append(",prevId:'" + prevId + "'"); }
            if (prevText != "Previous") { script.Append(",prevText:'" + prevText + "'"); }

            if (nextId != "nextBtn") { script.Append(",nextId:'" + nextId + "'"); }
            if (nextText != "Next") { script.Append(",nextText:'" + nextText + "'"); }

            if (numeric) { script.Append(",numeric:true"); }
            if (numericId != "controls") { script.Append(",numericId:'" + numericId + "'"); }

            script.Append("}); ");


            script.Append("});");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());

        }

        private void SetupMainScript()
        {
            //we are assuming that jquery is alreadyloaded in the page via script loader

            Page.ClientScript.RegisterClientScriptBlock(typeof(Page),
                    "jeasyslider", "\n<script  src=\""
                    + Page.ResolveUrl("~/ClientScript/jqmojo/" + scriptFile) + "\" type=\"text/javascript\" ></script>");


        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (autoLocalize)
            {
                prevText = Resource.Previous;
                nextText = Resource.Next;
                firstText = Resource.First;
                lastText = Resource.Last;

            }
            if (CssClass.Length == 0) { CssClass = "easyslider"; }
            SetupMainScript();
            SetupInstanceScript();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            writer.Write("<div class='easyslidewrap'>");
            base.Render(writer);
            writer.Write("</div>");
        }

    }
}