// Author:					Kevin Needham
// Created:					2009-6-19
// Last Modified:			2013-04-23
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
using System.Globalization;
//using System.Linq;
using System.Text;
using mojoPortal.Data;
using System.Data;
using mojoPortal.Business.Properties;

namespace mojoPortal.Business
{
    public class ContentWorkflow
    {
        #region Private Variables

        private Guid guid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid moduleGuid = Guid.Empty;
        private DateTime createdDateUtc = DateTime.UtcNow;
        private Guid userGuid = Guid.Empty;
        private Guid lastModUserGuid = Guid.Empty;
        private DateTime lastModUtc = DateTime.UtcNow;
        private ContentWorkflowStatus status = ContentWorkflowStatus.None;
        private ContentWorkflowStatus originalStatus = ContentWorkflowStatus.None;
        private string contentText = string.Empty;
        private string customData = string.Empty;
        private int customReferenceNumber = -1;
        private Guid customReferenceGuid = Guid.Empty;
        private string createdByUserName = string.Empty;
        private string createdByUserLogin = string.Empty;
        private string recentActionByUserLogin = string.Empty;
        private string recentActionByUserName = string.Empty;
        private string recentActionByUserEmail = string.Empty;
        private string notes = string.Empty;
        private DateTime recentActionOn = DateTime.UtcNow;
        private string moduleTitle = string.Empty;
        private int moduleId = -1;

        

        //private Module module;
        //private string notes = string.Empty;

