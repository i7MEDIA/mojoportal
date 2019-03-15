// The use and distribution terms for this software are covered by the 
// Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file CPL.TXT at the root of this distribution.
// By using this software in any fashion, you are agreeing to be bound by 
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.

using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Collections;
using mojoPortal.Data;

namespace mojoPortal.Business
{
	/// <summary>
	/// Represents a page in the content management system.
	/// </summary>
	public class PageSettings : IComparable 
	{
		#region Constructors

		public PageSettings()
		{
            
        }

        public PageSettings(Guid pageGuid)
        {
            if (pageGuid == null) { return; }
            if (pageGuid == Guid.Empty) { return; }

            GetPage(pageGuid);

        }

        public PageSettings(int siteId, int pageId)
        {
            if (siteId > -1)
            {
                GetPage(siteId, pageId);
            }

        }

		#endregion

		#region Properties
		//http://www.w3schools.com/TAGS/att_a_rel.asp
		public string LinkRel { get; set; } = string.Empty;

		public string PageHeading { get; set; } = string.Empty;

		public bool ShowPageHeading { get; set; } = true;
		/// <summary>
		/// this is for future use, only the db part has been implemented so far
		/// </summary>
		public DateTime PubDateUtc { get; set; } = DateTime.UtcNow;

		public string MenuDescription { get; set; } = string.Empty;

		public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

		public Guid CreatedBy { get; set; } = Guid.Empty;

		public string CreatedFromIp { get; set; } = string.Empty;
		/// <summary>
		/// This is the last time the page settings were saved
		/// </summary>
		public DateTime LastModUtc { get; set; } = DateTime.UtcNow;

		public Guid LastModBy { get; set; } = Guid.Empty;

		public string LastModFromIp { get; set; } = string.Empty;

		public Guid SiteGuid { get; set; } = Guid.Empty;

		public string CompiledMeta { get; set; } = string.Empty;

		public DateTime CompiledMetaUtc { get; set; } = DateTime.UtcNow;

		public ArrayList Modules { get; set; } = new ArrayList();

		public Guid PageGuid { get; set; } = Guid.Empty;

		public int PageId { get; set; } = -1;

		public Guid ParentGuid { get; set; } = Guid.Empty;

		public int ParentId { get; set; } = -1;

		public int SiteId { get; set; } = 0;

		public int PageIndex { get; set; }

		public int PageOrder { get; set; } = 0;

		public string PageName { get; set; } = string.Empty;

		public string PageTitle { get; set; } = String.Empty;

		public int PublishMode { get; set; } = 0;

		public string Skin { get; set; } = string.Empty;

		public string BodyCssClass { get; set; } = string.Empty;
		
		public string MenuCssClass { get; set; } = string.Empty;

		public string AuthorizedRoles { get; set; } = string.Empty;

		public string EditRoles { get; set; } = string.Empty;

		public string DraftEditOnlyRoles { get; set; } = string.Empty;

		public string DraftApprovalRoles { get; set; } = string.Empty;

		public string CreateChildPageRoles { get; set; } = string.Empty;

		public string CreateChildDraftRoles { get; set; } = string.Empty;
		
		public bool RequireSsl { get; set; } = false;

		public bool AllowBrowserCache { get; set; } = true;

		public bool ShowBreadcrumbs { get; set; } = false;

		public bool ShowChildPageBreadcrumbs { get; set; } = false;

		public bool HideMainMenu { get; set; } = false;

		public bool HidePageMenu { get; set; } = true;


		public bool HideAfterLogin { get; set; } = false;

		public bool EnableComments { get; set; } = false;

		public bool UseUrl { get; set; } = true;

		public string Url { get; set; } = string.Empty;
		
		public string UnmodifiedUrl { get; set; } = string.Empty;

		public bool OpenInNewWindow { get; set; } = false;

		public bool ShowChildPageMenu { get; set; } = false;

		public string PageMetaKeyWords { get; set; } = string.Empty;

		public string PageMetaDescription { get; set; } = string.Empty;

