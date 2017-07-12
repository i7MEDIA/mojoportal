using System;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// Author:		        
    /// Created:            2007/05/18
    /// Last Modified:      2007/05/30
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public abstract class EditorProvider : ProviderBase
    {
        public abstract IWebEditor GetEditor();
    }
}
