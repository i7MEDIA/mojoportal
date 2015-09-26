// Author:					Joe Audette
// Created:					2009-3-3
// Last Modified:			2009-3-3
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
using System.Collections.Generic;
using System.Data;
using WebStore.Data;

namespace WebStore.Business
{

    public class Discount
    {

        #region Constructors

        public Discount()
        { }


        public Discount(Guid discountGuid)
        {
            GetDiscount(discountGuid);
        }

        public Discount(Guid moduleGuid, string discountCode)
        {
            GetDiscount(moduleGuid, discountCode);
        }

        #endregion

        #region Private Properties

        private Guid discountGuid = Guid.Empty;
        private string discountCode = string.Empty;
        private string description = string.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid storeGuid = Guid.Empty;
        private Guid offerGuid = Guid.Empty;
        private DateTime validityStartDate = DateTime.UtcNow;
        private DateTime validityEndDate = DateTime.MaxValue;
        private int useCount = 0;
        private int maxCount = 0;
        private decimal minOrderAmount = 0;
        private decimal absoluteDiscount = 0;
        private decimal percentageDiscount = 0;
        private Guid createdBy = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid lastModBy = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;

        // TODO: forgot to add a db field for this
        private bool allowOtherDiscounts = false;

        

        #endregion

        #region Public Properties

        public Guid DiscountGuid
        {
            get { return discountGuid; }
            set { discountGuid = value; }
        }
        public string DiscountCode
        {
            get { return discountCode; }
            set { discountCode = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }
        public Guid StoreGuid
        {
            get { return storeGuid; }
            set { storeGuid = value; }
        }
        public Guid OfferGuid
        {
            get { return offerGuid; }
            set { offerGuid = value; }
        }
        public DateTime ValidityStartDate
        {
            get { return validityStartDate; }
            set { validityStartDate = value; }
        }
        public DateTime ValidityEndDate
        {
            get { return validityEndDate; }
            set { validityEndDate = value; }
        }
        public int UseCount
        {
            get { return useCount; }
            set { useCount = value; }
        }
        public int MaxCount
        {
            get { return maxCount; }
            set { maxCount = value; }
        }
        public decimal MinOrderAmount
        {
            get { return minOrderAmount; }
            set { minOrderAmount = value; }
        }
        public bool AllowOtherDiscounts
        {
            get { return allowOtherDiscounts; }
            set { allowOtherDiscounts = value; }
        }
        public decimal AbsoluteDiscount
        {
            get { return absoluteDiscount; }
            set { absoluteDiscount = value; }
        }
        public decimal PercentageDiscount
        {
            get { return percentageDiscount; }
            set { percentageDiscount = value; }
        }
        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }
        public Guid LastModBy
        {
            get { return lastModBy; }
            set { lastModBy = value; }
        }
        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of Discount.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        private void GetDiscount(Guid discountGuid)
        {
            using (IDataReader reader = DBDiscount.GetOne(discountGuid))
            {
                PopulateFromReader(reader);
            }

        }

        /// <summary>
        /// Gets an instance of Discount.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        private void GetDiscount(Guid moduleGuid, string discountCode)
        {
            using (IDataReader reader = DBDiscount.GetOne(moduleGuid, discountCode))
            {
                PopulateFromReader(reader);
            }

        }


