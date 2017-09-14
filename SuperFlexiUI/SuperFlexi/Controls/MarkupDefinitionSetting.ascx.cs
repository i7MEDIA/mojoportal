// Author:					i7MEDIA (Joe Davis)
// Created:					2015-01-06
// Last Modified:			2017-09-14

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using System.Linq;
namespace SuperFlexiUI
{
    public partial class MarkupDefinitionSetting : mojoUserControl, mojoPortal.Web.UI.ISettingControl
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(MarkupDefinitionSetting));
        private static SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
        //private int roleID = -1;
        //private SiteUser siteUser;

  //      private string markupDefinitionsPath = string.Empty;
  //      private string solutionsPath = string.Empty;
		//private string globalMarkupDefinitionsPath = string.Empty; //these are at a higher level than the current site, can be used by multiple sites
		//private string globalSolutionsPath = string.Empty; //these are at a higher level than the current site, can be used by multiple sites

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

			string siteSuperFlexiPath = "/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SuperFlexi/";
			string globalSuperFlexiPath = "/Data/SuperFlexi/";
			Dictionary<string, string> solutions = new Dictionary<string, string>();
			List<string> names = new List<string>();
			List<SolutionFileLocation> solutionLocations = new List<SolutionFileLocation>()
			{
				new SolutionFileLocation {
					Path = WebUtils.GetApplicationRoot() + siteSuperFlexiPath + "Solutions/",
					Pattern = "*.sfmarkup",
					SearchOption = SearchOption.AllDirectories,
					Global = false
				},
				new SolutionFileLocation {
					Path = WebUtils.GetApplicationRoot() + siteSuperFlexiPath + "MarkupDefinitions/",
					Pattern = "*.config",
					SearchOption = SearchOption.TopDirectoryOnly,
					Global = false
				},
				new SolutionFileLocation {
					Path = WebUtils.GetApplicationRoot() + globalSuperFlexiPath + "Solutions/",
					Pattern = "*.sfmarkup",
					SearchOption = SearchOption.AllDirectories,
					Global = true
				},
				new SolutionFileLocation {
					Path = WebUtils.GetApplicationRoot() + globalSuperFlexiPath + "MarkupDefinitions/",
					Pattern = "*.config",
					SearchOption = SearchOption.TopDirectoryOnly,
					Global = true
				}
			};
			
			foreach (var location in solutionLocations)
			{
				DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(location.Path));
				if (dir.Exists)
				{
					foreach (FileInfo file in dir.GetFiles(location.Pattern, location.SearchOption))
					{
						if (File.Exists(file.FullName))
						{
							string nameAppendage = string.Empty;

							if (location.Global)
							{
								nameAppendage = " (global)";
							}

							XmlDocument doc = new XmlDocument();
							doc.Load(file.FullName);

							XmlNode node = doc.DocumentElement.SelectSingleNode("/Definitions/MarkupDefinition");

							if (node != null)
							{
								XmlAttributeCollection attrCollection = node.Attributes;
								string solutionName = string.Empty;
								if (attrCollection["name"] != null)
								{
									solutionName = attrCollection["name"].Value + nameAppendage; ;
								}
								else
								{
									solutionName = file.Name.ToString().Replace(location.Pattern, "") + nameAppendage; ;
								}

								names.Add(solutionName);

								if (solutions.ContainsKey(solutionName))
								{
									solutionName += string.Format(" [{0}]", names.Where(n => n.Equals(solutionName)).Count());
								}
								//todo: add capability to nest folders in a solution folder?
								solutions.Add(
									solutionName, 
									location.Path + (location.SearchOption == SearchOption.AllDirectories ? file.Directory.Name + "/" : "") + file.Name);
							}
						}
					}
				}
			}

			ddDefinitions.DataSource = solutions.OrderBy(i => i.Key);
			ddDefinitions.DataTextField = "Key";
			ddDefinitions.DataValueField = "Value";
			ddDefinitions.DataBind();


			//List<FileInfo> solutionFiles = new List<FileInfo>();

			//GetListOfFilesFromPath(solutionsPath, "*.sfmarkup", SearchOption.AllDirectories, ref solutionFiles);
			//GetListOfFilesFromPath(globalSolutionsPath, "*.sfmarkup", SearchOption.AllDirectories, ref solutionFiles);
			//GetListOfFilesFromPath(markupDefinitionsPath, "*.config", SearchOption.TopDirectoryOnly, ref solutionFiles);
			//GetListOfFilesFromPath(globalMarkupDefinitionsPath, "*.config", SearchOption.TopDirectoryOnly, ref solutionFiles);

			//List<ListItem> items = new List<ListItem>();

   //         if (solutionFiles.Count > 0)
   //         {
   //             PopulateList(solutionFiles);
   //         }

			//items.Sort()
			//ddDefinitions.DataSource = items;
			//ddDefinitions.DataBind();


            ddDefinitions.Items.Insert(0, new ListItem(SuperFlexiResources.SolutionDropDownPleaseSelect, string.Empty));
        }

		#region ISettingControl

		public string GetValue()
        {
            EnsureItems();
            //log.Info("solution selected: " + ddDefinitions.SelectedValue);
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
	class SolutionFileLocation
	{
		public string Path { get; set; }
		public string Pattern { get; set; }
		public SearchOption SearchOption { get; set; }
		public bool Global{ get; set; }
	}
}