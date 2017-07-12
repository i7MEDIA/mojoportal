using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Controls.DatePicker
{
    /// <summary>
    /// Author:		        
    /// Created:            2007-11-07
    /// Last Modified:      2007-11-07
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class DatePickerProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is DatePickerProvider))
                throw new ArgumentException("The provider parameter must be of type DatePickerProvider.");

            base.Add(provider);
        }

        new public DatePickerProvider this[string name]
        {
            get { return (DatePickerProvider)base[name]; }
        }

        public void CopyTo(DatePickerProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }

    }
}
