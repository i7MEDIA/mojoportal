// Author:					
// Created:				    2011-06-26
// Last Modified:			2011-06-26
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
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    public partial class PublishTypeSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { EnsureItems(); }
        }

        private void EnsureItems()
        {
            if (dd.Items.Count > 0) { return; }

            dd.DataSource = UIHelper.EnumToDictionary<ContentPublishMode>();
            dd.DataBind();
            Localize();
        }

        private void Localize()
        {
            foreach (ListItem item in dd.Items)
            {
                item.Text = ResourceHelper.GetResourceString("Resource", item.Text);
            }
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            return dd.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = dd.Items.FindByValue(val);
            if (item != null)
            {
                dd.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion
    }
}