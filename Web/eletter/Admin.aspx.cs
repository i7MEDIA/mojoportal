/// Author:					
/// Created:				2007-09-23
/// Last Modified:			2018-10-25
///
/// You must not remove this notice, or any other, from this software.

using System;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using Resources;

namespace mojoPortal.Web.ELetterUI
{

	public partial class AdminPage : NonCmsBasePage
    {
        //private int totalPages = 1;
        //private int pageNumber = 1;
        //private int pageSize = 999;
        private bool isSiteEditor = false;
		private SiteUser currentUser;
		private static readonly ILog log = LogManager.GetLogger(typeof(AdminPage));

		protected void Page_Load(object sender, EventArgs e)
        {
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}

			LoadSettings();

            if ((!isSiteEditor) && (!WebUser.IsNewsletterAdmin))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            PopulateLabels();
            PopulateControls();

        }

		private void PopulateControls()
		{
			var letterInfoList = LetterInfo.GetAll(siteSettings.SiteGuid);

			try
			{
				litLetters.Text = RazorBridge.RenderPartialToString("_Admin", letterInfoList, "eletter");
			}
			catch (System.Web.HttpException ex)
			{
				log.ErrorFormat(
					"layout for Newsletter _Admin was not found in skin {0}. perhaps it is in a different skin. Error was: {1}",
					SiteUtils.GetSkinBaseUrl(true, Page),
					ex
				);
			}
		}
        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminMenuNewsletterAdminLabel);

            heading.Text = Resource.AdminMenuNewsletterAdminLabel;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkThisPage.Text = Resource.NewsLetterAdministrationHeading;
            lnkThisPage.NavigateUrl = SiteRoot + "/eletter/Admin.aspx";
        }

        private void LoadSettings()
        {
            //pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            isSiteEditor = SiteUtils.UserIsSiteEditor();

            lnkAdminMenu.Visible = WebUser.IsAdminOrContentAdmin;
            litLinkSeparator1.Visible = lnkAdminMenu.Visible;

			currentUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("administration");
            AddClassToBody("eletteradmin");

        }


        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            SuppressMenuSelection();
            SuppressPageMenu();
            ScriptConfig.IncludeJQTable = true;
            
        }

        #endregion
    }
}
