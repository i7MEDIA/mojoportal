// Author:					
// Created:				    2007-09-01
// Last Modified:			2009-07-19
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software. 

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// This class represents an instance of a ContentModule published on
    /// a page. It is abridge between page class and module class and corresponds
    /// to the mp_PageModules table
    /// </summary>
    public class PageModule
    {
        private int moduleID = -1;
        private int pageID = -1;
        private int moduleOrder = 999;
        private string paneName = String.Empty;
        private DateTime publishBeginDate = DateTime.MinValue;
        private DateTime publishEndDate = DateTime.MaxValue;
        private string moduleTitle = string.Empty;
        private string pageName = string.Empty;
        private string pageUrl = string.Empty;

        public int ModuleId
        {
            get { return moduleID; }
        }

        public int PageId
        {
            get { return pageID; }
        }

        public int ModuleOrder
        {
            get { return moduleOrder; }
        }
        public string PaneName
        {
            get { return paneName; }
        }

        public DateTime PublishBeginDate
        {
            get { return publishBeginDate; }

        }

        public DateTime PublishEndDate
        {
            get { return publishEndDate; }

        }

        public string ModuleTitle
        {
            get { return moduleTitle; }
        }

        public string PageName
        {
            get { return pageName; }
        }

        public string PageUrl
        {
            get { return pageUrl; }
        }

        private static List<PageModule> LoadListFromReader(IDataReader reader)
        {
            List<PageModule> pageModules = new List<PageModule>();

            while (reader.Read())
            {
                PageModule pageModule = new PageModule();
                pageModule.moduleID = Convert.ToInt32(reader["ModuleID"]);
                pageModule.pageID = Convert.ToInt32(reader["PageID"]);
                pageModule.paneName = reader["PaneName"].ToString();
                pageModule.moduleOrder = Convert.ToInt32(reader["ModuleOrder"]);
                if (reader["PublishBeginDate"] != DBNull.Value)
                {
                    pageModule.publishBeginDate
                        = Convert.ToDateTime(reader["PublishBeginDate"]);
                }

                if (reader["PublishEndDate"] != DBNull.Value)
                {
                    pageModule.publishEndDate
                        = Convert.ToDateTime(reader["PublishEndDate"]);
                }

                pageModule.pageName = reader["PageName"].ToString();
                bool useUrl = Convert.ToBoolean(reader["UseUrl"]);
                if (useUrl)
                {
                    pageModule.pageUrl = reader["Url"].ToString();
                }
                else
                {
                    pageModule.pageUrl = "~/Default.aspx?pageid=" + pageModule.pageID.ToString();
                }

                pageModules.Add(pageModule);
            }
            
            return pageModules;
        }


        /// <summary>
        /// Returns all PageModules for the given pageID
        /// including un published ones
        /// </summary>
        public static List<PageModule> GetPageModulesByPage(int pageId)
        {
            List<PageModule> pageModules = new List<PageModule>();

            using (IDataReader reader = DBModule.PageModuleGetReaderByPage(pageId))
            {
                pageModules = LoadListFromReader(reader);
            }

            return pageModules;
        }

        public static List<PageModule> GetPageModulesByModule(int moduleId)
        {
            List<PageModule> pageModules = new List<PageModule>();

            using (IDataReader reader = DBModule.PageModuleGetReaderByModule(moduleId))
            {
                pageModules = LoadListFromReader(reader);
                
            }

            return pageModules;
        }



    }
}
