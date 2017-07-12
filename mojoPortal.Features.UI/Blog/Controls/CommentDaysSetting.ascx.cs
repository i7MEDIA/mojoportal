// Author:					
// Created:				    2009-08-02
// Last Modified:			2009-08-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.UI;

namespace mojoPortal.Web.BlogUI
{
    public partial class CommentDaysSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ISettingControl

        public string GetValue()
        {
            return ddCommentDays.SelectedValue;
        }

        public void SetValue(string val)
        {
            ListItem item = ddCommentDays.Items.FindByValue(val);
            if (item != null)
            {
                ddCommentDays.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion
    }
}