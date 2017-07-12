// Author:					i7MEDIA (Joe Davis)
// Created:					2015-01-06
// Last Modified:			2016-09-08

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
namespace SuperFlexiUI
{
    public partial class MarkupDefinitionSetting : mojoUserControl, mojoPortal.Web.UI.ISettingControl
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(MarkupDefinitionSetting));
        private static SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        //private int roleID = -1;
        //private SiteUser siteUser;

        private string markupDefinitionsPath = string.Empty;
        private string globalMarkupDefinitionsPath = string.Empty; //these are at a higher level than the current site, can be used by multiple sites

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }


        private void EnsureItems()
        {
            if (ddDefinitions == null)
            {

                ddDefinitions = new DropDownList();
                if (this.Controls.Count == 0) { this.Controls.Add(ddDefinitions); }
            }
            if (ddDefinitions.Items.Count > 0) { return; }

            markupDefinitionsPath = WebUtils.GetApplicationRoot() + "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SuperFlexi/MarkupDefinitions/";
            globalMarkupDefinitionsPath = WebUtils.GetApplicationRoot() + "/Data/SuperFlexi/MarkupDefinitions/";

            FileInfo[] files = GetMarkupDefinitions(markupDefinitionsPath);
            FileInfo[] globalFiles = GetMarkupDefinitions(globalMarkupDefinitionsPath);

            List<ListItem> items = new List<ListItem>();



            if (files != null)
            {
                PopulateDefinitionList(files, markupDefinitionsPath, false);
            }
            if (globalFiles != null)
            {
                PopulateDefinitionList(globalFiles, globalMarkupDefinitionsPath, true);
            }

            

            //items.Sort()
            //ddDefinitions.DataSource = items;
            //ddDefinitions.DataBind();
            ddDefinitions.Items.Insert(0, new ListItem("Please Select", string.Empty));
        }

        private void PopulateDefinitionList(FileInfo[] files, string path, bool isGlobal)
        {
            string nameAppendage = isGlobal ? " (g)" : "";

            foreach (FileInfo file in files)
            {
                //log.Info("superflexi markup definition found: " + file.FullName);
                if (File.Exists(file.FullName))
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(file.FullName);

                    XmlNode node = doc.DocumentElement.SelectSingleNode("/Definitions/MarkupDefinition");

                    if (node != null)
                    {
                        XmlAttributeCollection attrCollection = node.Attributes;
                        if (attrCollection["name"] != null)
                        {
                            ddDefinitions.Items.Add(new ListItem(attrCollection["name"].Value + nameAppendage, path + file.Name));
                        }
                        else
                        {
                            ddDefinitions.Items.Add(new ListItem(file.Name.ToString().Replace(".config", "") + nameAppendage, path + file.Name));
                        }
                    }
                }
            }
        }

        private FileInfo[] GetMarkupDefinitions(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));         
            return dir.Exists ? dir.GetFiles("*.config") : null;
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            log.Info("Markup Definition selected: " + ddDefinitions.SelectedValue);
            return ddDefinitions.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();

            if (val != null)
            {
                ListItem item = ddDefinitions.Items.FindByValue(val);
                if (item != null)
                {
                    ddDefinitions.ClearSelection();
                    item.Selected = true;
                }
            }
        }

        #endregion
    }
}