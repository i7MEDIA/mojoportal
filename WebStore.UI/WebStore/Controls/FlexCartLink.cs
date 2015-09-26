// Author:					Joe Audette
// Created:					2013-06-01
// Last Modified:			2013-10-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using WebStore.Helpers;

namespace WebStore.UI.Controls
{
    /// <summary>
    /// a control that can be used in layout.master to make a cart link that is visible on every page
    /// However it must be configured with the PageID, ModuleID and ModuleGuid corresponding to the store
    /// </summary>
    public class FlexCartLink : HyperLink
    {

       
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (pageId == -1) { Visible = false; }
            if (moduleId == -1) { Visible = false; }

            // actually may only need this if we want to show cart info such as number of items and subtotal
            if (moduleGuid == Guid.Empty) { Visible = false; }

            if (!Visible) { return; }

            if (NavigateUrl.Length == 0)
            {
                NavigateUrl = SiteUtils.GetNavigationSiteRoot()
                    + "/WebStore/Cart.aspx?pageid=" + pageId.ToInvariantString()  + "&amp;mid=" + moduleId.ToInvariantString();
            }

            GetCartInfo();

            FormatText();
        }

        private void FormatText()
        {
            if (
                ((!includeItemCount) && (!includeCartTotal)) 
                ||(currencyCulture == null)
                )
            {
                Text = cartText;
                return; 
            }

            if ((includeItemCount) && (includeCartTotal))
            {
                Text = string.Format(currencyCulture,
                    cartTextWithItemCountAndTotalFormat,
                    itemCount.ToInvariantString(),
                    cartTotal.ToString("c", currencyCulture)
                    );
            }
            else if (includeItemCount) 
            {
                Text = string.Format(currencyCulture,
                    cartTextWithItemCountFormat,
                    itemCount.ToInvariantString()  
                    );
            }

        }


        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (literalTopMarkup.Length > 0)
            {
                writer.Write(literalTopMarkup);
            }

            writer.Write("<a");

            if (CssClass.Length > 0)
            {
                writer.Write(" class='" + CssClass + "'");
            }

            writer.Write(" href='" + Page.ResolveUrl(NavigateUrl) + "'>");

            if (literalInsideLinkTopMarkup.Length > 0)
            {
                writer.Write(literalInsideLinkTopMarkup);
            }

            writer.Write(Text);

            //base.Render(writer);

            if (literalInsideLinkBottomMarkup.Length > 0)
            {
                writer.Write(literalInsideLinkBottomMarkup);
            }

            writer.Write("</a>");

            if (literalBottomMarkup.Length > 0)
            {
                writer.Write(literalBottomMarkup);
            }
        }


        private void GetCartInfo()
        {
            if ((!includeItemCount) && (!includeCartTotal)) { return; }
           
            

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            cart = StoreHelper.GetCartIfExists(moduleGuid, Page.Request.IsAuthenticated);
            if (cart == null) { return; }

            itemCount = cart.CartOffers.Count();
            cartTotal = cart.SubTotal;


        }

        private SiteSettings siteSettings = null;
        private Cart cart = null;
        private CultureInfo currencyCulture = null;

        private int itemCount = 0;
        private decimal cartTotal = 0;


        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
            set { pageId = value; }
        }

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private Guid moduleGuid = Guid.Empty;

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        private string literalTopMarkup = string.Empty;

        public string LiteralTopMarkup
        {
            get { return literalTopMarkup; }
            set { literalTopMarkup = value; }
        }

        private string literalBottomMarkup = string.Empty;

        public string LiteralBottomMarkup
        {
            get { return literalBottomMarkup; }
            set { literalBottomMarkup = value; }
        }

        private string literalInsideLinkTopMarkup = string.Empty;

        public string LiteralInsideLinkTopMarkup
        {
            get { return literalInsideLinkTopMarkup; }
            set { literalInsideLinkTopMarkup = value; }
        }

        private string literalInsideLinkBottomMarkup = string.Empty;

        public string LiteralInsideLinkBottomMarkup
        {
            get { return literalInsideLinkBottomMarkup; }
            set { literalInsideLinkBottomMarkup = value; }
        }

        private string cartText = "Cart";

        public string CartText
        {
            get { return cartText; }
            set { cartText = value; }
        }

        private string cartTextWithItemCountFormat = "Cart ({0} Items)";

        public string CartTextWithItemCountFormat
        {
            get { return cartTextWithItemCountFormat; }
            set { cartTextWithItemCountFormat = value; }
        }

        private bool includeItemCount = false;

        public bool IncludeItemCount
        {
            get { return includeItemCount; }
            set { includeItemCount = value; }
        }

        //http://www.smashingmagazine.com/2008/02/07/shopping-carts-gallery-examples-and-good-practices/

        private string cartTextWithItemCountAndTotalFormat = "Cart ({0} Items) | Total {1}";

        public string CartTextWithItemCountAndTotalFormat
        {
            get { return cartTextWithItemCountAndTotalFormat; }
            set { cartTextWithItemCountAndTotalFormat = value; }
        }

        private bool includeCartTotal = false;

        public bool IncludeCartTotal
        {
            get { return includeCartTotal; }
            set { includeCartTotal = value; }
        }

        private bool includeCartTotalWhenZero = false;
        /// <summary>
        /// true only applies if IncludeCartTotal is also true
        /// </summary>
        public bool IncludeCartTotalWhenZero
        {
            get { return includeCartTotalWhenZero; }
            set { includeCartTotalWhenZero = value; }
        }

    }
}