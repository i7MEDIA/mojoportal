/// Author:					Joe Audette
/// Created:				2007-03-17
/// Last Modified:			2012-01-21
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Data;
using WebStore.Data;
using mojoPortal.Business.Commerce;
using mojoPortal.Business.WebHelpers.PaymentGateway;


namespace WebStore.Business
{
    /// <summary>
    /// Represents an order.
    /// </summary>
    public class Order
    {

        #region Constructors

        public Order()
        { }

        
        public Order(Guid orderGuid)
        {
            GetOrder(orderGuid);
        }

        #endregion

        #region Private Properties

        private Guid orderGuid = Guid.Empty;
        bool isExisting = false;
        private int orderNo;
        private Guid storeGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        //private string customerName;
        private string customerFirstName = string.Empty;
        private string customerLastName = string.Empty;

        private string customerCompany = string.Empty;
        private string customerAddressLine1 = string.Empty;
        private string customerAddressLine2 = string.Empty;
        private string customerSuburb = string.Empty;
        private string customerCity = string.Empty;
        private string customerPostalCode = string.Empty;
        private string customerState = string.Empty;
        private string customerCountry = string.Empty;
        private string customerTelephoneDay = string.Empty;
        private string customerTelephoneNight = string.Empty;
        private string customerEmail = string.Empty;
        private bool customerEmailVerified = false;
        //private string deliveryName;
        private string deliveryFirstName = string.Empty;
        private string deliveryLastName = string.Empty;

        private string deliveryCompany = string.Empty;
        private string deliveryAddress1 = string.Empty;
        private string deliveryAddress2 = string.Empty;
        private string deliverySuburb = string.Empty;
        private string deliveryCity = string.Empty;
        private string deliveryPostalCode = string.Empty;
        private string deliveryState = string.Empty;
        private string deliveryCountry = string.Empty;
        //private string billingName;
        private string billingFirstName = string.Empty;
        private string billingLastName = string.Empty;

        private string billingCompany = string.Empty;
        private string billingAddress1 = string.Empty;
        private string billingAddress2 = string.Empty;
        private string billingSuburb = string.Empty;
        private string billingCity = string.Empty;
        private string billingPostalCode = string.Empty;
        private string billingState = string.Empty;
        private string billingCountry = string.Empty;
        private Guid cardTypeGuid = Guid.Empty;
        private string cardOwner = string.Empty;
        private string cardNumber = string.Empty;
        private string cardExpires = string.Empty;
        private string cardSecurityCode = string.Empty;
       
        private decimal subTotal = 0;
        private decimal taxTotal = 0;
        private decimal shippingTotal = 0;
        private decimal discount = 0;
        private decimal orderTotal = 0;
        private DateTime created =DateTime.UtcNow;
        private string createdFromIP = string.Empty;
        private DateTime completed = DateTime.UtcNow;
        private string completedFromIP = string.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private DateTime lastUserActivity = DateTime.UtcNow;
        private Guid statusGuid = Guid.Empty;
        private string gatewayTransID = string.Empty;
        private string gatewayRawResponse = string.Empty;
        private string gatewayAuthCode = string.Empty;
        private Guid taxZoneGuid = Guid.Empty;
        private string paymentMethod = string.Empty;
        private bool analyticsTracked = false;
        //private Guid languageGuid = Guid.Empty;
        //private string currencyCode = "USD";

        private List<OrderOffer> orderOffers = new List<OrderOffer>();

        private string discountCodesCsv = string.Empty;
        private string customData = string.Empty;

        private Guid clerkGuid = Guid.Empty;

        #endregion

        

        #region Public Properties

        /// <summary>
        ///  Represents the collection of offers in this order
        /// </summary>
        public List<OrderOffer> OrderOffers
        {
            get
            {
                if (orderOffers == null)
                {
                    orderOffers = OrderOffer.GetByOrder(this.orderGuid);
                }

                if (orderOffers.Count == 0)
                {
                    orderOffers = OrderOffer.GetByOrder(this.orderGuid);
                }

                return orderOffers;
            }
        }

        //public Guid LanguageGuid
        //{
        //    get { return languageGuid; }
        //    set { languageGuid = value; }
        //}

