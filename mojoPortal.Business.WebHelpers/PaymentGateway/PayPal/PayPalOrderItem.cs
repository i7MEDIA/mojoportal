// Author:					
// Created:				    2008-07-12
// Last Modified:			2008-07-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


namespace mojoPortal.Business.WebHelpers.PaymentGateway
{
    /// <summary>
    ///
    /// </summary>
    public class PayPalOrderItem
    {
        public PayPalOrderItem()
        { }

        private string itemName = string.Empty;
        private int quantity = 1;
        private string itemNumber = string.Empty;
        private decimal amount = 0;
        private decimal tax = 0;

        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }

        public string ItemNumber
        {
            get { return itemNumber; }
            set { itemNumber = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public decimal Tax
        {
            get { return tax; }
            set { tax = value; }
        }

    }
}
