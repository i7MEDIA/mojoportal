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
// Modified: 2011-12-03 by  initial integration

using System;
using System.Collections.Generic;
using System.Data;
using mojoPortal.MediaPlayer.Data;

namespace mojoPortal.Features.Business
{
    public class MediaTrack
    {
        /// <summary>
        /// The MediaTrack constructor.
        /// </summary>
        public MediaTrack()
        {
        }

        #region Properties
        private int trackId = -1;
        /// <summary>
        /// The ID of the Track.
        /// </summary>
        public int TrackId
        {
            get { return trackId; }
            set { trackId = value; }
        }

        private int playerId = -1;
        /// <summary>
        /// The ID of the Module that the Track belongs to.
        /// </summary>
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        private MediaType trackType = MediaType.Unknown;
        /// <summary>
        /// The Media Type of the Track.
        /// </summary>
        public MediaType TrackType
        {
            get { return trackType; }
            set { trackType = value; }
        }

        private int trackOrder = -1;
        /// <summary>
        /// The order of the Track in the list of tracks for the module.
        /// </summary>
        public int TrackOrder
        {
            get { return trackOrder; }
            set { trackOrder = value; }
        }

        private string name = string.Empty;
        /// <summary>
        /// The Name of the Track.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string artist = string.Empty;
        /// <summary>
        /// The Artist of the Track.
        /// </summary>
        public string Artist
        {
            get { return artist; }
            set { artist = value; }
        }

