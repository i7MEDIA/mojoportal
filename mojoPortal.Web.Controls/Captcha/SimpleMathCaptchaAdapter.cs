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

namespace mojoPortal.Web.Controls.Captcha
{
    
    public class SimpleMathCaptchaAdapter : ICaptcha
    {
        #region Constructors

        public SimpleMathCaptchaAdapter() 
        {
            InitializeAdapter();
        }

        #endregion

        private SimpleMathCaptchaControl captchaControl 
            = new SimpleMathCaptchaControl();

        public bool IsValid
        {
            get { return captchaControl.IsValid; }
           
        }

        public bool Enabled
        {
            get { return captchaControl.Enabled; }
            set { captchaControl.Enabled = value; }

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
            return captchaControl;
        }



        #endregion
    }
}
