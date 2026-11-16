using System;
using System.Collections;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Web.UI;

public partial class CustomScriptModule : SiteModuleControl
{
	//Feature Guid: 662d49fd-cb44-42a5-a6a7-b905bbe65889

	private CustomScriptConfiguration config = new CustomScriptConfiguration();
	private string scriptRefFormat = "\n<script data-loader=\"CustomScript.ascx\" src=\"{0}\"></script>";
	private string rawScriptFormat = "\n<script data-loader=\"CustomScript.ascx\">\n{0}\n</script>";

	protected void Page_Load(object sender, EventArgs e)
	{
		LoadSettings();
		SetupScriptUrl();
		SetupRawScript();

		TitleTop.Visible = IsEditable && CustomScriptConfiguration.ForceTitleWhenEditable && !pnlOuterWrap.Visible;
	}

	private void SetupScriptUrl()
	{

		if (string.IsNullOrWhiteSpace(config.ScriptUrl)) { litScriptUrl.Visible = false; return; }

		//script position
		// inHead
		// inBody (register script)
		// inContentPosition
		// bottomStartup (register startup script)

		switch (config.ScriptUrlPosition)
		{
			case "inHead":
				if (!IsPostBack && !Page.IsCallback)
				{
					Page.Header.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl)));
				}
				break;

			case "inContentPosition":
				if (config.RenderStandardWrapperDivs)
				{
					pnlOuterWrap.Visible = true;
					//pnlInnerBody.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, scriptUrl)));
					litScriptUrl.Text = string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl);
				}
				else
				{
					if (!IsPostBack && !Page.IsCallback)
					{
						Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl)));
					}
				}
				break;

			case "bottomStartup":
				ScriptManager.RegisterStartupScript(
					this,
					typeof(Page),
					ClientID + "scripturl",
					string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl),
					false);
				break;

			case "inBody":
			default:
				ScriptManager.RegisterClientScriptBlock(
					this,
					typeof(Page),
					ClientID + "scripturl",
					string.Format(CultureInfo.InvariantCulture, scriptRefFormat, config.ScriptUrl),
					false);
				break;
		}
	}

	private void SetupRawScript()
	{

		if (string.IsNullOrWhiteSpace(config.RawScript)) { litScript.Visible = false; return; }

		//script position
		// inHead
		// inBody (register script)
		// inContentPosition
		// bottomStartup (register startup script)

		switch (config.RawScriptPosition)
		{
			case "inHead":
				if (!IsPostBack && !Page.IsCallback)
				{
					if (config.AddScriptElementToRawScript)
					{
						Page.Header.Controls.Add(new LiteralControl(string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript)));
					}
					else
					{
						Page.Header.Controls.Add(new LiteralControl(config.RawScript));
					}
				}
				break;

			case "inContentPosition":
				if (config.RenderStandardWrapperDivs)
				{
					pnlOuterWrap.Visible = true;

					if (config.AddScriptElementToRawScript)
					{
						litScript.Text = string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript);
					}
					else
					{
						litScript.Text = config.RawScript;
					}
				}
				else
				{
					if (!IsPostBack && !Page.IsCallback)
					{
						var litScript = new LiteralControl();
						if (config.AddScriptElementToRawScript)
						{
							litScript.Text = string.Format(CultureInfo.InvariantCulture, rawScriptFormat, config.RawScript);
						}
						else
						{
							litScript.Text = config.RawScript;
						}

						Controls.Add(litScript);
					}
				}
				break;

			case "bottomStartup":
				ScriptManager.RegisterStartupScript(
					this,
					typeof(Page),
					ClientID + "script",
					config.RawScript,
					config.AddScriptElementToRawScript);
				break;


			case "inBody":
			default:
				ScriptManager.RegisterClientScriptBlock(
					this,
					typeof(Page),
					ClientID + "script",
					config.RawScript,
					config.AddScriptElementToRawScript);
				break;
		}
	}

	private void LoadSettings()
	{
		config = new CustomScriptConfiguration(Settings);

		if (config.CustomCssClass.Length > 0)
		{
			pnlOuterWrap.SetOrAppendCss(config.CustomCssClass);
		}

		pnlOuterWrap.Visible = config.RenderStandardWrapperDivs;
	}
}

