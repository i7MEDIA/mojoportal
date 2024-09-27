// Author:					Kerry Doan
// Created:					2011-09-06

using System;
using System.IO;
using System.Web.UI.WebControls;
using mojoPortal.Features.Business;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.MediaPlayerUI;

public partial class EditMediaPlayerPage : NonCmsBasePage
{
	private int pageId = -1;
	private int moduleId = -1;
	private MediaPlayer thePlayer = new();

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
		if (!UserCanEditModule(moduleId, MediaPlayer.AudioPlayerFeatureGuid)
			&& !UserCanEditModule(moduleId, MediaPlayer.VideoPlayerFeatureGuid))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		PopulateLabels();
		PopulateControls();
	}


	void AddMediaFileLinkButton_Click(object sender, EventArgs e)
	{
		//Need to perform checks to make sure that multiple files of the same type are not added.
		bool fileTypeExists = false;
		string addingExt = Path.GetExtension(MediaFileTextBox.Text);

		foreach (ListItem item in SelectedFilesListBox.Items)
		{
			string fileExt = Path.GetExtension(item.Text);
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
			AddTrackErrorLabel.Text = string.Format(MediaPlayerResources.FileTypeAlreadyExistsErrorText, addingExt);
		}
	}


	void AddTrackButton_Click(object sender, EventArgs e)
	{
		if (SelectedFilesListBox.Items.Count > 0)
		{
			//Populate the MediaTrack object.
			var mt = new MediaTrack
			{
				PlayerId = thePlayer.PlayerId,
				TrackType = thePlayer.PlayerType,
				UserGuid = SiteUtils.GetCurrentSiteUser().UserGuid,
				Name = TrackNameTextBox.Text,
				Artist = ArtistTextBox.Text,
				TrackOrder = TracksGridView.Rows.Count + 1
			};

			try
			{
				//Add the MediaTrack
				mt.TrackId = MediaTrack.Add(mt);

				//Add the MediaFiles
				foreach (ListItem filePath in SelectedFilesListBox.Items)
				{
					var mf = new MediaFile();
					mf.TrackId = mt.TrackId;

					if (filePath.Text.StartsWith("http"))
					{
						mf.FilePath = filePath.Text;
					}
					else
					{
						mf.FilePath = "~" + filePath.Text;
					}

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


	void CancelButton_Click(object sender, EventArgs e) => WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());


	void TracksGridView_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		var arg = Convert.ToInt32(e.CommandArgument);

		switch (e.CommandName)
		{
			case "MoveUp":
				MediaTrack.MoveTrackUp(thePlayer.PlayerId, arg);
				RedirectToThisPage();
				break;
			case "MoveDown":
				MediaTrack.MoveTrackDown(thePlayer.PlayerId, arg);
				RedirectToThisPage();
				break;
			case "EditTrack":
				EditTrack(arg.ToString());
				break;
			default:
				break;
		}
	}


	private void EditTrack(string trackId) => WebUtils.SetupRedirect(
		Page, "KDMediaPlayer/EditTrack.aspx".ToLinkBuilder()
									  .PageId(pageId)
									  .ModuleId(moduleId)
									  .AddParam("trackid", trackId)
									  .ToString());


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

		AddTrackErrorLabel.Text = string.Empty;
	}


	private void LoadParams()
	{
		//Get the PageId and ModuleId from the QueryString
		pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
		moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);

		thePlayer = MediaPlayer.GetForModule(moduleId);

		if (thePlayer is null)
		{
			Response.Redirect(SiteUtils.GetCurrentPageUrl(), true);
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


	private void RedirectToThisPage() => WebUtils.SetupRedirect(
		this, "KDMediaPlayer/Edit.aspx".ToLinkBuilder()
						   .PageId(pageId)
						   .ModuleId(moduleId)
						   .ToString());


	#region OnInit

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
		AddMediaFileLinkButton.Click += new EventHandler(AddMediaFileLinkButton_Click);
		AddTrackButton.Click += new EventHandler(AddTrackButton_Click);
		CancelButton.Click += new EventHandler(CancelButton_Click);
		TracksGridView.RowDataBound += new GridViewRowEventHandler(TracksGridView_RowDataBound);
		TracksGridView.RowCommand += new GridViewCommandEventHandler(TracksGridView_RowCommand);

		if (VideoPlayerConfiguration.EditPageSuppressPageMenu)
		{
			SuppressPageMenu();
		}

		ScriptConfig.IncludeJQTable = true;
	}
	#endregion
}