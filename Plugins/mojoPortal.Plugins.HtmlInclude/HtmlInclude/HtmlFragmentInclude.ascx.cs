using System;
using System.IO;
using System.Web;
using System.Web.UI;
using mojoPortal.Web;
using mojoPortal.Web.Framework;
using Resources;

namespace mojoPortal.Plugins.HtmlInclude;

public partial class HtmlIncludeModule : SiteModuleControl
{
	protected override void OnInit(EventArgs e)
	{
		base.OnInit(e);
		Load += new EventHandler(Page_Load);
	}


	protected void Page_Load(object sender, EventArgs e)
	{
		Title1.EditUrl = "HtmlInclude/Edit.aspx".ToLinkBuilder().ToString();
		Title1.EditText = HtmlIncludeResources.HtmlFragmentIncludeEditLink;

		if (ModuleConfiguration != null)
		{
			Title = ModuleConfiguration.ModuleTitle;
			Description = ModuleConfiguration.FeatureName;
		}

		string includePath;

		if (WebConfigSettings.HtmlFragmentUseMediaFolder)
		{
			includePath = Invariant($"/Data/Sites/{siteSettings.SiteId}/media/htmlfragments");
		}
		else
		{
			includePath = Invariant($"/Data/Sites/{siteSettings.SiteId}/htmlfragments");
		}

		string includeFileName = Settings["HtmlFragmentSourceSetting"].ToString();
		string includeContentFile = HttpContext.Current.Server.MapPath($"{WebUtils.GetApplicationRoot()}{includePath}{Path.DirectorySeparatorChar}{includeFileName}");

		if (includeFileName != null)
		{
			if (File.Exists(includeContentFile))
			{
				var file = new FileInfo(includeContentFile);
				using StreamReader sr = file.OpenText();
				litInclude.Text = sr.ReadToEnd();
			}
			else
			{
				Controls.Add(new LiteralControl($"<div class=\"txterror danger\">File {includeContentFile} not found."));
			}
		}

		var instanceCssClass = Settings.ParseString("CustomCssClassSetting");

		pnlOuterWrap.SetOrAppendCss(instanceCssClass);
	}
}
