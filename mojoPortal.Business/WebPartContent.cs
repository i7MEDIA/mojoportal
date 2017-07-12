// Author:					
// Created:				    2006-06-03
// Last Modified:			2009-02-01
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Represents webparts used on content pages without personalization state,because content pages have no webpart manager.
    /// </summary>
    public class WebPartContent
    {
        #region Constructors

	    public WebPartContent()
		{}


        public WebPartContent(Guid webPartId) 
		{
	        GetWebPart(webPartId); 
	    }

        #endregion

        #region Private Properties

		private Guid webPartID = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
		private int siteID = -1; 
		private string title = string.Empty; 
		private string description = string.Empty; 
		private string imageUrl = string.Empty; 
		private string className = string.Empty; 
		private string assemblyName = string.Empty; 
		private bool availableForMyPage; 
		private bool allowMultipleInstancesOnMyPage; 
		private bool availableForContentSystem; 
		
        #endregion

        #region Public Properties

		public Guid WebPartId 
		{
			get { return webPartID; }
			set { webPartID = value; }
		}

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

		public int SiteId 
		{
			get { return siteID; }
			set { siteID = value; }
		}
		public string Title 
		{
			get { return title; }
			set { title = value; }
		}
		public string Description 
		{
			get { return description; }
			set { description = value; }
		}
		public string ImageUrl 
		{
			get { return imageUrl; }
			set { imageUrl = value; }
		}
		public string ClassName 
		{
			get { return className; }
			set { className = value; }
		}
		public string AssemblyName 
		{
			get { return assemblyName; }
			set { assemblyName = value; }
		}
		public bool AvailableForMyPage 
		{
			get { return availableForMyPage; }
			set { availableForMyPage = value; }
		}
		public bool AllowMultipleInstancesOnMyPage 
		{
			get { return allowMultipleInstancesOnMyPage; }
			set { allowMultipleInstancesOnMyPage = value; }
		}
		public bool AvailableForContentSystem 
		{
			get { return availableForContentSystem; }
			set { availableForContentSystem = value; }
		}

        #endregion

        #region Private Methods

		private void GetWebPart(Guid webPartId) 
		{
            using (IDataReader reader = DBWebPartContent.GetWebPart(webPartId))
            {
                if (reader.Read())
                {
                    this.webPartID = new Guid(reader["WebPartID"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    this.imageUrl = reader["ImageUrl"].ToString();
                    this.className = reader["ClassName"].ToString();
                    this.assemblyName = reader["AssemblyName"].ToString();
                    this.availableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
                    this.allowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
                    this.availableForContentSystem = Convert.ToBoolean(reader["AvailableForContentSystem"]);

                }

            }
		
		}
		
		private bool Create()
		{
            Guid newID = Guid.NewGuid();

            this.webPartID = newID;

            int rowsAffected = DBWebPartContent.AddWebPart(
                this.webPartID,
                this.siteGuid,
                this.siteID,
                this.title,
                this.description,
                this.imageUrl,
                this.className,
                this.assemblyName,
                this.availableForMyPage,
                this.allowMultipleInstancesOnMyPage,
                this.availableForContentSystem);

            return (rowsAffected > 0);

		}

		

		private bool Update()
		{

			return DBWebPartContent.UpdateWebPart(
				this.webPartID, 
				this.siteID, 
				this.title, 
				this.description, 
				this.imageUrl, 
				this.className, 
				this.assemblyName, 
				this.availableForMyPage, 
				this.allowMultipleInstancesOnMyPage, 
				this.availableForContentSystem); 
				
		}


        #endregion

        #region Public Methods

        public bool Save()
        {
            if (this.webPartID != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

		
		
        #endregion


        #region Static Methods

        public static DataTable SelectPage(
            int siteId,
            int pageNumber,
            int pageSize,
            bool sortByClassName,
            bool sortByAssemblyName)
        {

            return DBWebPartContent.SelectPage(
                siteId,
                pageNumber,
                pageSize,
                sortByClassName,
                sortByAssemblyName);

        }

        public static bool Exists(Int32 siteId, String className, String assemblyName)
        {
            return DBWebPartContent.Exists(siteId, className, assemblyName);

        }


        

        public static IDataReader SelectBySite(int siteId)
        {
            return DBWebPartContent.SelectBySite(siteId);

        }

        public static IDataReader GetWebPartsForMyPage(int siteId)
        {
            return DBWebPartContent.GetWebPartsForMyPage(siteId);

        }

        public static IDataReader GetMostPopular(int siteId, int numberToGet)
        {
            return DBWebPartContent.GetMostPopular(siteId, numberToGet);
        }

        public static int Count(int siteId)
        {
            return DBWebPartContent.Count(siteId);
        }

        public static bool DeleteWebPart(Guid webPartId)
        {
            return DBWebPartContent.DeleteWebPart(webPartId);

        }

        public static bool UpdateCountOfUseOnMyPage(Guid webPartId, int increment)
        {
            return DBWebPartContent.UpdateCountOfUseOnMyPage(webPartId, increment);
        }
       



        #endregion



    }
}
