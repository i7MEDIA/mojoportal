using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Controls.Captcha
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-08-15
    /// Last Modified:      2007-08-15
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class CaptchaProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is CaptchaProvider))
                throw new ArgumentException("The provider parameter must be of type CaptchaProvider.");

            base.Add(provider);
        }

        new public CaptchaProvider this[string name]
        {
            get { return (CaptchaProvider)base[name]; }
        }

        public void CopyTo(CaptchaProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }
    }
}
