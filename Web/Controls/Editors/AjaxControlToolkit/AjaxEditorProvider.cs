// Author:					
// Created:					2010-08-01
// Last Modified:			2010-08-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Collections.Specialized;

namespace mojoPortal.Web.Editor
{
    public class AjaxEditorProvider : EditorProvider
    {
        public override IWebEditor GetEditor()
        {
            return new AjaxEditorAdapter();
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