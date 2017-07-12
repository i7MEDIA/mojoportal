using System;
using System.Collections.Generic;
using System.Text;

namespace mojoPortal.Web.Controls.google
{
    /// <summary>
    ///	Author:				
    ///	Created:			2008-08-12
    ///	Last Modified:		2008-08-12
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// An item you can map to from your commerce order items for tracking purchases in google Analytics
    ///  		
    /// </summary>
    public class AnalyticsTransactionItem
    {
        /// <summary>
        /// An item you can map to from your commerce order items for tracking purchases in google Analytics
        /// </summary>
        public AnalyticsTransactionItem()
        { }

        private string orderId = string.Empty; //required
        private string sku = string.Empty;
        private string productName = string.Empty;
        private string category = string.Empty;
        private string price = string.Empty; // required
        private string quantity = string.Empty; // required

        /// <summary>
        /// Call this to check validity before rendering the google analytics transaction
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (orderId.Length == 0) { return false; }
            if (price.Length == 0) { return false; }
            if (quantity.Length == 0) { return false; }

            return true;
        }

        /// <summary>
        /// Required
        /// </summary>
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        /// <summary>
        /// Optional
        /// </summary>
        public string Sku
        {
            get { return sku; }
            set { sku = value; }
        }

        /// <summary>
        /// Optional
        /// </summary>
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        /// <summary>
        /// Optional
        /// </summary>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Required
        /// </summary>
        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        /// <summary>
        /// Required
        /// </summary>
        public string Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }


    }
}
