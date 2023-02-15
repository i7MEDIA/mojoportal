// Author:					i7MEDIA (Joe Davis)
// Created:					2015-01-06
// Last Modified:			2017-09-18

using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using Core = mojoPortal.Core;
using mojoPortal.FileSystem;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace SuperFlexiUI
{
	public partial class MarkupDefinitionSetting : mojoUserControl, mojoPortal.Web.UI.ISettingControl
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(MarkupDefinitionSetting));
		private static SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
		private static string originalValue = string.Empty;

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
			FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
			if (p == null)
			{
				log.Error("File System Provider Could Not Be Loaded.");
				return;
			}
			IFileSystem fileSystem = p.GetFileSystem();
			if (fileSystem == null)
			{
				log.Error("File System Could Not Be Loaded.");
				return;
			}

			//string virtualPath = fileSystem.VirtualRoot;

			if (ddDefinitions == null)
			{
				ddDefinitions = new DropDownList();
				if (this.Controls.Count == 0) { this.Controls.Add(ddDefinitions); }
			}

			if (ddDefinitions.Items.Count > 0) { return; }

			string siteSuperFlexiPath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/SuperFlexi/";
			string globalSuperFlexiPath = "~/Data/SuperFlexi/";
			Dictionary<string, string> solutions = new Dictionary<string, string>();
			List<string> names = new List<string>();
			List<SolutionFileLocation> solutionLocations = new List<SolutionFileLocation>()
			{
				new SolutionFileLocation {
					Path = siteSuperFlexiPath + "Solutions/",
					Extension = ".sfmarkup",
					RecurseLevel = RecurseLevel.OneLevel,
					Global = false
				},
				new SolutionFileLocation {
					Path = siteSuperFlexiPath + "MarkupDefinitions/",
					Extension = ".config",
					RecurseLevel = RecurseLevel.TopDirectoryOnly,
					Global = false
				},
				new SolutionFileLocation {
					Path = globalSuperFlexiPath + "Solutions/",
					Extension = ".sfmarkup",
					RecurseLevel = RecurseLevel.OneLevel,
					Global = true
				},
				new SolutionFileLocation {
					Path = globalSuperFlexiPath + "MarkupDefinitions/",
					Extension = ".config",
					RecurseLevel = RecurseLevel.TopDirectoryOnly,
					Global = true
				}
			};

			foreach (var location in solutionLocations)
			{
				//WebFolder folder = new WebFolder();
				//DirectoryInfo dir = new DirectoryInfo(HttpContext.Current.Server.MapPath(location.Path));
				//if (dir.Exists)
				if (fileSystem.FolderExists(location.Path))
				{
					List<WebFile> files = new List<WebFile>();

					switch (location.RecurseLevel)
					{
						case RecurseLevel.OneLevel:
							var folders = fileSystem.GetFolderList(location.Path);

							foreach (var folder in folders)
							{
								files.AddRange(fileSystem.GetFileList(folder.VirtualPath).Where(f => f.Extension.ToLower() == location.Extension));
							}
							break;

						case RecurseLevel.TopDirectoryOnly:
							files.AddRange(fileSystem.GetFileList(location.Path).Where(f => f.Extension.ToLower() == location.Extension));
							break;
					}


					//foreach (FileInfo file in dir.GetFiles(location.Pattern, location.SearchOption))
					foreach (var file in files)
					{
						//if (File.Exists(file.FullName))
						//{
						string nameAppendage = string.Empty;

						if (location.Global)
						{
							nameAppendage = " (global)";
						}

						XmlDocument doc = Core.Helpers.XmlHelper.GetXmlDocument(file.Path);

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
								solutionName = file.Name.ToString().ToLower().Replace(location.Extension, "") + nameAppendage; ;
							}

							names.Add(solutionName);

							if (solutions.ContainsKey(solutionName))
							{
								solutionName += string.Format(" [{0}]", names.Where(n => n.Equals(solutionName)).Count());
							}
							//todo: add capability to nest folders in a solution folder?
							solutions.Add(
								solutionName,
								//location.Path + (location.RecurseLevel == RecurseLevel.ImmediateSubDirectory ? file.Directory.Name + "/" : "") + file.Name);
								file.VirtualPath.Replace("\\", "/").TrimStart('~'));
						}
						//}
					}
				}
			}

			ddDefinitions.DataSource = solutions.OrderBy(i => i.Key);
			ddDefinitions.DataTextField = "Key";
			ddDefinitions.DataValueField = "Value";
			ddDefinitions.DataBind();

			ddDefinitions.Items.Insert(0, new ListItem(SuperFlexiResources.SolutionDropDownPleaseSelect, "0"));
		}

		#region ISettingControl

		public string GetValue()
		{
			EnsureItems();
			//log.Info("solution selected: " + ddDefinitions.SelectedValue);

			if (ddDefinitions.SelectedValue == "0")
			{
				return string.Empty;
			}
			string selectedSolution = ddDefinitions.SelectedValue;
			if (originalValue != selectedSolution)
			{
				int moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);

				if (moduleId > -1)
				{
					Module module = new Module(moduleId);
					if (module != null)
					{
						ModuleSettings.UpdateModuleSetting(module.ModuleGuid, moduleId, "MarkupDefinitionContent", string.Empty);
					}
				}
			}

			return selectedSolution;

		}

		public void SetValue(string val)
		{
			EnsureItems();

			if (val != null)
			{
				if (val == "") val = "0";
				originalValue = val;
				ListItem item = ddDefinitions.Items.FindByValue(val);

				if (item != null)
				{
					ddDefinitions.ClearSelection();
					item.Selected = true;
				}
			}
		}
		#endregion
		class SolutionFileLocation
		{
			public string Path { get; set; }
			public string Extension { get; set; }
			public RecurseLevel RecurseLevel { get; set; }
			public bool Global { get; set; }
		}

		enum RecurseLevel { OneLevel, TopDirectoryOnly };
	}
}