        public Guid OrderGuid
        {
            get { return orderGuid; }
            set { orderGuid = value; }
        }
        public int OrderNo
        {
            get { return orderNo; }
            set { orderNo = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public Guid ClerkGuid
        {
            get { return clerkGuid; }
            set { clerkGuid = value; }
        }

        //public string CurrencyCode
        //{
        //    get { return currencyCode; }
        //    set { currencyCode = value; }
        //}
        
        public string CustomerFirstName
        {
            get { return customerFirstName; }
            set { customerFirstName = value; }
        }
        public string CustomerLastName
        {
            get { return customerLastName; }
            set { customerLastName = value; }
        }

        public string CustomerCompany
        {
            get { return customerCompany; }
            set { customerCompany = value; }
        }
        public string CustomerAddressLine1
        {
            get { return customerAddressLine1; }
            set { customerAddressLine1 = value; }
        }
        public string CustomerAddressLine2
        {
            get { return customerAddressLine2; }
            set { customerAddressLine2 = value; }
        }
        public string CustomerSuburb
        {
            get { return customerSuburb; }
            set { customerSuburb = value; }
        }
        public string CustomerCity
        {
            get { return customerCity; }
            set { customerCity = value; }
        }
        public string CustomerPostalCode
        {
            get { return customerPostalCode; }
            set { customerPostalCode = value; }
        }
        public string CustomerState
        {
            get { return customerState; }
            set { customerState = value; }
        }
        public string CustomerCountry
        {
            get { return customerCountry; }
            set { customerCountry = value; }
        }
        public string CustomerTelephoneDay
        {
            get { return customerTelephoneDay; }
            set { customerTelephoneDay = value; }
        }
        public string CustomerTelephoneNight
        {
            get { return customerTelephoneNight; }
            set { customerTelephoneNight = value; }
        }
        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }
        public bool CustomerEmailVerified
        {
            get { return customerEmailVerified; }
            set { customerEmailVerified = value; }
        }
        //public string DeliveryName
        //{
        //    get { return deliveryName; }
        //    set { deliveryName = value; }
        //}
        public string DeliveryFirstName
        {
            get { return deliveryFirstName; }
            set { deliveryFirstName = value; }
        }
        public string DeliveryLastName
        {
            get { return deliveryLastName; }
            set { deliveryLastName = value; }
        }

        public string DeliveryCompany
        {
            get { return deliveryCompany; }
            set { deliveryCompany = value; }
        }
        public string DeliveryAddress1
        {
            get { return deliveryAddress1; }
            set { deliveryAddress1 = value; }
        }
        public string DeliveryAddress2
        {
            get { return deliveryAddress2; }
            set { deliveryAddress2 = value; }
        }
        public string DeliverySuburb
        {
            get { return deliverySuburb; }
            set { deliverySuburb = value; }
        }
        public string DeliveryCity
        {
            get { return deliveryCity; }
            set { deliveryCity = value; }
        }
        public string DeliveryPostalCode
        {
            get { return deliveryPostalCode; }
            set { deliveryPostalCode = value; }
        }
        public string DeliveryState
        {
            get { return deliveryState; }
            set { deliveryState = value; }
        }
        public string DeliveryCountry
        {
            get { return deliveryCountry; }
            set { deliveryCountry = value; }
        }
        //public string BillingName
        //{
        //    get { return billingName; }
        //    set { billingName = value; }
        //}
        public string BillingFirstName
        {
            get { return billingFirstName; }
            set { billingFirstName = value; }
        }
        public string BillingLastName
        {
            get { return billingLastName; }
            set { billingLastName = value; }
        }

        public string BillingCompany
        {
            get { return billingCompany; }
            set { billingCompany = value; }
        }
        public string BillingAddress1
        {
            get { return billingAddress1; }
            set { billingAddress1 = value; }
        }
        public string BillingAddress2
        {
            get { return billingAddress2; }
            set { billingAddress2 = value; }
        }
        public string BillingSuburb
        {
            get { return billingSuburb; }
            set { billingSuburb = value; }
        }
        public string BillingCity
        {
            get { return billingCity; }
            set { billingCity = value; }
        }
        public string BillingPostalCode
        {
            get { return billingPostalCode; }
            set { billingPostalCode = value; }
        }
        public string BillingState
        {
            get { return billingState; }
            set { billingState = value; }
        }
        public string BillingCountry
        {
            get { return billingCountry; }
            set { billingCountry = value; }
        }
        public Guid CardTypeGuid
        {
            get { return cardTypeGuid; }
            set { cardTypeGuid = value; }
        }
        public string CardOwner
        {
            get { return cardOwner; }
            set { cardOwner = value; }
        }
        public string CardNumber
        {
            get { return cardNumber; }
            set { cardNumber = value; }
        }
        public string CardExpires
        {
            get { return cardExpires; }
            set { cardExpires = value; }
        }
        public string CardSecurityCode
        {
            get { return cardSecurityCode; }
            set { cardSecurityCode = value; }
        }
        //public Guid CurrencyGuid
        //{
        //    get { return currencyGuid; }
        //    set { currencyGuid = value; }
        //}
        //public decimal CurrencyValue
        //{
        //    get { return currencyValue; }
        //    set { currencyValue = value; }
        //}
        public decimal SubTotal
        {
            get { return subTotal; }
            set { subTotal = value; }
        }
        public decimal TaxTotal
        {
            get { return taxTotal; }
            set { taxTotal = value; }
        }
        public decimal ShippingTotal
        {
            get { return shippingTotal; }
            set { shippingTotal = value; }
        }

        public decimal Discount
        {
            get { return discount; }
            set { discount = value; }
        }

        public decimal OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set { createdFromIP = value; }
        }
        public DateTime Completed
        {
            get { return completed; }
            set { completed = value; }
        }
        public string CompletedFromIP
        {
            get { return completedFromIP; }
            set { completedFromIP = value; }
        }
        public DateTime LastModified
        {
            get { return lastModified; }
            set { lastModified = value; }
        }
        public DateTime LastUserActivity
        {
            get { return lastUserActivity; }
            set { lastUserActivity = value; }
        }
        public Guid StatusGuid
        {
            get { return statusGuid; }
            set { statusGuid = value; }
        }
        public string GatewayTransId
        {
            get { return gatewayTransID; }
            set { gatewayTransID = value; }
        }
        public string GatewayRawResponse
        {
            get { return gatewayRawResponse; }
            set { gatewayRawResponse = value; }
        }
        public string GatewayAuthCode
        {
            get { return gatewayAuthCode; }
            set { gatewayAuthCode = value; }
        }
        public Guid TaxZoneGuid
        {
            get { return taxZoneGuid; }
            set { taxZoneGuid = value; }
        }

        public string PaymentMethod
        {
            get { return paymentMethod; }
            set { paymentMethod = value; }
        }

        public bool AnalyticsTracked
        {
            get { return analyticsTracked; }
        }

