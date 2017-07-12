/// Author:					
/// Created:				2009-01-11
/// Last Modified:			2009-01-12
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Text;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class ColorSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            SetupColorPicker();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            SetupScript();
            
        }

        private void SetupColorPicker()
        {
            if (Page is mojoBasePage)
            {
                mojoBasePage p = Page as mojoBasePage;
                p.IncludeColorPickerCss = true;
                p.ScriptConfig.IncludeColorPicker = true;
            }

            pnlPicker.Attributes.Add("style", "position: relative; height:205px;");

        }

        protected void SetupScript()
        {

            StringBuilder script = new StringBuilder();
            script.Append("<script type=\"text/javascript\"> ");
            script.Append("\n<!-- \n");

            script.Append("function setup" + this.ClientID + " () {   ");

            script.Append("var windowImages = {imagefiles:[");
            script.Append("'" + Page.ResolveUrl("~/ClientScript/ddwindow/min.gif") + "',");
            script.Append("'" + Page.ResolveUrl("~/ClientScript/ddwindow/close.gif") + "',");
            script.Append("'" + Page.ResolveUrl("~/ClientScript/ddwindow/restore.gif") + "',");
            script.Append("'" + Page.ResolveUrl("~/ClientScript/ddwindow/resize.gif") + "'");
            script.Append("]};");
            script.Append("dhtmlwindow.imagefiles = windowImages.imagefiles;");
            
            script.Append("ddcolorpicker.init({");
            script.Append("colorcontainer: ['" + pnlPickerContainer.ClientID + "', '" + pnlPicker.ClientID + "'], ");

            script.Append("displaymode: 'float',  ");
            script.Append("floatattributes: ['" + Resource.ColorPickerDialogHeading + "', 'width=355px,height=205px,resize=1,scrolling=1,center=1'],  ");
            script.Append("fields: ['" + txtHexColor.ClientID + ":" + spnButton.ClientID + "'] ");

            script.Append(",images: {  ");
            script.Append("PICKER_THUMB: '" + Page.ResolveUrl("~/Data/SiteImages/picker_thumb.png") + "' ");
            script.Append(",HUE_THUMB: '" + Page.ResolveUrl("~/Data/SiteImages/hue_thumb.png") + "'");
            script.Append("} ");

            script.Append(",ids: {  ");
            script.Append("R:'" + this.ClientID + "yui-picker-r',");
            script.Append("R_HEX:'" + this.ClientID + "yui-picker-rhex',");
            script.Append("G:'" + this.ClientID + "yui-picker-g',");
            script.Append("G_HEX:'" + this.ClientID + "yui-picker-ghex',");
            script.Append("B:'" + this.ClientID + "yui-picker-b',");
            script.Append("B_HEX:'" + this.ClientID + "yui-picker-bhex',");
            script.Append("H:'" + this.ClientID + "yui-picker-h',");
            script.Append("S:'" + this.ClientID + "yui-picker-s',");
            script.Append("V:'" + this.ClientID + "yui-picker-v',");
            script.Append("PICKER_BG:'" + this.ClientID + "yui-picker-bg',");
            script.Append("PICKER_THUMB:'" + this.ClientID + "yui-picker-thumb',");
            script.Append("HUE_BG:'" + this.ClientID + "yui-picker-hue-bg',");
            script.Append("HUE_THUMB:'" + this.ClientID + "yui-picker-hue-thumb',");
            script.Append("HEX:'" + this.ClientID + "yui-picker-hex',");
            script.Append("SWATCH:'" + this.ClientID + "yui-picker-swatch',");
            script.Append("WEBSAFE_SWATCH:'" + this.ClientID + "yui-picker-websafe-swatch',");
            script.Append("CONTROLS:'" + this.ClientID + "yui-picker-controls',");
            script.Append("RGB_CONTROLS:'" + this.ClientID + "yui-picker-rgb-controls',");
            script.Append("HSV_CONTROLS:'" + this.ClientID + "yui-picker-hsv-controls',");
            script.Append("HEX_CONTROLS:'" + this.ClientID + "yui-picker-hex-controls',");
            script.Append("HEX_SUMMARY:'" + this.ClientID + "yui-picker-hex-summary',");
            script.Append("CONTROLS_LABEL:'" + this.ClientID + "yui-picker-controls-label'");
            script.Append("}");


            // TODO: localize
            script.Append(",txt: {");
            script.Append("ILLEGAL_HEX:'" + Resource.ColorPickerIllegalHexMessage + "',");
            script.Append("SHOW_CONTROLS:'" + Resource.ColorPickerShowDetails + "',");
            script.Append("HIDE_CONTROLS:'" + Resource.ColorPickerHideDetails + "',");
            script.Append("CURRENT_COLOR:'" + Resource.ColorPickerCurrentColorFormat + "',");
            script.Append("CLOSEST_WEBSAFE:'" + Resource.ColorPickerClosestWebSafeFormat + "',");
            script.Append("R:'" + Resource.ColorPickerRedIndicator + "',");
            script.Append("G:'" + Resource.ColorPickerGreenIndicator + "',");
            script.Append("B:'" + Resource.ColorPickerBlueIndicator + "',");
            script.Append("H:'" + Resource.ColorPickerHueIndicator + "',");
            script.Append("S:'" + Resource.ColorPickerSaturationIndicator + "',");
            script.Append("V:'" + Resource.ColorPickerBrightnessIndicator + "',");
            script.Append("HEX:'#',");
            script.Append("DEG:'\u00B0',");
            //script.Append("DEG:\"Deg\",");
            script.Append("PERCENT:'%'");
            script.Append("}");

            script.Append(",showrgbcontrols: true ");
            script.Append(",showwebsafe: true ");
            script.Append(",showhsvcontrols: false  ");
            script.Append(",showhexcontrols: true  ");
            script.Append(",showhexsummary: false  ");

            //script.Append("  ");
            //script.Append("  ");
            //script.Append("  ");
            
            script.Append("});");

   
            script.Append("} ");

            script.Append("\n Sys.Application.add_load(setup" + this.ClientID + ");");

            script.Append("\n//--> ");
            script.Append(" </script>");

            //Page.ClientScript.RegisterStartupScript(
            //    this.GetType(),
            //    "setup" + this.ClientID,
            //    script.ToString());

            ScriptManager.RegisterStartupScript(this,
                this.GetType(),
                "setup" + this.ClientID,
                script.ToString(), false);

        }

        


        #region ISettingControl

        public string GetValue()
        {
            return txtHexColor.Text;
        }

        public void SetValue(string val)
        {
            txtHexColor.Text = val;
        }

        #endregion

        //public string CurrentHexColor
        //{
        //    get 
        //    {
        //        EnsureChildControls();
        //        return txtHexColor.Text; 
        //    }
        //    set 
        //    {
        //        EnsureChildControls();
        //        txtHexColor.Text = value; 
        //    }
        //}
    }
}