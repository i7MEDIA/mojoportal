// Author:					Joe Audette
// Created:				    2007-02-19
// Last Modified:			2015-04-13 (Joe Davis)
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
using System.Collections.ObjectModel;
using System.Data;
using WebStore.Data;
using mojoPortal.Business;
using mojoPortal.Web.Framework;


namespace WebStore.Business
{
    /// <summary>
    /// Represents a store.
    /// </summary>
    public class Store
    {
        private const string featureGuid = "0cefbf18-56de-11dc-8f36-bac755d89593";

        public static Guid FeatureGuid
        {
            get { return new Guid(featureGuid); }
        }

        #region Constructors

        public Store()
        { }

        public Store(Guid guid)
        {
            GetStore(guid);
        }

        public Store(Guid siteGuid, int moduleId)
        {
            GetStore(moduleId);
            if (
                (this.SiteGuid != Guid.Empty)
                &&(this.SiteGuid != siteGuid)
                )
            {
                throw new ArgumentException("Invalid Site Guid for this store");
            }
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private int moduleID = -1;
        private Guid moduleGuid = Guid.Empty;
        private string name = string.Empty;
        private string description = string.Empty;
        private string ownerName = string.Empty;
        private string ownerEmail = string.Empty;
        private string salesEmail = string.Empty;
        private string supportEmail = string.Empty;
        private string emailFrom = string.Empty;
        private string orderBCCEmail = string.Empty;
        private string phone = string.Empty;
        private string fax = string.Empty;
        private string address = string.Empty;
        private string city = string.Empty;
        private Guid zoneGuid = Guid.Empty;
        private string postalCode = string.Empty;
        private Guid countryGuid = Guid.Empty;
        private bool isClosed = false;
        private string closedMessage = string.Empty;
        
        private DateTime created = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;


        private bool alwaysUseCustomerInfoForBilling = false;
        private bool requireRegistrationForPurchase = true;
        private bool requireRegistrationForDownloadProducts = true;
        private bool requireRegistrationForShippedProducts = ConfigHelper.GetBoolProperty("WebStore:RequireRegistrationForShippedProducts", false);
        private bool requireRegistrationForNoFulfillmentProducts = ConfigHelper.GetBoolProperty("WebStore:RequireRegistrationForNoFulfillmentProducts", false);

        //TODO: more research on a general money solution
        //http://lindelauf.com/?p=17
        //http://moneytype.codeplex.com/
        //http://blogs.msdn.com/bclteam/archive/2007/05/29/bcl-refresher-floating-point-types-the-good-the-bad-and-the-ugly-inbar-gazit-matthew-greig.aspx

        private MidpointRounding roundingMode = MidpointRounding.AwayFromZero;
        private int roundingDecimalPlaces = 2;

        //private string currencyFormatString = "0.00";

        //private Currency currentCurrency = null;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
        }
        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public MidpointRounding RoundingMode
        {
            get { return roundingMode; }
            set { roundingMode = value; }
        }

        public int RoundingDecimalPlaces
        {
            get { return roundingDecimalPlaces; }
            set { roundingDecimalPlaces = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        
        
        public string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }
        public string OwnerEmail
        {
            get { return ownerEmail; }
            set { ownerEmail = value; }
        }
        public string SalesEmail
        {
            get { return salesEmail; }
            set { salesEmail = value; }
        }
        public string SupportEmail
        {
            get { return supportEmail; }
            set { supportEmail = value; }
        }
        public string EmailFrom
        {
            get { return emailFrom; }
            set { emailFrom = value; }
        }
        public string OrderBccEmail
        {
            get { return orderBCCEmail; }
            set { orderBCCEmail = value; }
        }
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public Guid ZoneGuid
        {
            get { return zoneGuid; }
            set { zoneGuid = value; }
        }
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }
        public Guid CountryGuid
        {
            get { return countryGuid; }
            set { countryGuid = value; }
        }
        public bool IsClosed
        {
            get { return isClosed; }
            set { isClosed = value; }
        }
        public string ClosedMessage
        {
            get { return closedMessage; }
            set { closedMessage = value; }
        }
        
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        public bool RequireRegistrationForDownloadProducts
        {
            get { return requireRegistrationForDownloadProducts; }
            set { requireRegistrationForDownloadProducts = value; }
        }

        public bool RequireRegistrationForShippedProducts
        {
            get { return requireRegistrationForShippedProducts; }
            set { requireRegistrationForShippedProducts = value; }
        }