        public string DiscountCodesCsv
        {
            get { return discountCodesCsv; }
            set { discountCodesCsv = value; }
        }

        public string CustomData
        {
            get { return customData; }
            set { customData = value; }
        }
        
        #endregion

        #region Private Methods

        

        private void GetOrder(Guid orderGuid)
        {
            using (IDataReader reader = DBOrder.Get(orderGuid))
            {
                if (reader.Read())
                {
                    isExisting = true;
                    this.orderGuid = new Guid(reader["OrderGuid"].ToString());
                    this.orderNo = Convert.ToInt32(reader["OrderNo"]);
                    this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    //this.customerName = reader["CustomerName"].ToString();
                    this.customerFirstName = reader["CustomerFirstName"].ToString();
                    this.customerLastName = reader["CustomerLastName"].ToString();

                    this.customerCompany = reader["CustomerCompany"].ToString();
                    this.customerAddressLine1 = reader["CustomerAddressLine1"].ToString();
                    this.customerAddressLine2 = reader["CustomerAddressLine2"].ToString();
                    this.customerSuburb = reader["CustomerSuburb"].ToString();
                    this.customerCity = reader["CustomerCity"].ToString();
                    this.customerPostalCode = reader["CustomerPostalCode"].ToString();
                    this.customerState = reader["CustomerState"].ToString();
                    this.customerCountry = reader["CustomerCountry"].ToString();
                    this.customerTelephoneDay = reader["CustomerTelephoneDay"].ToString();
                    this.customerTelephoneNight = reader["CustomerTelephoneNight"].ToString();
                    this.customerEmail = reader["CustomerEmail"].ToString();
                    this.customerEmailVerified = Convert.ToBoolean(reader["CustomerEmailVerified"]);
                    //this.deliveryName = reader["DeliveryName"].ToString();
                    this.deliveryFirstName = reader["DeliveryFirstName"].ToString();
                    this.deliveryLastName = reader["DeliveryLastName"].ToString();

                    this.deliveryCompany = reader["DeliveryCompany"].ToString();
                    this.deliveryAddress1 = reader["DeliveryAddress1"].ToString();
                    this.deliveryAddress2 = reader["DeliveryAddress2"].ToString();
                    this.deliverySuburb = reader["DeliverySuburb"].ToString();
                    this.deliveryCity = reader["DeliveryCity"].ToString();
                    this.deliveryPostalCode = reader["DeliveryPostalCode"].ToString();
                    this.deliveryState = reader["DeliveryState"].ToString();
                    this.deliveryCountry = reader["DeliveryCountry"].ToString();
                    //this.billingName = reader["BillingName"].ToString();
                    this.billingFirstName = reader["BillingFirstName"].ToString();
                    this.billingLastName = reader["BillingLastName"].ToString();

                    this.billingCompany = reader["BillingCompany"].ToString();
                    this.billingAddress1 = reader["BillingAddress1"].ToString();
                    this.billingAddress2 = reader["BillingAddress2"].ToString();
                    this.billingSuburb = reader["BillingSuburb"].ToString();
                    this.billingCity = reader["BillingCity"].ToString();
                    this.billingPostalCode = reader["BillingPostalCode"].ToString();
                    this.billingState = reader["BillingState"].ToString();
                    this.billingCountry = reader["BillingCountry"].ToString();
                    this.cardTypeGuid = new Guid(reader["CardTypeGuid"].ToString());
                    this.cardOwner = reader["CardOwner"].ToString();
                    this.cardNumber = reader["CardNumber"].ToString();
                    this.cardExpires = reader["CardExpires"].ToString();
                    this.cardSecurityCode = reader["CardSecurityCode"].ToString();
                    //this.currencyGuid = new Guid(reader["CurrencyGuid"].ToString());
                    //this.currencyValue = Convert.ToDecimal(reader["CurrencyValue"]);
                    this.subTotal = Convert.ToDecimal(reader["SubTotal"]);
                    this.taxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                    this.shippingTotal = Convert.ToDecimal(reader["ShippingTotal"]);
                    this.discount = Convert.ToDecimal(reader["Discount"]);
                    this.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
                    this.created = Convert.ToDateTime(reader["Created"]);
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    this.completed = Convert.ToDateTime(reader["Completed"]);
                    this.completedFromIP = reader["CompletedFromIP"].ToString();
                    this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                    this.lastUserActivity = Convert.ToDateTime(reader["LastUserActivity"]);

                    if (reader["StatusGuid"] != DBNull.Value)
                        this.statusGuid = new Guid(reader["StatusGuid"].ToString());

                    this.gatewayTransID = reader["GatewayTransID"].ToString();
                    this.gatewayRawResponse = reader["GatewayRawResponse"].ToString();
                    this.gatewayAuthCode = reader["GatewayAuthCode"].ToString();

                    if (reader["TaxZoneGuid"] != DBNull.Value)
                        this.taxZoneGuid = new Guid(reader["TaxZoneGuid"].ToString());

                    this.paymentMethod = reader["PaymentMethod"].ToString();
                    this.analyticsTracked = Convert.ToBoolean(reader["AnalyticsTracked"]);
                    //this.currencyCode = reader["CurrencyCode"].ToString();

                    this.customData = reader["CustomData"].ToString();
                    this.discountCodesCsv = reader["DiscountCodesCsv"].ToString();

                    if (reader["ClerkGuid"] != DBNull.Value)
                    {
                        this.clerkGuid = new Guid(reader["ClerkGuid"].ToString());
                    }

                }

            }

        }

