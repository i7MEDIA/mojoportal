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
    public class MediaPlayer
    {
        private const string audioPlayerFeatureGuid = "92fd0c20-19dc-44c7-a990-0d9e281fc87c";
        private const string videoPlayerFeatureGuid = "30560810-7624-4933-be42-0f95b92f2ff0";

        public static Guid AudioPlayerFeatureGuid
        {
            get { return new Guid(audioPlayerFeatureGuid); }
        }

        public static Guid VideoPlayerFeatureGuid
        {
            get { return new Guid(videoPlayerFeatureGuid); }
        }

        

        ///// <summary>
        ///// Gets the Regular Expression for checking against Audio files.
        ///// </summary>
        //public static string AudioRegularExpression
        //{
        //    get { return "(([^.;]*[.])+(mp3|m4a|oga|webma|webm|wav|fla|MP3|M4A|OGA|WEBMA|WEBM|WAV|FLA); *)*(([^.;]*[.])+(mp3|m4a|oga|webma|webm|wav|fla|MP3|M4A|OGA|WEBMA|WEBM|WAV|FLA))?$"; }
        //}

        ///// <summary>
        ///// Gets the Regular Expression for checking against Video files.
        ///// </summary>
        //public static string VideoRegularExpression
        //{
        //    get { return "(([^.;]*[.])+(m4v|ogv|webmv|webm|flv|M4V|OGV|WEBMV|WEBM|FLV); *)*(([^.;]*[.])+(m4v|ogv|webmv|webm|flv|M4V|OGV|WEBMV|WEBM|FLV))?$"; }
        //}

        #region Properties
        private int playerId = -1;
        /// <summary>
        /// Gets or sets the ID of the Media Player.
        /// </summary>
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        private int moduleId = -1;
        /// <summary>
        /// Gets or set the ID of the Module to which the Media Player belongs.
        /// </summary>
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        private MediaType playerType = MediaType.Unknown;
        /// <summary>
        /// Gets or sets the type of the Media Player.
        /// </summary>
        public MediaType PlayerType
        {
            get { return playerType; }
            set { playerType = value; }
        }

        private String skinName = String.Empty;
        /// <summary>
        /// Gets or sets the name of the skin.
        /// </summary>
        public String SkinName
        {
            get { return skinName; }
            set { skinName = value; }
        }

        private DateTime createdDate = DateTime.UtcNow;
        /// <summary>
        /// Gets or sets the Date that the Media Player was created.
        /// </summary>
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private Guid userGuid = Guid.Empty;
        /// <summary>
        /// Gets or sets the Guid of the user who created the Media Player.
        /// </summary>
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        private Guid moduleGuid = Guid.Empty;
        /// <summary>
        /// Gets or sets the Guid of the Module to which the Media Player belongs.
        /// </summary>
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        private List<MediaTrack> mediaTracks = new List<MediaTrack>();
        /// <summary>
        /// Gets or sets the List of Media Tracks that are part of the Media Player.
        /// </summary>
        public List<MediaTrack> MediaTracks
        {
            get { return mediaTracks; }
            set { mediaTracks = value; }
        }
        #endregion


        /// <summary>
        /// Adds a Media Player.
        /// </summary>
        /// <param name="mediaPlayer">The MediaPlayer.</param>
        /// <returns>The ID of the MediaPlayer.</returns>
        public static int Add(MediaPlayer mediaPlayer)
        {
            //Insert the MediaPlayer.
            int playerId = DBMediaPlayer.Insert(
                mediaPlayer.ModuleId, 
                mediaPlayer.PlayerType.ToString(), 
                mediaPlayer.SkinName,
                mediaPlayer.UserGuid, 
                mediaPlayer.ModuleGuid);

            mediaPlayer.playerId = playerId;

            //Insert each of the MediaPlayer's MediaTracks.
            foreach (MediaTrack track in mediaPlayer.MediaTracks)
            {
                track.PlayerId = playerId;
                MediaTrack.Add(track);
            }

            return playerId;
        }

        public static bool Update(MediaPlayer mediaPlayer)
        {
            //Get the current persisted MediaPlayer
            MediaPlayer currentPlayer = Get(mediaPlayer.PlayerId);

            //Update the MediaPlayer
            bool isUpdated = DBMediaPlayer.Update(
                mediaPlayer.PlayerId, 
                mediaPlayer.ModuleId, 
                mediaPlayer.PlayerType.ToString(),
                mediaPlayer.SkinName, 
                mediaPlayer.UserGuid, 
                mediaPlayer.ModuleGuid);

            //If the MediaPlayer is successfully updated, update the associated MediaTracks.
            if (isUpdated)
            {
                //Remove any MediaTracks that no longer exist.
                foreach (MediaTrack currentTrack in currentPlayer.MediaTracks)
                {
                    bool matchFound = false;
                    foreach (MediaTrack updatedTrack in mediaPlayer.MediaTracks)
                    {
                        if (currentTrack.TrackId == updatedTrack.TrackId)
                        {
                            matchFound = true;
                            break;
                        }
                    }

                    if (!matchFound)
                    {
                        MediaTrack.Remove(currentTrack);
                    }
                }

                //Add any MediaTracks that have not been persisted yet.
                foreach (MediaTrack updatedTrack in mediaPlayer.MediaTracks)
                {
                    if (updatedTrack.TrackId <= 0)
                    {
                        updatedTrack.TrackId = MediaTrack.Add(updatedTrack);
                    }
                }
            }

            return isUpdated;
        }

        /// <summary>
        /// Removes a Media Player.
        /// </summary>
        /// <param name="playerID">The ID of the MediaPlayer.</param>
        /// <returns>True if the remove was a success.</returns>
        public static bool Remove(int playerId)
        {
            return DBMediaPlayer.Delete(playerId);
        }

        /// <summary>
        /// Removes a Media Player for a Module.
        /// </summary>
        /// <param name="playerID">The ID of the Module.</param>
        /// <returns>True if the remove was a success.</returns>
        public static bool RemoveByModule(int moduleId)
        {
            return DBMediaPlayer.DeleteByModule(moduleId);
        }

        public static bool RemoveBySite(int siteId)
        {
            return DBMediaPlayer.DeleteBySite(siteId);
        }


        /// <summary>
        /// Gets a Media Player.
        /// </summary>
        /// <param name="playerID">The ID of the Media Player.</param>
        /// <returns>The Media Player object.</returns>
        public static MediaPlayer Get(int playerId)
        {
            //Get the player for the provided PlayerID.
            List<MediaPlayer> playerList = LoadListFromReader(DBMediaPlayer.Select(playerId));
            

            //Make sure there was data found for the provided PlayerID.
            if (playerList.Count > 0)
            {
                MediaPlayer player = playerList[0];
                //There was data found, so populate the Media Tracks for the Player.
                player.MediaTracks = MediaTrack.GetForPlayer(player.PlayerId);

                return player;
            }
            //else
            //{
            //    //The instance of the player was not located, present an exception.
            //    throw new ArgumentException("The Player ID of '" + playerID.ToString() + "' was not found.");
            //}

            //return thePlayer;

            return null;
        }

        /// <summary>
        /// Gets a Media Player associated to a Module.
        /// </summary>
        /// <param name="moduleID">The ID of the Module.</param>
        /// <returns>The MediaPlayer object.</returns>
        public static MediaPlayer GetForModule(int moduleId)
        {
            //Gets the player for the provided ModuleID
            List<MediaPlayer> playerList = LoadListFromReader(DBMediaPlayer.SelectByModule(moduleId));
            
            //Make sure there was data found for the provided ModuleID
            if (playerList.Count > 0)
            {
                MediaPlayer mediaPlayer = playerList[0];
                //The Player exists, so populate the Media Tracks for the Player.
                mediaPlayer.MediaTracks = MediaTrack.GetForPlayer(mediaPlayer.PlayerId);
                return mediaPlayer;
            }

            return null;
            
        }

        /// <summary>
        /// Creates a list of MediaPlayers from an IDataReader populated with Media Player data.
        /// </summary>
        /// <param name="reader">The IDataReader.</param>
        /// <returns>A List of MediaPlayers.</returns>
        private static List<MediaPlayer> LoadListFromReader(IDataReader reader)
        {
            List<MediaPlayer> playerList = new List<MediaPlayer>();

            try
            {
                while (reader.Read())
                {
                    MediaPlayer player = new MediaPlayer();
                    player.PlayerId = Convert.ToInt32(reader["PlayerID"]);
                    player.ModuleId = Convert.ToInt32(reader["ModuleID"]);
                    player.PlayerType = (MediaType)Enum.Parse(typeof(MediaType), reader["PlayerType"].ToString());
                    player.SkinName = reader["Skin"].ToString();
                    player.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    player.UserGuid = new Guid(reader["UserGuid"].ToString());
                    player.ModuleGuid = new Guid(reader["ModuleGuid"].ToString());
                    playerList.Add(player);
                }
            }
            finally
            {
                reader.Close();
            }

            return playerList;

        }

    }
}
