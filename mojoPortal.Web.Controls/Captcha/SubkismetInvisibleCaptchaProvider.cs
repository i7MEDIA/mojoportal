using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-08-17
    /// Last Modified:      2007-08-17
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class SubkismetInvisibleCaptchaProvider : CaptchaProvider
    {
        public override ICaptcha GetCaptcha()
        {
            return new SubkismetInvisibleCaptchaAdapter();
        }

        public override void Initialize(
            string name,
            NameValueCollection config)
        {
            base.Initialize(name, config);
            // don't read anything from config
            // here as this would raise an error under Medium Trust

        }
    }
}
