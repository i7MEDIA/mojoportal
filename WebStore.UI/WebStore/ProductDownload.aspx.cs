/// Author:					Joe Audette
/// Created:				2007-02-28
/// Last Modified:			2013-08-08
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using WebStore.Business;

namespace WebStore.UI
{

    public partial class ProductDownloadPage : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductDownloadPage));
        protected Guid ticketGuid = Guid.Empty;
        private Guid productGuid = Guid.Empty;
        
        protected FullfillDownloadTicket downloadTicket;
        private SiteUser currentUser = null;
        
        protected string upLoadPath;
        private IFileSystem fileSystem = null;
        private int pageId = -1;
        private int moduleId = -1;
        private bool userCanEdit = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableDownloadCache();

            if (!Request.IsAuthenticated)
            {
                WebUtils.SetupRedirect(this, SiteRoot);
                return;
            }

            LoadSettings();
           
            if (
                (fileSystem != null)
                &&(downloadTicket != null)
                && (downloadTicket.Guid == ticketGuid)
                && (!downloadTicket.IsExpired())
                )
            {

                DownloadFile();
            }
            else
            {
                if ((pageId != -1) && (productGuid != Guid.Empty) && userCanEdit)
                {
                    // this supports admin downloads from the product edit page
                    DownloadFile();
                }
                else
                {
                    LogRejection();

                    pnlExpiredDownload.Visible = true;
                }
            }


        }

        private void DownloadFile()
        {

            ProductFile productFile = null;
            if (downloadTicket != null)
            {
                productFile = new ProductFile(downloadTicket.ProductGuid);
            }
            else if (userCanEdit && (productGuid != Guid.Empty))
            {
                productFile = new ProductFile(productGuid);
            }


            if (productFile == null) { return; }

            string fileType = Path.GetExtension(productFile.FileName).Replace(".", string.Empty).ToLowerInvariant();
            string mimeType = SiteUtils.GetMimeType(fileType);
            Page.Response.ContentType = mimeType;

            if (WebConfigSettings.DownloadScriptTimeout > -1)
            {
                Server.ScriptTimeout = WebConfigSettings.DownloadScriptTimeout;
            }

            if (SiteUtils.IsNonAttacmentFileType(fileType))
            {
                Page.Response.AddHeader("Content-Disposition", "filename=" + productFile.FileName);
            }
            else
            {
                Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(productFile.FileName, Encoding.UTF8) + "\"");
            }

            //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());
                                                                                                                                            
            Page.Response.ContentType = "application/" + fileType;
            Page.Response.Buffer = false;
            Page.Response.BufferOutput = false;
            //Page.Response.TransmitFile(upLoadPath + productFile.ServerFileName);

            if (Page.Response.IsClientConnected)
            {
                using (System.IO.Stream stream = fileSystem.GetAsStream(upLoadPath + productFile.ServerFileName))
                {
                    stream.CopyTo(Page.Response.OutputStream);
                }

                if (downloadTicket != null)
                {
                    downloadTicket.RecordDownloadHistory(SiteUtils.GetIP4Address());
                }
            }
            
            
            try
            {
                Page.Response.End();
            }
            catch (System.Threading.ThreadAbortException) { }

        }

        private void LogRejection()
        {
            if (currentUser == null) { return; }
            
            log.Info("User " + currentUser.Email + " tried to download a product but it was either invalid or expired.");

            if (downloadTicket == null) { return; }

            log.Info("rejected download ticket Guid was " + downloadTicket.Guid.ToString());

        }

       

        private void LoadSettings()
        {


            ticketGuid = WebUtils.ParseGuidFromQueryString("ticket", ticketGuid);
            productGuid = WebUtils.ParseGuidFromQueryString("prod", productGuid);

            // these are only used for edit users downloads
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", true, -1);

            if (pageId > -1)
            {
                userCanEdit = UserCanEditModule(moduleId, Store.FeatureGuid);
            }

            currentUser = SiteUtils.GetCurrentSiteUser();

            if (ticketGuid != Guid.Empty)
            {
                downloadTicket = new FullfillDownloadTicket(ticketGuid);
                if (downloadTicket.UserGuid != currentUser.UserGuid)
                {
                    downloadTicket = null;
                    log.Info("downloadTicket UserGuid did not match the current user so not allowing the download.");
                }
            }

            
            upLoadPath = "~/Data/Sites/" + siteSettings.SiteId.ToString() + "/webstoreproductfiles/";

            

            

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p == null)
            {
                log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

            fileSystem = p.GetFileSystem();
            if (fileSystem == null)
            {
                log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                return;
            }

        }


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            SuppressPageMenu();
        }

        #endregion
    }
}
