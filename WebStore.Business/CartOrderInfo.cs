/// Author:					Joe Audette
/// Created:				2007-03-05
/// Last Modified:			2009-02-01
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents order information attached to a cart prior to completing an order.
    /// </summary>
    [Serializable()]
    public class CartOrderInfo
    {

        #region Constructors

        public CartOrderInfo()
        { }


        public CartOrderInfo(Guid cartGuid)
        {
            suppliedGuid = cartGuid;
            GetCartOrderInfo(cartGuid);
        }

        #endregion

        #region Private Properties

        private Guid suppliedGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
        //private string customerName = string.Empty;
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
        //private string deliveryName = string.Empty;
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
        //private string billingName = string.Empty;
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
        private DateTime completed = DateTime.UtcNow;
        private string completedFromIP = string.Empty;
        private Guid taxZoneGuid = Guid.Empty;

        private bool exists = false;

        
        

        #endregion

        #region Public Properties

        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }
        //public string CustomerName
        //{
        //    get { return customerName; }
        //    set { customerName = value; }
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

        public Guid TaxZoneGuid
        {
            get { return taxZoneGuid; }
            set { taxZoneGuid = value; }
        }

        public bool Exists
        {
            get { return exists; }
        }

        #endregion

        #region Private Methods

        private void GetCartOrderInfo(Guid cartGuid)
        {
            using (IDataReader reader = DBCartOrderInfo.Get(cartGuid))
            {
                GetCartOrderInfo(reader);
            }

        }

        private void GetCartOrderInfo(IDataReader reader)
        {
            if (reader.Read())
            {
                exists = true;
                this.cartGuid = new Guid(reader["CartGuid"].ToString());
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
                if (reader["Completed"] != DBNull.Value)
                {
                    this.completed = Convert.ToDateTime(reader["Completed"]);
                }
                this.completedFromIP = reader["CompletedFromIP"].ToString();

                if (reader["TaxZoneGuid"] != DBNull.Value)
                {
                    this.taxZoneGuid = new Guid(reader["TaxZoneGuid"].ToString());
                }

            }

            

        }

        private bool Create()
        {
            Guid newID;
            if (suppliedGuid == Guid.Empty)
            {
                newID = Guid.NewGuid();
            }
            else
            {
                newID = suppliedGuid;
            }

            this.cartGuid = newID;

            int rowsAffected = DBCartOrderInfo.Add(
                this.cartGuid,
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
                this.completed,
                this.completedFromIP,
                this.taxZoneGuid);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBCartOrderInfo.Update(
                this.cartGuid,
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
                this.completed,
                this.completedFromIP,
                this.taxZoneGuid);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.cartGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        public static bool Delete(Guid cartGuid)
        {
            return DBCartOrderInfo.Delete(cartGuid);
        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            return DBCartOrderInfo.DeleteAnonymousByStore(storeGuid, olderThan);
        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            return DBCartOrderInfo.DeleteByStore(storeGuid, olderThan);
        }

        

        #endregion


    }

}
