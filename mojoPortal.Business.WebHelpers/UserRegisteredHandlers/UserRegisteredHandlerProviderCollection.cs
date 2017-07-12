//  Author:                     
//  Created:                    2008-07-03
//	Last Modified:              2008-07-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration.Provider;

namespace mojoPortal.Business.WebHelpers.UserRegisteredHandlers
{
    /// <summary>
    ///  
    /// </summary>
    public class UserRegisteredHandlerProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is UserRegisteredHandlerProvider))
                throw new ArgumentException("The provider parameter must be of type UserRegisteredHandlerProvider.");

            base.Add(provider);
        }

        new public UserRegisteredHandlerProvider this[string name]
        {
            get { return (UserRegisteredHandlerProvider)base[name]; }
        }

        public void CopyTo(UserRegisteredHandlerProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }

    }
}