		[Obsolete("This property is obsolete.")]
		public string PageMetaEncoding { get; set; } = string.Empty;

		[Obsolete("This property is obsolete.")]
		public string PageMetaAdditional { get; set; } = string.Empty;

		public string IndexPath { get; set; } = string.Empty;

		public bool IncludeInMenu { get; set; } = true;

		public string MenuImage { get; set; } = String.Empty;

		public PageChangeFrequency ChangeFrequency { get; set; } = PageChangeFrequency.Daily;

		public string SiteMapPriority { get; set; } = "0.5";

		/// <summary>
		/// this is the lastr time a content feature on a page updated the time stamp
		/// so it represents the last time content changed on the page not the last time
		/// page settings changed
		/// </summary>
		public DateTime LastModifiedUtc { get; set; } = DateTime.UtcNow;

		public string DepthIndicator { get; set; } = string.Empty;

		public bool IncludeInSiteMap { get; set; } = true;

		public bool ExpandOnSiteMap { get; set; } = true;
			   
		public bool IsClickable { get; set; } = true;

		public bool ShowHomeCrumb { get; set; } = false;

		public bool UrlHasBeenAdjustedForFolderSites { get; set; } = false;

		public bool IsPending { get; set; } = false;

		public string CanonicalOverride { get; set; } = string.Empty;

		public bool IncludeInSearchMap { get; set; } = true;

		public bool IncludeInChildSiteMap { get; set; } = true;

		public Guid PubTeamId { get; set; } = Guid.Empty;

		public string CreatedByName { get; private set; } = string.Empty;

		public string CreatedByEmail { get; private set; } = string.Empty;

		public string CreatedByFirstName { get; private set; } = string.Empty;

		public string CreatedByLastName { get; private set; } = string.Empty;

		public string LastModByName { get; private set; } = string.Empty;

		public string LastModByEmail { get; private set; } = string.Empty;

		public string LastModByFirstName { get; private set; } = string.Empty;

		public string LastModByLastName { get; private set; } = string.Empty;

		#endregion

		#region IComparable

		public int CompareTo(object value) 
		{

			if (value == null) return 1;

			int compareOrder = ((PageSettings)value).PageOrder;
            
			if (this.PageOrder == compareOrder) return 0;
			if (this.PageOrder < compareOrder) return -1;
			if (this.PageOrder > compareOrder) return 1;
			return 0;
		}

		#endregion

		#region Private Methods

        

		private void GetPage(int siteId, int pageId)
		{
            using(IDataReader reader = DBPageSettings.GetPage(siteId, pageId))
            {
                LoadFromReader(reader);

                
            }
            
		}

        private void GetPage(Guid pageGuid)
        {
            using (IDataReader reader = DBPageSettings.GetPage(pageGuid))
            {
                LoadFromReader(reader);

            }
        }

        private void LoadFromReader(IDataReader reader)
        {
           
            if (reader.Read())
            {
                this.PageId = int.Parse(reader["PageID"].ToString());
                this.ParentId = int.Parse(reader["ParentID"].ToString());
                this.SiteId = int.Parse(reader["SiteID"].ToString());
                this.PageOrder = int.Parse(reader["PageOrder"].ToString());
                this.PageName = reader["PageName"].ToString();
                this.PageTitle = reader["PageTitle"].ToString();
                this.MenuImage = reader["MenuImage"].ToString();
                this.Skin = reader["Skin"].ToString();
                this.AuthorizedRoles = reader["AuthorizedRoles"].ToString();
                this.EditRoles = reader["EditRoles"].ToString();
                this.DraftEditOnlyRoles = reader["DraftEditRoles"].ToString();
                this.DraftApprovalRoles = reader["DraftApprovalRoles"].ToString();
                this.CreateChildPageRoles = reader["CreateChildPageRoles"].ToString();
                this.CreateChildDraftRoles = reader["CreateChildDraftRoles"].ToString();

                this.RequireSsl = Convert.ToBoolean(reader["RequireSSL"]);
                this.AllowBrowserCache = Convert.ToBoolean(reader["AllowBrowserCache"]);
                this.ShowBreadcrumbs = Convert.ToBoolean(reader["ShowBreadcrumbs"]);
                this.ShowChildPageBreadcrumbs = Convert.ToBoolean(reader["ShowChildBreadCrumbs"]);
                this.UseUrl = Convert.ToBoolean(reader["UseUrl"]);
                this.Url = reader["Url"].ToString();
                if (UseUrl)
                {
                    UnmodifiedUrl = reader["Url"].ToString();
                }
                else
                {
                    UnmodifiedUrl = "~/Default.aspx?pageid=" + PageId.ToString(CultureInfo.InvariantCulture);
                }
                this.OpenInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]);
                this.ShowChildPageMenu = Convert.ToBoolean(reader["ShowChildPageMenu"]);
                this.HideMainMenu = Convert.ToBoolean(reader["HideMainMenu"]);