        private void PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.discountGuid = new Guid(reader["DiscountGuid"].ToString());
                this.discountCode = reader["DiscountCode"].ToString();
                this.description = reader["Description"].ToString();
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.storeGuid = new Guid(reader["StoreGuid"].ToString());
                this.offerGuid = new Guid(reader["OfferGuid"].ToString());
                this.validityStartDate = Convert.ToDateTime(reader["ValidityStartDate"]);
                if (reader["ValidityEndDate"] != DBNull.Value)
                {
                    this.validityEndDate = Convert.ToDateTime(reader["ValidityEndDate"]);
                }
                this.useCount = Convert.ToInt32(reader["UseCount"]);
                this.maxCount = Convert.ToInt32(reader["MaxCount"]);
                this.minOrderAmount = Convert.ToDecimal(reader["MinOrderAmount"]);
                this.absoluteDiscount = Convert.ToDecimal(reader["AbsoluteDiscount"]);
                this.percentageDiscount = Convert.ToDecimal(reader["PercentageDiscount"]);
                this.createdBy = new Guid(reader["CreatedBy"].ToString());
                this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                this.lastModBy = new Guid(reader["LastModBy"].ToString());
                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                this.allowOtherDiscounts = Convert.ToBoolean(reader["AllowOtherDiscounts"]);

            }

        }

        /// <summary>
        /// Persists a new instance of Discount. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.discountGuid = Guid.NewGuid();

            int rowsAffected = DBDiscount.Create(
                this.discountGuid,
                this.discountCode,
                this.description,
                this.siteGuid,
                this.moduleGuid,
                this.storeGuid,
                this.offerGuid,
                this.validityStartDate,
                this.validityEndDate,
                this.useCount,
                this.maxCount,
                this.minOrderAmount,
                this.absoluteDiscount,
                this.percentageDiscount,
                this.allowOtherDiscounts,
                this.createdBy,
                this.createdUtc);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of Discount. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBDiscount.Update(
                this.discountGuid,
                this.discountCode,
                this.description,
                this.offerGuid,
                this.validityStartDate,
                this.validityEndDate,
                this.useCount,
                this.maxCount,
                this.minOrderAmount,
                this.absoluteDiscount,
                this.percentageDiscount,
                this.allowOtherDiscounts,
                this.lastModBy,
                this.lastModUtc);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of Discount. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.discountGuid != Guid.Empty)
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

        /// <summary>
        /// Deletes an instance of Discount. Returns true on success.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid discountGuid)
        {
            return DBDiscount.Delete(discountGuid);
        }

        /// <summary>
        /// Deletes instances of Discount. Returns true on success.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBDiscount.DeleteBySite(siteGuid);
        }

        /// <summary>
        /// Deletes instances of Discount. Returns true on success.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBDiscount.DeleteByModule(moduleGuid);
        }

        /// <summary>
        /// Deletes instances of Discount. Returns true on success.
        /// </summary>
        /// <param name="discountGuid"> discountGuid </param>
        /// <returns>bool</returns>
        public static bool DeleteByOffer(Guid offerGuid)
        {
            return DBDiscount.DeleteByOffer(offerGuid);
        }


        /// <summary>
        /// Gets a count of Discount. 
        /// </summary>
        public static int GetCount(Guid moduleGuid)
        {
            return DBDiscount.GetCount(moduleGuid);
        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCountOfActiveDiscountCodes(Guid moduleGuid, DateTime activeForDate)
        {
            return DBDiscount.GetCountOfActiveDiscountCodes(moduleGuid, activeForDate);
        }

        /// <summary>
        /// Gets a count of rows in the ws_Discount table.
        /// </summary>
        public static int GetCountOfActiveDiscountCodes(Guid moduleGuid)
        {
            return DBDiscount.GetCountOfActiveDiscountCodes(moduleGuid, DateTime.UtcNow);
        }

        private static List<Discount> LoadListFromReader(IDataReader reader)
        {
            List<Discount> discountList = new List<Discount>();
            try
            {
                while (reader.Read())
                {
                    Discount discount = new Discount();
                    discount.discountGuid = new Guid(reader["DiscountGuid"].ToString());
                    discount.discountCode = reader["DiscountCode"].ToString();
                    discount.description = reader["Description"].ToString();
                    discount.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    discount.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    discount.storeGuid = new Guid(reader["StoreGuid"].ToString());
                    discount.offerGuid = new Guid(reader["OfferGuid"].ToString());
                    if (reader["ValidityEndDate"] != DBNull.Value)
                    {
                        discount.validityStartDate = Convert.ToDateTime(reader["ValidityStartDate"]);
                    }
                    discount.validityEndDate = Convert.ToDateTime(reader["ValidityEndDate"]);
                    discount.useCount = Convert.ToInt32(reader["UseCount"]);
                    discount.maxCount = Convert.ToInt32(reader["MaxCount"]);
                    discount.minOrderAmount = Convert.ToDecimal(reader["MinOrderAmount"]);
                    discount.absoluteDiscount = Convert.ToDecimal(reader["AbsoluteDiscount"]);
                    discount.percentageDiscount = Convert.ToDecimal(reader["PercentageDiscount"]);
                    discount.createdBy = new Guid(reader["CreatedBy"].ToString());
                    discount.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    discount.lastModBy = new Guid(reader["LastModBy"].ToString());
                    discount.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    discount.allowOtherDiscounts = Convert.ToBoolean(reader["AllowOtherDiscounts"]);
                    discountList.Add(discount);

                }
            }
            finally
            {
                reader.Close();
            }

            return discountList;

        }

        public static List<Discount> GetValidDiscounts(Guid moduleGuid, Cart cart, string discountCodeCsv)
        {
            List<Discount> discountList = new List<Discount>();

            if (moduleGuid == Guid.Empty) { return discountList; }
            if (string.IsNullOrEmpty(discountCodeCsv)) { return discountList; }

            string[] discountCodes = discountCodeCsv.Split(',');
            bool foundAllowOtherConflict = false;

            foreach (string code in discountCodes)
            {
                if (code.Length > 0)
                {
                    Discount discount = new Discount(moduleGuid, code);
                    // validity checks
                    if (discount.DiscountGuid == Guid.Empty) { continue; }
                    if (discount.ValidityStartDate > DateTime.UtcNow) { continue; }
                    if (discount.ValidityEndDate < DateTime.UtcNow) { continue; }
                    if((discount.AbsoluteDiscount == 0)&&(discount.PercentageDiscount == 0)){ continue;}
                    if ((discount.MaxCount> 0)&&(discount.UseCount >= discount.MaxCount)) { continue; }
                    if((discount.MinOrderAmount > 0)&&(discount.MinOrderAmount > cart.SubTotal)) { continue; }
                    if ((discount.OfferGuid != Guid.Empty)&&(!cart.HasOffer(discount.offerGuid))) { continue; }

                    if (!discount.AllowOtherDiscounts) 
                    {
                        if (discountList.Count > 0)
                        {
                            discountList.Clear();
                        }
                        foundAllowOtherConflict = true;
                    }
                 
                    discountList.Add(discount);
                    
                    // only add the first valid discount if it doesn't allow others
                    if (foundAllowOtherConflict) return discountList;

                }

            }

            return discountList;
        }

        

        /// <summary>
        /// Gets an IDataReader with page of instances of Discount.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            Guid moduleGuid,
            int pageNumber, 
            int pageSize, 
            out int totalPages)
        {
            totalPages = 1;

            return DBDiscount.GetPage(
                moduleGuid,
                pageNumber, 
                pageSize, 
                out totalPages);
            
        }



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByDiscountCode(Discount discount1, Discount discount2)
        {
            return discount1.DiscountCode.CompareTo(discount2.DiscountCode);
        }
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByDescription(Discount discount1, Discount discount2)
        {
            return discount1.Description.CompareTo(discount2.Description);
        }
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByValidityStartDate(Discount discount1, Discount discount2)
        {
            return discount1.ValidityStartDate.CompareTo(discount2.ValidityStartDate);
        }
        
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByUseCount(Discount discount1, Discount discount2)
        {
            return discount1.UseCount.CompareTo(discount2.UseCount);
        }
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByMaxCount(Discount discount1, Discount discount2)
        {
            return discount1.MaxCount.CompareTo(discount2.MaxCount);
        }
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByCreatedUtc(Discount discount1, Discount discount2)
        {
            return discount1.CreatedUtc.CompareTo(discount2.CreatedUtc);
        }
        /// <summary>
        /// Compares 2 instances of Discount.
        /// </summary>
        public static int CompareByLastModUtc(Discount discount1, Discount discount2)
        {
            return discount1.LastModUtc.CompareTo(discount2.LastModUtc);
        }

        #endregion


    }

}
