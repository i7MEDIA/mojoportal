// Author:						Kevin Needham
// Created:					    2009-06-23
// Last Modified:               
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 


using System;
using System.Web;

namespace mojoPortal.Web.AdminUI
{
    /// <summary>
    /// This Handler enables you to take control of screen navigation once a page is saved. 
    /// To do this, create  your own page in your own Visual Studio project
    /// and transfer into the admin folder via a post build event.  
    /// Leaving the code as it is, the navigation will just redirect back to the PageSettings page, but you could 
    /// actually direct the user to anywhere you like.
    /// </summary>    
    public class PageSettingsSaved : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {            
            string pageId = context.Request.QueryString["pageId"];
            string pageIsNewlyCreated = context.Request.QueryString["newlycreated"];

            if (!string.IsNullOrEmpty(pageIsNewlyCreated) && Convert.ToBoolean(pageIsNewlyCreated))
            {  
                context.Response.Redirect(String.Format("PageSettings.aspx?pageid={0}", pageId));
            }
            else
            {
                context.Response.Redirect(String.Format("PageSettings.aspx?pageid={0}", pageId));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
