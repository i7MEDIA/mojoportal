// Author:					
// Created:				    2011-03-23
// Last Modified:			2011-03-26
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System.Globalization;
using System.IO;
using System.Xml;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.Features.UI
{
    public class SharedFilesContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            SiteSettings siteSettings = new SiteSettings(module.SiteId);
            SiteUser admin = SiteUser.GetNewestUser(siteSettings);

            string upLoadPath = HostingEnvironment.MapPath("~/Data/Sites/" + module.SiteId.ToInvariantString() + "/SharedFiles/");
            if (!Directory.Exists(upLoadPath))
            {
                Directory.CreateDirectory(upLoadPath);
            }

			FileStream stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
			var xml = Core.Helpers.XmlHelper.GetXmlDocument(stream);

			XmlAttributeCollection attributes = xml.DocumentElement.Attributes;

            if (attributes["filePath"].Value.Length > 0)
            {
                string destPath = "~/Data/Sites/" + module.SiteId.ToInvariantString() + "/SharedFiles/";

                if (!Directory.Exists(HostingEnvironment.MapPath(destPath)))
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath(destPath));
                }

                IOHelper.CopyFolderContents(HostingEnvironment.MapPath(attributes["filePath"].Value), HostingEnvironment.MapPath(destPath));

                destPath = "~/Data/Sites/" + module.SiteId.ToInvariantString() + "/SharedFiles/History/";

                if (!Directory.Exists(HostingEnvironment.MapPath(destPath)))
                {
                    Directory.CreateDirectory(HostingEnvironment.MapPath(destPath));
                }
   
            }

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "file") //root level files
                {
                    CreateFile(module, null, admin, node);
                }
            }

            XmlNode foldersNode = null;

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "folders")
                {
                    foldersNode = node;
                    break;
                }
            }

            if (foldersNode != null)
            {
                foreach (XmlNode folderNode in foldersNode.ChildNodes)
                {
                    if (folderNode.Name == "folder") 
                    {
                        XmlAttributeCollection folderAttributes = folderNode.Attributes;

                        if ((folderAttributes["folderName"] != null)&&(folderAttributes["folderName"].Value.Length > 0))
                        {
                            //create folder
                            SharedFileFolder folder = new SharedFileFolder();
                            folder.ModuleId = module.ModuleId;
                            folder.ModuleGuid = module.ModuleGuid;
                            folder.FolderName = folderAttributes["folderName"].Value;
                            folder.Save();

                            foreach (XmlNode fileNode in folderNode.ChildNodes)
                            {
                                if (fileNode.Name == "file")
                                {
                                    CreateFile(module, folder, admin, fileNode);
                                }
                            }

                        } 

                    }
                }

            }
   

        }

        private void CreateFile(Module module, SharedFileFolder folder, SiteUser siteUser, XmlNode fileNode)
        {
            SharedFile sharedFile = new SharedFile();

            sharedFile.ModuleId = module.ModuleId;
            sharedFile.ModuleGuid = module.ModuleGuid;
            if (folder != null)
            {
                sharedFile.FolderId = folder.FolderId;
                sharedFile.FolderGuid = folder.FolderGuid;
            }

            if (siteUser != null)
            {
                sharedFile.UploadUserId = siteUser.UserId;
                sharedFile.UserGuid = siteUser.UserGuid;
            }

            XmlAttributeCollection fileAttributes = fileNode.Attributes;

            if (fileAttributes["originalFileName"] != null)
            {
                sharedFile.OriginalFileName = fileAttributes["originalFileName"].Value;
            }

            if (fileAttributes["friendlyName"] != null)
            {
                sharedFile.FriendlyName = fileAttributes["friendlyName"].Value;
            }

            if (fileAttributes["serverFileName"] != null)
            {
                sharedFile.ServerFileName = fileAttributes["serverFileName"].Value;
            }

            if (fileAttributes["sortOrder"] != null)
            {
                int size = 0;
                if (int.TryParse(fileAttributes["sortOrder"].Value, out size))
                {
                    sharedFile.SizeInKB = size;
                }
            }
            
            sharedFile.Save();

        }
    }
}