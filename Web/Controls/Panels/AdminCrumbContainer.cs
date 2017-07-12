// Author:					
// Created:				    2012-04-27
// Last Modified:			2012-04-27
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.	

using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a sub class of BasePanel so it can be configured differently from theme.skin than panels used in other scenarios
    /// </summary>
    public class AdminCrumbContainer : BasePanel
    {

        private string adminCrumbSeparator = "&nbsp;&gt;";
        /// <summary>
        ///this can be set from theme.skin and is used to set the text on any <portal:AdminCrumbSeparator inside this panel
        /// 
        /// </summary>
        public string AdminCrumbSeparator
        {
            get { return adminCrumbSeparator; }
            set { adminCrumbSeparator = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            foreach (Control c in Controls)
            {
                if (c is AdminCrumbSeparator)
                {
                    AdminCrumbSeparator s = c as AdminCrumbSeparator;
                    s.Text = AdminCrumbSeparator;
                }
            }
        }
    }

    public class AdminCrumbSeparator : Literal
    {
        
    }
}