        private bool Create()
        {
            
            if(orderGuid == Guid.Empty)
                orderGuid = Guid.NewGuid();

            int rowsAffected = DBOrder.Create(
                this.orderGuid,
                this.orderNo,
                this.storeGuid,
                this.userGuid,
                this.customerFirstName,
                this.customerLastName,
                this.customerCompany,
                this.customerAddressLine1,
                this.customerAddressLine2,
                this.customerSuburb,
                this.customerCity,
                this.customerPostalCode,
                this.customerState,
                this.customerCountry,
                this.customerTelephoneDay,
                this.customerTelephoneNight,
                this.customerEmail,
                this.customerEmailVerified,
                this.deliveryFirstName,
                this.deliveryLastName,
                this.deliveryCompany,
                this.deliveryAddress1,
                this.deliveryAddress2,
                this.deliverySuburb,
                this.deliveryCity,
                this.deliveryPostalCode,
                this.deliveryState,
                this.deliveryCountry,
                this.billingFirstName,
                this.billingLastName,
                this.billingCompany,
                this.billingAddress1,
                this.billingAddress2,
                this.billingSuburb,
                this.billingCity,
                this.billingPostalCode,
                this.billingState,
                this.billingCountry,
                this.cardTypeGuid,
                this.cardOwner,
                this.cardNumber,
                this.cardExpires,
                this.cardSecurityCode,
                this.subTotal,
                this.taxTotal,
                this.shippingTotal,
                this.orderTotal,
                this.created,
                this.createdFromIP,
                this.completed,
                this.completedFromIP,
                this.lastModified,
                this.lastUserActivity,
                this.statusGuid,
                this.gatewayTransID,
                this.gatewayRawResponse,
                this.gatewayAuthCode,
                this.taxZoneGuid,
                this.discount,
                this.discountCodesCsv,
                this.customData,
                this.clerkGuid);

            // create order offers
            this.isExisting = true;

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBOrder.Update(
                this.orderGuid,
                this.userGuid,
                this.customerFirstName,
                this.customerLastName,
                this.customerCompany,
                this.customerAddressLine1,
                this.customerAddressLine2,
                this.customerSuburb,
                this.customerCity,
                this.customerPostalCode,
                this.customerState,
                this.customerCountry,
                this.customerTelephoneDay,
                this.customerTelephoneNight,
                this.customerEmail,
                this.customerEmailVerified,
                this.deliveryFirstName,
                this.deliveryLastName,
                this.deliveryCompany,
                this.deliveryAddress1,
                this.deliveryAddress2,
                this.deliverySuburb,
                this.deliveryCity,
                this.deliveryPostalCode,
                this.deliveryState,
                this.deliveryCountry,
                this.billingFirstName,
                this.billingLastName,
                this.billingCompany,
                this.billingAddress1,
                this.billingAddress2,
                this.billingSuburb,
                this.billingCity,
                this.billingPostalCode,
                this.billingState,
                this.billingCountry,
                this.cardTypeGuid,
                this.cardOwner,
                this.cardNumber,
                this.cardExpires,
                this.cardSecurityCode,
                this.subTotal,
                this.taxTotal,
                this.shippingTotal,
                this.orderTotal,
                this.completed,
                this.completedFromIP,
                this.lastModified,
                this.lastUserActivity,
                this.statusGuid,
                this.gatewayTransID,
                this.gatewayRawResponse,
                this.gatewayAuthCode,
                this.taxZoneGuid,
                this.paymentMethod,
                this.discount,
                this.discountCodesCsv,
                this.customData,
                this.clerkGuid);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (isExisting)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        public bool HasShippingProducts()
        {
            bool result = false;
            // TODO: implement


            return result;
        }

        public bool HasDownloadProducts()
        {
            Collection<FullfillDownloadTicket> t = GetDownloadTickets();

            return t.Count > 0;
        }


        public IDataReader GetProducts()
        {
            return DBOrder.GetProducts(orderGuid);

        }

        public Collection<FullfillDownloadTicket> GetDownloadTickets()
        {
            Collection<FullfillDownloadTicket> downloadTickets 
                = new Collection<FullfillDownloadTicket>();

            using (IDataReader reader = FullfillDownloadTicket.GetByOrder(this.orderGuid))
            {
                while (reader.Read())
                {
                    FullfillDownloadTicket downloadTicket = new FullfillDownloadTicket();
                    downloadTicket.Guid = new Guid(reader["Guid"].ToString());
                    downloadTicket.StoreGuid = new Guid(reader["StoreGuid"].ToString());
                    downloadTicket.ProductName = reader["Name"].ToString();
                    downloadTicket.CountAfterDownload = Convert.ToBoolean(reader["CountAfterDownload"]);
                    downloadTicket.DownloadedCount = Convert.ToInt32(reader["DownloadedCount"]);
                    downloadTicket.DownloadsAllowed = Convert.ToInt32(reader["DownloadsAllowed"]);
                    downloadTicket.ExpireAfterDays = Convert.ToInt32(reader["ExpireAfterDays"]);
                    downloadTicket.FullfillTermsGuid = new Guid(reader["FullfillTermsGuid"].ToString());
                    downloadTicket.OrderGuid = new Guid(reader["OrderGuid"].ToString());
                    downloadTicket.ProductGuid = new Guid(reader["ProductGuid"].ToString());
                    downloadTicket.PurchaseTime = Convert.ToDateTime(reader["PurchaseTime"]);
                    downloadTicket.UserGuid = new Guid(reader["UserGuid"].ToString());

                    downloadTickets.Add(downloadTicket);
                }
            }

            return downloadTickets;
        }


        #endregion

        #region Static Methods

        // TODO: ? do i want to allow deletes?

        //public static bool Delete(Guid orderGuid)
        //{
        //    return DBOrder.DeleteOrder(orderGuid);
        //}

        /// <summary>
        /// Flags an order as already tracked in google analytics
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static bool TrackAnalytics(Guid orderGuid)
        {
            return DBOrder.TrackAnalytics(orderGuid);
        }

        public static DataSet GetOrderOffersAndProducts(Guid storeGuid, Guid orderGuid)
        {
            DataSet dataSet = new DataSet();

            DataTable offerTable = new DataTable();
            offerTable.Columns.Add("OfferGuid", typeof(Guid));
            offerTable.Columns.Add("Name", typeof(string));
            offerTable.Columns.Add("Quantity", typeof(int));
            offerTable.Columns.Add("OfferPrice", typeof(decimal));
            

            using (IDataReader reader = DBOrderOffer.GetByOrder(orderGuid))
            {
                while (reader.Read())
                {
                    DataRow row = offerTable.NewRow();
                    row["OfferGuid"] = new Guid(reader["OfferGuid"].ToString());
                    row["Name"] = reader["Name"];
                    row["Quantity"] = Convert.ToInt32(reader["Quantity"]);
                    row["OfferPrice"] = Convert.ToDecimal(reader["OfferPrice"]);

                    offerTable.Rows.Add(row);
                }
            }

            DataTable productTable = new DataTable();
            productTable.Columns.Add("OfferGuid", typeof(Guid));
            productTable.Columns.Add("Name", typeof(string));
            productTable.Columns.Add("Quantity", typeof(int));

            using (IDataReader reader = DBOrder.GetProducts(orderGuid))
            {
                while (reader.Read())
                {
                    DataRow row = productTable.NewRow();
                    row["OfferGuid"] = new Guid(reader["OfferGuid"].ToString());
                    row["Name"] = reader["Name"];
                    row["Quantity"] = Convert.ToInt32(reader["Quantity"]);

                    productTable.Rows.Add(row);
                }
            }

            offerTable.TableName = "Offers";
            productTable.TableName = "Products";
            dataSet.Tables.Add(offerTable);
            dataSet.Tables.Add(productTable);

            // create a releationship
            dataSet.Relations.Add("offerprods",
                dataSet.Tables["Offers"].Columns["OfferGuid"],
                dataSet.Tables["Products"].Columns["OfferGuid"], false);

            return dataSet;
        }


        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBOrder.GetPage(storeGuid, pageNumber, pageSize, out totalPages);

        }
        
        
       
