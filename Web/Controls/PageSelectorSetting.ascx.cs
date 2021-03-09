using System;
using System.Text;
using System.Web.UI;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI
{
	public partial class PageSelectorSetting : UserControl, ISettingControl
    {
        PageSettings selectedPage = new PageSettings();
        int selectedPageId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            lnkPageIDSelector.Text = Resource.CustomMenuStartingPageBrowseLink;
            lnkPageIDSelector.ToolTip = Resource.CustomMenuStartingPageBrowseLinkTooltip;
            lnkPageIDSelector.NavigateUrl = WebUtils.GetSiteRoot() + "/Dialog/ParentPageDialog.aspx?pageid=-2";

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


            StringBuilder script = new StringBuilder();

            script.Append("\n<script type='text/javascript'>");

            script.Append("function SetPage(pageId, pageName) {");

            // script.Append("GB_hide();");


            script.Append("var hdnUI = document.getElementById('" + hdnPageId.ClientID + "'); ");
            script.Append("hdnUI.value = pageId; ");



            script.Append("var lbl = document.getElementById('" + lblPageName.ClientID + "');  ");
            script.Append("lbl.innerHTML = pageName; ");

            //script.Append("alert(pageName);");
            script.Append("$.colorbox.close(); ");

            script.Append("}");
            script.Append("</script>");


            Page.ClientScript.RegisterStartupScript(typeof(Page), "SelectPageHandler", script.ToString());

        }

        #region ISettingControl

        public string GetValue()
        {
            return hdnPageId.Value.ToString();
        }

        public void SetValue(string val)
        {
            if (!string.IsNullOrEmpty(val)) selectedPageId = Convert.ToInt32(val);
            //string[] args = val.Split('|');
            //if (args.Length < 2) { return; }

            //setCountry = args[0];
            //setState = args[1];

            //BindCountryList();

            //ListItem item = ddCountry.Items.FindByValue(setCountry);
            //if (item != null)
            //{
            //    ddCountry.ClearSelection();
            //    item.Selected = true;
            //}

            //BindGeoZoneList();
            //item = ddGeoZone.Items.FindByValue(setState);
            //if (item != null)
            //{
            //    ddGeoZone.ClearSelection();
            //    item.Selected = true;
            //}

            //UpdatePanel1.Update();
        }

        #endregion
    }
}