using System;
using System.Configuration.Provider;

namespace mojoPortal.Web.Editor
{
    /// <summary>
    /// Author:		        
    /// Created:            2007/05/18
    /// Last Modified:      2007/05/25
    /// 
    /// Licensed under the terms of the GNU Lesser General Public License:
    ///	http://www.opensource.org/licenses/lgpl-license.php
    ///
    /// You must not remove this notice, or any other, from this software.
    /// 
    /// </summary>
    public class EditorProviderCollection : ProviderCollection
    {
        public override void Add(ProviderBase provider)
        {
            if (provider == null)
                throw new ArgumentNullException("The provider parameter cannot be null.");

            if (!(provider is EditorProvider))
                throw new ArgumentException("The provider parameter must be of type EditorProvider.");

            base.Add(provider);
        }

        new public EditorProvider this[string name]
        {
            get { return (EditorProvider)base[name]; }
        }

        public void CopyTo(EditorProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }

    }
}