        public static DataTable GetPage(
            Guid storeGuid,
            int pageNumber, 
            int pageSize)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("OrderGuid",typeof(Guid));
            dataTable.Columns.Add("OrderNo",typeof(int));
            dataTable.Columns.Add("StoreGuid",typeof(Guid));
            dataTable.Columns.Add("UserGuid",typeof(Guid));
            dataTable.Columns.Add("CustomerName",typeof(string));
            dataTable.Columns.Add("CustomerCompany",typeof(string));
            dataTable.Columns.Add("CustomerAddressLine1",typeof(string));
            dataTable.Columns.Add("CustomerAddressLine2",typeof(string));
            dataTable.Columns.Add("CustomerSuburb",typeof(string));
            dataTable.Columns.Add("CustomerCity",typeof(string));
            dataTable.Columns.Add("CustomerPostalCode",typeof(string));
            dataTable.Columns.Add("CustomerState",typeof(string));
            dataTable.Columns.Add("CustomerCountry",typeof(string));
            dataTable.Columns.Add("CustomerTelephoneDay",typeof(string));
            dataTable.Columns.Add("CustomerTelephoneNight",typeof(string));
            dataTable.Columns.Add("CustomerEmail",typeof(string));
            dataTable.Columns.Add("CustomerEmailVerified",typeof(bool));
            dataTable.Columns.Add("DeliveryName",typeof(string));
            dataTable.Columns.Add("DeliveryCompany",typeof(string));
            dataTable.Columns.Add("DeliveryAddress1",typeof(string));
            dataTable.Columns.Add("DeliveryAddress2",typeof(string));
            dataTable.Columns.Add("DeliverySuburb",typeof(string));
            dataTable.Columns.Add("DeliveryCity",typeof(string));
            dataTable.Columns.Add("DeliveryPostalCode",typeof(string));
            dataTable.Columns.Add("DeliveryState",typeof(string));
            dataTable.Columns.Add("DeliveryCountry",typeof(string));
            dataTable.Columns.Add("BillingName",typeof(string));
            dataTable.Columns.Add("BillingCompany",typeof(string));
            dataTable.Columns.Add("BillingAddress1",typeof(string));
            dataTable.Columns.Add("BillingAddress2",typeof(string));
            dataTable.Columns.Add("BillingSuburb",typeof(string));
            dataTable.Columns.Add("BillingCity",typeof(string));
            dataTable.Columns.Add("BillingPostalCode",typeof(string));
            dataTable.Columns.Add("BillingState",typeof(string));
            dataTable.Columns.Add("BillingCountry",typeof(string));
            dataTable.Columns.Add("CardTypeGuid",typeof(Guid));
            dataTable.Columns.Add("CardOwner",typeof(string));
            dataTable.Columns.Add("CardNumber",typeof(string));
            dataTable.Columns.Add("CardExpires",typeof(string));
            dataTable.Columns.Add("CardSecurityCode",typeof(string));
            dataTable.Columns.Add("CurrencyGuid",typeof(Guid));
            dataTable.Columns.Add("CurrencyValue",typeof(decimal));
            dataTable.Columns.Add("SubTotal",typeof(decimal));
            dataTable.Columns.Add("TaxTotal",typeof(decimal));
            dataTable.Columns.Add("Discount", typeof(decimal));
            dataTable.Columns.Add("OrderTotal",typeof(decimal));
            dataTable.Columns.Add("Created",typeof(DateTime));
            dataTable.Columns.Add("CreatedFromIP",typeof(string));
            dataTable.Columns.Add("Completed",typeof(DateTime));
            dataTable.Columns.Add("CompletedFromIP",typeof(string));
            dataTable.Columns.Add("LastModified",typeof(DateTime));
            dataTable.Columns.Add("LastUserActivity",typeof(DateTime));
            dataTable.Columns.Add("StatusGuid",typeof(Guid));
            dataTable.Columns.Add("GatewayTransID",typeof(string));
            dataTable.Columns.Add("GatewayRawResponse",typeof(string));
            dataTable.Columns.Add("GatewayAuthCode",typeof(string));
            dataTable.Columns.Add("TaxZoneGuid",typeof(Guid));
            dataTable.Columns.Add("TotalPages", typeof(int));
            dataTable.Columns.Add("CustomData", typeof(string));
            dataTable.Columns.Add("DiscountCodesCsv", typeof(string));

