using System;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;

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
    public abstract class DatePickerProvider : ProviderBase
    {
        public abstract IDatePicker GetDatePicker();
    }
}
