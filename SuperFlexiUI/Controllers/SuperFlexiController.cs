using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using SuperFlexiUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SuperFlexiUI.Controllers
{
	public class SuperFlexiController : ApiController
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(SuperFlexiController));
		private Module module = null;
		private SiteSettings siteSettings = null;
		private PageSettings currentPage = null;
		private ModuleConfiguration config = new ModuleConfiguration();


		[HttpPost]
		public ReturnObject GetModuleItems(RequestObject r)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentPage = new PageSettings(siteSettings.SiteId, r.PageId);
			var allowed = false;

			if (currentPage != null)
			{
				allowed = WebUser.IsInRoles(currentPage.AuthorizedRoles);
			}

			module = new Module(r.ModuleId);

			if (module != null)
			{
				allowed = WebUser.IsInRoles(module.ViewRoles);
			}

			if (!allowed)
			{
				return new ReturnObject()
				{
					Status = "error",
					ExtraData = new Dictionary<string, string>
					{
						["ErrorCode"] = "100",
						["ErrorMessage"] = "Not Allowed"
					}
				};
			}

			config = new ModuleConfiguration(module);

			var totalPages = 0;
			var totalRows = 0;
			var items = new List<ItemWithValues>();

			if (r.SearchObject != null && r.SearchObject.Count > 0)
			{
				foreach (var set in r.SearchObject)
				{
					if (set.Value.Contains(";"))
					{
						foreach (var setValue in set.Value.SplitOnCharAndTrim(';'))
						{
							items.AddRange(
								GetItems(
									module.ModuleGuid,
									r.PageNumber,
									r.PageSize,
									out totalPages,
									out totalRows,
									setValue,
									set.Key,
									r.GetAllForSolution,
									r.SortDescending
								)
							);
						}
					}
					else
					{
						items.AddRange(
							GetItems(
								module.ModuleGuid,
								r.PageNumber,
								r.PageSize,
								out totalPages,
								out totalRows,
								set.Value,
								set.Key,
								r.GetAllForSolution,
								r.SortDescending
							)
						);
					}
				}

				items = items.Distinct(new SimpleItemWithValuesComparer()).ToList();
			}
			else
			{
				items.AddRange(
					GetItems(
						module.ModuleGuid,
						r.PageNumber,
						r.PageSize,
						out totalPages,
						out totalRows,
						r.SearchTerm,
						r.SearchField,
						r.GetAllForSolution,
						r.SortDescending
					)
				);
			}

			var sfObject = new SuperFlexiObject()
			{
				FriendlyName = config.ModuleFriendlyName,
				ModuleTitle = module.ModuleTitle,
				GlobalSortOrder = config.GlobalViewSortOrder,
				Items = items
			};

			return new ReturnObject()
			{
				Status = "success",
				TotalPages = totalPages,
				TotalRows = totalRows > items.Count ? totalRows : items.Count,
				AllowEdit = ShouldAllowEdit(),
				CmsModuleId = module.ModuleId,
				CmsPageId = module.PageId,
				Data = sfObject
			};
		}


		[HttpPost]
		public ReturnObject GetFieldValues(RequestObject r)
		{
			siteSettings = CacheHelper.GetCurrentSiteSettings();
			currentPage = new PageSettings(siteSettings.SiteId, r.PageId);
			var allowed = false;

			if (currentPage != null)
			{
				allowed = WebUser.IsInRoles(currentPage.AuthorizedRoles);
			}

			module = new Module(r.ModuleId);

			if (module != null)
			{
				allowed = WebUser.IsInRoles(module.ViewRoles);
			}

			if (!allowed)
			{
				return new ReturnObject()
				{
					Status = "error",
					ExtraData = new Dictionary<string, string>
					{
						["ErrorCode"] = "100",
						["ErrorMessage"] = "Not Allowed"
					}
				};
			}

			config = new ModuleConfiguration(module);

			var fieldValues = ItemFieldValue.GetPageOfValues(
				module.ModuleGuid,
				config.FieldDefinitionGuid,
				r.Field,
				r.PageNumber,
				r.PageSize,
				out int totalPages,
				out int totalRows
			);

			// much of the below is temporary, we needed to implement in a hurry
			// TODO: implement distinct on sql side
			var values = new List<string>();
			var dbField = new Field(fieldValues.Select(fv => fv.FieldGuid).FirstOrDefault());

			switch (dbField.ControlType)
			{
				case "DynamicCheckBoxList":
					foreach (ItemFieldValue val in fieldValues)
					{
						values.AddRange(val.FieldValue.SplitOnCharAndTrim(';'));
					}

					break;
				default:
					values = fieldValues.Select(fv => fv.FieldValue).ToList();

					break;
					//we will add the other cases later
			}

			values = values.Distinct().OrderBy(v => v).ToList();
			totalRows = values.Count();

			return new ReturnObject()
			{
				Status = "success",
				Data = values,
				TotalPages = totalPages,
				TotalRows = totalRows
			};
		}


		private List<ItemWithValues> GetItems(
			Guid moduleGuid,
			int pageNumber,
			int pageSize,
			out int totalPages,
			out int totalRows,
			string searchTerm = "",
			string searchField = "",
			bool byDefinition = false,
			bool descending = false
		)
		{
			if (byDefinition)
			{
				return ItemWithValues.GetListForDefinition(
					config.FieldDefinitionGuid,
					siteSettings.SiteGuid,
					out totalPages,
					out totalRows,
					pageNumber,
					pageSize,
					searchTerm,
					searchField
				);
			}
			else
			{
				return ItemWithValues.GetListForModule(
					moduleGuid,
					out totalPages,
					out totalRows,
					pageNumber,
					pageSize,
					searchTerm,
					searchField
				);
			}
		}


		private bool ShouldAllowEdit()
		{
			if (WebUser.IsAdmin)
			{
				return true;
			}

			if (module != null)
			{
				if (module.AuthorizedEditRoles == "Admins;" || currentPage.EditRoles == "Admins;")
				{
					return false;
				}

				if (WebUser.IsContentAdmin || 
					SiteUtils.UserIsSiteEditor() || 
					WebUser.IsInRoles(module.AuthorizedEditRoles) || 
					(!module.IsGlobal && WebUser.IsInRoles(currentPage.EditRoles)))
				{
					return true;
				}
			}

			return false;
		}
	}
}
