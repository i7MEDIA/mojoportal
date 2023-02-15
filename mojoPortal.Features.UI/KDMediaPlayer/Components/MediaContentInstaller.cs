// Author:					
// Created:				    2011-12-06
// Modified:                2012-10-31
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
using System.Xml;
using System.Text;
using System.Web.Hosting;
using mojoPortal.Business;
using mojoPortal.Features.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;

namespace mojoPortal.MediaPlayerUI
{
    public class MediaContentInstaller : IContentInstaller
    {
        public void InstallContent(Module module, string configInfo)
        {
            if (string.IsNullOrEmpty(configInfo)) { return; }

            SiteSettings siteSettings = new SiteSettings(module.SiteId);
            SiteUser admin = SiteUser.GetNewestUser(siteSettings);

			FileStream stream = File.OpenRead(HostingEnvironment.MapPath(configInfo));
			var xml = Core.Helpers.XmlHelper.GetXmlDocument(stream);

            MediaPlayer player = new MediaPlayer
            {
                ModuleGuid = module.ModuleGuid,
                ModuleId = module.ModuleId,
                PlayerType = MediaType.Audio
            };

            if ((xml.DocumentElement.Attributes["type"] != null) && (xml.DocumentElement.Attributes["type"].Value.Length > 0))
            {
                player.PlayerType = (MediaType)Enum.Parse(typeof(MediaType), xml.DocumentElement.Attributes["type"].Value);
            }

            MediaPlayer.Add(player);

            XmlNode tracksNode = null;
            foreach (XmlNode n in xml.DocumentElement.ChildNodes)
            {
                if (n.Name == "tracks")
                {
                    tracksNode = n;
                    break;
                }
            }

            if (tracksNode == null) { return; }

            foreach (XmlNode node in xml.DocumentElement.ChildNodes)
            {
                if (node.Name == "moduleSetting")
                {
                    XmlAttributeCollection settingAttributes = node.Attributes;

                    if ((settingAttributes["settingKey"] != null) && (settingAttributes["settingKey"].Value.Length > 0))
                    {
                        string key = settingAttributes["settingKey"].Value;
                        string val = string.Empty;
                        if (settingAttributes["settingValue"] != null)
                        {
                            val = settingAttributes["settingValue"].Value;
                            if ((key == "HeaderContent") || (key == "FooterContent"))
                            {
                                val = System.Web.HttpUtility.HtmlDecode(val);
                            }
                        }

                        ModuleSettings.UpdateModuleSetting(module.ModuleGuid, module.ModuleId, key, val);
                    }
                }
            }


            foreach (XmlNode trackNode in tracksNode.ChildNodes)
            {
                if (trackNode.Name == "track")
                {
                    XmlAttributeCollection trackAttrributes = trackNode.Attributes;

                    MediaTrack track = new MediaTrack();
                    track.PlayerId = player.PlayerId;
                    track.TrackType = player.PlayerType;

                    if ((trackAttrributes["name"] != null) && (trackAttrributes["name"].Value.Length > 0))
                    {
                        track.Name = trackAttrributes["name"].Value;
                    }

                    if ((trackAttrributes["artist"] != null) && (trackAttrributes["artist"].Value.Length > 0))
                    {
                        track.Artist = trackAttrributes["artist"].Value;
                    }

                    MediaTrack.Add(track);

                    XmlNode filesNode = null;
                    foreach (XmlNode n in trackNode.ChildNodes)
                    {
                        if (n.Name == "files")
                        {
                            filesNode = n;
                            break;
                        }
                    }

                    if (filesNode == null) { return; }

                    foreach (XmlNode fileNode in filesNode.ChildNodes)
                    {
                        if (fileNode.Name == "file")
                        {
                            MediaFile mediaFile = new MediaFile();
                            mediaFile.TrackId = track.TrackId;

                            XmlAttributeCollection fileAttrributes = fileNode.Attributes;

                            string targetFormat = string.Empty;
                            string sourcePath = string.Empty;

                            if ((fileAttrributes["targetPathFormat"] != null) && (fileAttrributes["targetPathFormat"].Value.Length > 0))
                            {
                                targetFormat = fileAttrributes["targetPathFormat"].Value;
                            }

                            if ((fileAttrributes["sourcePath"] != null) && (fileAttrributes["sourcePath"].Value.Length > 0))
                            {
                                sourcePath = HostingEnvironment.MapPath(fileAttrributes["sourcePath"].Value);
                            }

                            
                            if (targetFormat != string.Empty)
                            {
                                mediaFile.FilePath = string.Format(CultureInfo.InvariantCulture, targetFormat, module.SiteId.ToInvariantString());

                                string directory = Path.GetDirectoryName(HostingEnvironment.MapPath(mediaFile.FilePath));
                                if (!Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }

                                File.Copy(sourcePath, HostingEnvironment.MapPath(mediaFile.FilePath));

                                MediaFile.Add(mediaFile);
                            }
                            else //targetPathFormat is not defined so we don't need to move the file
                            {
                                mediaFile.FilePath = fileAttrributes["sourcePath"].Value;
                                MediaFile.Add(mediaFile);
                            }

                        }
                    }
                }
            }

        }

    }
}