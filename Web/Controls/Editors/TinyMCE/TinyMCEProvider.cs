/// Author:		        
/// Created:            2007-05-26
/// Last Modified:      2013-07-24
///
/// Licensed under the terms of the GNU Lesser General Public License:
///	http://www.opensource.org/licenses/lgpl-license.php
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
    public class TinyMCEProvider : EditorProvider
    {
        public override IWebEditor GetEditor()
        {
            if (WebConfigSettings.TinyMceUseV4)
            {
                // provides the newer 4.x version of TinyMce
                return new TinyMceEditorAdapter();
            }

            return new TinyMCEAdapter();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);
            // don't read anything from config
            // here as this would raise an error under Medium Trust
        }
    }
}
