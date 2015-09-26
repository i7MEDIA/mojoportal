/// Author:					Joe Audette
/// Created:				2007-03-05
/// Last Modified:			2015-04-13 (Joe Davis)
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.
/// [System.Xml.Serialization.XmlInclude(typeof(CartOffer))]

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml.Serialization;
using WebStore.Data;
using mojoPortal.Business;
using mojoPortal.Business.Commerce;

namespace WebStore.Business
{
    /// <summary>
    /// Represents a shopping cart
    /// </summary>
    [Serializable()]
    public class Cart
    {

        #region Constructors

        public Cart()
        { }


        public Cart(Guid cartGuid)
        {
            GetCart(cartGuid);
        }

        #endregion

        #region Private Properties

        private Guid cartGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        //private Guid currencyGuid = Guid.Empty;
        //private Guid languageGuid = Guid.Empty;
        //private decimal currencyValue = 1;
        private decimal subTotal = 0;
        private decimal taxTotal = 0;
        private decimal shippingTotal = 0;
        private decimal discount = 0;
        private decimal orderTotal = 0;
        private DateTime created = DateTime.UtcNow;
        private string createdFromIP = string.Empty;
        private DateTime lastModified = DateTime.UtcNow;
        private DateTime lastUserActivity = DateTime.UtcNow;
        private CartOrderInfo orderInfo = null;
        private bool cartExists = false;
        private Guid newCartID = Guid.NewGuid();

        private string discountCodesCsv = string.Empty;
        private string customData = string.Empty;

        private Guid clerkGuid = Guid.Empty;
        

        

        [NonSerializedAttribute()]
        private Store store = null;

        // SoapFormatter can't serialize generic collections
        [NonSerializedAttribute()]
        private List<CartOffer> cartOffers = null;
        //private ArrayList cartItems = new ArrayList();
        private string serializedCartOffers = string.Empty;

        

        #endregion

        #region Public Properties

        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }

        public Guid ClerkGuid
        {
            get { return clerkGuid; }
            set { clerkGuid = value; }
        }


       
        public List<CartOffer> CartOffers
        {
            get
            {
                if (cartOffers == null)
                {
                    cartOffers = GetCartOffers();
                }

                if (cartOffers.Count == 0)
                {
                    cartOffers = GetCartOffers();
                }

                return cartOffers;
            }
        }


        //[XmlIgnore]
        public string SerializedCartOffers
        {
            get { return serializedCartOffers; }
            set { serializedCartOffers = value; }
        }

        // This is needed to support xml serialization, string with special characterscan cause invalid xml, base 64 encoding them gets around the problem.