        private DateTime createdDate = DateTime.UtcNow;
        /// <summary>
        /// The Date that the Track was created.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private Guid userGuid = Guid.Empty;
        /// <summary>
        /// The Guid of the User who added the MediaTrack.
        /// </summary>
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        private List<MediaFile> mediaFiles = new List<MediaFile>();
        /// <summary>
        /// The associated MediaFiles.
        /// </summary>
        public List<MediaFile> MediaFiles
        {
            get { return mediaFiles; }
            set { mediaFiles = value; }
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Determines if the Track already contains a file with an extension.
        /// </summary>
        /// <param name="fileExt">The file extension.</param>
        /// <returns>True if the Track contains a file with the extension.</returns>
        public bool ContainsFileType(String fileExt)
        {
            foreach (MediaFile file in mediaFiles)
            {
                String existingExt = System.IO.Path.GetExtension(file.FilePath);
                if (fileExt == existingExt)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        /// <summary>
        /// Adds a MediaTrack.
        /// </summary>
        /// <param name="mediaTrack">The MediaTrack.</param>
        /// <returns>The ID of the MediaTrack.</returns>
        public static int Add(MediaTrack mediaTrack)
        {
            //Insert the MediaTrack.
            int trackId = DBMediaTrack.Insert(
                mediaTrack.PlayerId, 
                mediaTrack.TrackType.ToString(), 
                mediaTrack.TrackOrder, 
                mediaTrack.Name,
                mediaTrack.Artist, 
                mediaTrack.UserGuid
                );

            mediaTrack.trackId = trackId;

            //Insert each of MediaTrack's MediaFiles.
            foreach (MediaFile file in mediaTrack.MediaFiles)
            {
                file.TrackId = trackId;
                MediaFile.Add(file);
            }

            return trackId;
        }

        /// <summary>
        /// Updates a MediaTrack
        /// </summary>
        /// <param name="mediaTrack">The MediaTrack.</param>
        /// <returns>True if the the update is successful.</returns>
        public static bool Update(MediaTrack mediaTrack)
        {
            //Get the current persisted MediaTrack
            MediaTrack currentTrack = Get(mediaTrack.TrackId);

            //Update the MediaTrack
            bool isUpdated = DBMediaTrack.Update(
                mediaTrack.TrackId, 
                mediaTrack.PlayerId, 
                mediaTrack.TrackType.ToString(), 
                mediaTrack.TrackOrder,
                mediaTrack.Name, 
                mediaTrack.Artist, 
                mediaTrack.UserGuid);

            //If the MediaTrack successfully updated, update the associated MediaFiles.
            if (isUpdated)
            {
                //Remove any MediaFiles that no longer exist.
                foreach (MediaFile currentFile in currentTrack.MediaFiles)
                {
                    bool matchFound = false;
                    foreach (MediaFile updatedFile in mediaTrack.MediaFiles)
                    {
                        if (currentFile.FileId == updatedFile.FileId)
                        {
                            matchFound = true;
                            break;
                        }
                    }

                    if (!matchFound)
                    {
                        MediaFile.Remove(currentFile);
                    }
                }

                //Add any MediaFiles that have not been persisted yet.
                foreach (MediaFile updatedFile in mediaTrack.MediaFiles)
                {
                    if (updatedFile.FileId <= 0)
                    {
                        updatedFile.FileId = MediaFile.Add(updatedFile);
                    }
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// Creates a list of MediaTracks from an IDataReader populated with Media Track data.
        /// </summary>
        /// <param name="reader">The IDataReader.</param>
        /// <returns>A List of MediaTracks.</returns>
        private static List<MediaTrack> LoadListFromReader(IDataReader reader)
        {
            List<MediaTrack> mediaTrackList = new List<MediaTrack>();

            try
            {
                while (reader.Read())
                {
                    MediaTrack mediaTrack = new MediaTrack();
                    mediaTrack.TrackId = Convert.ToInt32(reader["TrackID"]);
                    mediaTrack.PlayerId = Convert.ToInt32(reader["PlayerID"]);
                    mediaTrack.TrackType = (MediaType)Enum.Parse(typeof(MediaType), reader["TrackType"].ToString());
                    mediaTrack.TrackOrder = Convert.ToInt32(reader["TrackOrder"]);
                    mediaTrack.Name = reader["Name"].ToString();
                    mediaTrack.Artist = reader["Artist"].ToString();
                    mediaTrack.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    mediaTrack.UserGuid = new Guid(reader["UserGuid"].ToString());
                    mediaTrackList.Add(mediaTrack);
                }
            }
            finally
            {
                reader.Close();
            }

            return mediaTrackList;

        }

        /// <summary>
        /// Gets a MediaTrack.
        /// </summary>
        /// <param name="trackID">The ID of the MediaTrack.</param>
        /// <returns>The MediaTrack object.</returns>
        public static MediaTrack Get(int trackId)
        {
            //Get the track for the provided trackID.
            List<MediaTrack> trackList = LoadListFromReader(DBMediaTrack.Select(trackId));
            MediaTrack mediaTrack = new MediaTrack();

            //Make sure there was data found
            if (trackList.Count > 0)
            {
                mediaTrack = trackList[0];
                //There was data found, so populate the Media Files for the Track.
                mediaTrack.MediaFiles = MediaFile.GetForTrack(trackId);
                return mediaTrack;
            }
           

            return null;
        }

        /// <summary>
        /// Gets all MediaTracks for a Player.
        /// </summary>
        /// <param name="playerID">The ID of the Player.</param>
        /// <returns>A List of MediaTracks.</returns>
        public static List<MediaTrack> GetForPlayer(int playerId)
        {
            //Get the MediaTracks for the Module.
            List<MediaTrack> mediaTracks = LoadListFromReader(DBMediaTrack.SelectByPlayer(playerId));
            

            // JA Note, in my opinion hitting the database in a loop should be avoided
            // this could/should be changed so that all of the files needed come back from one hit to the db

            //For each MediaTrack, get it's associated MediaFiles.
            //foreach (MediaTrack track in mediaTracks)
            //{
            //    track.MediaFiles = MediaFile.GetForTrack(track.TrackId);
            //}

            // get all the files in one hit to the db
            List<MediaFile> mediaFiles = MediaFile.GetForPlayer(playerId);

            foreach (MediaTrack track in mediaTracks)
            {
                foreach (MediaFile file in mediaFiles)
                {
                    if (file.TrackId == track.trackId)
                    {
                        track.MediaFiles.Add(file);
                    }
                }
            }

            return mediaTracks;
        }

        /// <summary>
        /// Gets the count of MediaTracks for a Player.
        /// </summary>
        /// <param name="moduleID">The ID of the player.</param>
        /// <returns>The count of MediaTracks.</returns>
        public static int GetCountForPlayer(int playerId)
        {
            return DBMediaTrack.GetCountByPlayer(playerId);
        }

        /// <summary>
        /// Moves an AudioTrack up in the play order for the module
        /// </summary>
        /// <param name="playerID">The ModuleID of the Audio Player</param>
        /// <param name="trackOrder">The TrackOrder of the AudioTrack to be moved up</param>
        public static void MoveTrackUp(int playerId, int trackOrder)
        {
            if (trackOrder > 1)
            {
                List<MediaTrack> tracks = GetForPlayer(playerId);

                //Find the MediaTrack to be moved up
                MediaTrack upTrack = new MediaTrack();
                foreach (MediaTrack mt in tracks)
                {
                    if (mt.TrackOrder == trackOrder)
                    {
                        upTrack = mt;
                    }
                }

                //Find the AudioTrack that is to be moved down
                MediaTrack downTrack = new MediaTrack();
                foreach (MediaTrack mt in tracks)
                {
                    if (mt.TrackOrder == upTrack.TrackOrder - 1)
                    {
                        downTrack = mt;
                    }
                }

                //Decrease the value to move the track up in the order, and increase the value to move the track up in the order.
                upTrack.TrackOrder--;
                downTrack.TrackOrder++;

                Update(upTrack);
                Update(downTrack);
            }

        }

            /// <summary>
        /// Moves an AudioTrack up in the play order for the module
        /// </summary>
        /// <param name="playerID">The ModuleID of the Audio Player</param>
        /// <param name="trackOrder">The TrackOrder of the AudioTrack to be moved up</param>
        public static void MoveTrackDown(int playerId, int trackOrder)
        {
            if (trackOrder < GetCountForPlayer(playerId))
            {
                List<MediaTrack> tracks = GetForPlayer(playerId);

                //Find the MediaTrack to be moved up
                MediaTrack downTrack = new MediaTrack();
                foreach (MediaTrack mt in tracks)
                {
                    if (mt.TrackOrder == trackOrder)
                    {
                        downTrack = mt;
                    }
                }

                //Find the MediaTrack that is to be moved down
                MediaTrack upTrack = new MediaTrack();
                foreach (MediaTrack mt in tracks)
                {
                    if (mt.TrackOrder == downTrack.TrackOrder + 1)
                    {
                        upTrack = mt;
                    }
                }

                //Decrease the value to move the track up in the order, and increase the value to move the track up in the order.
                upTrack.TrackOrder--;
                downTrack.TrackOrder++;

                Update(upTrack);
                Update(downTrack);
            }
        
        }

        /// <summary>
        /// Removes a MediaTrack.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>True if the MediaTrack was successfully removed.</returns>
        public static bool Remove(int trackId)
        {
            return Remove(Get(trackId));
        }

        /// <summary>
        /// Removes a MediaTrack
        /// </summary>
        /// <param name="mediaTrack">The MediaTrack.</param>
        /// <returns>True if the MediaTrack was successfully removed.</returns>
        public static bool Remove(MediaTrack mediaTrack)
        {
            //Delete the track from the database.
            bool successfulRemove = DBMediaTrack.Delete(mediaTrack.TrackId);

            //Adjust the TrackOrder values if the track delete was a success.
            if (successfulRemove)
            {
                DBMediaTrack.AdjustTrackOrdersForDelete(mediaTrack.PlayerId, mediaTrack.TrackOrder);
            }

            return successfulRemove;
        }

        /// <summary>
        /// Removes all MediaTracks for a player.
        /// </summary>
        /// <param name="playerID">The ID of the player.</param>
        /// <returns>True if the MediaTracks were successfully removed.</returns>
        public static bool RemoveForPlayer(int playerId)
        {
            return DBMediaTrack.DeleteByPlayer(playerId);
        }

    }
}
