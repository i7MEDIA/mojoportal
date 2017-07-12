// Author:					
// Created:				    2011-05-20
// Last Modified:			2011-05-20
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
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// a sub class of BasePanel so it can be configured differently than panels used in other scenarios
    /// this one is intended for empty panels use only for decoration in some designs. by theme.skin configuration it can be eliminated from designs not using it
    /// </summary>
    public class EmptyPanel : BasePanel
    {
    }
}