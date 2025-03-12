using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using mojoPortal.Business;
using mojoPortal.Business.WebHelpers;
using mojoPortal.Core.Extensions;
using mojoPortal.SearchIndex;
using mojoPortal.Web;
using mojoPortal.Web.Components;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using mojoPortal.Web.UI.Razor;
using Resources;
using SuperFlexiBusiness;

namespace SuperFlexiUI;

public partial class EditItems : NonCmsBasePage
{
	private static readonly ILog log = LogManager.GetLogger(typeof(EditItems));
	private Hashtable moduleSettings;
	private Module module;
	private int moduleId = -1;
	private int itemId = -1;
	private int pageId = -1;
	protected ModuleConfiguration config = new();
	private Item item = new();

	//protected Image imgPreview;
	//protected HiddenField hdnEmptyImageUrl;
	//protected HiddenField hdnImageBrowser;
	//private SiteSettings siteSettings = new SiteSettings();

	private bool advancedFilePickerAdded = false;

	#region OnInit

	protected override void OnPreInit(EventArgs e)
	{
		AllowSkinOverride = true;
		base.OnPreInit(e);
	}

	override protected void OnInit(EventArgs e)
	{
		base.OnInit(e);
		this.Load += new EventHandler(this.Page_Load);
		this.updateButton.Click += new EventHandler(this.UpdateBtn_Click);
		this.deleteButton.Click += new EventHandler(this.DeleteBtn_Click);
		this.saveAsNewButton.Click += new EventHandler(this.SaveAsNewBtn_Click);
		this.exportButton.Click += new EventHandler(this.ExportBtn_Click);
		SuppressPageMenu();
	}

	//protected override void OnLoad(EventArgs e)
	//{
	//    base.OnLoad(e);
	//    PopulateCustomControls();

	//    SetupScripts();
	//}

	//protected override void OnPreRender(EventArgs e)
	//{
	//    base.OnPreRender(e);
	//    PopulateCustomControls();
	//}
	#endregion

	private void Page_Load(object sender, EventArgs e)
	{
		if (!Request.IsAuthenticated)
		{
			SiteUtils.RedirectToLoginPage(this);
			return;
		}

		SecurityHelper.DisableBrowserCache();

		LoadParams();
		LoadSettings();

		if (!UserCanEditModule(moduleId, config.FeatureGuid) && !WebUser.IsInRoles(module.AuthorizedEditRoles) && !WebUser.IsInRoles(item.EditRoles))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (SiteUtils.IsFishyPost(this))
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (itemId > -1)
		{
			item = new Item(itemId);
		}
		else
		{
			if (config.MaxItems > -1 && config.MaxItems <= Item.GetCountForModule(moduleId))
			{
				SiteUtils.RedirectToAccessDeniedPage();
				return;
			}
		}

		PopulateCustomControls();
		SetupScripts();
		PopulateLabels();

		if (!IsPostBack)
		{

			PopulateControls();
			if ((Request.UrlReferrer is not null) && (hdnReturnUrl.Value.Length == 0))
			{
				hdnReturnUrl.Value = Request.UrlReferrer.ToString();
				lnkCancel.NavigateUrl = Request.UrlReferrer.ToString();

			}
		}
	}

	private void PopulateControls()
	{
		if (itemId > -1)
		{
			if (item.ModuleID != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}
			saveAsNewButton.Visible = config.ShowSaveAsNew;
			txtViewOrder.Text = item.SortOrder.ToString();

			exportButton.Visible = config.AllowExport;
		}
		else
		{
			if (Item.GetForModule(moduleId).Count > 0)
			{
				txtViewOrder.Text = (Item.GetHighestSortOrder(moduleId) + 10).ToString();
			}
			else
			{
				txtViewOrder.Text = "500";
			}
			saveAsNewButton.Visible = false;
			deleteButton.Visible = false;
			exportButton.Visible = false;
		}
	}

