// Author:					
// Created:					2010-12-10
// Last Modified:			2012-11-29 //Joe Davis, i7MEDIA
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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Ionic.Zip;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;

namespace mojoPortal.Web.Components
{
    public class SkinHelper
    {
        public SkinHelper()
        {

            debug = WebConfigSettings.DebugSkinImporter;
            allowedExtensions = StringHelper.SplitOnPipes(WebConfigSettings.AllowedSkinFileExtensions);
        }

        private static readonly ILog log = LogManager.GetLogger(typeof(SkinHelper));
        private bool debug = false;

        private List<string> allowedExtensions = new List<string>();

        public void InstallSkins(string siteSkinFolderPath, string zipFilePath, bool overwriteFiles)
        {
            if (string.IsNullOrEmpty(siteSkinFolderPath)) { return; }
            if (string.IsNullOrEmpty(zipFilePath)) { return; }

            if (debug) { log.Info("skin path for skin is " + siteSkinFolderPath); }

            List<string> basePathsToRemove = null;

            using (ZipFile zip = ZipFile.Read(zipFilePath))
            {
                basePathsToRemove = GetBasePathsToRemove(zip);
            }

            if ((basePathsToRemove != null) && (basePathsToRemove.Count > 0))
            {
                foreach (string basePath in basePathsToRemove)
                {
                    using (ZipFile zip = ZipFile.Read(zipFilePath))
                    {
                        RebaseFiles(zip, basePath);
                    }

                }
            }

            using (ZipFile zip = ZipFile.Read(zipFilePath))
            {
                if (IsSingleSkinInRoot(zip))
                {
                    ExtractSingleSkin(siteSkinFolderPath, zip, overwriteFiles);
                }
                else if (ContainsSkins(zip))
                {
                    if (debug) { log.Info("Not a single skin zip file"); }

                    ExtractMultipleSkins(siteSkinFolderPath, zip, overwriteFiles);
                }
                else
                {
                    log.Info("No skins found in file " + Path.GetFileName(zip.Name));
                }


            }

        }

        private void ExtractMultipleSkins(string siteSkinFolderPath, ZipFile zip, bool overwriteFiles)
        {
            ExtractExistingFileAction fileAction = ExtractExistingFileAction.DoNotOverwrite;
            if (overwriteFiles) { fileAction = ExtractExistingFileAction.OverwriteSilently; }

            foreach (ZipEntry e in zip)
            {
                if (e.IsDirectory)
                {
                    if (debug) { log.Info("processing folder " + e.FileName); }
                    e.Extract(siteSkinFolderPath, fileAction);
                }
                else
                {
                    if (IsAllowedExtension(Path.GetExtension(e.FileName)))
                    {
                        if (debug) { log.Info("extracting file " + e.FileName); }

                        e.Extract(siteSkinFolderPath, fileAction);

                        if (e.FileName.StartsWith("ContentStyles") && e.FileName.EndsWith(".xml"))
                        {
                            if (debug) { log.Info("importing content styles from " + e.FileName); }
                            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                            ImportStyles(e.InputStream, siteSettings.SiteGuid);
                        }
                    }
                    else
                    {
                        if (debug) { log.Info("skipping file, extension not allowed " + e.FileName); }
                    }

                }

            }
        }



        private void ExtractSingleSkin(string siteSkinFolderPath, ZipFile zip, bool overwriteFiles)
        {
            ExtractExistingFileAction fileAction = ExtractExistingFileAction.DoNotOverwrite;
            if (overwriteFiles) { fileAction = ExtractExistingFileAction.OverwriteSilently; }

            string destinationPath = Path.Combine(siteSkinFolderPath, CleanSkinFolderName(Path.GetFileNameWithoutExtension(zip.Name)));
            if (debug) { log.Info("destination path for skin is " + destinationPath); }

            foreach (ZipEntry e in zip)
            {

                if (e.IsDirectory)
                {
                    if (debug) { log.Info("processing folder " + e.FileName); }
                    e.Extract(destinationPath, fileAction);
                }
                else
                {
                    if (IsAllowedExtension(Path.GetExtension(e.FileName)))
                    {
                        if (debug) { log.Info("extracting file " + e.FileName); }

                        e.Extract(destinationPath, fileAction);

                        if (e.FileName.StartsWith("ContentStyles") && e.FileName.EndsWith(".xml"))
                        {
                            if (debug) { log.Info("importing content styles from " + e.FileName); }
                            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                            ImportStyles(e.InputStream, siteSettings.SiteGuid);
                        }
                    }
                    else
                    {
                        if (debug) { log.Info("skipping file, extension not allowed " + e.FileName); }
                    }

                }
            }
        }

