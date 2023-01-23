using log4net;
using mojoPortal.Business;
using mojoPortal.Web.Controls;
using mojoPortal.Web.Framework;
using mojoPortal.Web.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace mojoPortal.Web.Configuration
{
	public class mojoProfilePropertyDefinition
	{
		private static readonly ILog log = LogManager.GetLogger(typeof(mojoProfilePropertyDefinition));
		public const string TimeOffsetHoursKey = "TimeOffsetHours";
		public const string TimeZoneIdKey = "TimeZoneId";


		#region Public Properties

		public string Name { get; set; } = string.Empty;
		public string CssClass { get; set; } = string.Empty;
		public string ISettingControlSrc { get; set; } = string.Empty;
		public string Type { get; set; } = "System.String";
		public string StateValue { get; set; } = string.Empty;
		public bool IncludeTimeForDate { get; set; } = false;
		public bool DatePickerShowMonthList { get; set; } = true;
		public bool DatePickerShowYearList { get; set; } = true;
		public string DatePickerYearRange { get; set; } = string.Empty;
		public bool AllowMarkup { get; set; } = false;
		public string ResourceFile { get; set; } = "ProfileResource";
		public string LabelResourceKey { get; set; } = string.Empty;
		public bool LazyLoad { get; set; } = false;
		public bool RequiredForRegistration { get; set; } = false;
		public bool ShowOnRegistration { get; set; } = false;
		public bool AllowAnonymous { get; set; } = true;
		public string OnlyAvailableForRoles { get; set; } = string.Empty;
		public string OnlyVisibleForRoles { get; set; } = string.Empty;
		public bool IncludeHelpLink { get; set; } = false;
		public bool VisibleToAnonymous { get; set; } = false;
		public bool VisibleToAuthenticated { get; set; } = true;
		public bool VisibleToUser { get; set; } = true;
		public bool EditableByUser { get; set; } = true;
		public int MaxLength { get; set; } = 0;
		public int Rows { get; set; } = 0;
		public int Columns { get; set; } = 0;
		public string RegexValidationExpression { get; set; } = string.Empty;
		public string RegexValidationErrorResourceKey { get; set; } = string.Empty;
		public SettingsSerializeAs SerializeAs { get; set; } = SettingsSerializeAs.String;
		public string DefaultValue { get; set; } = string.Empty;
		public Collection<mojoProfilePropertyOption> OptionList { get; } = new Collection<mojoProfilePropertyOption>();

		#endregion


		#region UI Helper Methods

		public static void SetupPropertyControl(
			Page currentPage,
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition,
			double legacyTimeZoneOffset,
			TimeZoneInfo timeZone,
			string siteRoot
		)
		{
			if (propertyDefinition.StateValue.Length > 0)
			{
				SetupPropertyControl(
					currentPage,
					parentControl,
					propertyDefinition,
					propertyDefinition.StateValue,
					legacyTimeZoneOffset,
					timeZone,
					siteRoot
				);
			}
			else
			{
				SetupPropertyControl(
					currentPage,
					parentControl,
					propertyDefinition,
					propertyDefinition.DefaultValue,
					legacyTimeZoneOffset,
					timeZone,
					siteRoot
				);
			}
		}


		public static void SetupPropertyControl(
			Page currentPage,
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition,
			string propertyValue,
			double legacyTimeZoneOffset,
			TimeZoneInfo timeZone,
			string siteRoot
		)
		{
			if (propertyValue == null)
			{
				propertyValue = string.Empty;
			}

			var validatorSkinID = "Profile";

			if (currentPage is UI.Pages.Register)
			{
				validatorSkinID = "Registration";
			}

			var rowOpenTag = new Literal
			{
				Text = $"<div class='settingrow {propertyDefinition.CssClass}'>"
			};

			parentControl.Controls.Add(rowOpenTag);

			var label = new SiteLabel
			{
				ResourceFile = propertyDefinition.ResourceFile,
				// if key isn't in resource file use assume the resource hasn't been
				// localized and just use the key as the resource
				ShowWarningOnMissingKey = false,
				ConfigKey = propertyDefinition.LabelResourceKey,
				CssClass = "settinglabel"
			};

			if (propertyDefinition.ISettingControlSrc.Length > 0)
			{
				Control c = null;

				if (propertyDefinition.ISettingControlSrc.EndsWith(".ascx"))
				{
					c = currentPage.LoadControl(propertyDefinition.ISettingControlSrc);
				}
				else
				{
					try
					{
						c = Activator.CreateInstance(System.Type.GetType(propertyDefinition.ISettingControlSrc)) as Control;
					}
					catch (Exception ex)
					{
						log.Error(ex);
					}
				}

				if (c != null && c is ISettingControl)
				{
					c.ID = "isc" + propertyDefinition.Name;

					parentControl.Controls.Add(label);

					var settingControl = (ISettingControl)c;

					settingControl.SetValue(propertyValue);

					parentControl.Controls.Add(c);

					if (propertyDefinition.IncludeHelpLink)
					{
						AddHelpLink(parentControl, propertyDefinition);
					}
				}
			}
			else if (propertyDefinition.OptionList.Count > 0)
			{
				if (propertyDefinition.Type == "CheckboxList")
				{
					var cbl = CreateCheckBoxListQuestion(propertyDefinition, propertyValue);

					cbl.ID = "cbl" + propertyDefinition.Name;
					cbl.EnableTheming = false;
					cbl.CssClass = "forminput";
					cbl.TabIndex = 0;

					label.ForControl = cbl.ID;

					parentControl.Controls.Add(label);
					parentControl.Controls.Add(cbl);

					if (propertyDefinition.IncludeHelpLink)
					{
						AddHelpLink(parentControl, propertyDefinition);
					}

					if (propertyDefinition.RequiredForRegistration)
					{
						var rfv = new CheckBoxListValidator
						{
							SkinID = validatorSkinID,
							ControlToValidate = cbl.ID,
							ErrorMessage = string.Format(
								CultureInfo.InvariantCulture,
								Resources.ProfileResource.ProfileRequiredItemFormat,
								ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey)
							),
							ValidationGroup = "profile"
						};

						parentControl.Controls.Add(rfv);
					}
				}
				else
				{
					// add a dropdownlist with the options

					DropDownList dd = CreateDropDownQuestion(propertyDefinition, propertyValue);

					dd.ID = "dd" + propertyDefinition.Name;
					dd.EnableTheming = false;
					dd.CssClass = "forminput " + propertyDefinition.CssClass;
					dd.TabIndex = 0;

					label.ForControl = dd.ID;

					parentControl.Controls.Add(label);
					parentControl.Controls.Add(dd);

					if (propertyDefinition.IncludeHelpLink)
					{
						AddHelpLink(parentControl, propertyDefinition);
					}

					if (propertyDefinition.RequiredForRegistration)
					{
						var rfvDd = new RequiredFieldValidator
						{
							SkinID = validatorSkinID,
							ControlToValidate = dd.ID,
							ErrorMessage = string.Format(
								CultureInfo.InvariantCulture,
								Resources.ProfileResource.ProfileRequiredItemFormat,
								ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey)
							),
							ValidationGroup = "profile"
						};

						parentControl.Controls.Add(rfvDd);
					}

					if (propertyDefinition.RegexValidationExpression.Length > 0)
					{
						var regexValidator = new RegularExpressionValidator
						{
							SkinID = validatorSkinID,
							ControlToValidate = dd.ID,
							ValidationExpression = propertyDefinition.RegexValidationExpression,
							ValidationGroup = "profile"
						};

						if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
						{
							regexValidator.ErrorMessage = ResourceHelper.GetResourceString(
								propertyDefinition.ResourceFile,
								propertyDefinition.RegexValidationErrorResourceKey
							);
						}

						parentControl.Controls.Add(regexValidator);
					}
				}
			}
			else
			{
				switch (propertyDefinition.Type)
				{
					case "System.Boolean":
						var checkBox = new CheckBox
						{
							TabIndex = 0,
							ID = "chk" + propertyDefinition.Name,
							CssClass = "forminput " + propertyDefinition.CssClass
						};

						label.ForControl = checkBox.ID;

						parentControl.Controls.Add(label);
						parentControl.Controls.Add(checkBox);

						if (propertyDefinition.IncludeHelpLink)
						{
							AddHelpLink(parentControl, propertyDefinition);
						}

						if (propertyValue.ToLower() == "true")
						{
							checkBox.Checked = true;
						}

						break;

					case "System.DateTime":
						// TODO: to really make this culture aware we should store the users
						// culture as well and use the user's culture to 
						// parse the date
						var datePicker = CreateDatePicker(propertyDefinition, propertyValue, legacyTimeZoneOffset, timeZone, siteRoot);

						datePicker.TabIndex = 0;
						datePicker.ID = "dp" + propertyDefinition.Name;
						datePicker.CssClass = "forminput " + propertyDefinition.CssClass;

						parentControl.Controls.Add(label);
						parentControl.Controls.Add(datePicker);

						if (propertyDefinition.IncludeHelpLink)
						{
							AddHelpLink(parentControl, propertyDefinition);
						}

						if (propertyDefinition.RequiredForRegistration)
						{
							var rfvDate = new RequiredFieldValidator
							{
								SkinID = validatorSkinID,
								ControlToValidate = datePicker.ID,
								ErrorMessage = string.Format(
									CultureInfo.InvariantCulture,
									Resources.ProfileResource.ProfileRequiredItemFormat,
									ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey)
								),
								ValidationGroup = "profile"
							};

							parentControl.Controls.Add(rfvDate);
						}

						if (propertyDefinition.RegexValidationExpression.Length > 0)
						{
							var regexValidatorDate = new RegularExpressionValidator
							{
								SkinID = validatorSkinID,
								ControlToValidate = datePicker.ID,
								ValidationExpression = propertyDefinition.RegexValidationExpression,
								ValidationGroup = "profile"
							};

							if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
							{
								regexValidatorDate.ErrorMessage = ResourceHelper.GetResourceString(
									propertyDefinition.ResourceFile,
									propertyDefinition.RegexValidationErrorResourceKey
								);
							}

							parentControl.Controls.Add(regexValidatorDate);
						}

						break;

					case "System.String":
					default:

						var textBox = new TextBox
						{
							TabIndex = 0,
							ID = "txt" + propertyDefinition.Name,
							CssClass = "forminput " + propertyDefinition.CssClass
						};

						label.ForControl = textBox.ID;

						parentControl.Controls.Add(label);

						if (propertyDefinition.MaxLength > 0)
						{
							textBox.MaxLength = propertyDefinition.MaxLength;
						}

						if (propertyDefinition.Columns > 0)
						{
							textBox.Columns = propertyDefinition.Columns;
						}

						if (propertyDefinition.Rows > 1)
						{
							textBox.TextMode = TextBoxMode.MultiLine;
							textBox.Rows = propertyDefinition.Rows;
						}

						parentControl.Controls.Add(textBox);

						if (propertyDefinition.IncludeHelpLink)
						{
							AddHelpLink(parentControl, propertyDefinition);
						}

						if (propertyValue.Length > 0)
						{
							textBox.Text = propertyValue;
						}

						if (propertyDefinition.RequiredForRegistration)
						{
							var rfv = new RequiredFieldValidator
							{
								SkinID = validatorSkinID,
								ControlToValidate = textBox.ID,
								ErrorMessage = string.Format(
								CultureInfo.InvariantCulture,
								Resources.ProfileResource.ProfileRequiredItemFormat,
								ResourceHelper.GetResourceString(propertyDefinition.ResourceFile, propertyDefinition.LabelResourceKey)
							),
								ValidationGroup = "profile"
							};

							parentControl.Controls.Add(rfv);
						}

						if (propertyDefinition.RegexValidationExpression.Length > 0)
						{
							var regexValidator = new RegularExpressionValidator
							{
								SkinID = validatorSkinID,
								ControlToValidate = textBox.ID,
								ValidationExpression = propertyDefinition.RegexValidationExpression,
								ValidationGroup = "profile"
							};

							if (propertyDefinition.RegexValidationErrorResourceKey.Length > 0)
							{
								regexValidator.ErrorMessage = ResourceHelper.GetResourceString(
									propertyDefinition.ResourceFile,
									propertyDefinition.RegexValidationErrorResourceKey
								);
							}

							parentControl.Controls.Add(regexValidator);
						}

						break;
				}
			}


			var rowCloseTag = new Literal
			{
				Text = "</div>"
			};

			parentControl.Controls.Add(rowCloseTag);
		}


		private static DropDownList CreateDropDownQuestion(mojoProfilePropertyDefinition propertyDefinition, string propertyValue)
		{
			var dd = new DropDownList();

			if (dd.Items.Count == 0)
			{
				foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
				{
					var listItem = new ListItem
					{
						Value = option.Value,
						Text = option.TextResourceKey
					};

					if (option.TextResourceKey.Length > 0)
					{
						if (HttpContext.Current != null)
						{
							var obj = HttpContext.GetGlobalResourceObject(propertyDefinition.ResourceFile, option.TextResourceKey);

							if (obj != null)
							{
								listItem.Text = obj.ToString();
							}
						}
					}

					dd.Items.Add(listItem);
				}
			}

			ListItem defaultItem = dd.Items.FindByValue(propertyValue);

			if (defaultItem != null)
			{
				dd.ClearSelection();
				defaultItem.Selected = true;
			}

			return dd;
		}


		private static CheckBoxList CreateCheckBoxListQuestion(mojoProfilePropertyDefinition propertyDefinition, string propertyValue)
		{
			var cbl = new CheckBoxList();

			if (cbl.Items.Count == 0)
			{
				foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
				{
					var listItem = new ListItem
					{
						Value = option.Value,
						Text = option.TextResourceKey
					};

					if (option.TextResourceKey.Length > 0)
					{
						if (HttpContext.Current != null)
						{
							var obj = HttpContext.GetGlobalResourceObject(propertyDefinition.ResourceFile, option.TextResourceKey);

							if (obj != null)
							{
								listItem.Text = obj.ToString();
							}
						}
					}

					cbl.Items.Add(listItem);
				}
			}

			ListItem item;

			if (propertyValue.Contains(","))
			{

				List<string> items = propertyValue.SplitOnChar(',');

				foreach (string s in items)
				{
					item = cbl.Items.FindByValue(s);

					if (item != null)
					{
						item.Selected = true;
					}
				}
			}
			else
			{
				ListItem defaultItem = cbl.Items.FindByValue(propertyValue);

				if (defaultItem != null)
				{
					defaultItem.Selected = true;
				}
			}

			return cbl;
		}


		private static DatePickerControl CreateDatePicker(
			mojoProfilePropertyDefinition propertyDefinition,
			string propertyValue,
			double legacyTimeZoneOffset,
			TimeZoneInfo timeZone,
			string siteRoot
		)
		{
			var datePicker = new DatePickerControl();

			try
			{
				datePicker.SkinID = propertyDefinition.Name.Replace(" ", string.Empty);
			}
			catch (ArgumentException)
			{ }

			datePicker.ID = "dp" + propertyDefinition.Name;
			datePicker.ShowMonthList = propertyDefinition.DatePickerShowMonthList;
			datePicker.ShowYearList = propertyDefinition.DatePickerShowYearList;

			if (propertyDefinition.DatePickerYearRange.Length > 0)
			{
				datePicker.YearRange = propertyDefinition.DatePickerYearRange;
			}

			if (propertyValue.Length > 0)
			{
				if (DateTime.TryParse(
						propertyValue,
						CultureInfo.CurrentCulture,
						DateTimeStyles.AdjustToUniversal, out DateTime dt
					)
				)
				{
					if (propertyDefinition.IncludeTimeForDate)
					{
						if (timeZone != null)
						{
							dt = dt.ToLocalTime(timeZone);
						}
						else
						{
							dt = dt.AddHours(legacyTimeZoneOffset);
						}

						datePicker.Text = dt.ToString("g");
					}
					else
					{
						datePicker.Text = dt.Date.ToShortDateString();
					}
				}
				else
				{
					datePicker.Text = propertyValue;
				}
			}
			else
			{
				if (propertyDefinition.DefaultValue.Length > 0)
				{
					datePicker.Text = propertyDefinition.DefaultValue;
				}
			}

			datePicker.ShowTime = propertyDefinition.IncludeTimeForDate;

			return datePicker;
		}


		public static void SetupReadOnlyPropertyControl(
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition,
			string propertyValue,
			double legacyTimeZoneOffset,
			TimeZoneInfo timeZone
		)
		{
			if (propertyValue == null)
			{
				propertyValue = string.Empty;
			}

			var rowOpenTag = new Literal
			{
				Text = $"<div class='settingrow {propertyDefinition.CssClass}'>"
			};

			parentControl.Controls.Add(rowOpenTag);

			var label = new SiteLabel
			{
				ResourceFile = propertyDefinition.ResourceFile,
				// if key isn't in resource file use assume the resource hasn't been
				//localized and just use the key as the resource
				ShowWarningOnMissingKey = false,
				ConfigKey = propertyDefinition.LabelResourceKey,
				CssClass = "settinglabel",
				UseLabelTag = false
			};

			parentControl.Controls.Add(label);

			var propertyLabel = new Label();

			parentControl.Controls.Add(propertyLabel);

			var didLoadControl = false;

			if (propertyDefinition.ISettingControlSrc.Length > 0)
			{
				Control c = parentControl.Page.LoadControl(propertyDefinition.ISettingControlSrc);

				if (c != null && c is IReadOnlySettingControl control)
				{
					c.ID = "isc" + propertyDefinition.Name;

					parentControl.Controls.Add(label);

					IReadOnlySettingControl settingControl = control;

					settingControl.SetReadOnlyValue(propertyValue);
	
					parentControl.Controls.Add(c);
					
					didLoadControl = true;
				}
			}

			if (!didLoadControl)
			{
				if (propertyDefinition.OptionList.Count > 0 && propertyDefinition.Type != "CheckboxList")
				{
					var dd = new DropDownList
					{
						ID = "dd" + propertyDefinition.Name
					};

					foreach (mojoProfilePropertyOption option in propertyDefinition.OptionList)
					{
						var listItem = new ListItem
						{
							Value = option.Value,
							Text = option.TextResourceKey
						};

						if (option.TextResourceKey.Length > 0)
						{
							if (HttpContext.Current != null)
							{
								var obj = HttpContext.GetGlobalResourceObject(propertyDefinition.ResourceFile, option.TextResourceKey);

								if (obj != null)
								{
									listItem.Text = obj.ToString();
								}
							}
						}

						dd.Items.Add(listItem);
					}

					ListItem defaultItem = dd.Items.FindByValue(propertyValue);

					if (defaultItem != null)
					{
						propertyLabel.Text = HttpUtility.HtmlEncode(defaultItem.Text);
					}
				}
				else
				{
					switch (propertyDefinition.Type)
					{

						case "System.Boolean":
							var litBool = new Literal();

							var imgVal = propertyValue.ToLower();

							if (imgVal.Length == 0)
							{
								imgVal = propertyDefinition.DefaultValue.ToLower();
							}
							
							if (imgVal.Length == 0)
							{
								imgVal = "false";
							}

							litBool.Text = $"<img src='/Data/SiteImages/{imgVal}.png' alt='{propertyDefinition.Name}' />";

							parentControl.Controls.Add(litBool);

							break;

						case "System.DateTime":
							var litDateTime = new Literal();
							DateTime dt;

							if (DateTime.TryParse(
									propertyValue,
									CultureInfo.CurrentCulture,
									DateTimeStyles.AdjustToUniversal, out dt
								)
							)
							{
								if (propertyDefinition.IncludeTimeForDate)
								{
									dt = dt.AddHours(legacyTimeZoneOffset);
									litDateTime.Text = dt.ToString();
								}
								else
								{
									litDateTime.Text = dt.Date.ToShortDateString();
								}
							}
							else
							{
								litDateTime.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
							}

							parentControl.Controls.Add(litDateTime);

							break;

						case "System.String":
						default:
							if (propertyValue.Length > 0)
							{
								if (propertyDefinition.AllowMarkup)
								{
									propertyLabel.Text = SecurityHelper.PreventCrossSiteScripting(propertyValue);
								}
								else
								{
									if (propertyDefinition.Name.ToLower().IndexOf("url") > -1)
									{
										var litLink = new Literal
										{
											Text = $"<a href='{HttpUtility.HtmlEncode(propertyValue)}'>{HttpUtility.HtmlEncode(propertyValue)}</a>"
										};

										parentControl.Controls.Add(litLink);
									}
									else
									{
										propertyLabel.Text = HttpUtility.HtmlEncode(propertyValue);
									}
								}
							}
							else
							{
								propertyLabel.Text = "&nbsp;";
							}

							break;
					}
				}
			}

			if (propertyLabel.Text.Length > 0)
			{
				parentControl.Controls.Add(propertyLabel);
			}

			var rowCloseTag = new Literal
			{
				Text = "</div>"
			};

			parentControl.Controls.Add(rowCloseTag);
		}


		private static void AddHelpLink(
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition
		)
		{
			var litSpace = new Literal
			{
				Text = "&nbsp;"
			};

			parentControl.Controls.Add(litSpace);

			var helpLinkButton = new mojoHelpLink
			{
				HelpKey = "profile-" + propertyDefinition.Name.ToLower() + "-help"
			};

			parentControl.Controls.Add(helpLinkButton);

			litSpace = new Literal
			{
				Text = "&nbsp;"
			};

			parentControl.Controls.Add(litSpace);
		}

		public static void SaveProperty(
			SiteUser siteUser,
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition,
			double legacyTimeZoneOffset,
			TimeZoneInfo timeZone
		)
		{
			string controlID;
			Control control;

			if (propertyDefinition.ISettingControlSrc.Length > 0)
			{
				controlID = "isc" + propertyDefinition.Name;
				control = parentControl.FindControl(controlID);

				if (control != null)
				{
					siteUser.SetProperty(
						propertyDefinition.Name,
						((ISettingControl)control).GetValue(),
						propertyDefinition.SerializeAs,
						propertyDefinition.LazyLoad
					);
				}
			}
			else
			{
				switch (propertyDefinition.Type)
				{
					case "System.Boolean":
						controlID = "chk" + propertyDefinition.Name;
						control = parentControl.FindControl(controlID);

						if (control != null)
						{
							siteUser.SetProperty(
								propertyDefinition.Name,
								((CheckBox)control).Checked,
								propertyDefinition.SerializeAs,
								propertyDefinition.LazyLoad
							);
						}

						break;

					case "System.DateTime":
						controlID = "dp" + propertyDefinition.Name;
						control = parentControl.FindControl(controlID);

						if (control != null)
						{
							var dp = (DatePickerControl)control;

							if (dp.Text.Length > 0)
							{
								if (DateTime.TryParse(
										dp.Text,
										CultureInfo.CurrentCulture,
										DateTimeStyles.AdjustToUniversal, out DateTime dt
									)
								)
								{
									if (propertyDefinition.IncludeTimeForDate)
									{
										if (timeZone != null)
										{
											dt = dt.ToUtc(timeZone);
										}
										else
										{
											dt = dt.AddHours(-legacyTimeZoneOffset);
										}

										if (propertyDefinition.Name == "DateOfBirth")
										{
											siteUser.DateOfBirth = dt.Date;

											siteUser.Save();
										}
										else
										{
											siteUser.SetProperty(
												propertyDefinition.Name,
												dt.ToString(),
												propertyDefinition.SerializeAs,
												propertyDefinition.LazyLoad
											);
										}
									}
									else
									{
										if (propertyDefinition.Name == "DateOfBirth")
										{
											siteUser.DateOfBirth = dt.Date;

											siteUser.Save();
										}
										else
										{
											siteUser.SetProperty(
											propertyDefinition.Name,
											dt.Date.ToShortDateString(),
											propertyDefinition.SerializeAs,
											propertyDefinition.LazyLoad);
										}
									}
								}
								else
								{
									siteUser.SetProperty(
										propertyDefinition.Name,
										dp.Text,
										propertyDefinition.SerializeAs,
										propertyDefinition.LazyLoad
									);
								}
							}
							else // blank
							{
								if (propertyDefinition.Name == "DateOfBirth")
								{
									siteUser.DateOfBirth = DateTime.MinValue;

									siteUser.Save();
								}
								else
								{
									siteUser.SetProperty(
										propertyDefinition.Name,
										string.Empty,
										propertyDefinition.SerializeAs,
										propertyDefinition.LazyLoad
									);
								}
							}
						}

						break;

					case "System.String":
					default:
						if (propertyDefinition.OptionList.Count > 0)
						{
							if (propertyDefinition.Type == "CheckboxList")
							{
								controlID = "cbl" + propertyDefinition.Name;
								control = parentControl.FindControl(controlID);

								if (control != null)
								{
									if (control is CheckBoxList cbl)
									{
										siteUser.SetProperty(
											propertyDefinition.Name,
											cbl.Items.SelectedItemsToCommaSeparatedString(),
											propertyDefinition.SerializeAs,
											propertyDefinition.LazyLoad
										);
									}
								}
							}
							else
							{
								controlID = "dd" + propertyDefinition.Name;
								control = parentControl.FindControl(controlID);

								if (control != null)
								{
									if (control is DropDownList dd)
									{
										if (dd.SelectedIndex > -1)
										{
											siteUser.SetProperty(
												propertyDefinition.Name,
												dd.SelectedValue,
												propertyDefinition.SerializeAs,
												propertyDefinition.LazyLoad
											);
										}
									}
								}
							}
						}
						else
						{
							controlID = "txt" + propertyDefinition.Name;
							control = parentControl.FindControl(controlID);

							if (control != null)
							{
								siteUser.SetProperty(
									propertyDefinition.Name,
									((TextBox)control).Text,
									propertyDefinition.SerializeAs,
									propertyDefinition.LazyLoad
								);
							}
						}

						break;
				}
			}
		}


		public static void SavePropertyDefault(
			SiteUser siteUser,
			mojoProfilePropertyDefinition propertyDefinition
		)
		{
			siteUser.SetProperty(
				propertyDefinition.Name,
				propertyDefinition.DefaultValue,
				propertyDefinition.SerializeAs,
				propertyDefinition.LazyLoad
			);
		}


		public static void LoadState(
			Panel parentControl,
			mojoProfilePropertyDefinition propertyDefinition
		)
		{
			string controlID;
			Control control;

			switch (propertyDefinition.Type)
			{
				case "System.Boolean":
					controlID = "chk" + propertyDefinition.Name;
					control = parentControl.FindControl(controlID);

					if (control != null)
					{
						propertyDefinition.StateValue = ((CheckBox)control).Checked.ToString();
					}

					break;

				case "System.DateTime":
					controlID = "dp" + propertyDefinition.Name;
					control = parentControl.FindControl(controlID);

					if (control != null)
					{
						var dp = (DatePickerControl)control;

						if (dp.Text.Length > 0)
						{
							propertyDefinition.StateValue = dp.Text;
						}
					}

					break;

				case "System.String":
				default:
					if (propertyDefinition.OptionList.Count > 0)
					{
						if (propertyDefinition.Type == "CheckboxList")
						{
							controlID = "cbl" + propertyDefinition.Name;
							control = parentControl.FindControl(controlID);

							if (control != null)
							{
								if (control is CheckBoxList cbl)
								{
									propertyDefinition.StateValue = cbl.Items.SelectedItemsToCommaSeparatedString();
								}
							}
						}
						else
						{
							controlID = "dd" + propertyDefinition.Name;
							control = parentControl.FindControl(controlID);

							if (control != null)
							{
								if (control is DropDownList dd)
								{
									if (dd.SelectedIndex > -1)
									{
										propertyDefinition.StateValue = dd.SelectedValue;
									}
								}
							}
						}
					}
					else
					{
						controlID = "txt" + propertyDefinition.Name;
						control = parentControl.FindControl(controlID);

						if (control != null)
						{
							propertyDefinition.StateValue = ((TextBox)control).Text;
						}
					}

					break;
			}
		}

		#endregion
	}
}