                this.PageMetaKeyWords = reader["PageKeyWords"].ToString();
                this.PageMetaDescription = reader["PageDescription"].ToString();
                this.PageMetaEncoding = reader["PageEncoding"].ToString();
                this.PageMetaAdditional = reader["AdditionalMetaTags"].ToString();
                this.IncludeInMenu = Convert.ToBoolean(reader["IncludeInMenu"]);

                string cf = reader["ChangeFrequency"].ToString();
                switch (cf)
                {
                    case "Always":
                        this.ChangeFrequency = PageChangeFrequency.Always;
                        break;

                    case "Hourly":
                        this.ChangeFrequency = PageChangeFrequency.Hourly;
                        break;

                    case "Daily":
                        this.ChangeFrequency = PageChangeFrequency.Daily;
                        break;

                    case "Monthly":
                        this.ChangeFrequency = PageChangeFrequency.Monthly;
                        break;

                    case "Yearly":
                        this.ChangeFrequency = PageChangeFrequency.Yearly;
                        break;

                    case "Never":
                        this.ChangeFrequency = PageChangeFrequency.Never;
                        break;

                    case "Weekly":
                    default:
                        this.ChangeFrequency = PageChangeFrequency.Weekly;
                        break;


                }

                string smp = reader["SiteMapPriority"].ToString().Trim();
                if (smp.Length > 0) this.SiteMapPriority = smp;

                if (reader["LastModifiedUTC"] != DBNull.Value)
                {
                    this.LastModifiedUtc = Convert.ToDateTime(reader["LastModifiedUTC"]);
                }

                this.PageGuid = new Guid(reader["PageGuid"].ToString());
                this.ParentGuid = new Guid(reader["ParentGuid"].ToString());
                this.HideAfterLogin = Convert.ToBoolean(reader["HideAfterLogin"]);
                this.SiteGuid = new Guid(reader["SiteGuid"].ToString());
                this.CompiledMeta = reader["CompiledMeta"].ToString();
                if (reader["CompiledMetaUtc"] != DBNull.Value)
                {
                    this.CompiledMetaUtc = Convert.ToDateTime(reader["CompiledMetaUtc"]);
                }

                this.IncludeInSiteMap = Convert.ToBoolean(reader["IncludeInSiteMap"]);
                this.IsClickable = Convert.ToBoolean(reader["IsClickable"]);
                this.ShowHomeCrumb = Convert.ToBoolean(reader["ShowHomeCrumb"]);
                this.IsPending = Convert.ToBoolean(reader["IsPending"]);

                this.IncludeInSearchMap = Convert.ToBoolean(reader["IncludeInSearchMap"]);
                this.CanonicalOverride = reader["CanonicalOverride"].ToString();
                this.EnableComments = Convert.ToBoolean(reader["EnableComments"]);

                this.PubTeamId = new Guid(reader["PubTeamId"].ToString());
                this.IncludeInChildSiteMap = Convert.ToBoolean(reader["IncludeInChildSiteMap"]);
                this.ExpandOnSiteMap = Convert.ToBoolean(reader["ExpandOnSiteMap"]);

