// Author:					Kerry Doan
// Created:					2011-09-14
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified 2011-12-04 by , initial integration
// Modified 2011-12-05 by 
// separated Audio and Video Player into 2 features eleiminated the Setup.aspx page that set the player type
// 2012-07-05
// 2012-10-17 change throw from Exception to ArgumentException and handle the error so it doesn't crash the page



using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.UI;
using log4net;
using mojoPortal.Business;
using mojoPortal.Features.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.MediaPlayerUI
{
    public partial class AudioPlayer : SiteModuleControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AudioPlayer));
        private MediaPlayer thePlayer = null;
        private AudioPlayerConfiguration config = new AudioPlayerConfiguration();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            
            LoadSettings();
            PopulateLabels();

            if (thePlayer.MediaTracks.Count == 0)
            {
                SetupNeededLabel.Visible = true;
                PlayerContainer.Visible = false;
                return;
            }

            PopulateControls();

            try
            {
                SetupScripts();
            }
            catch (ArgumentException ex)
            {
                log.Error(ex);
            }


        }

        /// <summary>
        /// Builds the script of the jPlayer constructor.
        /// </summary>
        private void SetupScripts()
        {
            if (!(Page is mojoBasePage)) { return; }

            // include the main scripts
            mojoBasePage basePage = Page as mojoBasePage;
            basePage.ScriptConfig.IncludejPlayer = true;
            basePage.ScriptConfig.IncludejPlayerPlaylist = true;

            // setup the instance script
            StringBuilder script = new StringBuilder();
            script.Append("\n<script type=\"text/javascript\">\n");

            script.Append("(function() {");
            script.Append("var pl_" + this.ClientID + " = new jPlayerPlaylist({");
            script.Append("jPlayer: \"#" + PlayerInstance.ClientID + "\",");
            script.Append("cssSelectorAncestor: \"#" + PlayerContainer.ClientID + "\"");
            script.Append("}");

            //Start the construction of the playlist
            script.Append(",[");
            bool isFirstTrack = true;
            //Keep a list of the file types that were added for the track to use to create the 
            //"supplied" jPlayer constructor option
            List<string> suppliedTypes = new List<string>();
            foreach (MediaTrack track in thePlayer.MediaTracks)
            {
                //Gets the URL to the folder where the Media Files for the track exist (removing the ~ fromt the begining of
                //the returned path).
                //Use to be used when the MediaFile.FilePath was storing just the file name rather than the whole path.
                //string fileBasePath = SiteRoot + Utility.GetMediaFilePath(SiteId, thePlayer.PlayerID, track.TrackID).Remove(0, 1) + "/";

                //Get the file information for the track
                List<MediaFile> mediaFiles = track.MediaFiles;

                if (isFirstTrack)
                {
                    script.Append("{");
                    isFirstTrack = false;
                }
                else
                {
                    script.Append(",{");
                }

                script.Append("title:\"" + track.Name + "\",");
                script.Append("artist:\"" + track.Artist + "\",");

                //Add the proper property for each file depending upon it's file extension.
                bool isFirstFile = true;
                foreach (MediaFile file in mediaFiles)
                {
                    string filePath = file.FilePath;
                    if (!filePath.StartsWith("~/Data"))
                    {
                        // fix for this
                        //https://www.mojoportal.com/Forums/Thread.aspx?thread=10596&mid=34&pageid=5&ItemID=2&pagenumber=1#post44051
                        if (filePath.StartsWith("~"))
                        {
                            filePath = filePath.Substring(1);
                        }
                    }

                    string fullFilePath = Page.ResolveUrl(filePath);
  
                    string fileExt = Path.GetExtension(file.FilePath).ToLowerInvariant();

                    if (isFirstFile)
                    {
                        isFirstFile = false;
                    }
                    else
                    {
                        script.Append(",");
                    }

                    if (thePlayer.PlayerType == MediaType.Audio)
                    {
                        switch (fileExt)
                        {
                            case ".mp3":
                                script.Append("mp3:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("mp3"))
                                    suppliedTypes.Add("mp3");
                                break;
                            case ".m4a":
                                script.Append("m4a:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("m4a"))
                                    suppliedTypes.Add("m4a");
                                break;
                            case ".mp4":
                                script.Append("m4a:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("m4a"))
                                    suppliedTypes.Add("m4a");
                                break;
                            case ".webma":
                                script.Append("webma:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("webma"))
                                    suppliedTypes.Add("webma");
                                break;
                            case ".webm":
                                script.Append("webma:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("webma"))
                                    suppliedTypes.Add("webma");
                                break;
                            case ".oga":
                                script.Append("oga:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("oga"))
                                    suppliedTypes.Add("oga");
                                break;
                            case ".ogg":
                                script.Append("oga:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("oga"))
                                    suppliedTypes.Add("oga");
                                break;
                            //case ".fla":
                            //    script.Append("fla:\"" + fullFilePath + "\"");
                            //    if (!suppliedTypes.Contains("fla"))
                            //        suppliedTypes.Add("fla");
                            //    break;
                            case ".wav":
                                script.Append("wav:\"" + fullFilePath + "\"");
                                if (!suppliedTypes.Contains("wav"))
                                    suppliedTypes.Add("wav");
                                break;
                            default:
                                throw new ArgumentException("No Supported Audio File Extension Found");
                        }
                    }
                    
                }
                script.Append("}");
            }
            script.Append("]");
            //End of playlist

            script.Append(",{");
            script.Append("playlistOptions: {");
            script.Append("loopOnPrevious: true");
            if (config.AutoStart)
            {
                script.Append(",autoPlay: true");
            }

            script.Append("},");
            //script.Append("swfPath: \"" + Page.ResolveUrl(WebConfigSettings.JPlayerBasePath + "Jplayer.swf") + "\"");
            script.Append("supplied: \"");

            bool isFirstSupplied = true;
            foreach (string type in suppliedTypes)
            {
                if (isFirstSupplied)
                {
                    isFirstSupplied = false;
                }
                else
                {
                    script.Append(", ");
                }

                script.Append(type);
            }

            script.Append("\",preload: \"auto\"");
            script.Append(",wmode: \"window\"");
            if (config.ContinuousPlay)
            {
                script.Append(",loop: true");
            }
            script.Append("});");


            script.Append("})();");

            script.Append("\n</script>");

            this.Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                this.UniqueID,
                script.ToString());
        }

        /// <summary>
        /// Populates controls with data.
        /// </summary>
        private void PopulateControls()
        {
            TitleControl.Visible = !this.RenderInWebPartMode;
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }

            if (config.HeaderContent.Length > 0)
            {
                litUpperContent.Text = "<div class='mpltop'>" + config.HeaderContent + "</div>";
            }

            if (config.FooterContent.Length > 0)
            {
                litLowerContent.Text = "<div class='mplbottom'>" + config.FooterContent + "</div>";
            }
        }

        

        /// <summary>
        /// Loads needed settings from their repository.
        /// </summary>
        private void LoadSettings()
        {
            //Load the player.
            thePlayer = MediaPlayer.GetForModule(ModuleId);
            if (thePlayer == null)
            {
                thePlayer = new MediaPlayer();
                thePlayer.ModuleGuid = ModuleGuid;
                thePlayer.ModuleId = ModuleId;
                thePlayer.PlayerType = MediaType.Audio;
                if (IsEditable)
                {
                    SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                    if (currentUser != null)
                    {
                        thePlayer.UserGuid = currentUser.UserGuid;
                        MediaPlayer.Add(thePlayer);
                    } 
                } 
            }

            config = new AudioPlayerConfiguration(Settings);

            if (config.DisableShuffle)
            {
                ShuffleControl.Visible = false;
                ShuffleOffControl.Visible = false;
            }

            if (config.InstanceCssClass.Length > 0)
            {
                pnlOuterWrap.SetOrAppendCss(config.InstanceCssClass);
            }

        }

        /// <summary>
        /// Populates labels with their appropriate text.
        /// </summary>
        private void PopulateLabels()
        {
            TitleControl.EditUrl = SiteRoot + "/KDMediaPlayer/Edit.aspx";
            TitleControl.EditText = MediaPlayerResources.EditAudioPlayerLinkText;

            PreviousLink.InnerText = MediaPlayerResources.PreviousText;
            PlayLink.InnerText = MediaPlayerResources.PlayText;
            PauseLink.InnerText = MediaPlayerResources.PauseText;
            NextLink.InnerText = MediaPlayerResources.NextText;
            StopLink.InnerText = MediaPlayerResources.StopText;
            MuteLink.Title = MediaPlayerResources.MuteText;
            MuteLink.InnerText = MediaPlayerResources.MuteText;
            UnmuteLink.InnerText = MediaPlayerResources.UnmuteText;
            UnmuteLink.Title = MediaPlayerResources.UnmuteText;
            MaxVolumeLink.InnerText = MediaPlayerResources.MaxVolumeText;
            MaxVolumeLink.Title = MediaPlayerResources.MaxVolumeText;

            //Player toggle buttons
            ShuffleLink.InnerText = MediaPlayerResources.ShuffleText;
            ShuffleLink.Title = MediaPlayerResources.ShuffleText;
            ShuffleOffLink.InnerText = MediaPlayerResources.ShuffleOffText;
            ShuffleOffLink.Title = MediaPlayerResources.ShuffleOffText;
            RepeatLink.InnerText = MediaPlayerResources.RepeatText;
            RepeatLink.Title = MediaPlayerResources.RepeatText;
            RepeatOffLink.InnerText = MediaPlayerResources.RepeatOffText;
            RepeatOffLink.Title = MediaPlayerResources.RepeatOffText;

            //No Solution Statement
            NoSolutionLiteral.Text = MediaPlayerResources.NoSolutionMessageMarkup;
        }



        


        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        #endregion

    }
}