// Author:					Kerry Doan
// Created:					2011-10-01
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
// Modified 2011-12-04 by , initial integration
// Last Modified 2012-01-13

using System;
using System.IO;
using System.Web.UI.WebControls;
using mojoPortal.Features.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;


namespace mojoPortal.MediaPlayerUI
{

    public partial class EditTrackPage : NonCmsBasePage
    {
        private int pageId = -1;
        private int moduleId = -1;
        private int trackId = -1;
        private MediaPlayer thePlayer = null;
        private MediaTrack theTrack = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            // Make sure the user is allowed to edit the module
            // same edit page used for audio player and video player so need to check both
            if ((!UserCanEditModule(moduleId, MediaPlayer.AudioPlayerFeatureGuid))&&(!UserCanEditModule(moduleId, MediaPlayer.VideoPlayerFeatureGuid)))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        void AddFileButton_Click(object sender, EventArgs e)
        {
            String addingExt = Path.GetExtension(MediaFileTextBox.Text);
            //Determine if a file of this type already exists.
            if (!theTrack.ContainsFileType(addingExt))
            {
                //A file of this type does not already exist, so go ahead and add it.
                MediaFile mf = new MediaFile();
                mf.TrackId = theTrack.TrackId;

                if (MediaFileTextBox.Text.StartsWith("http"))
                {
                    mf.FilePath = MediaFileTextBox.Text;
                }
                else
                {
                    mf.FilePath = "~" + MediaFileTextBox.Text;
                }

                mf.UserGuid = SiteUtils.GetCurrentSiteUser().UserGuid;
                MediaFile.Add(mf);

                RedirectToThisPage();
            }
            else
            {
                AddFileErrorLabel.Text = String.Format(MediaPlayerResources.FileTypeAlreadyExistsErrorText, addingExt);
            }
        }

        void DeleteTrackButton_Click(object sender, EventArgs e)
        {
            //Remove the track data from the database, which also removes the MediaFile data from the data base.
            MediaTrack.Remove(theTrack);

            RedirectToEditPlayerPage();
        }

        void UpdateTrackButton_Click(object sender, EventArgs e)
        {
            theTrack.Name = TrackNameTextBox.Text;
            theTrack.Artist = ArtistTextBox.Text;

            MediaTrack.Update(theTrack);
            RedirectToEditPlayerPage();
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            RedirectToEditPlayerPage();
        }