        private bool IsAllowedExtension(string extension)
        {
            foreach (string ext in allowedExtensions)
            {
                if (string.Equals(extension, ext, StringComparison.InvariantCultureIgnoreCase)) { return true; }
            }

            return false;
        }

        private List<string> GetBasePathsToRemove(ZipFile zip)
        {
            // innitialize the result
            List<string> basePathsToRemove = new List<string>();

            //wherever we find a layout.master file it is in a root of a skin folder
            //so by getting a list of them we have a list of skns and the folder structure leading to it
            List<string> masterPagePaths = new List<string>();

            foreach (ZipEntry e in zip)
            {
                if (!e.IsDirectory)
                {
                    if (e.FileName.EndsWith("layout.Master", StringComparison.InvariantCultureIgnoreCase))
                    {
                        masterPagePaths.Add(e.FileName);
                    }
                }
            }

            //loop through the master page files looking for extra folder segments in the path
            // example of what we're looking for: extra-skins/artisteer-30alphamotors/layout.Master
            foreach (string s in masterPagePaths)
            {
                if (s.LastIndexOf("/") > s.IndexOf("/")) //we know there is more than one forward slash
                {
                    string chopped = s.Substring(0, s.LastIndexOf("/"));
                    if (debug) { log.Info("chopped is " + chopped); }

                    if (chopped.Length > chopped.LastIndexOf("/") + 1)
                    {
                        string basePath = chopped.Substring(0, chopped.LastIndexOf("/") + 1);

                        if (debug) { log.Info("basePath is " + basePath); }

                        if (!basePathsToRemove.Contains(basePath))
                        {
                            basePathsToRemove.Add(basePath);
                        }
                    }

                }
            }

            return basePathsToRemove;
        }


        /// <summary>
        /// this is used to rebase the paths in the .zip file if the skin folders are not directly in the root of the zip
        /// for example the extra-skins.zip we ship is structured like this: extra-skins/artisteer-30alphamotors/ 
        /// so skin folders are inside the extra-skins folder and we want to extract them directly without including the extra-skins folder
        /// so we have to remove that base folder by renaming the files and removing that part.
        /// </summary>
        /// <param name="zip"></param>
        /// <param name="baseToRemove"></param>
        private void RebaseFiles(ZipFile zip, string baseToRemove)
        {
            if (string.IsNullOrEmpty(baseToRemove)) { return; }

            // we cannot edit the file names while enumerating them so we first get a list of filenames
            List<string> fileNames = new List<string>();
            foreach (ZipEntry e in zip)
            {
                fileNames.Add(e.FileName);
            }

            foreach (string s in fileNames)
            {
                ZipEntry e = zip[s];
                if (e != null)
                {
                    if ((e.FileName.StartsWith(baseToRemove)) && (e.FileName.Length > baseToRemove.Length))
                    {
                        e.FileName = e.FileName.Replace(baseToRemove, string.Empty);
                    }
                }
            }

            // get rid of the outer folder
            zip.RemoveEntry(baseToRemove);

            zip.Save();

        }

        private string CleanSkinFolderName(string skinName)
        {
            //ensure valid folder name

            return skinName.Replace(".zip", string.Empty).ToCleanFolderName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
        }


        private bool IsSingleSkinInRoot(ZipFile zip)
        {
            if (
                ((zip.ContainsEntry("layout.Master")) || (zip.ContainsEntry("layout.master")))
                && (zip.ContainsEntry("theme.skin"))
                )
            {
                // these files exist in the root folder of the zip
                return true;
            }

            return false;
        }

