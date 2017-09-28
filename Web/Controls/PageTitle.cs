/// Author:				    
/// Created:			    2006-09-01
///	Last Modified:		    2011-04-11 by Joe Davis of i7MEDIA
///	                        2013-12-17
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.	

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.UI
{
    /// <summary>
    /// This control is used in layout.master to show the page name, title override or page heading in the page.
    /// </summary>
    public class PageTitle : WebControl
    {
        public Literal Title = new Literal();

        private string text = string.Empty;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

		private string literalExtraMarkup = string.Empty;
		public string LiteralExtraMarkup
		{
			get => literalExtraMarkup;
			set => literalExtraMarkup = value;
		}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // only show page title on pages where CmsPage is used or pageid is passed explicitely in the url 
            // (it can be not passed in on the home page so if it is CmsPage render anyway)
            int pageId = -1;
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            if ((pageId != -1) || ((Page is CmsPage)))
            {
                PageSettings currentPage = CacheHelper.GetCurrentPage();
                if (currentPage == null) { return; }
                if (!currentPage.ShowPageHeading) { return; }

                if(currentPage.PageHeading.Length > 0)
                {
                    Title.Text = HttpUtility.HtmlAttributeEncode(currentPage.PageHeading);
                }
                else if (currentPage.PageTitle.Length > 0)
                {
                    Title.Text = HttpUtility.HtmlAttributeEncode(currentPage.PageTitle);
                }
                else
                {
                    Title.Text = HttpUtility.HtmlAttributeEncode(currentPage.PageName);
                }
            }

        }

        

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
               
                writer.Write("[" + this.ID + "]");
            }
            else
            {
                if (Title.Text.Length == 0) { Title.Text = text; }
                if (Title.Text.Length == 0) { return; }
				Title.Text += LiteralExtraMarkup;
                Title.RenderControl(writer);
            }
        }

        
    }

    /// <summary>
    /// the purpose of this control is to find the PageTitle control if it exists and set the text property on it.
    /// this is only needed in supporting pages, ie pages that do not inherit from CmsPage
    /// </summary>
    public class PageTitleHelper : WebControl
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Control c = Page.Master.FindControl("PageTitle1");
            if (c != null)
            {
                PageTitle t = c as PageTitle;
                if (t != null)
                {
                    PageSettings currentPage = CacheHelper.GetCurrentPage();
                    if (currentPage == null) { return; }
                    if (currentPage.PageTitle.Length > 0)
                    {
                        t.Text = Context.Server.HtmlEncode(currentPage.PageTitle);
                    }
                    else
                    {
                        t.Text = Context.Server.HtmlEncode(currentPage.PageName);
                    }

                }

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //base.Render(writer);
            //don't render anything
        }

    }

}