        //[XmlElement(ElementName = "serializedCartOffers", DataType = "base64Binary")]
        //public byte[] OfferSerialization
        //{
        //    get
        //    {
        //        return System.Text.Encoding.Unicode.GetBytes(SerializedCartOffers);
        //    }
        //    set
        //    {
        //        SerializedCartOffers = System.Text.Encoding.Unicode.GetString(value);
        //    }
        //}  

        

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        
        public CartOrderInfo OrderInfo
        {
            get
            {
                if (orderInfo == null)
                {
                    orderInfo = new CartOrderInfo(this.cartGuid);
                    if (
                        (!orderInfo.Exists)
                        &&(this.userGuid != Guid.Empty)
                        )
                    {
                        EnsureStore();
                        store.LoadCustomerInfoFromMostRecentOrder(this);

                    }
                }
                return orderInfo;
            }

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
        public decimal ShippingTotal
        {
            get { return shippingTotal; }
            set { shippingTotal = value; }
        }

        public decimal TaxTotal
        {
            get { return taxTotal; }
            set { taxTotal = value; }
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

        public bool Exists
        {
            get { return cartExists; }

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

        private void EnsureStore()
        {
            if (store == null) store = new Store(storeGuid);

        }

        private void GetCart(Guid cartGuid)
        {
            using (IDataReader reader = DBCart.GetCart(cartGuid))
            {
                GetCart(reader);
            }

        }

        private void GetCart(IDataReader reader)
        { 
            if (reader.Read())
            {
                this.cartExists = true;
                this.cartGuid = new Guid(reader["CartGuid"].ToString());
                this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                this.userGuid = new Guid(reader["UserGuid"].ToString());

                this.subTotal = Convert.ToDecimal(reader["SubTotal"]);
                this.shippingTotal = Convert.ToDecimal(reader["ShippingTotal"]);
                this.taxTotal = Convert.ToDecimal(reader["TaxTotal"]);
                this.discount = Convert.ToDecimal(reader["Discount"]);
                this.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
                this.created = Convert.ToDateTime(reader["Created"]);
                this.createdFromIP = reader["CreatedFromIP"].ToString();
                this.lastModified = Convert.ToDateTime(reader["LastModified"]);
                this.lastUserActivity = Convert.ToDateTime(reader["LastUserActivity"]);

                this.customData = reader["CustomData"].ToString();
                this.discountCodesCsv = reader["DiscountCodesCsv"].ToString();
                if (reader["ClerkGuid"] != DBNull.Value)
                {
                    this.clerkGuid = new Guid(reader["ClerkGuid"].ToString());
                }

            }

        }

        private bool Create()
        {
            
            this.cartGuid = this.newCartID;

            int rowsAffected = DBCart.AddCart(
                this.cartGuid,
                this.storeGuid,
                this.userGuid,
                this.subTotal,
                this.shippingTotal,
                this.taxTotal,
                this.orderTotal,
                this.created,
                this.createdFromIP,
                this.lastModified,
                this.lastUserActivity,
                this.discount,
                this.discountCodesCsv,
                this.customData,
                this.clerkGuid);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBCart.UpdateCart(
                this.cartGuid,
                this.userGuid,
                this.subTotal,
                this.shippingTotal,
                this.taxTotal,
                this.orderTotal,
                this.lastModified,
                this.lastUserActivity,
                this.discount,
                this.discountCodesCsv,
                this.customData,
                this.clerkGuid);

        }


        private void CalculateSubTotal()
        {
            decimal calculatedSubTotal = 0;
            foreach (CartOffer cartOffer in CartOffers)
            {
                calculatedSubTotal += (cartOffer.Quantity * cartOffer.OfferPrice);

            }
            EnsureStore();

            this.subTotal = Math.Round(calculatedSubTotal, store.RoundingDecimalPlaces, store.RoundingMode);
            if (subTotal < 0) { subTotal = 0; }
           
        }

        public void ResetCartOffers()
        {
            cartOffers = null;
        }

        private void CalculateTaxTotal()
        {
            EnsureStore();
            store.CalculateTax(this);
            

        }

        private void CalculatShippingTotal()
        {
            if (this.HasShippingProducts())
            {
                EnsureStore();
                store.CalculateShipping(this);
                
            }

        }

        private void CalculateTotal()
        {
            EnsureStore();
            decimal total = (this.subTotal + this.shippingTotal + this.taxTotal) - this.discount;
            this.orderTotal = Math.Round(total, store.RoundingDecimalPlaces, store.RoundingMode);
            if (orderTotal < 0) { orderTotal = 0; }
            
        }

        private List<CartOffer> GetCartOffers()
        {

            List<CartOffer> cartOffers = CartOffer.GetByCart(this.cartGuid);

            //IDataReader reader = DBCartOffer.GetByCart(this.cartGuid, Guid.Empty);
            //while (reader.Read())
            //{
            //    CartOffer cartOffer = new CartOffer();

            //    cartOffer.AddedToCart = Convert.ToDateTime(reader["AddedToCart"]);
            //    cartOffer.CartGuid = this.cartGuid;
            //    cartOffer.CurrencyGuid = new Guid(reader["CurrencyGuid"].ToString());
            //    cartOffer.ItemGuid = new Guid(reader["ItemGuid"].ToString());
            //    cartOffer.OfferGuid = new Guid(reader["OfferGuid"].ToString());
            //    cartOffer.OfferPrice = Convert.ToDecimal(reader["OfferPrice"]);
            //    cartOffer.PriceGuid = new Guid(reader["PriceGuid"].ToString());
            //    cartOffer.Quantity = Convert.ToInt32(reader["Quantity"]);
            //    cartOffer.TaxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
            //    cartOffers.Add(cartOffer);

            //}
            //reader.Close();

            return cartOffers;

        }

        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.cartGuid != Guid.Empty)
            {
                this.lastModified = DateTime.UtcNow;
                return Update();
            }
            else
            {
                return Create();
            }
        }

        public void LoadExistingUserCartIfExists()
        {
            if (this.userGuid == Guid.Empty) return;

            using (IDataReader reader = DBCart.GetByUser(userGuid, storeGuid))
            {
                GetCart(reader);
            }

        }

        public void ClearCustomerData()
        {
            this.OrderInfo.CustomerEmail = string.Empty;
            this.OrderInfo.CustomerCompany = string.Empty;
            this.OrderInfo.CustomerAddressLine1 = string.Empty;
            this.OrderInfo.CustomerAddressLine2 = string.Empty;
            this.OrderInfo.CustomerCity = string.Empty;
            this.OrderInfo.CustomerFirstName = string.Empty;
            this.OrderInfo.CustomerLastName = string.Empty;
            this.OrderInfo.CustomerPostalCode = string.Empty;
            this.OrderInfo.CustomerState = string.Empty;
            this.OrderInfo.CustomerCountry = string.Empty;
            this.OrderInfo.CustomerTelephoneDay = string.Empty;
            this.OrderInfo.CustomerTelephoneNight = string.Empty;

            this.OrderInfo.BillingCompany = string.Empty;
            this.OrderInfo.BillingAddress1 = string.Empty;
            this.OrderInfo.BillingAddress2 = string.Empty;
            this.OrderInfo.BillingCity = string.Empty;
            this.OrderInfo.BillingFirstName = string.Empty;
            this.OrderInfo.BillingLastName = string.Empty;
            this.OrderInfo.BillingPostalCode = string.Empty;
            this.OrderInfo.BillingState = string.Empty;
            this.OrderInfo.BillingCountry = string.Empty;

            this.OrderInfo.DeliveryCompany = string.Empty;
            this.OrderInfo.DeliveryAddress1 = string.Empty;
            this.OrderInfo.DeliveryAddress2 = string.Empty;
            this.OrderInfo.DeliveryCity = string.Empty;
            this.OrderInfo.DeliveryFirstName = string.Empty;
            this.OrderInfo.DeliveryLastName = string.Empty;
            this.OrderInfo.DeliveryPostalCode = string.Empty;
            this.OrderInfo.DeliveryState = string.Empty;
            this.OrderInfo.DeliveryCountry = string.Empty;
            

        }

        public void CopyShippingToBilling()
        {
            this.OrderInfo.BillingCompany = this.OrderInfo.DeliveryCompany;
            this.OrderInfo.BillingAddress1 = this.OrderInfo.DeliveryAddress1;
            this.OrderInfo.BillingAddress2 = this.OrderInfo.DeliveryAddress2;
            this.OrderInfo.BillingCity = this.OrderInfo.DeliveryCity;
            this.OrderInfo.BillingFirstName = this.OrderInfo.DeliveryFirstName;
            this.OrderInfo.BillingLastName = this.OrderInfo.DeliveryLastName;
            this.OrderInfo.BillingPostalCode = this.OrderInfo.DeliveryPostalCode;
            this.OrderInfo.BillingState = this.OrderInfo.DeliveryState;
            this.OrderInfo.BillingCountry = this.OrderInfo.DeliveryCountry;
            this.OrderInfo.Save();

        }

        public void CopyBillingToShipping()
        {
            this.OrderInfo.DeliveryCompany = this.OrderInfo.BillingCompany;
            this.OrderInfo.DeliveryAddress1 = this.OrderInfo.BillingAddress1;
            this.OrderInfo.DeliveryAddress2 = this.OrderInfo.BillingAddress2;
            this.OrderInfo.DeliveryCity = this.OrderInfo.BillingCity;
            this.OrderInfo.DeliveryFirstName = this.OrderInfo.BillingFirstName;
            this.OrderInfo.DeliveryLastName = this.OrderInfo.BillingLastName;
            this.OrderInfo.DeliveryPostalCode = this.OrderInfo.BillingPostalCode;
            this.OrderInfo.DeliveryState = this.OrderInfo.BillingState;
            this.OrderInfo.DeliveryCountry = this.OrderInfo.BillingCountry;
            this.OrderInfo.Save();

        }

        public void CopyCustomerToBilling()
        {
            this.OrderInfo.BillingCompany = this.OrderInfo.CustomerCompany;
            this.OrderInfo.BillingAddress1 = this.OrderInfo.CustomerAddressLine1;
            this.OrderInfo.BillingAddress2 = this.OrderInfo.CustomerAddressLine2;
            this.OrderInfo.BillingCity = this.OrderInfo.CustomerCity;
            this.OrderInfo.BillingFirstName = this.OrderInfo.CustomerFirstName;
            this.OrderInfo.BillingLastName = this.OrderInfo.CustomerLastName;
            this.OrderInfo.BillingPostalCode = this.OrderInfo.CustomerPostalCode;
            this.OrderInfo.BillingState = this.OrderInfo.CustomerState;
            this.OrderInfo.BillingCountry = this.OrderInfo.CustomerCountry;
            this.OrderInfo.Save();

        }

        public void CopyShippingToCustomer()
        {
            this.OrderInfo.CustomerCompany = this.OrderInfo.DeliveryCompany;
            this.OrderInfo.CustomerAddressLine1 = this.OrderInfo.DeliveryAddress1;
            this.OrderInfo.CustomerAddressLine2 = this.OrderInfo.DeliveryAddress2;
            this.OrderInfo.CustomerCity = this.OrderInfo.DeliveryCity;
            this.OrderInfo.CustomerFirstName = this.OrderInfo.DeliveryFirstName;
            this.OrderInfo.CustomerLastName = this.OrderInfo.DeliveryLastName;
            this.OrderInfo.CustomerPostalCode = this.OrderInfo.DeliveryPostalCode;
            this.OrderInfo.CustomerState = this.OrderInfo.DeliveryState;
            this.OrderInfo.CustomerCountry = this.OrderInfo.DeliveryCountry;
            this.OrderInfo.Save();

        }

        

        public void CopyCustomerToShipping()
        {
            this.OrderInfo.DeliveryCompany = this.OrderInfo.CustomerCompany;
            this.OrderInfo.DeliveryAddress1 = this.OrderInfo.CustomerAddressLine1;
            this.OrderInfo.DeliveryAddress2 = this.OrderInfo.CustomerAddressLine2;
            this.OrderInfo.DeliveryCity = this.OrderInfo.CustomerCity;
            this.OrderInfo.DeliveryFirstName = this.OrderInfo.CustomerFirstName;
            this.OrderInfo.DeliveryLastName = this.OrderInfo.CustomerLastName;
            this.OrderInfo.DeliveryPostalCode = this.OrderInfo.CustomerPostalCode;
            this.OrderInfo.DeliveryState = this.OrderInfo.CustomerState;
            this.OrderInfo.DeliveryCountry = this.OrderInfo.CustomerCountry;
            this.OrderInfo.Save();
        }

        


        public bool AddOfferToCart(Offer offer, int quantity)
        {
            if (offer == null) { return false; }

            bool foundOfferInCart = false;
            bool deleted = false;
            foreach (CartOffer cOffer in CartOffers)
            {
                if (cOffer.OfferGuid == offer.Guid)
                {
                    foundOfferInCart = true;
                    cOffer.Quantity += quantity;
                    if (cOffer.Quantity <= 0)
                    {
                        DBCartOffer.Delete(cOffer.ItemGuid);
                        ResetCartOffers();
                        deleted = true;
                    }
                    else
                    {
                        cOffer.Save();
                    }
                    RefreshTotals();
                    Save();
                }
            }

            if (foundOfferInCart) 
            { 
                return !deleted; 
            }

            if (quantity <= 0) { return false; }

            CartOffer cartOffer = new CartOffer();
            cartOffer.CartGuid = this.cartGuid;
            //cartOffer.CurrencyGuid = currencyGuid;
            cartOffer.TaxClassGuid = offer.TaxClassGuid;
            cartOffer.OfferGuid = offer.Guid;
            cartOffer.OfferPrice = offer.Price;
            cartOffer.Quantity = quantity;
            // this will be updated later, just initialize to 0
            cartOffer.Tax = 0;
            cartOffer.IsDonation = offer.IsDonation;
            cartOffer.Save();

            // clear offers collection so it will be refreshed
            cartOffers = GetCartOffers();

            //CalculateSubTotal();
            //CalculatShippingTotal();
            //CalculateTaxTotal();
            //CalculateTotal();
            //Save();
            RefreshTotals();
           

            return true;
        }

        public bool UpdateCartItemQuantity(Guid itemGuid, int quantity)
        {
            foreach (CartOffer cartOffer in CartOffers)
            {
                if (cartOffer.ItemGuid == itemGuid)
                {
                    if (quantity <= 0)
                    {
                        DBCartOffer.Delete(itemGuid);
                        ResetCartOffers();
                    }
                    else
                    {
                        cartOffer.Quantity = quantity;
                        cartOffer.Save();
                    }

                    RefreshTotals();
                    Save();

                    return true;
                }

            }

            return false;

        }

        public void ClearItems()
        {
            foreach (CartOffer o in CartOffers)
            {
                DeleteItem(o.ItemGuid);
            }
            ResetCartOffers();
            RefreshTotals();
        }

        public bool DeleteItem(Guid itemGuid)
        {
           bool result = DBCartOffer.Delete(itemGuid);

           if (result)
           {
               RefreshTotals();
               Save();
           }

           return result;

        }

        public IDataReader GetItems()
        {
            return DBCartOffer.GetByCart(this.cartGuid);

        }

        public List<CartOffer> GetOffers()
        {
            return CartOffer.GetByCart(this.cartGuid);

        }

        /// <summary>
        /// Found out 2008-08-25 that you can't use google checkout to take donations unless you are a tax exempt non profit
        /// https://checkout.google.com/support/sell/bin/answer.py?answer=75724
        /// 
        /// Donation solicitations from parties without a valid 501(c)(3) tax exempt status clearly displayed to the public; 
        /// solicitations from parties without valid proof of exempt tax status or proof of registration with the relevant country's 
        /// regulatory bodies and authorities; and political organizations that have registered with the FEC.
        /// </summary>
        /// <returns></returns>
        public bool HasDonations()
        {
            foreach (CartOffer offer in CartOffers)
            {
                if (offer.IsDonation) { return true; }
            }

            return false;
        }

        public bool HasOffer(Guid offerGuid)
        {
            foreach (CartOffer offer in CartOffers)
            {
                if (offer.OfferGuid == offerGuid) { return true; }
            }

            return false;
        }

        public bool HasDownloadProducts()
        {
            int count = DBCart.GetItemCountByFulfillmentType(this.cartGuid, (byte)FulfillmentType.Download);
           
            return (count > 0);
        }

        public bool HasShippingProducts()
        {
            int count = DBCart.GetItemCountByFulfillmentType(this.cartGuid, (byte)FulfillmentType.PhysicalShipment);

            return (count > 0);
        }

        public void ResetUserInfo()
        {
            CartOrderInfo info = new CartOrderInfo();
            info.CartGuid = this.cartGuid;
            info.Save();
            orderInfo = info;
            


        }

        public void RefreshTotals()
        {
            CalculateSubTotal();
            CalculatShippingTotal();
            CalculateTaxTotal();
            CalculateTotal();
            Save();

        }


        public void SerializeCartOffers()
        {
            //if (cartOffers == null)
            //{
            //    cartOffers = GetCartOffers();
            //}

            ArrayList arrayList = new ArrayList();
            foreach (CartOffer c in CartOffers)
            {
        
                arrayList.Add(SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(c)));

            }

            this.serializedCartOffers = SerializationHelper.RemoveXmlDeclaration(SerializationHelper.SerializeToString(arrayList));

        }

