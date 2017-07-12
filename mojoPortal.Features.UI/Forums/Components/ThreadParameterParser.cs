/// Author:				
/// Created:			2012-10-24
/// Last Modified:	    2012-10-25


using mojoPortal.Business;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace mojoPortal.Web.ForumUI
{
    public class ThreadParameterParser
    {
        private mojoBasePage BasePage;

        public ThreadParameterParser(mojoBasePage basePage) 
        {
            if (basePage == null) { throw new ArgumentException("You must pass in mojoBasePage"); }

            BasePage = basePage;
        }

        private bool paramsAreValid = false;

        public bool ParamsAreValid
        {
            get { return paramsAreValid; }
        }

        private int pageId = -1;

        public int PageId
        {
            get { return pageId; }
        }

        private int itemId = -1;

        public int ItemId
        {
            get { return itemId; }
        }

        private int threadId = -1;

        public int ThreadId
        {
            get { return threadId; }
        }

        private int moduleId = -1;

        public int ModuleId
        {
            get { return moduleId; }
        }

        private int pageNumber = 1;

        public int PageNumber
        {
            get { return pageNumber; }
        }

        private Forum forum = null;

        public Forum Forum
        {
            get { return forum; }
        }

        private ForumThread thread = null;

        public ForumThread Thread
        {
            get { return thread; }
        }

        public void Parse()
        {
            if (BasePage == null) { return; }

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            threadId = WebUtils.ParseInt32FromQueryString("thread", -1);
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);

            // new option to combine params for better seo
            // we don't need moduleid or the forumid (ItemID) in the url we can get it from the thread
            // but we still need to validate that the page contains the moduleid of the forum
            // pageid must remain a separate param but we can combine thread and page number into one param
            // that can be split to get the individual params
            // so we can use ?pageid=x&t=y~z where y is threadid and z is pagenumber
            string f = WebUtils.ParseStringFromQueryString("t", string.Empty);

            if ((f.Length > 0) && (f.Contains("~")))
            {
                List<string> parms = f.SplitOnCharAndTrim('~');
                if (parms.Count == 2)
                {
                    int.TryParse(parms[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out threadId);
                    int.TryParse(parms[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out pageNumber);
                }
            }

            thread = new ForumThread(threadId);
            if (thread.ForumId == -1)
            {
                thread = null;
                return;
            }

            itemId = thread.ForumId;

            forum = new Forum(ItemId);
            if (forum.ModuleId == -1)
            {
                forum = null;
                return;
            }

            Module m = BasePage.GetModule(forum.ModuleId, Forum.FeatureGuid);

            if (m != null)
            { //module exists on the page and matches the forum module id
                moduleId = forum.ModuleId;
                paramsAreValid = true;
            }


        }

        public static string FormatCombinedParam(int itemId, int pageNumber)
        {
            return itemId.ToInvariantString() + "~" + pageNumber.ToInvariantString();
        }


    }
}