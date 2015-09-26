/// Author:					Joe Davis (i7MEDIA)
/// Created:				2014-02-21
/// Last Modified:		    2015-02-06 Joe Davis
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// 
using System;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class CustomScriptModule : SiteModuleControl
    {
        //Feature Guid: 662d49fd-cb44-42a5-a6a7-b905bbe65889

        private CustomScriptConfiguration config = new CustomScriptConfiguration();
        private string scriptRefFormat = "\n<script type='text/javascript' src=\"{0}\"></script>";
        private string rawScriptFormat = "\n<script type='text/javascript'>\n{0}\n</script>";

       
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            SetupScriptUrl();
            SetupRawScript();

            TitleTop.Visible = IsEditable && CustomScriptConfiguration.ForceTitleWhenEditable && !pnlOuterWrap.Visible;
            
            

        }

        private void SetupScriptUrl()
        {
#if NET35
            if (string.IsNullOrEmpty(config.ScriptUrl)) { litScriptUrl.Visible = false; return; }
#else
            if (string.IsNullOrWhiteSpace(config.ScriptUrl)) { litScriptUrl.Visible = false; return; }
#endif

            //script position
            // inHead
            // inBody (register script)
            // inContentPosition
            // bottomStartup (register startup script)

            switch(config.ScriptUrlPosition)
            {
                case "inHead":

                    if (!IsPostBack && !Page.IsCallback)
                    {
                        Page.Header.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl)));
                    }

                    break;

                case "inContentPosition":

                    if (config.RenderStandardWrapperDivs)
                    {
                        pnlOuterWrap.Visible = true;
                        //pnlInnerBody.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptUrl)));
                        litScriptUrl.Text = string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl);
                    }
                    else
                    {
                        if (!IsPostBack && !Page.IsCallback)
                        {
                            this.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl)));
                        }
                    }

                    break;

                case "bottomStartup":

                    ScriptManager.RegisterStartupScript(
                        this, 
                        typeof(Page),
                        ClientID + "scripturl", 
                        string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl), 
                        false);

                    break;


                case "inBody":
                default:

                    ScriptManager.RegisterClientScriptBlock(
                        this, 
                        typeof(Page),
                        ClientID + "scripturl", 
                        string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl), 
                        false);

                    break;

            }
        }

        private void SetupRawScript()
        {
#if NET35
            if (string.IsNullOrEmpty(config.RawScript)) { litScript.Visible = false; return; }
#else
            if (string.IsNullOrWhiteSpace(config.RawScript)) { litScript.Visible = false; return; }
#endif

            //script position
            // inHead
            // inBody (register script)
            // inContentPosition
            // bottomStartup (register startup script)

            switch (config.RawScriptPosition)
            {
                case "inHead":

                    if (!IsPostBack && !Page.IsCallback)
                    {
                        if(config.AddScriptElementToRawScript)
                        {
                            Page.Header.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript)));
                        }
                        else
                        {
                            Page.Header.Controls.Add(new LiteralControl(config.RawScript));
                        }
                        
                    }

                    break;

                case "inContentPosition":

                    if (config.RenderStandardWrapperDivs)
                    {
                        pnlOuterWrap.Visible = true;

                        if (config.AddScriptElementToRawScript)
                        {
                            litScript.Text = string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript);
                        }
                        else
                        {
                            litScript.Text = config.RawScript;
                        }

                    }
                    else
                    {
                        if (!IsPostBack && !Page.IsCallback)
                        {
                            LiteralControl litScript = new LiteralControl();
                            if (config.AddScriptElementToRawScript)
                            {
                                litScript.Text = string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript);
                            }
                            else
                            {
                                litScript.Text = config.RawScript;
                            }

                            this.Controls.Add(litScript);
                        }
                    }

                    break;

                case "bottomStartup":

                    ScriptManager.RegisterStartupScript(
                        this, 
                        typeof(Page),
                        ClientID + "script", 
                        config.RawScript, 
                        config.AddScriptElementToRawScript);

                    break;


                case "inBody":
                default:

                    ScriptManager.RegisterClientScriptBlock(
                        this, 
                        typeof(Page),
                        ClientID + "script", 
                        config.RawScript, 
                        config.AddScriptElementToRawScript);

                    break;

            }
        }

        private void LoadSettings()
        {

            config = new CustomScriptConfiguration(Settings);

            if (config.CustomCssClass.Length > 0)
            {
                pnlOuterWrap.SetOrAppendCss(config.CustomCssClass);
            }

            pnlOuterWrap.Visible = config.RenderStandardWrapperDivs;
        }
    }


    public class CustomScriptConfiguration
    {
        public CustomScriptConfiguration()
        { }

        public CustomScriptConfiguration(Hashtable settings)
        {
            LoadSettings(settings);

        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("ScriptUrl"))
            {
                scriptUrl = settings["ScriptUrl"].ToString();
            }

            if (settings.Contains("ScriptUrlPosition"))
            {
                scriptUrlPosition = settings["ScriptUrlPosition"].ToString();
            }

            if (settings.Contains("RawScriptPosition"))
            {
                rawScriptPosition = settings["RawScriptPosition"].ToString();
            }

            

            if (settings.Contains("RawScript"))
            {
                rawScript = settings["RawScript"].ToString();
            }

            if (settings.Contains("CustomCssClassSetting"))
            {
                customCssClass = settings["CustomCssClassSetting"].ToString();
            }


            addScriptElementToRawScript = WebUtils.ParseBoolFromHashtable(settings, "AddScriptElementToRawScript", addScriptElementToRawScript);

            renderStandardWrapperDivs = WebUtils.ParseBoolFromHashtable(settings, "RenderStandardWrapperDivs", renderStandardWrapperDivs);

        }

        private string scriptUrl = string.Empty;

        public string ScriptUrl
        {
            get { return scriptUrl; }
        }

        private string scriptUrlPosition = "inBody";
        ///
        /// script position
        /// inHead
        /// inBody (register script) default
        /// inContentPosition
        /// bottomStartup (register startup script)
        /// 
        public string ScriptUrlPosition
        {
            get { return scriptUrlPosition; }
        }

        private string rawScript = string.Empty;

        public string RawScript
        {
            get { return rawScript; }
        }

        private string rawScriptPosition = "bottomStartup";
        /// <summary>
        /// script position
        /// inHead
        /// inBody (register script) default
        /// inContentPosition
        /// bottomStartup (register startup script)
        /// </summary>
        public string RawScriptPosition
        {
            get { return rawScriptPosition; }
        }

        private bool addScriptElementToRawScript = true;
        /// <summary>
        /// default is true the outer <script></script> element will be added around the raw script content
        /// set to false if you will put the script element within the content.
        /// false is useful if you also want to add an html snippet that is needed by your script
        /// </summary>
        public bool AddScriptElementToRawScript
        {
            get { return addScriptElementToRawScript; }
        }

        private bool renderStandardWrapperDivs = false;

        public bool RenderStandardWrapperDivs
        {
            get { return renderStandardWrapperDivs; }
        }

        private string customCssClass = string.Empty;

        public string CustomCssClass
        {
            get { return customCssClass; }
        }

        public static bool ForceTitleWhenEditable
        {
            get { return ConfigHelper.GetBoolProperty("CustomScriptModule:ForceTitleWhenEditable", true); }
        }
    }

    public class ScriptPositionSetting : DropDownList, ISettingControl
    {

        protected override void EnsureChildControls()
        {
            base.EnsureChildControls();
            EnsureItems();
            

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            EnsureItems();
        }

        

        private void EnsureItems()
        {
            if (Items.Count == 0)
            {
                // inHead
                // inBody (register script)
                // inContentPosition
                // bottomStartup (register startup script)
                ListItem item = new ListItem();
                item.Text = Resource.ScriptPositionInHead;
                item.Value = "inHead";
                Items.Add(item);

                item = new ListItem();
                item.Text = Resource.ScriptPositionInBody;
                item.Value = "inBody";
                Items.Add(item);

                item = new ListItem();
                item.Text = Resource.ScriptPositionInContent;
                item.Value = "inContentPosition";
                Items.Add(item);

                item = new ListItem();
                item.Text = Resource.ScriptPositionBottomStartup;
                item.Value = "bottomStartup";
                Items.Add(item);

            }

        }
       


        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            //if (ViewState["SelectedVal"] != null)
            //{
            //    return ViewState["SelectedVal"].ToString();
            //}
            return SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = Items.FindByValue(val);
            if (item != null)
            {
                ClearSelection();
                item.Selected = true;
                //ViewState["SelectedVal"] = val;
            }
        }

        #endregion

    }
}