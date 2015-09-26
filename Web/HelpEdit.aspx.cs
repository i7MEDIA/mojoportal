///	Last Modified:              2010-12-01
///	
using System;
using System.Globalization;
using System.IO;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI.Pages
{
    public partial class HelpEdit : Page
    {
        private String helpKey = String.Empty;
        private SiteSettings siteSettings;
        private bool isSiteEditor = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            if ((!isSiteEditor)&&(!WebUser.IsAdminOrContentAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            siteSettings = CacheHelper.GetCurrentSiteSettings();

            if (Request.Params.Get("helpkey") != null)
            {
                helpKey = Request.Params.Get("helpkey");
            }

            PopulateLabels();

            

            if (!IsPostBack)
            {
                PopulateControls(); 
            }
        }


        protected void PopulateControls()
        {
            if (helpKey != String.Empty)
            {
                edContent.Text = ResourceHelper.GetHelpFileText(helpKey);
            }
        }


        protected void PopulateLabels()
        {
           
            edContent.WebEditor.ToolBar = ToolBar.Full;

            btnSave.Text = Resource.HelpEditSaveButton;
            SiteUtils.SetButtonAccessKey(btnSave, AccessKeys.HelpEditSaveButtonAccessKey);

            lnkCancel.Text = Resource.HelpEditCancelButton;
            
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            String updatedHelp = edContent.Text + "\r\n"; //new line needed to keep files consistent between windows an unix
            ResourceHelper.SetHelpFileText(helpKey, updatedHelp);
            

            WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot() + "/Help.aspx?helpkey=" + helpKey);

            

        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            lnkCancel.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/Help.aspx?helpkey=" + helpKey;
        }


        

        protected override void Render(HtmlTextWriter writer)
        {
            /* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
            * 
            * Custom HtmlTextWriter to fix Form Action
            * Based on Jesse Ezell's "Fixing Microsoft's Bugs: URL Rewriting"
            * http://weblogs.asp.net/jezell/archive/2004/03/15/90045.aspx
            * 
            * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
            // this removes the action attribute from the form
            // so that it posts back correctly when using url re-writing

            string action = Path.GetFileName(Request.RawUrl);
            if (action.IndexOf("?") == -1 && Request.QueryString.Count > 0)
            {
                action += "?" + Request.QueryString.ToString();
            }
            if (writer.GetType() == typeof(HtmlTextWriter))
            {
                writer = new MojoHtmlTextWriter(writer, action);
            }
            else if (writer.GetType() == typeof(Html32TextWriter))
            {
                writer = new MojoHtml32TextWriter(writer, action);
            }

            base.Render(writer);

        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SiteUtils.SetupEditor(edContent, false, this);
        }

    }
}
