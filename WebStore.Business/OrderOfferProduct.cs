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
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{
    /// <summary>
    /// Represents a product within an offer within an order
    /// </summary>
    public class OrderOfferProduct
    {

        #region Constructors

        public OrderOfferProduct()
        { }


        public OrderOfferProduct(Guid guid)
        {
            GetOrderOfferProduct(guid);
        }

        #endregion

        #region Private Properties

        private Guid guid = Guid.Empty;
        private Guid orderGuid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        private Guid productGuid = Guid.Empty;
        private byte fullfillType;
        private Guid fullfillTermsGuid = Guid.Empty;
        private DateTime created;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
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
        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }
        public byte FullfillType
        {
            get { return fullfillType; }
            set { fullfillType = value; }
        }
        public Guid FullfillTermsGuid
        {
            get { return fullfillTermsGuid; }
            set { fullfillTermsGuid = value; }
        }
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }

        #endregion

        #region Private Methods

        private void GetOrderOfferProduct(
            Guid guid)
        {
            using (IDataReader reader = DBOrderOfferProduct.Get(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.orderGuid = new Guid(reader["OrderGuid"].ToString());
                    this.offerGuid = new Guid(reader["OfferGuid"].ToString());
                    this.productGuid = new Guid(reader["ProductGuid"].ToString());
                    this.fullfillType = Convert.ToByte(reader["FullfillType"]);
                    this.fullfillTermsGuid = new Guid(reader["FullfillTermsGuid"].ToString());
                    this.created = Convert.ToDateTime(reader["Created"]);

                }

            }

        }

        private bool Create()
        {
            Guid newID = Guid.NewGuid();

            this.guid = newID;

            int rowsAffected = DBOrderOfferProduct.Add(
                this.guid,
                this.orderGuid,
                this.offerGuid,
                this.productGuid,
                this.fullfillType,
                this.fullfillTermsGuid,
                this.created);

            return (rowsAffected > 0);

        }



        private bool Update()
        {

            return DBOrderOfferProduct.Update(
                this.guid,
                this.orderGuid,
                this.offerGuid,
                this.productGuid,
                this.fullfillType,
                this.fullfillTermsGuid,
                this.created);

        }


        #endregion

        #region Public Methods


        public bool Save()
        {
            if (this.guid != Guid.Empty)
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

        public static bool Delete(Guid guid)
        {
            return DBOrderOfferProduct.Delete(guid);
        }


        public static bool DeleteByOrder(Guid orderGuid)
        {
            return DBOrderOfferProduct.DeleteByOrder(orderGuid);
        }
        

        #endregion


    }

}
