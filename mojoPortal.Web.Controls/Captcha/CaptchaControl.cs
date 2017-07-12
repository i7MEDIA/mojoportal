// Author:		        
// Created:            2007-08-15
// Last Modified:      2014-05-22
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls.Captcha;

namespace mojoPortal.Web.Controls
{
    
    public class CaptchaControl : Panel
    {
        private CaptchaProvider captchProvider;
        private ICaptcha captcha = null;
        private string providerName = "SimpleMathCaptchaProvider";

        private string validationGroup = string.Empty;
        public string ValidationGroup
        {
            get { return validationGroup; }
            set { validationGroup = value; }
        }

        public ICaptcha Captcha
        {
            get { return captcha; }
        }

        public bool IsValid
        {
            get 
            {
                if (captcha == null) { return true; }
                if (this.Visible == false) { return true; }
                if (this.Enabled == false) { return true; }

                try
                {
                    return captcha.IsValid;
                }
                catch (NullReferenceException)
                {
                    // this is a hack because of a bug in ReCaptcha where it still tries to valiate even when Enabled = false;
                    if (providerName == "RecaptchaCaptchaProvider") { return true; }
                }
                return false;
            }
        }

        public override bool Enabled
        {
            get { return captcha.Enabled; }
            set { captcha.Enabled = value; }
        }

        public string ProviderName
        {
            get { return providerName; }
            set
            {
                providerName = value;
                if (this.Site != null && this.Site.DesignMode)
                {

                }
                else
                {
                    CaptchaProvider newcaptchProvider = CaptchaManager.Providers[providerName];
                    if (newcaptchProvider != captchProvider)
                    {
                        captchProvider = newcaptchProvider;
                        captcha = captchProvider.GetCaptcha();
                        if (validationGroup.Length > 0)
                        {
                            captcha.ValidationGroup = validationGroup;
                        }
                        this.Controls.Clear();
                        this.Controls.Add(captcha.GetControl());
                    }

                }

            }
        }

        public CaptchaProvider Provider
        {
            get { return captchProvider; }
        }

        protected override void OnInit(EventArgs e)
        {

            if (this.Site != null && this.Site.DesignMode)
            {

            }
            else
            {
                base.OnInit(e);
                // an exception always happens here in design mode
                // this try is just to fix the display in design view in VS
                
                    try
                    {
                        captchProvider = CaptchaManager.Providers[providerName];
                        captcha = captchProvider.GetCaptcha();
                        this.Controls.Add(captcha.GetControl());
                    }
                    catch { }
                

            }

        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            //if (!this.Visible)
            //{
            //    captcha = captchProvider.GetCaptcha();
            //    Control innerCaptchaControl = captcha.GetControl();
            //    innerCaptchaControl.Visible = false;
                
            //    this.Controls.Clear();

            //}
        }

        private string recaptchaPrivateKey = string.Empty;
        /// <summary>
        /// deprecated not used internally properties remain only for backward compat with custom modules
        /// </summary>
        public string RecaptchaPrivateKey
        {
            // this is kind of hacky but recaptch requires a different set
            // of keys for each domain and needed a way to pass that in

            set 
            {
                recaptchaPrivateKey = value;
                //if (this.providerName == "RecaptchaCaptchaProvider")
                //{
                //    if (captcha is RecaptchaAdapter)
                //    {
                //        ((RecaptchaAdapter)captcha).PrivateKey = value;
                //    }

                //}

            }
        }

        private string recaptchaPublicKey = string.Empty;
        /// <summary>
        /// deprecated not used internally properties remain only for backward compat with custom modules
        /// </summary>
        public string RecaptchaPublicKey
        {
            // this is kind of hacky but recaptch requires a different set
            // of keys for each domain and needed a way to pass that in

            set
            {
                recaptchaPublicKey = value;
                //if (this.providerName == "RecaptchaCaptchaProvider")
                //{
                //    if (captcha is RecaptchaAdapter)
                //    {
                //        ((RecaptchaAdapter)captcha).PublicKey = value;
                //    }

                //}

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                // TODO: show a bmp or some other design time thing?
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                base.Render(writer);
            }
        }

    }
}
