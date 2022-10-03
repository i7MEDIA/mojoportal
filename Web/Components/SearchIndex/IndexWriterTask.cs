// Author:					
// Created:				    2008-06-19
// Last Modified:			2019-01-20
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
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;



namespace mojoPortal.SearchIndex
{
    [Serializable()]
    public class IndexWriterTask : ITaskQueueTask
    {
        #region Task Specific Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(IndexWriterTask));

        private int rowsToProcess = 0;
        private int rowsProcessed = 0;
        private int errorCount = 0;
        // we'll try to update every 10 seconds
        // we use this insteaod fo public updatefrequency because some things may take longer
        // to execute like indexWriter.Optimize()
        private int actualUpdateFrequency = 10;
        private DateTime nextStatusUpdateTime = DateTime.MinValue;
        private bool storeContentForResultsHighlighting = false;
        //private Analyzer analyzer = null;
        

        /// <summary>
        /// set to true based on Web.config setting. true makes it possible to do search results highlighting
        /// but also makes the index much bigger in terms of file size
        /// </summary>
        public bool StoreContentForResultsHighlighting
        {
            get { return storeContentForResultsHighlighting; }
            set { storeContentForResultsHighlighting = value; }
        }

        #endregion

        #region Task Specific Methods


        private static void RunTaskOnNewThread(object oTask)
        {
            if (oTask == null) return;
            IndexWriterTask task = oTask as IndexWriterTask;

            log.Debug("deserialized IndexWriterTask task");

            // give a little time to make sure the taskqueue was updated after spawning the thread
            Thread.Sleep(100); // 0.10 seconds

            task.RunTask();

            log.Debug("started IndexWriterTask task");

        }

        private void RunTask()
        {
            if (IsAlreadyRunning())
            {
                MarkAsComplete();
                return;
            }

            ProcessIndexingQueue();
            //MarkAsComplete();

        }