        public string GetStringOfCartOfferNames()
        {
            StringBuilder s = new StringBuilder();

            string comma = string.Empty;
            foreach (CartOffer c in CartOffers)
            {
                s.Append(comma);
                s.Append(c.Name);

                comma = ", ";
            }

            return s.ToString();

        }

        public string GetCommaSeparatedStringOfCartOfferGuids()
        {
            StringBuilder s = new StringBuilder();

            string comma = string.Empty;
            foreach (CartOffer c in CartOffers)
            {
                s.Append(comma);
                s.Append(c.OfferGuid.ToString());

                comma = ", ";
            }

            return s.ToString();

        }


        public void DeSerializeCartOffers()
        {
            

            if (this.serializedCartOffers.Length == 0) return;
            ArrayList arrayList = SerializationHelper.DeserializeFromString(typeof(ArrayList), 
                SerializationHelper.RestoreXmlDeclaration(this.serializedCartOffers)) as ArrayList;

            if (arrayList == null) return;

            if (this.cartOffers == null) this.cartOffers = new List<CartOffer>();

            if (this.cartOffers.Count > 0) this.cartOffers.Clear();

            foreach (string c in arrayList)
            {
                CartOffer offer = SerializationHelper.DeserializeFromString(typeof(CartOffer), 
                    SerializationHelper.RestoreXmlDeclaration(c)) as CartOffer;

                this.cartOffers.Add(offer);
            }

        }


