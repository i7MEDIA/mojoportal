using System;
using System.Data;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.AdminUI
{
	public partial class IndexBrowser : NonCmsBasePage
	{

		private static readonly ILog log = LogManager.GetLogger(typeof(Page));

		//private SiteSettings siteSettings = null;
		//protected string SiteRoot = string.Empty;
		private int pageNumber = 1;
		private int pageSize = WebConfigSettings.SearchResultsPageSize;
		private int totalHits = 0;
		private int totalPages = 1;
		private DateTime modifiedBeginDate = DateTime.MinValue;
		private DateTime modifiedEndDate = DateTime.MaxValue;
		private Guid featureGuid = Guid.Empty;

		//protected override void OnPreInit(EventArgs e)
		//{
		//	base.OnPreInit(e);
		//}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.Load += new EventHandler(this.Page_Load);
			btnGo.Click += new EventHandler(this.btnGo_Click);
			btnRebuildSearchIndex.Click += new EventHandler(this.btnRebuildSearchIndex_Click);
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			SiteUtils.ForceSsl();

			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);
				return;
			}
			LoadSettings();

			if (!WebUser.IsAdminOrContentAdmin || !SiteUtils.UserIsSiteEditor()  || (WebConfigSettings.DisableLoginInfo))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			if (SiteUtils.IsFishyPost(this))
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			PopulateLabels();

			if (!IsPostBack)
			{
				try
				{
					BindIndex();
					lblMessage.Text = string.Empty;
					BindFeatureList();

					ddFeatureList.Items.Insert(0, new ListItem(Resource.SearchAllContentItem, Guid.Empty.ToString()));
					if (ddFeatureList.Items.Count > 0)
					{
						ListItem item = ddFeatureList.Items.FindByValue(featureGuid.ToString());
						if (item != null)
						{
							ddFeatureList.ClearSelection();
							item.Selected = true;

						}
					}
					else
					{
						ddFeatureList.Visible = false;
					}
				}
				catch (IOException ex)
				{
					lblMessage.Text = $"{Resource.AdminIndexBrowserExceptionDescription}\r\n{ex.Message}\r\n{ex.StackTrace}";
				}

			}


		}

		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, Resource.AdminIndexBrowser);

			heading.Text = Resource.AdminIndexBrowser;

			lnkAdminMenu.Text = Resource.AdminMenuLink;
			lnkAdminMenu.NavigateUrl = SiteRoot + "/Admin/AdminMenu.aspx";

			lnkAdvancedTools.Text = Resource.AdvancedToolsLink;
			lnkAdvancedTools.NavigateUrl = SiteRoot + "/Admin/AdvancedTools.aspx";

			lnkCurrentPage.Text = Resource.AdminIndexBrowser;
			lnkCurrentPage.NavigateUrl = SiteRoot + "/Admin/IndexBrowser.aspx";

			btnRebuildSearchIndex.Text = Resource.SearchRebuildIndexButton;
			btnGo.Text = Resource.AdminIndexBrowserFilter;
		}

		private void LoadSettings()
		{

			//siteSettings = CacheHelper.GetCurrentSiteSettings();
			pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);

			modifiedBeginDate = WebUtils.ParseDateFromQueryString("bd", DateTime.MinValue).Date;
			modifiedEndDate = WebUtils.ParseDateFromQueryString("ed", DateTime.MaxValue).Date;
			featureGuid = WebUtils.ParseGuidFromQueryString("f", featureGuid);

			if (!IsPostBack)
			{
				if (modifiedBeginDate.Date > DateTime.MinValue.Date)
				{
					beginDate.Text = modifiedBeginDate.ToShortDateString();
				}

				if (modifiedEndDate.Date < DateTime.MaxValue.Date)
				{
					endDate.Text = modifiedEndDate.ToShortDateString();
				}

			}

			UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, Resource.SearchRebuildIndexWarning);
		}

		private void BindIndex()
		{

			IndexItemCollection searchResults = IndexHelper.Browse(
				siteSettings.SiteId,
				featureGuid,
				modifiedBeginDate,
				modifiedEndDate,
				pageNumber,
				pageSize,
				out totalHits);

			totalPages = 1;

			if (pageSize > 0) { totalPages = totalHits / pageSize; }

			if (totalHits <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				Math.DivRem(totalHits, pageSize, out int remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			string searchUrl = SiteRoot
					+ "/Admin/IndexBrowser.aspx?p={0}"
					+ "&amp;bd=" + modifiedBeginDate.Date.ToString("s")
					+ "&amp;ed=" + modifiedEndDate.Date.ToString("s")
					+ "&amp;f=" + featureGuid.ToString();

			pgrTop.PageURLFormat = searchUrl;
			pgrTop.ShowFirstLast = true;
			pgrTop.CurrentIndex = pageNumber;
			pgrTop.PageSize = pageSize;
			pgrTop.PageCount = totalPages;
			pgrTop.Visible = (totalPages > 1);

			pgrBottom.PageURLFormat = searchUrl;
			pgrBottom.ShowFirstLast = true;
			pgrBottom.CurrentIndex = pageNumber;
			pgrBottom.PageSize = pageSize;
			pgrBottom.PageCount = totalPages;
			pgrBottom.Visible = (totalPages > 1);

			if (searchResults.Count > 0 && searchResults.ItemCount > 0)
			{
				rptResults.Visible = true;
				rptResults.DataSource = searchResults;
				rptResults.DataBind();

			}

		}

		private void BindFeatureList()
		{
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

						ddFeatureList.Items.Add(listItem);
					}

				}

			}

		}

		protected void btnGo_Click(object sender, EventArgs e)
		{
			if (beginDate.Text.Length > 0)
			{
				DateTime.TryParse(beginDate.Text, out modifiedBeginDate);
			}

			if (endDate.Text.Length > 0)
			{
				DateTime.TryParse(endDate.Text, out modifiedEndDate);
			}

			if (ddFeatureList.SelectedValue.Length == 36)
			{
				featureGuid = new Guid(ddFeatureList.SelectedValue);
			}

			string searchUrl = SiteRoot
					+ "/Admin/IndexBrowser.aspx?p=1"
					+ "&bd=" + modifiedBeginDate.Date.ToString("s")
					+ "&ed=" + modifiedEndDate.Date.ToString("s")
					+ "&f=" + featureGuid.ToString();

			WebUtils.SetupRedirect(this, searchUrl);

		}




		protected string FormatItemTitle(string pageName, string moduleTitle, string itemTitle, string separator = "\\")
		{
			string returnValue = string.Empty;

			if (!string.IsNullOrWhiteSpace(pageName))
			{
				returnValue = pageName;
			}

			if (!string.IsNullOrWhiteSpace(moduleTitle))
			{
				if (string.IsNullOrWhiteSpace(returnValue))
				{
					returnValue = moduleTitle;
				}
				else
				{
					returnValue += $" {separator} {moduleTitle}";
				}
			}

			if (!string.IsNullOrWhiteSpace(itemTitle))
			{
				if (string.IsNullOrWhiteSpace(returnValue))
				{
					returnValue = itemTitle;
				}
				else
				{
					returnValue += $" {separator} {itemTitle}";
				}
			}
			return returnValue;
		}

		public string BuildUrl(IndexItem indexItem)
		{
			string value = string.Empty;
			if (indexItem.UseQueryStringParams)
			{
				value = "/" + indexItem.ViewPage
					+ "?pageid="
					+ indexItem.PageId.ToInvariantString()
					+ "&mid="
					+ indexItem.ModuleId.ToInvariantString()
					+ "&ItemID="
					+ indexItem.ItemId.ToInvariantString()
					+ indexItem.QueryStringAddendum;

			}
			else
			{
				value = "/" + indexItem.ViewPage;
			}

			if (value.StartsWith("/"))
			{
				value = SiteRoot + value;
			}

			return value;

		}

		protected void rptResults_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			if (siteSettings == null) { return; }
			if (e.CommandName == "delete")
			{
				string indexKey = e.CommandArgument.ToString();
				IndexHelper.DeleteIndexDoc(siteSettings.SiteId, indexKey);
			}

			// get out of postback and refresh the list
			WebUtils.SetupRedirect(this, Request.RawUrl);

		}

		protected void btnRebuildSearchIndex_Click(object sender, EventArgs e)
		{
			IndexingQueue.DeleteAll();
			IndexHelper.DeleteSearchIndex(siteSettings);
			IndexHelper.VerifySearchIndex(siteSettings);
			rptResults.Visible = false;
			lblMessage.Text = Resource.SearchResultsBuildingIndexMessage;
			Thread.Sleep(5000); //wait 1 seconds
			SiteUtils.QueueIndexing();
		}

		

		protected string FormatProperty(string propVal)
		{
			if (string.IsNullOrWhiteSpace(propVal)) { return Resource.NotApplicable; }

			return propVal;
		}

		protected void rptResults_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item)
			{
				var indexItem = ((IndexItem)e.Item.DataItem);
				//var title = row["Title"].ToString();
				//var moduleTitle = row["ModuleTitle"].ToString();
				//var pageName = row["PageName"].ToString();
				var title = FormatItemTitle(indexItem.PageName, indexItem.ModuleTitle, indexItem.Title);
				Button btnDelete = (Button)e.Item.FindControl("btnDelete");
				if (btnDelete != null)
				{
					btnDelete.Attributes.Add("OnClick", 
						$"return confirm(\"{string.Format(Resource.AdminIndexBrowserDeleteItemWarning, title.JsonEscape())}\");");
				}
			}

		}
	}
}