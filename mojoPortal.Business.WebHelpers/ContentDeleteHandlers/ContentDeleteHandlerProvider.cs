//  Author:                     
//  Created:                    2009-06-21
//	Last Modified:              2009-06-21
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

namespace mojoPortal.Business.WebHelpers
{
    /// <summary>
    /// when content instances aka modules are deleted from the content catalog
    /// we need a feature specific way to delete content for the feature that is associated with the deleted module
    /// this a a base class for features to implement.
    /// The FeatureDefinition.DeleteProvider must correspond to the name of the feature specific implementation,
    /// so that the system can invoke the correct ContentDeleteHandlerProvider
    /// </summary>
    public abstract class ContentDeleteHandlerProvider : ProviderBase
    {
        public abstract void DeleteContent(int moduleId, Guid moduleGuid);
    }
}