public class CustomScriptConfiguration
{
	public CustomScriptConfiguration()
	{ }

	public CustomScriptConfiguration(Hashtable settings)
	{
		LoadSettings(settings);
	}

	private void LoadSettings(Hashtable settings)
	{
		if (settings == null) { throw new ArgumentException("must pass in a hashtable of settings"); }

		ScriptUrl = WebUtils.ParseStringFromHashtable(settings, "ScriptUrl", ScriptUrl);
		ScriptUrlPosition = WebUtils.ParseStringFromHashtable(settings, "ScriptUrlPosition", ScriptUrlPosition);	
		RawScriptPosition = WebUtils.ParseStringFromHashtable(settings, "RawScriptPosition", RawScriptPosition);
		RawScript = WebUtils.ParseStringFromHashtable(settings, "RawScript", RawScript);
		CustomCssClass = WebUtils.ParseStringFromHashtable(settings, "CustomCssClassSetting", CustomCssClass);
		AddScriptElementToRawScript = WebUtils.ParseBoolFromHashtable(settings, "AddScriptElementToRawScript", AddScriptElementToRawScript);
		RenderStandardWrapperDivs = WebUtils.ParseBoolFromHashtable(settings, "RenderStandardWrapperDivs", RenderStandardWrapperDivs);
	}

	public string ScriptUrl { get; private set; } = string.Empty;

	/// <summary>
	/// script position
	/// inHead
	/// inBody (register script) default
	/// inContentPosition
	/// bottomStartup (register startup script)
	/// </summary>
	public string ScriptUrlPosition { get; private set; } = "inBody";

	public string RawScript { get; private set; } = string.Empty;

	/// <summary>
	/// script position
	/// inHead
	/// inBody (register script) default
	/// inContentPosition
	/// bottomStartup (register startup script)
	/// </summary>
	public string RawScriptPosition { get; private set; } = "bottomStartup";

	/// <summary>
	/// default is true the outer <script></script> element will be added around the raw script content
	/// set to false if you will put the script element within the content.
	/// false is useful if you also want to add an html snippet that is needed by your script
	/// </summary>
	public bool AddScriptElementToRawScript { get; private set; } = true;

	public bool RenderStandardWrapperDivs { get; private set; } = false;

	public string CustomCssClass { get; private set; } = string.Empty;

	public static bool ForceTitleWhenEditable => Core.Configuration.ConfigHelper.GetBoolProperty("CustomScriptModule:ForceTitleWhenEditable", true);
}

public class ScriptPositionSetting : DropDownList, ISettingControl
{
	protected override void EnsureChildControls()
	{
		base.EnsureChildControls();
		EnsureItems();
	}

	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		EnsureItems();
	}
	private void EnsureItems()
	{
		if (Items.Count == 0)
		{
			// inHead
			// inBody (register script)
			// inContentPosition
			// bottomStartup (register startup script)

			Items.Add(new ListItem
			{
				Text = Resource.ScriptPositionInHead,
				Value = "inHead"
			});

			Items.Add(new ListItem
			{
				Text = Resource.ScriptPositionInBody,
				Value = "inBody"
			});

			Items.Add(new ListItem
			{
				Text = Resource.ScriptPositionInContent,
				Value = "inContentPosition"
			});

			Items.Add(new ListItem
			{
				Text = Resource.ScriptPositionBottomStartup,
				Value = "bottomStartup"
			});
		}
	}

	#region ISettingControl

	public string GetValue()
	{
		EnsureItems();
		//if (ViewState["SelectedVal"] != null)
		//{
		//    return ViewState["SelectedVal"].ToString();
		//}
		return SelectedValue;
	}

	public void SetValue(string val)
	{
		EnsureItems();
		ListItem item = Items.FindByValue(val);
		if (item != null)
		{
			ClearSelection();
			item.Selected = true;
			//ViewState["SelectedVal"] = val;
		}
	}

	#endregion

}