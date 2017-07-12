// Author:					
// Created:				    2008-06-02
// Last Modified:			2013-09-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.


namespace mojoPortal.Web.UI
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISettingControl
    {
        void SetValue(string val);
        string GetValue();
    }

    /// <summary>
    /// when you have a user profile property that implements ISettingControl, if you also implement IReadOnlySettingControl
    /// then this control will be used also on ProfileView.aspx whereas otherwise if using ISettingControl the raw value is rendered
    /// so if you need to process the setting value in some way for display you should implement this in addition to the above ISettingControl
    /// ie maybe the ISettingControl has a dropdown list but you don't want to display the value you want to display what would have displayed as the text in the dropdown list
    /// but in this case you should not use a dropdown list since it would be read only and dropdown is for selecting a value.
    /// </summary>
    public interface IReadOnlySettingControl
    {
        void SetReadOnlyValue(string val);
        
    }
}
