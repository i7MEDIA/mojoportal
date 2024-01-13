using mojoPortal.Core.Extensions;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web.Framework;
using Resources;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

namespace mojoPortal.Web.Services;

/// <summary>
/// Summary description for SiteMapJson
/// </summary>
public class SiteMapJson : IHttpHandler
{
	private static readonly ILog log = LogManager.GetLogger(typeof(SiteMapJson));

	private HttpContext context = null;
	private SiteMapDataSource siteMapDataSource;
	private bool isAdmin = false;
	private bool isContentAdmin = false;
	private bool isSiteEditor = false;
	private SiteSettings siteSettings;
	private string secureSiteRoot = string.Empty;
	private string insecureSiteRoot = string.Empty;
	private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
	private bool isSecureRequest = false;
	private SiteMapNode rootNode = null;
	private SiteMapNode currentNode = null;
	// private int webOnly = (int)ContentPublishMode.WebOnly;
	//private bool htmlEncodePageNames = false;
	private string rootLabel = string.Empty;
	private string cmd = string.Empty;
	private StringBuilder script = null;
	private int pageId = -1;
	private bool logAllActions = true;
	bool renderChildren = false;
	private string currentUserName = string.Empty;

	public void ProcessRequest(HttpContext c)
	{
		context = c;

		LoadSettings();

		switch (cmd)
		{
			case "move":
				HandleMove();
				break;

			case "del":
				HandleDelete();
				break;

			case "sortalpha":
				SortChildPagesAlpha();
				break;

			case "get":
			default:
				LoadSiteMapSettings();
				RenderSiteMapJson();
				break;
		}

	}



	private void HandleMove()
	{
		PageActionResult result = new PageActionResult();

		if (!context.Request.IsAuthenticated)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("rejected page move request for anonymous user");
			return;

		}

		int movedNodeId = -1;
		int targetNodeId = -1;
		string position = string.Empty;

		if (context.Request.Form["position"] != null) { position = context.Request.Form["position"]; }
		if (context.Request.Form["movedNode"] != null)
		{
			int.TryParse(context.Request.Form["movedNode"], out movedNodeId);
		}
		if (context.Request.Form["targetNode"] != null)
		{
			int.TryParse(context.Request.Form["targetNode"], out targetNodeId);
		}

		//log.Info("movedNode = " + movedNodeId);
		//log.Info("targetNode = " + targetNodeId);
		//log.Info("position = " + position);






