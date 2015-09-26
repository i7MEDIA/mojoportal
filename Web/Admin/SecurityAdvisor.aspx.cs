// Author:					Joe Audette
// Created:					2010-09-19
// Last Modified:			2011-11-20
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
using System.Text;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using Resources;

namespace mojoPortal.Web.AdminUI
{

    public partial class SecurityAdvisorPage : NonCmsBasePage
    {
        SecurityAdvisor securityAdvisor = new SecurityAdvisor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (!siteSettings.IsServerAdminSite)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }


            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            PopulateLabels();
            PopulateControls();

        }

        private void PopulateControls()
        {
            
            if (securityAdvisor.UsingCustomMachineKey())
            {
                lblMachineKeyGood.Visible = true;
                imgMachineKeyOk.Visible = true;
                lblMachineKeyBad.Visible = false;
                imgMachineKeyDanger.Visible = false;
                lnkMachineKeyRefresh.Visible = false;
                txtRandomMachineKey.Visible = false;
                lblMachineKeyInstructions.Visible = false;
            }
            else
            {
                lblMachineKeyGood.Visible = false;
                imgMachineKeyOk.Visible = false;
                lblMachineKeyBad.Visible = true;
                imgMachineKeyDanger.Visible = true;
                lnkMachineKeyRefresh.Visible = true;
                lblMachineKeyInstructions.Visible = true;

                txtRandomMachineKey.Text = SiteUtils.GenerateRandomMachineKey();
            }

            if(WebUtils.ParseBoolFromQueryString("fc", false))
            {
                List<string> writableFolders = securityAdvisor.GetWritableFolders();

                if (writableFolders.Count > 0)
                {
                    imgFileSystemOk.Visible = false;
                    lblFileSystemOk.Visible = false;
                    imgFileSystemWarning.Visible = true;
                    lblFileSystemWarning.Visible = true;
                    lnkFileSystemHelp.Visible = true;

                    StringBuilder list = new StringBuilder();
                    list.Append("<ul class='simplelist writablefolders'>");

                    foreach (string f in writableFolders)
                    {
                        list.Append("<li>" + f + "</li>");
                    }

                    list.Append("</ul>");
                    litWritableFolderList.Text = list.ToString();

                }
                else
                {
                    imgFileSystemOk.Visible = true;
                    lblFileSystemOk.Visible = true;
                    imgFileSystemWarning.Visible = false;
                    lblFileSystemWarning.Visible = false;
                    lnkFileSystemHelp.Visible = false;
                }
            }

        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, Resource.SecurityAdvisor);

            heading.Text = Resource.SecurityAdvisor;
            litInfo.Text = Resource.SecurityAdvisorInfo;

            lnkAdminMenu.Text = Resource.AdminMenuLink;
            lnkAdminMenu.ToolTip = Resource.AdminMenuLink;
            lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

            lnkThisPage.Text = Resource.SecurityAdvisor;
            lnkThisPage.ToolTip = Resource.SecurityAdvisor;
            lnkThisPage.NavigateUrl = SiteRoot + "/Admin/SecurityAdvisor.aspx";

            imgMachineKeyOk.ImageUrl = "~/Data/SiteImages/accept.png";
            imgMachineKeyOk.AlternateText = Resource.OKLabel;

            imgMachineKeyDanger.ImageUrl = "~/Data/SiteImages/warning.png";
            imgMachineKeyDanger.AlternateText = Resource.SecurityDangerLabel;
            lnkMachineKeyRefresh.Text = Resource.GenerateMachineKey;
            lnkMachineKeyRefresh.NavigateUrl = Request.RawUrl;

            imgFileSystemOk.ImageUrl = "~/Data/SiteImages/accept.png";
            imgFileSystemOk.AlternateText = Resource.OKLabel;

            lnkCheckFolders.Text = Resource.CheckIfTooManyWritableFolders;
            lnkCheckFolders.NavigateUrl = SiteRoot + "/Admin/SecurityAdvisor.aspx?fc=true";

            imgFileSystemWarning.ImageUrl = "~/Data/SiteImages/warning-yellow.png";
            imgFileSystemWarning.AlternateText = Resource.SecurityDangerLabel;

            lnkFileSystemHelp.Text = Resource.InformationToSolveProblem;
            lnkFileSystemHelp.NavigateUrl = "http://www.mojoportal.com/securing-the-file-system.aspx";

            

        }

        private void LoadSettings()
        {
            AddClassToBody("administration");
            AddClassToBody("securityadvisor");

        }

        

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SuppressMenuSelection();
            SuppressPageMenu();
        }

        #endregion
    }
}