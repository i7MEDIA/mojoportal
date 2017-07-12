using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Globalization;

namespace mojoPortal.Web.Controls
{
    /// <summary>
    /// Based on article from CodeProject, with modifications by 
    /// 7/3/2006
    /// 
    /// Source: http://www.codeproject.com/useritems/WatermarkTextBoxControl.asp?df=100
    /// 
    /// The WatermarkTextBox is a asp.net servercontrol that extends a default textbox 
    /// by showing the "watermark" when no text is entered by the user.
    /// Typical watermark text can be " enter your text here " or " search terms "
    /// Copyright 2006, Wouter Steenbergen - We See Consultancy (www.wesee.nl)
    /// </summary>
    /// 
    [System.Obsolete("use placeholder attribute instead")]
    public class WatermarkTextBox : TextBox
    {

        

        //private bool _rendering = false;
        private string _watermark = "enter search terms";
        private string scriptDirectory = "~/ClientScript";

        private const string FUNCTIONBLOCKKEY = "WatermarkTextBoxFunctions";



        /// <summary>
        /// Public default constructor
        /// </summary>
        public WatermarkTextBox()
        {
        }

        /// <summary>
        /// The text to show when no text is entered by the user.
        /// </summary>
        public string Watermark
        {
            get { return _watermark; }
            set { _watermark = value; }
        }

        /// <summary>
        /// Gets or sets the script directory.
        /// </summary>
        /// <value>The script directory.</value>
        [Bindable(true), Category("Behavior"), DefaultValue("~/ClientScript")]
        public string ScriptDirectory
        {
            get { return scriptDirectory; }
            set { scriptDirectory = value; }
        }


        private bool usePlaceholder = true;
        /// <summary>
        /// Use the HTML5 placeholder attribute?
        /// </summary>
        public bool UsePlaceholder
        {
            get { return usePlaceholder; }
            set { usePlaceholder = value; }
        }

        private bool useWatermark = true;

        public bool UseWatermark
        {
            get { return useWatermark; }
            set { useWatermark = value; }
        }

        

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (HttpContext.Current == null) { return; }

            if (!useWatermark && !usePlaceholder) { return; }

            if (!usePlaceholder)
            {
                StringBuilder dbScript = new StringBuilder();
                dbScript.Append("<script type='text/javascript'>\n<!--\n");

                dbScript.Append("\n  var wm" + this.ClientID + " = document.getElementById('" + this.ClientID + "'); ");
                dbScript.Append("if(wm" + this.ClientID + "){");

                dbScript.Append("wm" + this.ClientID + ".value = '" + _watermark + "';");

                dbScript.Append("}");


                dbScript.Append("\n//-->\n</script>");

                this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.UniqueID, dbScript.ToString());
            }



            
        }

        /// <summary> 
        /// Render this control to the output parameter specified.
        /// </summary>
        /// <param name="output"> The HTML writer to write out to </param>
        protected override void Render(HtmlTextWriter output)
        {
            if (HttpContext.Current == null)
            {
                output.Write("[" + this.ID + "]");
                return;
            }

            //_rendering = true;
            if (useWatermark && !usePlaceholder)
            {
                output.AddAttribute("onfocus", "javascript:watermarkEnter(this, '" + _watermark + "');");
                output.AddAttribute("onblur", "javascript:watermarkLeave(this, '" + _watermark + "');");
            }
            else if (usePlaceholder)
            {
                output.AddAttribute("placeholder", _watermark);
            }

            base.Render(output);

            //_rendering = false;
            
        }

    }
}