        #endregion

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }
        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }
        public Guid ModuleGuid
        {
            get { return moduleGuid; }
            set { moduleGuid = value; }
        }

        public string ContentText
        {
            get { return contentText; }
            set { contentText = value; }
        }
        public string CustomData
        {
            get { return customData; }
            set { customData = value; }
        }

        public Guid LastModUserGuid
        {
            get { return lastModUserGuid; }
            set { lastModUserGuid = value; }
        }

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        public DateTime CreatedDateUtc
        {
            get { return createdDateUtc; }
            set { createdDateUtc = value; }
        }

        public int CustomReferenceNumber
        {
            get { return customReferenceNumber; }
            set { customReferenceNumber = value; }
        }

        public Guid CustomReferenceGuid
        {
            get { return customReferenceGuid; }
            set { customReferenceGuid = value; }
        }

        public ContentWorkflowStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        public string CreatedByUserName
        {
            get { return createdByUserName; }
        }

        private string createdByUserFirstName = string.Empty;

        public string CreatedByUserFirstName
        {
            get { return createdByUserFirstName; }
        }

        private string createdByUserLastName = string.Empty;

        public string CreatedByUserLastName
        {
            get { return createdByUserLastName; }
        }

        public string CreatedByUserLogin
        {
            get { return createdByUserLogin; }
        }

        private string authorEmail = string.Empty;

        public string AuthorEmail
        {
            get { return authorEmail; }
        }

        private string authorBio = string.Empty;

        public string AuthorBio
        {
            get { return authorBio; }
        }

        private string authorAvatar = string.Empty;

        public string AuthorAvatar
        {
            get { return authorAvatar; }
           
        }

        private int authorUserId = -1;

        public int AuthorUserId
        {
            get { return authorUserId; }
        }

        private string lastModByUserName = string.Empty;

        public string LastModByUserName
        {
            get { return lastModByUserName; }
        }


        private string lastModByUserFirstName = string.Empty;

        public string LastModByUserFirstName
        {
            get { return lastModByUserFirstName; }
        }

        private string lastModByUserLastName = string.Empty;

        public string LastModByUserLastName
        {
            get { return lastModByUserLastName; }
        }

        public string RecentActionByUserLogin
        {
            get { return recentActionByUserLogin; }
        }

        public string RecentActionByUserName
        {
            get { return recentActionByUserName; }
        }

        public string RecentActionByUserEmail
        {
            get { return recentActionByUserEmail; }
        }

        public int ModuleId
        {
            get { return moduleId; }
        }

        public string ModuleTitle
        {
            get { return moduleTitle; }
        }

        //public Module Module
        //{
        //    get
        //    {
        //        if (module == null && this.ModuleGuid != Guid.Empty)
        //        {
        //            module = new Module(this.ModuleGuid);
        //        }
        //        return module;
        //    }
        //}

        public string Notes
        {
            get { return notes; }
        }

        public DateTime RecentActionOn
        {
            get { return recentActionOn; }
        }

        //public bool IsValid
        //{
        //    get { return (GetRuleViolations().Count() == 0); }
        //}

        #endregion

        private static List<ContentWorkflow> LoadListFromReader(IDataReader reader)
        {
            List<ContentWorkflow> contentWorkflowList = new List<ContentWorkflow>();
            try
            {
                while (reader.Read())
                {
                    ContentWorkflow contentWorkflow = new ContentWorkflow();

                    contentWorkflow.guid = new Guid(reader["Guid"].ToString());
                    contentWorkflow.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    contentWorkflow.userGuid = new Guid(reader["UserGuid"].ToString());
                    contentWorkflow.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                    contentWorkflow.contentText = reader["ContentText"].ToString();
                    contentWorkflow.customData = reader["CustomData"].ToString();

                    object val = reader["CustomReferenceNumber"];
                    contentWorkflow.customReferenceNumber = val == DBNull.Value ? -1 : Convert.ToInt32(val);

                    val = reader["CustomReferenceGuid"];
                    contentWorkflow.customReferenceGuid = val == DBNull.Value ? Guid.Empty : new Guid(val.ToString());

                    contentWorkflow.createdDateUtc = Convert.ToDateTime(reader["CreatedDateUtc"]);

                    //val = reader["LastModUserGuid"];
                    //contentWorkflow.lastModUserGuid = val == DBNull.Value ? null : (Guid?)new Guid(val.ToString());
                    contentWorkflow.lastModUserGuid = new Guid(reader["LastModUserGuid"].ToString());

                    //val = reader["LastModUtc"];
                    //contentWorkflow.lastModUtc = val == DBNull.Value ? null : (DateTime?)DateTime.Parse(val.ToString());

                    contentWorkflow.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

                    contentWorkflow.createdByUserLogin = reader["CreatedByUserLogin"].ToString();
                    contentWorkflow.createdByUserName = reader["CreatedByUserName"].ToString();

                    contentWorkflow.recentActionByUserLogin = reader["RecentActionByUserLogin"].ToString();
                    contentWorkflow.recentActionByUserName = reader["RecentActionByUserName"].ToString();
                    contentWorkflow.recentActionByUserEmail = reader["RecentActionByUserEmail"].ToString();

                    string statusVal = reader["Status"].ToString();
                    contentWorkflow.status = String.IsNullOrEmpty(statusVal)
                        ? ContentWorkflowStatus.None
                        : (ContentWorkflowStatus)Enum.Parse(typeof(ContentWorkflowStatus), statusVal);

                    contentWorkflow.originalStatus = contentWorkflow.status;

                    contentWorkflow.notes = reader["Notes"].ToString();

                    //val = reader["RecentActionOn"];
                    //contentWorkflow.recentActionOn = val == null ? null : (DateTime?)DateTime.Parse(val.ToString());
                    if (reader["RecentActionOn"] != DBNull.Value)
                    {
                        contentWorkflow.recentActionOn = Convert.ToDateTime(reader["RecentActionOn"]);
                    }

                    contentWorkflow.moduleTitle = reader["ModuleTitle"].ToString();
                    contentWorkflow.moduleId = Convert.ToInt32(reader["ModuleID"]);

                    contentWorkflow.createdByUserFirstName = reader["CreatedByFirstName"].ToString();
                    contentWorkflow.createdByUserLastName = reader["CreatedByLastName"].ToString();

                    contentWorkflow.authorEmail = reader["CreatedByUserEmail"].ToString();
                    contentWorkflow.authorAvatar = reader["CreatedByAvatar"].ToString();
                    contentWorkflow.authorBio = reader["CreatedByAuthorBio"].ToString();
                    contentWorkflow.authorUserId = Convert.ToInt32(reader["CreatedByUserID"]);
                    
                    contentWorkflow.lastModByUserLastName = reader["ModifiedByUserName"].ToString();
                    contentWorkflow.lastModByUserFirstName = reader["ModifiedByFirstName"].ToString();
                    contentWorkflow.lastModByUserLastName = reader["ModifiedByLastName"].ToString();

                    contentWorkflowList.Add(contentWorkflow);
                }
            }
            finally
            {
                reader.Close();
            }

            return contentWorkflowList;

        }

        

        internal bool PopulateFromReader(IDataReader reader)
        {
            if (reader.Read())
            {
                this.guid = new Guid(reader["Guid"].ToString());
                this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                this.userGuid = new Guid(reader["UserGuid"].ToString());
                this.moduleGuid = new Guid(reader["ModuleGuid"].ToString());
                this.contentText = reader["ContentText"].ToString();
                this.customData = reader["CustomData"].ToString();

                object val = reader["CustomReferenceNumber"];
                this.customReferenceNumber = val == DBNull.Value ? -1 : Convert.ToInt32(val);

                val = reader["CustomReferenceGuid"];
                this.customReferenceGuid = val == DBNull.Value ? Guid.Empty : new Guid(val.ToString());

                this.createdDateUtc = Convert.ToDateTime(reader["CreatedDateUtc"]);

                //val = reader["LastModUserGuid"];
                //this.lastModUserGuid = val == DBNull.Value ? null : (Guid?)new Guid(val.ToString());
                this.lastModUserGuid = new Guid(reader["LastModUserGuid"].ToString());

                //val = reader["LastModUtc"];
                //this.lastModUtc = val == DBNull.Value ? null : (DateTime?)DateTime.Parse(val.ToString());

                this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);

                this.createdByUserLogin = reader["CreatedByUserLogin"].ToString();
                this.createdByUserName = reader["CreatedByUserName"].ToString();

                this.createdByUserFirstName = reader["CreatedByFirstName"].ToString();
                this.createdByUserLastName = reader["CreatedByLastName"].ToString();
                this.authorEmail = reader["CreatedByUserEmail"].ToString();
                this.authorAvatar = reader["CreatedByAvatar"].ToString();
                this.authorBio = reader["CreatedByAuthorBio"].ToString();
                this.authorUserId = Convert.ToInt32(reader["CreatedByUserID"]);

                this.lastModByUserLastName = reader["ModifiedByUserName"].ToString();
                this.lastModByUserFirstName = reader["ModifiedByFirstName"].ToString();
                this.lastModByUserLastName = reader["ModifiedByLastName"].ToString();

                this.recentActionByUserLogin = reader["RecentActionByUserLogin"].ToString();
                this.recentActionByUserName = reader["RecentActionByUserName"].ToString();
                this.recentActionByUserEmail = reader["RecentActionByUserEmail"].ToString();

                string statusVal = reader["Status"].ToString();
                this.status = String.IsNullOrEmpty(statusVal)
                    ? ContentWorkflowStatus.None
                    : (ContentWorkflowStatus)Enum.Parse(typeof(ContentWorkflowStatus), statusVal);

                this.originalStatus = status;

                this.notes = reader["Notes"].ToString();

                //val = reader["RecentActionOn"];
                //this.recentActionOn = val == null ? null : (DateTime?)DateTime.Parse(val.ToString());
                if (reader["RecentActionOn"] != DBNull.Value)
                {
                    this.recentActionOn = Convert.ToDateTime(reader["RecentActionOn"]);
                }

                this.moduleTitle = reader["ModuleTitle"].ToString();

                return true;
            }

            return false;

        }

        /// <summary>
        /// Saves this instance of Content Workflow. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if ((lastModUserGuid == Guid.Empty) && (userGuid != Guid.Empty))
            { lastModUserGuid = userGuid; }

            if ((userGuid == Guid.Empty) && (lastModUserGuid != Guid.Empty))
            { userGuid = lastModUserGuid; }

            if (this.Guid != Guid.Empty)
                return Update();
            else
                return Create();
        }

        public bool RejectContentChanges(Guid rejectorGuid, string rejectionReason)
        {
            this.status = ContentWorkflowStatus.ApprovalRejected;
            this.lastModUserGuid = rejectorGuid;
            this.lastModUtc = DateTime.UtcNow;
            this.notes = rejectionReason;
            return Save();


            //return DBContentWorkflow.RejectContentChanges(moduleGuid, rejectorGuid, rejectionReason);
        }

        

        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBContentWorkflow.Create(
                this.guid,
                this.siteGuid,
                this.moduleGuid,
                this.userGuid,
                this.createdDateUtc,
                this.contentText,
                this.customData,
                this.customReferenceNumber,
                this.customReferenceGuid,
                this.status.ToString());

            //make entry in approval process table
            DBContentWorkflow.DeactivateAudit(moduleGuid);
            DBContentWorkflow.CreateAuditHistory(
                Guid.NewGuid(),
                this.guid,
                this.moduleGuid,
                this.lastModUserGuid,
                this.lastModUtc,
                this.status.ToString(),
                this.notes,
                true);

            

            return (rowsAffected > 0);

        }

        /// <summary>
        /// Updates an existing instance of ContentHistory. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Update()
        {
            int rowsAffected = DBContentWorkflow.Update(
                this.guid,
                this.lastModUserGuid,
                this.lastModUtc,
                this.contentText,
                this.customData,
                this.customReferenceNumber,
                this.customReferenceGuid,
                this.status.ToString());

            if (status == ContentWorkflowStatus.Approved)
            {
                //make entry in approval process table
                DBContentWorkflow.CreateAuditHistory(
                    Guid.NewGuid(),
                    this.guid,
                    this.moduleGuid,
                    this.lastModUserGuid,
                    this.lastModUtc,
                    this.status.ToString(),
                    this.notes,
                    false);

                DBContentWorkflow.DeactivateAudit(moduleGuid);

            }
            else if(status != originalStatus)
            {
                DBContentWorkflow.DeactivateAudit(moduleGuid);
                DBContentWorkflow.CreateAuditHistory(
                    Guid.NewGuid(),
                    this.guid,
                    this.moduleGuid,
                    this.lastModUserGuid,
                    this.lastModUtc,
                    this.status.ToString(),
                    this.notes,
                    true);

            }

            return (rowsAffected > 0);

        }

        //public IEnumerable<RuleViolation> GetRuleViolations()
        //{

        //    if (moduleGuid == Guid.Empty)
        //        yield return new RuleViolation(Resources.ContentWorkflowRequiresModuleGuid);

           

        //    yield break;
        //}

        public static ContentWorkflow GetWorkInProgress(Guid moduleGuid)
        {
            using (IDataReader reader = DBContentWorkflow.GetWorkInProgress(moduleGuid))
            {
                ContentWorkflow wip = new ContentWorkflow();

                if (wip.PopulateFromReader(reader))
                    return wip;
                else
                    return null;
            }
        }

        public static ContentWorkflow GetWorkInProgress(Guid moduleGuid, string status)
        {
            using (IDataReader reader = DBContentWorkflow.GetWorkInProgress(moduleGuid, status))
            {
                ContentWorkflow wip = new ContentWorkflow();

                if (wip.PopulateFromReader(reader))
                    return wip;
                else
                    return null;
            }
        }

        public static ContentWorkflow CreateDraftVersion(
            Guid siteGuid,
            string contentText,
            string customData,
            int customReferenceNumber,
            Guid customReferenceGuid,
            Guid moduleGuid,
            Guid userGuid)
        {
            ContentWorkflow draftVersion = new ContentWorkflow();
            draftVersion.moduleGuid = moduleGuid;
            draftVersion.contentText = contentText;
            draftVersion.customData = customData;
            draftVersion.customReferenceNumber = customReferenceNumber;
            draftVersion.customReferenceGuid = customReferenceGuid;
            draftVersion.siteGuid = siteGuid;
            draftVersion.userGuid = userGuid;
            draftVersion.lastModUserGuid = userGuid;
            draftVersion.createdDateUtc = DateTime.UtcNow;
            draftVersion.status = ContentWorkflowStatus.Draft;
            draftVersion.Save();

            return draftVersion;
        }

        public static bool DeleteByModule(Guid moduleGuid)
        {
            return DBContentWorkflow.DeleteByModule(moduleGuid);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBContentWorkflow.DeleteBySite(siteGuid);
        }

        public static int GetWorkInProgressCountByPage(Guid pageGuid)
        {
            return DBContentWorkflow.GetWorkInProgressCountByPage(pageGuid);
        }

        public static Guid GetDraftSubmitter(Guid workflowGuid)
        {
            return DBContentWorkflow.GetDraftSubmitter(workflowGuid);
        }


        //private static bool SetWorkInProgressStatus(Guid moduleGuid, ContentWorkflowStatus status)
        //{
        //    ContentWorkflow wip = ContentWorkflow.GetWorkInProgress(moduleGuid);
        //    wip.Status = status;
        //    return wip.Save();
        //}

        //public static bool PostDraftContentForApproval(Guid moduleGuid)
        //{
        //    return SetWorkInProgressStatus(moduleGuid, ContentWorkflowStatus.AwaitingApproval);
        //}

        //public static bool CancelChanges(Guid moduleGuid)
        //{
        //    return SetWorkInProgressStatus(moduleGuid, ContentWorkflowStatus.Cancelled);
        //}

        private static DataTable GetEmptyTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("SiteGuid", typeof(Guid));
            dataTable.Columns.Add("UserGuid", typeof(Guid));
            dataTable.Columns.Add("LastModUserGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleGuid", typeof(Guid));
            dataTable.Columns.Add("CustomReferenceGuid", typeof(Guid));
            dataTable.Columns.Add("ModuleID", typeof(int));
            dataTable.Columns.Add("ModuleTitle", typeof(string));
            dataTable.Columns.Add("ContentText", typeof(string));
            dataTable.Columns.Add("CustomData", typeof(string));
            dataTable.Columns.Add("CustomReferenceNumber", typeof(int));
            dataTable.Columns.Add("CreatedDateUtc", typeof(DateTime));
            dataTable.Columns.Add("LastModUtc", typeof(DateTime));
            dataTable.Columns.Add("CreatedByUserLogin", typeof(string));
            dataTable.Columns.Add("CreatedByUserName", typeof(string));
            dataTable.Columns.Add("RecentActionByUserLogin", typeof(string));
            dataTable.Columns.Add("RecentActionByUserName", typeof(string));
            dataTable.Columns.Add("RecentActionByUserEmail", typeof(string));
            dataTable.Columns.Add("Status", typeof(string));
            dataTable.Columns.Add("Notes", typeof(string));
            dataTable.Columns.Add("RecentActionOn", typeof(DateTime));


            return dataTable;

        }

        private static DataTable GetEmptyPageInfoTable()
        {
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("PageID", typeof(int));
            dataTable.Columns.Add("PageGuid", typeof(Guid));
            dataTable.Columns.Add("WorkflowGuid", typeof(Guid));
            dataTable.Columns.Add("PageUrl", typeof(string));
            dataTable.Columns.Add("PageName", typeof(string));
            


            return dataTable;

        }

        /// <summary>
        /// Returns a DataSet with a page ContentWorkflow items and their related pagesettings info
        /// </summary>
        /// <param name="siteGuid"></param>
        /// <param name="status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>
        /// <returns></returns>
        public static DataSet GetPageOfWorkflowsWithPageInfo(Guid siteGuid, ContentWorkflowStatus status, int pageNumber, int pageSize, out int totalPages)
        {
            DataSet dataSet = new DataSet();

            DataTable workFlowsTable = GetEmptyTable();
            using (IDataReader reader = DBContentWorkflow.GetPage(
                                    siteGuid,
                                    status.ToString(),
                                    pageNumber,
                                    pageSize,
                                    out totalPages))
            {
                while (reader.Read())
                {
                    DataRow row = workFlowsTable.NewRow();

                    row["Guid"] = new Guid(reader["Guid"].ToString());
                    row["SiteGuid"] = new Guid(reader["SiteGuid"].ToString());
                    row["UserGuid"] = new Guid(reader["UserGuid"].ToString());
                    row["LastModUserGuid"] = new Guid(reader["LastModUserGuid"].ToString());
                    row["ModuleGuid"] = new Guid(reader["ModuleGuid"].ToString());
                    row["CustomReferenceGuid"] = new Guid(reader["CustomReferenceGuid"].ToString());
                    row["ModuleID"] = Convert.ToInt32(reader["ModuleID"]);
                    row["ModuleTitle"] = reader["ModuleTitle"];

                    row["ContentText"] = reader["ContentText"];
                    row["CustomData"] = reader["CustomData"];
                    row["CustomReferenceNumber"] = Convert.ToInt32(reader["CustomReferenceNumber"]);
                    row["CreatedDateUtc"] = Convert.ToDateTime(reader["CreatedDateUtc"]);
                    row["LastModUtc"] = Convert.ToDateTime(reader["LastModUtc"]);
                    row["CreatedByUserLogin"] = reader["CreatedByUserLogin"];
                    row["CreatedByUserName"] = reader["CreatedByUserName"];
                    row["RecentActionByUserLogin"] = reader["RecentActionByUserLogin"];
                    row["RecentActionByUserName"] = reader["RecentActionByUserName"];
                    row["RecentActionByUserEmail"] = reader["RecentActionByUserEmail"];
                    row["Status"] = reader["Status"];
                    row["Notes"] = reader["Notes"];
                    if (reader["RecentActionOn"] != DBNull.Value)
                    {
                        row["RecentActionOn"] = Convert.ToDateTime(reader["RecentActionOn"]);
                    }
                    else
                    {
                        row["RecentActionOn"] = DateTime.UtcNow;
                    }
                    
                    workFlowsTable.Rows.Add(row);

                }

            }

            DataTable pageInfoTable = GetEmptyPageInfoTable();

            using (IDataReader reader = DBContentWorkflow.GetPageInfoForPage(
                                    siteGuid,
                                    status.ToString(),
                                    pageNumber,
                                    pageSize))
            {
                while (reader.Read())
                {
                    DataRow row = pageInfoTable.NewRow();
                    int pageId = Convert.ToInt32(reader["PageID"]);
                    row["PageID"] = pageId;
                    row["PageGuid"] = new Guid(reader["PageGuid"].ToString());
                    row["WorkflowGuid"] = new Guid(reader["WorkflowGuid"].ToString());
                    row["PageName"] = reader["PageName"];
                    bool useUrl = Convert.ToBoolean(reader["UseUrl"]);

                    if (useUrl)
                    {
                        row["PageUrl"] = reader["PageUrl"];
                    }
                    else
                    {
                        row["PageUrl"] = "~/Default.aspx?pageid=" + pageId.ToString(CultureInfo.InvariantCulture);
                    }
                    
                    pageInfoTable.Rows.Add(row);

                }

            }

            workFlowsTable.TableName = "WorkFlows";
            dataSet.Tables.Add(workFlowsTable);

            pageInfoTable.TableName = "PageInfo";
            dataSet.Tables.Add(pageInfoTable);

            // create a relationship
            dataSet.Relations.Add("workflowPages",
                dataSet.Tables["WorkFlows"].Columns["Guid"],
                dataSet.Tables["PageInfo"].Columns["WorkflowGuid"]);



            return dataSet;
        }
        

        //public static List<ContentWorkflow> GetAwaitingApprovalPage(Guid siteGuid, int pageNumber, int pageSize, out int totalPages)
        //{
        //    IDataReader data = DBContentWorkflow.GetPage( 
        //                            siteGuid,
        //                            ContentWorkflowStatus.AwaitingApproval.ToString(),
        //                            pageNumber,
        //                            pageSize,
        //                            out totalPages);

        //    return ContentWorkflow.LoadListFromReader(data);
        //}

        //public static List<ContentWorkflow> GetRejectedContentPage(Guid siteGuid, int pageNumber, int pageSize, out int totalPages)
        //{
        //    IDataReader data = DBContentWorkflow.GetPage(  
        //                            siteGuid,
        //                            ContentWorkflowStatus.ApprovalRejected.ToString(),
        //                            pageNumber,
        //                            pageSize,
        //                            out totalPages);

        //    return ContentWorkflow.LoadListFromReader(data);
        //}
    }
}
