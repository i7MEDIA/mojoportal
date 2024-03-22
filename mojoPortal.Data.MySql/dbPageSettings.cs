using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;

namespace mojoPortal.Data;

public static class DBPageSettings
{
	public static int Create(
		int siteId,
		int parentId,
		string pageName,
		string pageTitle,
		string skin,
		int pageOrder,
		string authorizedRoles,
		string editRoles,
		string draftEditRoles,
		string draftApprovalRoles,
		string createChildPageRoles,
		string createChildDraftRoles,
		bool requireSsl,
		bool allowBrowserCache,
		bool showBreadcrumbs,
		bool showChildPageBreadcrumbs,
		string pageKeyWords,
		string pageDescription,
		string pageEncoding,
		string additionalMetaTags,
		bool useUrl,
		string url,
		bool openInNewWindow,
		bool showChildPageMenu,
		bool hideMainMenu,
		bool includeInMenu,
		string menuImage,
		string changeFrequency,
		string siteMapPriority,
		Guid pageGuid,
		Guid parentGuid,
		bool hideAfterLogin,
		Guid siteGuid,
		string compiledMeta,
		DateTime compiledMetaUtc,
		bool includeInSiteMap,
		bool isClickable,
		bool showHomeCrumb,
		bool isPending,
		string canonicalOverride,
		bool includeInSearchMap,
		bool enableComments,
		bool includeInChildSiteMap,
		bool expandOnSiteMap,
		Guid pubTeamId,
		string bodyCssClass,
		string menuCssClass,
		int publishMode,
		Guid createdBy,
		string createdFromIp,
		string menuDescription,
		string linkRel,
		string pageHeading,
		bool showPageHeading,
		DateTime pubDateUtc)
	{

		#region byte conversion

		int intIncludeInSearchMap = 0;
		if (includeInSearchMap) { intIncludeInSearchMap = 1; }

		int intExpandOnSiteMap = 0;
		if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

		int intenableComments = 0;
		if (enableComments) { intenableComments = 1; }

		int intincludeInSiteMap = 0;
		if (includeInSiteMap)
		{
			intincludeInSiteMap = 1;
		}

		int intisClickable = 0;
		if (isClickable)
		{
			intisClickable = 1;
		}

		int intshowHomeCrumb = 0;
		if (showHomeCrumb)
		{
			intshowHomeCrumb = 1;
		}

		byte hideauth = 0;
		if (hideAfterLogin)
		{
			hideauth = 1;
		}

		byte ssl = 0;
		if (requireSsl)
		{
			ssl = 1;
		}

		byte show = 0;
		if (showBreadcrumbs)
		{
			show = 1;
		}

		byte u = 0;
		if (useUrl)
		{
			u = 1;
		}

		byte nw = 0;
		if (openInNewWindow)
		{
			nw = 1;
		}

		byte cm = 0;
		if (showChildPageMenu)
		{
			cm = 1;
		}

		byte cb = 0;
		if (showChildPageBreadcrumbs)
		{
			cb = 1;
		}

		byte hm = 0;
		if (hideMainMenu)
		{
			hm = 1;
		}

		byte inMenu = 0;
		if (includeInMenu)
		{
			inMenu = 1;
		}

		int intIsPending = 0;
		if (isPending)
		{
			intIsPending = 1;
		}

		int intIncludeInChildSiteMap = 0;
		if (includeInChildSiteMap)
		{
			intIncludeInChildSiteMap = 1;
		}

		int intShowPageHeading = 0;
		if (showPageHeading)
		{
			intShowPageHeading = 1;
		}

		#endregion

		string sqlCommand = @"
INSERT INTO 
    mp_Pages ( 
        SiteID, 
        ParentID, 
        PageName, 
        PageTitle, 
        PageOrder, 
        AuthorizedRoles, 
        EditRoles, 
        DraftEditRoles, 
        DraftApprovalRoles, 
        CreateChildPageRoles, 
        CreateChildDraftRoles, 
        RequireSSL, 
        AllowBrowserCache, 
        ShowBreadcrumbs, 
        PageKeyWords, 
        PageDescription, 
        PageEncoding, 
        AdditionalMetaTags, 
        UseUrl, 
        Url, 
        OpenInNewWindow, 
        ShowChildPageMenu, 
        ShowChildBreadcrumbs, 
        HideMainMenu, 
        Skin, 
        MenuImage, 
        IncludeInMenu, 
        ChangeFrequency, 
        SiteMapPriority, 
        LastModifiedUTC, 
        PageGuid, 
        ParentGuid, 
        HideAfterLogin, 
        CanonicalOverride, 
        IncludeInSearchMap, 
        EnableComments, 
        IncludeInSiteMap, 
        IsClickable, 
        ShowHomeCrumb, 
        IsPending, 
        IncludeInChildSiteMap, 
        ExpandOnSiteMap, 
        PubTeamId, 
        PublishMode, 
        BodyCssClass, 
        MenuCssClass, 
        SiteGuid, 
        CompiledMeta, 
        CompiledMetaUtc, 
        MenuDesc, 
        LinkRel, 
        PageHeading, 
        ShowPageHeading, 
        PubDateUtc, 
        PCreatedUtc, 
        PCreatedBy, 
        PCreatedFromIp, 
        PLastModUtc, 
        PLastModBy, 
        PLastModFromIp 
    )
VALUES (
    ?SiteID , 
    ?ParentID , 
    ?PageName , 
    ?PageTitle , 
    ?PageOrder , 
    ?AuthorizedRoles , 
    ?EditRoles , 
    ?DraftEditRoles, 
    ?DraftApprovalRoles, 
    ?CreateChildPageRoles , 
    ?CreateChildDraftRoles, 
    ?RequireSSL , 
    ?AllowBrowserCache, 
    ?ShowBreadcrumbs , 
    ?PageKeyWords , 
    ?PageDescription , 
    ?PageEncoding , 
    ?AdditionalMetaTags,  
    ?UseUrl,  
    ?Url,  
    ?OpenInNewWindow,  
    ?ShowChildPageMenu,  
    ?ShowChildPageBreadcrumbs,  
    ?HideMainMenu,  
    ?Skin,  
    ?MenuImage,  
    ?IncludeInMenu,  
    ?ChangeFrequency,  
    ?SiteMapPriority,  
    ?LastModifiedUTC,  
    ?PageGuid,  
    ?ParentGuid,  
    ?HideAfterLogin, 
    ?CanonicalOverride, 
    ?IncludeInSearchMap, 
    ?EnableComments, 
    ?IncludeInSiteMap, 
    ?IsClickable, 
    ?ShowHomeCrumb, 
    ?IsPending, 
    ?IncludeInChildSiteMap, 
    ?ExpandOnSiteMap, 
    ?PubTeamId, 
    ?PublishMode, 
    ?BodyCssClass, 
    ?MenuCssClass, 
    ?SiteGuid, 
    ?CompiledMeta, 
    ?CompiledMetaUtc, 
    ?MenuDesc, 
    ?LinkRel, 
    ?PageHeading, 
    ?ShowPageHeading, 
    ?PubDateUtc, 
    ?PCreatedUtc, 
    ?PCreatedBy, 
    ?PCreatedFromIp, 
    ?PLastModUtc, 
    ?PLastModBy, 
    ?PLastModFromIp 
);
SELECT LAST_INSERT_ID();";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			},

			new("?PageName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageName
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			},

			new("?AuthorizedRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = authorizedRoles
			},

			new("?RequireSSL", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ssl
			},

			new("?ShowBreadcrumbs", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = show
			},

			new("?PageKeyWords", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = pageKeyWords
			},

			new("?PageDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageDescription
			},

			new("?PageEncoding", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageEncoding
			},

			new("?AdditionalMetaTags", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = additionalMetaTags
			},

			new("?UseUrl", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = u
			},

			new("?Url", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?OpenInNewWindow", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = nw
			},

			new("?ShowChildPageMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cm
			},

			new("?EditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = editRoles
			},

			new("?CreateChildPageRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = createChildPageRoles
			},

			new("?ShowChildPageBreadcrumbs", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cb
			},

			new("?HideMainMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = hm
			},

			new("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},

			new("?IncludeInMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inMenu
			},

			new("?MenuImage", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = menuImage
			},

			new("?PageTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitle
			},

			new("?AllowBrowserCache", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = allowBrowserCache
			},

			new("?ChangeFrequency", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = changeFrequency
			},

			new("?SiteMapPriority", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = siteMapPriority
			},

			new("?LastModifiedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},

			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},

			new("?HideAfterLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = hideauth
			},

			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid.ToString()
			},

			new("?CompiledMeta", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMeta
			},

			new("?CompiledMetaUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMetaUtc
			},

			new("?IncludeInSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSiteMap
			},

			new("?IsClickable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intisClickable
			},

			new("?ShowHomeCrumb", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intshowHomeCrumb
			},

			new("?DraftEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftEditRoles
			},

			new("?IsPending", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsPending
			},

			new("?CanonicalOverride", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = canonicalOverride
			},

			new("?IncludeInSearchMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInSearchMap
			},

			new("?EnableComments", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intenableComments
			},

			new("?CreateChildDraftRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = createChildDraftRoles
			},

			new("?IncludeInChildSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInChildSiteMap
			},

			new("?PubTeamId", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pubTeamId.ToString()
			},

			new("?BodyCssClass", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = bodyCssClass
			},

			new("?MenuCssClass", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = menuCssClass
			},

			new("?ExpandOnSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intExpandOnSiteMap
			},

			new("?PublishMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = publishMode
			},


			new("?PCreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?PCreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy
			},

			new("?PCreatedFromIp", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdFromIp
			},

			new("?PLastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?PLastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy
			},

			new("?PLastModFromIp", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdFromIp
			},

			new("?MenuDesc", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = menuDescription
			},

			new("?DraftApprovalRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftApprovalRoles
			},

			new("?LinkRel", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = linkRel
			},

			new("?PageHeading", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageHeading
			},

			new("?ShowPageHeading", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowPageHeading
			},

			//new("?PubDateUtc", MySqlDbType.DateTime)
			//{
			//	Direction = ParameterDirection.Input,
			//}
		};

