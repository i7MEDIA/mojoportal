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
    public class MediaFile
    {
        /// <summary>
        /// The MediaFile constructor.
        /// </summary>
        public MediaFile()
        {
        }

        #region Properties
        private int fileId = -1;
        /// <summary>
        /// The ID of the File.
        /// </summary>
        public int FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        private int trackId = -1;
        /// <summary>
        /// The ID of the Track that the MediaFile belongs to.
        /// </summary>
        public int TrackId
        {
            get { return trackId; }
            set { trackId = value; }
        }

        private string filePath = string.Empty;
        /// <summary>
        /// The name of the physical Media File.
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        private DateTime addedDate = DateTime.UtcNow;
        /// <summary>
        /// The Date that the File was added.
        /// </summary>
        public DateTime AddedDate
        {
            get { return addedDate; }
            set { addedDate = value; }
        }

        private Guid userGuid = Guid.Empty;
        /// <summary>
        /// The Guid of the User that added the File.
        /// </summary>
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        #endregion


        /// <summary>
        /// Adds a MediaFile.
        /// </summary>
        /// <param name="mediaFile">The MediaFile.</param>
        /// <returns>The ID of the MediaFile.</returns>
        public static int Add(MediaFile mediaFile)
        {
            mediaFile.trackId = DBMediaFile.Insert(mediaFile.TrackId, mediaFile.FilePath, mediaFile.UserGuid);
            return mediaFile.trackId;
        }

        /// <summary>
        /// Creates a List of MediaFile objects from and IDataReader.
        /// </summary>
        /// <param name="reader">The IDataReader that contains the Media File data.</param>
        /// <returns>A List of MediaFile objects.</returns>
        private static List<MediaFile> LoadListFromReader(IDataReader reader)
        {
            List<MediaFile> mediaFileList = new List<MediaFile>();

            try
            {
                while (reader.Read())
                {
                    MediaFile mediaFile = new MediaFile();
                    mediaFile.FileId = Convert.ToInt32(reader["FileID"]);
                    mediaFile.TrackId = Convert.ToInt32(reader["TrackID"]);
                    mediaFile.FilePath = reader["FilePath"].ToString();
                    mediaFile.AddedDate = Convert.ToDateTime(reader["AddedDate"]);
                    mediaFile.UserGuid = new Guid(reader["UserGuid"].ToString());
                    mediaFileList.Add(mediaFile);

                }
            }
            finally
            {
                reader.Close();
            }

            return mediaFileList;
        }

        /// <summary>
        /// Gets a MediaFile.
        /// </summary>
        /// <param name="fileID">the ID of the MediaFile.</param>
        /// <returns>The MediaFile Object.</returns>
        public static MediaFile Get(int fileId)
        {
            List<MediaFile> fileList = LoadListFromReader(DBMediaFile.Select(fileId));

            if (fileList.Count > 0)
            {
                return fileList[0];
            }
            //else
            //{
            //    //The instance of the file was not located, present an exception.
            //    throw new ArgumentException("The File ID of '" + fileID.ToString() + "' was not found.");
            //}
            return null;
        }


        public static List<MediaFile> GetForTrack(int trackId)
        {
            return LoadListFromReader(DBMediaFile.SelectByTrack(trackId));
        }

        public static List<MediaFile> GetForPlayer(int playerId)
        {
            return LoadListFromReader(DBMediaFile.SelectByPlayer(playerId));
        }

        /// <summary>
        /// Gets a count of MediaFiles for a MediaTrack.
        /// </summary>
        /// <param name="trackID">The ID of the Track.</param>
        /// <returns>The count of MediaFiles.</returns>
        public static int GetCountByTrack(int trackId)
        {
            return DBMediaFile.GetCountByTrack(trackId);
        }

        /// <summary>
        /// Removes a MediaFile.
        /// </summary>
        /// <param name="fileID">The ID of the MediaFile.</param>
        /// <returns>True if the MediaFile is successfully removed.</returns>
        public static bool Remove(int fileId)
        {
            return DBMediaFile.Delete(fileId);
        }

        /// <summary>
        /// Removes a MediaFile.
        /// </summary>
        /// <param name="mediaFile">The MediaFile.</param>
        /// <returns>True if the MediaFile is successfully removed.</returns>
        public static bool Remove(MediaFile mediaFile)
        {
            return Remove(mediaFile.FileId);
        }

    }
}
