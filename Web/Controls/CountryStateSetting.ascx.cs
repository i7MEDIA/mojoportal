using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business;

namespace mojoPortal.Web.UI
{
	public partial class CountryStateSetting : UserControl, ISettingControl
    {
        protected DataTable tblCountryList = null;
        private Guid currentCountryGuid = Guid.Empty;
        protected GeoCountry currentCountry;
        private string defaultCountry = WebConfigSettings.DefaultCountry;
        private string setCountry = string.Empty;
        private string setState = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            EnsureControls();
            ddCountry.SelectedIndexChanged += new EventHandler(ddCountry_SelectedIndexChanged);
        }

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

            if (!Page.IsPostBack)
            {
                BindCountryList();
                BindGeoZoneList();
            }
        }

        private void BindCountryList()
        {
            if (tblCountryList == null)
            {
                tblCountryList = GeoCountry.GetList();
            }

            if (tblCountryList != null)
            {
                ddCountry.DataSource = tblCountryList;
                ddCountry.DataBind();
            }

            ListItem item;

            if (setCountry.Length > 0)
            {
                item = ddCountry.Items.FindByValue(setCountry);
            }
            else
            {
                item = ddCountry.Items.FindByValue(defaultCountry);
            }

            if (item != null)
            {
                ddCountry.ClearSelection();
                item.Selected = true;
            }

        }

        private void BindGeoZoneList()
        {
            if ((ddCountry.SelectedIndex > -1)
                && (ddCountry.SelectedValue.Length > 0)
                )
            {
                currentCountry = new GeoCountry(ddCountry.SelectedValue);
                currentCountryGuid = currentCountry.Guid;

                using (IDataReader reader = GeoZone.GetByCountry(currentCountryGuid))
                {
                    ddGeoZone.DataSource = reader;
                    ddGeoZone.DataBind();
                }

                if (setState.Length > 0)
                {
                    ListItem item = ddGeoZone.Items.FindByValue(setState);
                    if (item != null)
                    {
                        ddGeoZone.ClearSelection();
                        item.Selected = true;
                    }

                }

            }
        }

        private void ddCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGeoZoneList();
            UpdatePanel1.Update();
        }

        private void EnsureControls()
        {
            if (ddCountry == null)
            {
                ddCountry = new DropDownList();
                ddCountry.DataValueField = "ISOCode2";
                ddCountry.DataTextField = "Name";

                if (this.Controls.Count == 0) { this.Controls.Add(ddCountry); }
            }

            if (ddGeoZone == null)
            {
                ddGeoZone = new DropDownList();
                ddGeoZone.DataValueField = "Code";
                ddGeoZone.DataTextField = "Name";

                if (this.Controls.Count == 1) { this.Controls.Add(ddGeoZone); }
            }

           


        }


        #region ISettingControl

        public string GetValue()
        {
            return ddCountry.SelectedValue + "|" + ddGeoZone.SelectedValue;
        }

        /// <summary>
        /// Expects pipe separated pair Country|State like US|TN
        /// </summary>
        /// <param name="val"></param>
        public void SetValue(string val)
        {
            string[] args = val.Split('|');
            if (args.Length < 2) { return; }

            setCountry = args[0];
            setState = args[1];

            BindCountryList();

            ListItem item = ddCountry.Items.FindByValue(setCountry);
            if (item != null)
            {
                ddCountry.ClearSelection();
                item.Selected = true;
            }

            BindGeoZoneList();
            item = ddGeoZone.Items.FindByValue(setState);
            if (item != null)
            {
                ddGeoZone.ClearSelection();
                item.Selected = true;
            }

            UpdatePanel1.Update();
        }

        #endregion

    }
}