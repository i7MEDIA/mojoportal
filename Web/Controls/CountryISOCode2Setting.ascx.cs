/// Author:					Ghalib Ghniem
/// Created:				2011-04-24
/// Last Modified:			2011-04-24
/// 


using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{

    public partial class CountryISOCode2Setting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public string GetText()
        {
            if (ddCountryISOCode2.SelectedItem != null)
            {
                return ddCountryISOCode2.SelectedItem.Text;
            }

            return string.Empty;
        }

        #region ISettingControl

        public string GetValue()
        {
            return ddCountryISOCode2.SelectedValue;
        }

        
        public void SetValue(string val)
        {
            ListItem item = ddCountryISOCode2.Items.FindByValue(val);
            if (item != null)
            {
                ddCountryISOCode2.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion


    }
}