        private bool ContainsSkins(ZipFile zip)
        {
            bool foundMasterPage = false;
            bool foundTheme = false;
            foreach (ZipEntry e in zip)
            {
                if (!e.IsDirectory)
                {
                    if (e.FileName.EndsWith("layout.Master", StringComparison.InvariantCultureIgnoreCase)) { foundMasterPage = true; }
                    if (e.FileName.EndsWith("theme.skin", StringComparison.InvariantCultureIgnoreCase)) { foundTheme = true; }

                }
            }

            return foundMasterPage && foundTheme;
        }


        #region Static Methods

        public static void CopySkin(string sourceFolderPath, string destinationFolderPath)
        {
            if (Directory.Exists(sourceFolderPath))
            {
                if (!Directory.Exists(destinationFolderPath)) { Directory.CreateDirectory(destinationFolderPath); }

                DirectoryInfo sourceRoot = new DirectoryInfo(sourceFolderPath);
                DirectoryInfo dirDestination = new DirectoryInfo(destinationFolderPath);
                FileInfo[] theFiles = sourceRoot.GetFiles();

                foreach (FileInfo f in theFiles)
                {
                    try
                    {
                        File.Copy(
                            f.FullName,
                            dirDestination.FullName + Path.DirectorySeparatorChar + f.Name,
                            true);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (System.IO.IOException) { }
                    //catch (System.IO.DirectoryNotFoundException) { }

                }

                DirectoryInfo[] subDirectories = sourceRoot.GetDirectories();

                foreach (DirectoryInfo d in subDirectories)
                {
                    try
                    {
                        dirDestination.CreateSubdirectory(d.Name);
                        theFiles = d.GetFiles();
                        foreach (FileInfo f in theFiles)
                        {
                            try
                            {
                                File.Copy(
                                    f.FullName,
                                    dirDestination.FullName + Path.DirectorySeparatorChar
                                    + d.Name + Path.DirectorySeparatorChar + f.Name, true);
                            }
                            catch (UnauthorizedAccessException) { }
                            catch (System.IO.IOException) { }
                            //catch (System.IO.DirectoryNotFoundException) { }

                        }

                    }
                    catch (System.Security.SecurityException ex)
                    {
                        log.Error("error trying to copy skins into site skins folder ", ex);
                    }
                }




            }

        }

        public static FileInfo[] GetCssFileList(string skinFolderPath, bool recursive = false)
        {

            DirectoryInfo dir = new DirectoryInfo(skinFolderPath);
            return dir.Exists ? dir.GetFiles("*.css", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly) : null;
        }

        public static string GetStyleExportString(Guid siteGuid)
        {
            List<ContentStyle> styles = ContentStyle.GetAll(siteGuid);
            XDocument stylesXml = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("ContentStyles",
                    from style in styles
                    select new XElement("Style",
                        new XAttribute("Name", style.Name),
                        new XAttribute("Element", style.Element),
                        new XAttribute("CssClass", style.CssClass),
                        new XAttribute("IsActive", style.IsActive)
                    )
                    )
                );

            return stylesXml.ToString();
        }

        public static void ImportStyles(Stream stylesXmlStream, Guid siteGuid)
        {
            try
            {
                var stylesXmlDoc = Core.Helpers.XmlHelper.GetXmlDocument(stylesXmlStream);

                if (stylesXmlDoc.DocumentElement.Name != "ContentStyles") { return; }
                foreach (XmlNode node in stylesXmlDoc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "Style")
                    {
                        ContentStyle style = ContentStyle.GetNew(siteGuid);
                        SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                        if (currentUser != null) { style.LastModBy = currentUser.UserGuid; }
                        style.Name = node.Attributes["Name"].Value;
                        style.Element = node.Attributes["Element"].Value;
                        style.CssClass = node.Attributes["CssClass"].Value;
                        style.IsActive = Convert.ToBoolean(node.Attributes["IsActive"].Value);
                        style.Save();
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error(ex);
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }
        }


        #endregion
    }
}
