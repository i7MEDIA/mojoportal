//  Author:                     
//  Created:                    2008-06-24
//	Last Modified:              2008-06-24
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
    ///  Represents the serialized state we log and use for various payment providers.
    /// </summary>
    [Serializable()]
    public class MerchantData
    {
        private string providerName = string.Empty;
        public string SerializedObject = string.Empty;
        //private object oCart = null;

        public string ProviderName
        {
            get { return providerName; }
            set { providerName = value; }
        }

        //public string SerializedObject
        //{
        //    get { return serializedObject; }
        //    set { serializedObject = value; }
        //}

        //public object CartObject
        //{
        //    get { return oCart; }
        //    set { oCart = value; }
        //}

    }
}
