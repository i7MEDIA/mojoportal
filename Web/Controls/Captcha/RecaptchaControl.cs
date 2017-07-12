// Copyright (c) 2007 Adrian Godong, Ben Maurer, Mike Hatalski
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 2014-05-22  changed to inherit from BaseValidator in order to support ValidationGroup
// so that multiple instances on a apge with diofferent vsalidation groups can work
// 2016-01-06 i7MEDIA changed to reCaptcha v2

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;

//namespace Recaptcha
namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This class encapsulates reCAPTCHA UI and logic into an ASP.NET server control.
    /// </summary>
    [ToolboxData("<{0}:RecaptchaControl runat=\"server\" />")]
    //[Designer(typeof(Recaptcha.Design.RecaptchaControlDesigner))]
    public class RecaptchaControl : WebControl, IValidator
    //public class RecaptchaControl : BaseValidator
    {
        #region Private Fields

        private const string RECAPTCHA_CHALLENGE_FIELD = "recaptcha_challenge_field";
        private const string RECAPTCHA_RESPONSE_FIELD = "g-recaptcha-response";

        private const string RECAPTCHA_SECURE_HOST = "https://www.google.com/recaptcha/api.js";
        private const string RECAPTCHA_HOST = "http://www.google.com/recaptcha/api.js";

        private RecaptchaResponse recaptchaResponse;

        private string publicKey;
        private string privateKey;
        private string theme = "light";
        private string language;
        private Dictionary<string, string> customTranslations;
        private string customThemeWidget;
        private bool skipRecaptcha;
        private bool allowMultipleInstances;
        private bool overrideSecureMode;
        private IWebProxy proxy;

        private TextBox hdnFake;

        #endregion

        #region Public Properties

        [Category("Settings")]
        [Description("The public key from https://www.google.com/recaptcha/admin/create. Can also be set using RecaptchaPublicKey in AppSettings.")]
        public string PublicKey
        {
            get { return this.publicKey; }
            set { this.publicKey = value; }
        }

        [Category("Settings")]
        [Description("The private key from https://www.google.com/recaptcha/admin/create. Can also be set using RecaptchaPrivateKey in AppSettings.")]
        public string PrivateKey
        {
            get { return this.privateKey; }
            set { this.privateKey = value; }
        }

        [Category("Appearance")]
        [DefaultValue("light")]
        [Description("The theme for the reCAPTCHA control. Currently supported values are 'dark', 'light'.")]
        public string Theme
        {
            get { return this.theme; }
            set { this.theme = value; }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("UI language for the reCAPTCHA control. Currently supported values are 'en', 'nl', 'fr', 'de', 'pt', 'ru', 'es', and 'tr'.")]
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Dictionary<string, string> CustomTranslations
        {
            get { return this.customTranslations; }
            set { this.customTranslations = value; }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        [Description("When using custom theming, this is a div element which contains the widget. ")]
        public string CustomThemeWidget
        {
            get { return this.customThemeWidget; }
            set { this.customThemeWidget = value; }
        }

        [Category("Settings")]
        [DefaultValue(false)]
        [Description("Set this to true to stop reCAPTCHA validation. Useful for testing platform. Can also be set using RecaptchaSkipValidation in AppSettings.")]
        public bool SkipRecaptcha
        {
            get { return this.skipRecaptcha; }
            set { this.skipRecaptcha = value; }
        }

        [Category("Settings")]
        [DefaultValue(false)]
        [Description("Set this to true to enable multiple reCAPTCHA on a single page. There may be complication between controls when this is enabled.")]
        public bool AllowMultipleInstances
        {
            get { return this.allowMultipleInstances; }
            set { this.allowMultipleInstances = value; }
        }

        [Category("Settings")]
        [DefaultValue(false)]
        [Description("Set this to true to override reCAPTCHA usage of Secure API.")]
        public bool OverrideSecureMode
        {
            get { return this.overrideSecureMode; }
            set { this.overrideSecureMode = value; }
        }

        [Category("Settings")]
        [Description("Set this to override proxy used to validate reCAPTCHA.")]
        public IWebProxy Proxy
        {
            get { return this.proxy; }
            set { this.proxy = value; }
        }

        private bool registerWithScriptManager = true;

        public bool RegisterWithScriptManager
        {
            get { return registerWithScriptManager; }
            set { registerWithScriptManager = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="RecaptchaControl"/> class.
        /// </summary>
        public RecaptchaControl()
        {
            //2011-10-20  added these checks because otherwise we are setting it to empty here even if it is declared on the control markup
            if (ConfigurationManager.AppSettings["RecaptchaPublicKey"] != null)
            {
                this.publicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"];

            }

            if (ConfigurationManager.AppSettings["RecaptchaPrivateKey"] != null)
            {
                this.privateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
            }

            if (!bool.TryParse(ConfigurationManager.AppSettings["RecaptchaSkipValidation"], out this.skipRecaptcha))
            {
                this.skipRecaptcha = false;
            }
        }

        #region Overriden Methods

        protected override void OnInit(EventArgs e)
        {
            //if(string.IsNullOrEmpty(ControlToValidate))
            //{
            //    // needed because otherwise an error is thrown by the base class if no control to vlaidate has been set
            //    hdnFake = new TextBox();
            //    hdnFake.ID = this.ID + "hdn";
            //    hdnFake.Visible = false;
            //    Controls.Add(hdnFake);
            //    ControlToValidate = hdnFake.ID;
            //}

            base.OnInit(e);

            if (string.IsNullOrEmpty(this.PublicKey) || string.IsNullOrEmpty(this.PrivateKey))
            {
                throw new ApplicationException("reCAPTCHA needs to be configured with a public & private key.");
            }

            if (this.allowMultipleInstances || !this.CheckIfRecaptchaExists())
            {
                Page.Validators.Add(this);
            }
        }

        /// <summary>
        /// Iterates through the Page.Validators property and look for registered instance of <see cref="RecaptchaControl"/>.
        /// </summary>
        /// <returns>True if an instance is found, False otherwise.</returns>
        private bool CheckIfRecaptchaExists()
        {
            foreach (var validator in Page.Validators)
            {
                if (validator is RecaptchaControl)
                {
                    return true;
                }
            }

            return false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (registerWithScriptManager) { SetupAjaxScripts(); }
        }

        private void SetupAjaxScripts()
        {
            //need to include async and defer on the script element. ScriptManager isn't allowing that so we'll try this.
            ScriptManager.RegisterClientScriptBlock(
                this,
                typeof(Page),
                "recaptchaajax",
                "var script = document.createElement('script'); script.type='text/javascript'; script.src='https://www.google.com/recaptcha/api.js\';" 
                + " script.setAttribute('async',''); script.setAttribute('defer',''); document.head.appendChild(script);",
                true);
        }


        protected override void Render(HtmlTextWriter writer)
        {
            if (this.skipRecaptcha)
            {
                writer.WriteLine("reCAPTCHA validation is skipped. Set SkipRecaptcha property to false to enable validation.");
            }
            else
            {
                this.RenderContents(writer);
            }
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            // <script> setting

            // added 2011-10-22 by  to support use inside UpdatePanel
            //if (registerWithScriptManager)
            //{
                // write a div where we will attach the recaptcha

                output.AddAttribute("id", "recaptcha_" + this.ClientID);
                output.AddAttribute("class", "g-recaptcha");
                output.AddAttribute("data-sitekey", this.publicKey);
                output.AddAttribute("data-theme", this.theme);
                output.AddAttribute("data-tabindex", base.TabIndex.ToString());
                output.RenderBeginTag(HtmlTextWriterTag.Div);
                output.RenderEndTag();
            //}
            //else
            //{
            //    output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            //    output.RenderBeginTag(HtmlTextWriterTag.Script);
            //    output.Indent++;
            //    output.WriteLine("var RecaptchaOptions = {");
            //    output.Indent++;
            //    output.WriteLine("theme : '{0}',", this.theme ?? string.Empty);
            //    if (!string.IsNullOrEmpty(this.language))
            //        output.WriteLine("lang : '{0}',", this.language);
            //    if (this.customTranslations != null && this.customTranslations.Count > 0)
            //    {
            //        var i = 0;
            //        output.WriteLine("custom_translations : {");
            //        foreach (var customTranslation in this.customTranslations)
            //        {
            //            i++;
            //            output.WriteLine(
            //                i != this.customTranslations.Count ?
            //                    "{0} : '{1}'," :
            //                    "{0} : '{1}'",
            //                customTranslation.Key,
            //                customTranslation.Value);
            //        }
            //        output.WriteLine("},");
            //    }
            //    if (!string.IsNullOrEmpty(this.customThemeWidget))
            //        output.WriteLine("custom_theme_widget : '{0}',", this.customThemeWidget);
            //    output.WriteLine("tabindex : {0}", base.TabIndex);
            //    output.Indent--;
            //    output.WriteLine("};");
            //    output.Indent--;
            //    output.RenderEndTag();

            //    // <script> display
            //    output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            //    output.AddAttribute(HtmlTextWriterAttribute.Src, this.GenerateChallengeUrl(false), false);
            //    output.RenderBeginTag(HtmlTextWriterTag.Script);
            //    output.RenderEndTag();

            //}

            // <noscript> display
            output.RenderBeginTag(HtmlTextWriterTag.Noscript);
            output.Indent++;
            output.AddAttribute(HtmlTextWriterAttribute.Src, this.GenerateChallengeUrl(true), false);
            output.AddAttribute(HtmlTextWriterAttribute.Width, "500");
            output.AddAttribute(HtmlTextWriterAttribute.Height, "300");
            output.AddAttribute("frameborder", "0");
            output.RenderBeginTag(HtmlTextWriterTag.Iframe);
            output.RenderEndTag();
            output.WriteBreak(); // modified to make XHTML-compliant. Patch by xitch13@gmail.com.
            output.AddAttribute(HtmlTextWriterAttribute.Name, RECAPTCHA_CHALLENGE_FIELD);
            output.AddAttribute(HtmlTextWriterAttribute.Rows, "3");
            output.AddAttribute(HtmlTextWriterAttribute.Cols, "40");
            output.RenderBeginTag(HtmlTextWriterTag.Textarea);
            output.RenderEndTag();
            output.AddAttribute(HtmlTextWriterAttribute.Name, RECAPTCHA_RESPONSE_FIELD);
            output.AddAttribute(HtmlTextWriterAttribute.Value, "manual_challenge");
            output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            output.RenderBeginTag(HtmlTextWriterTag.Input);
            output.RenderEndTag();
            output.Indent--;
            output.RenderEndTag();
        }

        #endregion

        #region IValidator Members

        [LocalizableAttribute(true)]
        [DefaultValue("The verification words are incorrect.")]
        public string ErrorMessage
        {
            get
            {
                return (this.recaptchaResponse != null) ?
                    this.customTranslations != null && !string.IsNullOrEmpty(this.customTranslations["incorrect_try_again"]) ?
                    this.customTranslations["incorrect_try_again"] :
                    this.recaptchaResponse.ErrorMessage :
                    null;
            }

            set
            {
                throw new NotImplementedException("ErrorMessage property is not settable. To customize reCAPTCHA error message, use custom translation settings.");
            }
        }

        [Browsable(false)]
        public bool IsValid
        {
            get
            {
                return this.recaptchaResponse == null ? true : this.recaptchaResponse.IsValid;
            }

            set
            {
                throw new NotImplementedException("IsValid property is not settable. This property is populated automatically.");
            }
        }

        private string validationGroup = string.Empty;

        public string ValidationGroup
        {
            get { return validationGroup; }
            set { validationGroup = value; }
        }

        /// <summary>
        /// Perform validation of reCAPTCHA.
        /// </summary>
        public void Validate()
        {
            if (Page.IsPostBack && Visible && Enabled && !this.skipRecaptcha)
            {
                if (this.recaptchaResponse == null)
                {
                    if (Visible && Enabled)
                    {
                        RecaptchaValidator validator = new RecaptchaValidator();
                        validator.PrivateKey = this.PrivateKey;
                        validator.RemoteIP = Page.Request.UserHostAddress;
                        //validator.Challenge = Context.Request.Form[RECAPTCHA_CHALLENGE_FIELD];
                        validator.Response = Context.Request.Form[RECAPTCHA_RESPONSE_FIELD];
                        validator.Proxy = this.proxy;

                        if (validator.Response == null)
                        {
                            this.recaptchaResponse = RecaptchaResponse.InvalidResponse;
                        }
                        else
                        {
                            this.recaptchaResponse = validator.Validate();
                        }
                    }
                }
            }
            else
            {
                this.recaptchaResponse = RecaptchaResponse.Valid;
            }
        }

        #endregion

        /// <summary>
        /// This function generates challenge URL.
        /// </summary>
        private string GenerateChallengeUrl(bool noScript)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Context.Request.IsSecureConnection || this.overrideSecureMode ? RECAPTCHA_SECURE_HOST : RECAPTCHA_HOST);
            urlBuilder.Append(noScript ? "/noscript?" : "/challenge?");
            urlBuilder.AppendFormat("k={0}", this.PublicKey);
            return urlBuilder.ToString();
        }
    }
}
