/// Author:                     Christian Fredh
/// Created:                    2007-07-25
///	Last Modified:              2018-03-28
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;
using mojoPortal.Web;
using PollFeature.Business;
using System.Web.UI;
using log4net;

namespace PollFeature.UI
{
    public partial class PollEdit : NonCmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PollEdit));

        private int pageId = -1;
        private int moduleId = -1;
        private mojoPortal.Business.Module currentModule = null;
        private Guid pollGuid = Guid.Empty;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        protected string EditContentImage = WebConfigSettings.EditContentImage;
        protected string DeleteLinkImage = WebConfigSettings.DeleteLinkImage;

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(Page_Load);
            this.btnSave.Click += new EventHandler(btnSave_Click);
            this.btnAddOption.Click += new EventHandler(btnAddOption_Click);
            this.cvOptionsLessThanTwo.ServerValidate += new ServerValidateEventHandler(cvOptionsLessThanTwo_ServerValidate);
            this.btnUp.Click += new ImageClickEventHandler(btnUp_Click);
            this.btnDown.Click += new ImageClickEventHandler(btnDown_Click);
            this.btnDeleteOption.Click += new ImageClickEventHandler(btnDeleteOption_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnEdit.Click += new ImageClickEventHandler(btnEdit_Click);
            this.btnActivateDeactivate.Click += new EventHandler(btnActivateDeactivate_Click);

            
            SuppressPageMenu();
            
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
        {
            
            SecurityHelper.DisableBrowserCache();
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

            if (!UserCanEditModule(moduleId, Poll.FeatureGuid))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (currentModule == null) return;

            lnkPageCrumb.Text = currentModule.ModuleTitle;

            lnkPolls.Text = string.Format(CultureInfo.InvariantCulture,
                PollResources.ChooseActivePollFormatString,
                currentModule.ModuleTitle);

            if (IsPostBack) return;
            if (Page.IsCallback) return;

            if (pollGuid == Guid.Empty)
            {
                btnDelete.Visible = false;
                btnActivateDeactivate.Visible = false;
                btnAddNewPoll.Visible = false;
                if (timeZone != null)
                {
                    dpActiveFrom.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.ToLocalTime(timeZone).ToString("g"));
                    dpActiveTo.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddYears(1).ToLocalTime(timeZone).ToString("g"));
                }
                else
                {
                    dpActiveFrom.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddHours(timeOffset).ToString("g"));
                    dpActiveTo.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddYears(1).AddHours(timeOffset).ToString("g"));
                }

     
                return;
            }

            lblStartDeactivated.Visible = false;
            chkStartDeactivated.Visible = false;

            Poll poll = new Poll(pollGuid);

            txtQuestion.Text = poll.Question;
            chkAllowViewingResultsBeforeVoting.Checked = poll.AllowViewingResultsBeforeVoting;
            chkAnonymousVoting.Checked = poll.AnonymousVoting;
            chkShowOrderNumbers.Checked = poll.ShowOrderNumbers;
            chkShowResultsWhenDeactivated.Checked = poll.ShowResultsWhenDeactivated;

            if (timeZone != null)
            {
                dpActiveFrom.Text = DateTimeHelper.LocalizeToCalendar(poll.ActiveFrom.ToLocalTime(timeZone).ToString("g"));
                dpActiveTo.Text = DateTimeHelper.LocalizeToCalendar(poll.ActiveTo.ToLocalTime(timeZone).ToString("g"));
            }
            else
            {
                dpActiveFrom.Text = DateTimeHelper.LocalizeToCalendar(poll.ActiveFrom.AddHours(timeOffset).ToString("g"));
                dpActiveTo.Text = DateTimeHelper.LocalizeToCalendar(poll.ActiveTo.AddHours(timeOffset).ToString("g"));
            }

            List<PollOption> pollOptions = PollOption.GetOptionsByPollGuid(pollGuid);
            ListItem li;
            foreach(PollOption option in pollOptions)
            {
                li = new ListItem(option.Answer, option.OptionGuid.ToString());
                lbOptions.Items.Add(li);
            }


            if (poll.Activated)
            {
                btnActivateDeactivate.Text = PollResources.PollEditDeactivateButton;
                btnActivateDeactivate.ToolTip = PollResources.PollEditDeactivateToolTip;
                btnActivateDeactivate.CommandName = "Deactivate";
            }
            else
            {
                btnActivateDeactivate.Text = PollResources.PollEditActivateButton;
                btnActivateDeactivate.ToolTip = PollResources.PollEditActivateToolTip;
                btnActivateDeactivate.CommandName = "Activate";
            }
            btnActivateDeactivate.CommandArgument = poll.PollGuid.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Page.Validate("poll");
            if (Page.IsValid)
            {
                Poll poll = new Poll(pollGuid);
                poll.SiteGuid = siteSettings.SiteGuid;
                poll.Question = txtQuestion.Text;
                poll.AnonymousVoting = chkAnonymousVoting.Checked;
                poll.AllowViewingResultsBeforeVoting = chkAllowViewingResultsBeforeVoting.Checked;
                poll.ShowOrderNumbers = chkShowOrderNumbers.Checked;
                poll.ShowResultsWhenDeactivated = chkShowResultsWhenDeactivated.Checked;

                if (dpActiveFrom.Text.Length > 0 && poll.ActiveFrom >= DateTime.UtcNow)
                {
                    // You can't change date if poll has started.

                    // TODO: promt user if invalid format/date
                    
                    DateTime activeFrom;
                    DateTime.TryParse(dpActiveFrom.Text, out activeFrom);

                    if (timeZone != null)
                    {
                        activeFrom = activeFrom.ToUtc(timeZone);
                    }
                    else
                    {
                        activeFrom = activeFrom.AddHours(-timeOffset);
                    }

                    poll.ActiveFrom = activeFrom;
                }

                if (dpActiveTo.Text.Length > 0)
                {
                    // TODO: promt user if invalid format/date
                    DateTime activeTo;
                    DateTime.TryParse(dpActiveTo.Text, out activeTo);

                    if (timeZone != null)
                    {
                        activeTo = activeTo.ToUtc(timeZone);
                    }
                    else
                    {
                        activeTo = activeTo.AddHours(-timeOffset);
                    }

                    // Make time 23:59:59
                    //activeTo = activeTo.AddHours(23).AddMinutes(59).AddSeconds(59);

                    // You can't change to past date.
                    if (activeTo >= DateTime.UtcNow)
                    {
                        poll.ActiveTo = activeTo;
                    }
                }

                if (chkStartDeactivated.Checked)
                {
                    // This only happens when new poll.
                    poll.Deactivate();
                }
                else
                {
                    poll.Activate();
                }

                poll.Save();

                // Get options
                PollOption option;
                int order = 1;
                foreach (ListItem item in lbOptions.Items)
                {
                    if (item.Text == item.Value)
                    {
                        option = new PollOption();
                    }
                    else
                    {
                        if (item.Value.Length == 36)
                        {
                            option = new PollOption(new Guid(item.Value));
                        }
                        else
                        {
                            option = new PollOption();
                        }
                    }

                    option.PollGuid = poll.PollGuid;
                    option.Answer = item.Text;
                    option.Order = order++;
                    option.Save();
                }


                WebUtils.SetupRedirect(this, 
                    SiteRoot + "/Poll/PollChoose.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    );

            }
        }



        private void btnActivateDeactivate_Click(object sender, EventArgs e)
        {
            Poll poll = new Poll(new Guid(btnActivateDeactivate.CommandArgument));
            if (btnActivateDeactivate.CommandName == "Activate")
            {
                poll.Activate();
                poll.Save();
                WebUtils.SetupRedirect(this, 
                    SiteRoot + "/Poll/PollEdit.aspx?PollGuid=" + pollGuid.ToString()
                    + "&pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    );
            }
            else if (btnActivateDeactivate.CommandName == "Deactivate")
            {
                poll.Deactivate();
                poll.Save();
                WebUtils.SetupRedirect(this, 
                    SiteRoot + "/Poll/PollEdit.aspx?PollGuid=" + pollGuid.ToString()
                    + "&pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    );
            }
        }

        private void btnEdit_Click(object sender, ImageClickEventArgs e)
        {
            if (lbOptions.SelectedItem == null) return;

            String itemText = lbOptions.SelectedItem.Text;
            String itemValue = lbOptions.SelectedItem.Value;

            txtNewOption.Text = itemText;
            btnAddOption.CommandArgument = itemValue;
            btnAddOption.Text = PollResources.PollEditOptionsSaveButton;
        }

        private void btnAddOption_Click(object sender, EventArgs e)
        {
            txtNewOption.Text = txtNewOption.Text.Trim();

            if (txtNewOption.Text.Length == 0) return;

            if (String.IsNullOrEmpty(btnAddOption.CommandArgument))
            {
                if (lbOptions.Items.FindByText(txtNewOption.Text) != null) return;

                ListItem li = new ListItem(txtNewOption.Text);
                lbOptions.Items.Add(li);
            }
            else
            {
                ListItem itemToEdit = lbOptions.Items.FindByValue(btnAddOption.CommandArgument);
                if (itemToEdit != null)
                {
                    itemToEdit.Text = txtNewOption.Text;
                }
                btnAddOption.CommandArgument = null;
                btnAddOption.Text = PollResources.PollEditOptionsAddButton;
            }

            txtNewOption.Text = String.Empty;
        }

        private void cvOptionsLessThanTwo_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (lbOptions.Items.Count >= 2);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (pollGuid != Guid.Empty)
            {
                Poll poll = new Poll(pollGuid);
                poll.Delete();

                WebUtils.SetupRedirect(this, 
                    SiteRoot + "/Poll/PollChoose.aspx"
                    + "?pageid=" + pageId.ToInvariantString()
                    + "&mid=" + moduleId.ToInvariantString()
                    );
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, PollResources.PollEditLabel);
            heading.Text = PollResources.PollEditLabel;
            if (pollGuid == Guid.Empty)
            {
                heading.Text = PollResources.PollAddNewLabel;
            }

            lnkPageCrumb.Text = CurrentPage.PageName;
            lnkPageCrumb.NavigateUrl = SiteUtils.GetCurrentPageUrl();

            lnkPolls.NavigateUrl = SiteRoot + "/Poll/PollChoose.aspx?pageid="
                + pageId.ToInvariantString()
                + "&mid=" + moduleId.ToInvariantString();

            btnAddNewPoll.Visible = false;
            //btnAddNewPoll.Text = PollResources.PollEditAddNewPollLabel;
            //btnAddNewPoll.ToolTip = PollResources.PollEditAddNewPollToolTip;

            btnViewPolls.Visible = false;
            //btnViewPolls.Text = PollResources.PollEditViewPollsLabel;
            //btnViewPolls.ToolTip = PollResources.PollEditViewPollsToolTip;

            btnSave.Text = PollResources.PollEditSaveButton;
            btnSave.ToolTip = PollResources.PollEditSaveToolTip;

            btnAddOption.Text = PollResources.PollEditOptionsAddButton;
            btnAddOption.ToolTip = PollResources.PollEditOptionsAddToolTip;

            btnDelete.Text = PollResources.PollEditDeleteButton;
            btnDelete.ToolTip = PollResources.PollEditDeleteToolTip;
            UIHelper.AddConfirmationDialog(btnDelete, PollResources.PollEditDeleteConfirmMessage);

            btnUp.AlternateText = PollResources.PollEditOptionsUpAlternateText;
            btnUp.ToolTip = PollResources.PollEditOptionsUpAlternateText;
            btnUp.ImageUrl = ImageSiteRoot + "/Data/SiteImages/up.png";

            btnDown.AlternateText = PollResources.PollEditOptionsDownAlternateText;
            btnDown.ToolTip = PollResources.PollEditOptionsDownAlternateText;
            btnDown.ImageUrl = ImageSiteRoot + "/Data/SiteImages/down.png";

            btnEdit.AlternateText = PollResources.PollEditOptionsEditAlternateText;
            btnEdit.ToolTip = PollResources.PollEditOptionsEditAlternateText;
            btnEdit.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + EditContentImage;

            btnDeleteOption.AlternateText = PollResources.PollEditOptionsDeleteAlternateText;
            btnDeleteOption.ToolTip = PollResources.PollEditOptionsDeleteAlternateText;
            btnDeleteOption.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + DeleteLinkImage;
            UIHelper.AddConfirmationDialog(btnDeleteOption, PollResources.PollEditOptionsDeleteConfirmMessage);

            chkAllowViewingResultsBeforeVoting.ToolTip = PollResources.PollEditAllowViewingResultsBeforeVotingToolTip;
            chkAnonymousVoting.ToolTip = PollResources.PollEditAnonymousVotingToolTip;
            chkShowOrderNumbers.ToolTip = PollResources.PollEditShowOrderNumbersToolTip;
            chkShowResultsWhenDeactivated.ToolTip = PollResources.PollEditShowResultsWhenDeactivatedToolTip;
            chkStartDeactivated.ToolTip = PollResources.PollEditStartDeactivatedToolTip;

            cvOptionsLessThanTwo.ErrorMessage = PollResources.PollEditLessThanTwoOptionsErrorMesssage;
            reqQuestion.ErrorMessage = PollResources.PollEditQuestionEmptyErrorMessage;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pollGuid = WebUtils.ParseGuidFromQueryString("PollGuid", Guid.Empty);

            pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);


            currentModule = GetModule(moduleId, Poll.FeatureGuid);

            if (!IsPostBack)
            {
                AddClassToBody("polledit");
            }
        }

        private void btnDown_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbOptions.SelectedItem == null) return;
            if (lbOptions.SelectedIndex == lbOptions.Items.Count - 1) return;

            ListItem selectedItem = lbOptions.SelectedItem;
            ListItem swapItem = lbOptions.Items[lbOptions.SelectedIndex + 1];

            String tmpText = selectedItem.Text;
            String tmpValue = selectedItem.Value;

            selectedItem.Text = swapItem.Text;
            selectedItem.Value = swapItem.Value;

            swapItem.Text = tmpText;
            swapItem.Value = tmpValue;

            lbOptions.SelectedIndex++;
        }

        private void btnUp_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbOptions.SelectedItem == null) return;
            if (lbOptions.SelectedIndex == 0) return;

            ListItem selectedItem = lbOptions.SelectedItem;
            ListItem swapItem = lbOptions.Items[lbOptions.SelectedIndex - 1];

            String tmpText = selectedItem.Text;
            String tmpValue = selectedItem.Value;

            selectedItem.Text = swapItem.Text;
            selectedItem.Value = swapItem.Value;

            swapItem.Text = tmpText;
            swapItem.Value = tmpValue;

            lbOptions.SelectedIndex--;
        }

        private void btnDeleteOption_Click(Object sender, ImageClickEventArgs e)
        {
            if (lbOptions.SelectedItem == null) return;

            // TODO: What should happen if user deletes options so only 0 or 1 are left,
            // and then presses cancel or something? Better if nothing happens before 
            // pressing Save but how to store deleted options for later?

            // If Text == Value option hasn't been saved yet, just remove it from list.
            if (lbOptions.SelectedItem.Text != lbOptions.SelectedItem.Value)
            {
                Guid optionGuid = new Guid(lbOptions.SelectedItem.Value);
                PollOption option = new PollOption(optionGuid);
                option.Delete();
            }

            lbOptions.Items.Remove(lbOptions.SelectedItem);
        }
    }
}
