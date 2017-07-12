// Author:					
// Created:				    2007-07-22
// Last Modified:		    2007-11-20
// 
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using mojoPortal.Business;

namespace mojoPortal.Web
{
    /// <summary>
    ///
    /// </summary>
    public class ModuleDecoratedSiteMapNode
    {
        public ModuleDecoratedSiteMapNode(mojoSiteMapNode siteNode)
        {
            if (siteNode == null) throw new ArgumentException("siteNode can't be null");
            this.siteMapNode = siteNode;

        }

        private mojoSiteMapNode siteMapNode;
        private int moduleID = -1;
        private int moduleOrder = 0;
        private string paneName = string.Empty;
        private bool isPublished = false;
        private string publishBeginDate = string.Empty;
        private string publishEndDate = string.Empty;

        public int PageId
        {
            get { return siteMapNode.PageId; }

        }

        public string PageName
        {
            get { return siteMapNode.Title; }

        }

        public string DepthIndicator
        {
            get { return siteMapNode.DepthIndicator; }

        }

        public string ViewRoles
        {
            get { return siteMapNode.ViewRoles; }

        }

        public string EditRoles
        {
            get { return siteMapNode.EditRoles; }

        }

        public int ModuleId
        {
            get { return moduleID; }
            set { moduleID = value; }
        }

        public int ModuleOrder
        {
            get { return moduleOrder; }
            set { moduleOrder = value; }
        }

        public string PaneName
        {
            get { return paneName; }
            set { paneName = value; }
        }

        public string PublishBeginDate
        {
            get { return publishBeginDate; }
            set { publishBeginDate = value; }
        }

        public string PublishEndDate
        {
            get { return publishEndDate; }
            set { publishEndDate = value; }
        }

        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }

        public static List<ModuleDecoratedSiteMapNode> GetDecoratedNodes(
            ArrayList pageSettingsCol,
            int moduleId)
        {

            List<ModuleDecoratedSiteMapNode> pageList = new List<ModuleDecoratedSiteMapNode>();

            if (pageSettingsCol == null) return pageList;

            DataTable dataTable = Module.GetPageModulesTable(moduleId);
            foreach (mojoSiteMapNode page in pageSettingsCol)
            {
                ModuleDecoratedSiteMapNode decoratedPage
                    = new ModuleDecoratedSiteMapNode(page);

                decoratedPage.ModuleId = moduleId;
                foreach (DataRow row in dataTable.Rows)
                {
                    int pageID = Convert.ToInt32(row["PageID"], CultureInfo.InvariantCulture);
                    if (pageID == page.PageId)
                    {
                        decoratedPage.IsPublished = true;
                        decoratedPage.PaneName = row["PaneName"].ToString();

                        if (decoratedPage.PaneName.Length == 0)
                        {
                            decoratedPage.PaneName = "contentpane";
                        }

                        decoratedPage.ModuleOrder = Convert.ToInt32(row["ModuleOrder"], CultureInfo.InvariantCulture);
                        decoratedPage.PublishBeginDate = row["PublishBeginDate"].ToString();
                        decoratedPage.PublishEndDate = row["PublishEndDate"].ToString();

                    }

                }

                pageList.Add(decoratedPage);


            }

            return pageList;
        }


    }
}
