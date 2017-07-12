// Author:					    
// Created:				        2011-03-14
// Last Modified:			    2011-11-29 //Joe Davis
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Ionic.Zip;
using log4net;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Components;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.DesignTools
{
    public partial class DownloadSkin : NonCmsBasePage
    {
        private string skinName = string.Empty;
        private string skinBasePath = string.Empty;
        private static readonly ILog log = LogManager.GetLogger(typeof(DownloadSkin));


        protected void Page_Load(object sender, EventArgs e)
        {
            if ((!WebUser.IsInRoles(siteSettings.RolesThatCanManageSkins)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            if (skinName.Length > 0)
            {
                if(Directory.Exists(Server.MapPath(skinBasePath + skinName)))
                {
                    Download();
                }
                else
                {
                    lblMessage.Text = "Invalid skin name passed in reuest.";
                }
            }
            else
            {
                lblMessage.Text = "No skin name passed in reuest.";
            }

        }

        private void Download()
        {
            if (WebConfigSettings.IncludeContentStylesWithSkinExport)
            {
                try
                {
                    string skinPath = Server.MapPath(skinBasePath + skinName);
                    //get the bytes for the style now because we need to compare it to other styletemplate files if any exist. 
                    Byte[] styleExportBytes = new UTF8Encoding(true).GetBytes(SkinHelper.GetStyleExportString(siteSettings.SiteGuid));

                    string[] styleTemplateFiles = Directory.GetFiles(skinPath, "ContentStyles*.xml", SearchOption.TopDirectoryOnly);
                    
                    bool foundMatch = false;

                    if (styleTemplateFiles.Length > 0)
                    {
                        foreach (string existingStyleFile in styleTemplateFiles)
                        {
                            Byte[] existingStyleFileBytes = File.ReadAllBytes(existingStyleFile);
                            if (existingStyleFileBytes.SequenceEqual(styleExportBytes))
                            {
                                //we found a file which matches the current styles. No need to export them again.
                                foundMatch = true;
                                break;
                            }
                        }
                    }

                    if (!foundMatch)
                    {
                        string fileName = "ContentStyles-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xml";
                        string fullPath = skinPath + "\\" + fileName;

                        if (!File.Exists(fullPath))
                        {
                            using (FileStream fs = File.Create(fullPath))
                            {

                                fs.Write(styleExportBytes, 0, styleExportBytes.Length);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    log.Error("error exporting content styles: " + ex);
                }
            }
            try
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.AddDirectory(Server.MapPath(skinBasePath + skinName));

                    Page.Response.ContentType = "application/zip";
                    Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(skinName + ".zip", Encoding.UTF8) + "\"");

                    //

                    zip.Save(Response.OutputStream);
                    

                }
            }
            catch (ZipException ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private void LoadSettings()
        {
            skinBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/skins/";
            skinName = WebUtils.ParseStringFromQueryString("s", string.Empty);
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