	public void PopulateCustomControls()
	{
		List<Field> savedFields = new List<Field>();
		if (!FieldUtils.EnsureFields(SiteInfo.SiteGuid, config, out savedFields, config.DeleteOrphanedFieldValues)) return;

		if (item.ItemID == -1)
		{
			foreach (Field field in savedFields)
			{
				if (field.ControlType.ToLower() == "instructionblock")
				{

					AddInstructionBlock(field);
				}
				else
				{
					AddSettingControl(field);
				}
			}
		}
		else
		{

			List<Field> fieldsWithValues = new List<Field>();
			List<ItemFieldValue> fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);

			foreach (ItemFieldValue fieldValue in fieldValues)
			{
				fieldsWithValues.Add(new Field(fieldValue.FieldGuid));
			}
			FieldComparer fieldComp = new FieldComparer();
			List<Field> newFields = savedFields.Except(fieldsWithValues, fieldComp).ToList();
			//List<Field> allFields = new List<Field>();

			foreach (Field field in newFields)
			{
				ItemFieldValue fieldValue = new ItemFieldValue();
				fieldValue.FieldValue = field.DefaultValue;
				fieldValue.FieldGuid = field.FieldGuid;
				fieldValue.FeatureGuid = field.FeatureGuid;
				fieldValue.ItemGuid = item.ItemGuid;
				fieldValue.ModuleGuid = item.ItemGuid;
				fieldValue.SiteGuid = item.SiteGuid;

				fieldValues.Add(fieldValue);
			}

			foreach (Field field in savedFields)
			{
				if (field.ControlType.ToLower() != "instructionblock")
				{
					foreach (ItemFieldValue fieldValue in fieldValues)
					{
						if (fieldValue.FieldGuid == field.FieldGuid)
						{
							AddSettingControl(field, fieldValue);
						}
					}
				}
				else
				{
					AddInstructionBlock(field);
				}
			}

		}
	}

	private void AddInstructionBlock(Field field)
	{
		Literal litInstructions = new Literal();
		litInstructions.Text = SuperFlexiHelpers.GetHelpText(field.HelpKey, config);
		if (!string.IsNullOrWhiteSpace(litInstructions.Text))
		{
			customControls.Controls.Add(litInstructions);
		}
	}

	private void AddSettingControl(Field field, ItemFieldValue fieldValue = null)
	{
		var panel = new Panel();
		var label = new SiteLabel();
		panel.CssClass = field.EditPageControlWrapperCssClass;
		label.CssClass = field.EditPageLabelCssClass;
		label.ResourceFile = "SuperFlexiResources";
		label.ConfigKey = field.Label;
		label.ShowWarningOnMissingKey = false; //chances are the configkey will not exist in the resource file.
		try
		{
			label.SkinID = field.DefinitionName.Replace(" ", string.Empty);
		}
		catch (ArgumentException) { }
		panel.Controls.Add(label);

		AttributeCollection attribs = null;

		string pickerStartFolder = string.Empty;
		string controlType = field.ControlType.ToLower().ToLower();
		switch (controlType)
		{
			case "textbox":
			case "":
			default:
				TextBox textBox = new TextBox
				{
					TabIndex = 10,
					ID = field.Name,
					CssClass = field.EditPageControlCssClass
				};
				label.ForControl = textBox.ID;

				textBox.TextMode = field.TextBoxMode.ToLower() switch
				{
					"multiline" => TextBoxMode.MultiLine,
					"password" => TextBoxMode.Password,
					"color" => TextBoxMode.Color,
					"date" => TextBoxMode.Date,
					"datetime" => TextBoxMode.DateTime,
					"datetimelocal" => TextBoxMode.DateTimeLocal,
					"email" => TextBoxMode.Email,
					"month" => TextBoxMode.Month,
					"number" => TextBoxMode.Number,
					"range" => TextBoxMode.Range,
					"search" => TextBoxMode.Search,
					"phone" => TextBoxMode.Phone,
					"time" => TextBoxMode.Time,
					"url" => TextBoxMode.Url,
					"week" => TextBoxMode.Week,
					"singleline" or _ => TextBoxMode.SingleLine
				};
				textBox.Text = fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue;
				try
				{
					textBox.SkinID = field.DefinitionName.Replace(" ", string.Empty);
				}
				catch (ArgumentException) { }

				attribs = textBox.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					if (key.ToLower() == "rows")
					{
						textBox.Rows = Convert.ToInt32(attribs[key]);
					}
					else if (key.ToLower() == "cols")
					{
						textBox.Columns = Convert.ToInt32(attribs[key]);
					}
					else
					{
						textBox.Attributes.Add(key, attribs[key]);
					}

				}
				if (!string.IsNullOrWhiteSpace(field.EditRoles) && !WebUser.IsAdmin && !WebUser.IsInRoles(field.EditRoles))
				{
					textBox.Enabled = false;
				}
				panel.Controls.Add(textBox);

				if (textBox.TextMode == TextBoxMode.Date)
				{
					panel.Controls.Add(CreateDateValidator(textBox, field));
				}

				if (field.Required)
				{
					var rfv = CreateGenericRFV(textBox, field);
					if (rfv is not null) panel.Controls.Add(rfv);
				}

				if (field.Regex.Length > 0)
				{
					var regexValidator = CreateRegexValidator(textBox, field);
					if (regexValidator is not null) panel.Controls.Add(regexValidator);
				}

				break;

			case "linkpicker":
				var linkPicker = new TextBox
				{
					TabIndex = 10,
					ID = field.Name,
					CssClass = field.EditPageControlCssClass + " hide"
				};
				linkPicker.SetOrAppendCss("advanced-file-picker__output");
				label.ForControl = linkPicker.ID;

				try
				{
					linkPicker.SkinID = field.DefinitionName.Replace(" ", string.Empty);
				}
				catch (ArgumentException) { }

				linkPicker.Text = fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue;

				attribs = linkPicker.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					if (key.ToLower() == "rows")
					{
						linkPicker.Rows = Convert.ToInt32(attribs[key]);
					}
					else if (key.ToLower() == "cols")
					{
						linkPicker.Columns = Convert.ToInt32(attribs[key]);
					}
					else if (key.ToLower() == "startfolder")
					{
						pickerStartFolder = attribs[key];
					}
					else
					{
						linkPicker.Attributes.Add(key, attribs[key]);
					}
				}

				linkPicker.TextMode = TextBoxMode.Url;
				panel.Controls.Add(linkPicker);

				AddUrlBrowserSupport(panel, field, pickerStartFolder);

				if (field.Required)
				{

					var rfv = CreateGenericRFV(linkPicker, field);
					if (rfv is not null) panel.Controls.Add(rfv);
				}

				if (field.Regex.Length > 0)
				{
					var regexValidator = CreateRegexValidator(linkPicker, field);
					if (regexValidator is not null) panel.Controls.Add(regexValidator);
				}

				break;
			case "imagepicker":

				var imagePicker = new TextBox
				{
					TabIndex = 10,
					ID = field.Name,
					CssClass = field.EditPageControlCssClass + " hide"
				};
				imagePicker.SetOrAppendCss("advanced-file-picker__output");
				label.ForControl = imagePicker.ID;

				try
				{
					imagePicker.SkinID = field.DefinitionName.Replace(" ", string.Empty);
				}
				catch (ArgumentException) { }

				imagePicker.Text = fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue;

				attribs = imagePicker.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					if (key.ToLower() == "rows")
					{
						imagePicker.Rows = Convert.ToInt32(attribs[key]);
					}
					else if (key.ToLower() == "cols")
					{
						imagePicker.Columns = Convert.ToInt32(attribs[key]);
					}
					else if (key.ToLower() == "startfolder")
					{
						pickerStartFolder = attribs[key];
					}
					else
					{
						imagePicker.Attributes.Add(key, attribs[key]);
					}

				}

				imagePicker.TextMode = TextBoxMode.Url;
				panel.Controls.Add(imagePicker);

				AddUrlBrowserSupport(panel, field, pickerStartFolder, true);

				if (field.Required)
				{

					RequiredFieldValidator rfv = CreateGenericRFV(imagePicker, field);
					if (rfv is not null) panel.Controls.Add(rfv);
				}

				if (field.Regex.Length > 0)
				{
					var regexValidator = CreateRegexValidator(imagePicker, field);
					if (regexValidator is not null) panel.Controls.Add(regexValidator);
				}
				break;
			case "checkbox":
				var checkBox = new CheckBox();
				checkBox.TabIndex = 10;
				checkBox.ID = field.Name;
				checkBox.CssClass = field.EditPageControlCssClass;
				label.ForControl = checkBox.ID;
				try
				{
					checkBox.SkinID = field.DefinitionName.Replace(" ", string.Empty);
				}
				catch (ArgumentException) { }

				if (field.CheckBoxReturnBool)
				{

					if (string.Equals(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue, "true", StringComparison.InvariantCultureIgnoreCase))
					{
						checkBox.Checked = true;
					}
				}
				else
				{
					if (string.Equals(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue, field.CheckBoxReturnValueWhenTrue, StringComparison.InvariantCultureIgnoreCase))
					{
						checkBox.Checked = true;
					}
				}

				attribs = checkBox.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					checkBox.Attributes.Add(key, (string)attribs[key]);
				}

				panel.Controls.Add(checkBox);
				break;
			case "checkboxlist":
				if (!string.IsNullOrWhiteSpace(field.Options))
				{
					var cbl = new CheckBoxList
					{
						TabIndex = 10,
						ID = field.Name,
						CssClass = field.EditPageControlCssClass,
						RepeatLayout = RepeatLayout.UnorderedList
					};

					label.ForControl = cbl.ID;
					try
					{
						cbl.SkinID = field.DefinitionName.Replace(" ", string.Empty);
					}
					catch (ArgumentException) { }


					var listItems = GetListItemsFromOptions(field.Options);

					var selected = new List<string>();

					cbl.Items.AddRange(listItems.ToArray());

					if (fieldValue is not null)
					{
						selected = fieldValue.FieldValue.SplitOnCharAndTrim(';');
						if (selected.Count > 0)
						{
							foreach (string selVal in selected)
							{
								var selItem = cbl.Items.FindByValue(selVal);
								if (selItem is not null)
								{
									selItem.Selected = true;
								}
							}
						}
					}

					if (fieldValue is null && selected.Count < 1)
					{
						var defaultItem = cbl.Items.FindByValue(field.DefaultValue);
						if (defaultItem is not null) defaultItem.Selected = true;
					}

					attribs = cbl.Attributes;
					FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

					foreach (string key in attribs.Keys)
					{
						cbl.Attributes.Add(key, (string)attribs[key]);
					}

					panel.Controls.Add(cbl);
				}
				break;
			case "dynamiccheckboxlist":
			case "dynamicradiobuttonlist":
				var dcblPanel = new BasePanel();
				var dcbl = new CheckBoxList();
				var drbl = new RadioButtonList();
				WebControl dynamicListControl;
				var dynamicListItems = new ListItemCollection();
				if (controlType == "dynamiccheckboxlist")
				{
					dynamicListControl = dcbl;
					dynamicListItems = dcbl.Items;
				}
				else
				{
					dynamicListControl = drbl;
					dynamicListItems = drbl.Items;
				}
				dynamicListControl.TabIndex = 10;
				dynamicListControl.ID = field.Name;
				dynamicListControl.CssClass = field.EditPageControlCssClass;
				label.ForControl = dynamicListControl.ID;

				var newTxt = new TextBox
				{
					ID = field.Name + "_newTxt",
					TabIndex = 10,
					CssClass = field.EditPageControlCssClass
				};

				try
				{
					dynamicListControl.SkinID = field.DefinitionName.Replace(" ", string.Empty);
					newTxt.SkinID = field.DefinitionName.Replace(" ", string.Empty);
					dcblPanel.SkinID = field.DefinitionName.Replace(" ", string.Empty);
				}
				catch (ArgumentException) { }

				dcbl.RepeatLayout = RepeatLayout.UnorderedList;
				drbl.RepeatLayout = RepeatLayout.UnorderedList;

				var dynamicOptions = new List<ItemFieldValue>();

				if (field.IsGlobal)
				{
					dynamicOptions = ItemFieldValue.GetByFieldGuid(field.FieldGuid);
				}
				else
				{
					dynamicOptions = ItemFieldValue.GetByFieldGuidForModule(field.FieldGuid, moduleId);
				}

				if (!string.IsNullOrWhiteSpace(field.Options))
				{
					dynamicListItems = GetListItemCollectionFromOptions(field.Options);
				}

				foreach (ItemFieldValue ifv in dynamicOptions)
				{
					foreach (ListItem dynamicItem in GetListItemsFromOptions(ifv.FieldValue))
					{
						if (!dynamicListItems.Contains(dynamicItem))
						{
							dynamicListItems.Add(dynamicItem);
						}
					}
				}

				if (fieldValue is not null)
				{
					var selectedItems = GetListItemsFromOptions(fieldValue.FieldValue);
					if (selectedItems.Count > 0)
					{
						foreach (ListItem selected in selectedItems)
						{
							ListItem staticItem = dynamicListItems.FindByValue(selected.Value);
							if (staticItem is not null)
							{
								staticItem.Selected = true;
							}
							else
							{
								selected.Selected = true;
								if (field.ControlType.ToLower() == "dynamiccheckboxlist")
								{
									dcbl.Items.Add(selected);
								}
								else
								{
									drbl.Items.Add(selected);
								}
							}
						}
					}
				}

				if (controlType == "dynamiccheckboxlist")
				{
					if (fieldValue is null && dcbl.SelectedIndex < 0)
					{
						var defaultItem = dcbl.Items.FindByValue(field.DefaultValue);
						if (defaultItem is not null) defaultItem.Selected = true;
					}

					var items = dynamicListItems.Cast<ListItem>().OrderBy(i => i.Text).ToArray();
					dcbl.Items.Clear();
					dcbl.Items.AddRange(items);
				}
				else
				{
					if (fieldValue is null && drbl.SelectedIndex < 0)
					{
						var defaultItem = drbl.Items.FindByValue(field.DefaultValue);
						if (defaultItem is not null) defaultItem.Selected = true;
					}

					var items = dynamicListItems.Cast<ListItem>().OrderBy(i => i.Text).ToArray();
					drbl.Items.Clear();
					drbl.Items.AddRange(items);
				}

				attribs = dynamicListControl.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					dynamicListControl.Attributes.Add(key, (string)attribs[key]);
					newTxt.Attributes.Add(key, (string)attribs[key]);
				}
				dcblPanel.Controls.Add(dynamicListControl);
				dcblPanel.Controls.Add(newTxt);

				panel.Controls.Add(dcblPanel);

				break;
			case "dropdown":
				if (!string.IsNullOrWhiteSpace(field.Options))
				{
					var ddl = new RazorDropDownList
					{
						TabIndex = 10,
						ID = field.Name,
						CssClass = field.EditPageControlCssClass,
						DefaultOption = SuperFlexiResources.SolutionDropDownPleaseSelect
					};

					label.ForControl = ddl.ID;
					try
					{
						ddl.SkinID = field.DefinitionName.Replace(" ", string.Empty);
					}
					catch (ArgumentException) { }

					ddl.Items = (List<SelectListItem>)GetSelectListFromOptions(field, fieldValue).Items;

					ddl.Items = ddl.Items.Select(x =>
						{
							x.Selected = x.Value == (fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue);
							return x;
						}).ToList();

					ddl.SelectedValue = fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue;

					FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

					foreach (string key in attribs.Keys)
					{
						ddl.Attributes.Add(key, attribs[key]);
					}

					panel.Controls.Add(ddl);
					if (field.Required)
					{
						var rfv = CreateGenericRFV(ddl, field);
						if (rfv is not null) panel.Controls.Add(rfv);
					}
				}
				break;
			case "radiobuttons":

				if (!string.IsNullOrWhiteSpace(field.Options))
				{
					var radioList = new RadioButtonList
					{
						TabIndex = 10,
						ID = field.Name,
						CssClass = field.EditPageControlCssClass
					};
					label.ForControl = radioList.ID;
					try
					{
						radioList.SkinID = field.DefinitionName.Replace(" ", string.Empty);
					}
					catch (ArgumentException) { }

					radioList.RepeatLayout = RepeatLayout.UnorderedList;

					var listItems = GetListItemsFromOptions(field.Options);

					radioList.Items.AddRange(listItems.ToArray());

					radioList.SelectedValue = fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue;

					attribs = radioList.Attributes;
					FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

					foreach (string key in attribs.Keys)
					{
						radioList.Attributes.Add(key, (string)attribs[key]);
					}

					panel.Controls.Add(radioList);
				}

				break;
			case "datetime":
				var datePicker = CreateDatePicker(field, fieldValue);
				datePicker.TabIndex = 10;
				datePicker.ID = field.Name;
				label.ForControl = datePicker.ID;
				datePicker.CssClass = field.EditPageControlCssClass;
				datePicker.ShowTime = field.DatePickerIncludeTimeForDate;
				datePicker.ShowMonthList = field.DatePickerShowMonthList;
				datePicker.ShowYearList = field.DatePickerShowYearList;

				if (datePicker.ShowYearList) datePicker.YearRange = field.DatePickerYearRange;

				attribs = datePicker.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					datePicker.Attributes.Add(key, attribs[key]);
				}

				panel.Controls.Add(datePicker);

				if (field.Required)
				{
					var rfv = CreateGenericRFV(datePicker, field);
					if (rfv is not null) panel.Controls.Add(rfv);
				}

				if (field.Regex.Length > 0)
				{
					var regexValidator = CreateRegexValidator(datePicker, field);
					if (regexValidator is not null) panel.Controls.Add(regexValidator);
				}

				break;
			case "date":
				var calTxt = new TextBox();
				var calBtn = new Label();

				calTxt.ID = field.Name;
				calTxt.TabIndex = 10;
				calTxt.CssClass = field.EditPageControlCssClass;
				label.ForControl = calTxt.ID;

				calTxt.Text = ParseDateString(field, fieldValue);

				calBtn.ID = field.Name + "_calBtn";
				calBtn.TabIndex = 10;
				calBtn.Text = "...";
				calBtn.CssClass = "btn btn-default";

				attribs = calTxt.Attributes;
				FieldUtils.GetFieldAttributes(field.Attributes, out attribs);

				foreach (string key in attribs.Keys)
				{
					calTxt.Attributes.Add(key, (string)attribs[key]);
				}

				panel.Controls.Add(calTxt);
				panel.Controls.Add(calBtn);
				panel.Controls.Add(CreateDateValidator(calTxt, field));

				if (field.Required)
				{
					var rfv = CreateGenericRFV(calTxt, field);
					if (rfv is not null) panel.Controls.Add(rfv);
				}

				if (field.Regex.Length > 0)
				{
					var regexValidator = CreateRegexValidator(calTxt, field);
					if (regexValidator is not null) panel.Controls.Add(regexValidator);
				}

				break;
			case "isettingcontrol":
				if (field.ControlSrc.Length > 0)
				{
					if (field.ControlSrc.EndsWith(".ascx"))
					{
						var uc = Page.LoadControl(field.ControlSrc);
						if (uc is ISettingControl)
						{
							var sc = uc as ISettingControl;
							if (!IsPostBack)
								sc.SetValue(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue);
							uc.ID = field.Name;
							label.ForControl = uc.ID;

							if (uc is InterfaceControl)
							{
								InterfaceControl ic = uc as InterfaceControl;
								ic.ControlField(field);
							}

							panel.Controls.Add(uc);
						}
					}
					else
					{
						try
						{
							var c = (Control)Activator.CreateInstance(Type.GetType(field.ControlSrc));
							if (c is not null)
							{
								if (c is ISettingControl)
								{
									var sc = c as ISettingControl;
									c.ID = field.Name;

									if (c is InterfaceControl)
									{
										var ic = c as InterfaceControl;
										ic.ControlField(field);
									}
									panel.Controls.Add(c);

									if (!IsPostBack)
									{
										sc.SetValue(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue);
									}
								}
								else
								{
									log.Error("setting control " + field.ControlSrc + " does not implement ISettingControl");
								}
							}

						}
						catch (Exception ex)
						{
							log.Error(ex);
						}
					}
				}
				else
				{
					log.Error("could not add setting control for ISettingControl, missing controlsrc for " + field.Name);
				}
				break;
			case "icustomfield":
			case "customfield":
				if (field.ControlSrc.Length > 0)
				{
					if (field.ControlSrc.EndsWith(".ascx"))
					{
						var uc = Page.LoadControl(field.ControlSrc);
						if (uc is ICustomField)
						{
							var sc = uc as ICustomField;
							if (!IsPostBack)
								sc.SetValue(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue);
							uc.ID = field.Name;
							label.ForControl = uc.ID;

							var dictAttribs = UIHelper.GetDictionaryFromString(field.Attributes);
							sc.Attributes(dictAttribs);

							if (uc is InterfaceControl)
							{
								var ic = uc as InterfaceControl;
								ic.ControlField(field);
							}

							panel.Controls.Add(uc);
						}
					}
					else
					{
						try
						{
							var c = (Control)Activator.CreateInstance(Type.GetType(field.ControlSrc));
							if (c is not null)
							{
								if (c is ICustomField)
								{
									var sc = c as ICustomField;
									c.ID = field.Name;

									var dictAttribs = UIHelper.GetDictionaryFromString(field.Attributes);
									sc.Attributes(dictAttribs);

									if (c is InterfaceControl)
									{
										var ic = c as InterfaceControl;
										ic.ControlField(field);
									}
									panel.Controls.Add(c);

									if (!IsPostBack)
									{
										sc.SetValue(fieldValue is not null ? fieldValue.FieldValue : field.DefaultValue);
									}
								}
								else
								{
									log.Error("setting control " + field.ControlSrc + " does not implement ICustomControl");
								}
							}
						}
						catch (Exception ex)
						{
							log.Error(ex);
						}
					}
				}
				else
				{
					log.Error("could not add setting control for ICustomField, missing controlsrc for " + field.Name);
				}
				break;
		}

		if (field.HelpKey.Length > 0)
		{
			if (field.HelpKey.EndsWith(".sfhelp") ||
				field.HelpKey.EndsWith(".config") ||
				field.HelpKey.StartsWith("$_FlexiHelp_$") ||
				field.HelpKey.StartsWith("$_SitePath_$"))
			{
				panel.Controls.Add(new Literal
				{
					Text = SuperFlexiHelpers.GetHelpText(field.HelpKey, config)
				});
			}
			else
			{
				mojoHelpLink.AddHelpLink(panel, field.HelpKey);
			}
		}

		customControls.Controls.Add(panel);
	}

	private void AddUrlBrowserSupport(Panel panel, Field field, string startFolder, bool isImage = false)
	{
		//create script reference
		var urlBrowserScript = new MarkupScript
		{
			Url = "~/SuperFlexi/js/advanced-file-picker.js",
			ScriptName = "AdvancedFilePickerJS",
			Position = "bottomStartup"
		};

		//create css reference
		var urlBrowserCss = new MarkupCss
		{
			Url = "~/SuperFlexi/css/advanced-file-picker.css",
			RenderAboveSSC = true,
			Name = "AdvancedFilePickerCSS"
		};

		//add script and css references to page scripts/css
		config.EditPageScripts.Add(urlBrowserScript);
		config.EditPageCSS.Add(urlBrowserCss);

		// Create model for razor templating
		var pickerModel = new AdvancedFilePicker
		{
			PickerType = isImage ? "image" : "file",
			FileText = SuperFlexiResources.Browse,
			ImageText = SuperFlexiResources.BrowseFeaturedImage,
			StartFolder = startFolder
		};

		// Render razor template to string
		panel.Controls.Add(new Literal
		{
			Text = RazorBridge.RenderPartialToString("_AdvancedFilePicker", pickerModel, "SuperFlexi")
		});

		if (!advancedFilePickerAdded)
		{
			pnlEdit.Controls.Add(new Literal
			{
				Text = RazorBridge.RenderPartialToString("_AdvancedFilePickerModal", "", "SuperFlexi")
			});
			advancedFilePickerAdded = true;
		}
	}

	private AttributeCollection GetDateTimeMinMax(AttributeCollection attrCol, string min, string max, string format)
	{
		min = min == "DateNow" ? DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) : min;
		max = max == "DateNow" ? DateTime.Now.ToString(CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern) : max;

		attrCol.Add("min", min);
		attrCol.Add("max", max);

		return attrCol;
	}

	private List<ListItem> GetListItemsFromOptions(string fieldOptions)
	{
		var options = fieldOptions.SplitOnChar(';');
		var listItems = new List<ListItem>();
		foreach (string option in options)
		{
			//var opt = option.SplitOnCharAndTrim('|');
			var opt = option.Split(['|', '^']).Select(x => x.Trim()).ToList();

			if (opt.Count < 2)
			{
				//no value so we set the value equal to the name
				opt.Add(opt[0]);
				//opt[1] = opt[0];
			}

			var item = new ListItem
			{
				Text = opt[0],
				Value = opt[1]
			};

			var li = new ListItem();

			listItems.Add(item);

		}
		return listItems;
	}

	private SelectList GetSelectListFromOptions(Field field, ItemFieldValue fieldValue)
	{
		var options = field.Options.SplitOnChar(';');
		var listItems = new List<SelectListItem>();
		var selectedValue = string.Empty;
		var groups = new List<SelectListGroup>();
		foreach (string option in options)
		{
			var opt = option.Split(['|', '^']).Select(x => x.Trim()).ToList();
			SelectListGroup group = null;

			if (opt.Count > 2)
			{
				group = groups.FirstOrDefault(x => x.Name == opt[2]);
				if (group is null)
				{
					group = new SelectListGroup() { Name = opt[2] };
					groups.Add(group);
				}
			}

			if (string.IsNullOrWhiteSpace(opt[1]))
			{
				//no value so we set the value equal to the name
				opt[1] = opt[0];
			}

			var item = new SelectListItem
			{
				Text = opt[0],
				Value = opt[1],
				Group = group
			};

			if (opt[1] == (fieldValue?.FieldValue))
			{
				selectedValue = opt[1];
			}

			listItems.Add(item);

		}
		return new SelectList(listItems);
	}

	private ListItemCollection GetListItemCollectionFromOptions(string fieldOptions)
	{
		var lic = new ListItemCollection();
		lic.AddRange(GetListItemsFromOptions(fieldOptions).ToArray());
		return lic;
	}

	/// <summary>
	/// Creates regular expression validator for a control with properties from an fieldDefinitions
	/// </summary>
	/// <param name="theControl"></param>
	/// <param name="field"></param>
	/// <returns></returns>
	private static RegularExpressionValidator CreateRegexValidator(Control theControl, Field field)
	{
		var regexValidator = new RegularExpressionValidator();
		try
		{
			regexValidator.SkinID = field.DefinitionName.Replace(" ", string.Empty);
		}
		catch (ArgumentException) { }

		regexValidator.ControlToValidate = theControl.ID;
		regexValidator.ValidationExpression = field.Regex;
		regexValidator.ValidationGroup = "flexi";

		regexValidator.ErrorMessage = string.Format(field.RegexMessageFormat, field.Label);
		regexValidator.Text = string.Format(field.RegexMessageFormat, field.Label);
		return regexValidator;
	}

	/// <summary>
	/// Creates required field validator for a control with properties from an fieldDefinitions
	/// </summary>
	/// <param name="theControl"></param>
	/// <param name="field"></param>
	/// <returns></returns>
	private static RequiredFieldValidator CreateGenericRFV(Control theControl, Field field)
	{
		var rfv = new RequiredFieldValidator
		{
			ControlToValidate = theControl.ID,
			ErrorMessage = string.Format(CultureInfo.InvariantCulture, field.RequiredMessageFormat, field.Label),
			ValidationGroup = "flexi"
		};

		try
		{
			rfv.SkinID = field.DefinitionName.Replace(" ", string.Empty);
		}
		catch (ArgumentException) { }

		return rfv;
	}

	private static CompareValidator CreateDateValidator (Control theControl, Field field)
	{
		var valDate = new CompareValidator
		{
			ErrorMessage = string.Format(SuperFlexiResources.GenericInvalidFormat, field.Label),
			ControlToValidate = theControl.ID,
			Type = ValidationDataType.Date,
			Operator = ValidationCompareOperator.DataTypeCheck
		};

		try
		{
			valDate.SkinID = field.DefinitionName.Replace(" ", string.Empty);
		}
		catch (ArgumentException) { }

		return valDate;
	}

	private static DatePickerControl CreateDatePicker(Field field, ItemFieldValue fieldValue)
	{

		var datePicker = new DatePickerControl();
		try
		{
			datePicker.SkinID = field.Name.Replace(" ", string.Empty);
		}
		catch (ArgumentException) { }
		datePicker.ID = "dp" + field.Name;
		datePicker.ShowMonthList = field.DatePickerShowMonthList;
		datePicker.ShowYearList = field.DatePickerShowYearList;
		if (field.DatePickerYearRange.Length > 0)
		{
			datePicker.YearRange = field.DatePickerYearRange;
		}

		datePicker.Text = ParseDateString(field, fieldValue);

		datePicker.ShowTime = field.DatePickerIncludeTimeForDate;

		return datePicker;
	}

	private static string ParseDateString(Field field, ItemFieldValue fieldValue)
	{
		TimeZoneInfo timeZone = SiteUtils.GetUserTimeZone();

		string dateString = string.Empty;
		if (fieldValue is not null)
		{
			if (DateTime.TryParse(
				fieldValue.FieldValue,
				CultureInfo.CurrentCulture,
			DateTimeStyles.AdjustToUniversal, out DateTime dt))
			{

				if (field.DatePickerIncludeTimeForDate)
				{
					dt = dt.ToLocalTime(timeZone);
					dateString = dt.ToString("g");
				}
				else
				{
					dateString = dt.Date.ToShortDateString();
				}
			}
			else
			{
				dateString = fieldValue.FieldValue;
			}
		}
		else
		{
			if (field.DefaultValue.Length > 0)
			{
				dateString = field.DefaultValue;
			}
		}

		return dateString;
	}

	private void ExportBtn_Click(object sender, EventArgs e)
	{
		//we need a list for the ExportDynamicListToCSV. 
		//No need for another method, just create a list with a single item in it.
		var expandoList = new List<dynamic>
		{
			SuperFlexiHelpers.GetExpandoForItem(item)
		};

		ExportHelper.ExportDynamicListToCSV(HttpContext.Current, expandoList, $"{SuperFlexiResources.ExportFileNamePrefix}-{config.MarkupDefinitionName}.csv");
	}

	private void SaveAsNewBtn_Click(object sender, EventArgs e)
	{
		item.ItemID = -1;
		UpdateBtn_Click(sender, e);
	}

	private void UpdateBtn_Click(object sender, EventArgs e)
	{
		Page.Validate("flexi");
		if (!Page.IsValid)
		{
			updateButton.Enabled = true;
			return;
		}

		if (item.ItemID > -1 && item.ModuleID != moduleId)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;
		}

		if (item.ItemID == -1)
		{
			item.SiteGuid = siteSettings.SiteGuid;
			item.FeatureGuid = config.FeatureGuid;
			item.ModuleGuid = module.ModuleGuid;
			item.ModuleID = module.ModuleId;
			item.DefinitionGuid = config.FieldDefinitionGuid;
			item.ItemGuid = Guid.NewGuid();
		}

		item.SortOrder = int.Parse(txtViewOrder.Text);
		item.LastModUtc = DateTime.UtcNow;

		if (item.Save())
		{
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

			if (fields is null) return;

			foreach (var field in fields)
			{
				SaveFieldValue(customControls, field);
			}

			//so indexing is a pain in the ass with how superflexi works so we're going to save the item again to fire contentchanged AFTER our field values have been saved
			//we totally need to do something different
			item.ContentChanged += new ContentChangedEventHandler(sflexiItem_ContentChanged);
			item.Save();

			CurrentPage.UpdateLastModifiedTime();
			//CacheHelper.TouchCacheDependencyFile(cacheDependencyKey);
			CacheHelper.ClearModuleCache(item.ModuleID);
			SiteUtils.QueueIndexing();

			DoRedirect();
		}
	}

	/// <summary>
	/// Saves value of control to ContentItem
	/// </summary>
	/// <param name="controlsPanel"></param>
	/// <param name="field"></param>
	private void SaveFieldValue(Panel controlsPanel, Field field)
	{
		string controlID = field.Name;

		var fieldValues = ItemFieldValue.GetItemValues(item.ItemGuid);
		ItemFieldValue fieldValue;

		try
		{
			fieldValue = fieldValues.Where(saved => saved.FieldGuid == field.FieldGuid).Single();
		}
		catch (InvalidOperationException)
		{
			//field is probably new
			fieldValue = new ItemFieldValue();
		}

		fieldValue.FieldGuid = field.FieldGuid;
		fieldValue.SiteGuid = field.SiteGuid;
		fieldValue.FeatureGuid = field.FeatureGuid;
		fieldValue.ModuleGuid = module.ModuleGuid;
		fieldValue.ItemGuid = item.ItemGuid;

		var control = ControlExtensions.FindControlRecursive(controlsPanel, controlID);
		if (control is not null)
		{
			string controlType = field.ControlType.ToLower();
			switch (controlType)
			{
				case "isettingcontrol":
					if (field.ControlSrc.Length > 0)
					{
						fieldValue.FieldValue = ((ISettingControl)control).GetValue();
					}
					break;
				case "icustomfield":
				case "customfield":
					if (field.ControlSrc.Length > 0)
					{
						fieldValue.FieldValue = ((ICustomField)control).GetValue();
					}
					break;
				case "checkbox":
					var cbox = (CheckBox)control;
					if (field.CheckBoxReturnBool)
					{
						fieldValue.FieldValue = cbox.Checked.ToString();
					}
					else if (cbox.Checked)
					{
						fieldValue.FieldValue = field.CheckBoxReturnValueWhenTrue;
					}
					else
					{
						fieldValue.FieldValue = field.CheckBoxReturnValueWhenFalse;
					}
					//we used to do this but we don't really care if the returnvaluewhen* has a value or not.
					//allowing for blanks here is useful when using the PreTokenString and PostTokenString because then we can hide entire items by putting the markup and field tokens in the pre/posttokenstrings
					//else if (!String.IsNullOrWhiteSpace(field.CheckBoxReturnValueWhenTrue) && !String.IsNullOrWhiteSpace(field.CheckBoxReturnValueWhenFalse))
					//{
					//   if (cbox.Checked)
					//   {
					//	   fieldValue.FieldValue = field.CheckBoxReturnValueWhenTrue;
					//   }
					//   else
					//   {
					//	   fieldValue.FieldValue = field.CheckBoxReturnValueWhenFalse;
					//   }
					//}
					break;
				case "checkboxlist":
					var cbl = (CheckBoxList)control;
					string selected = string.Empty;
					foreach (ListItem cboxItem in cbl.Items)
					{
						if (cboxItem.Selected) selected += cboxItem.Value + ";";
					}
					fieldValue.FieldValue = selected.TrimEnd(';');
					break;
				case "dynamiccheckboxlist":
				case "dynamicradiobuttonlist":
					ListItemCollection dynamicListItems;
					CheckBoxList dcbl;
					RadioButtonList drbl;
					if (controlType == "dynamiccheckboxlist")
					{
						dcbl = (CheckBoxList)control;
						dynamicListItems = dcbl.Items;
					}
					else
					{
						drbl = (RadioButtonList)control;
						dynamicListItems = drbl.Items;
					}
					var newTxt = (TextBox)ControlExtensions.FindControlRecursive(controlsPanel, controlID + "_newTxt");
					var dynamicList_selectedItems = new List<string>();
					var dynamicList_selected = string.Empty;


					if (controlType == "dynamiccheckboxlist")
					{
						dynamicList_selectedItems.AddRange(newTxt.Text.SplitOnCharAndTrim(';'));

						foreach (ListItem dynamicItem in dynamicListItems)
						{
							if (dynamicItem.Selected) dynamicList_selectedItems.Add(dynamicItem.Value);
						}

						foreach (string val in dynamicList_selectedItems.Distinct(StringComparer.CurrentCultureIgnoreCase))
						{
							dynamicList_selected += val + ";";
						}
					}
					else
					{
						if (string.IsNullOrWhiteSpace(newTxt.Text))
						{
							dynamicList_selected = dynamicListItems.SelectedItemsToSemiColonSeparatedString();
						}
						else
						{
							dynamicList_selected = newTxt.Text;
						}
					}

					fieldValue.FieldValue = dynamicList_selected.TrimEnd(';');

					break;
				case "radiobuttons":
					fieldValue.FieldValue = ((RadioButtonList)control).SelectedValue.ToString();
					break;
				case "dropdown":
					fieldValue.FieldValue = ((RazorDropDownList)control).SelectedValue.ToString();
					break;
				case "datetime":
				case "date":
					TextBox calText = null;
					DatePickerControl dp = null;
					if (controlType == "date")
					{
						calText = (TextBox)control;
					}
					else
					{
						dp = (DatePickerControl)control;
					}

					if ((dp is not null && dp.Text.Length > 0) || (calText is not null && calText.Text.Length > 0))
					{
						string textValue = string.Empty;
						if (dp is not null) textValue = dp.Text;
						if (calText is not null) textValue = calText.Text;

						if (DateTime.TryParse(
							textValue,
							CultureInfo.CurrentCulture,
							DateTimeStyles.AdjustToUniversal, out DateTime dt))
						{

							if (field.DatePickerIncludeTimeForDate)
							{
								var timeZone = SiteUtils.GetUserTimeZone();
								dt = dt.ToUtc(timeZone);
							}
							fieldValue.FieldValue = dt.ToString("o");
						}
						else
						{
							fieldValue.FieldValue = textValue;
						}
					}
					else // blank
					{
						fieldValue.FieldValue = string.Empty;
					}
					break;
				case "instructionblock":
					// don't do anything
					break;
				case "textbox":
				case "":
				default:
					fieldValue.FieldValue = ((TextBox)control).Text;
					break;

			}
			fieldValue.Save();
			if (field.Name == config.ItemViewRolesFieldName)
			{
				item.ViewRoles = fieldValue.FieldValue;
				item.Save();
			}
			if (field.Name == config.ItemEditRolesFieldName)
			{
				item.EditRoles = fieldValue.FieldValue;
				item.Save();
			}
		}
	}

	void sflexiItem_ContentChanged(object sender, ContentChangedEventArgs e)
	{
		var indexBuilder = IndexBuilderManager.Providers["SuperFlexiIndexBuilderProvider"];
		if (indexBuilder is not null)
		{
			indexBuilder.ContentChangedHandler(sender, e);
		}
	}

	private void DeleteBtn_Click(object sender, EventArgs e)
	{
		if (itemId != -1)
		{
			var item = new Item(itemId);

			if (item.ModuleID != moduleId)
			{
				SiteUtils.RedirectToAccessDeniedPage(this);
				return;
			}

			item.ContentChanged += new ContentChangedEventHandler(sflexiItem_ContentChanged);
			if (item.Delete())
			{
				ItemFieldValue.DeleteByItem(item.ItemGuid);
				CurrentPage.UpdateLastModifiedTime();
				CacheHelper.ClearModuleCache(moduleId);
				SiteUtils.QueueIndexing();
			}
		}

		DoRedirect();
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

	private void PopulateLabels()
	{
		Title = SiteUtils.FormatPageTitle(siteSettings, config.EditPageTitle);
		heading.Text = config.EditPageTitle;

		updateButton.Text = itemId > -1 ? config.EditPageUpdateButtonText : config.EditPageSaveButtonText;
		SiteUtils.SetButtonAccessKey(updateButton, SuperFlexiResources.UpdateButtonAccessKey);
		UIHelper.AddClearPageExitCode(updateButton);
		ScriptConfig.EnableExitPromptForUnsavedContent = true;

		lnkCancel.Text = config.EditPageCancelLinkText;

		deleteButton.Text = config.EditPageDeleteButtonText;
		SiteUtils.SetButtonAccessKey(deleteButton, SuperFlexiResources.DeleteButtonAccessKey);
		UIHelper.AddConfirmationDialogWithClearExitCode(deleteButton, config.EditPageDeleteWarning);
	}

	private void LoadParams()
	{
		moduleId = WebUtils.ParseInt32FromQueryString("mid", -1);
		itemId = WebUtils.ParseInt32FromQueryString("ItemID", -1);
		pageId = WebUtils.ParseInt32FromQueryString("pageid", -1);
	}

	private void LoadSettings()
	{
		moduleSettings = ModuleSettings.GetModuleSettings(moduleId);

		//we want to get the module using this method because it will let the module be editable when placed on the page with a ModuleWrapper
		module = SuperFlexiHelpers.GetSuperFlexiModule(moduleId);
		if (module is null)
		{
			SiteUtils.RedirectToAccessDeniedPage(this);
			return;

		}
		config = new ModuleConfiguration(module, reloadDefinitionFromDisk: true);

		lnkCancel.NavigateUrl = SiteUtils.GetCurrentPageUrl();

		AddClassToBody("flexi-edit " + config.EditPageCssClass);
	}

	private void SetupScripts()
	{
		var rawScript = @"var systemKeys = { 
		""moduleTitle"": ""$_RawModuleTitle_$"",
		""moduleID"": ""$_ModuleID_$"",
		""pageID"": ""$_PageID_$"",
		""moduleClass"": ""$_ModuleClass_$"",
		""siteID"": ""$_SiteID_$"",
		""siteRoot"": ""$_SiteRoot_$"",
		""skinPath"": ""$_SkinPath_$"",
		""customSettings"": ""$_CustomSettings_$"",
		""editorType"": ""$_EditorType_$"",
		""editorSkin"": ""$_EditorSkin_$"",
		""editorBasePath"": ""$_EditorBasePath_$"",
		""editorConfigPath"": ""$_EditorConfigPath_$"",
		""editorToolbarSet"": ""$_EditorToolbarSet_$"",
		""editorTemplatesUrl"": ""$_EditorTemplatesUrl_$"",
		""editorStylesUrl"": ""$_EditorStylesUrl_$"",
		""dropFileUploadUrl"": ""$_DropFileUploadUrl_$"",
		""fileBrowserUrl"": ""$_FileBrowserUrl_$""
		};";

		SuperFlexiHelpers.ReplaceStaticTokens(new StringBuilder(rawScript), config, true, displaySettings, module, CurrentPage, SiteInfo, out StringBuilder script);

		var headLit = new LiteralControl
		{
			ID = "sflexi-systemKeys",
			Text = $"\n<script data-loader=\"SuperFlexiEdit\" data-name=\"systemKeys\">\n{script}\n</script>",
			ClientIDMode = ClientIDMode.Static,
			EnableViewState = false
		};

		Page.Header.Controls.Add(headLit);

		//this is done in EnsureFields call of PopulateCustomControls()
		//config.EditPageScripts = FieldUtils.ParseScriptsFromXml(config);
		if (config.EditPageScripts.Count > 0)
		{
			SuperFlexiHelpers.SetupScripts(config.EditPageScripts, config, displaySettings, true, IsPostBack, ClientID, siteSettings, module, CurrentPage, Page, this);
		}

		if (config.EditPageCSS.Count > 0)
		{
			SuperFlexiHelpers.SetupStyle(config.EditPageCSS, config, displaySettings, true, ClientID, siteSettings, module, CurrentPage, Page, this);
		}

	}
}

public class AdvancedFilePicker
{
	public string PickerType { get; set; }
	public string FileText { get; set; }
	public string ImageText { get; set; }
	public string StartFolder { get; set; }
}
