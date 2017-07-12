// Author:					
// Created:					2009-04-02
// Last Modified:			2009-04-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
    public class CKEditorProvider : EditorProvider
    {
        public override IWebEditor GetEditor()
        {
            return new CKEditorAdapter();
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
