// Author:					
// Created:				    2013-01-21
// Last Modified:			2013-09-03
// 
// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)  
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Resources;

namespace mojoPortal.Web.UI
{
    public partial class SearchableFeatureFilterSetting : UserControl, ISettingControl
    {
        
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            EnsureItems();

            this.Load += new EventHandler(Page_Load);
            this.btnAddFeature.Click += btnAddFeature_Click;
            this.btnRemoveFeature.Click += btnRemoveFeature_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PopulateLabels();

        }

        private void btnAddFeature_Click(object sender, EventArgs e)
        {
            EnsureItems();
            BindSelectedItems();

            if (lstAllFeatures.SelectedIndex > -1)
            {
                int itemCount = lstAllFeatures.Items.Count;
                //foreach (ListItem item in lstAllFeatures.Items)
                for(int i = 0; i < itemCount; i++)
                {
                    ListItem item = lstAllFeatures.Items[i];
                    if (item.Selected)
                    {
                        item.Selected = false;
                        lstSelectedFeatures.Items.Add(item);
                        // can't do this inside a foreach it breaks the enumeration
                        lstAllFeatures.Items.Remove(item);

                        itemCount = lstAllFeatures.Items.Count;
                        
                    }

                }

                lstAllFeatures.ClearSelection();
                lstSelectedFeatures.ClearSelection();

                StoreSelectedGuids();
                upFeatures.Update();

            }
            //else
            //{
            //    lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToAddWarning;
            //}
        }

        private void btnRemoveFeature_Click(object sender, EventArgs e)
        {
            EnsureItems();
            BindSelectedItems();

            if (lstSelectedFeatures.SelectedIndex > -1)
            {
                int itemCount = lstSelectedFeatures.Items.Count;
                
                
                //foreach (ListItem item in lstSelectedFeatures.Items)
                for (int i = 0; i < itemCount; i++)
                {
                    ListItem item = lstSelectedFeatures.Items[i];
                    if (item.Selected)
                    {
                        item.Selected = false;
                        lstAllFeatures.Items.Add(item);
                        lstSelectedFeatures.Items.Remove(item);

                        itemCount = lstSelectedFeatures.Items.Count;
                    }

                }

                lstAllFeatures.ClearSelection();
                lstSelectedFeatures.ClearSelection();

                StoreSelectedGuids();
                upFeatures.Update();
            }
            //else
            //{
            //    //lblFeatureMessage.Text = Resource.SiteSettingsSelectFeatureToRemoveWarning;
            //}
        }

        private void PopulateLabels()
        {
            btnAddFeature.ToolTip = Resource.SiteSettingsAddFeatureTooltip;
            
            btnRemoveFeature.ToolTip = Resource.SiteSettingsRemoveFeatureTooltip;

            lblFeatureMessage.Text = string.Empty;
        }

        private void EnsureItems()
        {
            if (lstAllFeatures.Items.Count > 0) { return; }
            // if all items are selected the above can be 0
            // which would load them all again without this line below
            if (lstSelectedFeatures.Items.Count > 0) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            using (IDataReader reader = ModuleDefinition.GetSearchableModules(siteSettings.SiteId))
            {
                ListItem listItem;

                // this flag tells it to look first for a web config setting for the resource string
                // corresponding to SearchListName value
                // it allows you to customize searchlist names wheeas by default they are just localized
                bool useConfigOverrides = true;

                while (reader.Read())
                {
                    string featureid = reader["Guid"].ToString();

                    if (!WebConfigSettings.SearchableFeatureGuidsToExclude.Contains(featureid))
                    {
                        listItem = new ListItem(
                            ResourceHelper.GetResourceString(
                            reader["ResourceFile"].ToString(),
                            reader["SearchListName"].ToString(),
                            useConfigOverrides),
                            featureid);

                        lstAllFeatures.Items.Add(listItem);
                    }

                }

            }

        }

        private void StoreSelectedGuids()
        {
            if (lstSelectedFeatures.Items.Count == 0) { return; }

            featureGuidCsv.Value = string.Empty;
            string comma = string.Empty;
            foreach (ListItem item in lstSelectedFeatures.Items)
            {
                featureGuidCsv.Value += comma + item.Value;
                comma = ",";
            }

        }

        private void BindSelectedItems()
        {
            if (featureGuidCsv.Value.Length == 0) { return; }

            List<string> sGuids = featureGuidCsv.Value.SplitOnChar(',');

            foreach (string s in sGuids)
            {
                ListItem item = lstAllFeatures.Items.FindByValue(s);
                if (item != null)
                {
                    lstAllFeatures.Items.Remove(item);
                    lstSelectedFeatures.Items.Add(item);
                  
                }
            }

        }

        #region ISettingControl

        public string GetValue()
        {
            //EnsureItems();
           

            return featureGuidCsv.Value;
        }

        public void SetValue(string val)
        {
            
            featureGuidCsv.Value = val;
            EnsureItems();
            BindSelectedItems();
        }

        #endregion
    }
}