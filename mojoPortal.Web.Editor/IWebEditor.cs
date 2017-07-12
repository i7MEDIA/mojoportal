// Author:		        
// Created:            2007-05-29
// Last Modified:      2007-12-13
// 
// Licensed under the terms of the GNU Lesser General Public License:
//	http://www.opensource.org/licenses/lgpl-license.php
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Editor
{
    public interface IWebEditor
    {
        string ClientID { get;}
        string Text { get;set;}
        string SiteRoot { get;set;}
        string ScriptBaseUrl { get; set;}
        string EditorCSSUrl { get; set;}
        string ControlID { get;set;}
        Control GetEditorControl();
        Unit Width { get;set;}
        Unit Height { get;set;}
        Direction TextDirection { get;set;}
        ToolBar ToolBar { get;set;}
        string SkinName { get;set;}
        bool SetFocusOnStart { get;set;}
        bool FullPageMode { get; set; }
        bool UseFullyQualifiedUrlsForResources { get; set; }
    }
}
