using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Web.UI;

namespace mojoPortal.Web.UI
{
	public partial class PageSelectorSetting : UserControl, ISettingControl
	{
		PageSettings selectedPage = new();
		int selectedPageId = -1;
		protected void Page_Load(object sender, EventArgs e)
		{
			// this keeps the action from changing during ajax postback in folder based sites
			SiteUtils.SetFormAction(Page, Request.RawUrl);

			lnkPageIDSelector.Text = Resource.CustomMenuStartingPageBrowseLink;
			lnkPageIDSelector.ToolTip = Resource.CustomMenuStartingPageBrowseLinkTooltip;
			lnkPageIDSelector.NavigateUrl = WebUtils.GetSiteRoot() + "/Dialog/ParentPageDialog.aspx?pageid=-2";

			if (Page is mojoBasePage)
			{
				(Page as mojoBasePage).ScriptConfig.IncludeColorBox = true;
			}

			SetupScripts();

			if (!IsPostBack)
			{
				hdnPageId.Value = selectedPageId.ToInvariantString();
				if (selectedPageId == -1)
				{
					lblPageName.Text = "Root";
				}
				else
				{
					selectedPage = new PageSettings(CacheHelper.GetCurrentSiteSettings().SiteId, selectedPageId);
					lblPageName.Text = selectedPage.PageName;
				}
			}
		}

		private void SetupScripts()
		{
			string script = @$"
<script data-loader=""PageSelectorSetting"">
function SetPage(pageId, pageName) {{
    var hdnUI = document.getElementById(""{hdnPageId.ClientID}""); 
    hdnUI.value = pageId; 
    var lbl = document.getElementById(""{lblPageName.ClientID}"");  
    lbl.innerHTML = pageName; 
    $.colorbox.close(); 
}}
</script>";
			Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectPageHandler", script);
		}

		#region ISettingControl

		public string GetValue()
		{
			return hdnPageId.Value.ToString();
		}

		public void SetValue(string val)
		{
			if (!string.IsNullOrEmpty(val)) selectedPageId = Convert.ToInt32(val);
		}

		#endregion
	}
}