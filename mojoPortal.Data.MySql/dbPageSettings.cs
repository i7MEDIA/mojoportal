using System;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;

namespace mojoPortal.Data;

public static class DBPageSettings
{
	public static int Create(
		int SiteID,
		int ParentID,
		string PageName,
		string PageTitle,
		string Skin,
		int PageOrder,
		string AuthorizedRoles,
		string EditRoles,
		string DraftEditRoles,
		string DraftApprovalRoles,
		string CreateChildPageRoles,
		string CreateChildDraftRoles,
		bool RequireSSL,
		bool AllowBrowserCache,
		bool ShowBreadcrumbs,
		bool ShowChildBreadcrumbs,
		string PageKeyWords,
		string PageDescription,
		bool UseUrl,
		string Url,
		bool OpenInNewWindow,
		bool ShowChildPageMenu,
		bool HideMainMenu,
		bool IncludeInMenu,
		String MenuImage,
		string ChangeFrequency,
		string SiteMapPriority,
		Guid PageGuid,
		Guid ParentGuid,
		bool HideAfterLogin,
		Guid SiteGuid,
		string CompiledMeta,
		DateTime CompiledMetaUtc,
		bool IncludeInSiteMap,
		bool IsClickable,
		bool ShowHomeCrumb,
		bool IsPending,
		string CanonicalOverride,
		bool IncludeInSearchMap,
		bool EnableComments,
		bool IncludeInChildSiteMap,
		bool ExpandOnSiteMap,
		Guid PubTeamId,
		string BodyCssClass,
		string MenuCssClass,
		Guid PCreatedBy,
		string PCreatedFromIp,
		string MenuDesc,
		string LinkRel,
		string PageHeading,
		bool ShowPageHeading,
		DateTime PubDateUtc,
		string StyleSets
		)
	{
		string[] columns = [
			$"{nameof(ParentID)}",
			$"{nameof(PageName)}",
			$"{nameof(PageTitle)}",
			$"{nameof(PageOrder)}",
			$"{nameof(AuthorizedRoles)}",
			$"{nameof(EditRoles)}",
			$"{nameof(DraftEditRoles)}",
			$"{nameof(DraftApprovalRoles)}",
			$"{nameof(CreateChildPageRoles)}",
			$"{nameof(CreateChildDraftRoles)}",
			$"{nameof(RequireSSL)}",
			$"{nameof(AllowBrowserCache)}",
			$"{nameof(ShowBreadcrumbs)}",
			$"{nameof(PageKeyWords)}",
			$"{nameof(PageDescription)}",
			$"{nameof(UseUrl)}",
			$"{nameof(Url)}",
			$"{nameof(OpenInNewWindow)}",
			$"{nameof(ShowChildPageMenu)}",
			$"{nameof(ShowChildBreadcrumbs)}",
			$"{nameof(HideMainMenu)}",
			$"{nameof(Skin)}",
			$"{nameof(MenuImage)}",
			$"{nameof(IncludeInMenu)}",
			$"{nameof(ChangeFrequency)}",
			$"{nameof(SiteMapPriority)}",
			$"{nameof(PageGuid)}",
			$"{nameof(ParentGuid)}",
			$"{nameof(HideAfterLogin)}",
			$"{nameof(CanonicalOverride)}",
			$"{nameof(IncludeInSearchMap)}",
			$"{nameof(EnableComments)}",
			$"{nameof(IncludeInSiteMap)}",
			$"{nameof(IsClickable)}",
			$"{nameof(ShowHomeCrumb)}",
			$"{nameof(IsPending)}",
			$"{nameof(IncludeInChildSiteMap)}",
			$"{nameof(ExpandOnSiteMap)}",
			$"{nameof(PubTeamId)}",
			$"{nameof(BodyCssClass)}",
			$"{nameof(MenuCssClass)}",
			$"{nameof(SiteGuid)}",
			$"{nameof(CompiledMeta)}",
			$"{nameof(CompiledMetaUtc)}",
			$"{nameof(MenuDesc)}",
			$"{nameof(LinkRel)}",
			$"{nameof(PageHeading)}",
			$"{nameof(ShowPageHeading)}",
			$"{nameof(PubDateUtc)}",
			$"{nameof(PCreatedBy)}",
			$"{nameof(PCreatedFromIp)}",
			$"{nameof(StyleSets)}",
			"LastModifiedUTC",
			"PCreatedUtc",
			"PLastModUtc",
			"PLastModBy",
			"PLastModFromIp",
		];
		var sqlCommand = $"""
			INSERT INTO mp_Pages ({string.Join(", ", columns)})
			VALUES ({string.Join(", ", columns.Select(c => $"?{c}"))});
			SELECT LAST_INSERT_ID();
		""";

		MySqlParameter[] arParams = [
			new ($"?{nameof(SiteID)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = SiteID},
			new ($"?{nameof(ParentID)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ParentID},
			new ($"?{nameof(PageName)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageName},
			new ($"?{nameof(PageOrder)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = PageOrder},
			new ($"?{nameof(AuthorizedRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = AuthorizedRoles},
			new ($"?{nameof(RequireSSL)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = RequireSSL ? 1 :0},
			new ($"?{nameof(ShowBreadcrumbs)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowBreadcrumbs ? 1 : 0},
			new ($"?{nameof(PageKeyWords)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = PageKeyWords},
			new ($"?{nameof(PageDescription)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageDescription},
			new ($"?{nameof(UseUrl)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = UseUrl ? 1 : 0},
			new ($"?{nameof(Url)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = Url},
			new ($"?{nameof(OpenInNewWindow)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = OpenInNewWindow ? 1 : 0},
			new ($"?{nameof(ShowChildPageMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowChildPageMenu ? 1 :0},
			new ($"?{nameof(EditRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = EditRoles},
			new ($"?{nameof(CreateChildPageRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CreateChildPageRoles},
			new ($"?{nameof(ShowChildBreadcrumbs)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowChildBreadcrumbs ? 1 :0},
			new ($"?{nameof(HideMainMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = HideMainMenu ? 1 : 0},
			new ($"?{nameof(Skin)}", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Input, Value = Skin},
			new ($"?{nameof(IncludeInMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInMenu ? 1 :0},
			new ($"?{nameof(MenuImage)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = MenuImage},
			new ($"?{nameof(PageTitle)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageTitle},
			new ($"?{nameof(AllowBrowserCache)}", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = AllowBrowserCache},
			new ($"?{nameof(ChangeFrequency)}", MySqlDbType.VarChar, 20) { Direction = ParameterDirection.Input, Value = ChangeFrequency},
			new ($"?{nameof(SiteMapPriority)}", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = SiteMapPriority},
			new ($"?{nameof(PageGuid)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PageGuid.ToString()},
			new ($"?{nameof(ParentGuid)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = ParentGuid.ToString()},
			new ($"?{nameof(HideAfterLogin)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = HideAfterLogin ? 1 :0},
			new ($"?{nameof(SiteGuid)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = SiteGuid.ToString()},
			new ($"?{nameof(CompiledMeta)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CompiledMeta},
			new ($"?{nameof(CompiledMetaUtc)}", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = CompiledMetaUtc},
			new ($"?{nameof(IncludeInSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInSiteMap ? 1 :0},
			new ($"?{nameof(IsClickable)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IsClickable ? 1 :0},
			new ($"?{nameof(ShowHomeCrumb)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowHomeCrumb ? 1 :0},
			new ($"?{nameof(DraftEditRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = DraftEditRoles},
			new ($"?{nameof(IsPending)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IsPending ? 1 : 0},
			new ($"?{nameof(CanonicalOverride)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = CanonicalOverride},
			new ($"?{nameof(IncludeInSearchMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInSearchMap ? 1 : 0},
			new ($"?{nameof(EnableComments)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = EnableComments ? 1 : 0},
			new ($"?{nameof(CreateChildDraftRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CreateChildDraftRoles},
			new ($"?{nameof(IncludeInChildSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInChildSiteMap ? 1 : 0},
			new ($"?{nameof(PubTeamId)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PubTeamId.ToString()},
			new ($"?{nameof(BodyCssClass)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = BodyCssClass},
			new ($"?{nameof(MenuCssClass)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = MenuCssClass},
			new ($"?{nameof(ExpandOnSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ExpandOnSiteMap ? 1 : 0},
			new ($"?{nameof(MenuDesc)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = MenuDesc},
			new ($"?{nameof(DraftApprovalRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = DraftApprovalRoles},
			new ($"?{nameof(LinkRel)}", MySqlDbType.VarChar, 20) { Direction = ParameterDirection.Input, Value = LinkRel},
			new ($"?{nameof(PageHeading)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageHeading},
			new ($"?{nameof(ShowPageHeading)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowPageHeading ? 1 : 0},
			new ($"?{nameof(StyleSets)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = StyleSets},
			new ($"?{nameof(PubDateUtc)}", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = PubDateUtc == DateTime.MaxValue ? DBNull.Value : PubDateUtc},
			new ($"?{nameof(PCreatedBy)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PCreatedBy},
			new ($"?{nameof(PCreatedFromIp)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PCreatedFromIp},
			new ("PCreatedUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow},
			new ("PLastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow},
			new ("PLastModBy", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PCreatedBy},
			new ("PLastModFromIp", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PCreatedFromIp},
			new ("LastModifiedUTC", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow},
		];

		int newID = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams).ToString());

		return newID;
	}

	public static bool UpdatePage(
		int SiteID,
		int PageID,
		int ParentID,
		string PageName,
		string PageTitle,
		string Skin,
		int PageOrder,
		string AuthorizedRoles,
		string EditRoles,
		string DraftEditRoles,
		string DraftApprovalRoles,
		string CreateChildPageRoles,
		string CreateChildDraftRoles,
		bool RequireSSL,
		bool AllowBrowserCache,
		bool ShowBreadcrumbs,
		bool ShowChildBreadcrumbs,
		string PageKeyWords,
		string PageDescription,
		bool UseUrl,
		string Url,
		bool OpenInNewWindow,
		bool ShowChildPageMenu,
		bool HideMainMenu,
		bool IncludeInMenu,
		String MenuImage,
		string ChangeFrequency,
		string SiteMapPriority,
		Guid ParentGuid,
		bool HideAfterLogin,
		string CompiledMeta,
		DateTime CompiledMetaUtc,
		bool IncludeInSiteMap,
		bool IsClickable,
		bool ShowHomeCrumb,
		bool IsPending,
		string CanonicalOverride,
		bool IncludeInSearchMap,
		bool EnableComments,
		bool IncludeInChildSiteMap,
		bool ExpandOnSiteMap,
		Guid PubTeamId,
		string BodyCssClass,
		string MenuCssClass,
		Guid PLastModBy,
		string PLastModFromIp,
		string MenuDesc,
		string LinkRel,
		string PageHeading,
		bool ShowPageHeading,
		DateTime PubDateUtc,
		string StyleSets)
	{
		string[] columns = [
			$"{nameof(PageOrder)}",
			$"{nameof(ParentID)}",
			$"{nameof(PageName)}",
			$"{nameof(PageTitle)}",
			$"{nameof(AuthorizedRoles)}",
			$"{nameof(EditRoles)}",
			$"{nameof(DraftEditRoles)}",
			$"{nameof(DraftApprovalRoles)}",
			$"{nameof(CreateChildPageRoles)}",
			$"{nameof(CreateChildDraftRoles)}",
			$"{nameof(RequireSSL)}",
			$"{nameof(AllowBrowserCache)}",
			$"{nameof(ShowBreadcrumbs)}",
			$"{nameof(PageKeyWords)}",
			$"{nameof(PageDescription)}",
			$"{nameof(UseUrl)}",
			$"{nameof(Url)}",
			$"{nameof(OpenInNewWindow)}",
			$"{nameof(ShowChildPageMenu)}",
			$"{nameof(ShowChildBreadcrumbs)}",
			$"{nameof(HideMainMenu)}",
			$"{nameof(Skin)}",
			$"{nameof(MenuImage)}",
			$"{nameof(IncludeInMenu)}",
			$"{nameof(ChangeFrequency)}",
			$"{nameof(SiteMapPriority)}",
			$"{nameof(ParentGuid)}",
			$"{nameof(HideAfterLogin)}",
			$"{nameof(CanonicalOverride)}",
			$"{nameof(IncludeInSearchMap)}",
			$"{nameof(IncludeInSiteMap)}",
			$"{nameof(EnableComments)}",
			$"{nameof(IsClickable)}",
			$"{nameof(ShowHomeCrumb)}",
			$"{nameof(IsPending)}",
			$"{nameof(IncludeInChildSiteMap)}",
			$"{nameof(ExpandOnSiteMap)}",
			$"{nameof(PubTeamId)}",
			$"{nameof(BodyCssClass)}",
			$"{nameof(MenuCssClass)}",
			$"{nameof(CompiledMeta)}",
			$"{nameof(CompiledMetaUtc)}",
			$"{nameof(MenuDesc)}",
			$"{nameof(LinkRel)}",
			$"{nameof(PageHeading)}",
			$"{nameof(ShowPageHeading)}",
			$"{nameof(PubDateUtc)}",
			$"{nameof(PLastModBy)}",
			$"{nameof(PLastModFromIp)}",
			$"{nameof(StyleSets)}",
			"LastModifiedUTC",
			"PLastModUtc",
		];
		var sqlCommand = $"""
			UPDATE mp_Pages 
			SET {string.Join(", ", columns.Select(c => $"{c} = ?{c}"))}
			WHERE {nameof(PageID)} = ?{nameof(PageID)};
		""";

		MySqlParameter[] arParams = [
			new ($"?{nameof(PageID)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = PageID },
			new ($"?{nameof(ParentID)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ParentID },
			new ($"?{nameof(PageName)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageName },
			new ($"?{nameof(PageOrder)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = PageOrder },
			new ($"?{nameof(AuthorizedRoles)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = AuthorizedRoles },
			new ($"?{nameof(PageKeyWords)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = PageKeyWords },
			new ($"?{nameof(PageDescription)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageDescription },
			new ($"?{nameof(RequireSSL)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = RequireSSL ? 1 : 0 },
			new ($"?{nameof(ShowBreadcrumbs)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowBreadcrumbs ? 1 : 0 },
			new ($"?{nameof(UseUrl)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = UseUrl ? 1 : 0 },
			new ($"?{nameof(Url)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = Url },
			new ($"?{nameof(OpenInNewWindow)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = OpenInNewWindow ? 1 : 0 },
			new ($"?{nameof(ShowChildPageMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowChildPageMenu ? 1 : 0 },
			new ($"?{nameof(EditRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = EditRoles },
			new ($"?{nameof(CreateChildPageRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CreateChildPageRoles },
			new ($"?{nameof(ShowChildBreadcrumbs)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowChildBreadcrumbs ? 1 : 0 },
			new ($"?{nameof(HideMainMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = HideMainMenu ? 1 : 0 },
			new ($"?{nameof(Skin)}", MySqlDbType.VarChar, 100) { Direction = ParameterDirection.Input, Value = Skin },
			new ($"?{nameof(IncludeInMenu)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInMenu ? 1 : 0 },
			new ($"?{nameof(MenuImage)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = MenuImage },
			new ($"?{nameof(PageTitle)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageTitle },
			new ($"?{nameof(AllowBrowserCache)}", MySqlDbType.Bit) { Direction = ParameterDirection.Input, Value = AllowBrowserCache },
			new ($"?{nameof(ChangeFrequency)}", MySqlDbType.VarChar, 20) { Direction = ParameterDirection.Input, Value = ChangeFrequency },
			new ($"?{nameof(SiteMapPriority)}", MySqlDbType.VarChar, 10) { Direction = ParameterDirection.Input, Value = SiteMapPriority },
			new ($"?{nameof(ParentGuid)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = ParentGuid.ToString() },
			new ($"?{nameof(HideAfterLogin)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = HideAfterLogin ? 1 : 0 },
			new ($"?{nameof(CompiledMeta)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CompiledMeta },
			new ($"?{nameof(CompiledMetaUtc)}", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = CompiledMetaUtc },
			new ($"?{nameof(IncludeInSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInSiteMap ? 1 : 0 },
			new ($"?{nameof(IsClickable)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IsClickable ? 1 : 0 },
			new ($"?{nameof(ShowHomeCrumb)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowHomeCrumb ? 1 : 0 },
			new ($"?{nameof(DraftEditRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = DraftEditRoles },
			new ($"?{nameof(IsPending)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IsPending ? 1 : 0 },
			new ($"?{nameof(CanonicalOverride)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = CanonicalOverride },
			new ($"?{nameof(IncludeInSearchMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInSearchMap ? 1 : 0 },
			new ($"?{nameof(EnableComments)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = EnableComments ? 1 : 0 },
			new ($"?{nameof(CreateChildDraftRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = CreateChildDraftRoles },
			new ($"?{nameof(IncludeInChildSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = IncludeInChildSiteMap ? 1 : 0 },
			new ($"?{nameof(PubTeamId)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PubTeamId.ToString() },
			new ($"?{nameof(BodyCssClass)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = BodyCssClass },
			new ($"?{nameof(MenuCssClass)}", MySqlDbType.VarChar, 50) { Direction = ParameterDirection.Input, Value = MenuCssClass },
			new ($"?{nameof(ExpandOnSiteMap)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ExpandOnSiteMap ? 1 : 0 },
			new ($"?{nameof(PLastModBy)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PLastModBy },
			new ($"?{nameof(PLastModFromIp)}", MySqlDbType.VarChar, 36) { Direction = ParameterDirection.Input, Value = PLastModFromIp },
			new ($"?{nameof(MenuDesc)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = MenuDesc },
			new ($"?{nameof(DraftApprovalRoles)}", MySqlDbType.Text) { Direction = ParameterDirection.Input, Value = DraftApprovalRoles },
			new ($"?{nameof(LinkRel)}", MySqlDbType.VarChar, 20) { Direction = ParameterDirection.Input, Value = LinkRel },
			new ($"?{nameof(PageHeading)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = PageHeading },
			new ($"?{nameof(ShowPageHeading)}", MySqlDbType.Int32) { Direction = ParameterDirection.Input, Value = ShowPageHeading ? 1 : 0 },
			new ($"?{nameof(PubDateUtc)}", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = PubDateUtc == DateTime.MaxValue ? DBNull.Value : PubDateUtc },
			new ($"?{nameof(StyleSets)}", MySqlDbType.VarChar, 255) { Direction = ParameterDirection.Input, Value = StyleSets },
			new ("LastModifiedUTC", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
			new ("?PLastModUtc", MySqlDbType.DateTime) { Direction = ParameterDirection.Input, Value = DateTime.UtcNow },
		];



		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}

	public static int GetNextPageOrder(int SiteID, int ParentID)
	{
		var sqlCommand = $"""
			SELECT COALESCE(MAX(PageOrder), -1) 
			FROM mp_Pages 
			WHERE {nameof(SiteID)} = ?{nameof(SiteID)} 
			AND {nameof(ParentID)} = ?{nameof(ParentID)}; 
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = SiteID },
			new MySqlParameter($"?{nameof(ParentID)}", MySqlDbType.Int32){ Direction = ParameterDirection.Input, Value = ParentID},
		];

		int nextPageOrder = Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
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
		var sqlCommand = """
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
			FROM mp_Pages p 
			LEFT OUTER JOIN	mp_Users u1 ON	p.PCreatedBy = u1.UserGuid 
			LEFT OUTER JOIN	mp_Users u2 ON	p.PLastModBy = u2.UserGuid 
			WHERE p.PageGuid = ?PageGuid
			LIMIT 1;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter("?PageGuid", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = pageGuid.ToString()
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetPage(int SiteID, int PageID)
	{
		var sqlCommand = $"""
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
			FROM mp_Pages p 
			LEFT OUTER JOIN	mp_Users u1 ON	p.PCreatedBy = u1.UserGuid 
			LEFT OUTER JOIN	mp_Users u2 ON	p.PLastModBy = u2.UserGuid 
			WHERE (p.{nameof(PageID)} = ?{nameof(PageID)} OR ?{nameof(PageID)} = -1)  
			AND p.{nameof(SiteID)} = ?{nameof(SiteID)}  
			ORDER BY p.ParentID, p.PageOrder
			LIMIT 1  ;
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
			new MySqlParameter($"?{nameof(PageID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = PageID
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static IDataReader GetChildPages(int SiteID, int ParentID)
	{
		var sqlCommand = $"""
			SELECT * 
			FROM mp_Pages 
			WHERE {nameof(ParentID)} = ?{nameof(ParentID)}  
			AND {nameof(SiteID)} = ?{nameof(SiteID)} 
			ORDER BY PageOrder; 
		""";
		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
			new MySqlParameter($"?{nameof(ParentID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ParentID
			},
		];

		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static bool UpdateTimestamp(int PageID, DateTime LastModifiedUTC)
	{
		var sqlCommand = $"""
			UPDATE mp_Pages 
			SET {nameof(LastModifiedUTC)} = ?{nameof(LastModifiedUTC)}
			WHERE {nameof(PageID)} = ?{nameof(PageID)} ; 
		""";
		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(PageID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = PageID
			},
			new MySqlParameter($"?{nameof(LastModifiedUTC)}", MySqlDbType.DateTime)
			{
				Direction = ParameterDirection.Input,
				Value = LastModifiedUTC
			},
		];

		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;
	}

	public static bool UpdatePageOrder(int PageID, int PageOrder)
	{
		var sqlCommand = $"""
			UPDATE mp_Pages
			SET {nameof(PageOrder)} = ?{nameof(PageOrder)} 
			WHERE {nameof(PageID)} = ?{nameof(PageID)} ;
		""";
		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(PageID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = PageID
			},
			new MySqlParameter($"?{nameof(PageOrder)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = PageOrder
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > -1;
	}

	public static bool DeletePage(int PageID)
	{
		var sqlCommand = $"DELETE FROM mp_Pages WHERE {nameof(PageID)} = ?{nameof(PageID)};";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(PageID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = PageID
			},
		];
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			arParams);

		return rowsAffected > 0;
	}

	public static bool CleanupOrphans()
	{
		var sqlCommand = $"""
			UPDATE mp_Pages AS p1 
			LEFT JOIN ( SELECT * FROM mp_Pages ) AS p2 ON p1.ParentID = p2.PageID 
			SET p1.ParentID = -1, p1.ParentGuid = '{Guid.Empty}' 
			WHERE p1.ParentID <> -1 
			AND p2.PageID IS NULL;
		""";
		int rowsAffected = MySqlHelper.ExecuteNonQuery(
			ConnectionString.GetWriteConnectionString(),
			sqlCommand,
			null);

		return rowsAffected > 0;
	}

	public static IDataReader GetPageList(int SiteID)
	{
		var sqlCommand = $"""
			SELECT * 
			FROM mp_Pages 
			WHERE {nameof(SiteID)} = ?{nameof(SiteID)} 
			ORDER BY ParentID, PageOrder, PageName ;
		""";
		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	/// <summary>
	/// parentid = -1 means root level pages
	/// parentid = -2 means get all pages regardless of parent
	/// </summary>
	/// <param name="SiteID"></param>
	/// <param name="ParentID"></param>
	/// <returns></returns>
	public static IDataReader GetChildPagesSortedAlphabetic(int SiteID, int ParentID)
	{
		var sqlCommand = $"""
			SELECT * 
			FROM mp_Pages 
			WHERE 
			SiteID = ?{nameof(SiteID)} 
			AND ({nameof(ParentID)} = ?{nameof(ParentID)} OR ?{nameof(ParentID)} = -2) 
			ORDER BY PageName ;
			""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
			new MySqlParameter($"?{nameof(ParentID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = ParentID
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}

	public static int GetPendingCount(Guid SiteGuid)
	{
		var sqlCommand = $"""
			SELECT Count(*) 
			FROM mp_Pages 
			WHERE {nameof(SiteGuid)} = ?{nameof(SiteGuid)} 
			AND IsPending = 1;
		""";
		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteGuid)}", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = SiteGuid
			},
		];
		return Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));
	}

	public static IDataReader GetPendingPageListPage(
		Guid SiteGuid,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetPendingCount(SiteGuid);

		if (pageSize > 0)
		{
			totalPages = totalRows / pageSize;
		}

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

		var sqlCommand = $"""
			SELECT p.*, COALESCE(wip.Total,0) as WipCount 
			FROM mp_Pages p  
			LEFT OUTER JOIN (
					SELECT Count(*) as Total, pm.PageGuid 
					FROM mp_PageModules pm 
					JOIN mp_ContentWorkflow cw ON cw.ModuleGuid = pm.ModuleGuid 
					WHERE cw.Status NOT IN ('Cancelled','Approved') 
					GROUP BY pm.PageGuid ) wip 
				ON wip.PageGuid = p.PageGuid 
			WHERE p.SiteGuid = ?SiteGuid  
			AND p.IsPending = 1 
			ORDER BY p.PageName 
			LIMIT ?{nameof(pageSize)} {(pageNumber > 1 ? "OFFSET ?OffsetRows " : string.Empty)};
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteGuid)}", MySqlDbType.VarChar, 36)
			{
				Direction = ParameterDirection.Input,
				Value = SiteGuid
			},
			new MySqlParameter($"?{nameof(pageSize)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
			new MySqlParameter($"?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}


	public static int GetCount(int SiteID, bool includePending)
	{
		var sqlCommand = $"""
			SELECT Count(*) 
			FROM mp_Pages 
			WHERE {nameof(SiteID)} = ?{nameof(SiteID)} 
			AND ((IsPending = 0) OR (?{nameof(includePending)} = 1));
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
			new MySqlParameter($"?{nameof(includePending)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = includePending ? 1 : 0
			},
		];
		return Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));
	}

	public static int GetCountChildPages(int pageId, bool includePending)
	{
		var sqlCommand = $"""
			SELECT Count(*) 
			FROM mp_Pages 
			WHERE ParentID = ?{nameof(pageId)} 
			AND ((IsPending = 0) OR (?{nameof(includePending)} = 1));
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(pageId)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageId
			},
			new MySqlParameter($"?{nameof(includePending)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = includePending ? 1 : 0
			},
		];
		return Convert.ToInt32(MySqlHelper.ExecuteScalar(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams));
	}

	public static IDataReader GetPageOfPages(
		int SiteID,
		bool includePending,
		int pageNumber,
		int pageSize,
		out int totalPages)
	{
		int pageLowerBound = (pageSize * pageNumber) - pageSize;
		totalPages = 1;
		int totalRows = GetCount(SiteID, includePending);

		if (pageSize > 0)
		{
			totalPages = totalRows / pageSize;
		}

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

		var sqlCommand = $"""
			SELECT * 
			FROM mp_Pages  
			WHERE {nameof(SiteID)} = ?{nameof(SiteID)} 
			AND ((IsPending = 0) OR (?{nameof(includePending)} = 1)) 
			ORDER BY ParentID, PageName 
			LIMIT ?{nameof(pageSize)} {(pageNumber > 1 ? "OFFSET ?OffsetRows " : string.Empty)};
		""";

		MySqlParameter[] arParams =
		[
			new MySqlParameter($"?{nameof(SiteID)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = SiteID
			},
			new MySqlParameter($"?{nameof(includePending)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = includePending ? 1 : 0
			},
			new MySqlParameter($"?{nameof(pageSize)}", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageSize
			},
			new MySqlParameter("?OffsetRows", MySqlDbType.Int32)
			{
				Direction = ParameterDirection.Input,
				Value = pageLowerBound
			},
		];
		return MySqlHelper.ExecuteReader(
			ConnectionString.GetReadConnectionString(),
			sqlCommand,
			arParams);
	}
}
