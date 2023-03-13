using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Business.WebHelpers;
using Resources;
using System.Collections.Generic;
using System.Linq;

namespace mojoPortal.Web.UI
{
	/// <summary>
	/// A convenience link for the File Manager. The link renders only for those in roles that can use it
	/// </summary>
	public class FileManagerLink : HyperLink
	{
		private mojoBasePage basePage = null;

		private bool renderAsListItem = false;
		public bool RenderAsListItem
		{
			get { return renderAsListItem; }
			set { renderAsListItem = value; }
		}

		private string listItemCSS = string.Empty;
		public string ListItemCss
		{
			get { return listItemCSS; }
			set { listItemCSS = value; }
		}

		private string listItemID = string.Empty;
		public string ListItemID
		{
			get { return listItemID; }
			set { listItemID = value; }
		}

		private bool openInModal = true;
		public bool OpenInModal
		{
			get { return openInModal; }
			set { openInModal = value; }
		}

		private string modalHookType = "CssClass"; // There's two types, "CssClass" and "Attributes". The value of Attributes should be key:value pairs that are comma seperated
		public string ModalHookType
		{
			get { return modalHookType; }
			set { modalHookType = value; }
		}

		private string modalHookValue = "cblink";
		public string ModalHookValue
		{
			get { return modalHookValue; }
			set { modalHookValue = value; }
		}

		private string queryString = string.Empty;
		public string QueryString
		{
			get { return queryString; }
			set { queryString = value; }
		}

		private string literalExtraTopContent = string.Empty;
		public string LiteralExtraTopContent
		{
			get { return literalExtraTopContent; }
			set { literalExtraTopContent = value; }
		}

		private string literalExtraBottomContent = string.Empty;
		public string LiteralExtraBottomContent
		{
			get { return literalExtraBottomContent; }
			set { literalExtraBottomContent = value; }
		}

		private string linkImageUrl = string.Empty;
		public string LinkImageUrl
		{
			get { return linkImageUrl; }
			set { linkImageUrl = value; }
		}

		public bool ShouldRender()
		{
			if (basePage == null) {
				return false;
			}

			if (!Page.Request.IsAuthenticated) {
				return false;
			}

			if (!WebConfigSettings.ShowFileManagerLink) {
				return false;
			}

			if (WebConfigSettings.DisableFileManager) {
				return false;
			}

			if (basePage.SiteInfo == null) {
				return false;
			}

			if ((!CacheHelper.GetCurrentSiteSettings().IsServerAdminSite) && (!WebConfigSettings.AllowFileManagerInChildSites))
			{
				return false;
			}

			if (SiteUtils.UserIsSiteEditor() || WebUser.IsAdminOrContentAdmin) {
				return true;
			}

			// Only roles that can delete can use File Manager
			if (!WebUser.IsInRoles(basePage.SiteInfo.RolesThatCanDeleteFilesInEditor)) {
				return false;
			}

			if (WebUser.IsInRoles(basePage.SiteInfo.UserFilesBrowseAndUploadRoles)
				|| WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseAndUploadRoles)
				|| WebUser.IsInRoles(basePage.SiteInfo.GeneralBrowseRoles))
			{
				return true;
			}

			return false;
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if (HttpContext.Current == null) {
				return;
			}

			EnableViewState = false;
			basePage = Page as mojoBasePage;
			Visible = ShouldRender();

			if (!Visible) {
				return;
			}

			if (basePage == null) {
				return;
			}


			if (string.IsNullOrWhiteSpace(Global.SkinConfig.ModalTemplatePath) || string.IsNullOrWhiteSpace(Global.SkinConfig.ModalScriptPath))
			{
				basePage.ScriptConfig.IncludeColorBox = true;
			}
			else
			{
				basePage.EnsureDefaultModal();
			}

			if (openInModal && modalHookType == "CssClass")
			{
				CssClass = $"adminlink filemanlink {CssClass} {modalHookValue}";
			}
			else
			{
				CssClass = $"adminlink filemanlink {CssClass}";
			}
			
			if (openInModal && (modalHookType == "Attributes"))
			{
				Dictionary<string, string> keyValuePairs = modalHookValue.Split(',')
					.Select(v => v.Split(':'))
					.ToDictionary(pair => pair[0], pair => pair[1]);

				foreach (KeyValuePair<string, string> item in keyValuePairs)
				{
					Attributes.Add(item.Key, item.Value);
				}
			}

			Controls.Add(new Literal
			{
				Text = literalExtraTopContent
			});

			Controls.Add(new Literal
			{
				Text = Resource.AdminMenuFileManagerLink
			});

			Controls.Add(new Literal
			{
				Text = literalExtraBottomContent
			});
			
			ToolTip = Resource.AdminMenuFileManagerLink;

			if (SiteUtils.SslIsAvailable())
			{
				NavigateUrl = basePage.SiteRoot + "/FileManager" + queryString;
			}
			else
			{
				NavigateUrl = basePage.RelativeSiteRoot + "/FileManager" + queryString;
			}

			if (!string.IsNullOrWhiteSpace(linkImageUrl) && !openInModal)
			{
				if (linkImageUrl.StartsWith("~/"))
				{
					ImageUrl = Page.ResolveUrl(linkImageUrl);
				}
				else
				{
					string skinPath = SiteUtils.GetSkinBaseUrl(Page);

					ImageUrl = Page.ResolveUrl(skinPath + linkImageUrl.TrimStart('/'));
				}
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (HttpContext.Current == null)
			{
				writer.Write("[" + ID + "]");
				return;
			}

			if (renderAsListItem)
			{
				string liClass = string.Empty;
				string liID = string.Empty;

				if (listItemID.Length > 0)
				{
					liID = " id=\"" + listItemID + "\"";
				}

				if (listItemCSS.Length > 0)
				{
					liClass = " class=\"" + listItemCSS + "\"";
				}

				writer.Write("<li" + liID + liClass + ">");
			}

			base.Render(writer);

			if (renderAsListItem)
			{
				writer.Write("</li>");
			}
		}
	}
}