        public bool RequireRegistrationForNoFulfillmentProducts
        {
            get { return requireRegistrationForNoFulfillmentProducts; }
            set { requireRegistrationForNoFulfillmentProducts = value; }
        }
        

        

        #endregion

        #region Checkout Settings

        public bool AlwaysUseCustomerInfoForBilling
        {
            get { return alwaysUseCustomerInfoForBilling; }
            set { alwaysUseCustomerInfoForBilling = value; }
        }

        public bool RequireRegistrationForPurchase
        {
            get { return requireRegistrationForPurchase; }
            set { requireRegistrationForPurchase = value; }
        }

        #endregion

        #region Private Methods

        private void GetStore(Guid guid)
        {
            if (guid == Guid.Empty) return;

            using (IDataReader reader = DBStore.Get(guid))
            {
                GetStore(reader);
            }
        }

        private void GetStore(int moduleId)
        {
            if (moduleId < 0) return;

            using (IDataReader reader = DBStore.Get(moduleId))
            {
                GetStore(reader);
            }
        }

        private void GetStore(IDataReader reader)
        {
           
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.moduleID = Convert.ToInt32(reader["ModuleID"]);
                this.name = reader["Name"].ToString();
                this.description = reader["Description"].ToString();

                this.ownerName = reader["OwnerName"].ToString();
                this.ownerEmail = reader["OwnerEmail"].ToString();
                this.salesEmail = reader["SalesEmail"].ToString();
                this.supportEmail = reader["SupportEmail"].ToString();
                this.emailFrom = reader["EmailFrom"].ToString();
                this.orderBCCEmail = reader["OrderBCCEmail"].ToString();
                this.phone = reader["Phone"].ToString();
                this.fax = reader["Fax"].ToString();
                this.address = reader["Address"].ToString();
                this.city = reader["City"].ToString();
                this.zoneGuid = new Guid(reader["ZoneGuid"].ToString());
                this.postalCode = reader["PostalCode"].ToString();
                this.countryGuid = new Guid(reader["CountryGuid"].ToString());
                this.isClosed = Convert.ToBoolean(reader["IsClosed"]);
                this.closedMessage = reader["ClosedMessage"].ToString();
                
               
                this.created = Convert.ToDateTime(reader["Created"]);
                this.createdBy = new Guid(reader["CreatedBy"].ToString());

            }

            

        }

        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBStore.Add(
                this.guid,
                this.siteGuid,
                this.moduleID,
                this.name,
                this.description,
                this.ownerName,
                this.ownerEmail,
                this.salesEmail,
                this.supportEmail,
                this.emailFrom,
                this.orderBCCEmail,
                this.phone,
                this.fax,
                this.address,
                this.city,
                this.zoneGuid,
                this.postalCode,
                this.countryGuid,
                this.isClosed,
                this.closedMessage,
                this.created,
                this.createdBy);

           

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBStore.Update(
                this.guid,
                this.name,
                this.description,
                this.ownerName,
                this.ownerEmail,
                this.salesEmail,
                this.supportEmail,
                this.emailFrom,
                this.orderBCCEmail,
                this.phone,
                this.fax,
                this.address,
                this.city,
                this.zoneGuid,
                this.postalCode,
                this.countryGuid,
                this.isClosed,
                this.closedMessage);
        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.guid == Guid.Empty)
            {
                return Create();
            }
            else
            {
                return Update();
            }
        }

        

        public bool IsValidForCheckout(Cart cart, out ArrayList errorList)
        {
            bool result = true;
            // TODO: implement rule enforcement
            errorList = new ArrayList();



            return result;

        }

        public void LoadCustomerInfoFromMostRecentOrder(Cart cart)
        {
            Order lastOrder = Order.GetMostRecentOrder(this.guid, cart.UserGuid);
            if (lastOrder != null)
            {
                cart.OrderInfo.CustomerAddressLine1 = lastOrder.CustomerAddressLine1;
                cart.OrderInfo.CustomerAddressLine2 = lastOrder.CustomerAddressLine2;
                cart.OrderInfo.CustomerCity = lastOrder.CustomerCity;
                cart.OrderInfo.CustomerCompany = lastOrder.CustomerCompany;
                cart.OrderInfo.CustomerCountry = lastOrder.CustomerCountry;
                cart.OrderInfo.CustomerEmail = lastOrder.CustomerEmail;
                cart.OrderInfo.CustomerEmailVerified = lastOrder.CustomerEmailVerified;
                cart.OrderInfo.CustomerFirstName = lastOrder.CustomerFirstName;
                cart.OrderInfo.CustomerLastName = lastOrder.CustomerLastName;
                cart.OrderInfo.CustomerPostalCode = lastOrder.CustomerPostalCode;
                cart.OrderInfo.CustomerState = lastOrder.CustomerState;
                cart.OrderInfo.CustomerSuburb = lastOrder.CustomerSuburb;
                cart.OrderInfo.CustomerTelephoneDay = lastOrder.CustomerTelephoneDay;
                cart.OrderInfo.CustomerTelephoneNight = lastOrder.CustomerTelephoneNight;

                cart.OrderInfo.BillingAddress1 = lastOrder.CustomerAddressLine1;
                cart.OrderInfo.BillingAddress2 = lastOrder.CustomerAddressLine2;
                cart.OrderInfo.BillingCity = lastOrder.CustomerCity;
                cart.OrderInfo.BillingCompany = lastOrder.CustomerCompany;
                cart.OrderInfo.BillingCountry = lastOrder.CustomerCountry;
                cart.OrderInfo.BillingFirstName = lastOrder.CustomerFirstName;
                cart.OrderInfo.BillingLastName = lastOrder.CustomerLastName;
                cart.OrderInfo.BillingPostalCode = lastOrder.CustomerPostalCode;
                cart.OrderInfo.BillingState = lastOrder.CustomerState;
                cart.OrderInfo.BillingSuburb = lastOrder.CustomerSuburb;

                cart.OrderInfo.DeliveryAddress1 = lastOrder.CustomerAddressLine1;
                cart.OrderInfo.DeliveryAddress2 = lastOrder.CustomerAddressLine2;
                cart.OrderInfo.DeliveryCity = lastOrder.CustomerCity;
                cart.OrderInfo.DeliveryCompany = lastOrder.CustomerCompany;
                cart.OrderInfo.DeliveryCountry = lastOrder.CustomerCountry;
                cart.OrderInfo.DeliveryFirstName = lastOrder.CustomerFirstName;
                cart.OrderInfo.DeliveryLastName = lastOrder.CustomerLastName;
                cart.OrderInfo.DeliveryPostalCode = lastOrder.CustomerPostalCode;
                cart.OrderInfo.DeliveryState = lastOrder.CustomerState;
                cart.OrderInfo.DeliverySuburb = lastOrder.CustomerSuburb;
                cart.OrderInfo.TaxZoneGuid = lastOrder.TaxZoneGuid;
                
                cart.OrderInfo.Save();


            }


        }


        public void CalculateTax(Cart cart)
        {
            // TODO: this doesn't take into account discounts

            decimal taxAmount = 0;

            if(cart.OrderInfo.TaxZoneGuid == Guid.Empty)
            {
                GeoCountry country = new GeoCountry(cart.OrderInfo.BillingCountry);
                GeoZone taxZone = GeoZone.GetByCode(country.Guid, cart.OrderInfo.BillingState);
                if (taxZone != null)
                {
                    cart.OrderInfo.TaxZoneGuid = taxZone.Guid;
                }
            }

            if (cart.OrderInfo.TaxZoneGuid != Guid.Empty)
            {
                Collection<TaxRate> taxRates = TaxRate.GetTaxRates(this.SiteGuid, cart.OrderInfo.TaxZoneGuid);
                if (taxRates.Count > 0)
                {
                    foreach (CartOffer offer in cart.CartOffers)
                    {
                        offer.Tax = 0;

                        foreach (TaxRate taxRate in taxRates)
                        {
                            if (offer.TaxClassGuid == taxRate.TaxClassGuid)
                            {
                                offer.Tax += (taxRate.Rate * (offer.OfferPrice * offer.Quantity));
                                offer.Save();
                                taxAmount += offer.Tax;
                                //break;
                            }
                        }
                    }
                }
            }

            cart.TaxTotal = Math.Round(taxAmount, this.RoundingDecimalPlaces, this.RoundingMode);
            if (cart.TaxTotal < 0) { cart.TaxTotal = 0; }
            cart.Save();

        }

        //public decimal CalculateItemTax(
        //    Guid siteGuid,
        //    Guid taxZoneGuid, 
        //    Guid taxClassGuid, 
        //    decimal itemPrice,
        //    int quantity)
        //{
        //    decimal taxAmount = 0;
        //    if (taxZoneGuid == Guid.Empty) { return taxAmount; }
        //    if (taxClassGuid == Guid.Empty) { return taxAmount; }
        //    if (itemPrice == 0) { return taxAmount; }

            
        //    Collection<TaxRate> taxRates = TaxRate.GetTaxRates(siteGuid, taxZoneGuid);
        //    if (taxRates.Count > 0)
        //    {
        //        foreach (TaxRate taxRate in taxRates)
        //        {
        //            if (taxClassGuid == taxRate.TaxClassGuid)
        //            {
        //                taxAmount += (taxRate.Rate * (itemPrice * quantity));
        //                break;
        //            }
        //        }
                
        //    }
        //    return taxAmount;

        //}

        public void CalculateShipping(Cart cart)
        {
            decimal shippingAmount = 0;

            foreach (CartOffer offer in cart.CartOffers)
            {
                Collection<OfferProduct> offerProducts = OfferProduct.GetbyOffer(offer.OfferGuid);

                if (offerProducts != null)
                {
                    foreach (OfferProduct offerProduct in offerProducts)
                    {
                        Product product = new Product(offerProduct.ProductGuid);
                        if (product != null)
                        {
                            shippingAmount += (product.ShippingAmount * offer.Quantity);
                        }
                    }
                }
            }
            
            cart.ShippingTotal = Math.Round(shippingAmount, this.RoundingDecimalPlaces, this.RoundingMode);
            cart.Save();

        }

        /// <summary>
        /// Returns a DataSet designed for use with a nested repeater to show products with the offers they are available in.
        /// We want to browse by product, but only offers can be added to a cart as products are only sold through offers.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public DataSet GetProductPageWithOffers(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            DataSet dataSet = new DataSet();

            DataTable productsTable = Product.GetPage(this.guid, pageNumber, pageSize, out totalPages);
            productsTable.TableName = "Products";
            dataSet.Tables.Add(productsTable);

            DataTable productOffersTable = Offer.GetListForPageOfProducts(this.guid, pageNumber, pageSize);
            productOffersTable.TableName = "ProductOffers";
            dataSet.Tables.Add(productOffersTable);

            // create a releationship
            dataSet.Relations.Add("prodoffers", 
                dataSet.Tables["Products"].Columns["Guid"],
                dataSet.Tables["ProductOffers"].Columns["ProductGuid"]);


            return dataSet;
        }

        /// <summary>
        /// Returns a DataSet designed for use with a nested repeater to show offers with the products they contain.
        /// We want to browse by offer, only offers can be added to a cart as products are only sold through offers.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public DataSet GetOfferPageWithProducts(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            DataSet dataSet = new DataSet();

            DataTable offersTable = Offer.GetPublicPage(this.guid, pageNumber, pageSize, out totalPages);
            offersTable.TableName = "Offers";
            dataSet.Tables.Add(offersTable);

            DataTable offerProductsTable = Product.GetListForPageOfOffers(this.guid, pageNumber, pageSize);
            offerProductsTable.TableName = "OfferProducts";
            dataSet.Tables.Add(offerProductsTable);

            // create a releationship
            dataSet.Relations.Add("offerprods",
                dataSet.Tables["Offers"].Columns["Guid"],
                dataSet.Tables["OfferProducts"].Columns["OfferGuid"]);


            return dataSet;
        }

        public bool CanCheckoutWithoutAuthentication(Cart cart)
        {
            if (cart == null) { return false; }
            if((cart.HasDownloadProducts())&&(this.requireRegistrationForDownloadProducts)) { return false; }
            if((cart.HasDonations())&&(this.requireRegistrationForNoFulfillmentProducts)) { return false; }
            if((cart.HasShippingProducts())&&(this.requireRegistrationForShippedProducts)) { return false; }

            return true;
        }

        #endregion

        #region Static Methods

        public static bool Delete(Guid guid)
        {
            return DBStore.Delete(guid);
        }

        public static bool DeleteByModule(int moduleId)
        {
            return DBStore.DeleteByModule(moduleId);
        }

        public static bool DeleteBySite(int siteId)
        {
            SiteSettings site = new SiteSettings(siteId);
            Discount.DeleteBySite(site.SiteGuid);

            return DBStore.DeleteBySite(siteId);
        }

        

        #endregion


    }

}
