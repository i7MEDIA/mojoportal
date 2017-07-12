// Author:					
// Created:				    2010-12-05
// Last Modified:			2010-12-05
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public class ModuleContainer : Panel
    {
        private bool useCollapsibleDataRole = false;

        public bool UseCollapsibleDataRole
        {
            get { return useCollapsibleDataRole; }
            set { useCollapsibleDataRole = value; }
        }

        private bool startCollapsed = false;

        public bool StartCollapsed
        {
            get { return startCollapsed; }
            set { startCollapsed = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (useCollapsibleDataRole) 
            { 
                Attributes.Add("data-role", "collapsible");
                if (startCollapsed) { Attributes.Add("data-collapsed", "true"); }

            }
        }
    }
}