using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    /// <summary>
    /// Author:					
    /// Created:				2007-07-22
    /// Last Modified:		    2007-07-23
    /// 
    /// 
    /// The use and distribution terms for this software are covered by the 
    /// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
    /// which can be found in the file CPL.TXT at the root of this distribution.
    /// By using this software in any fashion, you are agreeing to be bound by 
    /// the terms of this license.
    ///
    /// You must not remove this notice, or any other, from this software.
    /// </summary>
    public class ModuleDecoratedPageSettings
    {
        public ModuleDecoratedPageSettings(PageSettings pageSettings)
        {
            if (pageSettings == null) throw new ArgumentException("pageSettigns can't be null");
            this.pageSettings = pageSettings;

        }

        private PageSettings pageSettings;
        private int moduleID = -1;
        private int moduleOrder = 0;
        private string paneName = string.Empty;
        private bool isPublished = false;
        private string publishBeginDate = string.Empty;
        private string publishEndDate = string.Empty;

        public int PageID
        {
            get { return pageSettings.PageID; }
            
        }

        public string PageName
        {
            get { return pageSettings.PageName; }
            
        }

        public string DepthIndicator
        {
            get { return pageSettings.DepthIndicator; }
           
        }

        public int ModuleID
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

        public static List<ModuleDecoratedPageSettings> GetDecoratedPages(
            ArrayList pageSettingsCol, 
            int moduleID)
        {
            
            List<ModuleDecoratedPageSettings> pageList = new List<ModuleDecoratedPageSettings>();

            if (pageSettingsCol == null) return pageList;

            DataTable dataTable = dbPortal.PageModule_GetByModule(moduleID);
            foreach (PageSettings page in pageSettingsCol)
            {
                ModuleDecoratedPageSettings decoratedPage 
                    = new ModuleDecoratedPageSettings(page);

                decoratedPage.ModuleID = moduleID;
                foreach (DataRow row in dataTable.Rows)
                {
                    int pageID = Convert.ToInt32(row["PageID"]);
                    if (pageID == page.PageID)
                    {
                        decoratedPage.IsPublished = true;
                        decoratedPage.PaneName = row["PaneName"].ToString();

                        if (decoratedPage.PaneName.Length == 0)
                        {
                            decoratedPage.PaneName = "contentpane";
                        }

                        decoratedPage.ModuleOrder = Convert.ToInt32(row["ModuleOrder"]);
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
