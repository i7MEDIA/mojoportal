/// Author:					
/// Created:				2008-07-09
/// Last Modified:			2008-07-27
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace mojoPortal.Web.UI
{
    public partial class OrderStatusSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        
        #region ISettingControl

        public string GetValue()
        {
            return ddOrderStatus.SelectedValue;
        }

        public void SetValue(string val)
        {
            ListItem item = ddOrderStatus.Items.FindByValue(val);
            if (item != null)
            {
                ddOrderStatus.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

    }
}