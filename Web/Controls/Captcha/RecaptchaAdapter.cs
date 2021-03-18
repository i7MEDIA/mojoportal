// Author:		        
// Created:            2007-08-16
// Last Modified:      2014-05-22
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Controls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
//using Recaptcha;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.Controls.Captcha
{
    
    public class RecaptchaAdapter : ICaptcha
    {
        #region Constructors

        public RecaptchaAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        private RecaptchaControl captchaControl = new RecaptchaControl();

        //private RecaptchaControl captchaControl
        //    = new RecaptchaControl();


        //public string PrivateKey
        //{
        //    get { return captchaControl.PrivateKey; }
        //    set { captchaControl.PrivateKey = value; }
        //}

        //public string PublicKey
        //{
        //    get { return captchaControl.PublicKey; }
        //    set { captchaControl.PublicKey = value; }
        //}

        public bool IsValid
        {
            get {
                captchaControl.Validate();
                
                return captchaControl.IsValid; 
            }
           
        }

        public bool Enabled
        {
            get { return captchaControl.Enabled; }
            set 
            { 
                captchaControl.Enabled = value;
                if (!value) { captchaControl.SkipRecaptcha = true; }
            }

        }

        public string ControlID
        {
            get
            {
                return captchaControl.ID;
            }
            set
            {
                captchaControl.ID = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                return captchaControl.ValidationGroup;
            }
            set
            {
                captchaControl.ValidationGroup = value;
            }
        }
		public short TabIndex
		{
			get
			{
				return captchaControl.TabIndex;
			}
			set
			{
				captchaControl.TabIndex = value;
			}
		}

		private void InitializeAdapter()
        {
           
        }

        #region Public Methods

        public Control GetControl()
        {
			if ((WebConfigSettings.RecaptchaPrivateKey.Length > 0)&&(WebConfigSettings.RecaptchaPublicKey.Length > 0))
            {
				if (WebConfigSettings.RecaptchaHCaptcha == "recaptcha")
				{
					captchaControl.Theme = WebConfigSettings.ReCaptchaDefaultTheme;
					captchaControl.ClientScriptUrl = WebConfigSettings.ReCaptchaDefaultClientScriptUrl;
					captchaControl.VerifyUrl = WebConfigSettings.ReCaptchaDefaultVerifyUrl;
					captchaControl.Param = WebConfigSettings.ReCaptchaDefaultParam;
					captchaControl.ResponseField = WebConfigSettings.ReCaptchaDefaultResponseField;
				}
				else
				{
					captchaControl.Theme = WebConfigSettings.HCaptchaDefaultTheme;
					captchaControl.ClientScriptUrl = WebConfigSettings.HCaptchaDefaultClientScriptUrl;
					captchaControl.VerifyUrl = WebConfigSettings.HCaptchaDefaultVerifyUrl;
					captchaControl.Param = WebConfigSettings.HCaptchaDefaultParam;
					captchaControl.ResponseField = WebConfigSettings.HCaptchaDefaultResponseField;
				}

				captchaControl.PrivateKey = WebConfigSettings.RecaptchaPrivateKey;
                captchaControl.PublicKey = WebConfigSettings.RecaptchaPublicKey;
				captchaControl.RegisterWithScriptManager = true;
				captchaControl.TabIndex = 10;
                return captchaControl;
            }

			SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

			if ((siteSettings == null)||(siteSettings.RecaptchaPrivateKey.Length == 0)||(siteSettings.RecaptchaPublicKey.Length == 0))
            {
                return new Subkismet.Captcha.CaptchaControl();
            }

			captchaControl.Theme = siteSettings.CaptchaTheme;
			captchaControl.ClientScriptUrl = siteSettings.CaptchaClientScriptUrl;
			captchaControl.VerifyUrl = siteSettings.CaptchaVerifyUrl;
			captchaControl.Param = siteSettings.CaptchaParam;
			captchaControl.ResponseField = siteSettings.CaptchaResponseField;
			captchaControl.PrivateKey = siteSettings.RecaptchaPrivateKey;
            captchaControl.PublicKey = siteSettings.RecaptchaPublicKey;
            captchaControl.Theme = siteSettings.CaptchaTheme;
            captchaControl.RegisterWithScriptManager = true;
            
            return captchaControl;
        }



        #endregion
    }
}
