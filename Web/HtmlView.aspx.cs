/// Author:					        
/// Created:				        12/23/2004
/// Last Modified:			        1/20/2007
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.ContentUI
{
	
    public partial class HtmlView : mojoBasePage
	{
		int moduleID = -1;
		int itemID = -1;

        private HtmlRepository repository = new HtmlRepository();
        

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new System.EventHandler(this.Page_Load);
            base.OnInit(e);
        }
        #endregion

		private void Page_Load(object sender, System.EventArgs e)
		{
            LoadParams();

            if (!UserCanViewPage())
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateControls();

		}

        private void PopulateControls()
        {
            if ((moduleID > -1) && (itemID > -1))
            {
                HtmlContent html = repository.Fetch(moduleID, itemID);
                if (html == null) { return; }

                this.lblTitle.Text = html.Title;
                if (html.Body.Length > 0)
                {
                    Literal literal = new Literal();
                    literal.Text = html.Body;
                    divHtml.Controls.Add(literal);
                }
            }

        }

        private void LoadParams()
        {
            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemID = WebUtils.ParseInt32FromQueryString("ItemID", -1);

            AddClassToBody("htmlview");
        }

		
	}
}
