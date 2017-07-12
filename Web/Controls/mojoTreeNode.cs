/// Author:					
/// Created:				2006-08-28
/// Last Modified:			2010-02-08
///		
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace mojoPortal.Web.UI
{
   
    public class mojoTreeNode : TreeNode
    {

        private string onClientClick = string.Empty;

        public string OnClientClick
        {
            get { return onClientClick; }
            set { onClientClick = value; }
        }

        private bool hasVisibleChildren = false;

        public bool HasVisibleChildren
        {
            get { return hasVisibleChildren; }
            set { hasVisibleChildren = value; }
        }
    }
}