            using (IDataReader reader = DBOrder.GetByStore(storeGuid, pageNumber, pageSize))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();
                    row["OrderGuid"] = reader["OrderGuid"];
                    row["OrderNo"] = reader["OrderNo"];
                    row["StoreGuid"] = reader["StoreGuid"];
                    row["UserGuid"] = reader["UserGuid"];
                    row["CustomerName"] = reader["CustomerName"];
                    row["CustomerCompany"] = reader["CustomerCompany"];
                    row["CustomerAddressLine1"] = reader["CustomerAddressLine1"];
                    row["CustomerAddressLine2"] = reader["CustomerAddressLine2"];
                    row["CustomerSuburb"] = reader["CustomerSuburb"];
                    row["CustomerCity"] = reader["CustomerCity"];
                    row["CustomerPostalCode"] = reader["CustomerPostalCode"];
                    row["CustomerState"] = reader["CustomerState"];
                    row["CustomerCountry"] = reader["CustomerCountry"];
                    row["CustomerTelephoneDay"] = reader["CustomerTelephoneDay"];
                    row["CustomerTelephoneNight"] = reader["CustomerTelephoneNight"];
                    row["CustomerEmail"] = reader["CustomerEmail"];
                    row["CustomerEmailVerified"] = reader["CustomerEmailVerified"];
                    row["DeliveryName"] = reader["DeliveryName"];
                    row["DeliveryCompany"] = reader["DeliveryCompany"];
                    row["DeliveryAddress1"] = reader["DeliveryAddress1"];
                    row["DeliveryAddress2"] = reader["DeliveryAddress2"];
                    row["DeliverySuburb"] = reader["DeliverySuburb"];
                    row["DeliveryCity"] = reader["DeliveryCity"];
                    row["DeliveryPostalCode"] = reader["DeliveryPostalCode"];
                    row["DeliveryState"] = reader["DeliveryState"];
                    row["DeliveryCountry"] = reader["DeliveryCountry"];
                    row["BillingName"] = reader["BillingName"];
                    row["BillingCompany"] = reader["BillingCompany"];
                    row["BillingAddress1"] = reader["BillingAddress1"];
                    row["BillingAddress2"] = reader["BillingAddress2"];
                    row["BillingSuburb"] = reader["BillingSuburb"];
                    row["BillingCity"] = reader["BillingCity"];
                    row["BillingPostalCode"] = reader["BillingPostalCode"];
                    row["BillingState"] = reader["BillingState"];
                    row["BillingCountry"] = reader["BillingCountry"];
                    row["CardTypeGuid"] = reader["CardTypeGuid"];
                    row["CardOwner"] = reader["CardOwner"];
                    row["CardNumber"] = reader["CardNumber"];
                    row["CardExpires"] = reader["CardExpires"];
                    row["CardSecurityCode"] = reader["CardSecurityCode"];
                    row["CurrencyGuid"] = reader["CurrencyGuid"];
                    row["CurrencyValue"] = reader["CurrencyValue"];
                    row["SubTotal"] = reader["SubTotal"];
                    row["TaxTotal"] = reader["TaxTotal"];
                    row["Discount"] = reader["Discount"];
                    row["OrderTotal"] = reader["OrderTotal"];
                    row["Created"] = reader["Created"];
                    row["CreatedFromIP"] = reader["CreatedFromIP"];
                    row["Completed"] = reader["Completed"];
                    row["CompletedFromIP"] = reader["CompletedFromIP"];
                    row["LastModified"] = reader["LastModified"];
                    row["LastUserActivity"] = reader["LastUserActivity"];
                    row["StatusGuid"] = reader["StatusGuid"];
                    row["GatewayTransID"] = reader["GatewayTransID"];
                    row["GatewayRawResponse"] = reader["GatewayRawResponse"];
                    row["GatewayAuthCode"] = reader["GatewayAuthCode"];
                    row["TaxZoneGuid"] = reader["TaxZoneGuid"];
                    row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);

                    row["CustomData"] = reader["CustomData"];
                    row["DiscountCodesCsv"] = reader["DiscountCodesCsv"];

