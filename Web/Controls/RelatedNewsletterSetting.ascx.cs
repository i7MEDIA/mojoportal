// Author:					
// Created:				    2013-09-02
// Last Modified:			2017-08-23
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
using System.Web.UI;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.UI
{
	public partial class RelatedNewsletterSetting : UserControl, ISettingControl
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EnsureItems();

            this.Load += new EventHandler(Page_Load);
            this.btnAdd.Click += btnAdd_Click;
            this.btnRemove.Click += btnRemove_Click;
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            EnsureItems();
            BindSelectedItems();

            if (lstAll.SelectedIndex > -1)
            {
                int itemCount = lstAll.Items.Count;
                
                for (int i = 0; i < itemCount; i++)
                {
                    ListItem item = lstAll.Items[i];
                    if (item.Selected)
                    {
                        item.Selected = false;
                        
                        // can't do this inside a foreach it breaks the enumeration
                        lstAll.Items.Remove(item);
                        lstSelected.Items.Add(item);

                        itemCount = lstAll.Items.Count;

                    }

                }

                lstAll.ClearSelection();
                lstSelected.ClearSelection();

                StoreSelectedGuids();
                up.Update();

            }
           
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            EnsureItems();
            BindSelectedItems();

            if (lstSelected.SelectedIndex > -1)
            {
                int itemCount = lstSelected.Items.Count;


               
                for (int i = 0; i < itemCount; i++)
                {
                    ListItem item = lstSelected.Items[i];
                    if (item.Selected)
                    {
                        item.Selected = false;
                        
                        lstSelected.Items.Remove(item);
                        lstAll.Items.Add(item);

                        itemCount = lstSelected.Items.Count;
                    }

                }

                lstAll.ClearSelection();
                lstSelected.ClearSelection();

                StoreSelectedGuids();
                up.Update();
            }
           
        }

        private void EnsureItems()
        {
            if (lstAll.Items.Count > 0) { return; }
            // if all items are selected the above can be 0
            // which would load them all again without this line below
            if (lstSelected.Items.Count > 0) { return; }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }

            List<LetterInfo> availableNewsletters = LetterInfo.GetAll(siteSettings.SiteGuid);
            ListItem listItem;

            foreach (LetterInfo newsletter in availableNewsletters)
            {
                //if (newsletter.AvailableToRoles.Contains("All Users"))
                //{
                    listItem = new ListItem(newsletter.Title, newsletter.LetterInfoGuid.ToString());

                    lstAll.Items.Add(listItem);

               // }

            }

           

        }

        private void StoreSelectedGuids()
        {
            if (lstSelected.Items.Count == 0) { return; }

            GuidCsv.Value = string.Empty;
            string comma = string.Empty;
            foreach (ListItem item in lstSelected.Items)
            {
                GuidCsv.Value += comma + item.Value;
                comma = ",";
            }

        }

        private void BindSelectedItems()
        {
            if (GuidCsv.Value.Length == 0) { return; }

            List<string> sGuids = GuidCsv.Value.SplitOnChar(',');

            foreach (string s in sGuids)
            {
                ListItem item = lstAll.Items.FindByValue(s);
                if (item != null)
                {
                    lstAll.Items.Remove(item);
                    lstSelected.Items.Add(item);

                }
            }

        }

        #region ISettingControl

        public string GetValue()
        {
            //EnsureItems();


            return GuidCsv.Value;
        }

        public void SetValue(string val)
        {

            GuidCsv.Value = val;
            EnsureItems();
            BindSelectedItems();
        }

        #endregion
    }

}