                this.BodyCssClass = reader["BodyCssClass"].ToString();
                this.MenuCssClass = reader["MenuCssClass"].ToString();

                this.PublishMode = Convert.ToInt32(reader["PublishMode"].ToString());

                if (reader["PCreatedUtc"] != DBNull.Value)
                {
                    this.CreatedUtc = Convert.ToDateTime(reader["PCreatedUtc"]);
                }

                if (reader["PCreatedBy"] != DBNull.Value)
                {
                    string pcg = reader["PCreatedBy"].ToString();
                    if (pcg.Length == 36)
                    {
                        this.CreatedBy = new Guid(pcg);
                    }

                }

                this.CreatedFromIp = reader["PCreatedFromIp"].ToString();

                if (reader["PLastModUtc"] != DBNull.Value)
                {
                    this.LastModUtc = Convert.ToDateTime(reader["PLastModUtc"]);
                }

                if (reader["PLastModBy"] != DBNull.Value)
                {
                    string pcg = reader["PLastModBy"].ToString();
                    if (pcg.Length == 36)
                    {
                        this.LastModBy = new Guid(pcg);
                    }
                    
                }

                this.LastModFromIp = reader["PLastModFromIp"].ToString();

                this.CreatedByName = reader["CreatedByName"].ToString();
                this.CreatedByEmail = reader["CreatedByEmail"].ToString();
                this.CreatedByFirstName = reader["CreatedByFirstName"].ToString();
                this.CreatedByLastName = reader["CreatedByLastName"].ToString();
                this.LastModByName = reader["LastModByName"].ToString();
                this.LastModByEmail = reader["LastModByEmail"].ToString();
                this.LastModByFirstName = reader["LastModByFirstName"].ToString();
                this.LastModByLastName = reader["LastModByLastName"].ToString();
                this.MenuDescription = reader["MenuDesc"].ToString();

                if (reader["PubDateUtc"] != DBNull.Value)
                {
                    this.PubDateUtc = Convert.ToDateTime(reader["PubDateUtc"]);
                }