		if ((movedNodeId == -1) || (targetNodeId == -1) || (string.IsNullOrEmpty(position)))
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("rejected page move request due to invalid page parameters for user " + currentUserName);
			return;

		}
		LoadSiteMapSettings();

		mojoSiteMapNode movedNode = SiteUtils.GetSiteMapNodeForPage(rootNode, movedNodeId);
		mojoSiteMapNode targetNode = SiteUtils.GetSiteMapNodeForPage(rootNode, targetNodeId);

		if ((movedNode == null) || (targetNode == null))
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Error("movedNode or targetNode was null for user " + currentUserName);
			return;

		}




		if (
			(!isAdmin && !isContentAdmin && !isSiteEditor)
			&& (!WebUser.IsInRoles(movedNode.EditRoles))
			)
		{
			result.Success = false;
			result.Message = string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.MovePageNotAllowedFormat,
				movedNode.Title);

			RenderPageActionResult(result);

			if (logAllActions)
			{
				log.Info(
				string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.MoveNodeRequestDeniedFormat,
				currentUserName,
				movedNode.Title, movedNode.PageId,
				position,
				targetNode.Title, targetNode.PageId
				)
				);

			}

			return;
		}

		mojoSiteMapNode selectedParentNode = null;

		PageSettings movedPage;
		PageSettings targetPage;


		switch (position)
		{
			case "inside":
				// this case is when moving to a new parent node that doesn't have any children yet
				// target is the new parent
				// or when momving to the first position of the current parent
				if (
					(!isAdmin && !isContentAdmin && !isSiteEditor)
				   && (!WebUser.IsInRoles(targetNode.CreateChildPageRoles))
					)
				{
					result.Success = false;
					result.Message = string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveToNewParentNotAllowedFormat,
						targetNode.Title);

					RenderPageActionResult(result);

					if (logAllActions)
					{
						log.Info(
						string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveNodeRequestDeniedFormat,
						currentUserName,
						movedNode.Title, movedNode.PageId,
						position,
						targetNode.Title, targetNode.PageId
						)
						);

					}

					return;
				}

				// change parent page id
				movedPage = new PageSettings(siteSettings.SiteId, movedNode.PageId);
				targetPage = new PageSettings(siteSettings.SiteId, targetNode.PageId);

				movedPage.ParentId = targetPage.PageId;
				movedPage.ParentGuid = targetPage.PageGuid;
				movedPage.PageOrder = 0;

				//reset site map cache   
				movedPage.Save();

				ResortChildPages(movedPage.ParentId);
				CacheHelper.ResetSiteMapCache();

				result.Success = true;
				result.Message = "Success"; // no message is shown for success in the ui

				RenderPageActionResult(result);

				if (logAllActions)
				{
					log.Info(
					string.Format(
					CultureInfo.InvariantCulture,
					PageManagerResources.MoveNodeRequestLogFormat,
					currentUserName,
					movedNode.Title, movedNode.PageId,
					position,
					targetNode.Title, targetNode.PageId
					)
					);

				}

				return;

			//break;

			case "before":
				// put this page before the target page beneath the same parent as the target
				if (targetNode.ParentId != movedNode.ParentId)
				{
					if (targetNode.ParentId == -1)
					{
						//trying to move a page to root
						if ((!isAdmin && !isContentAdmin && !isSiteEditor)
							&& (!WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
							)
						{
							result.Success = false;
							result.Message = PageManagerResources.MoveToRootNotAllowed;

							RenderPageActionResult(result);

							if (logAllActions)
							{
								log.Info(
								string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveNodeRequestDeniedFormat,
								currentUserName,
								movedNode.Title, movedNode.PageId,
								position,
								targetNode.Title, targetNode.PageId
								)
								);

							}
							return;
						}


					}
					else
					{
						selectedParentNode = SiteUtils.GetSiteMapNodeForPage(rootNode, targetNode.ParentId);
						if (
							(!isAdmin && !isContentAdmin && !isSiteEditor)
							&& (!WebUser.IsInRoles(selectedParentNode.CreateChildPageRoles))
							)
						{
							result.Success = false;
							result.Message = string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveToNewParentNotAllowedFormat,
								targetNode.Title);

							RenderPageActionResult(result);

							if (logAllActions)
							{
								log.Info(
								string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveNodeRequestDeniedFormat,
								currentUserName,
								movedNode.Title, movedNode.PageId,
								position,
								targetNode.Title, targetNode.PageId
								)
								);

							}

							return;
						}
					}

					movedPage = new PageSettings(siteSettings.SiteId, movedNode.PageId);
					targetPage = new PageSettings(siteSettings.SiteId, targetNode.PageId);

					movedPage.ParentId = targetPage.ParentId;
					movedPage.ParentGuid = targetPage.ParentGuid;

					// set sort and re-sort
					movedPage.PageOrder = targetPage.PageOrder - 1;
					movedPage.Save();

					ResortChildPages(targetNode.ParentId);
					CacheHelper.ResetSiteMapCache();

					result.Success = true;
					result.Message = "Success"; // no message is shown for success in the ui

					RenderPageActionResult(result);

					if (logAllActions)
					{
						log.Info(
						string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveNodeRequestLogFormat,
						currentUserName,
						movedNode.Title, movedNode.PageId,
						position,
						targetNode.Title, targetNode.PageId
						)
						);

					}
					return;

				}
				else
				{
					//parent did not change just sort if allowed to edit

					movedPage = new PageSettings(siteSettings.SiteId, movedNode.PageId);
					targetPage = new PageSettings(siteSettings.SiteId, targetNode.PageId);

					// set sort and re-sort
					movedPage.PageOrder = targetPage.PageOrder - 1;
					movedPage.Save();

					ResortChildPages(targetNode.ParentId);
					CacheHelper.ResetSiteMapCache();

					result.Success = true;
					result.Message = "Success"; // no message is shown for success in the ui

					RenderPageActionResult(result);

					if (logAllActions)
					{
						log.Info(
						string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveNodeRequestLogFormat,
						currentUserName,
						movedNode.Title, movedNode.PageId,
						position,
						targetNode.Title, targetNode.PageId
						)
						);

					}
					return;

				}

			//break;

			case "after":
			default:
				// put this page after the target page beneath the same parent as the target
				if (targetNode.ParentId != movedNode.ParentId)
				{
					if (targetNode.ParentId == -1)
					{
						//trying to move a page to root
						if (
							(!isAdmin && !isContentAdmin && !isSiteEditor)
							&& (!WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
							)
						{
							result.Success = false;
							result.Message = PageManagerResources.MoveToRootNotAllowed;

							RenderPageActionResult(result);

							if (logAllActions)
							{
								log.Info(
								string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveNodeRequestDeniedFormat,
								currentUserName,
								movedNode.Title, movedNode.PageId,
								position,
								targetNode.Title, targetNode.PageId
								)
								);

							}

							return;
						}


					}
					else
					{
						selectedParentNode = SiteUtils.GetSiteMapNodeForPage(rootNode, targetNode.ParentId);
						if (
							(!isAdmin && !isContentAdmin && !isSiteEditor)
							&& (!WebUser.IsInRoles(selectedParentNode.CreateChildPageRoles))
							)
						{
							result.Success = false;
							result.Message = string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveToNewParentNotAllowedFormat,
								targetNode.Title);

							RenderPageActionResult(result);

							if (logAllActions)
							{
								log.Info(
								string.Format(
								CultureInfo.InvariantCulture,
								PageManagerResources.MoveNodeRequestDeniedFormat,
								currentUserName,
								movedNode.Title, movedNode.PageId,
								position,
								targetNode.Title, targetNode.PageId
								)
								);

							}

							return;
						}
					}



					// change parent page id
					movedPage = new PageSettings(siteSettings.SiteId, movedNode.PageId);
					targetPage = new PageSettings(siteSettings.SiteId, targetNode.PageId);

					movedPage.ParentId = targetPage.ParentId;
					movedPage.ParentGuid = targetPage.ParentGuid;

					// set sort and re-sort
					movedPage.PageOrder = targetPage.PageOrder + 1;
					movedPage.Save();

					ResortChildPages(targetNode.ParentId);
					CacheHelper.ResetSiteMapCache();

					result.Success = true;
					result.Message = "Success"; // no message is shown for success in the ui

					RenderPageActionResult(result);

					if (logAllActions)
					{
						log.Info(
						string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveNodeRequestLogFormat,
						currentUserName,
						movedNode.Title, movedNode.PageId,
						position,
						targetNode.Title, targetNode.PageId
						)
						);

					}
					return;
				}
				else
				{
					//parent did not change just sort

					movedPage = new PageSettings(siteSettings.SiteId, movedNode.PageId);
					targetPage = new PageSettings(siteSettings.SiteId, targetNode.PageId);

					// set sort and re-sort
					movedPage.PageOrder = targetPage.PageOrder + 1;
					movedPage.Save();

					ResortChildPages(targetNode.ParentId);
					CacheHelper.ResetSiteMapCache();

					result.Success = true;
					result.Message = "Success"; // no message is shown for success in the ui

					RenderPageActionResult(result);


					if (logAllActions)
					{
						log.Info(
						string.Format(
						CultureInfo.InvariantCulture,
						PageManagerResources.MoveNodeRequestLogFormat,
						currentUserName,
						movedNode.Title, movedNode.PageId,
						position,
						targetNode.Title, targetNode.PageId
						)
						);

					}
					return;
				}


				//break;


		}




	}

	private void ResortChildPages(int pageId)
	{
		// this is keeping the current sort order of child pages
		// but resetting the sort of each page to ensure the gaps
		// needed for moving a page, ie the resulting sorts will be 1,3,5,7,9...
		PageSettings page = new PageSettings(siteSettings.SiteId, pageId);
		if (page.PageId > -1) { page.ResortPages(); }

	}

	private void HandleDelete()
	{
		PageActionResult result = new PageActionResult();

		if (!context.Request.IsAuthenticated)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("rejected page delete request for anonymous user");
			return;

		}

		int delNodeId = -1;
		if (context.Request.Form["delNode"] != null)
		{
			int.TryParse(context.Request.Form["delNode"], out delNodeId);
		}

		if (delNodeId == -1)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("no pageid provided for delete command");
			return;
		}

		//log.Info("node to delete " + delNodeId);

		LoadSiteMapSettings();

		mojoSiteMapNode deleteNode = SiteUtils.GetSiteMapNodeForPage(rootNode, delNodeId);

		if (deleteNode == null)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("node not found for delete command");
			return;
		}

		if (!WebUser.IsInRoles(deleteNode.EditRoles))
		{
			result.Success = false;
			result.Message = string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.DeletePageNotAllowedFormat,
				deleteNode.Title);

			RenderPageActionResult(result);

			if (logAllActions)
			{
				log.Info(
				string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.DeletePageDeniedLogFormat,
				currentUserName,
				deleteNode.Title, deleteNode.PageId
				)
				);

			}

			return;
		}


		if (deleteNode.ChildNodes.Count > 0)
		{
			if (!WebUser.IsInRoles(siteSettings.RolesThatCanCreateRootPages))
			{
				// child pages would be orphaned to becomne root level
				// but user does not have permission to create root pages

				result.Success = false;
				result.Message = PageManagerResources.CantOrphanPagesWarning;
				RenderPageActionResult(result);
				log.Info("delete would orphan child pages to root but user does not have permission");
				return;

			}


		}

		ContentMetaRespository metaRepository = new ContentMetaRespository();

		PageSettings pageSettings = new PageSettings(siteSettings.SiteId, deleteNode.PageId);
		metaRepository.DeleteByContent(deleteNode.PageGuid);
		Module.DeletePageModules(deleteNode.PageId);
		PageSettings.DeletePage(deleteNode.PageId);
		FriendlyUrl.DeleteByPageGuid(deleteNode.PageGuid);

		mojoPortal.SearchIndex.IndexHelper.ClearPageIndexAsync(pageSettings);

		CacheHelper.ResetSiteMapCache();

		result.Success = true;
		result.Message = "Success";
		RenderPageActionResult(result);

		if (logAllActions)
		{
			log.Info(
			string.Format(
			CultureInfo.InvariantCulture,
			PageManagerResources.DeletePageSuccessLogFormat,
			currentUserName,
			deleteNode.Title, deleteNode.PageId
			)
			);

		}


	}

	private void SortChildPagesAlpha()
	{
		PageActionResult result = new PageActionResult();

		if (!context.Request.IsAuthenticated)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("rejected page sort children request for anonymous user");
			return;

		}

		int selNodeId = -1;
		if (context.Request.Form["selNode"] != null)
		{
			int.TryParse(context.Request.Form["selNode"], out selNodeId);
		}

		if (selNodeId == -1)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("no pageid provided for sort children command");
			return;
		}

		//log.Info("node to sort " + selNodeId);

		LoadSiteMapSettings();

		mojoSiteMapNode sortNode = SiteUtils.GetSiteMapNodeForPage(rootNode, selNodeId);

		if (sortNode == null)
		{
			result.Success = false;
			result.Message = PageManagerResources.InvalidRequest;
			RenderPageActionResult(result);
			log.Info("node not found for sort children command");
			return;
		}

		if (!WebUser.IsInRoles(sortNode.EditRoles))
		{
			result.Success = false;
			result.Message = string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.MovePageNotAllowedFormat,
				sortNode.Title);

			RenderPageActionResult(result);

			if (logAllActions)
			{
				log.Info(
				string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.SortChildPagesDeniedLogFromat,
				currentUserName,
				sortNode.Title, sortNode.PageId
				)
				);

			}

			return;
		}

		if (!WebUser.IsInRoles(sortNode.CreateChildPageRoles))
		{
			result.Success = false;
			result.Message = string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.MovePageNotAllowedFormat,
				sortNode.Title);

			RenderPageActionResult(result);

			if (logAllActions)
			{
				log.Info(
				string.Format(
				CultureInfo.InvariantCulture,
				PageManagerResources.SortChildPagesDeniedLogFromat,
				currentUserName,
				sortNode.Title, sortNode.PageId
				)
				);

			}

			return;
		}

		PageSettings pageSettings = new PageSettings(siteSettings.SiteId, sortNode.PageId);
		pageSettings.ResortPagesAlphabetically();

		CacheHelper.ResetSiteMapCache();

		result.Success = true;
		result.Message = "Success";
		RenderPageActionResult(result);

		if (logAllActions)
		{
			log.Info(
			string.Format(
			CultureInfo.InvariantCulture,
			PageManagerResources.SortChildPagesSuccessLogFormat,
			currentUserName,
			sortNode.Title, sortNode.PageId
			)
			);

		}
	}

	private void RenderPageActionResult(PageActionResult result)
	{
		context.Response.ContentType = "application/json";

		JavaScriptSerializer js = new JavaScriptSerializer();

		var jsonObj = js.Serialize(result);
		context.Response.Write(jsonObj.ToString());

	}

	private void RenderSiteMapJson()
	{

		context.Response.ContentType = "application/json";

		//JsonSerializerSettings jsSettings = new JsonSerializerSettings();
		//jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		////http://stackoverflow.com/questions/5872855/using-json-net-how-do-i-prevent-serializing-properties-of-a-derived-class-when

		////jsSettings.ContractResolver

		//string result = JsonConvert.SerializeObject(new { Data = rootNode }, Formatting.None, jsSettings);

		//context.Response.Write(result);

		script = new StringBuilder();

		script.Append("[");

		if (currentNode == null)
		{
			BuildChildPages(script, (mojoSiteMapNode)rootNode);
		}
		else
		{
			BuildChildPages(script, (mojoSiteMapNode)currentNode);
		}


		script.Append("]");
		context.Response.Write(script.ToString());

	}



	private void BuildChildPages(StringBuilder script, mojoSiteMapNode currentPageNode)
	{
		if (currentPageNode == null) { return; }

		string comma = string.Empty;

		foreach (SiteMapNode childNode in currentPageNode.ChildNodes)
		{
			mojoSiteMapNode mapNode = (mojoSiteMapNode)childNode;

			bool remove = false;
			bool canEdit = isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(mapNode.EditRoles);
			bool canDelete = canEdit;
			bool canCreateChildPages = isAdmin || isContentAdmin || isSiteEditor || WebUser.IsInRoles(mapNode.CreateChildPageRoles);

			if (mapNode.Roles == null)
			{
				if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { remove = true; }
			}
			else
			{
				// filter out content locked down to admins only unless admin
				if ((!isAdmin) && (mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")) { remove = true; }
				// if the user is not an editor filter out nodes where user is not in view roles
				if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.ViewRoles))) { remove = true; }
			}

			//if (!mapNode.IncludeInMenu) { remove = true; }

			if (mapNode.IsPending && !WebUser.IsAdminOrContentAdminOrContentPublisherOrContentAuthor) { remove = true; }


			if (!remove)
			{
				script.Append(comma);
				script.Append("{");
				script.Append("\"id\":" + mapNode.PageId.ToInvariantString());
				script.Append(",\"label\":\"" + Encode(mapNode.Title) + "\"");
				script.Append(",\"Url\":\"" + FormatUrl(mapNode) + "\"");
				script.Append(",\"RelativeUrl\":\"" + mapNode.Url.Replace("~/", "/") + "\"");
				script.Append(",\"IsRoot\":false");
				script.Append($",\"ShowInMenu\": {mapNode.IncludeInMenu.ToString().ToLower()}");
				script.Append($",\"CssClass\": \"{mapNode.MenuCssClass}\"");
				script.Append(",\"ParentId\":" + mapNode.ParentId.ToInvariantString());
				script.Append(",\"childcount\":" + mapNode.ChildNodes.Count.ToInvariantString());
				script.Append(",\"children\":[");
				if (renderChildren && mapNode.ChildNodes.Count > 0)
				{
					BuildChildPages(script, mapNode);
				}

				script.Append("]");
				if (mapNode.ChildNodes.Count > 0)
				{
					script.Append(",\"load_on_demand\":true");
				}

				if (canEdit)
				{
					script.Append(",\"canEdit\":true");
				}
				else
				{
					script.Append(",\"canEdit\":false");
				}


				if (canDelete)
				{
					script.Append(",\"canDelete\":true");
				}
				else
				{
					script.Append(",\"canDelete\":false");
				}

				if (canCreateChildPages)
				{
					script.Append(",\"canCreateChild\":true");
				}
				else
				{
					script.Append(",\"canCreateChild\":false");
				}

				if (mapNode.ViewRoles.Contains("All Users;"))
				{
					script.Append(",\"protection\":\"" + PageManagerResources.Public + "\"");
				}
				else
				{
					script.Append(",\"protection\":\"" + PageManagerResources.Protected + "\"");
				}

				script.Append("}");
				comma = ",";
			}

		}

	}










	//private void BuildCurrentPage(StringBuilder script, mojoSiteMapNode currentPageNode)
	//{
	//    if (currentPageNode != null)
	//    {
	//        script.Append("\"id\":" + currentPageNode.PageId.ToInvariantString());
	//        script.Append(",\"label\":\"" + Encode(currentPageNode.Title) + "\"");
	//        script.Append(",\"Url\":\"" + FormatUrl(currentPageNode) + "\"");
	//        script.Append(",\"IsRoot\":false");
	//        script.Append(",\"ParentId\":" + currentPageNode.ParentId.ToInvariantString());
	//        script.Append(",\"children\":[");
	//        BuildChildPages(script, currentPageNode);
	//        script.Append("]");
	//    }
	//}

	//private void BuildParentTree(StringBuilder script, mojoSiteMapNode pageNode, string suppliedComma)
	//{
	//    string comma = suppliedComma;

	//    if (pageNode == null)
	//    {
	//        script.Append("{}");
	//        return;
	//    }

	//    if (pageNode.IsRootNode)
	//    {
	//        script.Append(comma);
	//        script.Append("{");
	//        script.Append("\"id\":-1");
	//        script.Append(",\"label\":\"" + Encode(rootLabel) + "\"");
	//        script.Append(",\"Url\":\"" + SiteUtils.GetNavigationSiteRoot() + "\"");
	//        script.Append(",\"IsRoot\":true");
	//        script.Append(",\"ParentId\":-2");
	//        script.Append(",\"children\":[");
	//        BuildChildPages(script, pageNode);
	//        script.Append("]");
	//        script.Append("}");

	//        return;
	//    }
	//    else
	//    {
	//        script.Append(comma);
	//        script.Append("{");
	//        script.Append("\"id\":" + pageNode.PageId.ToInvariantString());
	//        script.Append(",\"label\":\"" + Encode(pageNode.Title) + "\"");
	//        script.Append(",\"Url\":\"" + FormatUrl(pageNode) + "\"");
	//        script.Append(",\"IsRoot\":false");
	//        script.Append(",\"ParentId\":" + pageNode.ParentId.ToInvariantString());
	//        script.Append(",\"ChildPages\":[");
	//        BuildChildPages(script, pageNode);
	//        script.Append("]");
	//        script.Append("}");
	//        comma = ",";

	//        if (pageNode.ParentNode != null)
	//        {
	//            BuildParentTree(script, (mojoSiteMapNode)pageNode.ParentNode, comma);
	//        }
	//    }
	//}

	public static string JsonEscape(string s)
	{
		StringBuilder sb = new StringBuilder();
		//sb.Append("\"");
		foreach (char c in s)
		{
			switch (c)
			{
				case '\"':
					sb.Append("\\\"");
					break;
				case '\\':
					sb.Append("\\\\");
					break;
				case '\b':
					sb.Append("\\b");
					break;
				case '\f':
					sb.Append("\\f");
					break;
				case '\n':
					sb.Append("\\n");
					break;
				case '\r':
					sb.Append("\\r");
					break;
				case '\t':
					sb.Append("\\t");
					break;
				default:
					int i = (int)c;
					if (i < 32 || i > 127)
					{
						sb.AppendFormat("\\u{0:X04}", i);
					}
					else
					{
						sb.Append(c);
					}
					break;
			}
		}
		//sb.Append("\"");

		return sb.ToString();
	}

	private string Encode(string input)
	{
		//return HttpUtility.HtmlEncode(input); 
		return JsonEscape(HttpUtility.HtmlDecode(input));

	}

	private string FormatUrl(SiteMapNode mapNode)
	{
		return FormatUrl((mojoSiteMapNode)mapNode);
	}

	private string FormatUrl(mojoSiteMapNode mapNode)
	{
		string itemUrl = WebUtils.ResolveServerUrl(mapNode.Url);
		if (resolveFullUrlsForMenuItemProtocolDifferences)
		{
			if (isSecureRequest)
			{
				if (
					(!mapNode.UseSsl)
					&& (!siteSettings.UseSslOnAllPages)
					&& (mapNode.Url.StartsWith("~/"))
					)
				{
					itemUrl = insecureSiteRoot + mapNode.Url.Replace("~/", "/");
				}
			}
			else
			{
				if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
				{
					if (mapNode.Url.StartsWith("~/"))
						itemUrl = secureSiteRoot + mapNode.Url.Replace("~/", "/");
				}
			}
		}

		return itemUrl;
	}

	private void LoadSiteMapSettings()
	{
		resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
		if (resolveFullUrlsForMenuItemProtocolDifferences)
		{
			secureSiteRoot = WebUtils.GetSecureSiteRoot();
			insecureSiteRoot = WebUtils.GetInSecureSiteRoot();
		}

		isSecureRequest = mojoPortal.Core.Helpers.WebHelper.IsSecureRequest();

		//siteMapDataSource = (SiteMapDataSource)this.Page.Master.FindControl("SiteMapData");
		siteMapDataSource = new SiteMapDataSource();
		siteMapDataSource.SiteMapProvider = "mojosite" + siteSettings.SiteId.ToInvariantString();

		rootNode = siteMapDataSource.Provider.RootNode;

		pageId = WebUtils.ParseInt32FromQueryString("node", pageId);
		if (pageId > -1)
		{
			currentNode = SiteUtils.GetSiteMapNodeForPage(rootNode, pageId);
		}
		//if (currentNode == null) { currentNode = rootNode; }
		renderChildren = WebUtils.ParseBoolFromQueryString("renderchildren", renderChildren);
		if (rootLabel.Length == 0) { rootLabel = ResourceHelper.GetResourceString("Resource", "PageSettingsRootLabel"); }
	}

	private void LoadSettings()
	{
		siteSettings = CacheHelper.GetCurrentSiteSettings();

		isAdmin = WebUser.IsAdmin;
		if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
		if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

		if (context.Request.QueryString["cmd"] != null) { cmd = context.Request.QueryString["cmd"]; }

		logAllActions = mojoPortal.Core.Configuration.ConfigHelper.GetBoolProperty("PageManager:LogAllActions", logAllActions);
		if (context.Request.IsAuthenticated)
		{
			currentUserName = context.User.Identity.Name;
		}
	}

	public bool IsReusable
	{
		get
		{
			return false;
		}
	}
}


public class PageActionResult
{
	public PageActionResult() { }

	private bool success = false;
	public bool Success
	{
		get { return success; }
		set { success = value; }
	}

	private string message = string.Empty;

	public string Message
	{
		get { return message; }
		set { message = value; }
	}

}

