// Author:					Joe Audette
// Created:					2011-10-11
// Last Modified:			2011-10-11
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using mojoPortal.Business;
using WebStore.Business;

namespace WebStore.Components
{
    public delegate void OrderCompletedEventHandler(object sender, OrderCompletedEventArgs e);

    public class OrderCompletedEventArgs : EventArgs
    {
        public OrderCompletedEventArgs(Order order, CultureInfo currencyCulture)
        {
            _order = order;
            _currencyCulture = currencyCulture;
        }
        
        private Order _order = null;

        public Order Order
        {
            get { return _order; }
        }

        private CultureInfo _currencyCulture = new CultureInfo("en-US");

        public CultureInfo CurrencyCulture
        {
            get { return _currencyCulture; }
        }

    }
}