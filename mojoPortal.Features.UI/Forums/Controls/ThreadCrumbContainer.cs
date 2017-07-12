// Author:					
// Created:				    2012-08-08
// Last Modified:			2012-08-08
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
using mojoPortal.Web.UI;

namespace mojoPortal.Web.ForumUI
{
    /// <summary>
    /// a sub class of BasePanel so it can be configured differently from theme.skin than panels used in other scenarios
    /// </summary>
    public class ThreadCrumbContainer : BasePanel
    {
        private string crumbSeparator = "&nbsp;&gt;&nbsp;";
        /// <summary>
        ///this can be set from theme.skin and is used to set the text on any <portal:AdminCrumbSeparator inside this panel
        /// 
        /// </summary>
        public string CrumbSeparator
        {
            get { return crumbSeparator; }
            set { crumbSeparator = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            foreach (Control c in Controls)
            {
                if (c is CrumbSeparatorLiteral)
                {
                    CrumbSeparatorLiteral s = c as CrumbSeparatorLiteral;
                    s.Text = CrumbSeparator;
                }
            }
        }
    }

    public class CrumbSeparatorLiteral : Literal
    {

    }

}