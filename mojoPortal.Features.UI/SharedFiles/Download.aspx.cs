/// Author:					    Joe Audette
/// Created:				    2007-01-28
/// Last Modified:			    2013-09-06
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
using System.Globalization;
using System.Text;
using System.Web;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.FileSystem;
using mojoPortal.Web.Framework;
using log4net;


namespace mojoPortal.Web.SharedFilesUI
{

    public partial class SharedFilesDownload : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SharedFilesDownload));

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            
            base.OnInit(e);

            if (WebConfigSettings.DownloadScriptTimeout > -1)
            {
                Server.ScriptTimeout = WebConfigSettings.DownloadScriptTimeout;
            }
        }
        #endregion

        private IFileSystem fileSystem = null;
        private int pageID = -1;
        private int moduleID = -1;
        private int fileID = -1;
        private SharedFile sharedFile = null;
        


        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableDownloadCache();
            

            if (
                (LoadAndCheckParams())
                && (UserCanViewPage(moduleID, SharedFile.FeatureGuid))
                )
            {
                DownloadFile();
            }
            else
            {
                if (!Request.IsAuthenticated)
                {
                    SiteUtils.RedirectToLoginPage(this);
                    return;
                }
                else
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

            }

        }

        private void DownloadFile()
        {
            if (
                (CurrentPage != null)
                &&(sharedFile != null)
                )
            {
                string virtualPath =  "~/Data/Sites/" + this.CurrentPage.SiteId.ToInvariantString() + "/SharedFiles/"
                    + sharedFile.ServerFileName;

                if (fileSystem.FileExists(virtualPath))
                {
                    //FileInfo fileInfo = new System.IO.FileInfo(downloadPath);
                    WebFile fileInfo = fileSystem.RetrieveFile(virtualPath);
                    
                    Page.Response.AppendHeader("Content-Length", fileInfo.Size.ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    log.Error("Shared File Not Found. User tried to download file " + virtualPath);
                    return;
                }

                string fileType = Path.GetExtension(sharedFile.FriendlyName).Replace(".", string.Empty);

                string mimeType = SiteUtils.GetMimeType(fileType);
                //Page.Response.ContentType = mimeType;
                Page.Response.ContentType = "application/" + fileType;

                if ((!SharedFilesConfiguration.TreatPdfAsAttachment) && (SiteUtils.IsNonAttacmentFileType(fileType)))
                {
                    //this will display the pdf right in the browser
                    // and the file may be cached by the web browser
                    Page.Response.AddHeader("Content-Disposition", "filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName, Encoding.UTF8) + "\"");
                    if (SharedFilesConfiguration.NonAttachmentDownloadExpireDays != 0)
                    {
                        Page.Response.AddHeader("Expires", DateTime.Now.AddDays(SharedFilesConfiguration.NonAttachmentDownloadExpireDays).ToUniversalTime().ToString("R")); 

                    }
                }
                else
                {
                    // other files just use file save dialog
                    Page.Response.AddHeader("Content-Disposition", "attachment; filename=\"" + HttpUtility.UrlEncode(sharedFile.FriendlyName, Encoding.UTF8) + "\"");

                    // 0 is the default so we should not be settings a cache header here
                    // attachments are not stored in the web browser cache 
                    if (SharedFilesConfiguration.AttachmentDownloadExpireDays != 0) 
                    {
                        Page.Response.AddHeader("Expires", DateTime.Now.AddDays(SharedFilesConfiguration.AttachmentDownloadExpireDays).ToUniversalTime().ToString("R"));

                    }
                }

                //Page.Response.AddHeader("Content-Length", documentFile.DocumentImage.LongLength.ToString());

                try
                {
                    Page.Response.Buffer = false;
                    Page.Response.BufferOutput = false;
                    if (Page.Response.IsClientConnected)
                    {
                        
                        //Page.Response.TransmitFile(downloadPath);
                        using (System.IO.Stream stream = fileSystem.GetAsStream(virtualPath))
                        {
                            stream.CopyTo(Page.Response.OutputStream);
                            SharedFile.IncrementDownloadCount(sharedFile.ItemId);
                        }
                        try
                        {
                            Page.Response.End();
                        }
                        catch (System.Threading.ThreadAbortException) { }
                        
                        
                    }
                   
                }
                catch (HttpException) { }
               
            }

        }

       

        private bool LoadAndCheckParams()
        {
            bool result = true;

            pageID = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleID = WebUtils.ParseInt32FromQueryString("mid", -1);
            fileID = WebUtils.ParseInt32FromQueryString("fileid", -1);
            if (pageID == -1) result = false;
            if (moduleID == -1) result = false;
            if (fileID == -1) result = false;

            if (result)
            {
                sharedFile = new SharedFile(moduleID, fileID);
                result = (sharedFile.ModuleId == moduleID);

                FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                if (p == null)
                {
                    log.Error("Could not load file system provider " + WebConfigSettings.FileSystemProvider);
                    result = false;
                }

                fileSystem = p.GetFileSystem();
                if (fileSystem == null)
                {
                    log.Error("Could not load file system from provider " + WebConfigSettings.FileSystemProvider);
                    result = false;
                }

            }


            return result;

        }

        
    }
}
