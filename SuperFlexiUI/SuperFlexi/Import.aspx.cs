using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using SuperFlexiBusiness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperFlexiUI
{
	public partial class Import : NonCmsBasePage
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(Import));
		private Hashtable moduleSettings;
		private Module module;
		private int moduleId = -1;
		private int pageId = -1;
		protected ModuleConfiguration config = new ModuleConfiguration();


		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Request.IsAuthenticated)
			{
				SiteUtils.RedirectToLoginPage(this);

				return;
			}

			SecurityHelper.DisableBrowserCache();

			LoadParams();
			LoadSettings();

			if (!UserCanEditModule(moduleId, config.FeatureGuid) || !config.AllowImport)
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
				if (Request.UrlReferrer != null && hdnReturnUrl.Value.Length == 0)
				{
					hdnReturnUrl.Value = Request.UrlReferrer.ToString();
					lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

					var fields = new List<Field>();
					_ = FieldUtils.EnsureFields(siteSettings.SiteGuid, config, out fields, config.DeleteOrphanedFieldValues);
				}
			}
		}


		void sflexiItem_ContentChanged(object sender, ContentChangedEventArgs e)
		{
			IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["SuperFlexiIndexBuilderProvider"];

			if (indexBuilder != null)
			{
				indexBuilder.ContentChangedHandler(sender, e);
			}
		}


		private void DoRedirect()
		{
			if (hdnReturnUrl.Value.Length > 0)
			{
				WebUtils.SetupRedirect(this, hdnReturnUrl.Value + "#module" + moduleId.ToString());

				return;
			}

			WebUtils.SetupRedirect(this, SiteUtils.GetCurrentPageUrl() + "#module" + moduleId.ToString());

			return;
		}


		private void ImportBtn_Click(object sender, EventArgs e)
		{
			var results = new StringBuilder();
			var importCount = 0;
			var partialImportCount = 0;
			var updateCount = 0;
			var partialUpdateCount = 0;
			var failCount = 0;

			if (uploader.HasFile)
			{
				var recordsToImport = ImportHelper.GetDynamicListFromCSV(uploader.FileContent);

				if (recordsToImport != null)
				{
					if (chkDelete.Checked)
					{
						_ = ItemFieldValue.DeleteByModule(module.ModuleGuid);
						_ = Item.DeleteByModule(module.ModuleGuid);
					}

					foreach (IDictionary<string, object> record in recordsToImport)
					{
						var existingGuid = Guid.Empty;
						var hasGuid = false;

						if (record.ContainsKey("Guid"))
						{
							hasGuid = Guid.TryParse(Convert.ToString(record["Guid"]), out existingGuid);
						}

						var isUpdate = true;
						Item importedItem = null;

						if (hasGuid && chkUpdate.Checked)
						{
							importedItem = new Item(existingGuid);
						}

						if (importedItem == null || importedItem.DefinitionGuid != config.FieldDefinitionGuid || importedItem.ModuleGuid != module.ModuleGuid)
						{
							//todo: report to user why record isn't being updated
							isUpdate = false;
							importedItem = new Item
							{
								SiteGuid = siteSettings.SiteGuid,
								FeatureGuid = config.FeatureGuid,
								ModuleGuid = module.ModuleGuid,
								ModuleID = module.ModuleId,
								DefinitionGuid = config.FieldDefinitionGuid,
								ItemGuid = Guid.NewGuid()
							};
						}

						var sortOrder = 500;

						if (record.ContainsKey("SortOrder"))
						{
							var sortOrderStr = Convert.ToString(record["SortOrder"]);

							if (!string.IsNullOrWhiteSpace(sortOrderStr))
							{
								sortOrder = int.Parse(sortOrderStr);
							}
						}

						importedItem.SortOrder = sortOrder;
						importedItem.LastModUtc = DateTime.UtcNow;
						//we don't want to do this on each item that has been imported because that's a lot of work during the import process
						//importedItem.ContentChanged += new ContentChangedEventHandler(sflexiItem_ContentChanged);

						if (importedItem.Save())
						{
							var fullyImported = true;
							//partialCount++;
							List<Field> fields = null;

							if (config.FieldDefinitionGuid != Guid.Empty)
							{
								fields = Field.GetAllForDefinition(config.FieldDefinitionGuid);
							}
							else
							{
								//todo: need to show a message about definition guid missing
								log.ErrorFormat("definitionGuid is missing from the field configuration file named {0}.", config.FieldDefinitionSrc);

								return;
							}

							if (fields == null) return;

							foreach (Field field in fields)
							{
								foreach (var kvp in record)
								{
									if (field.Name == kvp.Key.Replace(" ", string.Empty))
									{
										List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(importedItem.ItemGuid);
										ItemFieldValue fieldValue = null;

										try
										{
											fieldValue = fieldValues.Where(saved => saved.FieldGuid == field.FieldGuid).Single();
										}
										catch (InvalidOperationException)
										{
											//field is probably new

											fieldValue = new ItemFieldValue();
										}

										//ItemFieldValue fieldValue = new ItemFieldValue(item.ItemGuid, field.FieldGuid);
										fieldValue.FieldGuid = field.FieldGuid;
										fieldValue.SiteGuid = field.SiteGuid;
										fieldValue.FeatureGuid = field.FeatureGuid;
										fieldValue.ModuleGuid = module.ModuleGuid;
										fieldValue.ItemGuid = importedItem.ItemGuid;

										fieldValue.FieldValue = kvp.Value.ToString();

										if (!fieldValue.Save())
										{
											fullyImported = false;
											_ = results.AppendLine(string.Format("<div><strong>Partial Failure:</strong> {0}</div>", string.Join(";", record.Select(x => x.Key + "=" + x.Value))));
										}
									}
								}
							}

							if (fullyImported)
							{
								if (isUpdate)
								{
									updateCount++;
								}
								else
								{
									importCount++;
								}
							}
							else
							{
								if (isUpdate)
								{
									partialUpdateCount++;
								}
								else
								{
									partialImportCount++;
								}
							}
						}
						else
						{
							failCount++;
							_ = results.AppendFormat("<div><strong>Failed:</strong> {0}</div>", string.Join(";", record.Select(x => x.Key + "=" + x.Value)));
						}
					}

					_ = results.Insert(0, string.Format(@"
                        <div><strong>Imported</strong>&nbsp;{0}</div>
                        <div><strong>Partially Imported</strong>&nbsp;{1}</div>
						<div><strong>Updated</strong>&nbsp;{2}</div>
						<div><strong>Partially Updated</strong>&nbsp;{3}</div>
                        <div><strong>Failed</strong>&nbsp;{4}</div>"
						, importCount.ToString()
						, partialImportCount.ToString()
						, updateCount.ToString()
						, partialUpdateCount.ToString()
						, failCount.ToString()));
				}
				else
				{
					_ = results.Insert(0, "<div class=\"alert alert-danger\">No records found in CSV file</div>");
				}

				litResults.Text = results.ToString();

				CacheHelper.ClearModuleCache(moduleId);
				SuperFlexiIndexBuilderProvider indexBuilder = new SuperFlexiIndexBuilderProvider();
				indexBuilder.RebuildIndex(CacheHelper.GetCurrentPage(), IndexHelper.GetSearchIndexPath(siteSettings.SiteId));
				SiteUtils.QueueIndexing();
			}
		}


		private void PopulateLabels()
		{
			Title = SiteUtils.FormatPageTitle(siteSettings, config.ImportPageTitle);
			heading.Text = config.ImportPageTitle;
			lnkCancel.Text = config.ImportPageCancelLinkText;
			litInstructions.Text = Resources.SuperFlexiResources.ImportInstructions;
		}


		private void LoadParams()
		{
			moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
			pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
		}


		private void LoadSettings()
		{
			moduleSettings = ModuleSettings.GetModuleSettings(moduleId);
			//we want to get the module using this method because it will let the module be editable when placed on the page with a ModuleWrapper
			module = SuperFlexiHelpers.GetSuperFlexiModule(moduleId);

			if (module == null)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);

				return;
			}

			config = new ModuleConfiguration(module);
			lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();
			AddClassToBody("flexi-import " + config.EditPageCssClass);
		}


		protected override void OnPreInit(EventArgs e)
		{
			AllowSkinOverride = true;
			base.OnPreInit(e);
		}

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);
			Load += new EventHandler(Page_Load);
			importButton.Click += new EventHandler(ImportBtn_Click);
			SuppressPageMenu();
		}
	}
}