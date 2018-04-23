// Author:					Kerry Doan
// Created:					2011-09-06
// Modified:				2018-03-28
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.IO;
using System.Web.UI.WebControls;
using mojoPortal.Features.Business;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.MediaPlayerUI
{

    public partial class EditMediaPlayerPage : NonCmsBasePage
    {
        
        private int pageId = -1;
        private int moduleId = -1;
        private MediaPlayer thePlayer = new MediaPlayer();

        protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
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

        void AddMediaFileLinkButton_Click(object sender, EventArgs e)
        {
            //Need to perform checks to make sure that multiple files of the same type are not added.
            bool fileTypeExists = false;
            String addingExt = Path.GetExtension(MediaFileTextBox.Text);

            foreach (ListItem item in SelectedFilesListBox.Items)
            {
                String fileExt = Path.GetExtension(item.Text);
                if (addingExt == fileExt)
                {
                    fileTypeExists = true;
                    continue;
                }
            }

            if (!fileTypeExists)
            {
                //A file of this type does not already exist, so add the file.
                SelectedFilesListBox.Items.Add(MediaFileTextBox.Text);
                MediaFileTextBox.Text = string.Empty;
            }
            else
            {
                AddTrackErrorLabel.Text = String.Format(MediaPlayerResources.FileTypeAlreadyExistsErrorText, addingExt);
            }
        }

        void AddTrackButton_Click(object sender, EventArgs e)
        {
            if (SelectedFilesListBox.Items.Count > 0)
            {
                MediaTrack mt = new MediaTrack();

                //Populate the MediaTrack object.
                mt.PlayerId = thePlayer.PlayerId;
                mt.TrackType = thePlayer.PlayerType;
                mt.UserGuid = SiteUtils.GetCurrentSiteUser().UserGuid;
                mt.Name = TrackNameTextBox.Text;
                mt.Artist = ArtistTextBox.Text;
                mt.TrackOrder = TracksGridView.Rows.Count + 1;

                try
                {
                    //Add the MediaTrack
                    mt.TrackId = MediaTrack.Add(mt);

                    //Add the MediaFiles
                    foreach (ListItem filePath in SelectedFilesListBox.Items)
                    {
                        MediaFile mf = new MediaFile();
                        mf.TrackId = mt.TrackId;
                        if (filePath.Text.StartsWith("http"))
                            mf.FilePath = filePath.Text;
                        else
                            mf.FilePath = "~" + filePath.Text;
                        mf.UserGuid = mt.UserGuid;
                        MediaFile.Add(mf);
                    }

                    RedirectToThisPage();
                }
                catch (Exception ex)
                {
                    //Perform backout actions of anything already saved
                    //The Forgien key of the MediaFiles table is set up with a Cascading delete, so
                    //the deleting of the MediaTracks will also delete any related MediaFiles.
                    MediaTrack.RemoveForPlayer(thePlayer.PlayerId);
                    //Display error to user
                    AddTrackErrorLabel.Text = ex.Message;
                }
            }
            else
            {
                AtLeastOneFileSelectedValidator.IsValid = false;
                AddTrackErrorLabel.Text = MediaPlayerResources.AtLeastOneFileSelectedValidatorErrorMessage;
            }
        }

        void CancelButton_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }

        void TracksGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Int32 trackOrder = 0;

            switch (e.CommandName)
            {
                case "MoveUp":
                    trackOrder = Convert.ToInt32(e.CommandArgument);
                    MediaTrack.MoveTrackUp(thePlayer.PlayerId, trackOrder);
                    RedirectToThisPage();
                    break;
                case "MoveDown":
                    trackOrder = Convert.ToInt32(e.CommandArgument);
                    MediaTrack.MoveTrackDown(thePlayer.PlayerId, trackOrder);
                    RedirectToThisPage();
                    break;
                case "EditTrack":
                    EditTrack(e.CommandArgument.ToString());
                    break;
                default:
                    break;
            }
        }

        private void EditTrack(string trackId)
        {
            //Session.Add("KDMediaPlayer_EditTrackID", trackId);

            WebUtils.SetupRedirect(Page, SiteRoot + "/KDMediaPlayer/EditTrack.aspx?mid=" 
                + moduleId.ToInvariantString() 
                + "&pageid=" + pageId.ToInvariantString()
                + "&trackid=" + trackId
                );
        }

        protected void TracksGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Wire up the controls within each TracksGridView row
                Button upButton = (Button)e.Row.FindControl("UpButton");
                upButton.Text = MediaPlayerResources.MoveTrackUpButtonText;
                upButton.ToolTip = MediaPlayerResources.MoveTrackUpOrderTooltip;
                if (e.Row.RowIndex == 0)
                {
                    upButton.Enabled = false;
                }

                Button downButton = (Button)e.Row.FindControl("DownButton");
                downButton.Text = MediaPlayerResources.MoveTrackDownButtonText;
                downButton.ToolTip = MediaPlayerResources.MoveTrackDownOrderTooltip;

                if (e.Row.RowIndex == (thePlayer.MediaTracks.Count - 1))
                {
                    downButton.Enabled = false;
                }

                ListView fileListView = (ListView)e.Row.FindControl("FileListView");
                fileListView.DataSourceID = "FilesObjectDataSource";

                ObjectDataSource filesObjectDataSource = (ObjectDataSource)e.Row.FindControl("FilesObjectDataSource");
                filesObjectDataSource.SelectParameters["trackID"].DefaultValue = ((MediaTrack)e.Row.DataItem).TrackId.ToInvariantString();

                LinkButton editTrackLinkButton = (LinkButton)e.Row.FindControl("EditTrackLinkButton");
                editTrackLinkButton.Text = MediaPlayerResources.EditTrackHyperLinkText;
                editTrackLinkButton.ToolTip = MediaPlayerResources.EditTrackHyperLinkToolTip;
            }
        }
       

      
        private void PopulateControls()
        {
            //The FileBrowserTextBoxExtender control needs the ClientID of the TextBox to work correctly.
            MediaFileBrowser.TextBoxClientId = MediaFileTextBox.ClientID;
            MediaFileBrowser.Text = MediaPlayerResources.MediaFileBrowserLinkText;

            
            if (thePlayer.PlayerType == MediaType.Audio)
            {
                MediaFileRegularExpValidator.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(WebConfigSettings.AudioFileExtensions);
            }
            if (thePlayer.PlayerType == MediaType.Video)
            {
                MediaFileRegularExpValidator.ValidationExpression = SecurityHelper.GetRegexValidationForAllowedExtensions(WebConfigSettings.VideoFileExtensions);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, MediaPlayerResources.EditAudioPlayerPageHeaderText);
            heading.Text = MediaPlayerResources.EditAudioPlayerPageHeaderText;

            MediaTypeValueLabel.Text = thePlayer.PlayerType.ToString();

            TrackNameRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            TrackNameRequiredValidator.ErrorMessage = MediaPlayerResources.TrackNameRequiredValidatorErrorMessage;
            ArtistRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            ArtistRequiredValidator.ErrorMessage = MediaPlayerResources.ArtistRequiredValidatorErrorMessage;

            MediaFileRequiredValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            MediaFileRequiredValidator.ErrorMessage = MediaPlayerResources.MediaFileRequiredValidatorErrorMessage;
            MediaFileRegularExpValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            MediaFileRegularExpValidator.ErrorMessage = MediaPlayerResources.MediaFileRegularExpValidatorErrorMessage;

            AtLeastOneFileSelectedValidator.Text = MediaPlayerResources.ValidationErrorMarking;
            AtLeastOneFileSelectedValidator.ErrorMessage = MediaPlayerResources.AtLeastOneFileSelectedValidatorErrorMessage;

            AddMediaFileLinkButton.Text = MediaPlayerResources.AddMediaFileLinkButtonText;

            AddTrackButton.Text = MediaPlayerResources.AddTrackButtonText;
            CancelButton.Text = MediaPlayerResources.CancelButtonText;

            AddFileValidationSummary.HeaderText = MediaPlayerResources.AddFileValidationSummaryHeaderText;
            AddTrackValidationSummary.HeaderText = MediaPlayerResources.AddTrackValidationSummaryHeaderText;

            AddTrackErrorLabel.Text = String.Empty;
        }

        private void LoadParams()
        {
            //Get the PageId and ModuleId from the QueryString
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

            thePlayer = MediaPlayer.GetForModule(moduleId);

            
            if (thePlayer == null)
            {
                Response.Redirect(SiteUtils.GetCurrentPageUrl() ,true);
                return;
            }

            TracksObjectDataSource.SelectParameters.Add("playerID", TypeCode.Int32, thePlayer.PlayerId.ToInvariantString());

            if (thePlayer.PlayerType == MediaType.Audio)
            {
                MediaFileBrowser.BrowserType = "audio";
            }

            if (thePlayer.PlayerType == MediaType.Video)
            {
                MediaFileBrowser.BrowserType = "video";
            }
        }

        private void LoadSettings()
        {
            
        }

        private void RedirectToThisPage()
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/KDMediaPlayer/Edit.aspx?mid=" + moduleId.ToInvariantString() + "&pageid=" + pageId.ToInvariantString());
        }
        

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            AddMediaFileLinkButton.Click += new EventHandler(AddMediaFileLinkButton_Click);
            AddTrackButton.Click += new EventHandler(AddTrackButton_Click);
            CancelButton.Click += new EventHandler(CancelButton_Click);
            TracksGridView.RowDataBound += new GridViewRowEventHandler(TracksGridView_RowDataBound);
            TracksGridView.RowCommand += new GridViewCommandEventHandler(TracksGridView_RowCommand);

            if (VideoPlayerConfiguration.EditPageSuppressPageMenu) { SuppressPageMenu(); }
            ScriptConfig.IncludeJQTable = true;
        }
        #endregion
    }
}