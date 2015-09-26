// Author:					Joe Audette
// Created:				    2007-09-07
// Last Modified:		    2015-04-24
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
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Business.WebHelpers.PaymentGateway;
using WebStore.Business;
using WebStore.Components;
using mojoPortal.Net;
using log4net;
using Resources;
using WebStore.UI;

namespace WebStore.Helpers
{    
    
    public static class StoreHelper
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(StoreHelper));

        public static Store GetStore()
        {
            int pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            int moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

            if (moduleId == -1)
            {
                PageSettings currentPage = CacheHelper.GetCurrentPage();
                if (currentPage != null)
                    moduleId = FindStoreModuleId(currentPage);

            }

            

            if (moduleId == -1) return null;
            if (pageId == -1) return null;

            string key = "WebStore" + moduleId.ToString(CultureInfo.InvariantCulture);

            if (HttpContext.Current.Items[key] != null)
            {
                return (Store)HttpContext.Current.Items[key];
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

            if ((siteSettings == null) || (siteSettings.SiteGuid == Guid.Empty)) return null;

            Module module = new Module(moduleId, pageId);
            if (module.SiteId != siteSettings.SiteId) return null;

            if (module.FeatureGuid != new Guid("0cefbf18-56de-11dc-8f36-bac755d89593")) return null;

            Store store = new Store(siteSettings.SiteGuid, moduleId);
            if (store.Guid == Guid.Empty) // No store created yet
            {
                store = new Store();
                store.SiteGuid = siteSettings.SiteGuid;
                store.ModuleId = moduleId;
                store.Save();
            }

            HttpContext.Current.Items.Add(key, store);

            return store;
        }

        public static int FindStoreModuleId(PageSettings currentPage)
        {
            Guid storeGuid = new Guid("0cefbf18-56de-11dc-8f36-bac755d89593");
            foreach (Module m in currentPage.Modules)
            {
                if (m.FeatureGuid == storeGuid) return m.ModuleId;

                if (m.ControlSource == "WebStore/WebStoreModule.ascx") return m.ModuleId;

            }

            return -1;
        }

        //Added by Ismail Hassan  Sami (200904712)
        public static string GetOrderStatusName(Guid statusGuid)
        {
            if (statusGuid == mojoPortal.Business.Commerce.OrderStatus.OrderStatusReceivedGuid)
                return WebStoreResources.OrderStatusReceived;

            if (statusGuid == mojoPortal.Business.Commerce.OrderStatus.OrderStatusFulfillableGuid)
                return WebStoreResources.OrderStatusFulfillable;

            if (statusGuid == mojoPortal.Business.Commerce.OrderStatus.OrderStatusFulfilledGuid)
                return WebStoreResources.OrderStatusFulfilled;

            if (statusGuid == mojoPortal.Business.Commerce.OrderStatus.OrderStatusCancelledGuid)
                return WebStoreResources.OrderStatusCancelled;

            return WebStoreResources.OrderStatusNone;
        }

        public static string GetFulfillmentTypeLabel(FulfillmentType fulfillmentType)
        {
            switch (fulfillmentType)
            {
                case FulfillmentType.Download:
                    return WebStoreResources.FullfillmentTypeEnumDownload;

                case FulfillmentType.PhysicalShipment:
                    return WebStoreResources.FullfillmentTypeEnumShipped;

                case FulfillmentType.None:
                    return WebStoreResources.FullfillmentTypeEnumNone;

            }

            return WebStoreResources.FullfillmentTypeEnumNone;
        }

        public static string GetFulfillmentTypeLabel(string sFulfillmentType)
        {
            FulfillmentType fulfillmentType = Product.FulfillmentTypeFromString(sFulfillmentType);

            return GetFulfillmentTypeLabel(fulfillmentType);

           
        }

        public static bool ApplyDiscount(Store store, Cart cart, string discountCode, out string errorMessage)
        {
            if (cart == null)
            {
                errorMessage = WebStoreResources.DiscountInvalidCartError;
                return false;
            }

            if (store == null)
            {
                errorMessage = WebStoreResources.DiscountInvalidCartError;
                return false;
            }

            if (string.IsNullOrEmpty(discountCode))
            {
                errorMessage = WebStoreResources.DiscountCodeNotProvidedError;
                return false;
            }

            Discount discount = new Discount(store.ModuleGuid, discountCode);
            if (discount.DiscountGuid == Guid.Empty)
            {
                errorMessage = WebStoreResources.DiscountCodeNotFoundError;
                return false;
            }

            if (discount.ValidityEndDate < DateTime.UtcNow)
            {
                errorMessage = WebStoreResources.DiscountCodeExpiredError;
                return false;
            }

            if (discount.ValidityStartDate > DateTime.UtcNow)
            {
                errorMessage = WebStoreResources.DiscountCodeNotActiveError;
                return false;
            }

            if (!discount.AllowOtherDiscounts)
            {
                cart.DiscountCodesCsv = discount.DiscountCode;
            }

            if (cart.DiscountCodesCsv.Length == 0)
            {
                cart.DiscountCodesCsv = discount.DiscountCode;
            }
            else
            {
                if (!cart.DiscountCodesCsv.Contains(discount.DiscountCode))
                {
                    cart.DiscountCodesCsv += "," + discount.DiscountCode;
                }
            }

            

            return ValidateAndApplyDiscounts(store, cart, out errorMessage);

            

        }

        public static void EnsureValidDiscounts(Store store, Cart cart)
        {
            string errorMessage = string.Empty;
            ValidateAndApplyDiscounts(store, cart, out errorMessage);
        }

        public static bool ValidateAndApplyDiscounts(Store store, Cart cart, out string errorMessage)
        {
            if (cart == null)
            {
                errorMessage = WebStoreResources.DiscountInvalidCartError;
                return false;
            }

            if (store == null)
            {
                errorMessage = WebStoreResources.DiscountInvalidCartError;
                return false;
            }

            errorMessage = string.Empty;
            List<Discount> discountList = Discount.GetValidDiscounts(store.ModuleGuid, cart, cart.DiscountCodesCsv);
            cart.Discount = 0;
            cart.RefreshTotals();

            cart.DiscountCodesCsv = string.Empty;
            if (cart.SubTotal <= 0)
            {
                cart.Save();
                return false;
            }

            string comma = string.Empty;
            bool appliedDiscount = false;
            foreach (Discount d in discountList)
            {
                cart.DiscountCodesCsv += comma + d.DiscountCode;
                comma = ",";
                if (d.AbsoluteDiscount > 0)
                {
                    cart.Discount += d.AbsoluteDiscount;
                    appliedDiscount = true;
                }

                if (d.PercentageDiscount > 0)
                {
                    cart.Discount += (cart.SubTotal * d.PercentageDiscount);
                    appliedDiscount = true;
                }

            }

            cart.RefreshTotals();

            if (!appliedDiscount) { errorMessage = WebStoreResources.DiscountInvalidCartError; }

            return appliedDiscount;

        }

        public static void EnsureUserForOrder(Cart cart)
        {
            if (cart == null) { return; }

            //cart already has a valid userGuid
            if (cart.UserGuid != Guid.Empty) { return; }

            if (cart.OrderInfo == null) { return; }
            if (cart.OrderInfo.CustomerEmail.Length == 0) { return; }

            // if cart.UserGuid == Guid.Empty autocreate a user based on the email and customer info
            // this way we can support ordering without register/sign in
            // at least for fulfillmenttypenone and probably for shipped orders
            // for download orders we should make the usersign in first because they will need to login to download

            try
            {
                SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                if (siteSettings == null) { return; }
                if (siteSettings.SiteGuid == Guid.Empty) { return; }

                bool includeInMemberList = false;

                SiteUser newUser = SiteUtils.CreateMinimalUser(
                    siteSettings,
                    cart.OrderInfo.CustomerEmail,
                    includeInMemberList,
                    WebStoreResources.UserAutoCreatedComment);

                if (newUser != null) { cart.UserGuid = newUser.UserGuid; }
            }
            catch (Exception ex)
            {
                log.Error("Store Helper failed to auto create user for cart/order " + cart.CartGuid.ToString(), ex);
            }

        }

        public static void ConfirmOrder(Store store, Order order)
        {
            if (store == null) { return; }
            if (order == null) { return; }

            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();
            CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();

            string subjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        commerceConfig.DefaultConfirmationEmailSubjectTemplate);

            string textBodyTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        commerceConfig.DefaultConfirmationEmailTextBodyTemplate);

            int pageId = -1;

            List<PageModule> pageModules = PageModule.GetPageModulesByModule(store.ModuleId);
            foreach (PageModule pm in pageModules)
            {
                // use first pageid found, really a store should only
                // be on one page
                pageId = pm.PageId;
                break;

            }

            

            Module m = new Module(store.ModuleId);

            Order.EnsureSalesReportData(order.OrderGuid, m.ModuleGuid, pageId, store.ModuleId);

            SiteUser.UpdateTotalRevenue(order.UserGuid);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            CultureInfo currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            OrderCompletedEventArgs e = new OrderCompletedEventArgs(order, currencyCulture);

            foreach (OrderCompletedHandlerProvider p in OrderCompletedHandlerProviderManager.Providers)
            {
                
                try
                {
                    p.HandelOrderCompleted(e);
                }
                catch (Exception ex)
                {
                    log.Error(ex);
                }
            }

            //TODO: make this asynchronous so if it takes a long time or has a timeout error it does
            //not prevent the redirect 
            SendConfirmEmail(
                defaultCulture,
                currencyCulture,
                siteSettings,
                pageId,
                commerceConfig,
                store,
                order,
                subjectTemplate,
                textBodyTemplate);




        }



        /// <summary>
        /// This method sends confirmation that the order was received and is being processed by the payment processor, ie PayPal or Google Checkout, but payment has not yet cleared
        /// </summary>
        /// <param name="store"></param>
        /// <param name="order"></param>
        public static void ConfirmOrderReceived(Store store, Order order)
        {
            ConfirmOrderReceived(store, order, false);

        }

        public static void ConfirmOrderReceived(Store store, Order order, bool isEcheck)
        {
            CommerceConfiguration commerceConfig = SiteUtils.GetCommerceConfig();
            CultureInfo defaultCulture = SiteUtils.GetDefaultUICulture();
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            CultureInfo currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);

            string subjectTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        commerceConfig.DefaultOrderReceivedEmailSubjectTemplate);

            string textBodyTemplate;

            if (isEcheck)
            {
                textBodyTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        commerceConfig.DefaultEcheckOrderReceivedEmailTextBodyTemplate);
            }
            else
            {
                textBodyTemplate
                        = ResourceHelper.GetMessageTemplate(defaultCulture,
                        commerceConfig.DefaultOrderReceivedEmailTextBodyTemplate);
            }

            int pageId = -1;

            List<PageModule> pageModules = PageModule.GetPageModulesByModule(store.ModuleId);
            foreach (PageModule pm in pageModules)
            {
                // use first pageid found, really a store should only
                // be on one page
                pageId = pm.PageId;
                break;

            }


            SendConfirmEmail(
                defaultCulture,
                currencyCulture,
                siteSettings,
                pageId,
                commerceConfig,
                store,
                order,
                subjectTemplate,
                textBodyTemplate);

        }

        private static bool ShouldSendConfirmation(int siteId, Order order)
        {
            if (WebStoreConfiguration.DisableOrderConfirmationEmail) { return false; }

            if(order.PaymentMethod == "PayPal")
            {
                // with download products the paypal confirmation is not
                // sufficient because it has no link to the order details where the user can download
                // so only let this setting disable confirmation if the order has no downloads
                if(!order.HasDownloadProducts())
                {
                    string siteKey = "WebStore:Site" + siteId.ToInvariantString() + "-UseOnlyPayPalOrderConfirmation";
                    bool cancel = ConfigHelper.GetBoolProperty(siteKey, false);
                    if (cancel) { return false; }
                }
                
            }
            

            return true;
        }

        private static void SendConfirmEmail(
            CultureInfo defaultCulture,
            CultureInfo currencyCulture,
            SiteSettings siteSettings,
            int pageId,
            CommerceConfiguration commerceConfig, 
            Store store, 
            Order order,
            string subjectTemplate,
            string textBodyTemplate)
        {
            if (store == null) { return; }
            if (order == null) { return; }
            if(siteSettings == null) { return; }

            if (!ShouldSendConfirmation(siteSettings.SiteId, order)) 
            {
                log.Info("confirmation email for order " + order.OrderGuid.ToString() + " not sent due to disabled by configuration");
                return; 
            }
            
            //SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //CultureInfo currencyCulture = ResourceHelper.GetCurrencyCulture(siteSettings.GetCurrency().Code);
            
            //EmailMessageTask messageTask = new EmailMessageTask(SiteUtils.GetSmtpSettings());
            //messageTask.EmailFrom = store.SalesEmail;
            // TODO: implement from alias for store
            //messageTask.EmailFromAlias = siteSettings.DefaultFromEmailAlias;
            string email;
            if (order.CustomerEmail.Length > 0)
            {
                //messageTask.EmailTo = order.CustomerEmail;
                email = order.CustomerEmail;
            }
            else
            {
                SiteUser siteUser = new SiteUser(siteSettings, order.UserGuid);
                //messageTask.EmailTo = siteUser.Email;

                email = siteUser.Email;

            }

            //if (store.OrderBccEmail.Length > 0)
            //{
            //    messageTask.EmailBcc = store.OrderBccEmail;
            //}

            
            PageSettings page = new PageSettings(siteSettings.SiteId, pageId);

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string storeLink = string.Empty;

            if (page.Url.StartsWith("~/"))
            {
                storeLink = siteRoot + page.Url.Replace("~/", "/");
            }
            else
            {
                storeLink = siteRoot;
            }

            string orderLink = siteRoot +
                "/WebStore/OrderDetail.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + store.ModuleId.ToInvariantString()
                + "&orderid=" + order.OrderGuid.ToString();

            StringBuilder orderDetails = new StringBuilder();
            DataSet dsOffers = Order.GetOrderOffersAndProducts(store.Guid, order.OrderGuid);

            foreach (DataRow row in dsOffers.Tables["Offers"].Rows)
            {
                string og = row["OfferGuid"].ToString();
                orderDetails.Append(row["Name"].ToString() + " ");
                orderDetails.Append(row["Quantity"].ToString() + " @ ");
                orderDetails.Append(string.Format(currencyCulture, "{0:c}", Convert.ToDecimal(row["OfferPrice"])));
                orderDetails.Append("\r\n");

                string whereClause = string.Format("OfferGuid = '{0}'", og);
                DataView dv = new DataView(dsOffers.Tables["Products"], whereClause, "", DataViewRowState.CurrentRows);

                if (dv.Count > 1)
                {
                    foreach (DataRow r in dsOffers.Tables["Products"].Rows)
                    {
                        string pog = r["OfferGuid"].ToString();
                        if (og == pog)
                        {
                            orderDetails.Append(r["Name"].ToString() + " ");
                            orderDetails.Append(r["Quantity"].ToString() + "  \r\n");

                        }

                    }
                }

            }


            //messageTask.Subject = string.Format(
            //    defaultCulture,
            //    subjectTemplate,
            //    store.Name,
            //    order.OrderGuid.ToString()
            //    );


            //messageTask.TextBody = string.Format(
            //    defaultCulture,
            //    textBodyTemplate,
            //    order.CustomerFirstName + " " + order.CustomerLastName,
            //    store.Name,
            //    order.OrderGuid.ToString(),
            //    storeLink,
            //    orderLink,
            //    orderDetails.ToString(),
            //    order.OrderTotal.ToString("c", currencyCulture),
            //    order.ShippingTotal.ToString("c", currencyCulture),
            //    order.TaxTotal.ToString("c", currencyCulture),
            //    order.SubTotal.ToString("c", currencyCulture),
            //    order.Discount.ToString("c", currencyCulture)
            //    ).ToAscii();


            //messageTask.SiteGuid = siteSettings.SiteGuid;
            //messageTask.QueueTask();

            //WebTaskManager.StartOrResumeTasks();

            string fromAddress = store.EmailFrom;
            if (fromAddress.Length == 0) { fromAddress = siteSettings.DefaultEmailFromAddress; }



            Email.Send(
                        SiteUtils.GetSmtpSettings(),
                        fromAddress,
                        string.Empty,
                        string.Empty,
                        email,
                        string.Empty,
                        store.OrderBccEmail,
                        string.Format(defaultCulture, subjectTemplate, store.Name, order.OrderGuid.ToString()),
                        string.Format(
                                defaultCulture,
                                textBodyTemplate,
                                order.CustomerFirstName + " " + order.CustomerLastName,
                                store.Name,
                                order.OrderGuid.ToString(),
                                storeLink,
                                orderLink,
                                orderDetails.ToString(),
                                order.OrderTotal.ToString("c", currencyCulture),
                                order.ShippingTotal.ToString("c", currencyCulture),
                                order.TaxTotal.ToString("c", currencyCulture),
                                order.SubTotal.ToString("c", currencyCulture),
                                order.Discount.ToString("c", currencyCulture)
                                ),
                        false,
                        Email.PriorityNormal);

            
        }

        

        //public static CommerceConfiguration GetCommerceConfig()
        //{
        //    if (HttpContext.Current == null) return null;

        //    if (HttpContext.Current.Items["commerceConfig"] != null)
        //    {
        //        return (CommerceConfiguration)HttpContext.Current.Items["commerceConfig"];
        //    }

        //    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();

        //    if ((siteSettings == null) || (siteSettings.SiteGuid == Guid.Empty)) return null;

        //    CommerceConfiguration commerceConfig = new CommerceConfiguration(siteSettings);

        //    HttpContext.Current.Items.Add("commerceConfig", commerceConfig);

        //    return commerceConfig;


        //}

        public static void MoveOrderToUser(Guid orderGuid, Guid currentOrderUserGuid, Guid newUserGuid)
        {
            if (currentOrderUserGuid == newUserGuid) { return; }
            if (newUserGuid == Guid.Empty) { return; }

            Order.MoveOrder(orderGuid, newUserGuid);
            FullfillDownloadTicket.MoveOrder(orderGuid, newUserGuid);
            CommerceReport.MoveOrder(orderGuid, newUserGuid);

        }

        public static Cart GetCartIfExists(Guid storeGuid, bool isAuthenticated)
        {

            if (HttpContext.Current == null) return null;

            //Store store = GetStore();

            //if (store == null) return null;

            Cart cart = null;

            //string cartKey = "cart" + store.Guid.ToString();
            string cartKey = "cart" + storeGuid.ToString();
            
            if (HttpContext.Current.Items[cartKey] != null)
            {
                cart = (Cart)HttpContext.Current.Items[cartKey];
                if(isAuthenticated)
                {
                    VerifyCartUser(cart);
                }
                return cart;
            }

            if (UserHasCartCookie(storeGuid))
            {
                string cartCookie = GetCartCookie(storeGuid);
                if (cartCookie.Length == 36)
                {
                    Guid cartGuid = new Guid(cartCookie);
                    cart = new Cart(cartGuid);
                    if ((!cart.Exists) || (cart.StoreGuid != storeGuid))
                    {
                        return null;
                    }

                    if (isAuthenticated)
                    {
                        VerifyCartUser(cart);
                    }
                    HttpContext.Current.Items[cartKey] = cart;
                    return cart;

                }
                else
                {
                    // cookie is invalid
                    return null;
                }
            }
            else
            {
                // TODO: handle use case where user adds to cart on 1 machine
                // then comes back to site on another machine and has no cart cookie
                // look for a cart that has the userguid, 
                // if found set cookie for that cart

                // doesn't exist
                return null;

            }




            //return null;


        }

        public static Cart GetCart()
        {

            if (HttpContext.Current == null) return null;

            Store store = GetStore();

            if (store == null) return null;

            Cart cart = null;
            
            string cartKey = "cart" + store.Guid.ToString();
            if (HttpContext.Current.Items[cartKey] != null)
            {
                cart = (Cart)HttpContext.Current.Items[cartKey];
                VerifyCartUser(cart);
                return cart;
            }
            
            if (UserHasCartCookie(store.Guid))
            {
                string cartCookie = GetCartCookie(store.Guid);
                if (cartCookie.Length == 36)
                {
                    Guid cartGuid = new Guid(cartCookie);
                    cart = new Cart(cartGuid);
                    if ((!cart.Exists)||(cart.StoreGuid != store.Guid))
                    {
                        return CreateCartAndSetCookie(store);
                    }
                    VerifyCartUser(cart);
                    HttpContext.Current.Items[cartKey] = cart;
                    return cart;

                }
                else
                {
                    // cookie is invalid
                    return CreateCartAndSetCookie(store);
                }
            }
            else
            {
                // TODO: handle use case where user adds to cart on 1 machine
                // then comes back to site on another machine and has no cart cookie
                // look for a cart that has the userguid, 
                // if found set cookie for that cart

                // new cart
                return CreateCartAndSetCookie(store);

            }


            

            //return null;


        }

        private static void VerifyCartUser(Cart cart)
        {
            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }
            if (cart == null) { return; }
            if (cart.UserGuid == Guid.Empty) { return; }

            if (HttpContext.Current.Request.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser == null)
                {
                    cart.ClearCustomerData();
                    cart.UserGuid = Guid.Empty;
                    cart.OrderInfo.Save();
                    cart.Save();
                }
                else
                {
                    if (currentUser.UserGuid != cart.UserGuid)
                    {
                        cart.ClearCustomerData();
                        cart.OrderInfo.CustomerEmail = currentUser.Email;
                        cart.OrderInfo.CustomerFirstName = currentUser.FirstName;
                        cart.OrderInfo.CustomerLastName = currentUser.LastName;
                        cart.OrderInfo.CustomerTelephoneDay = currentUser.PhoneNumber;
                        cart.UserGuid = currentUser.UserGuid;
                        cart.OrderInfo.Save();
                        cart.Save();

                    }
                }

            }
            else
            {
                cart.UserGuid = Guid.Empty;
                cart.Save();
            }
            

        }

        


        


        private static Cart CreateCartAndSetCookie(Store store)
        {
            if (store == null) return null;
            if (store.Guid == Guid.Empty) return null;

            string cartKey = "cart" + store.Guid.ToString();
            Cart cart = new Cart();
            cart.StoreGuid = store.Guid;
            cart.CreatedFromIP = SiteUtils.GetIP4Address();
            
            if (
                (HttpContext.Current != null)
                && (HttpContext.Current.Request.IsAuthenticated)
                )
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                cart.UserGuid = siteUser.UserGuid;
                cart.LoadExistingUserCartIfExists();

            }
            cart.Save();

            SetCartCookie(store.Guid, cart.CartGuid);
            HttpContext.Current.Items[cartKey] = cart;

            return cart;


        }


        public static bool UserHasCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return false;
            string cartKey = "cart" + storeGuid.ToString();

            return CookieHelper.CookieExists(cartKey);

        }

        public static void SetCartCookie(Guid storeGuid, Guid cartGuid)
        {
            if ((storeGuid != Guid.Empty) && (cartGuid != Guid.Empty))
            {
                string cartKey = "cart" + storeGuid.ToString();

                // TODO encrypt, sign?

                CookieHelper.SetPersistentCookie(cartKey, cartGuid.ToString());


            }

        }

        public static void ClearCartCookie(Guid storeGuid)
        {
            if (storeGuid != Guid.Empty)
            {
                string cartKey = "cart" + storeGuid.ToString();

                // TODO encrypt, sign?

                CookieHelper.SetPersistentCookie(cartKey, string.Empty);


            }

        }

        public static string GetCartCookie(Guid storeGuid)
        {
            if (storeGuid == Guid.Empty) return string.Empty;

            string cartKey = "cart" + storeGuid.ToString();

            // TODO: decrypt and verify?

            return CookieHelper.GetCookieValue(cartKey);


        }


        public static Cart GetClerkCart(Store store)
        {

            if (HttpContext.Current == null) return null;
            if (store == null) return null;

            string cartKey = "clerkcart" + store.Guid.ToString();
            if (HttpContext.Current.Items[cartKey] != null)
            {
                return (Cart)HttpContext.Current.Items[cartKey];
            }

            if (CookieHelper.CookieExists(cartKey))
            {
                string cartCookie = CookieHelper.GetCookieValue(cartKey);
                if (cartCookie.Length == 36)
                {
                    Guid cartGuid = new Guid(cartCookie);
                    Cart cart = new Cart(cartGuid);
                    if (!cart.Exists)
                    {
                        return CreateClerkCartAndSetCookie(store, cartKey);
                    }
                    
                    HttpContext.Current.Items[cartKey] = cart;
                    return cart;

                }
                else
                {
                    // cookie is invalid
                    return CreateClerkCartAndSetCookie(store, cartKey);
                }
            }
            else
            {

                return CreateClerkCartAndSetCookie(store, cartKey);

            }
            

        }

        private static Cart CreateClerkCartAndSetCookie(Store store, string cartKey)
        {
            if (store == null) return null;
            if (store.Guid == Guid.Empty) return null;

            Cart cart = new Cart();
            cart.StoreGuid = store.Guid;
            cart.CreatedFromIP = SiteUtils.GetIP4Address();
           
            if (
                (HttpContext.Current != null)
                && (HttpContext.Current.Request.IsAuthenticated)
                )
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                cart.ClerkGuid = siteUser.UserGuid;
                
            }
            cart.Save();

            CookieHelper.SetPersistentCookie(cartKey, cart.CartGuid.ToString());
            
            HttpContext.Current.Items[cartKey] = cart;

            return cart;


        }

        public static void ClearClerkCartCookie(Guid storeGuid)
        {
            if (storeGuid != Guid.Empty)
            {
                string cartKey = "clerkcart" + storeGuid.ToString();
                CookieHelper.SetPersistentCookie(cartKey, string.Empty);
            }

        }

        //public static Cart GetCart(Guid storeGuid)
        //{


        //    if (HttpContext.Current != null)
        //    {
        //        string cartKey = "cart" + storeGuid.ToString();
        //        if (HttpContext.Current.Items[cartKey] != null)
        //        {
        //            return (Cart)HttpContext.Current.Items[cartKey];
        //        }
        //        else
        //        {
        //            if (UserHasCartCookie(storeGuid))
        //            {
        //                string cartCookie = GetCartCookie(storeGuid);
        //                if (cartCookie.Length == 36)
        //                {
        //                    Guid cartGuid = new Guid(cartCookie);
        //                    Cart cart = new Cart(cartGuid);
        //                    if (!cart.Exists)
        //                    {
        //                        return CreateCartAndSetCookie(storeGuid);
        //                    }
        //                    HttpContext.Current.Items[cartKey] = cart;
        //                    return cart;

        //                }
        //                else
        //                {
        //                    // cookie is invalid
        //                    return CreateCartAndSetCookie(storeGuid);
        //                }
        //            }
        //            else
        //            {
        //                // TODO: handle use case where user adds to cart on 1 machine
        //                // then comes back to site on another machine and has no cart cookie
        //                // look for a cart that has the userguid, 
        //                // if found set cookie for that cart

        //                // new cart
        //                return CreateCartAndSetCookie(storeGuid);

        //            }

        //        }

        //    }

        //    return null;


        //}

        public static void InitializeOrderInfo(Cart cart, SiteUser siteUser)
        {
            if (cart.OrderInfo.CustomerLastName.Length == 0)
            {
                if(siteUser.LastName.Length > 0)
                {
                    cart.OrderInfo.CustomerLastName = siteUser.LastName;
                }
                else
                {
                    cart.OrderInfo.CustomerLastName = siteUser.Name;
                }
                cart.OrderInfo.CustomerFirstName = siteUser.FirstName;
                
            }
            cart.OrderInfo.CustomerTelephoneDay = siteUser.PhoneNumber;
            cart.OrderInfo.CustomerEmail = siteUser.Email;
            cart.OrderInfo.Save();
            cart.UserGuid = siteUser.UserGuid;
            cart.Save();

        }

        //public static string GetFormattedAmount(
        //    decimal amount, 
        //    bool formatWithCurrencySymbol,
        //    Currency currency,
        //    CultureInfo cultureInfo)
        //{
            
        //    if (formatWithCurrencySymbol)
        //    {
        //        return string.Format("{0} {1}",
        //            currency.SymbolLeft, 
        //            decimal.Round(amount, 
        //            cultureInfo.NumberFormat.CurrencyDecimalDigits).ToString(cultureInfo)
        //            );
        //    }
        //    else
        //    {
        //        return decimal.Round(
        //            amount, 
        //            cultureInfo.NumberFormat.CurrencyDecimalDigits).ToString(cultureInfo);
        //    }
        //}


        /// <summary>
        /// The goal here is to make sure we always have a PayPalLog with current cart serialized.
        /// We need to do this because we could transfer the user to PayPal, then they add more items to the cart,
        /// then complete the checkout at PayPal for the previous version of the cart.
        /// If we were to just use the current cart from the db we might give them more or less than they actually
        /// bought. By keeping a copy of the cart as it was at time of transfer, we can be sure
        /// we only give them the products they had at checkout time.
        /// </summary>
        public static PayPalLog EnsurePayPalStandardCheckoutLog(
            Cart cart,
            Store store,
            string siteRoot,
            int pageId,
            int moduleId)
        {
            if (cart == null) return null;
            if (store == null) return null;

            PayPalLog payPalLog = PayPalLog.GetMostRecent(cart.CartGuid, "StandardCheckout");
            if ((payPalLog == null) || (payPalLog.SerializedObject.Length == 0))
            {
                return CreatePayPalStandardCheckoutLog(cart, store, siteRoot, pageId, moduleId);
            }

            Cart payPalCart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), payPalLog.SerializedObject);

            if (payPalCart.LastModified < cart.LastModified)
            {
                // cart has been modified since we last serialized it so create a new one
                return CreatePayPalStandardCheckoutLog(cart, store, siteRoot, pageId, moduleId);
            }


            return payPalLog;

        }

        private static PayPalLog CreatePayPalStandardCheckoutLog(
            Cart cart,
            Store store,
            string siteRoot,
            int pageId,
            int moduleId)
        {
            PayPalLog payPalLog = new PayPalLog();

            payPalLog.ProviderName = "WebStorePayPalHandler";
            payPalLog.PDTProviderName = "WebStorePayPalPDTHandlerProvider";
            payPalLog.IPNProviderName = "WebStorePayPalIPNHandlerProvider";
            payPalLog.ReturnUrl = siteRoot +
                                "/WebStore/OrderDetail.aspx?pageid="
                                + pageId.ToInvariantString()
                                + "&mid=" + moduleId.ToInvariantString()
                                + "&orderid=" + cart.CartGuid.ToString();

            payPalLog.RequestType = "StandardCheckout";

            cart.SerializeCartOffers();
            payPalLog.SerializedObject = SerializationHelper.SerializeToString(cart);

            //Currency currency = new Currency(store.DefaultCurrencyId);

            payPalLog.CartGuid = cart.CartGuid;
            //Store store = new Store(cart.StoreGuid);
            payPalLog.SiteGuid = store.SiteGuid;
            payPalLog.StoreGuid = store.Guid;
            payPalLog.UserGuid = cart.UserGuid;
            payPalLog.CartTotal = cart.OrderTotal;
            //payPalLog.CurrencyCode = currency.Code;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            payPalLog.CurrencyCode = siteSettings.GetCurrency().Code;

            payPalLog.Save();

            return payPalLog;

        }

        /// <summary>
        /// we are using the paypal log with a different request type rather than making a new log specifically for WorldPay
        /// 
        /// The goal here is to make sure we always have a PayPalLog with current cart serialized.
        /// We need to do this because we could transfer the user to WorldPay, then they add more items to the cart,
        /// then complete the checkout at WorldPay for the previous version of the cart.
        /// If we were to just use the current cart from the db we might give them more or less than they actually
        /// bought. By keeping a copy of the cart as it was at time of transfer, we can be sure
        /// we only give them the products they had at checkout time.
        /// </summary>
        public static PayPalLog EnsureWorldPayCheckoutLog(
            Cart cart,
            Store store,
            string siteRoot,
            string storePageUrl,
            int pageId,
            int moduleId)
        {
            if (cart == null) return null;
            if (store == null) return null;

            PayPalLog worldPayLog = PayPalLog.GetMostRecent(cart.CartGuid, "WorldPay");
            if ((worldPayLog == null) || (worldPayLog.SerializedObject.Length == 0))
            {
                return CreateWorldPayCheckoutLog(cart, store, siteRoot, storePageUrl, pageId, moduleId);
            }

            Cart worldPayCart = (Cart)SerializationHelper.DeserializeFromString(typeof(Cart), worldPayLog.SerializedObject);

            if (worldPayCart.LastModified < cart.LastModified)
            {
                // cart has been modified since we last serialized it so create a new one
                return CreateWorldPayCheckoutLog(cart, store, siteRoot, storePageUrl, pageId, moduleId);
            }


            return worldPayLog;

        }

        // we are using the paypal log with a different request type rather than making a new log specifically for WorldPay
        private static PayPalLog CreateWorldPayCheckoutLog(
            Cart cart,
            Store store,
            string siteRoot,
            string storePageUrl,
            int pageId,
            int moduleId)
        {
            PayPalLog worldPayLog = new PayPalLog();

            worldPayLog.ProviderName = "WebStoreWorldPayResponseHandler";
            worldPayLog.RequestType = "WorldPay";
            worldPayLog.RawResponse = storePageUrl;

            worldPayLog.ReturnUrl = siteRoot +
                                "/WebStore/OrderDetail.aspx?pageid="
                                + pageId.ToInvariantString()
                                + "&mid=" + moduleId.ToInvariantString()
                                + "&orderid=" + cart.CartGuid.ToString();

            cart.SerializeCartOffers();
            worldPayLog.SerializedObject = SerializationHelper.SerializeToString(cart);

            worldPayLog.CartGuid = cart.CartGuid;

            worldPayLog.SiteGuid = store.SiteGuid;
            worldPayLog.StoreGuid = store.Guid;
            worldPayLog.UserGuid = cart.UserGuid;
            worldPayLog.CartTotal = cart.OrderTotal;
            
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            worldPayLog.CurrencyCode = siteSettings.GetCurrency().Code;

            worldPayLog.Save();

            return worldPayLog;

        }

        /// <summary>
        /// Creates the URL for PayPal Standard BuyNow button
        /// </summary>
        /// <returns></returns>
        public static string GetBuyNowUrl(
            Guid payPalLogGuid,
            Cart cart, 
            Store store,
            CommerceConfiguration commerceConfig)
        {

            //PayPalStandardPaymentGateway gateway = new PayPalStandardPaymentGateway();
            //gateway.PayPalStandardUrl = commerceConfig.PayPalStandardUrl;
            //gateway.BusinessEmail = commerceConfig.PayPalStandardEmailAddress;
            //gateway.PDTId = commerceConfig.PayPalStandardPDTId;

            PayPalStandardPaymentGateway gateway
                = new PayPalStandardPaymentGateway(
                    commerceConfig.PayPalStandardUrl,
                    commerceConfig.PayPalStandardEmailAddress,
                    commerceConfig.PayPalStandardPDTId);
            
            
            gateway.Amount = cart.OrderTotal;
            gateway.Tax = cart.TaxTotal;
            gateway.Shipping = cart.ShippingTotal;

            //Currency currency = new Currency(cart.CurrencyGuid);
            //gateway.CurrencyCode = store.DefaultCurrency;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            gateway.CurrencyCode = siteSettings.GetCurrency().Code;

            gateway.OrderHasShippableProducts = cart.HasShippingProducts();

            // TODO: guess we need to split this into first and last
            gateway.ShippingFirstName =  cart.OrderInfo.DeliveryFirstName;
            gateway.ShippingLastName = cart.OrderInfo.DeliveryLastName;

            gateway.ShippingAddress1 = cart.OrderInfo.DeliveryAddress1;
            gateway.ShippingAddress2 = cart.OrderInfo.DeliveryAddress2;
            gateway.ShippingCity = cart.OrderInfo.DeliveryCity;
            gateway.ShippingState = cart.OrderInfo.DeliveryState;
            gateway.ShippingPostalCode = cart.OrderInfo.DeliveryPostalCode;

            //add the items
            //foreach (CartOffer offer in cart.CartOffers)
            //{
            //    PayPalOrderItem item = new PayPalOrderItem();
            //    item.Amount = offer.OfferPrice;
            //    item.ItemName = offer.Name;
            //    item.ItemNumber = offer.OfferGuid.ToString();
            //    item.Quantity = offer.Quantity;

            //    gateway.Items.Add(item);

            //}

            gateway.OrderDescription = store.Name + " " + WebStoreResources.OrderHeading;

            gateway.Custom = payPalLogGuid.ToString();

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string storePageUrl = SiteUtils.GetCurrentPageUrl();
  
            gateway.ReturnUrl = siteRoot + "/Services/PayPalPDTHandler.aspx";
            gateway.NotificationUrl = siteRoot + "/Services/PayPalIPNHandler.aspx";
            gateway.CancelUrl = storePageUrl;

            return gateway.GetBuyNowButtonUrl();

        }

        /// <summary>
        /// Creates hidden form fields for PayPal Standard Cart upload
        /// </summary>
        /// <returns></returns>
        public static string GetCartUploadFormFields(
            Guid payPalLogGuid,
            Cart cart,
            Store store,
            CommerceConfiguration commerceConfig)
        {
            //TODO: PayPal is not seeing discounts?

            PayPalStandardPaymentGateway gateway
                = new PayPalStandardPaymentGateway(
                    commerceConfig.PayPalStandardUrl,
                    commerceConfig.PayPalStandardEmailAddress,
                    commerceConfig.PayPalStandardPDTId);


            gateway.Amount = cart.OrderTotal;
            gateway.Tax = cart.TaxTotal;
            gateway.Shipping = cart.ShippingTotal;
            gateway.CartDiscount = cart.Discount;
            
            
            //Currency currency = new Currency(cart.CurrencyGuid);
            //if (currency.Guid != Guid.Empty)
            //{
            //    gateway.CurrencyCode = currency.Code;
            //}
            //gateway.CurrencyCode = store.DefaultCurrency;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            gateway.CurrencyCode = siteSettings.GetCurrency().Code;

            gateway.OrderHasShippableProducts = cart.HasShippingProducts();
            gateway.ShippingFirstName = cart.OrderInfo.DeliveryFirstName;
            gateway.ShippingLastName = cart.OrderInfo.DeliveryLastName;

            gateway.ShippingAddress1 = cart.OrderInfo.DeliveryAddress1;
            gateway.ShippingAddress2 = cart.OrderInfo.DeliveryAddress2;
            gateway.ShippingCity = cart.OrderInfo.DeliveryCity;
            gateway.ShippingState = cart.OrderInfo.DeliveryState;
            gateway.ShippingPostalCode = cart.OrderInfo.DeliveryPostalCode;

            
            //add the items
            foreach (CartOffer offer in cart.CartOffers)
            {
                PayPalOrderItem item = new PayPalOrderItem();
                item.Amount = offer.OfferPrice;
                item.ItemName = offer.Name;
                item.ItemNumber = offer.OfferGuid.ToString();
                item.Quantity = offer.Quantity;
                item.Tax = offer.Tax;
                
                gateway.Items.Add(item);

            }

            gateway.OrderDescription = store.Name + " " + WebStoreResources.OrderHeading;

            gateway.Custom = payPalLogGuid.ToString();

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            string storePageUrl = SiteUtils.GetCurrentPageUrl();

            gateway.ReturnUrl = siteRoot + "/Services/PayPalPDTHandler.aspx";
            gateway.NotificationUrl = siteRoot + "/Services/PayPalIPNHandler.aspx";
            gateway.CancelUrl = storePageUrl;

            return gateway.GetCartUploadFormFields();

        }


    }
}
