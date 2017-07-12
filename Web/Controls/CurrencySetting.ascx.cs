// Author:					
// Created:				    2008-07-31
// Last Modified:			2009-02-03
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
    
    public partial class CurrencySetting : UserControl, ISettingControl
    {
        //ff2dde1b-e7d7-4c3a-9ab4-6474345e0f31 USD

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
            if (ddCurrency == null)
            {
                ddCurrency = new DropDownList();
                ddCurrency.DataValueField = "Guid";
                ddCurrency.DataTextField = "Code";

                if (this.Controls.Count == 0) { this.Controls.Add(ddCurrency); }
            }

            if (ddCurrency.Items.Count > 0) { return; }

            ddCurrency.DataSource = Currency.GetAll();
            ddCurrency.DataBind();


        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            return ddCurrency.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = ddCurrency.Items.FindByValue(val);
            if (item != null)
            {
                ddCurrency.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

    }
}