                    dataTable.Rows.Add(row);
                }

            }
		
            return dataTable;
		
        }


        public static Order CreateOrder(
            Store store,
            Cart cart, 
            IPaymentGateway gateway, 
            string currencyCode, 
            string paymentMethod)
        {
            return CreateOrder(
                store,
                cart,
                gateway.RawResponse,
                gateway.TransactionId,
                gateway.ApprovalCode,
                currencyCode,
                paymentMethod,
                OrderStatus.OrderStatusFulfillableGuid);



        }

        public static Order CreateOrder(
            Store store,
            Cart cart, 
            string gatewayRawResponse,
            string gatewayTransactionId,
            string gatewayApprovalCode,
            string currencyCode,
            string paymentMethod,
            Guid orderStatusGuid)
        {
            Order order = new Order(cart.CartGuid);
            
            order.billingAddress1 = cart.OrderInfo.BillingAddress1;
            order.billingAddress2 = cart.OrderInfo.BillingAddress2;
            order.billingCity = cart.OrderInfo.BillingCity;
            order.billingCompany = cart.OrderInfo.BillingCompany;
            order.billingCountry = cart.OrderInfo.BillingCountry;
            order.billingFirstName = cart.OrderInfo.BillingFirstName;
            order.billingLastName = cart.OrderInfo.BillingLastName;
            order.billingPostalCode = cart.OrderInfo.BillingPostalCode;
            order.billingState = cart.OrderInfo.BillingState;
            order.billingSuburb = cart.OrderInfo.BillingSuburb;

            order.completed = cart.OrderInfo.Completed;
            order.completedFromIP = cart.OrderInfo.CompletedFromIP;
            order.created = DateTime.UtcNow;
            order.createdFromIP = cart.OrderInfo.CompletedFromIP;
            
            order.customerAddressLine1 = cart.OrderInfo.CustomerAddressLine1;
            order.customerAddressLine2 = cart.OrderInfo.CustomerAddressLine2;
            order.customerCity = cart.OrderInfo.CustomerCity;
            order.customerCompany = cart.OrderInfo.CustomerCompany;
            order.customerCountry = cart.OrderInfo.CustomerCountry;
            order.customerEmail = cart.OrderInfo.CustomerEmail;
            order.customerEmailVerified = cart.OrderInfo.CustomerEmailVerified;
            order.customerFirstName = cart.OrderInfo.CustomerFirstName;
            order.customerLastName = cart.OrderInfo.CustomerLastName;
            order.customerPostalCode = cart.OrderInfo.CustomerPostalCode;
            order.customerState = cart.OrderInfo.CustomerState;
            order.customerSuburb = cart.OrderInfo.CustomerSuburb;
            order.customerTelephoneDay = cart.OrderInfo.CustomerTelephoneDay;
            order.customerTelephoneNight = cart.OrderInfo.CustomerTelephoneNight;

            order.deliveryAddress1 = cart.OrderInfo.DeliveryAddress1;
            order.deliveryAddress2 = cart.OrderInfo.DeliveryAddress2;
            order.deliveryCity = cart.OrderInfo.DeliveryCity;
            order.deliveryCompany = cart.OrderInfo.DeliveryCompany;
            order.deliveryCountry = cart.OrderInfo.DeliveryCountry;
            order.deliveryFirstName = cart.OrderInfo.DeliveryFirstName;
            order.deliveryLastName = cart.OrderInfo.DeliveryLastName;
            order.deliveryPostalCode = cart.OrderInfo.DeliveryPostalCode;
            order.deliveryState = cart.OrderInfo.DeliveryState;
            order.deliverySuburb = cart.OrderInfo.DeliverySuburb;

            order.gatewayAuthCode = gatewayApprovalCode;
            order.gatewayRawResponse = gatewayRawResponse;
            order.gatewayTransID = gatewayTransactionId;

            order.lastModified = DateTime.UtcNow;
            order.lastUserActivity = cart.LastUserActivity;

            order.orderGuid = cart.CartGuid;

            
            order.OrderTotal = cart.OrderTotal;
            order.statusGuid = orderStatusGuid;

            order.storeGuid = cart.StoreGuid;
            order.clerkGuid = cart.ClerkGuid;
            order.subTotal = cart.SubTotal;
            order.taxTotal = cart.TaxTotal;
            order.discount = cart.Discount;
            order.shippingTotal = cart.ShippingTotal;
            order.taxZoneGuid = cart.OrderInfo.TaxZoneGuid;
            order.userGuid = cart.UserGuid;
            order.discountCodesCsv = cart.DiscountCodesCsv;
            order.customData = cart.CustomData;

            order.Save();

            

            // TODO: need to add this in insert so we don't have to save 2x
            order.paymentMethod = paymentMethod;
            order.Save();

            foreach (CartOffer cartOffer in cart.CartOffers)
            {
                OrderOffer orderOffer = new OrderOffer();
                orderOffer.AddedToCart = cartOffer.AddedToCart;
                //orderOffer.CurrencyGuid = cartOffer.CurrencyGuid;
                orderOffer.OfferGuid = cartOffer.OfferGuid;
                orderOffer.OfferPrice = cartOffer.OfferPrice;
                orderOffer.OrderGuid = order.OrderGuid;
                //orderOffer.PriceGuid = cartOffer.PriceGuid;
                orderOffer.Quantity = cartOffer.Quantity;
                orderOffer.TaxClassGuid = cartOffer.TaxClassGuid;
                orderOffer.Save();

               
                Collection<OfferProduct> offerProducts
                    = OfferProduct.GetbyOffer(orderOffer.OfferGuid);

                foreach (OfferProduct offerProduct in offerProducts)
                {
                    OrderOfferProduct orderProduct = new OrderOfferProduct();
                    orderProduct.Created = DateTime.UtcNow;
                    orderProduct.FullfillTermsGuid = offerProduct.FullFillTermsGuid;
                    orderProduct.FullfillType = offerProduct.FullfillType;
                    orderProduct.OfferGuid = offerProduct.OfferGuid;
                    orderProduct.OrderGuid = order.OrderGuid;
                    orderProduct.ProductGuid = offerProduct.ProductGuid;
                    orderProduct.Save();

                    if (
                        (offerProduct.FullfillType
                        == (byte)FulfillmentType.Download)
                        && (offerProduct.FullFillTermsGuid != Guid.Empty)
                        )
                    {
                        // create download fullfillment ticket
                        FullfillDownloadTerms downloadTerms
                            = new FullfillDownloadTerms(offerProduct.FullFillTermsGuid);

                        FullfillDownloadTicket downloadTicket = new FullfillDownloadTicket();
                        downloadTicket.CountAfterDownload = downloadTerms.CountAfterDownload;
                        downloadTicket.DownloadsAllowed = downloadTerms.DownloadsAllowed;
                        downloadTicket.ExpireAfterDays = downloadTerms.ExpireAfterDays;
                        downloadTicket.FullfillTermsGuid = offerProduct.FullFillTermsGuid;
                        downloadTicket.OrderGuid = order.OrderGuid;
                        downloadTicket.ProductGuid = offerProduct.ProductGuid;
                        downloadTicket.PurchaseTime = order.Completed;
                        downloadTicket.StoreGuid = order.StoreGuid;
                        downloadTicket.UserGuid = order.UserGuid;
                        downloadTicket.Save();

                    }
                }

            }

            if (order.DiscountCodesCsv.Length > 0)
            {
                List<WebStore.Business.Discount> discountList = WebStore.Business.Discount.GetValidDiscounts(store.ModuleGuid, cart, cart.DiscountCodesCsv);
                foreach (WebStore.Business.Discount d in discountList)
                {
                    d.UseCount += 1;
                    d.Save();
                    // TODO: Discount log

                }
            }

            Cart.Delete(cart.CartGuid);

            return order;

        }

        public static bool MoveOrder(
             Guid orderGuid,
             Guid newUserGuid)
        {
            return DBOrder.MoveOrder(orderGuid, newUserGuid);
        }

        public static IDataReader GetByUser(Guid storeGuid, Guid userGuid)
        {
            return DBOrder.GetByUser(storeGuid, userGuid);
        }

        public static Order GetMostRecentOrder(Guid storeGuid, Guid userGuid)
        {
            Guid lastOrderGuid = Guid.Empty;
            using (IDataReader reader = DBOrder.GetByUser(storeGuid, userGuid))
            {
                if (reader.Read())
                {
                    lastOrderGuid = new Guid(reader["OrderGuid"].ToString());
                }
            }

            if (lastOrderGuid != Guid.Empty) return new Order(lastOrderGuid);

            return null;
        }

        public static bool Delete(Guid orderGuid)
        {
            OrderOfferProduct.DeleteByOrder(orderGuid);
            OrderOffer.DeleteByOrder(orderGuid);
            return DBOrder.Delete(orderGuid);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid moduleGuid, int pageId, int moduleId)
        {
            DBOrder.EnsureSalesReportOrderData(moduleGuid, pageId, moduleId);
            DBOrder.EnsureSalesReportData(moduleGuid, pageId, moduleId);

        }

        /// <summary>
        /// Updates the mp_CommerceReport table with any missing orders
        /// </summary>
        /// <param name="orderGuid"> orderGuid </param>
        /// <returns>bool</returns>
        public static void EnsureSalesReportData(Guid orderGuid, Guid moduleGuid, int pageId, int moduleId)
        {
            DBOrder.EnsureSalesReportDataForOrders(orderGuid, moduleGuid, pageId, moduleId);
            DBOrder.EnsureSalesReportData(orderGuid, moduleGuid, pageId, moduleId);
        }

        public static DataTable GetSalesByYearMonth(Guid storeGuid)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Y", typeof(int));
            dataTable.Columns.Add("M", typeof(int)); ;
            dataTable.Columns.Add("Sales", typeof(decimal));

            using (IDataReader reader = DBOrder.GetSalesByYearMonth(storeGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Y"] = Convert.ToInt32(reader["Y"], CultureInfo.InvariantCulture);
                    row["M"] = Convert.ToInt32(reader["M"], CultureInfo.InvariantCulture);
                    row["Sales"] = Convert.ToDecimal(reader["Sales"], CultureInfo.InvariantCulture);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public static DataTable GetRevenueSummary(Guid storeGuid)
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("OfferGuid", typeof(Guid)); ;
            dataTable.Columns.Add("UnitsSold", typeof(int));
            dataTable.Columns.Add("Revenue", typeof(decimal));

            using (IDataReader reader = DBOrder.GetRevenueSummary(storeGuid))
            {
                while (reader.Read())
                {
                    DataRow row = dataTable.NewRow();

                    row["Name"] = reader["Name"].ToString();
                    row["OfferGuid"] = new Guid(reader["OfferGuid"].ToString());
                    row["UnitsSold"] = Convert.ToInt32(reader["UnitsSold"], CultureInfo.InvariantCulture);
                    row["Revenue"] = Convert.ToDecimal(reader["Revenue"], CultureInfo.InvariantCulture);

                    dataTable.Rows.Add(row);
                }
            }

            return dataTable;
        }

        public static decimal GetAllTimeRevenueTotal(Guid storeGuid)
        {
            return DBOrder.GetAllTimeRevenueTotal(storeGuid);
        }



        #endregion


    }

}