                this.ShowPageHeading = Convert.ToBoolean(reader["ShowPageHeading"]);
                this.LinkRel = reader["LinkRel"].ToString();
                this.PageHeading = reader["PageHeading"].ToString();


            }
           

        }

		private IDataReader GetChildPages() 
		{
			return DBPageSettings.GetChildPages(this.SiteId, this.ParentId);
		}

        private DataTable GetChildPageIds()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageID", typeof(int));
            using (IDataReader reader = GetChildPages())
            {
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    row["PageID"] = reader["PageID"];

                    dt.Rows.Add(row);
                }
            }
            
            return dt;

        }

		private bool Create()
		{ 
			int newID = -1;

            if (this.PageGuid == Guid.Empty) this.PageGuid = Guid.NewGuid();
			
			newID = DBPageSettings.Create(
				this.SiteId,
				this.ParentId,
				this.PageName,
                this.PageTitle,
				this.Skin,
				this.PageOrder,
				this.AuthorizedRoles,
				this.EditRoles,
                this.DraftEditOnlyRoles,
                this.DraftApprovalRoles,
				this.CreateChildPageRoles,
                this.CreateChildDraftRoles,
				this.RequireSsl,
                this.AllowBrowserCache,
				this.ShowBreadcrumbs,
				this.ShowChildPageBreadcrumbs,
				this.PageMetaKeyWords,
				this.PageMetaDescription,
				this.PageMetaEncoding,
				this.PageMetaAdditional,
				this.UseUrl,
				this.Url,
				this.OpenInNewWindow,
				this.ShowChildPageMenu,
				this.HideMainMenu,
                this.IncludeInMenu,
                this.MenuImage,
                this.ChangeFrequency.ToString(),
                this.SiteMapPriority,
                this.PageGuid,
                this.ParentGuid,
                this.HideAfterLogin,
                this.SiteGuid,
                this.CompiledMeta,
                this.CompiledMetaUtc,
                this.IncludeInSiteMap,
                this.IsClickable,
                this.ShowHomeCrumb,
                this.IsPending,
                this.CanonicalOverride,
                this.IncludeInSearchMap,
                this.EnableComments,
                this.IncludeInChildSiteMap,
                this.ExpandOnSiteMap,
                this.PubTeamId,
                this.BodyCssClass,
                this.MenuCssClass,
                this.PublishMode,
                this.CreatedBy,
                this.CreatedFromIp,
                this.MenuDescription,
                this.LinkRel,
                this.PageHeading,
                this.ShowPageHeading,
                this.PubDateUtc);

			this.PageId = newID;

            bool result = (newID > -1);

            
            if (result)
            {
                PageCreatedEventArgs e = new PageCreatedEventArgs();
                OnPageCreated(e);
            }
					
			return result;
		}

		private bool Update()
		{

			return DBPageSettings.UpdatePage(
				this.SiteId,
				this.PageId,
				this.ParentId,
				this.PageName,
                this.PageTitle,
				this.Skin,
				this.PageOrder,
				this.AuthorizedRoles,
				this.EditRoles,
                this.DraftEditOnlyRoles,
                this.DraftApprovalRoles,
				this.CreateChildPageRoles,
                this.CreateChildDraftRoles,
				this.RequireSsl,
                this.AllowBrowserCache,
				this.ShowBreadcrumbs,
				this.ShowChildPageBreadcrumbs,
				this.PageMetaKeyWords,
				this.PageMetaDescription,
				this.PageMetaEncoding,
				this.PageMetaAdditional,
				this.UseUrl,
				this.Url,
				this.OpenInNewWindow,
				this.ShowChildPageMenu,
				this.HideMainMenu,
                this.IncludeInMenu,
                this.MenuImage,
                this.ChangeFrequency.ToString(),
                this.SiteMapPriority,
                this.ParentGuid,
                this.HideAfterLogin,
                this.CompiledMeta,
                this.CompiledMetaUtc,
                this.IncludeInSiteMap,
                this.IsClickable,
                this.ShowHomeCrumb,
                this.IsPending,
                this.CanonicalOverride,
                this.IncludeInSearchMap,
                this.EnableComments,
                this.IncludeInChildSiteMap,
                this.ExpandOnSiteMap,
                this.PubTeamId,
                this.BodyCssClass,
                this.MenuCssClass,
                this.PublishMode,
                this.CreatedUtc,
                this.CreatedBy,
                this.LastModBy,
                this.LastModFromIp,
                this.MenuDescription,
                this.LinkRel,
                this.PageHeading,
                this.ShowPageHeading,
                this.PubDateUtc);
		}


        public void RefreshModules()
        {
            this.Modules.Clear();
            using (IDataReader reader = Module.GetPageModules(this.PageId))
            {
                while (reader.Read())
                {
					Module m = new Module();
					m.ModuleId = Convert.ToInt32(reader["ModuleID"]);
					m.SiteId = Convert.ToInt32(reader["SiteID"]);
					m.ModuleDefId = Convert.ToInt32(reader["ModuleDefID"]);
					m.ModuleTitle = reader["ModuleTitle"].ToString();
					m.AuthorizedEditRoles = reader["AuthorizedEditRoles"].ToString();
					m.CacheTime = Convert.ToInt32(reader["CacheTime"]);
					string showTitle = reader["ShowTitle"].ToString();
					m.ShowTitle = (showTitle == "True" || showTitle == "1");
					if (reader["EditUserID"] != DBNull.Value)
					{
						m.EditUserId = Convert.ToInt32(reader["EditUserID"]);
					}
					//m.AvailableForMyPage = Convert.ToBoolean(reader["AvailableForMyPage"]);
					//m.AllowMultipleInstancesOnMyPage = Convert.ToBoolean(reader["AllowMultipleInstancesOnMyPage"]);
					//m.Icon = reader["Icon"].ToString();
					m.CreatedByUserId = Convert.ToInt32(reader["CreatedByUserID"]);
					if (reader["CreatedDate"] != DBNull.Value)
					{
						m.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
					}
					//m.CountOfUseOnMyPage
					m.ModuleGuid = new Guid(reader["Guid"].ToString());
					m.FeatureGuid = new Guid(reader["FeatureGuid"].ToString());
					m.SiteGuid = new Guid(reader["SiteGuid"].ToString());
					if (reader["EditUserGuid"] != DBNull.Value)
					{
						m.EditUserGuid = new Guid(reader["EditUserGuid"].ToString());
					}
					m.HideFromUnauthenticated = Convert.ToBoolean(reader["HideFromUnAuth"]);
					m.HideFromAuthenticated = Convert.ToBoolean(reader["HideFromAuth"]);
					m.ViewRoles = reader["ViewRoles"].ToString();
					m.DraftEditRoles = reader["DraftEditRoles"].ToString();
					m.IncludeInSearch = Convert.ToBoolean(reader["IncludeInSearch"]);
					m.IsGlobal = Convert.ToBoolean(reader["IsGlobal"]);
					m.HeadElement = reader["HeadElement"].ToString();
					m.PublishMode = Convert.ToInt32(reader["PublishMode"]);
					m.DraftApprovalRoles = reader["DraftApprovalRoles"].ToString();

					m.PageId = Convert.ToInt32(reader["PageID"]);
					m.PaneName = reader["PaneName"].ToString();
					m.ModuleOrder = Convert.ToInt32(reader["ModuleOrder"]);
					m.ControlSource = reader["ControlSrc"].ToString();

					this.Modules.Add(m);
                }
            }


        }

		
		#endregion

		#region Public Methods

		

		public bool Save()
		{
			if(this.PageId > -1)
			{
				return Update();
			}
			else
			{
				return Create();
			}
			
		}


        public void MoveToTop()
        {
            this.PageOrder = 0;
            PageSettings.UpdatePageOrder(this.PageId, this.PageOrder);
            //now get all children of my parent id and reset sort using current order
            ResortPages(); 

        }

        public void MoveToBottom()
        {
            this.PageOrder = 999999;
            PageSettings.UpdatePageOrder(this.PageId, this.PageOrder);
            //now get all children of my parent id and reset sort using current order
            ResortPages();

        }



		public void MoveUp()
		{
			//if there are pages with the same parentID and a lower sort than me
			//subtract 3 from my sort and save it, then reset sort order on
			//all children of my parent by 2 starting from 1

			//no need to look up whther pages above me, assume yes if my sort is > 1
			if(this.PageOrder > 1)
			{
				//pages exist above me so update my page order
				this.PageOrder -= 3;
				PageSettings.UpdatePageOrder(this.PageId, this.PageOrder);
				//now get all children of my parent id and reset sort using current order
				ResortPages();
				
			}
			else
			{
				if(this.PageOrder < 1)
				{
					this.PageOrder = 1;
					PageSettings.UpdatePageOrder(this.PageId, this.PageOrder);
					ResortPages();
					
				}
				//else must be equal to 1 no need to do anything
				
			}

		}

		public void MoveDown()
		{
			this.PageOrder += 3;
			PageSettings.UpdatePageOrder(this.PageId, this.PageOrder);
			//now get all children of my parent id and reset sort using current order
			ResortPages();
				
		}

		public void ResortPages()
		{
			int i = 1;
            DataTable table = GetChildPageIds();

			foreach(DataRow row in table.Rows)
			{
				int pageID = Convert.ToInt32(row["PageID"]);
				PageSettings.UpdatePageOrder(pageID, i);
				i += 2;
			}	
		}

        public void ResortPagesAlphabetically()
        {
            int i = 1;
            DataTable dataTable = new DataTable();
            using (IDataReader reader = PageSettings.GetChildPagesSortedAlphabetic(SiteId, PageId))
            {
                dataTable.Load(reader);
            }

            foreach (DataRow row in dataTable.Rows)
            {
                int pageID = Convert.ToInt32(row["PageID"]);
                PageSettings.UpdatePageOrder(pageID, i);
                i += 2;
            }
        }

        public bool ContainsModule(int moduleId)
        {
            bool result = false;
            
            foreach (Module m in Modules)
            {
                if (m.ModuleId == moduleId) result = true;
            }

            return result;
        }

        public bool ContainsModuleInProgress()
        {
            int count = ContentWorkflow.GetWorkInProgressCountByPage(this.PageGuid);
            return (count > 0);
        }

        [Obsolete("This method is obsolete, use SiteUtils.GetCurrentPageUrl() instead")]
        public string ResolveUrl(SiteSettings siteSettings)
        {
            if (siteSettings == null) return Url;
            string resolvedUrl;
            if (this.UseUrl)
            {
                if((this.Url.StartsWith("~/"))&&(this.Url.EndsWith(".aspx")))
                {
                    if (UrlHasBeenAdjustedForFolderSites)
                    {
                        resolvedUrl = Url.Replace("~/", "/");
                    }
                    else
                    {
                        resolvedUrl = siteSettings.SiteRoot
                            + Url.Replace("~/", "/");
                    }

                }
                else
                {
                    resolvedUrl = Url;
                }

            }
            else
            {
                resolvedUrl = siteSettings.SiteRoot 
                    + "/Default.aspx?pageid=" 
                    + this.PageId.ToString();
            }

            return resolvedUrl;

        }

        public bool UpdateLastModifiedTime()
        {
            if (this.PageId == -1) return false;
            return UpdateTimestamp(this.PageId, DateTime.UtcNow);

        }

		#endregion

		#region Static Methods

        public static IDataReader GetPageList(int siteId)
        {
            return DBPageSettings.GetPageList(siteId);
        }

        public static int GetCountChildPages(int parentPageId, bool includePending)
        {
            return DBPageSettings.GetCountChildPages(parentPageId, includePending);
        }

        public static IDataReader GetChildPagesSortedAlphabetic(int siteId, int parentId)
        {
            return DBPageSettings.GetChildPagesSortedAlphabetic(siteId, parentId);
        }

        public static IDataReader GetPendingPageListPage(Guid siteGuid, int pageNumber, int pageSize, out int totalPages)
        {
            return DBPageSettings.GetPendingPageListPage(siteGuid, pageNumber, pageSize, out totalPages);
        }

		public static bool UpdatePageOrder(int pageId, int pageOrder)
		{
			return DBPageSettings.UpdatePageOrder(pageId,pageOrder);
		}

        public static bool UpdateTimestamp(
            int pageId,
            DateTime lastModifiedUtc)
        {
            return DBPageSettings.UpdateTimestamp(pageId, lastModifiedUtc);
        }

		public static bool DeletePage(int pageId)
		{
			bool result =  DBPageSettings.DeletePage(pageId);
            DBPageSettings.CleanupOrphans();
            return result;
		}

        public static int GetCountOfPages(int siteId)
        {
            return GetCount(siteId, true);
            
        }

        public static int GetCount(int siteId, bool includePending)
        {
            return DBPageSettings.GetCount(siteId, includePending);
        }

        
        
		public static int GetNextPageOrder(int siteId, int parentId) 
		{
			return DBPageSettings.GetNextPageOrder(siteId, parentId);
		}

        //public static IDataReader GetChildPagesByPageId(int pageId) 
        //{
        //    return DBPageSettings.GetChildPages(pageId);
        //}

        public static bool Exists(Guid pageGuid)
        {
            bool result = false;
            using (IDataReader reader = DBPageSettings.GetPage(pageGuid))
            {
                if (reader.Read())
                {
                    result = true;
                }
            }

            return result;

        }

        

		#endregion

        #region Events

        public event PageCreatedEventHandler PageCreated;

        protected void OnPageCreated(PageCreatedEventArgs e)
        {
            if (PageCreated != null)
            {
                PageCreated(this, e);
            }
        }

        #endregion

    }
}
