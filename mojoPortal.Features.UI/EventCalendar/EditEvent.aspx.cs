/// Author:				        
/// Created:			        2005-04-10
///	Last Modified:              2013-01-15
/// 
/// The use and distribution terms for this software are covered by the 
/// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
/// which can be found in the file CPL.TXT at the root of this distribution.
/// By using this software in any fashion, you are agreeing to be bound by 
/// the terms of this license.
///
/// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Editor;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.EventCalendarUI
{

    public partial class EventCalendarEdit : NonCmsBasePage
	{
		private int moduleId = -1;
        private int itemId = -1;
        //private String cacheDependencyKey;
        private DateTime thisDay = DateTime.Today;
        private string virtualRoot;
		private bool enableMap = true;
        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
            
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            //this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            base.OnInit(e);

            SiteUtils.SetupEditor(edContent, AllowSkinOverride, this);
            edContent.WebEditor.ToolBar = ToolBar.Full;
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

            if (!UserCanEditModule(moduleId, CalendarEvent.FeatureGuid))
			{
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (SiteUtils.IsFishyPost(this))
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

			PopulateLabels();

			if (!IsPostBack) 
			{
				PopulateControls();

                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                    lnkCancel.NavigateUrl = hdnReturnUrl.Value;
                }
			}
			
		}

		private void PopulateControls()
		{
            if (itemId > -1)
            {
                CalendarEvent calEvent = new CalendarEvent(itemId);

                if (calEvent.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                this.txtTitle.Text = calEvent.Title;
                edContent.Text = calEvent.Description;
                this.dpEventDate.Text = calEvent.EventDate.ToShortDateString();

                ListItem item;
                item = ddStartTime.Items.FindByValue(calEvent.StartTime.ToShortTimeString());
                if (item != null)
                {
                    ddStartTime.ClearSelection();
                    item.Selected = true;
                }

                item = ddEndTime.Items.FindByValue(calEvent.EndTime.ToShortTimeString());
                if (item != null)
                {
                    ddEndTime.ClearSelection();
                    item.Selected = true;
                }

                txtLocation.Text = calEvent.Location;

				lblShowMap.Visible = calEvent.ShowMap;
				chkShowMap.Visible = calEvent.ShowMap;
				chkShowMap.Checked = calEvent.ShowMap;
            }
            else
            {
                btnDelete.Visible = false;
            }
			
		}

		private void PopulateLabels()
		{

            Title = SiteUtils.FormatPageTitle(siteSettings, EventCalResources.EditEventPageTitle);

            heading.Text = EventCalResources.EventCalendarEditEntryLabel;

            btnUpdate.Text = EventCalResources.EventCalendarEditUpdateButton;
            SiteUtils.SetButtonAccessKey(btnUpdate, EventCalResources.EventCalendarEditUpdateButtonAccessKey);

            ScriptConfig.EnableExitPromptForUnsavedContent = true;

            UIHelper.DisableButtonAfterClickAndClearExitCode(
                btnUpdate,
                EventCalResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            lnkCancel.Text = EventCalResources.EventCalendarEditCancelButton;
            //btnCancel.Text = EventCalResources.EventCalendarEditCancelButton;
            //SiteUtils.SetButtonAccessKey(btnCancel, EventCalResources.EventCalendarEditCancelButtonAccessKey);

            btnDelete.Text = EventCalResources.EventCalendarEditDeleteButton;
            SiteUtils.SetButtonAccessKey(btnDelete, EventCalResources.EventCalendarEditDeleteButtonAccessKey);
            UIHelper.AddConfirmationDialogWithClearExitCode(btnDelete, EventCalResources.EventCalendarDeleteEventWarning);

            
		}


        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
        }


	    private void btnDelete_Click(object sender, EventArgs e)
		{
			if(itemId > -1)
			{
                CalendarEvent calendarEvent = new CalendarEvent(itemId);
                if (calendarEvent.ModuleId != moduleId)
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                calendarEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);
                

                calendarEvent.Delete();
                CurrentPage.UpdateLastModifiedTime();
                SiteUtils.QueueIndexing();
			}

            if (hdnReturnUrl.Value.Length > 0)
            {
                WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                return;
            }

            WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());

        }

        void calendarEvent_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["CalendarEventIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }
        

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Page.Validate("eventcalendaredit");
			if (Page.IsValid) 
			{
				int userId = -1;
                Guid userGuid = Guid.Empty;
				if(Request.IsAuthenticated)
				{
					//SiteUser siteUser = new SiteUser(siteSettings, Context.User.Identity.Name);
                    SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                    userId = siteUser.UserId;
                    userGuid = siteUser.UserGuid;
				}
				CalendarEvent calEvent = new CalendarEvent(itemId);

                if ((itemId > -1)&&(calEvent.ModuleId != moduleId))
                {
                    SiteUtils.RedirectToAccessDeniedPage(this);
                    return;
                }

                calEvent.ContentChanged += new ContentChangedEventHandler(calendarEvent_ContentChanged);
                
                Module m = new Module(this.moduleId);
                calEvent.ModuleId = m.ModuleId;
                calEvent.ModuleGuid = m.ModuleGuid;
				calEvent.UserId = userId;
                calEvent.UserGuid = userGuid;
                calEvent.LastModUserGuid = userGuid;
				calEvent.Title = this.txtTitle.Text;
                calEvent.Description = edContent.Text;
				calEvent.EventDate = DateTime.Parse(this.dpEventDate.Text);
				calEvent.StartTime = DateTime.Parse(this.ddStartTime.SelectedValue);
				calEvent.EndTime = DateTime.Parse(this.ddEndTime.SelectedValue);
                calEvent.Location = txtLocation.Text;

               

				if(calEvent.Save())
				{
                    CurrentPage.UpdateLastModifiedTime();
                    //CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
                    CacheHelper.ClearModuleCache(moduleId);

                    SiteUtils.QueueIndexing();

                    if (hdnReturnUrl.Value.Length > 0)
                    {
                        WebUtils.SetupRedirect(this, hdnReturnUrl.Value);
                        return;
                    }

                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl());
         
				}

			}

		}


        private void LoadSettings()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
            itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
            thisDay = WebUtils.ParseDateFromQueryString("date", DateTime.Today);
            //cacheDependencyKey = "Module-" + moduleId.ToString();
            
            lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
            
            if (!Page.IsPostBack)
            {
                dpEventDate.Text = thisDay.ToShortDateString();
            }

            DateTime tomorrow = thisDay.AddDays(1).AddMinutes(-15);
            ddStartTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
            ddEndTime.Items.Insert(0, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
            int i = 0;
            while (thisDay < tomorrow)
            {
                i += 1;
                thisDay = thisDay.AddMinutes(15);

                ddStartTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));
                ddEndTime.Items.Insert(i, new ListItem(thisDay.ToShortTimeString(), thisDay.ToShortTimeString()));

            }

            AddClassToBody("eventcaledit");
        }

		
	}
}
