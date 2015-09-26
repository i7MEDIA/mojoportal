// Author:					Joe Audette
// Created:				    2011-06-21
// Last Modified:		    2011-06-21
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI.Controls
{
    public partial class CartList : UserControl
    {
        private Cart cart = null;

        public Cart ShoppingCart
        {
            get { return cart; }
            set { cart = value; }
        }

        private Store store = null;

        public Store Store
        {
            get { return store; }
            set { store = value; }
        }


        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;

        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Visible) { return; }

            PopulateControls();
        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) { return; }
            if (cart == null) { return; }
            
            using (IDataReader reader = cart.GetItems())
            {
                rptCartItems.DataSource = reader;
                rptCartItems.DataBind();
            }

        }


        private void rptCartItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (cart == null) { return; }
            if (store == null) { return; }

            string strGuid = e.CommandArgument.ToString();
            if (strGuid.Length != 36) { return; }

            Guid itemGuid = new Guid(strGuid);

            switch (e.CommandName)
            {
                case "updateQuantity":

                    int quantity = 1;
                    TextBox txtQty = e.Item.FindControl("txtQuantity") as TextBox;
                    if (txtQty != null)
                    {
                        try
                        {
                            int.TryParse(txtQty.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out quantity);
                        }
                        catch (ArgumentException) { }
                    }
                    cart.UpdateCartItemQuantity(itemGuid, quantity);

                    break;

                case "delete":

                    cart.DeleteItem(itemGuid);
                    cart.ResetCartOffers();
                    cart.RefreshTotals();
                    cart.Save();

                    break;

            }

            StoreHelper.EnsureValidDiscounts(store, cart);

            WebUtils.SetupRedirect(this, Request.RawUrl);

        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);

            rptCartItems.ItemCommand += new RepeaterCommandEventHandler(rptCartItems_ItemCommand);
        }
    }
}