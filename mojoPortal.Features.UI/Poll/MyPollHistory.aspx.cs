/// Author:                     Christian Fredh
/// Created:                    2007-08-01
///	Last Modified:              2009-11-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using Resources;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using mojoPortal.Business;
using PollFeature.Business;
using System.Web.UI;
using PollFeature.UI.Helpers;

namespace PollFeature.UI
{
   
    public partial class MyPollHistory : mojoBasePage
    {
        private SiteUser currentUser = null;
        protected Double timeOffset = 0;
        private string resultBarColor = "#3082af";
        protected Hashtable moduleSettings;
        private int pageId = -1;
        private int moduleId = -1;
        
        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            
            this.dlPolls.ItemDataBound += new DataListItemEventHandler(dlPolls_ItemDataBound);
            

            SuppressPageMenu();
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            PopulateLabels();
            PopulateControls();

            

        }

        private void PopulateControls()
        {
            if (IsPostBack) return;

            currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser == null) return;
            using (IDataReader reader = Poll.GetPollsByUser(currentUser.UserGuid))
            {
                dlPolls.DataSource = reader;
                dlPolls.DataBind();
            }
        }

        private void dlPolls_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Guid pollGuid = new Guid(dlPolls.DataKeys[e.Item.ItemIndex].ToString());
            DataList dlResults = (DataList)e.Item.FindControl("dlResults");
            
            //IDataReader reader = PollOption.GetOptionsByPollGuid(pollGuid);
            List<PollOption> pollOptions = PollOption.GetOptionsByPollGuid(pollGuid);
            dlResults.DataSource = pollOptions;
            dlResults.DataBind();
            

            foreach (DataListItem item in dlResults.Items)
            {
                Control spnResultImage = (Control)item.FindControl("spnResultImage");
                Guid optionGuid = new Guid(dlResults.DataKeys[item.ItemIndex].ToString());
                //PollOption option = new PollOption(optionGuid);
                PollOption option = pollOptions[item.ItemIndex];
                PollUIHelper.AddResultBarToControl(option, resultBarColor, spnResultImage);
            }
        }

        

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkPollHistory.Text = PollResources.PollHistoryLabel;
            lnkPollHistory.NavigateUrl = Request.RawUrl;

            heading.Text = PollResources.PollHistoryLabel;
            
        }

        protected String GetOptionResultText(Object oOptionGuid)
        {
            Guid optionGuid = new Guid(oOptionGuid.ToString());

            PollOption option = new PollOption(optionGuid);

            String orderNumber = String.Empty;
            if (option.Poll.ShowOrderNumbers)
            {
                orderNumber = option.Order.ToString() + ". ";
            }

            String votesText = (option.Votes == 1) ? PollResources.PollVoteText : PollResources.PollVotesText;

            // TODO: Some pattern based resource...
            String text = orderNumber + option.Answer + ", " + option.Votes + " " + votesText;

            if (option.Poll.TotalVotes != 0)
            {
                int percent = (int)((double)option.Votes * 100 / option.Poll.TotalVotes + 0.5);
                text += " (" + percent + " %)";
            }

            return text;
        }

        protected String GetActiveText(Object oActiveFrom, Object oActiveTo)
        {
            DateTime activeFrom = (DateTime)oActiveFrom;
            DateTime activeTo = (DateTime)oActiveTo;

            activeFrom = activeFrom.AddHours(timeOffset);
            activeTo = activeTo.AddHours(timeOffset);

            String text = string.Empty;

            if (activeFrom.Year == activeTo.Year
                && activeFrom.Month == activeTo.Month
                && activeFrom.Day == activeTo.Day)
            {
                text = string.Format(CultureInfo.InvariantCulture,
                    PollResources.PollRunsFormatString,
                    activeFrom.ToShortDateString());
            }
            else
            {
                text = string.Format(CultureInfo.InvariantCulture,
                    PollResources.PollDateRangeFormatString,
                    activeFrom.ToShortDateString(),
                    activeTo.ToShortDateString());

            }

            return text;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            if (moduleSettings.Contains("ResultBarColor"))
            {
                resultBarColor = moduleSettings["ResultBarColor"].ToString();
            }

            AddClassToBody("pollhistory");
        }

        //protected String GetActiveText(Object oActiveFrom, Object oActiveTo)
        //{
        //    DateTime activeFrom = (DateTime)oActiveFrom;
        //    DateTime activeTo = (DateTime)oActiveTo;

        //    String text = PollResources.PollPollRunsText + " ";

        //    if (activeFrom.Date == activeTo.Date)
        //    {
        //        text += activeFrom.ToShortDateString();
        //    }
        //    else
        //    {
        //        text += PollResources.PollFromText + " " + activeFrom.ToShortDateString()
        //                        + " " + PollResources.PollToText + " " + activeTo.ToShortDateString();
        //    }

        //    return text;
        //}


    }
}
