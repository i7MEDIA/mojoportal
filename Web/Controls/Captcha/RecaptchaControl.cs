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
using mojoPortal.Business.WebHelpers;
using log4net;

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
		private static readonly ILog log = LogManager.GetLogger(typeof(RecaptchaControl));

		#region Private Fields

		//private string captcha_challenge_field = "recaptcha_challenge_field";

		//private string RECAPTCHA_SECURE_HOST = "https://www.google.com/recaptcha/api.js";
		//private string RECAPTCHA_HOST = "http://www.google.com/recaptcha/api.js";

		private RecaptchaResponse recaptchaResponse;
		private TextBox hdnFake;

		#endregion

		#region Public Properties

		[Category("Settings")]
		public string PublicKey { get; set; }

		[Category("Settings")]
		public string PrivateKey { get; set; }

		[Category("Appearance")]
		[DefaultValue("light")]
		[Description("The theme for the CAPTCHA control. Currently supported values are 'dark', 'light'.")]
		public string Theme { get; set; } = "light";

		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("UI language for the CAPTCHA control. reCaptcha and hCaptcha both support automatically detection of user culture")]
		public string Language { get; set; }

		[Category("Appearance")]
		[DefaultValue(null)]
		public Dictionary<string, string> CustomTranslations { get; set; }

		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("When using custom theming, this is a div element which contains the widget. ")]
		public string CustomThemeWidget { get; set; }

		[Category("Settings")]
		[DefaultValue(null)]
		[Description("URL to site that verifies captcha ")]
		public string VerifyUrl { get; set; }

		[Category("Settings")]
		[DefaultValue(null)]
		[Description("URL to script that handles client side functions for captcha ")]
		public string ClientScriptUrl { get; set; }

		[Category("Settings")]
		[DefaultValue(null)]
		[Description("Parameter Name used by captcha to identify the html control for the captcha ")]
		public string Param { get; set; }

		[Category("Settings")]
		[DefaultValue(null)]
		[Description("Field ID used by the captcha to store the response")]
		public string ResponseField { get; set; }
		

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to stop CAPTCHA validation. Useful for testing platform. Can also be set using RecaptchaSkipValidation in AppSettings.")]
		public bool SkipRecaptcha { get; set; }

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to enable multiple reCAPTCHA on a single page. There may be complication between controls when this is enabled.")]
		public bool AllowMultipleInstances { get; set; }

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to override reCAPTCHA usage of Secure API.")]
		public bool OverrideSecureMode { get; set; }

		[Category("Settings")]
		[Description("Set this to override proxy used to validate reCAPTCHA.")]
		public IWebProxy Proxy { get; set; }

		public bool RegisterWithScriptManager { get; set; } = true;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="RecaptchaControl"/> class.
		/// </summary>
		public RecaptchaControl()
        {
            //2011-10-20  added these checks because otherwise we are setting it to empty here even if it is declared on the control markup
            //if (ConfigurationManager.AppSettings["RecaptchaPublicKey"] != null)
            //{
            //    this.publicKey = ConfigurationManager.AppSettings["RecaptchaPublicKey"];
            //}

            //if (ConfigurationManager.AppSettings["RecaptchaPrivateKey"] != null)
            //{
            //    this.privateKey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
            //}

            //if (!bool.TryParse(ConfigurationManager.AppSettings["RecaptchaSkipValidation"], out this.skipRecaptcha))
            //{
            //    this.skipRecaptcha = false;
            //}

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
                throw new ApplicationException("CAPTCHA needs to be configured with a secret & site key.");
            }

            if (this.AllowMultipleInstances || !this.CheckIfRecaptchaExists())
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

            if (RegisterWithScriptManager) { SetupAjaxScripts(); }
        }

        private void SetupAjaxScripts()
        {
            //need to include async and defer on the script element. ScriptManager isn't allowing that so we'll try this.
            ScriptManager.RegisterClientScriptBlock(
                this,
                typeof(Page),
                "captcharemotesript",
                "var script = document.createElement('script'); script.type='text/javascript'; script.src='"+ ClientScriptUrl +"';" 
                + " script.setAttribute('async',''); script.setAttribute('defer',''); document.head.appendChild(script);",
                true);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.SkipRecaptcha)
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
            output.AddAttribute("id", "recaptcha_" + ClientID);
            output.AddAttribute("class", Param);
            output.AddAttribute("data-sitekey", PublicKey);
            output.AddAttribute("data-theme", Theme);
            output.AddAttribute("data-tabindex", base.TabIndex.ToString());
            output.RenderBeginTag(HtmlTextWriterTag.Div);
            output.RenderEndTag();
            
            //output.RenderBeginTag(HtmlTextWriterTag.Noscript);
            //output.Indent++;
            //output.AddAttribute(HtmlTextWriterAttribute.Src, this.GenerateChallengeUrl(true), false);
            //output.AddAttribute(HtmlTextWriterAttribute.Width, "500");
            //output.AddAttribute(HtmlTextWriterAttribute.Height, "300");
            //output.AddAttribute("frameborder", "0");
            //output.RenderBeginTag(HtmlTextWriterTag.Iframe);
            //output.RenderEndTag();
            //output.WriteBreak(); // modified to make XHTML-compliant. Patch by xitch13@gmail.com.
            //output.AddAttribute(HtmlTextWriterAttribute.Name, RECAPTCHA_CHALLENGE_FIELD);
            //output.AddAttribute(HtmlTextWriterAttribute.Rows, "3");
            //output.AddAttribute(HtmlTextWriterAttribute.Cols, "40");
            //output.RenderBeginTag(HtmlTextWriterTag.Textarea);
            //output.RenderEndTag();
            //output.AddAttribute(HtmlTextWriterAttribute.Name, WebConfigSettings.CaptchaResponseField);
            //output.AddAttribute(HtmlTextWriterAttribute.Value, "manual_challenge");
            //output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
            //output.RenderBeginTag(HtmlTextWriterTag.Input);
            //output.RenderEndTag();
            //output.Indent--;
            //output.RenderEndTag();
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
                    this.CustomTranslations != null && !string.IsNullOrEmpty(this.CustomTranslations["incorrect_try_again"]) ?
                    this.CustomTranslations["incorrect_try_again"] :
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
            if (Page.IsPostBack && Visible && Enabled && !SkipRecaptcha)
            {
                if (recaptchaResponse == null)
                {
                    if (Visible && Enabled)
                    {
						RecaptchaValidator validator = new RecaptchaValidator
						{
							VerifyUrl = VerifyUrl,
							PrivateKey = PrivateKey,
							RemoteIP = Page.Request.UserHostAddress,
							Response = Context.Request.Form[ResponseField],
							Proxy = Proxy
						};

						//validator.Challenge = Context.Request.Form[RECAPTCHA_CHALLENGE_FIELD];
						if (validator.Response == null)
                        {
                            recaptchaResponse = RecaptchaResponse.InvalidResponse;
                        }
                        else
                        {
                            recaptchaResponse = validator.Validate();
                        }
                    }
                }
            }
            else
            {
                recaptchaResponse = RecaptchaResponse.Valid;
            }
        }

        #endregion

        /// <summary>
        /// This function generates challenge URL.
        /// </summary>
        //private string GenerateChallengeUrl(bool noScript)
        //{
        //    StringBuilder urlBuilder = new StringBuilder();
        //    urlBuilder.Append(Context.Request.IsSecureConnection || OverrideSecureMode ? RECAPTCHA_SECURE_HOST : RECAPTCHA_HOST);
        //    urlBuilder.Append(noScript ? "/noscript?" : "/challenge?");
        //    urlBuilder.AppendFormat("k={0}", PublicKey);
        //    return urlBuilder.ToString();
        //}
    }
}
