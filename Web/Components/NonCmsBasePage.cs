//	Author:                 
//  Created:			    2010-08-13
//	Last Modified:		    2010-08-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

namespace mojoPortal.Web
{
    /// <summary>
    /// the main purpose of this class is to be used as a base page for non public pages
    /// such as administrative or edit pages. It makes it possible for control developers 
    /// to add logic to hide a control or not render if the current page is a NonCmsPage
    /// like if(Page is NonCmsPage){ this.Visible = false; return; }
    /// this page inherits from mojoBasePage so the functionality is the same as inheriting 
    /// directly from mojoBasePage
    /// </summary>
    public class NonCmsBasePage : mojoBasePage
    {
    }
}
