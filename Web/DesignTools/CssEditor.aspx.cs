// Author:					
// Created:					2011-03-21
// Last Modified:			2018-05-02
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Globalization;
using System.IO;
using System.Text;
using mojoPortal.Web.Framework;
using mojoPortal.Business.WebHelpers;
using Resources;


namespace mojoPortal.Web.AdminUI
{
	public partial class CssEditorPage : NonCmsBasePage
	{
		protected string skinName = string.Empty;
		protected string cssFile = string.Empty;
		private string skinBasePath = string.Empty;
		protected string chosenSkinPath = string.Empty;
		private bool allowEditing = false;


		protected void Page_Load(object sender, EventArgs e)
		{
			if ((!WebUser.IsSkinManager))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			LoadSettings();

			if (!allowEditing)
			{
				WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/SkinList.aspx");
				return;
			}

			if (!Directory.Exists(Server.MapPath(skinBasePath + skinName)))
			{   //skin doesn't exist go back to skin list
				WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/SkinList.aspx");
				return;
			}

			if (!File.Exists(Server.MapPath(skinBasePath + skinName + "/" + cssFile)))
			{   //css file doesn't exist go back to skin manager
				WebUtils.SetupRedirect(this, SiteRoot + "/DesignTools/ManageSkin.aspx?s=" + skinName);
				return;
			}

			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			if (!IsPostBack) 
			{ 
				BindCss(); 
			}
		}


		private void BindCss()
		{
			edCss.Text = File.ReadAllText(Server.MapPath(skinBasePath + skinName + "/" + cssFile), Encoding.UTF8); //should this be ascii?
		}


		void btnSave_Click(object sender, EventArgs e)
		{
			using (StreamWriter writer = File.CreateText(Server.MapPath(skinBasePath + skinName + "/" + cssFile)))
			{
				writer.Write(edCss.Text);
			}

			WebUtils.SetupRedirect(this, Request.RawUrl);
		}


		private void PopulateLabels()
		{
			string title = string.Format(CultureInfo.InvariantCulture, Resource.ManageSkinFormat, skinName);
			Title = SiteUtils.FormatPageTitle(siteSettings, title);

			lnkAdminMenu.Text = Resource.AdminMenuLink;
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkDesignerTools.Text = DevTools.DesignTools;
			lnkDesignerTools.NavigateUrl = SiteRoot + "/DesignTools/Default.aspx";

			lnkSkinList.Text = DevTools.SkinManagement;
			lnkSkinList.NavigateUrl = SiteRoot + "/DesignTools/SkinList.aspx";

			lnkSkin.Text = skinName;
			lnkSkin.NavigateUrl = SiteRoot + "/DesignTools/ManageSkin.aspx?s=" + skinName;

			lnkThisPage.Text = cssFile;
			lnkThisPage.NavigateUrl = Request.RawUrl;

			btnSave.Text = Resource.SaveButton;
		}


		private void LoadSettings()
		{
			skinBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/";
			skinName = WebUtils.ParseStringFromQueryString("s", string.Empty);
			cssFile = WebUtils.ParseStringFromQueryString("f", string.Empty);
			chosenSkinPath = skinBasePath + skinName + "/";
			
			allowEditing = cssFile.EndsWith(".css") && IOHelper.IsDecendentDirectory(skinBasePath, chosenSkinPath) && IOHelper.IsDecendentFile(chosenSkinPath, chosenSkinPath + cssFile) && WebConfigSettings.AllowEditingSkins && (WebConfigSettings.AllowEditingSkinsInChildSites || siteSettings.IsServerAdminSite);

			if (WebConfigSettings.DisableEditAreaForCssEditor) { edCss.Disable = true; }
			if (BrowserHelper.IsIE9())
			{
				// currently has major problems in IE 9
				// it does weird selection of all text
				edCss.Disable = true;
			}

			AddClassToBody("administration");
			AddClassToBody("designtools");
		}

  
		#region OnInit

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			btnSave.Click += new EventHandler(btnSave_Click);

			SuppressMenuSelection();
			SuppressPageMenu();
		}

		#endregion
	}
}
