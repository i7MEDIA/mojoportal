// Author:					
// Created:				    2008-01-04
// Last Modified:			2008-07-24
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Net;

namespace mojoPortal.Business
{
    /// <summary>
    /// 
    /// </summary>
    public static class IPAddressHelper
    {
        /// <summary>
        /// Takes an ip address in standard ipv4 notation and converts to its long representation
        /// </summary>
        /// <param name="ipv4Address"></param>
        /// <returns></returns>
        public static Int64 ConvertToLong(string ipv4Address)
        {
            Int64 result = 0;
            if (ipv4Address.Contains(":")) { return result; } // an ipv6 address was passed instead

            IPAddress ipAddress;
            if (IPAddress.TryParse(ipv4Address, out ipAddress))
            {
                byte[] b = ipAddress.GetAddressBytes();
                if (b.Length >= 4) // prevent index out of range error
                {
                    result = (long)(b[0] * 16777216);
                    result += (long)(b[1] * 65536);
                    result += (long)(b[2] * 256);
                    result += (long)(b[3] * 1);
                }
            }

            return result;
        }

        // previous implementation below
        // above implementation based on forum post:
        //https://www.mojoportal.com/Forums/Thread.aspx?thread=10686&mid=34&pageid=5&ItemID=9&pagenumber=1#post44446

        //public static Int64 ConvertToLong(string ipv4Address)
        //{
        //    Int64 result = 0;

        //    if (ipv4Address.Contains(":")) { return result; } // an ipv6 address was passed instead

        //    IPAddress ipAddress;
        //    if (IPAddress.TryParse(ipv4Address, out ipAddress))
        //    {
        //        byte[] b = ipAddress.GetAddressBytes();
        //        for (int i = 0; i < b.Length; i++)
        //        {
        //            result += (long)(b[i] * Math.Pow(256, i));
        //        }

        //    }

        //    return result;
        //}


        /// <summary>
        /// Takes a long value and converts it to standard ipv4 notation
        /// </summary>
        public static string ConvertToIPAddressString(Int64 ipAddressAsLong)
        {
            IPAddress ipAddress = new IPAddress(ipAddressAsLong);
            return ipAddress.ToString();

        }


    }
}
