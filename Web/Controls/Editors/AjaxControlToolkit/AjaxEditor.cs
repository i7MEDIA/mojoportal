// Author:					
// Created:					2010-08-01
// Last Modified:			2010-08-01
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
using System.Web;
using AjaxControlToolkit.HtmlEditor;
using log4net;

// TODO: might be able to add custom buttons and dialogs
//http://forums.asp.net/p/1432800/3218360.aspx
//http://wiki.asp.net/page.aspx/1366/aspnet-html-editor---upload-images/

namespace mojoPortal.Web.UI
{
    public class AjaxEditor : AjaxControlToolkit.HtmlEditor.Editor
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AjaxEditor));

        protected override void FillTopToolbar()
        {
            // here we could add more buttons
            base.FillTopToolbar();

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Bold());

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Italic());

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Copy());

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Cut());

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Paste());

            //TopToolbar.Buttons.Add(new AjaxControlToolkit.HtmlEditor.ToolbarButton.Redo());

            //AjaxControlToolkit.HtmlEditor.ToolbarButton.MethodButton btn = new AjaxControlToolkit.HTMLEditor.ToolbarButton.MethodButton();

            //btn.NormalSrc = "images/ed_upload_image_n.gif";

            //btn.ID = "btnUplaodImg";

            //btn.Attributes.Add("onclick", "show();");

            ////this method show() is calling from  user control where we add the reference  

            //TopToolbar.Buttons.Add(btn);
            

        }

        //protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        //{
        //    //trying to solve this medium trust issue
        //    //http://ajaxcontroltoolkit.codeplex.com/workitem/26727

        //    ActiveMode = ActiveModeType.Html;

            
           
        //    try
        //    {
        //        return base.LoadPostData(postDataKey, postCollection);
        //    }
        //    catch (System.Security.SecurityException ex)
        //    {
        //        log.Error("swallowed exception in ajaxeditor, must be running in medium trust.", ex);

        //        return false;
        //    }
        //}
    }
}