		if (pubDateUtc == DateTime.MaxValue)
		{
			arParams.Add(new("?PubDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?PubDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = pubDateUtc
			});
		}

		int newID = Convert.ToInt32(CommandHelper.ExecuteScalar(
					ConnectionString.GetWrite(),
					sqlCommand,
					arParams).ToString());

		return newID;
	}

	public static bool UpdatePage(
		int siteId,
		int pageId,
		int parentId,
		string pageName,
		string pageTitle,
		string skin,
		int pageOrder,
		string authorizedRoles,
		string editRoles,
		string draftEditRoles,
		string draftApprovalRoles,
		string createChildPageRoles,
		string createChildDraftRoles,
		bool requireSsl,
		bool allowBrowserCache,
		bool showBreadcrumbs,
		bool showChildPageBreadcrumbs,
		string pageKeyWords,
		string pageDescription,
		string pageEncoding,
		string additionalMetaTags,
		bool useUrl,
		string url,
		bool openInNewWindow,
		bool showChildPageMenu,
		bool hideMainMenu,
		bool includeInMenu,
		string menuImage,
		string changeFrequency,
		string siteMapPriority,
		Guid parentGuid,
		bool hideAfterLogin,
		string compiledMeta,
		DateTime compiledMetaUtc,
		bool includeInSiteMap,
		bool isClickable,
		bool showHomeCrumb,
		bool isPending,
		string canonicalOverride,
		bool includeInSearchMap,
		bool enableComments,
		bool includeInChildSiteMap,
		bool expandOnSiteMap,
		Guid pubTeamId,
		string bodyCssClass,
		string menuCssClass,
		int publishMode,
		DateTime createdUtc,
		Guid createdBy,
		Guid lastModBy,
		string lastModFromIp,
		string menuDescription,
		string linkRel,
		string pageHeading,
		bool showPageHeading,
		DateTime pubDateUtc)
	{

		#region byte conversion

		int intIncludeInSearchMap = 0;
		if (includeInSearchMap) { intIncludeInSearchMap = 1; }

		int intExpandOnSiteMap = 0;
		if (expandOnSiteMap) { intExpandOnSiteMap = 1; }

		int intenableComments = 0;
		if (enableComments) { intenableComments = 1; }

		int intincludeInSiteMap = 0;
		if (includeInSiteMap)
		{
			intincludeInSiteMap = 1;
		}

		int intisClickable = 0;
		if (isClickable)
		{
			intisClickable = 1;
		}

		int intshowHomeCrumb = 0;
		if (showHomeCrumb)
		{
			intshowHomeCrumb = 1;
		}

		byte hideauth;
		if (hideAfterLogin)
		{
			hideauth = 1;
		}
		else
		{
			hideauth = 0;
		}

		byte ssl;
		if (requireSsl)
		{
			ssl = 1;
		}
		else
		{
			ssl = 0;
		}

		byte show;
		if (showBreadcrumbs)
		{
			show = 1;
		}
		else
		{
			show = 0;
		}

		byte u;
		if (useUrl)
		{
			u = 1;
		}
		else
		{
			u = 0;
		}

		byte nw;
		if (openInNewWindow)
		{
			nw = 1;
		}
		else
		{
			nw = 0;
		}

		byte cm;
		if (showChildPageMenu)
		{
			cm = 1;
		}
		else
		{
			cm = 0;
		}

		byte cb;
		if (showChildPageBreadcrumbs)
		{
			cb = 1;
		}
		else
		{
			cb = 0;
		}

		byte hm;
		if (hideMainMenu)
		{
			hm = 1;
		}
		else
		{
			hm = 0;
		}

		byte inMenu;
		if (includeInMenu)
		{
			inMenu = 1;
		}
		else
		{
			inMenu = 0;
		}

		int intIsPending = 0;
		if (isPending)
		{
			intIsPending = 1;
		}

		int intIncludeInChildSiteMap = 0;
		if (includeInChildSiteMap)
		{
			intIncludeInChildSiteMap = 1;
		}

		int intShowPageHeading = 0;
		if (showPageHeading)
		{
			intShowPageHeading = 1;
		}

		#endregion

		string sqlCommand = @"
UPDATE 
    mp_Pages 
SET 
    PageOrder = ?PageOrder , 
    ParentID = ?ParentID,  
    PageName = ?PageName  , 
    PageTitle = ?PageTitle  , 
    AuthorizedRoles = ?AuthorizedRoles  , 
    EditRoles = ?EditRoles  , 
    DraftEditRoles = ?DraftEditRoles  , 
    DraftApprovalRoles = ?DraftApprovalRoles  , 
    CreateChildPageRoles = ?CreateChildPageRoles  , 
    CreateChildDraftRoles = ?CreateChildDraftRoles  , 
    RequireSSL = ?RequireSSL , 
    AllowBrowserCache = ?AllowBrowserCache , 
    ShowBreadcrumbs = ?ShowBreadcrumbs, 
    PageKeyWords = ?PageKeyWords , 
    PageDescription = ?PageDescription , 
    PageEncoding = ?PageEncoding , 
    AdditionalMetaTags = ?AdditionalMetaTags,  
    UseUrl = ?UseUrl,  
    Url = ?Url,  
    OpenInNewWindow = ?OpenInNewWindow,  
    ShowChildPageMenu = ?ShowChildPageMenu,  
    ShowChildBreadcrumbs = ?ShowChildPageBreadcrumbs,  
    HideMainMenu = ?HideMainMenu,  
    Skin = ?Skin,  
    MenuImage = ?MenuImage,  
    IncludeInMenu = ?IncludeInMenu,  
    ChangeFrequency = ?ChangeFrequency,  
    SiteMapPriority = ?SiteMapPriority,  
    LastModifiedUTC = ?LastModifiedUTC,  
    ParentGuid = ?ParentGuid,  
    HideAfterLogin = ?HideAfterLogin, 
    CanonicalOverride = ?CanonicalOverride, 
    IncludeInSearchMap = ?IncludeInSearchMap, 
    IncludeInSiteMap = ?IncludeInSiteMap, 
    EnableComments = ?EnableComments, 
    IsClickable = ?IsClickable, 
    ShowHomeCrumb = ?ShowHomeCrumb, 
    IsPending = ?IsPending  , 
    IncludeInChildSiteMap = ?IncludeInChildSiteMap  , 
    ExpandOnSiteMap = ?ExpandOnSiteMap  , 
    PubTeamId = ?PubTeamId  , 
    PublishMode = ?PublishMode  , 
    BodyCssClass = ?BodyCssClass  , 
    MenuCssClass = ?MenuCssClass  , 
    CompiledMeta = ?CompiledMeta, 
    CompiledMetaUtc = ?CompiledMetaUtc, 
    MenuDesc = ?MenuDesc, 
    LinkRel = ?LinkRel, 
    PageHeading = ?PageHeading, 
    ShowPageHeading = ?ShowPageHeading, 
    PubDateUtc = ?PubDateUtc, 
    PCreatedUtc = ?PCreatedUtc  , 
    PCreatedBy = ?PCreatedBy  , 
    PLastModUtc = ?PLastModUtc  , 
    PLastModBy = ?PLastModBy, 
    PLastModFromIp = ?PLastModFromIp 
WHERE PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			},

			new("?PageName", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageName
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			},

			new("?AuthorizedRoles", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = authorizedRoles
			},

			new("?PageKeyWords", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = pageKeyWords
			},

			new("?PageDescription", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageDescription
			},

			new("?PageEncoding", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageEncoding
			},

			new("?AdditionalMetaTags", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = additionalMetaTags
			},

			new("?RequireSSL", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ssl
			},

			new("?ShowBreadcrumbs", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = show
			},

			new("?UseUrl", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = u
			},

			new("?Url", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = url
			},

			new("?OpenInNewWindow", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = nw
			},

			new("?ShowChildPageMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cm
			},

			new("?EditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = editRoles
			},

			new("?CreateChildPageRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = createChildPageRoles
			},

			new("?ShowChildPageBreadcrumbs", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = cb
			},

			new("?HideMainMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = hm
			},

			new("?Skin", MySqlDbType.VarChar, 100)
			{
				Direction = ParameterDirection.Input,
				Value = skin
			},

			new("?IncludeInMenu", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = inMenu
			},

			new("?MenuImage", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = menuImage
			},

			new("?PageTitle", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageTitle
			},

			new("?AllowBrowserCache", MySqlDbType.Bit)
			{
				Direction = ParameterDirection.Input,
				Value = allowBrowserCache
			},

			new("?ChangeFrequency", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = changeFrequency
			},

			new("?SiteMapPriority", MySqlDbType.VarChar, 10)
			{
				Direction = ParameterDirection.Input,
				Value = siteMapPriority
			},

			new("?LastModifiedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?ParentGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = parentGuid.ToString()
			},

			new("?HideAfterLogin", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = hideauth
			},

			new("?CompiledMeta", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMeta
			},

			new("?CompiledMetaUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = compiledMetaUtc
			},

			new("?IncludeInSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intincludeInSiteMap
			},

			new("?IsClickable", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intisClickable
			},

			new("?ShowHomeCrumb", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intshowHomeCrumb
			},

			new("?DraftEditRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftEditRoles
			},

			new("?IsPending", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIsPending
			},

			new("?CanonicalOverride", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = canonicalOverride
			},

			new("?IncludeInSearchMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInSearchMap
			},

			new("?EnableComments", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intenableComments
			},

			new("?CreateChildDraftRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = createChildDraftRoles
			},

			new("?IncludeInChildSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludeInChildSiteMap
			},

			new("?PubTeamId", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pubTeamId.ToString()
			},

			new("?BodyCssClass", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = bodyCssClass
			},

			new("?MenuCssClass", MySqlDbType.VarChar, 50)
			{
				Direction = ParameterDirection.Input,
				Value = menuCssClass
			},

			new("?ExpandOnSiteMap", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intExpandOnSiteMap
			},

			new("?PublishMode", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = publishMode
			},

			new("?PCreatedUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = createdUtc
			},

			new("?PCreatedBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = createdBy
			},

			new("?PLastModUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DateTime.UtcNow
			},

			new("?PLastModBy", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModBy
			},

			new("?PLastModFromIp", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = lastModFromIp
			},

			new("?MenuDesc", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = menuDescription
			},

			new("?DraftApprovalRoles", MySqlDbType.Text)
			{
				Direction = ParameterDirection.Input,
				Value = draftApprovalRoles
			},

			new("?LinkRel", MySqlDbType.VarChar, 20)
			{
				Direction = ParameterDirection.Input,
				Value = linkRel
			},

			new("?PageHeading", MySqlDbType.VarChar, 255)
			{
				Direction = ParameterDirection.Input,
				Value = pageHeading
			},

			new("?ShowPageHeading", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intShowPageHeading
			},

			new("?PubDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
			}
		};

		if (pubDateUtc == DateTime.MaxValue)
		{
			arParams.Add(new("?PubDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = DBNull.Value
			});
		}
		else
		{
			arParams.Add(new("?PubDateUtc", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = pubDateUtc
			});
		}


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;



	}

	public static int GetNextPageOrder(
		int siteId,
		int parentId)
	{
		string sqlCommand = @"
SELECT	COALESCE(MAX(PageOrder),-1) 
FROM	mp_Pages 
WHERE	SiteID = ?SiteID 
	AND ParentID = ?ParentID ;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			}
		};

		int nextPageOrder = Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams)) + 2;
		if (nextPageOrder == 1)
		{
			nextPageOrder = 3;
		}

		return nextPageOrder;

	}

	public static IDataReader GetPage(Guid pageGuid)
	{
		string sqlCommand = @"
SELECT 
		p.*, 
		u1.Name As CreatedByName, 
		u1.Email As CreatedByEmail, 
		u1.FirstName As CreatedByFirstName, 
		u1.LastName As CreatedByLastName, 
		u2.Name As LastModByName, 
		u2.Email As LastModByEmail, 
		u2.FirstName As LastModByFirstName, 
		u2.LastName As LastModByLastName 
FROM	mp_Pages p 
LEFT OUTER JOIN	mp_Users u1 ON p.PCreatedBy = u1.UserGuid 
LEFT OUTER JOIN	mp_Users u2 ON p.PLastModBy = u2.UserGuid 
WHERE	p.PageGuid = ?PageGuid  
LIMIT	1;";

		var param = new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36)
		{
			Direction = ParameterDirection.Input,
			Value = pageGuid.ToString()
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			param);
	}

	public static IDataReader GetPage(int siteId, int pageId)
	{

		string sqlCommand = @"
SELECT	p.*, 
		u1.Name As CreatedByName, 
		u1.Email As CreatedByEmail, 
		u1.FirstName As CreatedByFirstName, 
		u1.LastName As CreatedByLastName, 
		u2.Name As LastModByName, 
		u2.Email As LastModByEmail, 
		u2.FirstName As LastModByFirstName, 
		u2.LastName As LastModByLastName 
FROM	mp_Pages p 
LEFT OUTER JOIN	mp_Users u1 ON p.PCreatedBy = u1.UserGuid 
LEFT OUTER JOIN	mp_Users u2 ON p.PLastModBy = u2.UserGuid 
WHERE	(p.PageID = ?PageID OR ?PageID = -1)  
AND		p.SiteID = ?SiteID
ORDER BY p.ParentID, p.PageOrder  
LIMIT 1;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetChildPages(int siteId, int parentPageId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Pages 
WHERE ParentID = ?ParentPageID  
AND SiteID = ?SiteID 
ORDER BY PageOrder;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ParentPageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentPageId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static bool UpdateTimestamp(int pageId, DateTime lastModifiedUtc)
	{
		string sqlCommand = @"
UPDATE mp_Pages 
SET LastModifiedUTC = ?LastModifiedUTC 
WHERE PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?LastModifiedUTC", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = lastModifiedUtc
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;
	}

	public static bool UpdatePageOrder(int pageId, int pageOrder)
	{
		string sqlCommand = @"
UPDATE mp_Pages 
SET PageOrder = ?PageOrder 
WHERE PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?PageOrder", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageOrder
			}
		};


		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;
	}

	public static bool DeletePage(int pageId)
	{
		string sqlCommand = @"
DELETE FROM mp_Pages 
WHERE PageID = ?PageID;";

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			}
		};

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}

	public static bool CleanupOrphans()
	{
		string sqlCommand = @"
UPDATE mp_Pages AS p1 
LEFT JOIN ( SELECT * FROM mp_Pages ) AS p2 
ON p1.ParentID = p2.PageID 
SET p1.ParentID = -1, p1.ParentGuid = '00000000-0000-0000-0000-000000000000' 
WHERE p1.ParentID <> -1 
AND p2.PageID IS NULL;";

		int rowsAffected = CommandHelper.ExecuteNonQuery(
			ConnectionString.GetWrite(),
			sqlCommand);

		return rowsAffected > 0;
	}

	public static IDataReader GetPageList(int siteId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Pages 
WHERE SiteID = ?SiteID 
ORDER BY ParentID, PageOrder, PageName;";


		var param = new MySqlParameter("?SiteID", MySqlDbType.Int32)
		{
			Direction = ParameterDirection.Input,
			Value = siteId
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			param);
	}

	/// <summary>
	/// parentid = -1 means root level pages
	/// parentid = -2 means get all pages regardless of parent
	/// </summary>
	/// <param name="siteId"></param>
	/// <param name="parentId"></param>
	/// <returns></returns>
	public static IDataReader GetChildPagesSortedAlphabetic(int siteId, int parentId)
	{
		string sqlCommand = @"
SELECT * 
FROM mp_Pages 
WHERE SiteID = ?SiteID 
AND (ParentID = ?ParentID OR ?ParentID = -2) 
ORDER BY PageName;";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?ParentID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = parentId
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);
	}

	public static int GetPendingCount(Guid siteGuid)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Pages 
