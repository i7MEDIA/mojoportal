// Author:		        
// Created:            2007-08-15
// Last Modified:      2014-05-22
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System.Web.UI;

namespace mojoPortal.Web.Controls.Captcha
{
    public interface ICaptcha
    {
        Control GetControl();
        bool IsValid { get;}
        string ControlID { get;set;}
        string ValidationGroup { get; set; }
        bool Enabled { get; set; }
		short TabIndex { get; set; }
        //string ValidationGroup { get; set; }
    }
}