        void MediaFilesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteFile":
                    MediaFile mf = MediaFile.Get(Convert.ToInt32(e.CommandArgument));
                    if (mf != null)
                    {
                        MediaFile.Remove(mf.FileId);
                    }
                    RedirectToThisPage();
                    break;
                default:
                    break;
            }
        }

        void MediaFilesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteFileLinkButton = (LinkButton)e.Row.FindControl("DeleteFileLinkButton");
                //Only display the Delete link if there is more than 1 file for the track
                if (theTrack.MediaFiles.Count > 1)
                {
                    MediaFile mf = (MediaFile)e.Row.DataItem;
                    deleteFileLinkButton.Text = MediaPlayerResources.DeleteFileLinkButtonText;
                    deleteFileLinkButton.ToolTip = MediaPlayerResources.DeleteFileLinkButtonTooltip;
                    UIHelper.AddConfirmationDialog(deleteFileLinkButton, string.Format(MediaPlayerResources.DeleteFileConfirmationDialogText, mf.FilePath));
                }
                else
                {
                    deleteFileLinkButton.Visible = false;
                }
            }
        }
        

       
        private void PopulateControls()
        {
            if (!IsPostBack)
            {
                TrackNameTextBox.Text = theTrack.Name;
                ArtistTextBox.Text = theTrack.Artist;
            }

            //The FileBrowserTextBoxExtender control needs the ClientID of the TextBox to work correctly.
            MediaFileBrowser.TextBoxClientId = MediaFileTextBox.ClientID;

            if (theTrack.TrackType == MediaType.Audio)
            {
                MediaFileRegularExpValidator.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(WebConfigSettings.JPlayerAudioFileExtensions);
            }
            if (theTrack.TrackType == MediaType.Video)
            {
                MediaFileRegularExpValidator.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(WebConfigSettings.JPlayerVideoFileExtensions);
            }
        }

        private void LoadSettings()
        {
            thePlayer = MediaPlayer.GetForModule(moduleId);
            theTrack = MediaTrack.Get(trackId);

            // need to enforce that the track is connected to the player that is connected to the module instance
            if ((theTrack == null) || (thePlayer == null) || (theTrack.PlayerId != thePlayer.PlayerId))
            {
                // url manipulation hard redirect to current page
                Response.Redirect(SiteUtils.GetCurrentPageUrl(), true);
            }

            FilesObjectDataSource.SelectParameters.Add("trackID", TypeCode.Int32, theTrack.TrackId.ToInvariantString());

            if (thePlayer.PlayerType == MediaType.Audio)
            {
                MediaFileBrowser.BrowserType = "audio";
            }

            if (thePlayer.PlayerType == MediaType.Video)
            {
                MediaFileBrowser.BrowserType = "video";
            }
        }

        private void LoadParams()
        {
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            trackId = WebUtils.ParseInt32FromQueryString("trackid", trackId);

        }

        private void RedirectToEditPlayerPage()
        {
            WebUtils.SetupRedirect(this, SiteRoot 
                + "/KDMediaPlayer/Edit.aspx?mid=" 
                + moduleId.ToInvariantString() 
                + "&pageid=" + pageId.ToInvariantString()
                + "&trackid=" + trackId.ToInvariantString()
                );
        }

        private void RedirectToThisPage()
        {
            WebUtils.SetupRedirect(this, 
                SiteRoot + "/KDMediaPlayer/EditTrack.aspx?mid=" + moduleId.ToInvariantString() 
                + "&pageid=" + pageId.ToInvariantString()
                + "&trackid=" + trackId.ToInvariantString()
                );
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, MediaPlayerResources.EditMediaTrackPageTitleText);
            heading.Text = MediaPlayerResources.EditMediaTrackPageTitleText;

            MediaTypeValueLabel.Text = theTrack.TrackType.ToString();

            TrackNameRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            TrackNameRequiredValidator.ErrorMessage = MediaPlayerResources.TrackNameRequiredValidatorErrorMessage;

            ArtistRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            ArtistRequiredValidator.ErrorMessage = MediaPlayerResources.ArtistRequiredValidatorErrorMessage;

            UpdateTrackButton.Text = MediaPlayerResources.UpdateTrackButtonText;
            UpdateTrackButton.ToolTip = MediaPlayerResources.UpdateTrackButtonTooltip;

            DeleteTrackButton.Text = MediaPlayerResources.DeleteTrackButtonText;
            DeleteTrackButton.ToolTip = MediaPlayerResources.DeleteTrackButtonTooltip;
            UIHelper.AddConfirmationDialog(DeleteTrackButton, MediaPlayerResources.DeleteTrackConfirmationDialogText);

            CancelButton.Text = MediaPlayerResources.CancelButtonText;
            CancelButton.ToolTip = MediaPlayerResources.CancelButtonTooltip;

            UpdateTrackValidationSummary.HeaderText = MediaPlayerResources.UpdateTrackValidationSummaryHeaderText;

            MediaFileBrowser.Text = MediaPlayerResources.MediaFileBrowserLinkText;
            AddMediaFileLinkButton.Text = MediaPlayerResources.AddMediaFileLinkButtonText;

            MediaFileRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            MediaFileRequiredValidator.ErrorMessage = MediaPlayerResources.MediaFileRequiredValidatorErrorMessage;

            MediaFileRegularExpValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            MediaFileRegularExpValidator.ErrorMessage = MediaPlayerResources.MediaFileRegularExpValidatorErrorMessage;

            AddFileErrorLabel.Text = String.Empty;
            AddFileValidationSummary.HeaderText = MediaPlayerResources.AddFileValidationSummaryHeaderText;
        }
      

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            UpdateTrackButton.Click += new EventHandler(UpdateTrackButton_Click);
            DeleteTrackButton.Click += new EventHandler(DeleteTrackButton_Click);
            CancelButton.Click += new EventHandler(CancelButton_Click);
            MediaFilesGridView.RowDataBound += new GridViewRowEventHandler(MediaFilesGridView_RowDataBound);
            MediaFilesGridView.RowCommand += new GridViewCommandEventHandler(MediaFilesGridView_RowCommand);
            AddMediaFileLinkButton.Click += new EventHandler(AddFileButton_Click);

            if (VideoPlayerConfiguration.EditPageSuppressPageMenu) { SuppressPageMenu(); }
            ScriptConfig.IncludeJQTable = true;
        }
        #endregion
    }
}