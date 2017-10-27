// Created:				    2008-06-02
// Last Modified:			2016-08-15
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
//
// 2016-08-12 Added Attributes 
// 2016-08-15 Renamed to ICustomField to prevent conflicts with ISettingControl in mojoPortal
// 2017-06-16 Moved from SuperFlexi to mojoPortal.Web

using System.Collections.Generic;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomField
    {
        void SetValue(string val);
        string GetValue();

		//void Attributes(string attribs);
		void Attributes(IDictionary<string, string> attribs);
    }
}