        #endregion

        #region Static Methods

        public static bool Delete(Guid cartGuid)
        {
            CartOffer.DeleteByCart(cartGuid);
            CartOrderInfo.Delete(cartGuid);

            return DBCart.DeleteCart(cartGuid);
        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            CartOffer.DeleteAnonymousByStore(storeGuid, olderThan);
            CartOrderInfo.DeleteAnonymousByStore(storeGuid, olderThan);

            return DBCart.DeleteAnonymousByStore(storeGuid, olderThan);
        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            CartOffer.DeleteByStore(storeGuid, olderThan);
            CartOrderInfo.DeleteByStore(storeGuid, olderThan);

            return DBCart.DeleteByStore(storeGuid, olderThan);
        }


        /// <summary>
        /// Gets a page of data from the ws_Cart table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid storeGuid,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            return DBCart.GetPage(
                storeGuid,
                pageNumber,
                pageSize,
                out totalPages);

        }
        

        /*
        // TODO: uncomment and implement if needed
        public static DataTable GetPage(int pageNumber, int pageSize)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("CartGuid",typeof(Guid));
            dataTable.Columns.Add("StoreGuid",typeof(Guid));
            dataTable.Columns.Add("UserGuid",typeof(Guid));
            dataTable.Columns.Add("CurrencyGuid",typeof(Guid));
            dataTable.Columns.Add("CurrencyValue",typeof(decimal));
            dataTable.Columns.Add("SubTotal",typeof(decimal));
            dataTable.Columns.Add("TaxTotal",typeof(decimal));
            dataTable.Columns.Add("OrderTotal",typeof(decimal));
            dataTable.Columns.Add("Created",typeof(DateTime));
            dataTable.Columns.Add("CreatedFromIP",typeof(string));
            dataTable.Columns.Add("LastModified",typeof(DateTime));
            dataTable.Columns.Add("LastUserActivity",typeof(DateTime));
            dataTable.Columns.Add("TotalPages", typeof(int));
		
            IDataReader reader = DBCart.GetCartPage(pageNumber, pageSize);	
            while (reader.Read())
            {
                DataRow row = dataTable.NewRow();
                row["CartGuid"] = reader["CartGuid"];
                row["StoreGuid"] = reader["StoreGuid"];
                row["UserGuid"] = reader["UserGuid"];
                row["CurrencyGuid"] = reader["CurrencyGuid"];
                row["CurrencyValue"] = reader["CurrencyValue"];
                row["SubTotal"] = reader["SubTotal"];
                row["TaxTotal"] = reader["TaxTotal"];
                row["OrderTotal"] = reader["OrderTotal"];
                row["Created"] = reader["Created"];
                row["CreatedFromIP"] = reader["CreatedFromIP"];
                row["LastModified"] = reader["LastModified"];
                row["LastUserActivity"] = reader["LastUserActivity"];
                row["TotalPages"] = Convert.ToInt32(reader["TotalPages"]);
                dataTable.Rows.Add(row);
            }
		
            reader.Close();
		
            return dataTable;
		
        }
	
        */

        #endregion


    }

}
