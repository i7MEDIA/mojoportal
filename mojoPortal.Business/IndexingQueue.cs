// Author:					
// Created:				    2008-06-18
// Last Modified:			2008-12-13
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Data;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents a sequential queue for updating the search index.
    /// Items are queued into the database then processed by a task running on a background thread.
    /// </summary>
    public class IndexingQueue
    {

        #region Constructors

        public IndexingQueue()
        { }


        //public IndexingQueue(
        //    long rowId)
        //{
        //    GetIndexingQueue(
        //        rowId);
        //}

        #endregion

        #region Private Properties

        private int siteId = -1;
        private long rowId;
        private string indexPath = string.Empty;
        private string serializedItem = string.Empty;
        private string itemKey = string.Empty;
        private bool removeOnly;

        #endregion

        #region Public Properties

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        public long RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }
        public string IndexPath
        {
            get { return indexPath; }
            set { indexPath = value; }
        }
        public string SerializedItem
        {
            get { return serializedItem; }
            set { serializedItem = value; }
        }
        public string ItemKey
        {
            get { return itemKey; }
            set { itemKey = value; }
        }
        public bool RemoveOnly
        {
            get { return removeOnly; }
            set { removeOnly = value; }
        }

        #endregion

        #region Private Methods

        ///// <summary>
        ///// Gets an instance of IndexingQueue.
        ///// </summary>
        ///// <param name="rowId"> rowId </param>
        //private void GetIndexingQueue(
        //    long rowId)
        //{
        //    IDataReader reader = DBIndexingQueue.GetOne(
        //        rowId);
        //    PopulateFromReader(reader);

        //}


        //private void PopulateFromReader(IDataReader reader)
        //{
        //    if (reader.Read())
        //    {
        //        this.rowId = Convert.ToInt64(reader["RowId"]);
        //        this.indexPath = reader["IndexPath"].ToString();
        //        this.serializedItem = reader["SerializedItem"].ToString();
        //        this.itemKey = reader["ItemKey"].ToString();
        //        this.removeOnly = Convert.ToBoolean(reader["RemoveOnly"]);

        //    }
        //    reader.Close();
        //}

        /// <summary>
        /// Persists a new instance of IndexingQueue. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            Int64 newID = 0;

            newID = DBIndexingQueue.Create(
                this.siteId,
                this.indexPath,
                this.serializedItem,
                this.itemKey,
                this.removeOnly);

            this.rowId = newID;

            return (newID > 0);

        }


        ///// <summary>
        ///// Updates this instance of IndexingQueue. Returns true on success.
        ///// </summary>
        ///// <returns>bool</returns>
        //private bool Update()
        //{

        //    return DBIndexingQueue.Update(
        //        this.rowId,
        //        this.indexPath,
        //        this.serializedItem,
        //        this.itemKey,
        //        this.removeOnly);

        //}





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of IndexingQueue. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            //if (this.rowId > 0)
            //{
            //    return Update();
            //}
            //else
            //{
                return Create();
            //}
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Gets a count of rows in the mp_IndexingQueue table.
        /// </summary>
        public static int GetCount()
        {
            return DBIndexingQueue.GetCount();

        }

        /// <summary>
        /// Deletes an instance of IndexingQueue. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            long rowId)
        {
            return DBIndexingQueue.Delete(
                rowId);
        }

        public static bool DeleteAll()
        {
            return DBIndexingQueue.DeleteAll();
        }




        /// <summary>
        /// Gets an DataTable with distinct paths.
        /// </summary>
        public static DataTable GetIndexPaths()
        {
            return DBIndexingQueue.GetIndexPaths();
           
        }

        public static DataTable GetSiteIDs()
        {
            return DBIndexingQueue.GetSiteIDs();

        }


        /// <summary>
        /// Gets an DataTable with one row from the mp_IndexingQueue table with the passed path.
        /// </summary>
        /// <param name="indexPath"> indexPath </param>
        public static DataTable GetByPath(string indexPath)
        {
            return DBIndexingQueue.GetByPath(indexPath);

        }

        public static DataTable GetBySite(int siteId)
        {
            return DBIndexingQueue.GetBySite(siteId);

        }

        



        #endregion

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of IndexingQueue.
        /// </summary>
        public static int CompareByIndexPath(IndexingQueue indexingQueue1, IndexingQueue indexingQueue2)
        {
            return indexingQueue1.IndexPath.CompareTo(indexingQueue2.IndexPath);
        }
        /// <summary>
        /// Compares 2 instances of IndexingQueue.
        /// </summary>
        public static int CompareBySerializedItem(IndexingQueue indexingQueue1, IndexingQueue indexingQueue2)
        {
            return indexingQueue1.SerializedItem.CompareTo(indexingQueue2.SerializedItem);
        }
        /// <summary>
        /// Compares 2 instances of IndexingQueue.
        /// </summary>
        public static int CompareByItemKey(IndexingQueue indexingQueue1, IndexingQueue indexingQueue2)
        {
            return indexingQueue1.ItemKey.CompareTo(indexingQueue2.ItemKey);
        }

        #endregion


    }

}