        private void ProcessIndexingQueue()
        {

            try
            {
                bool markAsComplete = false;


                // this gets executed while content is still being queued.
                // if we start right away we may finish and miss some content
                // so pause a bit but log progress so it knows this task is running
                for (int i = 0; i < 10; i++)
                {
                    rowsToProcess = 1;
                    Thread.Sleep(5000); // 5 seconds
                    ReportStatus(markAsComplete);

                }

                rowsToProcess = 0;

                DataTable siteIds = IndexingQueue.GetSiteIDs();

                foreach (DataRow row in siteIds.Rows)
                {
                    int siteId = Convert.ToInt32(row["SiteID"]);


                    string indexPath = IndexHelper.GetSearchIndexPath(siteId);

                    try
                    {
                        if (indexPath.Length > 0)
                        {
                            if (!System.IO.Directory.Exists(indexPath))
                            {
                                System.IO.Directory.CreateDirectory(indexPath);
                            }

                            DataTable q = IndexingQueue.GetByPath(indexPath);
                            ProcessQueue(q, siteId, indexPath);
                        }
                    }
                    catch (System.IO.IOException ex)
                    {
                        errorCount += 1;
                        log.Error(ex);

                    }

                }

                int countOfRowsNotProcessed = IndexingQueue.GetCount();

                if ((countOfRowsNotProcessed > 0) && (errorCount < 3))
                {
                    // recurse
                    ProcessIndexingQueue();
                }
                else
                {
                    this.rowsToProcess = 0;
                    markAsComplete = true;
                    ReportStatus(markAsComplete);


                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }

        }

        

        private void ProcessQueue(DataTable q,  int siteId, string indexPath)
        {
            rowsProcessed = 0;
            rowsToProcess = q.Rows.Count;

            // first process deletes with reader
            try
            {

                using (Lucene.Net.Store.Directory searchDirectory = IndexHelper.GetDirectory(siteId))
                {

                    using (IndexReader reader = IndexReader.Open(searchDirectory, false))
                    {

                        foreach (DataRow row in q.Rows)
                        {
                            Term term = new Term("Key", row["ItemKey"].ToString());
                            try
                            {
                                reader.DeleteDocuments(term);
                                log.Debug("reader.DeleteDocuments(term) for Key " + row["ItemKey"].ToString());
                            }
                            catch (Exception ge)
                            {
                                // TODO: monitor what real exceptions if any occur and then
                                // change this catch to catch only the expected ones
                                // instead of non specific exception
                                log.Error(ge);
                            }

                            bool removeOnly = Convert.ToBoolean(row["RemoveOnly"]);
                            if (removeOnly)
                            {
                                Int64 rowId = Convert.ToInt64(row["RowId"]);
                                IndexingQueue.Delete(rowId);

                            }


                            if (DateTime.UtcNow > nextStatusUpdateTime)
                            {
                                // don't mark as complete because there may be more qu items 
                                //for different index paths in a multi site installation
                                bool markAsComplete = false;
                                ReportStatus(markAsComplete);
                            }

                        }

                    }

                }

                
            }
            catch (System.IO.IOException ex)
            {
                log.Info("IndexWriter swallowed exception this is expected if building or rebuilding the search index ",ex);
                errorCount += 1;
            }
            catch (TypeInitializationException ex)
            {
                log.Info("IndexWriter swallowed exception ", ex);
                errorCount += 1;
            }


            // next add items with writer
            using (Lucene.Net.Store.Directory searchDirectory = IndexHelper.GetDirectory(siteId))
            {
                using (IndexWriter indexWriter = GetWriter(siteId, searchDirectory))
                {
                    if (indexWriter == null)
                    {
                        log.Error("failed to get IndexWriter for path: " + indexPath);
                        errorCount += 1;
                        return;
                    }

                    foreach (DataRow row in q.Rows)
                    {
                        bool removeOnly = Convert.ToBoolean(row["RemoveOnly"]);
                        if (!removeOnly)
                        {
                            try
                            {
                                IndexItem indexItem
                                        = (IndexItem)SerializationHelper.DeserializeFromString(typeof(IndexItem), row["SerializedItem"].ToString());
                                // if the content is locked down to only admins it is a special case
                                // we just won't add it to the search index
                                // because at search time we avoid the role check for all admins, content admins and siteeditors
                                // we don't have a good way to prevent content admins and site editors from seeing the content
                                // in search
                                if ((indexItem.ViewRoles != "Admins;") && (indexItem.ModuleViewRoles != "Admins;"))
                                {

                                    if (indexItem.ViewPage.Length > 0)
                                    {
                                        Document doc = GetDocument(indexItem);
                                        WriteToIndex(doc, indexWriter);
                                        log.Debug("called WriteToIndex(doc, indexWriter) for key " + indexItem.Key);
                                    }
                                }

                                Int64 rowId = Convert.ToInt64(row["RowId"]);
                                IndexingQueue.Delete(rowId);

                            }
                            catch (Exception ex)
                            {
                                log.Error(ex);
                            }


                        }

                        if (DateTime.UtcNow > nextStatusUpdateTime)
                        {
                            // don't mark as complete because there may be more qu items 
                            //for different index paths in a multi site installation
                            bool markAsComplete = false;
                            ReportStatus(markAsComplete);
                        }

                    }

                    try
                    {
                        indexWriter.Optimize();
                    }
                    catch (System.IO.IOException ex)
                    {
                        log.Error(ex);
                    }
                }
            }
        }

        private void WriteToIndex(Document doc, IndexWriter indexWriter)
        {
            try
            {
                indexWriter.AddDocument(doc);
            }
            catch (System.IO.IOException ex)
            {
                log.Error(ex);
            }
        }


        private Document GetDocument(IndexItem indexItem)
        {
            Document doc = new Document();

            // searchable fields
            doc.Add(new Field("Key", indexItem.Key, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("Author", indexItem.Author, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("SiteID", indexItem.SiteId.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ViewRoles", indexItem.ViewRoles, Field.Store.YES, Field.Index.NO));

            string[] roles = indexItem.ViewRoles.Split(';');
            foreach (string role in roles)
            {
                if (role.Length > 0)
                {
                    doc.Add(new Field("Role", role, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
            }

            roles = indexItem.ModuleViewRoles.Split(';');
            foreach (string role in roles)
            {
                if (role.Length > 0)
                {
                    doc.Add(new Field("ModuleRole", role, Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
            }

            doc.Add(new Field("FeatureId", indexItem.FeatureId, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("PageID", indexItem.PageId.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ModuleID", indexItem.ModuleId.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("ItemID", indexItem.ItemId.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NOT_ANALYZED));
            
            doc.Add(new Field("PublishBeginDate", indexItem.PublishBeginDate.ToString("s"), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("PublishEndDate", indexItem.PublishEndDate.ToString("s"), Field.Store.YES, Field.Index.NOT_ANALYZED));
            //doc.Add(new Field("IndexedUtc", DateTime.UtcNow.ToString("s"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            //doc.Add(new NumericField("PubBeginDate", Field.Store.YES, true).SetLongValue(indexItem.PublishBeginDate.Ticks));
            //doc.Add(new NumericField("PubEndDate", Field.Store.YES, true).SetLongValue(indexItem.PublishEndDate.Ticks));

            //2013-01-08 adding created and lastmod date and storage to Numeric field for sorting and range queries
            //http://stackoverflow.com/questions/2685490/lucene-net-sorting-by-int

            //if (indexItem.CreatedUtc > DateTime.MinValue)
            //{
            //    doc.Add(new NumericField("CreatedUtc", Field.Store.YES, true).SetLongValue(indexItem.CreatedUtc.Ticks));
            //}
            //else
            //{
            //    doc.Add(new NumericField("CreatedUtc", Field.Store.YES, true).SetLongValue(DateTime.UtcNow.Ticks));
            //}

            //if (indexItem.LastModUtc > DateTime.MinValue)
            //{
            //    doc.Add(new NumericField("LastModUtc", Field.Store.YES, true).SetLongValue(indexItem.LastModUtc.Ticks));
            //}
            //else
            //{
            //    doc.Add(new NumericField("LastModUtc", Field.Store.YES, true).SetLongValue(DateTime.UtcNow.Ticks));
            //}

            doc.Add(new Field("CreatedUtc", indexItem.CreatedUtc.ToString("s"), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("LastModUtc", indexItem.LastModUtc.ToString("s"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            //doc.Add(new NumericField("CreatedUtc", Field.Store.YES, true).SetIntValue(indexItem.CreatedUtc.ToDateInteger()));
            //doc.Add(new NumericField("LastModUtc", Field.Store.YES, true).SetIntValue(indexItem.LastModUtc.ToDateInteger()));

            



            doc.Add(new Field("PageName", indexItem.PageName, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            doc.Add(new Field("ModuleTitle", indexItem.ModuleTitle, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            doc.Add(new Field("Title", indexItem.Title, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));
            doc.Add(new Field("PageMetaDesc", indexItem.PageMetaDescription, Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));

            string[] keywords = indexItem.PageMetaKeywords.Split(',');
            foreach (string word in keywords)
            {
                if (word.Trim().Length > 0)
                {
                    doc.Add(new Field("Keyword", word.Trim(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                }
            }

            // TODO: store an abstract that can be displayed in alternate to intro and contain raw html
           
            string introContent = ConvertToText(indexItem.Content);
            if (introContent.Length > WebConfigSettings.SearchResultsFragmentSize)
            {
                introContent = UIHelper.CreateExcerpt(introContent, (WebConfigSettings.SearchResultsFragmentSize - 3)) 
                    + "...";
            }

            doc.Add(new Field("Intro", introContent, Field.Store.YES, Field.Index.NOT_ANALYZED));
            
            //doc.Add(new Field("Intro",
            //   (textContent.Length < WebConfigSettings.SearchResultsFragmentSize ? textContent : (UIHelper.CreateExcerpt(textContent, (WebConfigSettings.SearchResultsFragmentSize -3)) + "..."))
            //   , Field.Store.YES, Field.Index.NOT_ANALYZED
            //   )
            //   );

            // for display in recent content features, html is allowed here
            if (indexItem.ContentAbstract.Length == 0) { indexItem.ContentAbstract = introContent; }
            doc.Add(new Field("Abstract", indexItem.ContentAbstract, Field.Store.YES, Field.Index.NO));

            //// other content is optional, used for blog comments
            //// could be used elsewhere
            //if (storeContentForResultsHighlighting)
            //{

            // this is the main searched field
            doc.Add(new Field("contents", ConvertToText(indexItem.Content) + " "
                    + ConvertToText(indexItem.OtherContent), Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.YES));

            //}
            //else
            //{
            //    doc.Add(new Field("contents", textContent + " "
            //        + ConvertToText(indexItem.OtherContent), Field.Store.NO, Field.Index.ANALYZED, Field.TermVector.YES));
            //}

           
            //unsearchable fields
            doc.Add(new Field("Feature", indexItem.FeatureName, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("FeatureResourceFile", indexItem.FeatureResourceFile, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("PageNumber", indexItem.PageNumber.ToString(CultureInfo.InvariantCulture), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("ViewPage", indexItem.ViewPage, Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("UseQueryStringParams", indexItem.UseQueryStringParams.ToString(), Field.Store.YES, Field.Index.NO));
            doc.Add(new Field("QueryStringAddendum", indexItem.QueryStringAddendum, Field.Store.YES, Field.Index.NO));

            doc.Add(new Field("ExcludeFromRecentContent", indexItem.ExcludeFromRecentContent.ToString().ToLower(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            

            return doc;

        }

        

        private IndexWriter GetWriter(int siteId, Lucene.Net.Store.Directory indexDirectory)
        {
            IndexWriter indexWriter = null;
            LuceneSettingsProvider provider =  LuceneSettingsManager.Providers[IndexHelper.GetSiteProviderName(siteId)];
            if (provider == null)
            {
                log.Error("LuceneSettingsProvider could not be obtained");
                return null;
            }

            Analyzer analyzer = provider.GetAnalyzer();
            
            if (!IndexReader.IndexExists(indexDirectory))
            {
                try
                {
  
                    indexWriter = new IndexWriter(indexDirectory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED);
                    
                }
                catch (System.IO.IOException ex)
                {
                    log.Error(ex);
                    try
                    {
                        indexWriter = new IndexWriter(indexDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
                    }
                    catch (System.IO.FileNotFoundException ex2)
                    {
                        log.Error(ex2);

                    }

                }
            }
            else
            {
                try
                {
                    indexWriter = new IndexWriter(indexDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
                }
                catch (System.IO.FileNotFoundException ex2)
                {
                    log.Error(ex2);

                }

            }

            return indexWriter;

        }


        private void ReportStatus()
        {
            bool markAsComplete = true;
            ReportStatus(markAsComplete);
        }

        private void ReportStatus(bool markAsComplete)
        {

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (markAsComplete)
            {
                if (rowsToProcess > 0)
                {

                    task.CompleteRatio = (rowsProcessed / rowsToProcess);
                }
                else
                {
                    task.CompleteRatio = 1; //nothing to do so mark as complete
                }
            }

            if (task.CompleteRatio >= 1)
            {
                task.Status = statusCompleteMessage;

                if (task.CompleteUTC == DateTime.MinValue)
                    task.CompleteUTC = DateTime.UtcNow;




            }
            else
            {
                task.Status = string.Format(
                    CultureInfo.InvariantCulture,
                    statusRunningMessage,
                    rowsProcessed,
                    rowsToProcess);
            }


            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            nextStatusUpdateTime = DateTime.UtcNow.AddSeconds(actualUpdateFrequency);


        }

        private void MarkAsComplete()
        {
            TaskQueue task = new TaskQueue(this.taskGuid);
            task.Status = statusCompleteMessage;
            task.CompleteRatio = 1;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.CompleteUTC = DateTime.UtcNow;
            task.Save();


        }


        //private bool IsStalled(TaskQueue task)
        //{
        //    // TODO: make config setting ?
        //    int taskTimeoutPaddingSeconds = 2;
        //    return (DateTime.UtcNow > task.LastStatusUpdateUTC.AddSeconds(task.UpdateFrequency + taskTimeoutPaddingSeconds));

        //}

        private bool IsAlreadyRunning()
        {
            List<TaskQueue> unfinishedTasks = TaskQueue.GetUnfinished();

            string thisType = typeof(IndexWriterTask).AssemblyQualifiedName;

            Type taskType = Type.GetType(thisType);

            foreach (TaskQueue task in unfinishedTasks)
            {
                Type t = Type.GetType(task.SerializedTaskType);
                if (t == null)
                {
                    task.CompleteRatio = 1;
                    task.CompleteUTC = DateTime.UtcNow;
                    task.Status = this.statusCompleteMessage;
                    task.Save();

                }
                else
                {

                    if (
                        (t.FullName == taskType.FullName)
                        && (task.Guid != this.taskGuid)
                        )
                    {
                        if (TaskQueue.IsStalled(task))
                        {
                            task.CompleteRatio = 1;
                            task.CompleteUTC = DateTime.UtcNow;
                            task.Status = this.statusCompleteMessage;
                            task.Save();
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static bool IsRunning()
        {
            List<TaskQueue> unfinishedTasks = TaskQueue.GetUnfinished();

            string thisType = typeof(IndexWriterTask).AssemblyQualifiedName;
            Type taskType = Type.GetType(thisType);
            
            foreach (TaskQueue task in unfinishedTasks)
            {
                Type t = Type.GetType(task.SerializedTaskType);

                if ((t == null) || (t.FullName == taskType.FullName))
                {
                    if (TaskQueue.IsStalled(task))
                    {
                        task.CompleteRatio = 1;
                        task.CompleteUTC = DateTime.UtcNow;
                        task.Status = task.TaskCompleteMessage;
                        task.Save();
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static Regex MarkupRegex = new Regex("<[/a-zA-Z]+[^>]*>|<!--(?!-->)*-->");

        private static string ConvertToText(string markup)
        {
            return MarkupRegex.Replace(markup, " ");
        }


        #endregion


        #region ITaskQueueTask

        private Guid taskGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        private Guid queuedBy = Guid.Empty;
        private string taskName = "IndexWriterTask";
        private bool notifyOnCompletion = false;
        private string notificationToEmail = String.Empty;
        private string notificationFromEmail = String.Empty;
        private string notificationSubject = String.Empty;
        private string taskCompleteMessage = string.Empty;
        private string statusQueuedMessage = Resources.Resource.IndexWriterTaskQueuedMessage;
        private string statusStartedMessage = Resources.Resource.IndexWriterTaskStartedMessage;
        private string statusRunningMessage = Resources.Resource.IndexWriterTaskRunningFormatString;
        private string statusCompleteMessage = Resources.Resource.IndexWriterTaskCompleteMessage;
        private bool canStop = false;
        private bool canResume = true;
        // report status every 300 seconds by default
        private int updateFrequency = 300;

        #region Public ITaskQueueTask Properties

        public Guid TaskGuid
        {
            get { return taskGuid; }
            set { taskGuid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid QueuedBy
        {
            get { return queuedBy; }
            set { queuedBy = value; }
        }

        public string TaskName
        {
            get { return taskName; }
            set { 
                //taskName = value; 
            }
        }

        public bool NotifyOnCompletion
        {
            get { return notifyOnCompletion; }
            set { notifyOnCompletion = value; }
        }

        public string NotificationToEmail
        {
            get { return notificationToEmail; }
            set { notificationToEmail = value; }
        }

        public string NotificationFromEmail
        {
            get { return notificationFromEmail; }
            set { notificationFromEmail = value; }
        }

        public string NotificationSubject
        {
            get { return notificationSubject; }
            set { notificationSubject = value; }
        }

        public string TaskCompleteMessage
        {
            get { return taskCompleteMessage; }
            set { taskCompleteMessage = value; }
        }

        public string StatusQueuedMessage
        {
            get { return statusQueuedMessage; }
            set { statusQueuedMessage = value; }
        }

        public string StatusStartedMessage
        {
            get { return statusStartedMessage; }
            set { statusStartedMessage = value; }
        }

        public string StatusRunningMessage
        {
            get { return statusRunningMessage; }
            set { statusRunningMessage = value; }
        }

        public string StatusCompleteMessage
        {
            get { return statusCompleteMessage; }
            set { statusCompleteMessage = value; }
        }


        /// <summary>
        /// The frequency in second at which task status updates are expected.
        /// If no update to taskqueue status for 3x this value the taks is considered stalled.
        /// </summary>
        public int UpdateFrequency
        {
            get { return updateFrequency; }

        }

        public bool CanStop
        {
            get { return canStop; }

        }

        public bool CanResume
        {
            get { return canResume; }

        }

        #endregion

        public void QueueTask()
        {

            if (this.taskGuid != Guid.Empty) return;

            TaskQueue task = new TaskQueue();
            task.SiteGuid = SiteSettings.GetRootSiteGuid();

            if (task.SiteGuid == Guid.Empty) return;

            task.QueuedBy = this.queuedBy;
            task.SerializedTaskType = this.GetType().AssemblyQualifiedName;
            task.TaskName = task.SerializedTaskType;
            task.NotifyOnCompletion = this.notifyOnCompletion;
            task.NotificationToEmail = this.notificationToEmail;
            task.NotificationFromEmail = this.notificationFromEmail;
            task.NotificationSubject = this.notificationSubject;
            task.TaskCompleteMessage = this.taskCompleteMessage;
            task.CanResume = this.canResume;
            task.CanStop = this.canStop;
            task.UpdateFrequency = this.updateFrequency;
            task.Status = statusQueuedMessage;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            this.taskGuid = task.NewGuid;
            task.SerializedTaskObject = SerializationHelper.SerializeToString(this);

            task.Save();


        }

        public void StartTask()
        {
            if (this.taskGuid == Guid.Empty) return;

            TaskQueue task = new TaskQueue(this.taskGuid);

            if (task.Guid == Guid.Empty) return; // task not found

            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(RunTaskOnNewThread), this))
            {
                throw new Exception("Couldn't queue the task on a new thread.");
            }

            task.Status = statusRunningMessage;
            task.StartUTC = DateTime.UtcNow;
            task.LastStatusUpdateUTC = DateTime.UtcNow;
            task.Save();

            log.Debug("Queued IndexWriterTask on a new thread");


        }

        public void StopTask()
        {
            throw new System.NotImplementedException("This feature is not implemented");

        }

        public void ResumeTask()
        {
            StartTask();

        }

        #endregion

        

    }
}
