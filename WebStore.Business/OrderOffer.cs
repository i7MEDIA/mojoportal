/// Author:					Joe Audette
/// Created:				2007-03-18
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
using System.Globalization;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents an offer included in an order
    /// </summary>
    public class OrderOffer
    {

        #region Constructors

        public OrderOffer()
        { }


        public OrderOffer(Guid itemGuid)
        {
            GetOrderOffer(itemGuid);
        }

        #endregion

        #region Private Properties

        private Guid itemGuid = Guid.Empty;
        private Guid orderGuid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        
        private Guid taxClassGuid = Guid.Empty;
        private decimal offerPrice;
        private DateTime addedToCart;
        private int quantity;
        private string name = string.Empty;

        

        #endregion

        #region Public Properties

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }
        public Guid OrderGuid
        {
            get { return orderGuid; }
            set { orderGuid = value; }
        }
        public Guid OfferGuid
        {
            get { return offerGuid; }
            set { offerGuid = value; }
        }
        
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

        #endregion

        #region Private Methods

        private void GetOrderOffer(Guid itemGuid)
        {
            using (IDataReader reader = DBOrderOffer.Get(itemGuid))
            {
                if (reader.Read())
                {
                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.orderGuid = new Guid(reader["OrderGuid"].ToString());
                    this.offerGuid = new Guid(reader["OfferGuid"].ToString());

                    this.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    this.offerPrice = Convert.ToDecimal(reader["OfferPrice"]);
                    this.addedToCart = Convert.ToDateTime(reader["AddedToCart"]);
                    this.quantity = Convert.ToInt32(reader["Quantity"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.itemGuid = newID;

            int rowsAffected = DBOrderOffer.Add(
                this.itemGuid,
                this.orderGuid,
                this.offerGuid,
                this.taxClassGuid,
                this.offerPrice,
                this.addedToCart,
                this.quantity);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBOrderOffer.Update(
                this.itemGuid,
                this.orderGuid,
                this.offerGuid,
                this.taxClassGuid,
                this.offerPrice,
                this.addedToCart,
                this.quantity);

        }


        #endregion

        #region Public Methods


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




        #endregion

        #region Static Methods

        //public static bool Delete(Guid itemGuid)
        //{
        //    return DBOrderOffer.DeleteOrderOffers(itemGuid);
        //}

        public static bool DeleteByOrder(Guid orderGuid)
        {
            return DBOrderOffer.DeleteByOrder(orderGuid);
        }

        public static List<OrderOffer> GetByOrder(Guid orderGuid)
        {
            IDataReader reader = DBOrderOffer.GetByOrder(orderGuid);
            return LoadListFromReader(reader);


        }

        private static List<OrderOffer> LoadListFromReader(IDataReader reader)
        {
            List<OrderOffer> orderOfferList = new List<OrderOffer>();
            try
            {
                while (reader.Read())
                {
                    OrderOffer orderOffer = new OrderOffer();
                    orderOffer.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    orderOffer.orderGuid = new Guid(reader["OrderGuid"].ToString());
                    orderOffer.offerGuid = new Guid(reader["OfferGuid"].ToString());
                    orderOffer.taxClassGuid = new Guid(reader["TaxClassGuid"].ToString());
                    orderOffer.offerPrice = Convert.ToDecimal(reader["OfferPrice"], CultureInfo.InvariantCulture);

                    orderOffer.quantity = Convert.ToInt32(reader["Quantity"]);
                    orderOffer.name = reader["Name"].ToString();

                    orderOfferList.Add(orderOffer);

                }
            }
            finally
            {
                reader.Close();
            }

            return orderOfferList;

        }
        

        

        #endregion


    }

}
