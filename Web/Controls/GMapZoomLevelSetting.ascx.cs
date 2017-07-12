/// Author:					
/// Created:				2008-06-11
/// Last Modified:			2008-06-11
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
    public partial class GMapZoomLevelSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        #region ISettingControl

        public string GetValue()
        {
            return ddGMapZoom.SelectedValue;
        }

        public void SetValue(string val)
        {
            ListItem item = ddGMapZoom.Items.FindByValue(val);
            if (item != null)
            {
                ddGMapZoom.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

    }
}