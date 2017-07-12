using System;
using System.Collections;
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
    ///  An transaction you can map to from your commerce order for tracking purchases in google Analytics.
    ///  You must add transaction items corresponding to your order items.
    /// </summary>
    public class AnalyticsTransaction
    {
        /// <summary>
        /// An transaction you can map to from your commerce order for tracking purchases in google Analytics.
        /// You must add transaction items corresponding to your order items.
        /// </summary>
        public AnalyticsTransaction()
        {

        }

        private string orderId = string.Empty; //required
        private string storeName = string.Empty;
        private string total = string.Empty; // required
        private string tax = string.Empty;
        private string shipping = string.Empty;
        private string city = string.Empty;
        private string state = string.Empty;
        private string country = string.Empty;
        private List<AnalyticsTransactionItem> items = new List<AnalyticsTransactionItem>();

        /// <summary>
        /// Requires at least one item
        /// </summary>
        public List<AnalyticsTransactionItem> Items
        {
            get { return items; }
            
        }

        /// <summary>
        /// Call this to check validity before rendering the google analytics transaction
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            if (orderId.Length == 0) { return false; }
            if (total.Length == 0) { return false; }
            if (items.Count == 0) { return false; }

            // if there is at least one valid item we will render
            // not sure this is the very best decision, but its the one I'm making at the moment, 
            // the order total won't jibe with the items though. My immediate thinking is some data is better than no data.
            bool foundValidItem = false;
            foreach (AnalyticsTransactionItem item in items)
            {
                if (item.IsValid()) { foundValidItem = true; }
            }

            return foundValidItem;
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
        /// 
        /// </summary>
        public string StoreName
        {
            get { return storeName; }
            set { storeName = value; }
        }

        /// <summary>
        /// Required
        /// </summary>
        public string Total
        {
            get { return total; }
            set { total = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Tax
        {
            get { return tax; }
            set { tax = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Shipping
        {
            get { return shipping; }
            set { shipping = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            get { return city; }
            set { city = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string State
        {
            get { return state; }
            set { state = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

    }
}
