/// Author:					Joe Audette
/// Created:				2007-03-14
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
using System.Collections.Generic;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents an offer in the cart
    /// </summary>
    [Serializable()]
    public class CartOffer
    {

        #region Constructors

        public CartOffer()
        { }

        #endregion

        #region Private Properties

        private string name = string.Empty;
        private Guid itemGuid = Guid.Empty;
        private Guid cartGuid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        //private Guid priceGuid = Guid.Empty;
        //private Guid currencyGuid = Guid.Empty;
        private Guid taxClassGuid = Guid.Empty;
        private decimal offerPrice = 0;
        private decimal tax = 0;
        private DateTime addedToCart = DateTime.UtcNow;
        private int quantity = 1;
        private bool isDonation = false;

        #endregion

        #region Public Properties

        public string Name
        {
            get { return name; }
            
        }

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; } //needs to get writeable to apply guid to current cart
        }
        public Guid CartGuid
        {
            get { return cartGuid; }
            set { cartGuid = value; }
        }
        public Guid OfferGuid
        {
            get { return offerGuid; }
            set { offerGuid = value; }
        }
        //public Guid PriceGuid
        //{
        //    get { return priceGuid; }
        //    set { priceGuid = value; }
        //}
        //public Guid CurrencyGuid
        //{
        //    get { return currencyGuid; }
        //    set { currencyGuid = value; }
        //}
        public Guid TaxClassGuid
        {
            get { return taxClassGuid; }
            set { taxClassGuid = value; }
        }
        public decimal OfferPrice
        {
            get { return offerPrice; }
            set { offerPrice = value; }
        }
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        public DateTime AddedToCart
        {
            get { return addedToCart; }
            set { addedToCart = value; }
        }
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public bool IsDonation
        {
            get { return isDonation; }
            set { isDonation = value; }
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Persists a new instance of CartOffer. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.itemGuid = Guid.NewGuid();

            int rowsAffected = DBCartOffer.Add(
                this.itemGuid,
                this.cartGuid,
                this.offerGuid,
                this.taxClassGuid,
                this.offerPrice,
                this.addedToCart,
                this.quantity,
                this.tax,
                this.isDonation);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of CartOffer. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBCartOffer.Update(
                this.itemGuid,
                this.offerGuid,
                this.taxClassGuid,
                this.offerPrice,
                this.addedToCart,
                this.quantity,
                this.tax,
                this.isDonation);

        }


        #endregion


        /// <summary>
        /// Saves this instance of CartOffer. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.itemGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }


        public static List<CartOffer> GetByCart(Guid cartGuid)
        {
            IDataReader reader = DBCartOffer.GetByCart(cartGuid);
            return LoadListFromReader(reader);


        }

        private static List<CartOffer> LoadListFromReader(IDataReader reader)
        {
            List<CartOffer> cartOfferList = new List<CartOffer>();
            try
            {
                while (reader.Read())
                {
                    CartOffer cartOffer = new CartOffer();
                    cartOffer.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    cartOffer.cartGuid = new Guid(reader["CartGuid"].ToString());
                    cartOffer.offerGuid = new Guid(reader["OfferGuid"].ToString());

                    cartOffer.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    cartOffer.offerPrice = Convert.ToDecimal(reader["OfferPrice"]);
                    cartOffer.addedToCart = Convert.ToDateTime(reader["AddedToCart"]);
                    cartOffer.quantity = Convert.ToInt32(reader["Quantity"]);
                    cartOffer.name = reader["Name"].ToString();
                    if (reader["Tax"] != DBNull.Value)
                    {
                        cartOffer.tax = Convert.ToDecimal(reader["Tax"]);
                    }

                    cartOffer.isDonation = Convert.ToBoolean(reader["IsDonation"]);

                    cartOfferList.Add(cartOffer);

                }
            }
            finally
            {
                reader.Close();
            }

            return cartOfferList;

        }

        public static bool DeleteByCart(Guid cartGuid)
        {
            return DBCartOffer.DeleteByCart(cartGuid);
        }

        public static bool DeleteAnonymousByStore(Guid storeGuid, DateTime olderThan)
        {
            return DBCartOffer.DeleteAnonymousByStore(storeGuid, olderThan);
        }

        public static bool DeleteByStore(Guid storeGuid, DateTime olderThan)
        {
            return DBCartOffer.DeleteByStore(storeGuid, olderThan);
        }

    }

}
