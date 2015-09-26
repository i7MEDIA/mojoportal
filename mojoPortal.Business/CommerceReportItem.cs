using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using mojoPortal.Data;

namespace mojoPortal.Business
{
    public class CommerceReportItem
    {
        private Guid itemGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private string itemName = string.Empty;
        private string moduleTitle = string.Empty;
        private decimal totalRevenue = 0;

        public Guid ItemGuid
        {
            get { return itemGuid; }
        }

        public Guid ModuleGuid
        {
            get { return moduleGuid; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
        }

        public string ItemName
        {
            get { return itemName; }
        }

        public string ModuleTitle
        {
            get { return moduleTitle; }
        }

        public decimal TotalRevenue
        {
            get { return totalRevenue; }
        }

        public static CommerceReportItem GetByGuid(Guid itemGuid)
        {
            CommerceReportItem item = new CommerceReportItem();
            using (IDataReader reader = DBCommerceReport.GetItemSummary(itemGuid))
            {
                if (reader.Read())
                {
                    item.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    item.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    item.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    item.itemName = reader["ItemName"].ToString();
                    item.moduleTitle = reader["ModuleTitle"].ToString();
                    item.totalRevenue = Convert.ToDecimal(reader["Revenue"]);

                }

            }

            return item;
        }

    }
}
