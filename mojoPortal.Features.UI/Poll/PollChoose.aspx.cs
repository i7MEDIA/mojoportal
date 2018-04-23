/// Author:                     Christian Fredh
/// Created:                    2007-07-21
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
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Web;
using mojoPortal.Business;
using PollFeature.Business;
using System.Web.UI;
using PollFeature.UI.Helpers;

namespace PollFeature.UI
{
    public partial class PollChoose : NonCmsBasePage
    {
        protected int pageId = -1;
        protected int moduleId = -1;
        protected Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private mojoPortal.Business.Module currentModule = null;
        private Poll currentPoll = null;
        private string resultBarColor = "#3082af";
        protected Hashtable moduleSettings;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            //this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnRemoveCurrent.Click += new EventHandler(btnRemoveCurrent_Click);
            this.dlPolls.ItemCommand += new DataListCommandEventHandler(dlPolls_ItemCommand);
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

            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            if (!UserCanEditModule(moduleId, Poll.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();
            PopulateControls();

        }

        private void btnRemoveCurrent_Click(object sender, EventArgs e)
        {
            Poll.RemoveFromModule(moduleId);
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void PopulateControls()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, PollResources.PollChooseLabel);

            if (currentModule == null) return;

            lnkPageCrumb.Text = currentModule.ModuleTitle;

            lnkPolls.Text = string.Format(CultureInfo.InvariantCulture,
                PollResources.ChooseActivePollFormatString,
                currentModule.ModuleTitle);

            heading.Text = lnkPolls.Text;


            if (IsPostBack) return;

            if(currentPoll != null)
            btnRemoveCurrent.Visible = currentPoll.PollGuid != Guid.Empty;


            using (IDataReader reader = Poll.GetPolls(siteSettings.SiteGuid))
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
            //reader.Close();

            foreach (DataListItem item in dlResults.Items)
            {
                Control spnResultImage = (Control)item.FindControl("spnResultImage");
                Guid optionGuid = new Guid(dlResults.DataKeys[item.ItemIndex].ToString());
                //PollOption option = new PollOption(optionGuid);
                PollOption option = pollOptions[item.ItemIndex];
                PollUIHelper.AddResultBarToControl(option, resultBarColor, spnResultImage);
            }

            // Only admin and content admin can edit poll content
            //HyperLink lnkEdit = (HyperLink)e.Item.FindControl("lnkEdit");
            //lnkEdit.Visible = WebUser.IsAdminOrContentAdmin;

            Button btnChoose = (Button)e.Item.FindControl("btnChoose");

            if (
                (currentPoll != null) 
                && (btnChoose.CommandArgument == currentPoll.PollGuid.ToString())
                &&(currentPoll.IsActive)
                )
            {
                btnChoose.Text = btnChoose.Text = PollResources.ActiveLink;
                
            }
            else
            {
                btnChoose.Text = PollResources.MakeActive;
                
            }

            
        }

        private void dlPolls_ItemCommand(object source, DataListCommandEventArgs e)
        {
            Poll poll;

            switch (e.CommandName)
            {
                

                case "Choose":

                    Guid pollGuid = new Guid(e.CommandArgument.ToString());
                    poll = new Poll(pollGuid);
                    poll.AddToModule(moduleId);

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    return;

                    //break;

                case "Copy":
                    poll = new Poll(new Guid(e.CommandArgument.ToString()));
                    Poll newPoll;
                    if (poll.CopyToNewPoll(out newPoll))
                    {
                        WebUtils.SetupRedirect(this, SiteRoot
                            + "/Poll/PollEdit.aspx?PollGuid="
                            + newPoll.PollGuid.ToString()
                            + "&pageid="
                            + pageId.ToString(CultureInfo.InvariantCulture)
                            + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture)
                            );

                        return;
                    }

                    break;

            }

            
        }

        

        private void PopulateLabels()
        {
            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkPolls.NavigateUrl = Request.RawUrl;

            btnRemoveCurrent.Text = PollResources.PollChooseRemoveCurrentPollButton;
            btnRemoveCurrent.ToolTip = PollResources.PollChooseRemoveCurrentPollToolTip;

            
            lnkNewPoll.Text = PollResources.PollChooseAddNewPollButton;
            lnkNewPoll.NavigateUrl = SiteRoot + "/Poll/PollEdit.aspx?pageid="
                + pageId.ToString(CultureInfo.InvariantCulture)
                + "&mid=" + moduleId.ToString(CultureInfo.InvariantCulture);

           

            
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
            moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

            if (moduleSettings.Contains("ResultBarColor"))
            {
                resultBarColor = moduleSettings["ResultBarColor"].ToString();
            }

            currentModule = GetModule(moduleId, Poll.FeatureGuid);
            currentPoll = new Poll(moduleId);

            AddClassToBody("pollchoose");
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

            if (timeZone != null)
            {
                activeFrom = activeFrom.ToLocalTime(timeZone);
                activeTo = activeTo.ToLocalTime(timeZone);
            }
            else
            {
                activeFrom = activeFrom.AddHours(timeOffset);
                activeTo = activeTo.AddHours(timeOffset);
            }

            //activeFrom = activeFrom.AddHours(timeOffset);
            //activeTo = activeTo.AddHours(timeOffset);
            
            String text = string.Empty;

            if (activeFrom.Year == activeTo.Year
                && activeFrom.Month == activeTo.Month
                && activeFrom.Day == activeTo.Day)
            {
                text = string.Format(CultureInfo.InvariantCulture,
                    PollResources.PollRunsFormatString,
                    activeFrom.ToString("g"));
            }
            else
            {
                text = string.Format(CultureInfo.InvariantCulture,
                    PollResources.PollDateRangeFormatString,
                    activeFrom.ToString("g"),
                    activeTo.ToString("g"));

            }

            return text;
        }
    }
}
