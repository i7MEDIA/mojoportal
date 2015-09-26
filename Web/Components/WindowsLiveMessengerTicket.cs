//-----------------------------------------------------------------------
// <copyright file="MessengerTicket.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <summary>
//     Represents a Messenger Ticket 
// </summary>
//-----------------------------------------------------------------------
//namespace WindowsLive
namespace mojoPortal.Web
{
    using System.Diagnostics;

    /// <summary>
    /// MessengerTicket
    /// </summary>
    public class MessengerTicket
    {
        private readonly string ticket;
        private readonly string signature;

        public MessengerTicket(string ticket, string signature)
        {
            Debug.Assert(!string.IsNullOrEmpty(ticket));
            Debug.Assert(!string.IsNullOrEmpty(signature));

            this.ticket = ticket;
            this.signature = signature;
        }

        public string Signature
        {
            get { return signature; }
        }

        public string Ticket
        {
            get { return ticket; }
        }
    }
}