WHERE SiteGuid = ?SiteGuid 
AND IsPending = 1;";

		var param = new MySqlParameter("?SiteGuid", MySqlDbType.VarChar, 36)
		{
			Direction = ParameterDirection.Input,
			Value = siteGuid
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			param));
	}

	public static IDataReader GetPendingPageListPage(
		Guid siteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetPendingCount(siteGuid);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT 
	p.*, 
COALESCE(wip.Total,0) as WipCount 
FROM
    mp_Pages p  
LEFT OUTER JOIN (
    SELECT Count(*) as Total, 
        pm.PageGuid 
    FROM 
        mp_PageModules pm 
    JOIN 
        mp_ContentWorkflow cw 
    ON 
        cw.ModuleGuid = pm.ModuleGuid 
    WHERE 
        cw.Status Not In ('Cancelled','Approved') 
    GROUP BY 
        pm.PageGuid 
) wip 
ON 
    wip.PageGuid = p.PageGuid 
WHERE 
    p.SiteGuid = ?SiteGuid  
AND 
    p.IsPending = 1 
ORDER BY  
    p.PageName 
LIMIT 
    ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		var arParams = new List<MySqlParameter>
		{
			new("?SiteGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = siteGuid
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(ConnectionString.GetRead(), sqlCommand, arParams);
	}


	public static int GetCount(int siteId, bool includePending)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Pages 
WHERE SiteID = ?SiteID 
AND ((IsPending = 0) OR (?IncludePending = 1));";

		int intIncludePending = 0;
		if (includePending) { intIncludePending = 1; }

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?IncludePending", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludePending
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams));

	}

	public static int GetCountChildPages(int pageId, bool includePending)
	{
		string sqlCommand = @"
SELECT Count(*) 
FROM mp_Pages 
WHERE ParentID = ?PageID 
AND ((IsPending = 0) OR (?IncludePending = 1));";

		int intIncludePending = 0;
		if (includePending) { intIncludePending = 1; }

		var arParams = new List<MySqlParameter>
		{
			new("?PageID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},

			new("?IncludePending", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludePending
			}
		};

		return Convert.ToInt32(CommandHelper.ExecuteScalar(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams));

	}

	public static IDataReader GetPageOfPages(
		int siteId,
		bool includePending,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(siteId, includePending);

		if (pageSize > 0) totalPages = totalRows / pageSize;

		if (totalRows <= pageSize)
		{
			totalPages = 1;
		}
		else
		{
			Math.DivRem(totalRows, pageSize, out int remainder);
			if (remainder > 0)
			{
				totalPages += 1;
			}
		}

		string sqlCommand = @"
SELECT	* 
FROM mp_Pages  
WHERE SiteID = ?SiteID 
AND ((IsPending = 0) OR (?IncludePending = 1)) 
ORDER BY ParentID, PageName 
LIMIT ?PageSize ";

		if (pageNumber > 1)
		{
			sqlCommand += "OFFSET ?OffsetRows ";
		}

		sqlCommand += ";";

		int intIncludePending = 0;
		if (includePending) { intIncludePending = 1; }

		var arParams = new List<MySqlParameter>
		{
			new("?SiteID", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = siteId
			},

			new("?IncludePending", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = intIncludePending
			},

			new("?PageSize", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},

			new("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			}
		};

		return CommandHelper.ExecuteReader(
			ConnectionString.GetRead(),
			sqlCommand,
			arParams);

	}

}
