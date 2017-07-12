// Author:					
// Created:				    2007-02-18
// Last Modified:			2008-07-30
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;

namespace mojoPortal.Business.Commerce
{
    /// <summary>
    /// Represents the status of an order
    /// </summary>
    public class OrderStatus
    {
        private OrderStatus()
        {

        }

        const string OrderStatusReceivedId = "0db28432-d9a9-423e-84f2-8a94db434643";
        const string OrderStatusFulfillableId = "70443443-f665-42c9-b69f-48cbf011a14b";
        const string OrderStatusFulfilledId = "67e92035-e8d0-4700-822b-a4002f2f1a15";
        const string OrderStatusCancelledId = "de3b9331-b98f-493f-be5e-926ffe5003bc";
        const string OrderStatusAnyId = "11111111-1111-1111-1111-111111111111";

        public static Guid OrderStatusNoneGuid
        {
            get { return Guid.Empty; }
        }

        public static Guid OrderStatusReceivedGuid
        {
            get { return new Guid(OrderStatusReceivedId); }
        }

        public static Guid OrderStatusFulfillableGuid
        {
            get { return new Guid(OrderStatusFulfillableId); }
        }

        public static Guid OrderStatusFulfilledGuid
        {
            get { return new Guid(OrderStatusFulfilledId); }
        }

        public static Guid OrderStatusCancelledGuid
        {
            get { return new Guid(OrderStatusCancelledId); }
        }

        public static Guid OrderStatusAny
        {
            get { return new Guid(OrderStatusAnyId); }
        }

    }
}
