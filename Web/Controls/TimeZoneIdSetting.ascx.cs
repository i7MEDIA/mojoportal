// Author:					
// Created:				    2010-03-28
// Last Modified:			2010-03-28
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;

namespace mojoPortal.Web.UI
{
    public partial class TimeZoneIdSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        protected void Page_Load(object sender, EventArgs e)
        {


        }

        private void EnsureItems()
        {
            if (dd == null)
            {
                dd = new DropDownList();
                dd.DataValueField = "Id";
                dd.DataTextField = "DisplayName";
                dd.CssClass = "forminput timezonelist";

                if (this.Controls.Count == 0) { this.Controls.Add(dd); }
            }

            if (dd.Items.Count > 0) { return; }
#if!MONO
            dd.DataSource = SiteUtils.GetTimeZoneList();
            dd.DataBind();

#endif
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