// Author:
// Created:       2010-01-14
// Last Modified: 2017-08-30
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using Resources;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.ContactUI
{
	public partial class MessageListDialog : mojoDialogBasePage, ICallbackEventHandler
	{
		private int totalPages = 1;
		private int pageNumber = 1;
		private int pageSize = 4;
		private Guid moduleGuid = Guid.Empty;
		private int moduleId = -1;
		private int pageId = -1;
		protected string sCallBackFunctionInvocation;
		private string callbackArg = string.Empty;
		private Hashtable moduleSettings;
		private Double timeOffset = 0;
		private TimeZoneInfo timeZone = null;


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

			if (!UserCanEditModule(moduleId, ContactFormMessage.FeatureGuid))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			PopulateLabels();
			PopulateControls();
		}


		private void PopulateControls()
		{
			if (Page.IsPostBack)
			{
				return;
			}

			BindGrid();
		}


		private void BindGrid()
		{
			if (moduleGuid == Guid.Empty)
			{
				return;
			}

			using (IDataReader reader = ContactFormMessage.GetPageReader(
				moduleGuid,
				pageNumber,
				pageSize,
				out totalPages))
			{

				string pageUrl = SiteRoot + "/ContactForm/MessageListDialog.aspx"
						+ "?pageid=" + pageId.ToInvariantString()
						+ "&amp;mid=" + moduleId.ToInvariantString()
						+ "&amp;pagenumber={0}";

				pgrContactFormMessage.PageURLFormat = pageUrl;
				pgrContactFormMessage.ShowFirstLast = true;
				pgrContactFormMessage.CurrentIndex = pageNumber;
				pgrContactFormMessage.PageSize = pageSize;
				pgrContactFormMessage.PageCount = totalPages;
				grdContactFormMessage.PageIndex = pageNumber;

				pgrContactFormMessage.Visible = (totalPages > 1);

				grdContactFormMessage.DataSource = reader;
				grdContactFormMessage.DataBind();
			}
		}


		void grdContactFormMessage_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			string arg = e.CommandArgument.ToString();

			if (arg.Length != 36)
			{
				return;
			}

			Guid messageGuid = new Guid(arg);

			switch (e.CommandName)
			{
				case "remove":
					ContactFormMessage.Delete(messageGuid);
					WebUtils.SetupRedirect(this, Request.RawUrl);
					break;

				case "view":
				default:
					ContactFormMessage message = new ContactFormMessage(messageGuid);
					litMessage.Text = SecurityHelper.SanitizeHtml(message.Message);
					//upMessage.Update();
					break;
			}
		}


		public string GetViewOnClick(string arg)
		{
			return "GetMessage('" + arg + "',''); return false;";
		}


		public string GetDeleteOnClick(string arg)
		{
			return "return confirm('" + ContactFormResources.ContactFormConfirmDeleteMessage + "');";
		}


		public void RaiseCallbackEvent(string eventArgument)
		{
			callbackArg = eventArgument;
		}


		public string GetCallbackResult()
		{
			if (callbackArg.Length != 36)
			{
				return string.Empty;
			}

			Guid messageGuid = new Guid(callbackArg);
			ContactFormMessage message = new ContactFormMessage(messageGuid);
			return SecurityHelper.SanitizeHtml(message.Message);
		}


		private void PopulateLabels()
		{
			Title = ContactFormResources.ContactFormViewMessagesLink;

			Control c = Master.FindControl("Breadcrumbs");

			if (c != null)
			{
				BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
				crumbs.ForceShowBreadcrumbs = true;

			}

			lnkRefresh.NavigateUrl = Request.RawUrl;
			lnkRefresh.Text = ContactFormResources.ContactFormMessageListRefreshLink;

			grdContactFormMessage.Columns[0].HeaderText = ContactFormResources.ContactFormMessageListFromHeader;
		}


		protected string FormatDate(DateTime theDate)
		{
			if (timeZone != null)
			{
				return TimeZoneInfo.ConvertTimeFromUtc(theDate, timeZone).ToString();

			}

			return theDate.AddHours(timeOffset).ToString();
		}


		//private void SetupScript()
		//{
		//	StringBuilder script = new StringBuilder();

		//	script.Append("\n<script type='text/javascript'>");
		//	script.Append("var myLayout; ");
		//	script.Append("$(document).ready(function () {");

		//	script.Append("myLayout = $('body').layout({  ");
		//	script.Append("applyDefaultStyles: true");
		//	script.Append(",west__paneSelector:'#" + pnlLeft.ClientID + "'");
		//	script.Append(",center__paneSelector:'#" + pnlCenter.ClientID + "'");
		//	script.Append(",west__size: 420 ");

		//	script.Append("});");
		//	script.Append("});");

		//	script.Append("\n</script>");

		//	Page.ClientScript.RegisterStartupScript(typeof(Page), "cmessages", script.ToString());
		//}


		private void LoadSettings()
		{
			pageId = WebUtils.ParseInt32FromQueryString("pageid", pageId);
			moduleId = WebUtils.ParseInt32FromQueryString("mid", moduleId);
			pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);
			timeOffset = SiteUtils.GetUserTimeOffset();
			timeZone = SiteUtils.GetUserTimeZone();
			//var headControls = Parent.FindControl("phHead");
			//todo: add control to layout.master to allow adding head elements to other pages.
			//headControls.Controls.Add(litHead);
			if (moduleId > -1)
			{
				Module m = GetModule(moduleId, ContactFormMessage.FeatureGuid);

				if (m == null)
				{
					SiteUtils.RedirectToAccessDeniedPage(this);
					return;
				}

				moduleGuid = m.ModuleGuid;
				moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
				pageSize = WebUtils.ParseInt32FromHashtable(moduleSettings, "ContactFormMessageListPageSizeSetting", pageSize);
			}

			sCallBackFunctionInvocation = ClientScript.GetCallbackEventReference(this, "messageGuid", "ShowMessage", "context", "OnError", true);

			//ScriptLoader sl = Page.Master.FindControl("ScriptInclude") as ScriptLoader;

			//if (sl != null)
			//{
			//	sl.IncludejQueryLayout = true;
			//}

			//SetupScript();
		}

		
		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			grdContactFormMessage.RowCommand += new GridViewCommandEventHandler(grdContactFormMessage_RowCommand);
			ScriptConfig.IncludeJQTable = true;
		}
	}
}