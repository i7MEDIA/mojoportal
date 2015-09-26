/// Author:					Joe Audette
/// Created:				2007-03-05
/// Last Modified:			2010-11-08
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
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using WebStore.Business;
using WebStore.Helpers;

namespace WebStore.UI
{
    public partial class CartAdd : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Store store;
        protected Guid offerGuid = Guid.Empty;
        protected int qtyOrdered = 1;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load +=new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();

            if ((!UserCanViewPage(moduleId, Store.FeatureGuid))||(store == null))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if(store.IsClosed)
            {
                WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
                return;
            }

            AddToCart();
        }

        private void AddToCart()
        {
            if (offerGuid != Guid.Empty)
            {

                Offer offer = new Offer(offerGuid);
                //int quantity = 1;
                qtyOrdered = WebUtils.ParseInt32FromQueryString("qty", qtyOrdered);

                if (!offer.IsAvailable)
                {
                    WebUtils.SetupRedirect(this, Page.ResolveUrl(
                        "~/WebStore/OfferDetail.aspx?pageid="
                        + pageId.ToString()
                        + "&mid=" + moduleId.ToString()
                        + "&offer=" + offerGuid.ToString()));

                    return;
                }

                


                if (
                    (offer.Guid == offerGuid)
                    &&(offer.StoreGuid == store.Guid)
                    &&(store.Guid != Guid.Empty)
                    )
                {
                    //Cart cart = StoreHelper.GetCart(store.Guid);
                    Cart cart = StoreHelper.GetCart();

                    if (cart != null)
                    {

                        if (cart.AddOfferToCart(offer, qtyOrdered))
                        {
                            // redirect to cart page
                            WebUtils.SetupRedirect(this, 
                                SiteRoot + "/WebStore/Cart.aspx?pageid="
                                + pageId.ToString()
                                + "&mid=" + moduleId.ToString()
                                + "&cart=" + cart.CartGuid.ToString());

                            return;

                        }

                    }


                }



            }

        }

        private void LoadSettings()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            if (moduleId == -1) { return; }

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            store = StoreHelper.GetStore();
              
            offerGuid = WebUtils.ParseGuidFromQueryString("offer", offerGuid);
        }

    }
}
